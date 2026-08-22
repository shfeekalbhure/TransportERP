using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransportERP.Infrastructure.Persistence;

public static class Wave1CashFlowActivities
{
    public const string Operating = "OPERATING";
    public const string Investing = "INVESTING";
    public const string Financing = "FINANCING";
    public const string Unclassified = "UNCLASSIFIED";
    public static bool IsKnown(string? value) => value is Operating or Investing or Financing or Unclassified;
}

public sealed class Wave1AccountGroupRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public bool AllowsPostingAccounts { get; set; }
    public bool ShowInFinancialStatements { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1AccountTypeRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public string FinancialClassification { get; set; } = string.Empty;
    public string NormalBalance { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1CustomerRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public Guid ControlAccountId { get; set; }
    public Guid? DefaultCurrencyId { get; set; }
    public Guid? DefaultBranchId { get; set; }
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1SupplierRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public Guid ControlAccountId { get; set; }
    public Guid? DefaultCurrencyId { get; set; }
    public Guid? DefaultBranchId { get; set; }
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1OpenItemRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string PartyType { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public Guid? SupplierId { get; set; }
    public string SourceDocumentType { get; set; } = string.Empty;
    public Guid SourceDocumentId { get; set; }
    public Guid JournalEntryId { get; set; }
    public int JournalLineNo { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal OriginalAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "OPEN";
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1PaymentAllocationRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string SourcePaymentType { get; set; } = string.Empty;
    public Guid SourcePaymentId { get; set; }
    public Guid TargetOpenItemId { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public DateTimeOffset AllocationDate { get; set; }
    public string Status { get; set; } = "APPLIED";
    public Guid? ReversesAllocationId { get; set; }
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1CashFlowAccountMappingRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid AccountId { get; set; }
    public string Activity { get; set; } = Wave1CashFlowActivities.Unclassified;
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1CashFlowMovementOverrideRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public Guid MovementId { get; set; }
    public string Activity { get; set; } = Wave1CashFlowActivities.Unclassified;
    public string Reason { get; set; } = string.Empty;
    public Guid ApprovedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1AccountingAuthorityDbContext(DbContextOptions<Wave1AccountingAuthorityDbContext> options) : DbContext(options)
{
    public DbSet<Wave1AccountGroupRecord> AccountGroups => Set<Wave1AccountGroupRecord>();
    public DbSet<Wave1AccountTypeRecord> AccountTypes => Set<Wave1AccountTypeRecord>();
    public DbSet<Wave1CustomerRecord> Customers => Set<Wave1CustomerRecord>();
    public DbSet<Wave1SupplierRecord> Suppliers => Set<Wave1SupplierRecord>();
    public DbSet<Wave1OpenItemRecord> OpenItems => Set<Wave1OpenItemRecord>();
    public DbSet<Wave1PaymentAllocationRecord> PaymentAllocations => Set<Wave1PaymentAllocationRecord>();
    public DbSet<Wave1CashFlowAccountMappingRecord> CashFlowAccountMappings => Set<Wave1CashFlowAccountMappingRecord>();
    public DbSet<Wave1CashFlowMovementOverrideRecord> CashFlowMovementOverrides => Set<Wave1CashFlowMovementOverrideRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");
        var g = mb.Entity<Wave1AccountGroupRecord>();
        g.ToTable("account_groups", t => t.HasCheckConstraint("ck_account_groups_display_order", "\"DisplayOrder\" >= 0"));
        g.HasKey(x => x.Id); g.Property(x => x.Code).HasMaxLength(60).IsRequired(); g.Property(x => x.ArabicName).HasMaxLength(200).IsRequired();
        g.Property(x => x.EnglishName).HasMaxLength(200); g.Property(x => x.Version).IsConcurrencyToken();
        g.Property(x => x.CreatedAt).HasColumnType("timestamptz"); g.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        g.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique(); g.HasIndex(x => new { x.CompanyId, x.IsActive, x.DisplayOrder });

        var at = mb.Entity<Wave1AccountTypeRecord>();
        at.ToTable("account_types", t =>
        {
            t.HasCheckConstraint("ck_account_types_financial_class", "\"FinancialClassification\" IN ('ASSET','LIABILITY','EQUITY','REVENUE','EXPENSE')");
            t.HasCheckConstraint("ck_account_types_normal_balance", "\"NormalBalance\" IN ('DEBIT','CREDIT')");
        });
        at.HasKey(x => x.Id); at.Property(x => x.Code).HasMaxLength(60).IsRequired(); at.Property(x => x.ArabicName).HasMaxLength(200).IsRequired();
        at.Property(x => x.EnglishName).HasMaxLength(200); at.Property(x => x.FinancialClassification).HasMaxLength(20).IsRequired();
        at.Property(x => x.NormalBalance).HasMaxLength(10).IsRequired(); at.Property(x => x.Version).IsConcurrencyToken();
        at.Property(x => x.CreatedAt).HasColumnType("timestamptz"); at.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        at.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique(); at.HasIndex(x => new { x.CompanyId, x.FinancialClassification, x.IsActive });

        ConfigureParty(mb.Entity<Wave1CustomerRecord>(), "customers");
        ConfigureParty(mb.Entity<Wave1SupplierRecord>(), "suppliers");

        var oi = mb.Entity<Wave1OpenItemRecord>();
        oi.ToTable("open_items", t =>
        {
            t.HasCheckConstraint("ck_open_items_party_type", "\"PartyType\" IN ('CUSTOMER','SUPPLIER')");
            t.HasCheckConstraint("ck_open_items_party_ref", "(\"PartyType\"='CUSTOMER' AND \"CustomerId\" IS NOT NULL AND \"SupplierId\" IS NULL) OR (\"PartyType\"='SUPPLIER' AND \"SupplierId\" IS NOT NULL AND \"CustomerId\" IS NULL)");
            t.HasCheckConstraint("ck_open_items_amount", "\"OriginalAmount\" >= 0");
            t.HasCheckConstraint("ck_open_items_status", "\"Status\" IN ('OPEN','CLOSED','CANCELLED')");
        });
        oi.HasKey(x => x.Id); oi.Property(x => x.PartyType).HasMaxLength(20).IsRequired(); oi.Property(x => x.SourceDocumentType).HasMaxLength(80).IsRequired();
        oi.Property(x => x.OriginalAmount).HasPrecision(22, 6); oi.Property(x => x.Status).HasMaxLength(20).IsRequired(); oi.Property(x => x.Version).IsConcurrencyToken();
        oi.Property(x => x.CreatedAt).HasColumnType("timestamptz"); oi.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        oi.HasIndex(x => new { x.CompanyId, x.PartyType, x.Status, x.DueDate }); oi.HasIndex(x => new { x.CompanyId, x.SourceDocumentType, x.SourceDocumentId });
        oi.HasIndex(x => new { x.JournalEntryId, x.JournalLineNo }).IsUnique();
        oi.HasOne<Wave1CustomerRecord>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        oi.HasOne<Wave1SupplierRecord>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);

        var pa = mb.Entity<Wave1PaymentAllocationRecord>();
        pa.ToTable("payment_allocations", t =>
        {
            t.HasCheckConstraint("ck_payment_allocations_amount", "\"Amount\" > 0");
            t.HasCheckConstraint("ck_payment_allocations_status", "\"Status\" IN ('APPLIED','REVERSED')");
        });
        pa.HasKey(x => x.Id); pa.Property(x => x.SourcePaymentType).HasMaxLength(80).IsRequired(); pa.Property(x => x.Amount).HasPrecision(22, 6);
        pa.Property(x => x.Status).HasMaxLength(20).IsRequired(); pa.Property(x => x.Version).IsConcurrencyToken();
        pa.Property(x => x.AllocationDate).HasColumnType("timestamptz"); pa.Property(x => x.CreatedAt).HasColumnType("timestamptz"); pa.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        pa.HasIndex(x => new { x.TargetOpenItemId, x.Status, x.AllocationDate }); pa.HasIndex(x => new { x.CompanyId, x.SourcePaymentType, x.SourcePaymentId });
        pa.HasOne<Wave1OpenItemRecord>().WithMany().HasForeignKey(x => x.TargetOpenItemId).OnDelete(DeleteBehavior.Restrict);
        pa.HasOne<Wave1PaymentAllocationRecord>().WithMany().HasForeignKey(x => x.ReversesAllocationId).OnDelete(DeleteBehavior.Restrict);

        var cm = mb.Entity<Wave1CashFlowAccountMappingRecord>();
        cm.ToTable("cash_flow_account_mappings", t => t.HasCheckConstraint("ck_cash_flow_account_activity", "\"Activity\" IN ('OPERATING','INVESTING','FINANCING','UNCLASSIFIED')"));
        cm.HasKey(x => x.Id); cm.Property(x => x.Activity).HasMaxLength(20).IsRequired(); cm.Property(x => x.Version).IsConcurrencyToken();
        cm.Property(x => x.CreatedAt).HasColumnType("timestamptz"); cm.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        cm.HasIndex(x => new { x.CompanyId, x.AccountId }).IsUnique();

        var ov = mb.Entity<Wave1CashFlowMovementOverrideRecord>();
        ov.ToTable("cash_flow_movement_overrides", t => t.HasCheckConstraint("ck_cash_flow_override_activity", "\"Activity\" IN ('OPERATING','INVESTING','FINANCING','UNCLASSIFIED')"));
        ov.HasKey(x => x.Id); ov.Property(x => x.MovementType).HasMaxLength(40).IsRequired(); ov.Property(x => x.Activity).HasMaxLength(20).IsRequired();
        ov.Property(x => x.Reason).HasMaxLength(500).IsRequired(); ov.Property(x => x.Version).IsConcurrencyToken();
        ov.Property(x => x.CreatedAt).HasColumnType("timestamptz"); ov.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        ov.HasIndex(x => new { x.CompanyId, x.MovementType, x.MovementId }).IsUnique().HasFilter("\"IsActive\" = TRUE");
    }

    private static void ConfigureParty<T>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> p, string table) where T : class
    {
        p.ToTable(table);
        p.HasKey("Id");
        p.Property<string>("Code").HasMaxLength(80).IsRequired(); p.Property<string>("ArabicName").HasMaxLength(250).IsRequired(); p.Property<string?>("EnglishName").HasMaxLength(250);
        p.Property<long>("Version").IsConcurrencyToken(); p.Property<DateTimeOffset>("CreatedAt").HasColumnType("timestamptz"); p.Property<DateTimeOffset>("UpdatedAt").HasColumnType("timestamptz");
        p.HasIndex("CompanyId", "Code").IsUnique(); p.HasIndex("CompanyId", "IsActive");
    }
}

[DbContext(typeof(Wave1AccountingAuthorityDbContext))]
[Migration("20260823003000_Wave1AccountingAuthority")]
public sealed class Wave1AccountingAuthority : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.CreateTable("account_groups", "transport_erp", t => new
        {
            Id=t.Column<Guid>(type:"uuid",nullable:false), CompanyId=t.Column<Guid>(type:"uuid",nullable:false), Code=t.Column<string>(type:"character varying(60)",maxLength:60,nullable:false),
            ArabicName=t.Column<string>(type:"character varying(200)",maxLength:200,nullable:false), EnglishName=t.Column<string>(type:"character varying(200)",maxLength:200,nullable:true),
            AllowsPostingAccounts=t.Column<bool>(type:"boolean",nullable:false), ShowInFinancialStatements=t.Column<bool>(type:"boolean",nullable:false), DisplayOrder=t.Column<int>(type:"integer",nullable:false),
            IsActive=t.Column<bool>(type:"boolean",nullable:false,defaultValue:true), Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L), CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false), UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)
        }, c => { c.PrimaryKey("PK_account_groups",x=>x.Id); c.ForeignKey("FK_account_groups_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict); c.CheckConstraint("ck_account_groups_display_order","\"DisplayOrder\" >= 0"); });
        m.CreateIndex("IX_account_groups_CompanyId_Code","transport_erp","account_groups",new[]{"CompanyId","Code"},unique:true);

        m.CreateTable("account_types", "transport_erp", t => new
        {
            Id=t.Column<Guid>(type:"uuid",nullable:false), CompanyId=t.Column<Guid>(type:"uuid",nullable:false), Code=t.Column<string>(type:"character varying(60)",maxLength:60,nullable:false),
            ArabicName=t.Column<string>(type:"character varying(200)",maxLength:200,nullable:false), EnglishName=t.Column<string>(type:"character varying(200)",maxLength:200,nullable:true),
            FinancialClassification=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false), NormalBalance=t.Column<string>(type:"character varying(10)",maxLength:10,nullable:false),
            IsActive=t.Column<bool>(type:"boolean",nullable:false,defaultValue:true), Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L), CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false), UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)
        }, c => { c.PrimaryKey("PK_account_types",x=>x.Id); c.ForeignKey("FK_account_types_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict); c.CheckConstraint("ck_account_types_financial_class","\"FinancialClassification\" IN ('ASSET','LIABILITY','EQUITY','REVENUE','EXPENSE')"); c.CheckConstraint("ck_account_types_normal_balance","\"NormalBalance\" IN ('DEBIT','CREDIT')"); });
        m.CreateIndex("IX_account_types_CompanyId_Code","transport_erp","account_types",new[]{"CompanyId","Code"},unique:true);

        CreateParty(m,"customers"); CreateParty(m,"suppliers");

        m.CreateTable("open_items","transport_erp",t=>new
        {
            Id=t.Column<Guid>(type:"uuid",nullable:false), CompanyId=t.Column<Guid>(type:"uuid",nullable:false), BranchId=t.Column<Guid>(type:"uuid",nullable:true), PartyType=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false),
            CustomerId=t.Column<Guid>(type:"uuid",nullable:true), SupplierId=t.Column<Guid>(type:"uuid",nullable:true), SourceDocumentType=t.Column<string>(type:"character varying(80)",maxLength:80,nullable:false), SourceDocumentId=t.Column<Guid>(type:"uuid",nullable:false),
            JournalEntryId=t.Column<Guid>(type:"uuid",nullable:false), JournalLineNo=t.Column<int>(type:"integer",nullable:false), CurrencyId=t.Column<Guid>(type:"uuid",nullable:false), OriginalAmount=t.Column<decimal>(type:"numeric(22,6)",nullable:false), DueDate=t.Column<DateTime>(type:"timestamp without time zone",nullable:false),
            Status=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false), Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L), CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false), UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)
        },c=>{c.PrimaryKey("PK_open_items",x=>x.Id); c.ForeignKey("FK_open_items_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_open_items_branches",x=>x.BranchId,"transport_erp","branches","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_open_items_customers",x=>x.CustomerId,"transport_erp","customers","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_open_items_suppliers",x=>x.SupplierId,"transport_erp","suppliers","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_open_items_journal_entries",x=>x.JournalEntryId,"transport_erp","journal_entries","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_open_items_currencies",x=>x.CurrencyId,"transport_erp","currencies","Id",onDelete:ReferentialAction.Restrict); c.CheckConstraint("ck_open_items_party_type","\"PartyType\" IN ('CUSTOMER','SUPPLIER')"); c.CheckConstraint("ck_open_items_party_ref","(\"PartyType\"='CUSTOMER' AND \"CustomerId\" IS NOT NULL AND \"SupplierId\" IS NULL) OR (\"PartyType\"='SUPPLIER' AND \"SupplierId\" IS NOT NULL AND \"CustomerId\" IS NULL)"); c.CheckConstraint("ck_open_items_amount","\"OriginalAmount\" >= 0"); c.CheckConstraint("ck_open_items_status","\"Status\" IN ('OPEN','CLOSED','CANCELLED')");});
        m.CreateIndex("IX_open_items_Aging","transport_erp","open_items",new[]{"CompanyId","PartyType","Status","DueDate"}); m.CreateIndex("IX_open_items_JournalLine","transport_erp","open_items",new[]{"JournalEntryId","JournalLineNo"},unique:true);

        m.CreateTable("payment_allocations","transport_erp",t=>new
        {
            Id=t.Column<Guid>(type:"uuid",nullable:false), CompanyId=t.Column<Guid>(type:"uuid",nullable:false), SourcePaymentType=t.Column<string>(type:"character varying(80)",maxLength:80,nullable:false), SourcePaymentId=t.Column<Guid>(type:"uuid",nullable:false), TargetOpenItemId=t.Column<Guid>(type:"uuid",nullable:false), Amount=t.Column<decimal>(type:"numeric(22,6)",nullable:false), CurrencyId=t.Column<Guid>(type:"uuid",nullable:false), AllocationDate=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false), Status=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false), ReversesAllocationId=t.Column<Guid>(type:"uuid",nullable:true), Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L), CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false), UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)
        },c=>{c.PrimaryKey("PK_payment_allocations",x=>x.Id); c.ForeignKey("FK_payment_allocations_open_items",x=>x.TargetOpenItemId,"transport_erp","open_items","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_payment_allocations_reversal",x=>x.ReversesAllocationId,"transport_erp","payment_allocations","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_payment_allocations_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict); c.ForeignKey("FK_payment_allocations_currencies",x=>x.CurrencyId,"transport_erp","currencies","Id",onDelete:ReferentialAction.Restrict); c.CheckConstraint("ck_payment_allocations_amount","\"Amount\" > 0"); c.CheckConstraint("ck_payment_allocations_status","\"Status\" IN ('APPLIED','REVERSED')");});
        m.CreateIndex("IX_payment_allocations_Target","transport_erp","payment_allocations",new[]{"TargetOpenItemId","Status","AllocationDate"});

        m.CreateTable("cash_flow_account_mappings","transport_erp",t=>new { Id=t.Column<Guid>(type:"uuid",nullable:false),CompanyId=t.Column<Guid>(type:"uuid",nullable:false),AccountId=t.Column<Guid>(type:"uuid",nullable:false),Activity=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false),IsActive=t.Column<bool>(type:"boolean",nullable:false,defaultValue:true),Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L),CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false),UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)},c=>{c.PrimaryKey("PK_cash_flow_account_mappings",x=>x.Id);c.ForeignKey("FK_cash_flow_account_mappings_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict);c.ForeignKey("FK_cash_flow_account_mappings_accounts",x=>x.AccountId,"transport_erp","chart_of_accounts","Id",onDelete:ReferentialAction.Restrict);c.CheckConstraint("ck_cash_flow_account_activity","\"Activity\" IN ('OPERATING','INVESTING','FINANCING','UNCLASSIFIED')");});
        m.CreateIndex("IX_cash_flow_account_mappings_Company_Account","transport_erp","cash_flow_account_mappings",new[]{"CompanyId","AccountId"},unique:true);

        m.CreateTable("cash_flow_movement_overrides","transport_erp",t=>new { Id=t.Column<Guid>(type:"uuid",nullable:false),CompanyId=t.Column<Guid>(type:"uuid",nullable:false),MovementType=t.Column<string>(type:"character varying(40)",maxLength:40,nullable:false),MovementId=t.Column<Guid>(type:"uuid",nullable:false),Activity=t.Column<string>(type:"character varying(20)",maxLength:20,nullable:false),Reason=t.Column<string>(type:"character varying(500)",maxLength:500,nullable:false),ApprovedByUserId=t.Column<Guid>(type:"uuid",nullable:false),IsActive=t.Column<bool>(type:"boolean",nullable:false,defaultValue:true),Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L),CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false),UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)},c=>{c.PrimaryKey("PK_cash_flow_movement_overrides",x=>x.Id);c.ForeignKey("FK_cash_flow_overrides_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict);c.ForeignKey("FK_cash_flow_overrides_users",x=>x.ApprovedByUserId,"transport_erp","users","Id",onDelete:ReferentialAction.Restrict);c.CheckConstraint("ck_cash_flow_override_activity","\"Activity\" IN ('OPERATING','INVESTING','FINANCING','UNCLASSIFIED')");});
        m.CreateIndex("IX_cash_flow_overrides_Movement","transport_erp","cash_flow_movement_overrides",new[]{"CompanyId","MovementType","MovementId"},unique:true,filter:"\"IsActive\" = TRUE");
    }

    private static void CreateParty(MigrationBuilder m,string table)
    {
        m.CreateTable(table,"transport_erp",t=>new {Id=t.Column<Guid>(type:"uuid",nullable:false),CompanyId=t.Column<Guid>(type:"uuid",nullable:false),Code=t.Column<string>(type:"character varying(80)",maxLength:80,nullable:false),ArabicName=t.Column<string>(type:"character varying(250)",maxLength:250,nullable:false),EnglishName=t.Column<string>(type:"character varying(250)",maxLength:250,nullable:true),ControlAccountId=t.Column<Guid>(type:"uuid",nullable:false),DefaultCurrencyId=t.Column<Guid>(type:"uuid",nullable:true),DefaultBranchId=t.Column<Guid>(type:"uuid",nullable:true),IsActive=t.Column<bool>(type:"boolean",nullable:false,defaultValue:true),Version=t.Column<long>(type:"bigint",nullable:false,defaultValue:1L),CreatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false),UpdatedAt=t.Column<DateTimeOffset>(type:"timestamptz",nullable:false)},c=>{c.PrimaryKey($"PK_{table}",x=>x.Id);c.ForeignKey($"FK_{table}_companies",x=>x.CompanyId,"transport_erp","companies","Id",onDelete:ReferentialAction.Restrict);c.ForeignKey($"FK_{table}_accounts",x=>x.ControlAccountId,"transport_erp","chart_of_accounts","Id",onDelete:ReferentialAction.Restrict);c.ForeignKey($"FK_{table}_currencies",x=>x.DefaultCurrencyId,"transport_erp","currencies","Id",onDelete:ReferentialAction.Restrict);c.ForeignKey($"FK_{table}_branches",x=>x.DefaultBranchId,"transport_erp","branches","Id",onDelete:ReferentialAction.Restrict);});
        m.CreateIndex($"IX_{table}_CompanyId_Code","transport_erp",table,new[]{"CompanyId","Code"},unique:true);
    }

    protected override void Down(MigrationBuilder m)
    {
        m.DropTable("cash_flow_movement_overrides","transport_erp"); m.DropTable("cash_flow_account_mappings","transport_erp"); m.DropTable("payment_allocations","transport_erp"); m.DropTable("open_items","transport_erp"); m.DropTable("account_types","transport_erp"); m.DropTable("account_groups","transport_erp"); m.DropTable("suppliers","transport_erp"); m.DropTable("customers","transport_erp");
    }
}
