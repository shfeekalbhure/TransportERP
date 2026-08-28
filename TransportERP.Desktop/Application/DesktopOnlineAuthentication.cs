using System.Collections.Frozen;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransportERP.Application.Sync;
using TransportERP.Contracts.Identity;
using TransportERP.Desktop.Offline;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Desktop.Application;

internal sealed record DesktopOnlineSignInRequest(
    string UserNameOrEmail,
    string Password,
    Guid CompanyId,
    Guid BranchId,
    string DeviceId,
    string DeviceCredential,
    string DeviceSigningCertificateThumbprint);

internal sealed record DesktopOnlineAuthenticationResult(
    bool Succeeded,
    string Code,
    DesktopAuthenticatedOfflineActivation? Activation)
{
    internal static DesktopOnlineAuthenticationResult Denied(string code) => new(false, code, null);
    internal static DesktopOnlineAuthenticationResult Authorized(DesktopAuthenticatedOfflineActivation activation) =>
        new(true, "AUTHORIZED", activation);
}

internal interface IDesktopOnlineSessionAuthenticator : IDisposable
{
    Task<DesktopOnlineAuthenticationResult> AuthenticateAsync(
        DesktopOnlineSignInRequest request,
        CancellationToken cancellationToken);

    Task EndSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}

/// <summary>
/// Uses the production HTTPS identity and device APIs. Authentication artifacts are retained only
/// in volatile memory and are never accepted from process arguments, environment variables or disk.
/// Offline authorization is proven by the exact scoped sync-activation contract before a local
/// database, DPAPI key or Windows signing handle can be created.
/// </summary>
internal sealed class DesktopOnlineSessionAuthenticator : IDesktopOnlineSessionAuthenticator
{
    private static readonly JsonSerializerOptions ApiJson = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 32
    };

    private readonly Func<HttpMessageHandler> _handlerFactory;
    private HttpClient? _authenticationHttpClient;
    private HttpClient? _syncHttpClient;
    private VolatileBearerTokenProvider? _bearer;
    private Uri? _origin;
    private Guid _sessionId;
    private bool _disposed;

    internal DesktopOnlineSessionAuthenticator(Func<HttpMessageHandler>? handlerFactory = null) =>
        _handlerFactory = handlerFactory ?? (() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                ConnectTimeout = TimeSpan.FromSeconds(15),
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false,
                AllowAutoRedirect = false
            });

    public async Task<DesktopOnlineAuthenticationResult> AuthenticateAsync(
        DesktopOnlineSignInRequest request,
        CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_bearer is not null)
            return DesktopOnlineAuthenticationResult.Denied("DESKTOP_SESSION_REPLAY_DENIED");
        var origin = SyncClientDeploymentAuthority.Origin;
        if (string.IsNullOrWhiteSpace(request.UserNameOrEmail) ||
            string.IsNullOrEmpty(request.Password) ||
            request.CompanyId == Guid.Empty || request.BranchId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.DeviceId) ||
            string.IsNullOrWhiteSpace(request.DeviceCredential) ||
            string.IsNullOrWhiteSpace(request.DeviceSigningCertificateThumbprint))
            return DesktopOnlineAuthenticationResult.Denied("SIGN_IN_INPUT_INVALID");

        try
        {
            var measuredBuildIdentity = DesktopBuildIdentityProbe.Measure();
            var session = await CreateSessionAsync(origin, request, cancellationToken);
            if (session is null)
                return DesktopOnlineAuthenticationResult.Denied("AUTHENTICATED_SCOPE_INVALID");
            if (session.SessionId == Guid.Empty || session.UserId == Guid.Empty ||
                session.CompanyId != request.CompanyId || session.BranchId != request.BranchId ||
                !string.Equals(session.DeviceId, request.DeviceId.Trim(), StringComparison.Ordinal) ||
                session.AccessTokenExpiresAt <= DateTimeOffset.UtcNow.AddSeconds(30))
                return await RejectProvisionalSessionAsync(
                    origin, session, "AUTHENTICATED_SCOPE_INVALID", cancellationToken);

            var bearer = new VolatileBearerTokenProvider(session.AccessToken);
            try
            {
                var authorization = await GetSyncActivationAsync(
                    origin, bearer, measuredBuildIdentity, cancellationToken);
                if (!TryValidateActivation(origin, session, authorization, out var batchEndpoint,
                        out var allowedActions, out var proofBinding, out var effectivePolicy) ||
                    authorization!.AuthorizedBuildIdentity is not { IsValid: true } authorizedBuild ||
                    !authorizedBuild.FixedTimeEquals(measuredBuildIdentity))
                    return await RejectProvisionalSessionAsync(
                        origin, session, "OFFLINE_AUTHORIZATION_DENIED", cancellationToken, bearer);

                var scope = new DesktopAuthenticatedSessionScope(
                    session.CompanyId, session.BranchId.Value, session.UserId, authorization!.RegisteredDeviceId);
                // The sync transport is a distinct HTTPS client created only after the server has
                // authorized the authenticated device and Offline policy.
                _syncHttpClient = CreateHttpClient();
                var network = new AuthenticatedDesktopSyncNetworkProvider(_syncHttpClient);
                var options = CreateCompositionOptions(
                    session, authorization, batchEndpoint!, request.DeviceSigningCertificateThumbprint,
                    proofBinding!, effectivePolicy!, measuredBuildIdentity);
                var dependencies = new DesktopOfflineDependencies(
                    bearer,
                    network,
                    new ServerValidatedOfflineWritePolicy(allowedActions!),
                    new ServerValidatedOperationsPermissionPolicy(
                        authorization.CanRetryFailedOperations,
                        authorization.CanResolveConflicts));
                var activation = new DesktopAuthenticatedOfflineActivation(
                    session.SessionId,
                    session.AccessTokenExpiresAt,
                    scope,
                    scope,
                    OfflineRuntimeAuthorized: true,
                    EffectivePolicy: effectivePolicy!,
                    MeasuredBuildIdentity: measuredBuildIdentity,
                    AllowedActions: allowedActions!.ToFrozenSet(),
                    CreateRuntimeAsync: cancellation => DesktopOfflineComposition.CreateAsync(
                        options, dependencies, cancellationToken: cancellation));

                _origin = origin;
                _sessionId = session.SessionId;
                _bearer = bearer;
                bearer = null;
                return DesktopOnlineAuthenticationResult.Authorized(activation);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                _syncHttpClient?.Dispose();
                _syncHttpClient = null;
                return await RejectProvisionalSessionAsync(
                    origin, session, "OFFLINE_AUTHORIZATION_DENIED", cancellationToken, bearer);
            }
            finally
            {
                bearer?.Dispose();
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return DesktopOnlineAuthenticationResult.Denied("AUTH_SERVICE_UNAVAILABLE");
        }
        catch (TaskCanceledException)
        {
            return DesktopOnlineAuthenticationResult.Denied("AUTH_SERVICE_TIMEOUT");
        }
        catch (JsonException)
        {
            return DesktopOnlineAuthenticationResult.Denied("AUTH_RESPONSE_INVALID");
        }
        catch
        {
            return DesktopOnlineAuthenticationResult.Denied("DESKTOP_AUTHENTICATION_FAILED");
        }
    }

    public async Task EndSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        if (_disposed || sessionId == Guid.Empty || sessionId != _sessionId || _origin is null || _bearer is null)
            return;
        var token = await _bearer.GetBearerTokenAsync(cancellationToken);
        var origin = _origin;
        // Invalidate the shared runtime capability before attempting best-effort remote revocation.
        _bearer.Dispose();
        _bearer = null;
        _syncHttpClient?.Dispose();
        _syncHttpClient = null;
        try
        {
            using var message = new HttpRequestMessage(
                HttpMethod.Post, new Uri(origin, $"/api/v1/auth/sessions/{sessionId:D}:revoke"));
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            message.Headers.TryAddWithoutValidation("X-Correlation-Id", Guid.NewGuid().ToString("D"));
            message.Content = JsonContent.Create(new RevokeIdentitySessionRequest("DESKTOP_SESSION_ENDED"));
            using var _ = await AuthenticationHttpClient.SendAsync(
                message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Local teardown remains mandatory even when remote revocation cannot complete.
        }
        catch (HttpRequestException)
        {
            // Local bearer invalidation below is fail-closed; the server session expires normally.
        }
        finally
        {
            _origin = null;
            _sessionId = Guid.Empty;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        _bearer?.Dispose();
        _bearer = null;
        _origin = null;
        _sessionId = Guid.Empty;
        _syncHttpClient?.Dispose();
        _authenticationHttpClient?.Dispose();
        _syncHttpClient = null;
        _authenticationHttpClient = null;
    }

    private async Task<IdentitySessionResponse?> CreateSessionAsync(
        Uri origin,
        DesktopOnlineSignInRequest request,
        CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Post, new Uri(origin, "/api/v1/auth/sessions"));
        message.Headers.TryAddWithoutValidation("X-Correlation-Id", Guid.NewGuid().ToString("D"));
        message.Content = JsonContent.Create(new CreateIdentitySessionRequest(
            request.UserNameOrEmail.Trim(), request.Password, request.CompanyId,
            request.BranchId, request.DeviceId.Trim(), request.DeviceCredential));
        using var response = await AuthenticationHttpClient.SendAsync(
            message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        return await DeserializeBoundedAsync<IdentitySessionResponse>(response, cancellationToken);
    }

    private async Task<DesktopOnlineAuthenticationResult> RejectProvisionalSessionAsync(
        Uri origin,
        IdentitySessionResponse session,
        string code,
        CancellationToken cancellationToken,
        VolatileBearerTokenProvider? existingBearer = null)
    {
        VolatileBearerTokenProvider? ownedBearer = null;
        try
        {
            var bearer = existingBearer ?? (ownedBearer = new VolatileBearerTokenProvider(session.AccessToken));
            using var message = new HttpRequestMessage(
                HttpMethod.Post, new Uri(origin, $"/api/v1/auth/sessions/{session.SessionId:D}:revoke"));
            message.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer", await bearer.GetBearerTokenAsync(cancellationToken));
            message.Headers.TryAddWithoutValidation("X-Correlation-Id", Guid.NewGuid().ToString("D"));
            message.Content = JsonContent.Create(new RevokeIdentitySessionRequest("DESKTOP_ACTIVATION_DENIED"));
            using var _ = await AuthenticationHttpClient.SendAsync(
                message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            // The provisional bearer is discarded below even if remote revocation is unavailable.
        }
        finally
        {
            ownedBearer?.Dispose();
        }
        return DesktopOnlineAuthenticationResult.Denied(code);
    }

    private async Task<DesktopSyncActivationResponse?> GetSyncActivationAsync(
        Uri origin,
        VolatileBearerTokenProvider bearer,
        BuildIdentityV1 measuredBuildIdentity,
        CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Get, new Uri(origin, "/api/v1/sync/activation"));
        message.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer", await bearer.GetBearerTokenAsync(cancellationToken));
        message.Headers.TryAddWithoutValidation("X-Correlation-Id", Guid.NewGuid().ToString("D"));
        AddBuildIdentityHeaders(message, measuredBuildIdentity);
        using var response = await AuthenticationHttpClient.SendAsync(
            message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        return await DeserializeBoundedAsync<DesktopSyncActivationResponse>(response, cancellationToken);
    }

    private static bool TryValidateActivation(
        Uri origin,
        IdentitySessionResponse session,
        DesktopSyncActivationResponse? activation,
        out Uri? batchEndpoint,
        out IReadOnlySet<(string Action, string Operation, string Entity)>? allowedActions,
        out DesktopDeviceProofBinding? proofBinding,
        out SyncClientEffectivePolicy? effectivePolicy)
    {
        batchEndpoint = null;
        allowedActions = null;
        proofBinding = null;
        effectivePolicy = null;
        if (activation is null || !activation.Enabled ||
            activation.ClosedReason is not null ||
            activation.CompanyId != session.CompanyId || activation.BranchId != session.BranchId ||
            activation.UserId != session.UserId || activation.SessionId != session.SessionId ||
            activation.RegisteredDeviceId == Guid.Empty ||
            !string.Equals(activation.DeviceId, session.DeviceId, StringComparison.Ordinal) ||
            activation.AllowedActions is null || activation.AllowedActions.Count == 0 ||
            activation.ProofKeyVersion is not > 0 ||
            activation.ProofPublicJwk is null ||
            string.IsNullOrWhiteSpace(activation.PolicySourceVersion) ||
            !IsLowerHex64(activation.PolicySourceFingerprint) ||
            !TryEffectivePolicy(activation, out effectivePolicy) ||
            !TryHttpsBatchEndpoint(origin, activation.BatchEndpoint, out batchEndpoint))
            return false;

        var catalog = SyncActionCatalog.Definitions
            .Where(definition => definition.RuntimeAvailability == SyncActionRuntimeAvailability.Available)
            .ToDictionary(
                definition => definition.ActionCodeValue,
                definition => (definition.OperationTypeValue, definition.EntityTypeValue),
                StringComparer.Ordinal);
        var set = new HashSet<(string Action, string Operation, string Entity)>();
        foreach (var action in activation.AllowedActions)
        {
            if (action is null || string.IsNullOrEmpty(action.ActionCode) ||
                string.IsNullOrEmpty(action.OperationType) || string.IsNullOrEmpty(action.EntityType) ||
                !catalog.TryGetValue(action.ActionCode, out var expected) ||
                !string.Equals(action.OperationType, expected.OperationTypeValue, StringComparison.Ordinal) ||
                !string.Equals(action.EntityType, expected.EntityTypeValue, StringComparison.Ordinal) ||
                !set.Add((action.ActionCode, action.OperationType, action.EntityType)))
                return false;
        }

        var jwk = activation.ProofPublicJwk;
        if (jwk.X is null || jwk.Y is null ||
            !string.Equals(jwk.Kty, "EC", StringComparison.Ordinal) ||
            !string.Equals(jwk.Crv, "P-256", StringComparison.Ordinal) ||
            !IsBase64UrlCoordinate(jwk.X) || !IsBase64UrlCoordinate(jwk.Y) ||
            string.IsNullOrEmpty(activation.ProofKeyThumbprint) ||
            activation.ProofKeyThumbprint.Length != 43)
            return false;

        allowedActions = set;
        proofBinding = new DesktopDeviceProofBinding(
            activation.ProofKeyVersion.Value,
            activation.ProofKeyThumbprint,
            jwk.X,
            jwk.Y);
        return true;
    }

    private static DesktopOfflineCompositionOptions CreateCompositionOptions(
        IdentitySessionResponse session,
        DesktopSyncActivationResponse activation,
        Uri batchEndpoint,
        string certificateThumbprint,
        DesktopDeviceProofBinding proofBinding,
        SyncClientEffectivePolicy effectivePolicy,
        BuildIdentityV1 measuredBuildIdentity)
    {
        var root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TransportERP", "offline", session.CompanyId.ToString("N"),
            session.BranchId!.Value.ToString("N"), session.UserId.ToString("N"),
            activation.RegisteredDeviceId.ToString("N"));
        return new DesktopOfflineCompositionOptions(
            session.CompanyId,
            session.BranchId.Value,
            session.UserId,
            activation.RegisteredDeviceId,
            Path.Combine(root, "write-outbox.db"),
            Path.Combine(root, "read-cache.db"),
            Path.Combine(root, "keys"),
            certificateThumbprint,
            proofBinding,
            new OfflineSyncTransportOptions(
                batchEndpoint,
                session.DeviceId,
                activation.RegisteredDeviceId,
                session.CompanyId,
                session.BranchId.Value,
                session.UserId,
                $"desktop-{Guid.NewGuid():N}",
                MaximumBatchOperations: effectivePolicy.MaxBatchOperations,
                MaximumRequestBodyBytes: effectivePolicy.MaximumRequestBodyBytes,
                MaximumPayloadBytes: effectivePolicy.MaximumPayloadBytes,
                BuildIdentity: measuredBuildIdentity),
            effectivePolicy,
            new OfflineRetryPolicy(
                effectivePolicy.ClientTransportMaxRetryCount,
                effectivePolicy.ClientRetryBaseDelay,
                effectivePolicy.ClientRetryMaxDelay),
            OfflineRuntimeAuthorized: true);
    }

    private static void AddBuildIdentityHeaders(HttpRequestMessage request, BuildIdentityV1 identity)
    {
        if (!identity.IsValid) throw new InvalidOperationException("BUILD_IDENTITY_UNAVAILABLE");
        request.Headers.TryAddWithoutValidation(BuildIdentityV1.PlatformHeader, identity.Platform);
        request.Headers.TryAddWithoutValidation(BuildIdentityV1.ArtifactSha256Header, identity.ArtifactSha256);
        if (identity.SignerCertificateSha256 is { } signer)
            request.Headers.TryAddWithoutValidation(BuildIdentityV1.SignerCertificateSha256Header, signer);
    }

    private static bool TryEffectivePolicy(
        DesktopSyncActivationResponse activation,
        out SyncClientEffectivePolicy? policy)
    {
        policy = new SyncClientEffectivePolicy(
            activation.MaxBatchOperations,
            activation.ClientTransportMaxRetryCount,
            activation.ClientTransportBaseSeconds,
            activation.ClientTransportMaxDelayMinutes,
            activation.LocalSuccessHours,
            activation.LocalRejectedDays,
            activation.ServerPayloadDays,
            activation.CacheMaxAgeHours,
            activation.MaximumRequestBodyBytes,
            activation.MaximumPayloadBytes,
            activation.PolicySourceVersion ?? string.Empty,
            activation.PolicySourceFingerprint ?? string.Empty,
            activation.ActivationImplementationSha ?? string.Empty);
        if (policy.IsValid) return true;
        policy = null;
        return false;
    }

    private static bool TryHttpsBatchEndpoint(Uri origin, string value, out Uri? endpoint)
    {
        endpoint = null;
        if (!Uri.TryCreate(value, UriKind.Absolute, out var candidate) ||
            candidate.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(candidate.UserInfo) ||
            !string.IsNullOrEmpty(candidate.Query) || !string.IsNullOrEmpty(candidate.Fragment) ||
            candidate.AbsolutePath != "/api/v1/sync/operations:batch" ||
            !string.Equals(candidate.IdnHost, origin.IdnHost, StringComparison.OrdinalIgnoreCase) ||
            candidate.Port != origin.Port)
            return false;
        endpoint = candidate;
        return true;
    }

    private static bool IsBase64UrlCoordinate(string value)
    {
        if (value.Length != 43 || value.Any(character =>
                !(character is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_')))
            return false;
        try
        {
            var bytes = Convert.FromBase64String(value.Replace('-', '+').Replace('_', '/') + "=");
            return bytes.Length == 32;
        }
        catch (FormatException) { return false; }
    }

    private static bool IsLowerHex64(string? value) =>
        value is { Length: 64 } && value.All(character =>
            character is >= '0' and <= '9' or >= 'a' and <= 'f');

    private static async Task<T?> DeserializeBoundedAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.Content.Headers.ContentLength is > 65_536)
            throw new JsonException("AUTH_RESPONSE_TOO_LARGE");
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var bounded = new MemoryStream();
        var buffer = new byte[4096];
        while (true)
        {
            var read = await stream.ReadAsync(buffer, cancellationToken);
            if (read == 0)
                break;
            if (bounded.Length + read > 65_536)
                throw new JsonException("AUTH_RESPONSE_TOO_LARGE");
            await bounded.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
        }
        return JsonSerializer.Deserialize<T>(bounded.ToArray(), ApiJson);
    }

    private HttpClient AuthenticationHttpClient =>
        _authenticationHttpClient ??= CreateHttpClient();

    private HttpClient CreateHttpClient()
    {
        var client = new HttpClient(_handlerFactory(), disposeHandler: true)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        return client;
    }

    private sealed class VolatileBearerTokenProvider : IInMemoryBearerTokenProvider, IDisposable
    {
        private string? _token;

        internal VolatileBearerTokenProvider(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || token.Any(char.IsWhiteSpace))
                throw new ArgumentException("The bearer token is invalid.", nameof(token));
            _token = token;
        }

        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(_token ?? throw new InvalidOperationException("DESKTOP_SESSION_ENDED"));
        }

        public void Dispose() => _token = null;
    }

    private sealed class AuthenticatedDesktopSyncNetworkProvider(HttpClient httpClient) : IDesktopSyncNetworkProvider
    {
        public bool IsTransportAvailable => true;
        public bool IsNetworkAvailable => NetworkInterface.GetIsNetworkAvailable();
        public HttpClient SyncHttpClient { get; } = httpClient;
    }

    private sealed class ServerValidatedOfflineWritePolicy(
        IReadOnlySet<(string Action, string Operation, string Entity)> available) : IDesktopOfflineWritePolicy
    {
        public bool Allows(string actionCode, string operationType, string entityType) =>
            available.Contains((actionCode, operationType, entityType));
    }

    private sealed class ServerValidatedOperationsPermissionPolicy(
        bool canRetryFailedOperations,
        bool canResolveConflicts)
        : ISyncOperationsPermissionPolicy
    {
        public bool CanRetry(OfflineOperation operation) =>
            canRetryFailedOperations && operation.Status == OfflineOperationStatus.Failed;

        public bool CanResolveConflict(OfflineOperation operation, SyncConflictDecision decision) =>
            canResolveConflicts && operation.Status == OfflineOperationStatus.Conflict;
    }

}

