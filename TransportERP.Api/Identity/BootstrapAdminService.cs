using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Identity;

public sealed record BootstrapAdminOptions(
    string CurrencyCode, string CurrencyNameAr, int CurrencyMinorUnit,
    string CompanyCode, string CompanyNameAr, Guid DefaultCalendarId,
    string BranchCode, string BranchNameAr, string BranchTimezone,
    string AdminUserName, string AdminDisplayName, string AdminPassword)
{
    public static BootstrapAdminOptions FromConfiguration(IConfiguration configuration)
    {
        if (configuration["BootstrapAdmin:AdminPassword"] is not null)
            throw new InvalidOperationException("BOOTSTRAP_PASSWORD_CONFIGURATION_PROVIDER_FORBIDDEN");
        string Required(string key, int max)
        {
            var value = configuration[$"BootstrapAdmin:{key}"]?.Trim();
            if (string.IsNullOrWhiteSpace(value) || value.Length > max)
                throw new InvalidOperationException($"BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:{key}");
            return value;
        }
        var minorRaw = Required("CurrencyMinorUnit", 1);
        if (!int.TryParse(minorRaw, out var minorUnit) || minorUnit is < 0 or > 6)
            throw new InvalidOperationException("BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:CurrencyMinorUnit");
        var password = Environment.GetEnvironmentVariable("TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(password) || password.Length > IdentitySessionService.MaxPasswordLength)
            throw new InvalidOperationException("BOOTSTRAP_SECRET_REQUIRED:TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD");
        if (password.Length < 12) throw new InvalidOperationException("BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:AdminPassword");
        if (!Guid.TryParse(Required("DefaultCalendarId", 36), out var defaultCalendarId) || defaultCalendarId == Guid.Empty)
            throw new InvalidOperationException("BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:DefaultCalendarId");
        var timezone = Required("BranchTimezone", 80);
        try { _ = TimeZoneInfo.FindSystemTimeZoneById(timezone); }
        catch (TimeZoneNotFoundException) { throw new InvalidOperationException("BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:BranchTimezone"); }
        catch (InvalidTimeZoneException) { throw new InvalidOperationException("BOOTSTRAP_CONFIG_REQUIRED_OR_INVALID:BranchTimezone"); }
        var currencyCode = Required("CurrencyCode", 3).ToUpperInvariant();
        var adminUserName = Required("AdminUserName", 100);
        if (currencyCode.Length > 3 || adminUserName.ToUpperInvariant().Length > 100)
            throw new InvalidOperationException("BOOTSTRAP_CONFIG_NORMALIZATION_OVERFLOW");
        return new(
            currencyCode, Required("CurrencyNameAr", 100), minorUnit,
            Required("CompanyCode", 40), Required("CompanyNameAr", 250), defaultCalendarId,
            Required("BranchCode", 40), Required("BranchNameAr", 200), timezone,
            adminUserName, Required("AdminDisplayName", 200), password);
    }
}

public sealed class BootstrapAdminService(
    TransportErpDbContext db,
    IPasswordHasher<User> passwordHasher,
    AuditEventService audit)
{
    public const string MarkerKey = "system.bootstrap.admin.v1";

    public async Task ExecuteAsync(BootstrapAdminOptions options, CancellationToken ct = default)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        await db.Database.ExecuteSqlRawAsync(
            "SELECT pg_advisory_xact_lock(hashtextextended('transporterp.bootstrap.admin.v1', 0))", ct);

        if (await db.GlobalSettings.AnyAsync(x => x.Key == MarkerKey, ct))
            throw new InvalidOperationException("BOOTSTRAP_ALREADY_COMPLETED");
        if (await db.Users.IgnoreQueryFilters().AnyAsync(ct))
            throw new InvalidOperationException("BOOTSTRAP_REFUSED_USERS_EXIST");

        await SystemPermissionCatalog.EnsureAsync(db, allowCreate: true, ct);
        var now = DateTimeOffset.UtcNow;
        var currency = await db.Currencies.SingleOrDefaultAsync(x => x.Code == options.CurrencyCode, ct);
        if (await db.Currencies.AnyAsync(x => x.IsBase && x.Code != options.CurrencyCode, ct))
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:BASE_CURRENCY");
        if (currency is null)
        {
            currency = NewEntity(new Currency
            {
                Code = options.CurrencyCode, NameAr = options.CurrencyNameAr,
                MinorUnit = options.CurrencyMinorUnit, IsBase = true, Status = "ACTIVE"
            }, now);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync(ct);
        }
        else if (currency.NameAr != options.CurrencyNameAr || currency.MinorUnit != options.CurrencyMinorUnit ||
                 !currency.IsBase || currency.Status != "ACTIVE")
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:CURRENCY");

        var company = await db.Companies.SingleOrDefaultAsync(x => x.Code == options.CompanyCode, ct);
        if (company is null)
        {
            company = NewEntity(new Company
            {
                Code = options.CompanyCode, LegalNameAr = options.CompanyNameAr, BaseCurrencyId = currency.Id,
                DefaultCalendarId = options.DefaultCalendarId, Status = "ACTIVE"
            }, now);
            db.Companies.Add(company);
            await db.SaveChangesAsync(ct);
        }
        else if (company.LegalNameAr != options.CompanyNameAr || company.BaseCurrencyId != currency.Id ||
                 company.DefaultCalendarId != options.DefaultCalendarId || company.Status != "ACTIVE")
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:COMPANY");

        var branch = await db.Branches.SingleOrDefaultAsync(
            x => x.CompanyId == company.Id && x.Code == options.BranchCode, ct);
        if (branch is null)
        {
            branch = NewEntity(new Branch
            {
                CompanyId = company.Id, Code = options.BranchCode, NameAr = options.BranchNameAr,
                Timezone = options.BranchTimezone, BranchType = "MAIN", Status = "ACTIVE"
            }, now);
            db.Branches.Add(branch);
            await db.SaveChangesAsync(ct);
        }
        else if (branch.NameAr != options.BranchNameAr || branch.Timezone != options.BranchTimezone ||
                 branch.BranchType != "MAIN" || branch.Status != "ACTIVE")
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:BRANCH");

        var user = NewEntity(new User
        {
            UserName = options.AdminUserName, NormalizedUserName = options.AdminUserName.ToUpperInvariant(),
            DisplayName = options.AdminDisplayName, CompanyId = company.Id, BranchId = branch.Id,
            SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE"
        }, now);
        user.PasswordHash = passwordHasher.HashPassword(user, options.AdminPassword);
        db.Users.Add(user);

        var role = await db.Roles.IgnoreQueryFilters().SingleOrDefaultAsync(
            x => x.Code == "SYSTEM_ADMIN" && x.CompanyId == company.Id, ct);
        var roleWasCreated = role is null;
        if (role is null)
        {
            role = NewEntity(new Role
            {
                Code = "SYSTEM_ADMIN", NameAr = "مدير النظام", Description = "دور الإدارة الأولي للنظام",
                IsSystem = true, CompanyId = company.Id, Status = "ACTIVE"
            }, now);
            db.Roles.Add(role);
        }
        else if (!role.IsSystem || role.Status != "ACTIVE" || role.DeletedAt.HasValue ||
                 role.NameAr != "مدير النظام" || role.Description != "دور الإدارة الأولي للنظام")
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE");

        await db.SaveChangesAsync(ct);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        var permissions = await db.Permissions.Where(x => x.IsSystem && x.Status == "ACTIVE").ToDictionaryAsync(x => x.Code, ct);
        var existingGrants = await db.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync(ct);
        if (existingGrants.Any(x => !SystemPermissionCatalog.Definitions.Any(d =>
                permissions.TryGetValue(d.Code, out var p) && p.Id == x.PermissionId)))
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE_EXTRA_GRANT");
        if (existingGrants.GroupBy(x => x.PermissionId).Any(x => x.Count() != 1))
            throw new InvalidOperationException("BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE_DUPLICATE_GRANT");
        foreach (var definition in SystemPermissionCatalog.Definitions)
        {
            var permission = permissions[definition.Code];
            var existingGrant = existingGrants.SingleOrDefault(x => x.PermissionId == permission.Id);
            if (existingGrant is not null)
            {
                var companyId = definition.ScopeType == "BRANCH" ? company.Id : (Guid?)null;
                var branchId = definition.ScopeType == "BRANCH" ? branch.Id : (Guid?)null;
                if (existingGrant.ScopeType != definition.ScopeType || existingGrant.CompanyId != companyId ||
                    existingGrant.BranchId != branchId)
                    throw new InvalidOperationException($"BOOTSTRAP_REFERENCE_DRIFT:ROLE_PERMISSION:{definition.Code}");
                continue;
            }
            if (!roleWasCreated)
                throw new InvalidOperationException($"BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE_MISSING_GRANT:{definition.Code}");
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id, PermissionId = permission.Id, ScopeType = definition.ScopeType,
                CompanyId = definition.ScopeType == "BRANCH" ? company.Id : null,
                BranchId = definition.ScopeType == "BRANCH" ? branch.Id : null,
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            });
        }
        db.GlobalSettings.Add(NewEntity(new GlobalSetting
        {
            Key = MarkerKey,
            ValueJson = JsonSerializer.Serialize(new { companyId = company.Id, branchId = branch.Id, userId = user.Id }),
            ValueType = "JSON", Version = 1, IsSecret = false, Status = "ACTIVE"
        }, now));
        await db.SaveChangesAsync(ct);
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "BootstrapAdminCreated", "SUCCESS", nameof(User), user.Id, user.Id, company.Id, branch.Id,
            Guid.NewGuid(), Reason: "ONE_TIME_BOOTSTRAP"), ct);
        await transaction.CommitAsync(ct);
    }

    private static T NewEntity<T>(T entity, DateTimeOffset now) where T : P1Entity
    {
        entity.Id = Guid.NewGuid(); entity.CreatedAt = now; entity.UpdatedAt = now;
        entity.RowVersion = Guid.NewGuid().ToByteArray();
        return entity;
    }
}
