using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpP2FinanceModel
{
    public static void Configure(ModelBuilder mb)
    {
        var waybill = mb.Entity<WaybillEntity>();
        waybill.Property(x => x.FinancialStatus).HasMaxLength(20).IsRequired().HasDefaultValue("UNPAID");
        waybill.HasIndex(x => new { x.CompanyId, x.BranchId, x.FinancialStatus });
        waybill.ToTable("waybills", "transport_erp", t =>
            t.HasCheckConstraint("ck_waybills_financial_status", "\"FinancialStatus\" IN ('UNPAID','PARTIAL','PAID','OVERPAID')"));

        var plan = mb.Entity<PaymentPlanLineEntity>();
        plan.ToTable("payment_plan_lines", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_payment_plan_status", "\"Status\" IN ('DRAFT','ACTIVE','SATISFIED','CANCELLED')");
            t.HasCheckConstraint("ck_payment_plan_mode", "(\"Amount\" IS NOT NULL AND \"Percent\" IS NULL) OR (\"Amount\" IS NULL AND \"Percent\" IS NOT NULL)");
            t.HasCheckConstraint("ck_payment_plan_amount", "\"Amount\" IS NULL OR \"Amount\" > 0");
            t.HasCheckConstraint("ck_payment_plan_percent", "\"Percent\" IS NULL OR (\"Percent\" > 0 AND \"Percent\" <= 100)");
        });
        plan.HasKey(x => x.Id);
        plan.Property(x => x.PayerRole).HasMaxLength(20).IsRequired();
        plan.Property(x => x.PaymentMethodCode).HasMaxLength(60).IsRequired();
        plan.Property(x => x.Amount).HasPrecision(19,4);
        plan.Property(x => x.Percent).HasPrecision(9,4);
        plan.Property(x => x.DueTrigger).HasMaxLength(40).IsRequired();
        plan.Property(x => x.DueAt).HasColumnType("timestamptz");
        plan.Property(x => x.Status).HasMaxLength(20).IsRequired();
        plan.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        plan.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        plan.Property(x => x.Version).IsConcurrencyToken();
        plan.HasIndex(x => new { x.WaybillId, x.LineNo }).IsUnique();
        plan.HasIndex(x => new { x.WaybillId, x.Status });
        plan.HasOne(x => x.Waybill).WithMany(x => x.PaymentPlanLines).HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Cascade);
        plan.HasOne<OperationalPartyEntity>().WithMany().HasForeignKey(x => x.PartyId).OnDelete(DeleteBehavior.Restrict);
        plan.HasOne<Currency>().WithMany().HasForeignKey(x => x.AmountCurrencyId).OnDelete(DeleteBehavior.Restrict);

        var collection = mb.Entity<CollectionTransactionEntity>();
        collection.ToTable("collection_transactions", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_collection_status", "\"Status\" IN ('ACCEPTED','REVERSED')");
            t.HasCheckConstraint("ck_collection_amount", "\"Amount\" > 0 AND \"ExchangeRate\" > 0");
            t.HasCheckConstraint("ck_collection_reversal_shape", "(\"Status\" = 'ACCEPTED' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
        });
        collection.HasKey(x => x.Id);
        collection.Property(x => x.PayerRole).HasMaxLength(20).IsRequired();
        collection.Property(x => x.PaymentMethodCode).HasMaxLength(60).IsRequired();
        collection.Property(x => x.ExchangeRate).HasPrecision(19,8);
        collection.Property(x => x.Amount).HasPrecision(19,4);
        collection.Property(x => x.CollectedByType).HasMaxLength(40).IsRequired();
        collection.Property(x => x.CollectedAt).HasColumnType("timestamptz");
        collection.Property(x => x.ClientOperationId).HasMaxLength(160).IsRequired();
        collection.Property(x => x.Status).HasMaxLength(20).IsRequired();
        collection.Property(x => x.ReversalReason).HasMaxLength(500);
        collection.HasIndex(x => new { x.CompanyId, x.ClientOperationId }).IsUnique();
        collection.HasIndex(x => new { x.WaybillId, x.Status });
        collection.HasIndex(x => new { x.CollectedById, x.CollectedAt });
        collection.HasIndex(x => x.ReversalOfId).IsUnique().HasFilter("\"ReversalOfId\" IS NOT NULL");
        collection.HasOne(x => x.Waybill).WithMany(x => x.Collections).HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
        collection.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        collection.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        collection.HasOne<OperationalPartyEntity>().WithMany().HasForeignKey(x => x.PartyId).OnDelete(DeleteBehavior.Restrict);
        collection.HasOne(x => x.ReversalOf).WithMany().HasForeignKey(x => x.ReversalOfId).OnDelete(DeleteBehavior.Restrict);

        var link = mb.Entity<FinancialLinkEntity>();
        link.ToTable("waybill_financial_links", "transport_erp", t =>
            t.HasCheckConstraint("ck_financial_link_status", "\"Status\" IN ('ACTIVE','REVERSED')"));
        link.HasKey(x => x.Id);
        link.Property(x => x.DocumentType).HasMaxLength(80).IsRequired();
        link.Property(x => x.Amount).HasPrecision(19,4);
        link.Property(x => x.LinkType).HasMaxLength(60).IsRequired();
        link.Property(x => x.Status).HasMaxLength(20).IsRequired();
        link.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        link.HasIndex(x => new { x.WaybillId, x.DocumentType, x.DocumentId, x.LinkType }).IsUnique();
        link.HasIndex(x => x.DocumentId);
        link.HasOne(x => x.Waybill).WithMany(x => x.FinancialLinks).HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
        link.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
