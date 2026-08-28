using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class P1SecurityIdentityTests
{
    [Fact]
    public void Refresh_token_validation_is_bounded_and_invalid_partitions_are_constant()
    {
        var valid = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
        Assert.True(IdentitySessionService.IsValidRefreshToken(valid));
        Assert.False(IdentitySessionService.IsValidRefreshToken(Convert.ToBase64String(new byte[31])));
        Assert.False(IdentitySessionService.IsValidRefreshToken("not-base64"));
        Assert.False(IdentitySessionService.IsValidRefreshToken(new string('a', IdentitySessionService.MaxRefreshTokenLength + 1)));

        var malformedPartition = IdentitySessionService.HashRefreshPartition("not-base64");
        var oversizedPartition = IdentitySessionService.HashRefreshPartition(
            new string('a', IdentitySessionService.MaxRefreshTokenLength + 1));
        Assert.Equal(malformedPartition, oversizedPartition);
        Assert.NotEqual(malformedPartition, IdentitySessionService.HashRefreshPartition(valid));
    }

    [Fact]
    public async Task Identity_rate_limit_requires_both_ip_and_account_device_budgets_and_exposes_retry_window()
    {
        await using var limiter = new IdentityRateLimiter(Options.Create(new TransportSecurityOptions
        {
            LoginRateLimitPermitCount = 1,
            RefreshRateLimitPermitCount = 1,
            RateLimitWindowSeconds = 60
        }));

        Assert.True((await limiter.TryAcquireLoginAsync("192.0.2.1", "USER-A", "DEVICE-A", default)).IsAcquired);

        var ipLimited = await limiter.TryAcquireLoginAsync("192.0.2.1", "USER-B", "DEVICE-B", default);
        Assert.False(ipLimited.IsAcquired);
        Assert.True(ipLimited.RetryAfter > TimeSpan.Zero);

        var accountDeviceLimited = await limiter.TryAcquireLoginAsync("192.0.2.2", "USER-A", "DEVICE-A", default);
        Assert.False(accountDeviceLimited.IsAcquired);
        Assert.True(accountDeviceLimited.RetryAfter > TimeSpan.Zero);
    }

    [Fact]
    public async Task Refresh_rate_limit_uses_independent_ip_and_token_hash_device_budgets()
    {
        await using var limiter = new IdentityRateLimiter(Options.Create(new TransportSecurityOptions
        {
            LoginRateLimitPermitCount = 1, RefreshRateLimitPermitCount = 1, RateLimitWindowSeconds = 60
        }));
        Assert.True((await limiter.TryAcquireRefreshAsync("192.0.2.10", "HASH-A", "DEVICE-A", default)).IsAcquired);

        var ipLimited = await limiter.TryAcquireRefreshAsync("192.0.2.10", "HASH-B", "DEVICE-B", default);
        Assert.False(ipLimited.IsAcquired);
        Assert.True(ipLimited.RetryAfter > TimeSpan.Zero);

        var replayPartitionLimited = await limiter.TryAcquireRefreshAsync("192.0.2.11", "HASH-A", "DEVICE-A", default);
        Assert.False(replayPartitionLimited.IsAcquired);
        Assert.True(replayPartitionLimited.RetryAfter > TimeSpan.Zero);

        Assert.True((await limiter.TryAcquireRefreshAsync("192.0.2.12", "HASH-B", "DEVICE-A", default)).IsAcquired);
    }

    [Fact]
    public void Local_security_defaults_are_valid_and_modes_cannot_be_hybrid()
    {
        var validator = new TransportSecurityOptionsValidator();
        var valid = validator.Validate(null, new TransportSecurityOptions
        {
            Mode = TransportAuthMode.LocalSessions,
            Issuer = "issuer", Audience = "audience",
            SigningKeyId = "current",
            SigningKey = "01234567890123456789012345678901",
            AccessTokenMinutes = 15, RefreshTokenDays = 30, MaxFailures = 5, LockoutMinutes = 15
        });
        Assert.True(valid.Succeeded);

        var hybrid = validator.Validate(null, new TransportSecurityOptions
        {
            Mode = TransportAuthMode.LocalSessions, Authority = "https://identity.example",
            Issuer = "issuer", Audience = "audience", SigningKeyId = "current",
            SigningKey = "01234567890123456789012345678901"
        });
        Assert.True(hybrid.Failed);
    }

    [Fact]
    public void Signing_key_ring_requires_distinct_valid_key_ids_and_is_forbidden_in_external_mode()
    {
        var validator = new TransportSecurityOptionsValidator();
        var duplicateCurrent = validator.Validate(null, new TransportSecurityOptions
        {
            Mode = TransportAuthMode.LocalSessions, Issuer = "issuer", Audience = "audience",
            SigningKeyId = "current", SigningKey = "01234567890123456789012345678901",
            PreviousSigningKeys = new() { ["current"] = "abcdefghijklmnopqrstuvwxyz123456" }
        });
        Assert.True(duplicateCurrent.Failed);

        var externalHybrid = validator.Validate(null, new TransportSecurityOptions
        {
            Mode = TransportAuthMode.ExternalAuthority, Authority = "https://identity.example", Audience = "audience",
            SigningKeyId = "local-key", PreviousSigningKeys = new() { ["old"] = "abcdefghijklmnopqrstuvwxyz123456" }
        });
        Assert.True(externalHybrid.Failed);

        var multiNodeWithoutDistributedLimiter = validator.Validate(null, new TransportSecurityOptions
        {
            Mode = TransportAuthMode.LocalSessions, Issuer = "issuer", Audience = "audience",
            SigningKeyId = "current", SigningKey = "01234567890123456789012345678901",
            RateLimiterMode = "SingleNode", ApplicationInstanceCount = 2
        });
        Assert.True(multiNodeWithoutDistributedLimiter.Failed);
    }

    [Fact]
    public async Task Explicit_deny_wins_over_role_grant_and_user_allow()
    {
        await using var db = CreateDb();
        var seed = SeedBase(db);
        var permission = Entity<Permission>();
        permission.Code = "waybill.test"; permission.NameAr = "اختبار"; permission.Resource = "waybill";
        permission.Action = "test"; permission.ScopeType = "COMPANY"; permission.Status = "ACTIVE";
        var role = Entity<Role>(); role.Code = "TEST"; role.NameAr = "اختبار"; role.CompanyId = seed.Company.Id; role.Status = "ACTIVE";
        db.Permissions.Add(permission); db.Roles.Add(role);
        db.UserRoles.Add(new UserRole { UserId = seed.User.Id, RoleId = role.Id, CompanyId = seed.Company.Id,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray() });
        db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id, ScopeType = "COMPANY",
            CompanyId = seed.Company.Id, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray() });
        await db.SaveChangesAsync();

        var resolver = new EffectivePermissionResolver(db);
        Assert.True(await resolver.HasPermissionAsync(seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));

        db.UserPermissionOverrides.Add(new UserPermissionOverride { UserId = seed.User.Id, PermissionId = permission.Id,
            IsAllowed = false, CompanyId = seed.Company.Id, Reason = "security-test", CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray() });
        await db.SaveChangesAsync();
        Assert.False(await resolver.HasPermissionAsync(seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));
    }

    [Theory]
    [InlineData("PLATFORM")]
    [InlineData("COMPANY")]
    [InlineData("BRANCH")]
    public async Task User_allow_override_must_match_the_permission_scope_shape(string scopeType)
    {
        await using var db = CreateDb();
        var seed = SeedBase(db);
        var permission = Entity<Permission>();
        permission.Code = $"override.{scopeType.ToLowerInvariant()}"; permission.NameAr = "اختبار";
        permission.Resource = "override"; permission.Action = "test"; permission.ScopeType = scopeType;
        permission.Status = "ACTIVE";
        db.Permissions.Add(permission);
        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = seed.User.Id, PermissionId = permission.Id, IsAllowed = true,
            CompanyId = scopeType is "COMPANY" or "BRANCH" ? seed.Company.Id : null,
            BranchId = scopeType == "BRANCH" ? seed.Branch.Id : null,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();

        Assert.True(await new EffectivePermissionResolver(db).HasPermissionAsync(
            seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));
    }

    [Fact]
    public async Task User_allow_override_cannot_widen_branch_permission_and_malformed_scope_fails_closed()
    {
        await using var db = CreateDb();
        var seed = SeedBase(db);
        var permission = Entity<Permission>();
        permission.Code = "override.branch.widen"; permission.NameAr = "اختبار"; permission.Resource = "override";
        permission.Action = "test"; permission.ScopeType = "BRANCH"; permission.Status = "ACTIVE";
        db.Permissions.Add(permission);
        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = seed.User.Id, PermissionId = permission.Id, IsAllowed = true,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        var resolver = new EffectivePermissionResolver(db);
        Assert.False(await resolver.HasPermissionAsync(seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));

        var entity = await db.UserPermissionOverrides.SingleAsync();
        entity.CompanyId = null;
        entity.BranchId = seed.Branch.Id;
        await db.SaveChangesAsync();
        Assert.False(await resolver.HasPermissionAsync(seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));
    }

    [Theory]
    [InlineData("PLATFORM")]
    [InlineData("COMPANY")]
    [InlineData("BRANCH")]
    public async Task Applicable_deny_at_platform_company_or_branch_level_blocks_branch_role_grant(string denyScope)
    {
        await using var db = CreateDb();
        var seed = SeedBase(db);
        var permission = Entity<Permission>();
        permission.Code = $"deny.{denyScope.ToLowerInvariant()}"; permission.NameAr = "اختبار";
        permission.Resource = "deny"; permission.Action = "test"; permission.ScopeType = "BRANCH";
        permission.Status = "ACTIVE";
        var role = Entity<Role>(); role.Code = $"DENY-{denyScope}"; role.NameAr = "اختبار";
        role.CompanyId = seed.Company.Id; role.Status = "ACTIVE";
        db.AddRange(permission, role);
        db.UserRoles.Add(new UserRole { UserId = seed.User.Id, RoleId = role.Id, CompanyId = seed.Company.Id,
            BranchId = seed.Branch.Id, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray() });
        db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id, ScopeType = "BRANCH",
            CompanyId = seed.Company.Id, BranchId = seed.Branch.Id, CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray() });
        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = seed.User.Id, PermissionId = permission.Id, IsAllowed = false,
            CompanyId = denyScope is "COMPANY" or "BRANCH" ? seed.Company.Id : null,
            BranchId = denyScope == "BRANCH" ? seed.Branch.Id : null,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();

        Assert.False(await new EffectivePermissionResolver(db).HasPermissionAsync(
            seed.User.Id, seed.Company.Id, seed.Branch.Id, permission.Code));
    }

    [Fact]
    public void Auth_session_model_uses_composite_branch_company_fk_and_unique_refresh_hash()
    {
        using var db = CreateDb();
        var entity = db.Model.FindEntityType(typeof(AuthSession))!;
        Assert.Contains(entity.GetIndexes(), x => x.IsUnique && x.Properties.Select(p => p.Name).SequenceEqual(new[] { "RefreshTokenHash" }));
        Assert.Contains(entity.GetForeignKeys(), x =>
            x.PrincipalEntityType.ClrType == typeof(Branch) &&
            x.Properties.Select(p => p.Name).SequenceEqual(new[] { "BranchId", "CompanyId" }));
    }

    private static TransportErpDbContext CreateDb() => new(new DbContextOptionsBuilder<TransportErpDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    private static (Company Company, Branch Branch, User User) SeedBase(TransportErpDbContext db)
    {
        var currency = Entity<Currency>(); currency.Code = "TST"; currency.NameAr = "اختبار"; currency.Status = "ACTIVE";
        var company = Entity<Company>(); company.Code = "SEC"; company.LegalNameAr = "شركة"; company.BaseCurrencyId = currency.Id;
        company.DefaultCalendarId = Guid.NewGuid(); company.Status = "ACTIVE";
        var branch = Entity<Branch>(); branch.CompanyId = company.Id; branch.Code = "MAIN"; branch.NameAr = "فرع";
        branch.Timezone = "Asia/Riyadh"; branch.Status = "ACTIVE";
        var user = Entity<User>(); user.UserName = "security"; user.NormalizedUserName = "SECURITY";
        user.DisplayName = "Security"; user.PasswordHash = "not-used"; user.SecurityStamp = Guid.NewGuid().ToString("N"); user.AuthVersion = 1;
        user.CompanyId = company.Id; user.BranchId = branch.Id; user.Status = "ACTIVE";
        db.AddRange(currency, company, branch, user); db.SaveChanges();
        return (company, branch, user);
    }

    private static T Entity<T>() where T : P1Entity, new()
    {
        var now = DateTimeOffset.UtcNow;
        return new T { Id = Guid.NewGuid(), CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() };
    }
}