internal sealed record DesktopSyncActivationAction(string ActionCode, string OperationType, string EntityType);

internal sealed record DesktopSyncActivationProofPublicJwk(string Kty, string Crv, string X, string Y);

internal sealed record DesktopSyncActivationResponse(
    bool Enabled,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    Guid SessionId,
    string DeviceId,
    string BatchEndpoint,
    IReadOnlyList<DesktopSyncActivationAction> AllowedActions,
    bool CanRetryFailedOperations,
    bool CanResolveConflicts,
    int? ProofKeyVersion,
    string? ProofKeyThumbprint,
    DesktopSyncActivationProofPublicJwk? ProofPublicJwk,
    bool KeyEnrollmentAllowed,
    bool KeyRecoveryAllowed,
    string? PolicySourceVersion,
    string? PolicySourceFingerprint,
    int MaxBatchOperations,
    int ClientTransportMaxRetryCount,
    int ClientTransportBaseSeconds,
    int ClientTransportMaxDelayMinutes,
    int LocalSuccessHours,
    int LocalRejectedDays,
    int ServerPayloadDays,
    int CacheMaxAgeHours,
    int MaximumRequestBodyBytes,
    int MaximumPayloadBytes,
    string? ActivationImplementationSha,
    BuildIdentityV1? AuthorizedBuildIdentity,
    string? ClosedReason);
