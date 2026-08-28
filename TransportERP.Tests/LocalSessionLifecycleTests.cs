using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Identity;
using TransportERP.Contracts.Identity;

namespace TransportERP.Tests;

public sealed class LocalSessionLifecycleTests
{
    [Fact]
    public async Task Valid_login_creates_session_and_issues_tokens()
    {
        var fixture = Fixture.Create();

        var result = await fixture.Service.LoginAsync(fixture.Login());

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Tokens);
        Assert.NotEmpty(result.Tokens.RefreshToken);
        Assert.Equal(fixture.Authority.Current!.CompanyId, result.Tokens.CompanyId);
        Assert.Equal(LocalCredentialDisposition.Keep, result.CredentialDisposition);
    }

    [Fact]
    public async Task Invalid_credentials_fail_closed_and_clear_client_credentials()
    {
        var fixture = Fixture.Create();

        var result = await fixture.Service.LoginAsync(fixture.Login(password: "wrong"));

        Assert.False(result.Succeeded);
        Assert.Equal(LocalSessionFailure.InvalidCredentials, result.Failure);
        Assert.Equal(LocalCredentialDisposition.ClearAndSuspendOffline, result.CredentialDisposition);
    }

    [Fact]
    public async Task Disabled_user_is_denied()
    {
        var fixture = Fixture.Create();
        fixture.Authority.AuthenticationStatus = LocalAuthenticationStatus.Disabled;

        var result = await fixture.Service.LoginAsync(fixture.Login());

        Assert.Equal(LocalSessionFailure.AccountDisabled, result.Failure);
    }

    [Fact]
    public async Task Wrong_tenant_membership_is_denied()
    {
        var fixture = Fixture.Create();

        var result = await fixture.Service.LoginAsync(
            fixture.Login(companyId: Guid.NewGuid()));

        Assert.Equal(LocalSessionFailure.ScopeDenied, result.Failure);
    }

    [Fact]
    public async Task Issued_access_token_is_accepted_while_session_and_authority_are_current()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());

        var validation = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);

        Assert.True(login.Succeeded);
        Assert.True(validation.Succeeded);
    }

    [Fact]
    public async Task Expired_access_token_is_denied()
    {
        var fixture = Fixture.Create();
        await fixture.Service.LoginAsync(fixture.Login());
        fixture.Clock.UtcNow = fixture.Issuer.Last!.ExpiresAt;

        var validation = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last);

        Assert.Equal(LocalSessionFailure.SessionExpired, validation.Failure);
    }

    [Fact]
    public async Task Revoked_session_is_denied()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        await fixture.Service.RevokeCurrentSessionAsync(login.Tokens!.SessionId, "ADMIN_REVOKE");

        var validation = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);

        Assert.Equal(LocalSessionFailure.SessionRevoked, validation.Failure);
    }

    [Fact]
    public async Task Refresh_rotates_one_time_token_and_preserves_family()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());

        var refresh = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens!.RefreshToken, login.Tokens.DeviceId));

        Assert.True(refresh.Succeeded);
        Assert.NotEqual(login.Tokens.SessionId, refresh.Tokens!.SessionId);
        Assert.Equal(login.Tokens.SessionFamilyId, refresh.Tokens.SessionFamilyId);
        Assert.NotEqual(login.Tokens.RefreshToken, refresh.Tokens.RefreshToken);
    }

    [Fact]
    public async Task Reused_refresh_token_revokes_entire_family()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        var rotated = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens!.RefreshToken, login.Tokens.DeviceId));

        var reuse = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens.RefreshToken, login.Tokens.DeviceId));
        var rotatedAccess = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);

        Assert.True(rotated.Succeeded);
        Assert.Equal(LocalSessionFailure.RefreshReuseDetected, reuse.Failure);
        Assert.Equal(LocalSessionFailure.SessionRevoked, rotatedAccess.Failure);
    }

    [Fact]
    public async Task Expired_refresh_token_is_denied_and_family_revoked()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        fixture.Clock.UtcNow = login.Tokens!.RefreshTokenExpiresAt;

        var refresh = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens.RefreshToken, login.Tokens.DeviceId));

        Assert.Equal(LocalSessionFailure.RefreshExpired, refresh.Failure);
        Assert.True(fixture.Store.IsFamilyRevoked(login.Tokens.SessionId));
    }

    [Fact]
    public async Task Logout_denies_access_and_refresh()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        await fixture.Service.LogoutAsync(login.Tokens!.SessionId);

        var access = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);
        var refresh = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens.RefreshToken, login.Tokens.DeviceId));

        Assert.Equal(LocalSessionFailure.SessionRevoked, access.Failure);
        Assert.Equal(LocalSessionFailure.RefreshReuseDetected, refresh.Failure);
    }

    [Fact]
    public async Task Revoked_membership_after_issue_revokes_family()
    {
        var fixture = Fixture.Create();
        await fixture.Service.LoginAsync(fixture.Login());
        fixture.Authority.Current = null;

        var validation = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);

        Assert.Equal(LocalSessionFailure.SecurityContextChanged, validation.Failure);
        Assert.True(fixture.Store.IsFamilyRevoked(fixture.Issuer.Last!.SessionId));
    }

    [Fact]
    public async Task Stale_security_version_is_denied()
    {
        var fixture = Fixture.Create();
        await fixture.Service.LoginAsync(fixture.Login());
        fixture.Authority.Current = fixture.Authority.Current! with { SecurityVersion = 2 };

        var validation = await fixture.Service.ValidateAccessAsync(fixture.Issuer.Last!);

        Assert.Equal(LocalSessionFailure.SecurityContextChanged, validation.Failure);
    }

    [Fact]
    public async Task Concurrent_refresh_allows_at_most_one_rotation_and_revokes_family()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        fixture.Store.CoordinateNextTwoRefreshReads();
        var request = new LocalRefreshRequest(login.Tokens!.RefreshToken, login.Tokens.DeviceId);

        var results = await Task.WhenAll(
            fixture.Service.RefreshAsync(request),
            fixture.Service.RefreshAsync(request));

        Assert.Single(results.Where(x => x.Succeeded));
        Assert.Single(results.Where(x => x.Failure == LocalSessionFailure.RefreshReuseDetected));
        Assert.Equal(2, fixture.Store.Family(login.Tokens.SessionFamilyId).Count);
        Assert.All(fixture.Store.Family(login.Tokens.SessionFamilyId), x => Assert.NotNull(x.RevokedAt));
    }

    [Fact]
    public async Task Cross_company_session_misuse_is_denied()
    {
        var fixture = Fixture.Create();
        await fixture.Service.LoginAsync(fixture.Login());
        var forged = fixture.Issuer.Last! with { CompanyId = Guid.NewGuid() };

        var validation = await fixture.Service.ValidateAccessAsync(forged);

        Assert.Equal(LocalSessionFailure.SecurityContextChanged, validation.Failure);
    }

    [Fact]
    public async Task Offline_submission_after_revoke_is_denied_and_suspended()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        await fixture.Service.RevokeSessionFamilyAsync(login.Tokens!.SessionId, "DEVICE_LOST");

        var validation = await fixture.Service.ValidateOfflineMutationAsync(fixture.Issuer.Last!);

        Assert.False(validation.Succeeded);
        Assert.Equal(LocalCredentialDisposition.ClearAndSuspendOffline, validation.CredentialDisposition);
    }

    [Fact]
    public void Local_access_token_is_cryptographically_accepted_and_has_no_permission_authority_claims()
    {
        const string signingKey = "unit-test-signing-key-that-is-at-least-32-bytes-long";
        var options = new LocalAccessTokenOptions(
            "TransportERP.Local", "TransportERP.Clients",
            signingKey);
        var issuer = new JwtLocalAccessTokenIssuer(options);
        var now = DateTimeOffset.UtcNow;

        var issued = issuer.Issue(new LocalAccessTokenDescriptor(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "device-1", 1, now, now.AddMinutes(10)));
        var token = new JwtSecurityTokenHandler().ReadJwtToken(issued.Token);
        var principal = new JwtSecurityTokenHandler().ValidateToken(
            issued.Token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            },
            out _);

        Assert.True(principal.Identity?.IsAuthenticated);
        Assert.DoesNotContain(token.Claims, x => x.Type is "permission" or ClaimTypes.Role);
        Assert.Contains(token.Claims, x => x.Type == "session_id");
        Assert.Contains(token.Claims, x => x.Type == "security_version" && x.Value == "1");
    }

    [Fact]
    public async Task Device_mismatch_during_refresh_revokes_family()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());

        var refresh = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens!.RefreshToken, "other-device"));

        Assert.Equal(LocalSessionFailure.DeviceMismatch, refresh.Failure);
        Assert.True(fixture.Store.IsFamilyRevoked(login.Tokens.SessionId));
    }

    [Fact]
    public async Task Session_mutations_commit_their_audit_intent_atomically()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        var refresh = await fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens!.RefreshToken, login.Tokens.DeviceId));
        await fixture.Service.LogoutAsync(refresh.Tokens!.SessionId);

        Assert.Equal(
            ["SESSION_CREATED", "SESSION_REFRESH_ROTATED", "SESSION_LOGOUT"],
            fixture.Store.AuditActions);
    }

    [Fact]
    public async Task Audit_failure_leaves_login_session_state_unchanged()
    {
        var fixture = Fixture.Create();
        fixture.Store.FailNextAudit = true;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => fixture.Service.LoginAsync(fixture.Login()));

        Assert.Empty(fixture.Store.AllSessions);
        Assert.Empty(fixture.Store.AuditActions);
    }

    [Fact]
    public async Task Audit_failure_leaves_refresh_rotation_state_unchanged()
    {
        var fixture = Fixture.Create();
        var login = await fixture.Service.LoginAsync(fixture.Login());
        fixture.Store.FailNextAudit = true;

        await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Service.RefreshAsync(
            new LocalRefreshRequest(login.Tokens!.RefreshToken, login.Tokens.DeviceId)));

        var family = fixture.Store.Family(login.Tokens.SessionFamilyId);
        Assert.Single(family);
        Assert.Null(family[0].RevokedAt);
        Assert.Null(family[0].ReplacedBySessionId);
        Assert.Equal(["SESSION_CREATED"], fixture.Store.AuditActions);
    }

    private sealed class Fixture
    {
        private Fixture(
            FakeAuthority authority,
            InMemoryAtomicSessionStore store,
            RecordingIssuer issuer,
            ManualTimeProvider clock,
            LocalSessionLifecycleService service)
        {
            Authority = authority;
            Store = store;
            Issuer = issuer;
            Clock = clock;
            Service = service;
        }

        public FakeAuthority Authority { get; }
        public InMemoryAtomicSessionStore Store { get; }
        public RecordingIssuer Issuer { get; }
        public ManualTimeProvider Clock { get; }
        public LocalSessionLifecycleService Service { get; }

        public static Fixture Create()
        {
            var authority = new FakeAuthority
            {
                Current = new LocalAuthoritySnapshot(
                    Guid.NewGuid(), "Test User", Guid.NewGuid(), Guid.NewGuid(), 1)
            };
            var store = new InMemoryAtomicSessionStore();
            var issuer = new RecordingIssuer();
            var clock = new ManualTimeProvider
            {
                UtcNow = new DateTimeOffset(2026, 8, 28, 12, 0, 0, TimeSpan.Zero)
            };
            var service = new LocalSessionLifecycleService(
                authority, store, issuer, clock,
                new LocalSessionLifecycleOptions(TimeSpan.FromMinutes(10), TimeSpan.FromDays(14)));
            return new(authority, store, issuer, clock, service);
        }

        public LocalLoginRequest Login(
            string password = "correct-password",
            Guid? companyId = null)
            => new("test", password, companyId ?? Authority.Current!.CompanyId,
                Authority.Current!.BranchId, "device-1");
    }

    private sealed class FakeAuthority : ILocalIdentityAuthority
    {
        public LocalAuthoritySnapshot? Current { get; set; }
        public LocalAuthenticationStatus AuthenticationStatus { get; set; }
            = LocalAuthenticationStatus.Succeeded;

        public Task<LocalAuthenticationResult> AuthenticateAsync(
            string userNameOrEmail,
            string password,
            Guid companyId,
            Guid? branchId,
            CancellationToken cancellationToken = default)
        {
            var status = AuthenticationStatus;
            if (password != "correct-password")
                status = LocalAuthenticationStatus.InvalidCredentials;
            else if (Current is null || Current.CompanyId != companyId || Current.BranchId != branchId)
                status = LocalAuthenticationStatus.ScopeDenied;
            return Task.FromResult(new LocalAuthenticationResult(
                status, status == LocalAuthenticationStatus.Succeeded ? Current : null));
        }

        public Task<LocalAuthoritySnapshot?> GetCurrentAsync(
            Guid userId,
            Guid companyId,
            Guid? branchId,
            CancellationToken cancellationToken = default)
            => Task.FromResult(Current is not null &&
                Current.UserId == userId && Current.CompanyId == companyId &&
                Current.BranchId == branchId ? Current : null);
    }

    private sealed class RecordingIssuer : ILocalAccessTokenIssuer
    {
        public LocalAccessTokenDescriptor? Last { get; private set; }

        public LocalIssuedAccessToken Issue(LocalAccessTokenDescriptor descriptor)
        {
            Last = descriptor;
            return new($"access:{descriptor.SessionId}", descriptor.ExpiresAt);
        }
    }

    private sealed class ManualTimeProvider : TimeProvider
    {
        public DateTimeOffset UtcNow { get; set; }
        public override DateTimeOffset GetUtcNow() => UtcNow;
    }

    /// <summary>Test-only implementation; it is never registered by the API.</summary>
    private sealed class InMemoryAtomicSessionStore : ILocalSessionStore
    {
        private readonly object gate = new();
        private readonly Dictionary<Guid, LocalSessionRecord> sessions = new();
        private readonly Dictionary<string, Guid> refreshIndex = new(StringComparer.Ordinal);
        private readonly List<LocalSessionAuditIntent> audits = [];
        private TaskCompletionSource? coordinatedReads;
        private int remainingCoordinatedReads;

        public void CoordinateNextTwoRefreshReads()
        {
            coordinatedReads = new(TaskCreationOptions.RunContinuationsAsynchronously);
            remainingCoordinatedReads = 2;
        }

        public bool FailNextAudit { get; set; }

        public IReadOnlyList<LocalSessionRecord> AllSessions
        {
            get { lock (gate) return sessions.Values.ToArray(); }
        }

        public IReadOnlyList<string> AuditActions
        {
            get { lock (gate) return audits.Select(x => x.Action).ToArray(); }
        }

        public Task CreateWithAuditAsync(
            LocalSessionRecord session,
            LocalSessionAuditIntent audit,
            CancellationToken cancellationToken = default)
        {
            lock (gate)
            {
                EnsureAuditCanCommit();
                sessions.Add(session.SessionId, session);
                refreshIndex.Add(session.RefreshTokenHash, session.SessionId);
                audits.Add(audit);
            }
            return Task.CompletedTask;
        }

        public async Task<LocalSessionRecord?> FindByRefreshTokenHashAsync(
            string refreshTokenHash,
            CancellationToken cancellationToken = default)
        {
            LocalSessionRecord? result;
            Task? wait = null;
            lock (gate)
            {
                result = refreshIndex.TryGetValue(refreshTokenHash, out var id) &&
                    sessions.TryGetValue(id, out var session) ? session : null;
                if (remainingCoordinatedReads > 0 && --remainingCoordinatedReads == 0)
                    coordinatedReads!.SetResult();
                else if (remainingCoordinatedReads > 0)
                    wait = coordinatedReads!.Task;
            }
            if (wait is not null)
                await wait.WaitAsync(cancellationToken);
            return result;
        }

        public Task<LocalSessionRecord?> FindBySessionIdAsync(
            Guid sessionId,
            CancellationToken cancellationToken = default)
        {
            lock (gate)
                return Task.FromResult(sessions.GetValueOrDefault(sessionId));
        }

        public Task<LocalRefreshRotationResult> RotateWithAuditAsync(
            LocalRefreshRotationRequest request,
            LocalSessionAuditIntent audit,
            CancellationToken cancellationToken = default)
        {
            lock (gate)
            {
                if (!refreshIndex.TryGetValue(request.PresentedTokenHash, out var id) ||
                    !sessions.TryGetValue(id, out var old) || old.SessionId != request.ExpectedSessionId)
                    return Task.FromResult(new LocalRefreshRotationResult(LocalRefreshRotationStatus.Invalid));
                if (old.RevokedAt.HasValue || old.ReplacedBySessionId.HasValue)
                {
                    EnsureAuditCanCommit();
                    RevokeFamilyCore(old.FamilyId, "REFRESH_REUSE", request.Now);
                    audits.Add(audit with { Action = "SESSION_FAMILY_REVOKED", Reason = "REFRESH_REUSE" });
                    return Task.FromResult(new LocalRefreshRotationResult(
                        LocalRefreshRotationStatus.ReuseDetectedAndFamilyRevoked));
                }
                if (old.RefreshTokenExpiresAt <= request.Now)
                {
                    EnsureAuditCanCommit();
                    RevokeFamilyCore(old.FamilyId, "REFRESH_EXPIRED", request.Now);
                    audits.Add(audit with { Action = "SESSION_FAMILY_REVOKED", Reason = "REFRESH_EXPIRED" });
                    return Task.FromResult(new LocalRefreshRotationResult(LocalRefreshRotationStatus.Expired));
                }
                if (!string.Equals(old.DeviceId, request.Replacement.DeviceId, StringComparison.Ordinal))
                {
                    EnsureAuditCanCommit();
                    RevokeFamilyCore(old.FamilyId, "DEVICE_MISMATCH", request.Now);
                    audits.Add(audit with { Action = "SESSION_FAMILY_REVOKED", Reason = "DEVICE_MISMATCH" });
                    return Task.FromResult(new LocalRefreshRotationResult(LocalRefreshRotationStatus.DeviceMismatch));
                }

                EnsureAuditCanCommit();
                sessions[old.SessionId] = old with
                {
                    RevokedAt = request.Now,
                    RevokeReason = "ROTATED",
                    ReplacedBySessionId = request.Replacement.SessionId
                };
                sessions.Add(request.Replacement.SessionId, request.Replacement);
                refreshIndex.Add(request.Replacement.RefreshTokenHash, request.Replacement.SessionId);
                audits.Add(audit);
                return Task.FromResult(new LocalRefreshRotationResult(
                    LocalRefreshRotationStatus.Rotated, request.Replacement));
            }
        }

        public Task RevokeSessionWithAuditAsync(
            Guid sessionId,
            string reason,
            DateTimeOffset now,
            LocalSessionAuditIntent audit,
            CancellationToken cancellationToken = default)
        {
            lock (gate)
            {
                EnsureAuditCanCommit();
                if (sessions.TryGetValue(sessionId, out var session) && !session.RevokedAt.HasValue)
                    sessions[sessionId] = session with { RevokedAt = now, RevokeReason = reason };
                audits.Add(audit);
            }
            return Task.CompletedTask;
        }

        public Task RevokeFamilyWithAuditAsync(
            Guid familyId,
            string reason,
            DateTimeOffset now,
            LocalSessionAuditIntent audit,
            CancellationToken cancellationToken = default)
        {
            lock (gate)
            {
                EnsureAuditCanCommit();
                RevokeFamilyCore(familyId, reason, now);
                audits.Add(audit);
            }
            return Task.CompletedTask;
        }

        public bool IsFamilyRevoked(Guid sessionId)
        {
            lock (gate)
                return sessions.TryGetValue(sessionId, out var session) &&
                    sessions.Values.Where(x => x.FamilyId == session.FamilyId)
                        .All(x => x.RevokedAt.HasValue);
        }

        public IReadOnlyList<LocalSessionRecord> Family(Guid familyId)
        {
            lock (gate)
                return sessions.Values.Where(x => x.FamilyId == familyId).ToArray();
        }

        private void RevokeFamilyCore(Guid familyId, string reason, DateTimeOffset now)
        {
            foreach (var session in sessions.Values.Where(x => x.FamilyId == familyId).ToArray())
                sessions[session.SessionId] = session with { RevokedAt = now, RevokeReason = reason };
        }

        private void EnsureAuditCanCommit()
        {
            if (!FailNextAudit)
                return;

            FailNextAudit = false;
            throw new InvalidOperationException("Injected audit write failure.");
        }
    }
}
