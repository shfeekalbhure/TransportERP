using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransportERP.Application.Sync;
using TransportERP.Mobile.Driver.Platforms.Android;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.Offline;

/// <summary>
/// Secrets supplied by the interactive sign-in surface. This is deliberately a class rather than
/// a record so generated formatting cannot disclose the password or device credential.
/// </summary>
public sealed class DriverInteractiveSignInRequest
{
    public DriverInteractiveSignInRequest(
        Uri serverOrigin,
        string userNameOrEmail,
        string password,
        Guid? companyId,
        Guid? branchId,
        string deviceId,
        string? deviceCredential)
    {
        ServerOrigin = serverOrigin;
        UserNameOrEmail = userNameOrEmail;
        Password = password;
        CompanyId = companyId;
        BranchId = branchId;
        DeviceId = deviceId;
        DeviceCredential = deviceCredential;
    }

    public Uri ServerOrigin { get; }
    internal string UserNameOrEmail { get; }
    internal string Password { get; }
    public Guid? CompanyId { get; }
    public Guid? BranchId { get; }
    public string DeviceId { get; }
    internal string? DeviceCredential { get; }
}

/// <summary>
/// The only production Mobile activation producer. It first creates an explicit authenticated
/// local session, then obtains a server-computed, exact-scope activation decision. No local
/// configuration or startup path can grant Offline authority.
/// </summary>
public sealed class DriverAuthenticatedActivationCoordinator(
    DriverOfflineActivationService activation,
    IDriverSyncNetworkProvider network,
    DriverServerOfflineFeatureGate featureGate,
    DriverServerDeviceKeyBindingVerifier bindingVerifier,
    AndroidKeystoreDeviceSigningKey signingKey)
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 16
    };

    private readonly SemaphoreSlim _gate = new(1, 1);
    private CancellationTokenSource? _expiryCancellation;
    private AuthenticatedSessionHandle? _authenticatedSession;

    public async Task<DriverOfflineActivationResult> SignInAndActivateAsync(
        DriverInteractiveSignInRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateInteractiveRequest(request);
        await _gate.WaitAsync(cancellationToken);
        try
        {
            await DeactivateCoreAsync(cancellationToken);
            var session = await CreateSessionAsync(request, cancellationToken);
            _authenticatedSession = new(
                request.ServerOrigin, session.SessionId, session.AccessToken, session.AccessTokenExpiresAt);
            try
            {
                var decision = await GetActivationDecisionAsync(
                    request.ServerOrigin, session.AccessToken, cancellationToken);
                ValidateDecision(request, session, decision);

                var localKeyAvailable = await signingKey.IsNativeSigningKeyAvailableAsync(cancellationToken);
                DevicePublicP256Jwk? localPublicKey = localKeyAvailable
                    ? await signingKey.GetPublicJwkAsync(cancellationToken)
                    : null;
                var serverHasBinding = decision.ProofKeyVersion is >= 1;
                var localMatchesServer = localPublicKey is not null && serverHasBinding &&
                    PublicKeyMatchesDecision(localPublicKey, decision);
                if (!serverHasBinding || !localMatchesServer)
                {
                    var provisioning = !localKeyAvailable
                        ? DriverKeyProvisioning.Create
                        : serverHasBinding
                            ? DriverKeyProvisioning.ReplaceForRecovery
                            : DriverKeyProvisioning.UseExisting;
                    await EnrollOrRecoverKeyAsync(
                        request.ServerOrigin, session, decision, provisioning,
                        cancellationToken);
                    if (serverHasBinding)
                        throw new DriverOfflineUnavailableException(
                            "DEVICE_KEY_RECOVERY_REAUTHENTICATION_REQUIRED");
                    decision = await GetActivationDecisionAsync(
                        request.ServerOrigin, session.AccessToken, cancellationToken);
                    ValidateDecision(request, session, decision);
                }

                featureGate.Authorize(decision, session.AccessTokenExpiresAt);
                bindingVerifier.Authorize(decision, session.AccessTokenExpiresAt);
                var result = await activation.ActivateAsync(
                    new DriverOfflineActivationRequest(
                        decision.CompanyId,
                        decision.BranchId,
                        decision.UserId,
                        decision.RegisteredDeviceId,
                        decision.SessionId,
                        decision.DeviceId,
                        session.AccessToken,
                        decision.BatchEndpoint,
                        decision.AllowedActions.Select(action => new DriverOfflineActionGrant(
                            action.ActionCode, action.OperationType, action.EntityType)).ToArray(),
                        new DriverOfflineOperationPermissions(
                            decision.CanRetryFailedOperations,
                            decision.CanResolveConflicts),
                        offlineRuntimeAuthorized: true),
                    cancellationToken);
                ArmExpiry(session.AccessTokenExpiresAt);
                return result;
            }
            catch
            {
                featureGate.Clear();
                bindingVerifier.Clear();
                await DeactivateCoreAsync(CancellationToken.None);
                throw;
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try { await DeactivateCoreAsync(cancellationToken); }
        finally { _gate.Release(); }
    }

    private async Task<DriverSessionResponse> CreateSessionAsync(
        DriverInteractiveSignInRequest request,
        CancellationToken cancellationToken)
    {
        var endpoint = Endpoint(request.ServerOrigin, "/api/v1/auth/sessions");
        using var message = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new
            {
                userNameOrEmail = request.UserNameOrEmail,
                password = request.Password,
                companyId = request.CompanyId,
                branchId = request.BranchId,
                deviceId = request.DeviceId,
                deviceCredential = request.DeviceCredential
            })
        };
        using var response = await network.SyncHttpClient.SendAsync(
            message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DriverOfflineUnavailableException("AUTHENTICATION_FAILED");
        var session = await ReadJsonAsync<DriverSessionResponse>(response, cancellationToken);
        if (session.SessionId == Guid.Empty || session.UserId == Guid.Empty ||
            session.CompanyId == Guid.Empty || session.BranchId is null || session.BranchId == Guid.Empty ||
            session.CompanyId != request.CompanyId || session.BranchId != request.BranchId ||
            string.IsNullOrEmpty(session.AccessToken) || !IsSafeBearer(session.AccessToken) ||
            session.AccessTokenExpiresAt <= DateTimeOffset.UtcNow ||
            !string.Equals(session.DeviceId, request.DeviceId, StringComparison.Ordinal))
        {
            throw new DriverOfflineUnavailableException("AUTHENTICATED_SCOPE_INVALID");
        }
        return session;
    }

    private async Task<DriverServerActivationDecision> GetActivationDecisionAsync(
        Uri origin,
        string bearer,
        CancellationToken cancellationToken)
    {
        using var message = AuthorizedRequest(
            HttpMethod.Get, Endpoint(origin, "/api/v1/sync/activation"), bearer);
        using var response = await network.SyncHttpClient.SendAsync(
            message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            throw new DriverOfflineUnavailableException(
                response.StatusCode == HttpStatusCode.Forbidden ? "OFFLINE_CLOSED" :
                "OFFLINE_ACTIVATION_AUTHORITY_UNAVAILABLE");
        return await ReadJsonAsync<DriverServerActivationDecision>(response, cancellationToken);
    }

    private async Task EnrollOrRecoverKeyAsync(
        Uri origin,
        DriverSessionResponse session,
        DriverServerActivationDecision decision,
        DriverKeyProvisioning provisioning,
        CancellationToken cancellationToken)
    {
        var changeType = decision.ProofKeyVersion is >= 1 ? "RECOVER" : "BIND";
        if (changeType == "BIND" && !decision.KeyEnrollmentAllowed ||
            changeType == "RECOVER" && !decision.KeyRecoveryAllowed)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_NOT_AUTHORIZED");

        if (provisioning is not DriverKeyProvisioning.UseExisting)
        {
            var authority = new DriverDeviceKeyEnrollmentAuthorization(
                decision.CompanyId,
                decision.BranchId,
                decision.UserId,
                decision.RegisteredDeviceId,
                decision.SessionId,
                changeType,
                DateTimeOffset.UtcNow.AddMinutes(2));
            if (provisioning == DriverKeyProvisioning.ReplaceForRecovery)
                await signingKey.ReplaceForAuthorizedRecoveryAsync(authority, cancellationToken);
            else
                await signingKey.ProvisionForAuthorizedEnrollmentAsync(authority, cancellationToken);
        }
        var publicKey = await signingKey.GetPublicJwkAsync(cancellationToken);
        var changeRequestId = Guid.NewGuid();
        var expectedVersion = decision.ProofKeyVersion;
        var challengeEndpoint = Endpoint(origin,
            $"/api/v1/devices/{decision.RegisteredDeviceId:D}/proof-key-challenges");
        using var challengeRequest = AuthorizedRequest(HttpMethod.Post, challengeEndpoint, session.AccessToken);
        challengeRequest.Content = JsonContent.Create(new
        {
            changeRequestId,
            changeType,
            expectedProofKeyVersion = expectedVersion,
            newPublicJwk = PublicJwk(publicKey)
        });
        using var challengeResponse = await network.SyncHttpClient.SendAsync(
            challengeRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (challengeResponse.StatusCode != HttpStatusCode.OK)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_CHALLENGE_REJECTED");
        var challenge = await ReadJsonAsync<DriverProofKeyChallengeResponse>(
            challengeResponse, cancellationToken);
        if (challenge.ChallengeId == Guid.Empty || challenge.ChangeRequestId != changeRequestId ||
            challenge.ChangeType != changeType || challenge.ExpectedProofKeyVersion != expectedVersion ||
            challenge.ExpiresAt <= DateTimeOffset.UtcNow || string.IsNullOrEmpty(challenge.Challenge) ||
            !FixedEquals(challenge.NewProofKeyThumbprint, Thumbprint(publicKey)))
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_CHALLENGE_INVALID");

        var suffix = changeType == "BIND" ? "bind-proof-key" : "recover-proof-key";
        var changeEndpoint = Endpoint(origin,
            $"/api/v1/devices/{decision.RegisteredDeviceId:D}:{suffix}");
        var reason = changeType == "RECOVER" ? "Android Keystore alias unavailable; explicit recovery requested." : null;
        var rawBody = JsonSerializer.SerializeToUtf8Bytes(new
        {
            challengeId = challenge.ChallengeId,
            changeRequestId,
            changeType,
            expectedProofKeyVersion = expectedVersion,
            newPublicJwk = PublicJwk(publicKey),
            reason
        }, Json);
        var proof = await CreateProofAsync(
            publicKey, challenge, decision.RegisteredDeviceId, changeEndpoint,
            session.AccessToken, rawBody, signingKey, cancellationToken);
        HttpResponseMessage changeResponse;
        try
        {
            using var changeRequest = AuthorizedRequest(HttpMethod.Post, changeEndpoint, session.AccessToken);
            changeRequest.Headers.TryAddWithoutValidation("Device-Key-Proof-New", proof);
            changeRequest.Content = new ByteArrayContent(rawBody);
            changeRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            changeResponse = await network.SyncHttpClient.SendAsync(
                changeRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(rawBody);
        }
        using (changeResponse)
        {
        if (changeResponse.StatusCode != HttpStatusCode.OK)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_REJECTED");
        var changed = await ReadJsonAsync<DriverProofKeyChangeResponse>(changeResponse, cancellationToken);
        if (changed.RegisteredDeviceId != decision.RegisteredDeviceId ||
            changed.ChangeRequestId != changeRequestId || changed.ChangeType != changeType ||
            !FixedEquals(changed.ProofKeyThumbprint, Thumbprint(publicKey)) || changed.ProofKeyVersion < 1)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_RESPONSE_INVALID");
        }
    }

    private static async Task<string> CreateProofAsync(
        DevicePublicP256Jwk publicKey,
        DriverProofKeyChallengeResponse challenge,
        Guid registeredDeviceId,
        Uri endpoint,
        string bearer,
        byte[] rawBody,
        IDriverNativeDeviceSigningKey signer,
        CancellationToken cancellationToken)
    {
        var header = Base64Url(JsonSerializer.SerializeToUtf8Bytes(new
        {
            typ = "transporterp-key-change+jwt",
            alg = "ES256",
            jwk = PublicJwk(publicKey)
        }, Json));
        var payload = Base64Url(JsonSerializer.SerializeToUtf8Bytes(new
        {
            cid = challenge.ChallengeId.ToString("D"),
            rid = challenge.ChangeRequestId.ToString("D"),
            did = registeredDeviceId.ToString("D"),
            ct = challenge.ChangeType,
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            jti = Guid.NewGuid().ToString("D"),
            chl = challenge.Challenge,
            nkt = challenge.NewProofKeyThumbprint,
            htm = "POST",
            htu = endpoint.AbsoluteUri,
            ath = HashAsciiBase64Url(bearer),
            tbh = Base64Url(SHA256.HashData(rawBody))
        }, Json));
        var signingInput = Encoding.ASCII.GetBytes($"{header}.{payload}");
        try
        {
            var signature = await signer.SignEs256Async(signingInput, cancellationToken);
            try { return $"{header}.{payload}.{Base64Url(signature)}"; }
            finally { CryptographicOperations.ZeroMemory(signature); }
        }
        finally { CryptographicOperations.ZeroMemory(signingInput); }
    }

    private static void ValidateDecision(
        DriverInteractiveSignInRequest request,
        DriverSessionResponse session,
        DriverServerActivationDecision decision)
    {
        if (!decision.Enabled || decision.CompanyId != session.CompanyId ||
            decision.BranchId != session.BranchId || decision.UserId != session.UserId ||
            decision.SessionId != session.SessionId || decision.RegisteredDeviceId == Guid.Empty ||
            !string.Equals(decision.DeviceId, session.DeviceId, StringComparison.Ordinal) ||
            !HasOnlySupportedUniqueActions(decision.AllowedActions) ||
            string.IsNullOrWhiteSpace(decision.PolicySourceVersion) ||
            !IsSha256Hex(decision.PolicySourceFingerprint) ||
            decision.BatchEndpoint is null || !decision.BatchEndpoint.IsAbsoluteUri ||
            decision.BatchEndpoint.Scheme != Uri.UriSchemeHttps ||
            !SameOrigin(request.ServerOrigin, decision.BatchEndpoint) ||
            decision.BatchEndpoint.AbsolutePath != "/api/v1/sync/operations:batch" ||
            decision.ProofKeyVersion is <= 0 ||
            decision.ProofKeyVersion is null &&
                (decision.ProofPublicJwk is not null || decision.ProofKeyThumbprint is not null ||
                 decision.ClosedReason != "PROOF_KEY_BINDING_REQUIRED") ||
            decision.ProofKeyVersion is >= 1 &&
                (decision.ProofPublicJwk is null || string.IsNullOrWhiteSpace(decision.ProofKeyThumbprint) ||
                 decision.ClosedReason is not null) ||
            decision.ProofPublicJwk is { } publicJwk &&
                (publicJwk.Kty != "EC" || publicJwk.Crv != "P-256" ||
                 !IsCoordinate(publicJwk.X) || !IsCoordinate(publicJwk.Y) ||
                 !FixedEquals(decision.ProofKeyThumbprint!, Thumbprint(
                     new DevicePublicP256Jwk(publicJwk.X, publicJwk.Y)))))
            throw new DriverOfflineUnavailableException("OFFLINE_ACTIVATION_DECISION_INVALID");
    }

    private async Task DeactivateCoreAsync(CancellationToken cancellationToken)
    {
        _expiryCancellation?.Cancel();
        _expiryCancellation?.Dispose();
        _expiryCancellation = null;
        featureGate.Clear();
        bindingVerifier.Clear();
        var session = Interlocked.Exchange(ref _authenticatedSession, null);
        if (session is not null)
        {
            try
            {
                using var revoke = AuthorizedRequest(HttpMethod.Post,
                    Endpoint(session.ServerOrigin, $"/api/v1/auth/sessions/{session.SessionId:D}:revoke"),
                    session.Bearer);
                revoke.Content = JsonContent.Create(new { reason = "Mobile driver sign-out" });
                using var _ = await network.SyncHttpClient.SendAsync(
                    revoke, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch { /* Local teardown remains mandatory when revocation is unavailable. */ }
        }
        await activation.DeactivateAsync(CancellationToken.None);
    }

    private void ArmExpiry(DateTimeOffset expiresAt)
    {
        var cancellation = new CancellationTokenSource();
        _expiryCancellation = cancellation;
        _ = Task.Run(async () =>
        {
            try
            {
                var delay = expiresAt - DateTimeOffset.UtcNow;
                if (delay > TimeSpan.Zero) await Task.Delay(delay, cancellation.Token);
                await SignOutAsync(CancellationToken.None);
            }
            catch (OperationCanceledException) when (cancellation.IsCancellationRequested) { }
            catch { featureGate.Clear(); bindingVerifier.Clear(); }
        });
    }

    private static HttpRequestMessage AuthorizedRequest(HttpMethod method, Uri endpoint, string bearer)
    {
        var request = new HttpRequestMessage(method, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        request.Headers.Add("X-Correlation-Id", Guid.NewGuid().ToString("D"));
        return request;
    }

    private static async Task<T> ReadJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<T>(Json, cancellationToken)
                ?? throw new DriverOfflineUnavailableException("SERVER_RESPONSE_INVALID");
        }
        catch (JsonException exception)
        {
            throw new DriverOfflineUnavailableException("SERVER_RESPONSE_INVALID", exception);
        }
    }

    private static Uri Endpoint(Uri origin, string path)
    {
        if (origin.Scheme != Uri.UriSchemeHttps || origin.AbsolutePath != "/" ||
            !string.IsNullOrEmpty(origin.Query) || !string.IsNullOrEmpty(origin.Fragment) ||
            !string.IsNullOrEmpty(origin.UserInfo))
            throw new DriverOfflineUnavailableException("SERVER_ORIGIN_INVALID");
        return new Uri(origin, path);
    }

    private static void ValidateInteractiveRequest(DriverInteractiveSignInRequest request)
    {
        _ = Endpoint(request.ServerOrigin, "/");
        if (string.IsNullOrWhiteSpace(request.UserNameOrEmail) || string.IsNullOrEmpty(request.Password) ||
            request.CompanyId is null || request.CompanyId == Guid.Empty ||
            request.BranchId is null || request.BranchId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.DeviceId) || request.DeviceId.Any(char.IsWhiteSpace) ||
            request.DeviceCredential is { Length: 0 } ||
            request.DeviceCredential?.Any(character => character > 0x7f || char.IsWhiteSpace(character)) == true)
            throw new DriverOfflineUnavailableException("AUTHENTICATION_INPUT_INVALID");
    }

    private static bool IsSafeBearer(string value) =>
        value.Length is > 0 and <= 8192 && value.All(character => character <= 0x7f && !char.IsWhiteSpace(character));
    private static bool SameOrigin(Uri left, Uri right) =>
        string.Equals(left.Scheme, right.Scheme, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(left.IdnHost, right.IdnHost, StringComparison.OrdinalIgnoreCase) && left.Port == right.Port;
    private static object PublicJwk(DevicePublicP256Jwk key) => new { kty = "EC", crv = "P-256", x = key.X, y = key.Y };
    private static bool PublicKeyMatchesDecision(
        DevicePublicP256Jwk local,
        DriverServerActivationDecision decision) => decision.ProofPublicJwk is { } server &&
        string.Equals(local.X, server.X, StringComparison.Ordinal) &&
        string.Equals(local.Y, server.Y, StringComparison.Ordinal) &&
        decision.ProofKeyThumbprint is { } thumbprint && FixedEquals(thumbprint, Thumbprint(local));
    private static bool HasOnlySupportedUniqueActions(
        IReadOnlyList<DriverServerActivationAction>? actions)
    {
        if (actions is null || actions.Count == 0) return false;
        var supported = SyncActionCatalog.Definitions
            .Where(definition => definition.RuntimeAvailability == SyncActionRuntimeAvailability.Available)
            .Select(definition => (
                definition.ActionCodeValue,
                definition.OperationTypeValue,
                definition.EntityTypeValue))
            .ToHashSet();
        var actual = new HashSet<(string ActionCode, string OperationType, string EntityType)>();
        foreach (var action in actions)
        {
            if (action is null || !actual.Add((action.ActionCode, action.OperationType, action.EntityType)) ||
                !supported.Contains((action.ActionCode, action.OperationType, action.EntityType)))
                return false;
        }
        return true;
    }
    private static bool IsCoordinate(string value)
    {
        try { return DecodeBase64Url(value).Length == 32; }
        catch (FormatException) { return false; }
    }
    private static byte[] DecodeBase64Url(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Contains('=') || value.Any(character =>
                character is not (>= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_')))
            throw new FormatException();
        var normalized = value.Replace('-', '+').Replace('_', '/');
        normalized += new string('=', (4 - normalized.Length % 4) % 4);
        return Convert.FromBase64String(normalized);
    }
    private static bool IsSha256Hex(string? value) => value is { Length: 64 } && value.All(character =>
        character is >= '0' and <= '9' or >= 'a' and <= 'f');
    private static string Thumbprint(DevicePublicP256Jwk key) => Base64Url(SHA256.HashData(
        Encoding.ASCII.GetBytes($"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{key.X}\",\"y\":\"{key.Y}\"}}")));
    private static string Base64Url(byte[] value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    private static string HashAsciiBase64Url(string value)
    {
        var bytes = Encoding.ASCII.GetBytes(value);
        byte[]? hash = null;
        try
        {
            hash = SHA256.HashData(bytes);
            return Base64Url(hash);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(bytes);
            if (hash is not null) CryptographicOperations.ZeroMemory(hash);
        }
    }
    private static bool FixedEquals(string left, string right) =>
        CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(left), Encoding.UTF8.GetBytes(right));

    private sealed record DriverSessionResponse(
        Guid SessionId,
        string AccessToken,
        DateTimeOffset AccessTokenExpiresAt,
        string RefreshToken,
        DateTimeOffset RefreshTokenExpiresAt,
        Guid UserId,
        string DisplayName,
        Guid CompanyId,
        Guid? BranchId,
        string DeviceId);

    private sealed record DriverProofKeyChallengeResponse(
        Guid ChallengeId,
        Guid ChangeRequestId,
        string ChangeType,
        int? ExpectedProofKeyVersion,
        string NewProofKeyThumbprint,
        DateTimeOffset IssuedAt,
        DateTimeOffset ExpiresAt,
        string? Challenge);

    private sealed record DriverProofKeyChangeResponse(
        Guid RegisteredDeviceId,
        Guid ChangeRequestId,
        string ChangeType,
        string ProofKeyThumbprint,
        int ProofKeyVersion,
        DateTimeOffset ChangedAt);

    private sealed record AuthenticatedSessionHandle(
        Uri ServerOrigin,
        Guid SessionId,
        string Bearer,
        DateTimeOffset ExpiresAt);

    private enum DriverKeyProvisioning
    {
        UseExisting,
        Create,
        ReplaceForRecovery
    }
}

public sealed record DriverServerActivationAction(
    string ActionCode,
    string OperationType,
    string EntityType);

public sealed record DriverServerPublicJwk(string Kty, string Crv, string X, string Y);

public sealed record DriverServerActivationDecision(
    bool Enabled,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    Guid SessionId,
    string DeviceId,
    Uri BatchEndpoint,
    IReadOnlyList<DriverServerActivationAction> AllowedActions,
    bool CanRetryFailedOperations,
    bool CanResolveConflicts,
    int? ProofKeyVersion,
    string? ProofKeyThumbprint,
    DriverServerPublicJwk? ProofPublicJwk,
    bool KeyEnrollmentAllowed,
    bool KeyRecoveryAllowed,
    string? PolicySourceVersion,
    string? PolicySourceFingerprint,
    string? ClosedReason);
