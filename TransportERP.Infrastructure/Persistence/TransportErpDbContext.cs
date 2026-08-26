using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed class TransportErpDbContext(DbContextOptions<TransportErpDbContext> options) : DbContext(options)
{
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<User> Users => Set<User>();
    public DbSet<AuthSession> AuthSessions => Set<AuthSession>();
    public DbSet<RegisteredDevice> RegisteredDevices => Set<RegisteredDevice>();
    public DbSet<RegisteredDeviceAssignment> RegisteredDeviceAssignments => Set<RegisteredDeviceAssignment>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserPermissionOverride> UserPermissionOverrides => Set<UserPermissionOverride>();
    public DbSet<GlobalSetting> GlobalSettings => Set<GlobalSetting>();
    public DbSet<CompanySetting> CompanySettings => Set<CompanySetting>();
    public DbSet<BranchSetting> BranchSettings => Set<BranchSetting>();
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<FinancialDimension> FinancialDimensions => Set<FinancialDimension>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
    public DbSet<ReceiptVoucher> ReceiptVouchers => Set<ReceiptVoucher>();
    public DbSet<PaymentVoucher> PaymentVouchers => Set<PaymentVoucher>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();
    public DbSet<AuditStreamHead> AuditStreamHeads => Set<AuditStreamHead>();
    public DbSet<SyncOperation> SyncOperations => Set<SyncOperation>();
    public DbSet<SyncProofNonce> SyncProofNonces => Set<SyncProofNonce>();
    public DbSet<SyncProofReplay> SyncProofReplays => Set<SyncProofReplay>();
    public DbSet<ConflictCase> ConflictCases => Set<ConflictCase>();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        RejectAuditMutation();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        RejectAuditMutation();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void RejectAuditMutation()
    {
        var illegal = ChangeTracker.Entries<AuditEvent>()
            .Where(x => x.State is EntityState.Modified or EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .ToArray();
        if (illegal.Length > 0)
            throw new InvalidOperationException($"AuditEvent is append-only; mutation denied: {string.Join(',', illegal)}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("transport_erp");
        ConfigureCommon(modelBuilder);
        ConfigureReferenceAndOrganization(modelBuilder);
        ConfigureIdentityAndSettings(modelBuilder);
        ConfigureAccounting(modelBuilder);
        ConfigureAuditAndSync(modelBuilder);
    }

    private static void ConfigureCommon(ModelBuilder mb)
    {
        foreach (var type in new[] {
            typeof(Currency), typeof(Company), typeof(Branch), typeof(User), typeof(AuthSession), typeof(RegisteredDevice),
            typeof(RegisteredDeviceAssignment), typeof(Role), typeof(Permission),
            typeof(GlobalSetting), typeof(CompanySetting), typeof(BranchSetting), typeof(ChartOfAccount),
            typeof(FiscalPeriod), typeof(FinancialDimension), typeof(JournalEntry), typeof(ReceiptVoucher),
            typeof(PaymentVoucher), typeof(SyncOperation), typeof(ConflictCase) })
        {
            var entity = mb.Entity(type);
            entity.Property<byte[]>("RowVersion").HasColumnType("bytea").IsConcurrencyToken();
            entity.Property<DateTimeOffset>("CreatedAt").HasColumnType("timestamptz");
            entity.Property<DateTimeOffset>("UpdatedAt").HasColumnType("timestamptz");
        }
    }

    private static void ConfigureReferenceAndOrganization(ModelBuilder mb)
    {
        var currency = mb.Entity<Currency>();
        currency.ToTable("currencies", t =>
        {
            t.HasCheckConstraint("ck_currencies_minor_unit", "\"MinorUnit\" BETWEEN 0 AND 6");
            t.HasCheckConstraint("ck_currencies_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
        });
        currency.HasKey(x => x.Id);
        currency.Property(x => x.Code).HasMaxLength(3).IsRequired();
        currency.Property(x => x.NameAr).HasMaxLength(100).IsRequired();
        currency.Property(x => x.NameEn).HasMaxLength(100);
        currency.Property(x => x.Status).HasMaxLength(20).IsRequired();
        currency.HasIndex(x => x.Code).IsUnique();
        currency.HasIndex(x => x.Status);

        var company = mb.Entity<Company>();
        company.ToTable("companies", t => t.HasCheckConstraint("ck_companies_status", "\"Status\" IN ('DRAFT','ACTIVE','SUSPENDED','CLOSED')"));
        company.HasKey(x => x.Id);
        company.Property(x => x.Code).HasMaxLength(40).IsRequired();
        company.Property(x => x.LegalNameAr).HasMaxLength(250).IsRequired();
        company.Property(x => x.LegalNameEn).HasMaxLength(250);
        company.Property(x => x.TaxIdentifier).HasMaxLength(80);
        company.Property(x => x.Status).HasMaxLength(20).IsRequired();
        company.HasIndex(x => x.Code).IsUnique();
        company.HasIndex(x => x.TaxIdentifier).IsUnique().HasFilter("\"TaxIdentifier\" IS NOT NULL");
        company.HasIndex(x => x.Status);
        company.HasOne(x => x.BaseCurrency).WithMany().HasForeignKey(x => x.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict);

        var branch = mb.Entity<Branch>();
        branch.ToTable("branches", t => t.HasCheckConstraint("ck_branches_status", "\"Status\" IN ('DRAFT','ACTIVE','INACTIVE')"));
        branch.HasKey(x => x.Id);
        branch.HasAlternateKey(x => new { x.Id, x.CompanyId });
        branch.Property(x => x.Code).HasMaxLength(40).IsRequired();
        branch.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        branch.Property(x => x.NameEn).HasMaxLength(200);
        branch.Property(x => x.BranchType).HasMaxLength(60);
        branch.Property(x => x.Address).HasMaxLength(500);
        branch.Property(x => x.Timezone).HasMaxLength(80).IsRequired();
        branch.Property(x => x.Status).HasMaxLength(20).IsRequired();
        branch.HasOne(x => x.Company).WithMany(x => x.Branches).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        branch.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        branch.HasIndex(x => new { x.CompanyId, x.Status });
    }

    private static void ConfigureIdentityAndSettings(ModelBuilder mb)
    {
        var user = mb.Entity<User>();
        user.ToTable("users", t =>
        {
            t.HasCheckConstraint("ck_users_status", "\"Status\" IN ('ACTIVE','LOCKED','DISABLED')");
            t.HasCheckConstraint("ck_users_security_stamp", "length(\"SecurityStamp\") >= 32");
            t.HasCheckConstraint("ck_users_auth_version", "\"AuthVersion\" >= 1");
            t.HasCheckConstraint("ck_users_branch_company", "\"BranchId\" IS NULL OR \"CompanyId\" IS NOT NULL");
        });
        user.HasKey(x => x.Id);
        user.Property(x => x.UserName).HasMaxLength(100).IsRequired();
        user.Property(x => x.NormalizedUserName).HasMaxLength(100).IsRequired();
        user.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        user.Property(x => x.Email).HasMaxLength(320);
        user.Property(x => x.NormalizedEmail).HasMaxLength(320);
        user.Property(x => x.Phone).HasMaxLength(30);
        user.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
        user.Property(x => x.SecurityStamp).HasMaxLength(64).IsRequired();
        user.Property(x => x.AccessFailedCount).HasDefaultValue(0);
        user.Property(x => x.AuthVersion).HasDefaultValue(1);
        user.Property(x => x.Status).HasMaxLength(20).IsRequired();
        user.HasIndex(x => new { x.NormalizedUserName, x.CompanyId }).IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL")
            .HasAnnotation("Npgsql:NullsDistinct", false);
        user.HasIndex(x => new { x.NormalizedEmail, x.CompanyId }).IsUnique()
            .HasFilter("\"NormalizedEmail\" IS NOT NULL AND \"DeletedAt\" IS NULL")
            .HasAnnotation("Npgsql:NullsDistinct", false);
        user.HasIndex(x => new { x.BranchId, x.Status });
        user.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        user.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        user.HasQueryFilter(x => x.DeletedAt == null);

        var session = mb.Entity<AuthSession>();
        session.ToTable("auth_sessions", t =>
        {
            t.HasCheckConstraint("ck_auth_sessions_mode", "\"Mode\" IN ('LOCAL')");
            t.HasCheckConstraint("ck_auth_sessions_expiry", "\"AccessTokenExpiresAt\" <= \"RefreshTokenExpiresAt\"");
            t.HasCheckConstraint("ck_auth_sessions_security_stamp", "length(\"SecurityStampAtIssue\") >= 32");
            t.HasCheckConstraint("ck_auth_sessions_auth_version", "\"AuthVersionAtIssue\" >= 1");
            t.HasCheckConstraint("ck_auth_sessions_registered_device_binding",
                "(\"RegisteredDeviceId\" IS NULL AND \"DeviceCredentialVersion\" IS NULL) OR " +
                "(\"RegisteredDeviceId\" IS NOT NULL AND \"DeviceCredentialVersion\" >= 1 AND \"BranchId\" IS NOT NULL)");
        });
        session.HasKey(x => x.Id);
        session.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        session.Property(x => x.Mode).HasMaxLength(20).IsRequired();
        session.Property(x => x.SecurityStampAtIssue).HasMaxLength(64).IsRequired();
        session.Property(x => x.RefreshTokenHash).HasMaxLength(64).IsRequired();
        session.Property(x => x.RevokeReason).HasMaxLength(200);
        session.HasIndex(x => x.RefreshTokenHash).IsUnique();
        session.HasIndex(x => new { x.UserId, x.RevokedAt, x.RefreshTokenExpiresAt });
        session.HasIndex(x => x.RefreshTokenFamilyId);
        session.HasIndex(x => new { x.CompanyId, x.BranchId });
        session.HasIndex(x => x.DeviceId);
        session.HasIndex(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId });
        session.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        session.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        session.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        session.HasOne<AuthSession>().WithMany().HasForeignKey(x => x.ReplacedBySessionId).OnDelete(DeleteBehavior.Restrict);
        session.HasOne<RegisteredDevice>().WithMany()
            .HasForeignKey(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId, x.DeviceId }).OnDelete(DeleteBehavior.Restrict);

        var registeredDevice = mb.Entity<RegisteredDevice>();
        registeredDevice.ToTable("registered_devices", t =>
        {
            t.HasCheckConstraint("ck_registered_devices_status",
                "\"Status\" IN ('PENDING','ACTIVE','SUSPENDED','REVOKED','EXPIRED')");
            t.HasCheckConstraint("ck_registered_devices_credential_version", "\"CredentialVersion\" >= 1");
            t.HasCheckConstraint("ck_registered_devices_credential_hash", "length(\"CredentialHash\") = 64");
        });
        registeredDevice.HasKey(x => x.Id);
        registeredDevice.HasAlternateKey(x => new { x.Id, x.CompanyId });
        registeredDevice.HasAlternateKey(x => new { x.Id, x.CompanyId, x.DeviceId });
        registeredDevice.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        registeredDevice.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        registeredDevice.Property(x => x.Platform).HasMaxLength(40).IsRequired();
        registeredDevice.Property(x => x.AppVersion).HasMaxLength(40).IsRequired();
        registeredDevice.Property(x => x.DeviceModel).HasMaxLength(120);
        registeredDevice.Property(x => x.OsVersion).HasMaxLength(80);
        registeredDevice.Property(x => x.RegistrationRequestId).HasMaxLength(120).IsRequired();
        registeredDevice.Property(x => x.CredentialHash).HasMaxLength(64).IsRequired();
        registeredDevice.Property(x => x.Status).HasMaxLength(20).IsRequired();
        registeredDevice.HasIndex(x => new { x.CompanyId, x.DeviceId }).IsUnique();
        registeredDevice.HasIndex(x => new { x.CompanyId, x.RegistrationRequestId }).IsUnique();
        registeredDevice.HasIndex(x => new { x.CompanyId, x.Status });
        registeredDevice.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        registeredDevice.HasOne<User>().WithMany().HasForeignKey(x => x.RegisteredByUserId).OnDelete(DeleteBehavior.Restrict);
        registeredDevice.HasOne<User>().WithMany().HasForeignKey(x => x.ApprovedByUserId).OnDelete(DeleteBehavior.Restrict);

        var deviceAssignment = mb.Entity<RegisteredDeviceAssignment>();
        deviceAssignment.ToTable("registered_device_assignments", t =>
            t.HasCheckConstraint("ck_registered_device_assignments_status", "\"Status\" IN ('ACTIVE','REVOKED')"));
        deviceAssignment.HasKey(x => x.Id);
        deviceAssignment.HasAlternateKey(x => new { x.Id, x.RegisteredDeviceId, x.CompanyId, x.UserId, x.BranchId })
            .HasName("ux_device_assignment_proof_scope");
        deviceAssignment.Property(x => x.Status).HasMaxLength(20).IsRequired();
        deviceAssignment.HasIndex(x => new { x.RegisteredDeviceId, x.CompanyId });
        deviceAssignment.HasIndex(x => new { x.UserId, x.CompanyId, x.BranchId, x.Status });
        deviceAssignment.HasIndex(x => new { x.RegisteredDeviceId, x.UserId, x.BranchId })
            .IsUnique().HasFilter("\"Status\" = 'ACTIVE'")
            .HasDatabaseName("IX_registered_device_assignments_active");
        deviceAssignment.HasOne<RegisteredDevice>().WithMany()
            .HasForeignKey(x => new { x.RegisteredDeviceId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        deviceAssignment.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        deviceAssignment.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        deviceAssignment.HasOne<User>().WithMany().HasForeignKey(x => x.AssignedByUserId).OnDelete(DeleteBehavior.Restrict);
        deviceAssignment.HasOne<User>().WithMany().HasForeignKey(x => x.RemovedByUserId).OnDelete(DeleteBehavior.Restrict);

        var role = mb.Entity<Role>();
        role.ToTable("roles", t => t.HasCheckConstraint("ck_roles_status", "\"Status\" IN ('ACTIVE','INACTIVE')"));
        role.HasKey(x => x.Id);
        role.Property(x => x.Code).HasMaxLength(80).IsRequired();
        role.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        role.Property(x => x.NameEn).HasMaxLength(200);
        role.Property(x => x.Description).HasMaxLength(500);
        role.Property(x => x.Status).HasMaxLength(20).IsRequired();
        role.HasIndex(x => new { x.Code, x.CompanyId }).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        role.HasIndex(x => new { x.CompanyId, x.Status });
        role.HasQueryFilter(x => x.DeletedAt == null);

        var permission = mb.Entity<Permission>();
        permission.ToTable("permissions", t => t.HasCheckConstraint("ck_permissions_scope", "\"ScopeType\" IN ('PLATFORM','COMPANY','BRANCH')"));
        permission.HasKey(x => x.Id);
        permission.Property(x => x.Code).HasMaxLength(120).IsRequired();
        permission.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        permission.Property(x => x.Resource).HasMaxLength(120).IsRequired();
        permission.Property(x => x.Action).HasMaxLength(80).IsRequired();
        permission.Property(x => x.ScopeType).HasMaxLength(20).IsRequired();
        permission.Property(x => x.Status).HasMaxLength(20).IsRequired();
        permission.HasIndex(x => x.Code).IsUnique();
        permission.HasIndex(x => new { x.Resource, x.Action });
        permission.HasQueryFilter(x => x.DeletedAt == null);

        var rolePermission = mb.Entity<RolePermission>();
        rolePermission.ToTable("role_permissions", t => t.HasCheckConstraint("ck_role_permissions_scope_fields",
            "(\"ScopeType\" = 'PLATFORM' AND \"CompanyId\" IS NULL AND \"BranchId\" IS NULL) OR " +
            "(\"ScopeType\" = 'COMPANY' AND \"CompanyId\" IS NOT NULL AND \"BranchId\" IS NULL) OR " +
            "(\"ScopeType\" = 'BRANCH' AND \"CompanyId\" IS NOT NULL AND \"BranchId\" IS NOT NULL)"));
        rolePermission.HasKey(x => new { x.RoleId, x.PermissionId, x.ScopeType });
        rolePermission.Property(x => x.ScopeType).HasMaxLength(20).IsRequired();
        rolePermission.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        rolePermission.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        rolePermission.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);

        var userRole = mb.Entity<UserRole>();
        userRole.ToTable("user_roles", t => t.HasCheckConstraint("ck_user_roles_scope_fields",
            "\"BranchId\" IS NULL OR \"CompanyId\" IS NOT NULL"));
        userRole.HasKey(x => new { x.UserId, x.RoleId });
        userRole.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        userRole.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        userRole.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);

        var overrideEntity = mb.Entity<UserPermissionOverride>();
        overrideEntity.ToTable("user_permission_overrides", t => t.HasCheckConstraint("ck_user_permission_overrides_scope_fields",
            "\"BranchId\" IS NULL OR \"CompanyId\" IS NOT NULL"));
        overrideEntity.HasKey(x => new { x.UserId, x.PermissionId });
        overrideEntity.Property(x => x.Reason).HasMaxLength(500);
        overrideEntity.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        overrideEntity.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        overrideEntity.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);

        var global = mb.Entity<GlobalSetting>();
        global.ToTable("global_settings", t => t.HasCheckConstraint("ck_global_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')"));
        global.HasKey(x => x.Id);
        global.Property(x => x.Key).HasMaxLength(160).IsRequired();
        global.Property(x => x.ValueJson).HasColumnType("text").IsRequired();
        global.Property(x => x.ValueType).HasMaxLength(40).IsRequired();
        global.Property(x => x.Status).HasMaxLength(20).IsRequired();
        global.HasIndex(x => x.Key).IsUnique();
        global.HasIndex(x => x.Status);

        var companySetting = mb.Entity<CompanySetting>();
        companySetting.ToTable("company_settings", t => t.HasCheckConstraint("ck_company_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')"));
        companySetting.HasKey(x => x.Id);
        companySetting.Property(x => x.Key).HasMaxLength(160).IsRequired();
        companySetting.Property(x => x.ValueJson).HasColumnType("text").IsRequired();
        companySetting.Property(x => x.ValueType).HasMaxLength(40).IsRequired();
        companySetting.Property(x => x.Status).HasMaxLength(20).IsRequired();
        companySetting.HasIndex(x => new { x.CompanyId, x.Key }).IsUnique();
        companySetting.HasIndex(x => new { x.CompanyId, x.Status });
        companySetting.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);

        var branchSetting = mb.Entity<BranchSetting>();
        branchSetting.ToTable("branch_settings", t => t.HasCheckConstraint("ck_branch_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')"));
        branchSetting.HasKey(x => x.Id);
        branchSetting.Property(x => x.Key).HasMaxLength(160).IsRequired();
        branchSetting.Property(x => x.ValueJson).HasColumnType("text").IsRequired();
        branchSetting.Property(x => x.ValueType).HasMaxLength(40).IsRequired();
        branchSetting.Property(x => x.Status).HasMaxLength(20).IsRequired();
        branchSetting.HasIndex(x => new { x.BranchId, x.Key }).IsUnique();
        branchSetting.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        branchSetting.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        branchSetting.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAccounting(ModelBuilder mb)
    {
        var coa = mb.Entity<ChartOfAccount>();
        coa.ToTable("chart_of_accounts", t => t.HasCheckConstraint("ck_chart_accounts_type", "\"AccountType\" IN ('ASSET','LIABILITY','EQUITY','REVENUE','EXPENSE')"));
        coa.HasKey(x => x.Id);
        coa.Property(x => x.Code).HasMaxLength(60).IsRequired();
        coa.Property(x => x.NameAr).HasMaxLength(250).IsRequired();
        coa.Property(x => x.NameEn).HasMaxLength(250);
        coa.Property(x => x.AccountType).HasMaxLength(20).IsRequired();
        coa.Property(x => x.Status).HasMaxLength(20).IsRequired();
        coa.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        coa.HasIndex(x => new { x.CompanyId, x.ParentId });
        coa.HasIndex(x => new { x.CompanyId, x.AccountType, x.Status });
        coa.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        coa.HasOne<ChartOfAccount>().WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        coa.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        coa.HasQueryFilter(x => x.DeletedAt == null);

        var period = mb.Entity<FiscalPeriod>();
        period.ToTable("fiscal_periods", t =>
        {
            t.HasCheckConstraint("ck_fiscal_periods_status", "\"Status\" IN ('OPEN','SOFT_CLOSED','CLOSED')");
            t.HasCheckConstraint("ck_fiscal_periods_range", "\"EndDate\" >= \"StartDate\"");
        });
        period.HasKey(x => x.Id);
        period.Property(x => x.Code).HasMaxLength(40).IsRequired();
        period.Property(x => x.Status).HasMaxLength(20).IsRequired();
        period.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        period.HasIndex(x => new { x.CompanyId, x.StartDate, x.EndDate }).IsUnique();
        period.HasIndex(x => new { x.CompanyId, x.Status });
        period.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        period.HasOne<User>().WithMany().HasForeignKey(x => x.ClosedBy).OnDelete(DeleteBehavior.Restrict);

        var dimension = mb.Entity<FinancialDimension>();
        dimension.ToTable("financial_dimensions", t => t.HasCheckConstraint("ck_financial_dimensions_dates", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\""));
        dimension.HasKey(x => x.Id);
        dimension.Property(x => x.DimensionCode).HasMaxLength(60).IsRequired();
        dimension.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        dimension.Property(x => x.ValueCode).HasMaxLength(80).IsRequired();
        dimension.Property(x => x.ValueNameAr).HasMaxLength(200).IsRequired();
        dimension.Property(x => x.Status).HasMaxLength(20).IsRequired();
        dimension.HasIndex(x => new { x.CompanyId, x.DimensionCode, x.ValueCode }).IsUnique();
        dimension.HasIndex(x => new { x.CompanyId, x.ParentId, x.Status });
        dimension.HasIndex(x => new { x.CompanyId, x.ValidFrom, x.ValidTo });
        dimension.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        dimension.HasOne<FinancialDimension>().WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);

        var journal = mb.Entity<JournalEntry>();
        journal.ToTable("journal_entries", t =>
        {
            t.HasCheckConstraint("ck_journal_entries_status", "\"Status\" IN ('DRAFT','CHECKED','APPROVED','POSTED','REVERSED')");
            t.HasCheckConstraint("ck_journal_entries_amounts", "\"TotalDebit\" >= 0 AND \"TotalCredit\" >= 0");
        });
        journal.HasKey(x => x.Id);
        journal.Property(x => x.DocumentNo).HasMaxLength(60).IsRequired();
        journal.Property(x => x.Description).HasMaxLength(500);
        journal.Property(x => x.Status).HasMaxLength(20).IsRequired();
        journal.Property(x => x.SourceType).HasMaxLength(80).IsRequired();
        journal.Property(x => x.TotalDebit).HasPrecision(19, 4);
        journal.Property(x => x.TotalCredit).HasPrecision(19, 4);
        journal.Property(x => x.ExchangeRate).HasPrecision(19, 8);
        journal.HasIndex(x => new { x.CompanyId, x.DocumentNo }).IsUnique();
        journal.HasIndex(x => new { x.CompanyId, x.FiscalPeriodId, x.Status });
        journal.HasIndex(x => new { x.SourceType, x.SourceId });
        journal.HasIndex(x => new { x.BranchId, x.EntryDate });
        journal.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        journal.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        journal.HasOne<FiscalPeriod>().WithMany().HasForeignKey(x => x.FiscalPeriodId).OnDelete(DeleteBehavior.Restrict);
        journal.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        journal.HasOne<JournalEntry>().WithMany().HasForeignKey(x => x.ReversalOfId).OnDelete(DeleteBehavior.Restrict);

        var line = mb.Entity<JournalEntryLine>();
        line.ToTable("journal_entry_lines", t => t.HasCheckConstraint("ck_journal_lines_amounts", "\"Debit\" >= 0 AND \"Credit\" >= 0 AND (\"Debit\" > 0 OR \"Credit\" > 0) AND NOT (\"Debit\" > 0 AND \"Credit\" > 0)"));
        line.HasKey(x => new { x.JournalEntryId, x.LineNo });
        line.Property(x => x.Description).HasMaxLength(500);
        line.Property(x => x.Debit).HasPrecision(19, 4);
        line.Property(x => x.Credit).HasPrecision(19, 4);
        line.Property(x => x.ForeignAmount).HasPrecision(19, 4);
        line.HasOne<JournalEntry>().WithMany(x => x.Lines).HasForeignKey(x => x.JournalEntryId).OnDelete(DeleteBehavior.Cascade);
        line.HasOne<ChartOfAccount>().WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<FinancialDimension>().WithMany().HasForeignKey(x => x.FinancialDimensionId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);

        ConfigureVoucher(mb.Entity<ReceiptVoucher>(), "receipt_vouchers", "PayerName", "CollectedBy", "ck_receipts_status");
        ConfigureVoucher(mb.Entity<PaymentVoucher>(), "payment_vouchers", "PayeeName", "PaidBy", "ck_payments_status");
    }

    private static void ConfigureVoucher<TEntity>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity, string table, string partyProperty, string actorProperty, string statusConstraint) where TEntity : P1Entity, IP1Voucher
    {
        entity.ToTable(table, t =>
        {
            t.HasCheckConstraint(statusConstraint, "\"Status\" IN ('DRAFT','APPROVED','POSTED','CANCELLED')");
            t.HasCheckConstraint($"ck_{table}_amount", "\"Amount\" > 0");
        });
        entity.HasKey(x => x.Id);
        entity.Property<string>(partyProperty).HasMaxLength(250).IsRequired();
        entity.Property(x => x.VoucherNo).HasMaxLength(60).IsRequired();
        entity.Property(x => x.ReferenceType).HasMaxLength(60).IsRequired();
        entity.Property(x => x.PaymentMethodCode).HasMaxLength(40).IsRequired();
        entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
        entity.Property(x => x.Notes).HasMaxLength(500);
        entity.Property(x => x.ExternalReference).HasMaxLength(120);
        entity.Property(x => x.Amount).HasPrecision(19, 4);
        entity.HasIndex(x => new { x.CompanyId, x.VoucherNo }).IsUnique();
        entity.HasIndex(x => new { x.CompanyId, x.ReferenceType, x.ReferenceId });
        entity.HasIndex(x => new { x.BranchId, x.VoucherDate, x.Status });
        entity.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        entity.Property<Guid>(actorProperty).IsRequired();
        entity.HasOne<User>().WithMany().HasForeignKey(actorProperty).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAuditAndSync(ModelBuilder mb)
    {
        var audit = mb.Entity<AuditEvent>();
        audit.ToTable("audit_events");
        audit.HasKey(x => x.Id);
        audit.Property(x => x.SequenceNo).ValueGeneratedOnAdd()
            .HasDefaultValueSql("nextval('transport_erp.audit_event_sequence_no_seq')");
        audit.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        audit.Property(x => x.Action).HasMaxLength(120).IsRequired();
        audit.Property(x => x.Outcome).HasMaxLength(40).IsRequired();
        audit.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        audit.Property(x => x.DeviceId).HasMaxLength(120);
        audit.Property(x => x.BeforeJson).HasColumnType("text");
        audit.Property(x => x.AfterJson).HasColumnType("text");
        audit.Property(x => x.Reason).HasMaxLength(500);
        audit.Property(x => x.Ip).HasMaxLength(64);
        audit.Property(x => x.Hash).HasMaxLength(128).IsRequired();
        audit.Property(x => x.PreviousHash).HasMaxLength(128);
        audit.HasIndex(x => new { x.CompanyId, x.OccurredAt });
        audit.HasIndex(x => new { x.EntityType, x.EntityId, x.OccurredAt });
        audit.HasIndex(x => x.CorrelationId);
        audit.HasIndex(x => x.OperationCorrelationId)
            .HasDatabaseName("ix_audit_event_operation_correlation");
        audit.HasIndex(x => x.Hash).IsUnique();
        audit.HasIndex(x => x.SequenceNo).IsUnique();
        audit.HasOne<User>().WithMany().HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.Restrict);
        audit.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        audit.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);

        var auditHead = mb.Entity<AuditStreamHead>();
        auditHead.ToTable("audit_stream_heads");
        auditHead.HasKey(x => x.StreamKey);
        auditHead.Property(x => x.StreamKey).HasMaxLength(200);
        auditHead.Property(x => x.LastHash).HasMaxLength(128);
        auditHead.Property(x => x.UpdatedAt).HasColumnType("timestamptz");

        var sync = mb.Entity<SyncOperation>();
        sync.ToTable("sync_operations", t =>
        {
            t.HasCheckConstraint("ck_sync_status", "\"Status\" IN ('QUEUED','SENDING','SUCCEEDED','FAILED','CONFLICT','REJECTED','RESOLVED')");
            t.HasCheckConstraint("ck_sync_operation_type", "\"OperationType\" IN ('CREATE','UPDATE','DELETE','COMMAND')");
            t.HasCheckConstraint("ck_sync_retry_count", "\"RetryCount\" >= 0");
            t.HasCheckConstraint("ck_sync_registered_device_binding",
                "(\"RegisteredDeviceId\" IS NULL AND \"RegisteredDeviceCredentialVersion\" IS NULL) OR " +
                "(\"RegisteredDeviceId\" IS NOT NULL AND \"RegisteredDeviceCredentialVersion\" >= 1 AND \"BranchId\" IS NOT NULL)");
            t.HasCheckConstraint("ck_sync_stage4_contract_bundle",
                "(\"ActionCode\" IS NULL AND \"ProtocolVersion\" IS NULL AND \"OperationCorrelationId\" IS NULL AND " +
                "\"RequestFingerprintVersion\" IS NULL AND \"RequestFingerprintHash\" IS NULL AND \"ProofKeyVersion\" IS NULL AND " +
                "\"ProofKeyThumbprint\" IS NULL AND \"AcceptedProofReplayId\" IS NULL) OR " +
                "(\"RequestFingerprintVersion\" = 'fp-v1' AND \"ProtocolVersion\" = 'sync-v1' AND " +
                "\"RegisteredDeviceId\" IS NOT NULL AND \"BranchId\" IS NOT NULL AND \"ActionCode\" IS NOT NULL AND " +
                "\"OperationCorrelationId\" IS NOT NULL AND \"OperationCorrelationId\" <> '00000000-0000-0000-0000-000000000000'::uuid AND " +
                "\"RequestFingerprintHash\" IS NOT NULL AND octet_length(\"RequestFingerprintHash\") = 32 AND " +
                "\"ProofKeyVersion\" IS NOT NULL AND \"ProofKeyVersion\" >= 1 AND " +
                "\"ProofKeyThumbprint\" IS NOT NULL AND length(\"ProofKeyThumbprint\") = 43 AND \"AcceptedProofReplayId\" IS NOT NULL)");
        });
        sync.HasKey(x => x.Id);
        sync.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        sync.Property(x => x.OperationType).HasMaxLength(20).IsRequired();
        sync.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        sync.Property(x => x.ClientOperationId).HasMaxLength(120).IsRequired();
        sync.Property(x => x.PayloadJson).HasColumnType("text").IsRequired();
        sync.Property(x => x.PayloadHash).HasMaxLength(128).IsRequired();
        sync.Property(x => x.Status).HasMaxLength(20).IsRequired();
        sync.Property(x => x.ErrorCode).HasMaxLength(80);
        sync.Property(x => x.ActionCode).HasMaxLength(120);
        sync.Property(x => x.ProtocolVersion).HasMaxLength(20);
        sync.Property(x => x.RequestFingerprintVersion).HasMaxLength(16);
        sync.Property(x => x.RequestFingerprintHash).HasColumnType("bytea");
        sync.Property(x => x.ProofKeyThumbprint).HasMaxLength(43);
        sync.HasIndex(x => new { x.CompanyId, x.RegisteredDeviceId, x.ClientOperationId }).IsUnique()
            .HasFilter("\"RegisteredDeviceId\" IS NOT NULL AND \"RequestFingerprintVersion\" = 'fp-v1'")
            .HasDatabaseName("ux_sync_op_registered_device_client");
        sync.HasIndex(x => new { x.CompanyId, x.DeviceId, x.ClientOperationId }).IsUnique()
            .HasFilter("\"RequestFingerprintVersion\" IS NULL")
            .HasDatabaseName("ux_sync_op_legacy_company_device_client");
        sync.HasIndex(x => x.AcceptedProofReplayId)
            .HasDatabaseName("ix_sync_op_accepted_proof");
        sync.HasIndex(x => new { x.CompanyId, x.Status, x.NextRetryAt });
        sync.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAt });
        sync.HasIndex(x => new { x.DeviceId, x.CreatedAt });
        sync.HasIndex(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId });
        sync.HasIndex(x => x.PayloadHash);
        sync.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        sync.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        sync.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        sync.HasOne<RegisteredDevice>().WithMany()
            .HasForeignKey(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId, x.DeviceId }).OnDelete(DeleteBehavior.Restrict);

        var nonce = mb.Entity<SyncProofNonce>();
        nonce.ToTable("sync_proof_nonces", t =>
        {
            t.HasCheckConstraint("ck_sync_nonce_key_version", "\"ProofKeyVersion\" >= 1");
            t.HasCheckConstraint("ck_sync_nonce_hash_len", "octet_length(\"NonceHash\") = 32");
            t.HasCheckConstraint("ck_sync_nonce_window", "\"ExpiresAt\" > \"IssuedAt\"");
        });
        nonce.HasKey(x => x.Id).HasName("pk_sync_proof_nonces");
        nonce.HasAlternateKey(x => new { x.Id, x.CompanyId, x.RegisteredDeviceId, x.DeviceId, x.ProofKeyVersion })
            .HasName("ux_sync_nonce_scope");
        nonce.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        nonce.Property(x => x.NonceHash).HasColumnType("bytea").IsRequired();
        nonce.Property(x => x.IssuedAt).HasColumnType("timestamptz");
        nonce.Property(x => x.ExpiresAt).HasColumnType("timestamptz");
        nonce.HasIndex(x => x.NonceHash).IsUnique().HasDatabaseName("ux_sync_nonce_hash");
        nonce.HasIndex(x => new { x.RegisteredDeviceId, x.ProofKeyVersion, x.ExpiresAt })
            .HasDatabaseName("ix_sync_nonce_device_key_expiry");
        nonce.HasIndex(x => x.ExpiresAt).HasDatabaseName("ix_sync_nonce_expiry");
        nonce.HasOne<RegisteredDevice>().WithMany()
            .HasForeignKey(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId, x.DeviceId })
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sync_nonce_registered_device");

        var replay = mb.Entity<SyncProofReplay>();
        replay.ToTable("sync_proof_replays", t =>
        {
            t.HasCheckConstraint("ck_sync_replay_key_version", "\"ProofKeyVersion\" >= 1");
            t.HasCheckConstraint("ck_sync_replay_hash_len",
                "octet_length(\"JtiHash\") = 32 AND octet_length(\"HtuHash\") = 32 AND char_length(\"ProofKeyThumbprint\") = 43");
            t.HasCheckConstraint("ck_sync_replay_method", "\"HttpMethod\" = 'POST'");
            t.HasCheckConstraint("ck_sync_replay_window",
                "\"ExpiresAt\" > \"FirstSeenAt\" AND \"FirstSeenAt\" >= \"IssuedAt\"");
        });
        replay.HasKey(x => x.Id).HasName("pk_sync_proof_replays");
        replay.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        replay.Property(x => x.ProofKeyThumbprint).HasMaxLength(43).IsRequired();
        replay.Property(x => x.JtiHash).HasColumnType("bytea").IsRequired();
        replay.Property(x => x.HtuHash).HasColumnType("bytea").IsRequired();
        replay.Property(x => x.HttpMethod).HasMaxLength(8).IsRequired();
        replay.Property(x => x.IssuedAt).HasColumnType("timestamptz");
        replay.Property(x => x.FirstSeenAt).HasColumnType("timestamptz");
        replay.Property(x => x.ExpiresAt).HasColumnType("timestamptz");
        replay.HasIndex(x => new { x.RegisteredDeviceId, x.ProofKeyVersion, x.JtiHash }).IsUnique()
            .HasDatabaseName("ux_sync_replay_device_key_jti");
        replay.HasIndex(x => x.ExpiresAt).HasDatabaseName("ix_sync_replay_expiry");
        replay.HasIndex(x => x.NonceRecordId).HasDatabaseName("ix_sync_replay_nonce");
        replay.HasOne<RegisteredDevice>().WithMany()
            .HasForeignKey(x => new { x.RegisteredDeviceId, x.CompanyId, x.DeviceId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId, x.DeviceId })
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sync_replay_registered_device");
        replay.HasOne<RegisteredDeviceAssignment>().WithMany()
            .HasForeignKey(x => new { x.DeviceAssignmentId, x.RegisteredDeviceId, x.CompanyId, x.UserId, x.BranchId })
            .HasPrincipalKey(x => new { x.Id, x.RegisteredDeviceId, x.CompanyId, x.UserId, x.BranchId })
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sync_replay_assignment_scope");
        replay.HasOne<SyncProofNonce>().WithMany()
            .HasForeignKey(x => new { x.NonceRecordId, x.CompanyId, x.RegisteredDeviceId, x.DeviceId, x.ProofKeyVersion })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId, x.RegisteredDeviceId, x.DeviceId, x.ProofKeyVersion })
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sync_replay_nonce_scope");

        var conflict = mb.Entity<ConflictCase>();
        conflict.ToTable("conflict_cases", t => t.HasCheckConstraint("ck_conflict_case_status", "\"Status\" IN ('OPEN','RESOLVED')"));
        conflict.HasKey(x => x.Id);
        conflict.Property(x => x.DeviceSnapshot).HasColumnType("text").IsRequired();
        conflict.Property(x => x.ServerSnapshot).HasColumnType("text").IsRequired();
        conflict.Property(x => x.ConflictReason).HasMaxLength(500).IsRequired();
        conflict.Property(x => x.Resolution).HasMaxLength(1000);
        conflict.Property(x => x.ResolvedBy).HasMaxLength(120);
        conflict.Property(x => x.Status).HasMaxLength(20).IsRequired();
        conflict.HasIndex(x => x.SyncOperationId).IsUnique();
        conflict.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status, x.CreatedAt });
        conflict.HasOne(x => x.SyncOperation).WithOne(x => x.ConflictCase)
            .HasForeignKey<ConflictCase>(x => x.SyncOperationId).OnDelete(DeleteBehavior.Restrict);
        conflict.HasOne<SyncOperation>().WithMany()
            .HasForeignKey(x => x.ReplacedByOperationId).OnDelete(DeleteBehavior.Restrict);
        conflict.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        conflict.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}
