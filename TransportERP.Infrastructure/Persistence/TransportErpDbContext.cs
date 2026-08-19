using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed class TransportErpDbContext(DbContextOptions<TransportErpDbContext> options) : DbContext(options)
{
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<User> Users => Set<User>();
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
    public DbSet<SyncOperation> SyncOperations => Set<SyncOperation>();

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
            typeof(Currency), typeof(Company), typeof(Branch), typeof(User), typeof(Role), typeof(Permission),
            typeof(GlobalSetting), typeof(CompanySetting), typeof(BranchSetting), typeof(ChartOfAccount),
            typeof(FiscalPeriod), typeof(FinancialDimension), typeof(JournalEntry), typeof(ReceiptVoucher),
            typeof(PaymentVoucher), typeof(SyncOperation) })
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
        currency.ToTable("currencies");
        currency.HasKey(x => x.Id);
        currency.Property(x => x.Code).HasMaxLength(3).IsRequired();
        currency.Property(x => x.NameAr).HasMaxLength(100).IsRequired();
        currency.Property(x => x.NameEn).HasMaxLength(100);
        currency.Property(x => x.Status).HasMaxLength(20).IsRequired();
        currency.HasIndex(x => x.Code).IsUnique();
        currency.HasIndex(x => x.Status);
        currency.HasCheckConstraint("ck_currencies_minor_unit", "\"MinorUnit\" BETWEEN 0 AND 6");
        currency.HasCheckConstraint("ck_currencies_status", "\"Status\" IN ('ACTIVE','INACTIVE')");

        var company = mb.Entity<Company>();
        company.ToTable("companies");
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
        company.HasCheckConstraint("ck_companies_status", "\"Status\" IN ('DRAFT','ACTIVE','SUSPENDED','CLOSED')");

        var branch = mb.Entity<Branch>();
        branch.ToTable("branches");
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
        branch.HasCheckConstraint("ck_branches_status", "\"Status\" IN ('DRAFT','ACTIVE','INACTIVE')");
    }

    private static void ConfigureIdentityAndSettings(ModelBuilder mb)
    {
        var user = mb.Entity<User>();
        user.ToTable("users", t => t.HasCheckConstraint("ck_users_status", "\"Status\" IN ('ACTIVE','LOCKED','DISABLED')"));
        user.HasKey(x => x.Id);
        user.Property(x => x.UserName).HasMaxLength(100).IsRequired();
        user.Property(x => x.NormalizedUserName).HasMaxLength(100).IsRequired();
        user.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
        user.Property(x => x.Email).HasMaxLength(320);
        user.Property(x => x.Phone).HasMaxLength(30);
        user.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
        user.Property(x => x.Status).HasMaxLength(20).IsRequired();
        user.HasIndex(x => new { x.NormalizedUserName, x.CompanyId }).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        user.HasIndex(x => new { x.Email, x.CompanyId }).IsUnique().HasFilter("\"Email\" IS NOT NULL AND \"DeletedAt\" IS NULL");
        user.HasIndex(x => new { x.BranchId, x.Status });
        user.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        user.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        user.HasQueryFilter(x => x.DeletedAt == null);

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
        rolePermission.ToTable("role_permissions");
        rolePermission.HasKey(x => new { x.RoleId, x.PermissionId, x.ScopeType });
        rolePermission.Property(x => x.ScopeType).HasMaxLength(20).IsRequired();
        rolePermission.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        rolePermission.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);

        var userRole = mb.Entity<UserRole>();
        userRole.ToTable("user_roles");
        userRole.HasKey(x => new { x.UserId, x.RoleId });
        userRole.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        userRole.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);

        var overrideEntity = mb.Entity<UserPermissionOverride>();
        overrideEntity.ToTable("user_permission_overrides");
        overrideEntity.HasKey(x => new { x.UserId, x.PermissionId });
        overrideEntity.Property(x => x.Reason).HasMaxLength(500);
        overrideEntity.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        overrideEntity.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);

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
        period.ToTable("fiscal_periods", t => t.HasCheckConstraint("ck_fiscal_periods_status", "\"Status\" IN ('OPEN','SOFT_CLOSED','CLOSED')"));
        period.HasKey(x => x.Id);
        period.Property(x => x.Code).HasMaxLength(40).IsRequired();
        period.Property(x => x.Status).HasMaxLength(20).IsRequired();
        period.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        period.HasIndex(x => new { x.CompanyId, x.StartDate, x.EndDate }).IsUnique();
        period.HasIndex(x => new { x.CompanyId, x.Status });
        period.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        period.HasOne<User>().WithMany().HasForeignKey(x => x.ClosedBy).OnDelete(DeleteBehavior.Restrict);
        period.HasCheckConstraint("ck_fiscal_periods_range", "\"EndDate\" >= \"StartDate\"");

        var dimension = mb.Entity<FinancialDimension>();
        dimension.ToTable("financial_dimensions");
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
        dimension.HasCheckConstraint("ck_financial_dimensions_dates", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");

        var journal = mb.Entity<JournalEntry>();
        journal.ToTable("journal_entries", t => t.HasCheckConstraint("ck_journal_entries_status", "\"Status\" IN ('DRAFT','CHECKED','APPROVED','POSTED','REVERSED')"));
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
        journal.HasCheckConstraint("ck_journal_entries_amounts", "\"TotalDebit\" >= 0 AND \"TotalCredit\" >= 0");

        var line = mb.Entity<JournalEntryLine>();
        line.ToTable("journal_entry_lines");
        line.HasKey(x => new { x.JournalEntryId, x.LineNo });
        line.Property(x => x.Description).HasMaxLength(500);
        line.Property(x => x.Debit).HasPrecision(19, 4);
        line.Property(x => x.Credit).HasPrecision(19, 4);
        line.Property(x => x.ForeignAmount).HasPrecision(19, 4);
        line.HasOne<JournalEntry>().WithMany(x => x.Lines).HasForeignKey(x => x.JournalEntryId).OnDelete(DeleteBehavior.Cascade);
        line.HasOne<ChartOfAccount>().WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<FinancialDimension>().WithMany().HasForeignKey(x => x.FinancialDimensionId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        line.HasCheckConstraint("ck_journal_lines_amounts", "\"Debit\" >= 0 AND \"Credit\" >= 0 AND (\"Debit\" > 0 OR \"Credit\" > 0) AND NOT (\"Debit\" > 0 AND \"Credit\" > 0)");

        ConfigureVoucher(mb.Entity<ReceiptVoucher>(), "receipt_vouchers", "PayerName", "CollectedBy", "ck_receipts_status");
        ConfigureVoucher(mb.Entity<PaymentVoucher>(), "payment_vouchers", "PayeeName", "PaidBy", "ck_payments_status");
    }

    private static void ConfigureVoucher<TEntity>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity> entity, string table, string partyProperty, string actorProperty, string statusConstraint) where TEntity : P1Entity, IP1Voucher
    {
        entity.ToTable(table, t => t.HasCheckConstraint(statusConstraint, "\"Status\" IN ('DRAFT','APPROVED','POSTED','CANCELLED')"));
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
        entity.HasCheckConstraint($"ck_{table}_amount", "\"Amount\" > 0");
    }

    private static void ConfigureAuditAndSync(ModelBuilder mb)
    {
        var audit = mb.Entity<AuditEvent>();
        audit.ToTable("audit_events");
        audit.HasKey(x => x.Id);
        audit.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        audit.Property(x => x.Action).HasMaxLength(120).IsRequired();
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
        audit.HasIndex(x => x.Hash).IsUnique();
        audit.HasOne<User>().WithMany().HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.Restrict);
        audit.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        audit.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);

        var sync = mb.Entity<SyncOperation>();
        sync.ToTable("sync_operations", t => t.HasCheckConstraint("ck_sync_status", "\"Status\" IN ('QUEUED','SENDING','SUCCEEDED','FAILED','CONFLICT','REJECTED','RESOLVED')"));
        sync.HasKey(x => x.Id);
        sync.Property(x => x.DeviceId).HasMaxLength(120).IsRequired();
        sync.Property(x => x.OperationType).HasMaxLength(20).IsRequired();
        sync.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        sync.Property(x => x.ClientOperationId).HasMaxLength(120).IsRequired();
        sync.Property(x => x.PayloadJson).HasColumnType("text").IsRequired();
        sync.Property(x => x.PayloadHash).HasMaxLength(128).IsRequired();
        sync.Property(x => x.Status).HasMaxLength(20).IsRequired();
        sync.Property(x => x.ErrorCode).HasMaxLength(80);
        sync.HasIndex(x => new { x.DeviceId, x.ClientOperationId }).IsUnique();
        sync.HasIndex(x => new { x.CompanyId, x.Status, x.NextRetryAt });
        sync.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAt });
        sync.HasIndex(x => new { x.DeviceId, x.CreatedAt });
        sync.HasIndex(x => x.PayloadHash);
        sync.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        sync.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        sync.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        sync.HasCheckConstraint("ck_sync_operation_type", "\"OperationType\" IN ('CREATE','UPDATE','DELETE','COMMAND')");
        sync.HasCheckConstraint("ck_sync_retry_count", "\"RetryCount\" >= 0");
    }
}
