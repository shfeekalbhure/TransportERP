using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Adds P2-C01-A entities to the existing TransportErpDbContext model without rewriting P1 mappings.
/// The base customizer invokes TransportErpDbContext.OnModelCreating first; this layer is additive only.
/// </summary>
public sealed class TransportErpP2ModelCustomizer(ModelCustomizerDependencies dependencies)
    : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);
        ConfigureWaybillFoundation(modelBuilder);
    }

    private static void ConfigureWaybillFoundation(ModelBuilder mb)
    {
        var party = mb.Entity<OperationalPartyEntity>();
        party.ToTable("operational_parties", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_operational_parties_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
            t.HasCheckConstraint("ck_operational_parties_version", "\"Version\" >= 1");
        });
        party.HasKey(x => x.Id);
        party.Property(x => x.PartyNo).HasMaxLength(40).IsRequired();
        party.Property(x => x.Name).HasMaxLength(250).IsRequired();
        party.Property(x => x.Mobile).HasMaxLength(40).IsRequired();
        party.Property(x => x.IdentityType).HasMaxLength(60);
        party.Property(x => x.IdentityNo).HasMaxLength(120);
        party.Property(x => x.AddressLine).HasMaxLength(500);
        party.Property(x => x.Status).HasMaxLength(20).IsRequired();
        party.Property(x => x.ClientOperationId).HasMaxLength(120).IsRequired();
        party.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        party.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        party.Property(x => x.Version).IsConcurrencyToken();
        party.HasIndex(x => new { x.CompanyId, x.PartyNo }).IsUnique();
        party.HasIndex(x => new { x.CompanyId, x.ClientOperationId }).IsUnique();
        party.HasIndex(x => new { x.CompanyId, x.Mobile });
        party.HasIndex(x => new { x.CompanyId, x.IdentityNo }).HasFilter("\"IdentityNo\" IS NOT NULL");
        party.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        party.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        party.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);

        var waybill = mb.Entity<WaybillEntity>();
        waybill.ToTable("waybills", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_waybills_status", "\"Status\" IN ('DRAFT','READY_FOR_APPROVAL','APPROVED','CANCELLED')");
            t.HasCheckConstraint("ck_waybills_exchange_rate", "\"ExchangeRate\" > 0");
            t.HasCheckConstraint("ck_waybills_amounts", "\"FreightTotal\" >= 0 AND \"DiscountTotal\" >= 0 AND \"DiscountTotal\" <= \"FreightTotal\"");
            t.HasCheckConstraint("ck_waybills_version", "\"Version\" >= 1");
            t.HasCheckConstraint("ck_waybills_number_state", "(\"Status\" = 'APPROVED' AND \"WaybillNo\" IS NOT NULL) OR (\"Status\" <> 'APPROVED' AND \"WaybillNo\" IS NULL)");
        });
        waybill.HasKey(x => x.Id);
        waybill.Property(x => x.DraftNo).HasMaxLength(60).IsRequired();
        waybill.Property(x => x.WaybillNo).HasMaxLength(60);
        waybill.Property(x => x.WaybillDateTime).HasColumnType("timestamptz");
        waybill.Property(x => x.ServiceType).HasMaxLength(60).IsRequired();
        waybill.Property(x => x.Priority).HasMaxLength(30).IsRequired();
        waybill.Property(x => x.ExchangeRate).HasPrecision(19, 8);
        waybill.Property(x => x.FreightTotal).HasPrecision(19, 4);
        waybill.Property(x => x.DiscountTotal).HasPrecision(19, 4);
        waybill.Property(x => x.Status).HasMaxLength(30).IsRequired();
        waybill.Property(x => x.CreateClientOperationId).HasMaxLength(120).IsRequired();
        waybill.Property(x => x.LastClientOperationId).HasMaxLength(120).IsRequired();
        waybill.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        waybill.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        waybill.Property(x => x.Version).IsConcurrencyToken();
        waybill.HasIndex(x => new { x.CompanyId, x.DraftNo }).IsUnique();
        waybill.HasIndex(x => new { x.CompanyId, x.WaybillNo }).IsUnique().HasFilter("\"WaybillNo\" IS NOT NULL");
        waybill.HasIndex(x => new { x.CompanyId, x.BranchId, x.CreateClientOperationId }).IsUnique();
        waybill.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status, x.WaybillDateTime });
        waybill.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        waybill.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        waybill.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);

        var wp = mb.Entity<WaybillPartyEntity>();
        wp.ToTable("waybill_parties", "transport_erp", t =>
            t.HasCheckConstraint("ck_waybill_parties_role", "\"Role\" IN ('SENDER','RECEIVER','PAYER')"));
        wp.HasKey(x => x.Id);
        wp.Property(x => x.Role).HasMaxLength(20).IsRequired();
        wp.Property(x => x.NameSnapshot).HasMaxLength(250).IsRequired();
        wp.Property(x => x.MobileSnapshot).HasMaxLength(40).IsRequired();
        wp.Property(x => x.IdentityTypeSnapshot).HasMaxLength(60);
        wp.Property(x => x.IdentityNoSnapshot).HasMaxLength(120);
        wp.Property(x => x.AddressLineSnapshot).HasMaxLength(500);
        wp.HasIndex(x => new { x.WaybillId, x.Sequence }).IsUnique();
        wp.HasIndex(x => new { x.WaybillId, x.Role });
        wp.HasIndex(x => x.OperationalPartyId);
        wp.HasOne(x => x.Waybill).WithMany(x => x.Parties).HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Cascade);
        wp.HasOne<OperationalPartyEntity>().WithMany().HasForeignKey(x => x.OperationalPartyId).OnDelete(DeleteBehavior.Restrict);

        var item = mb.Entity<WaybillItemEntity>();
        item.ToTable("waybill_items", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_waybill_items_quantity", "\"Quantity\" > 0");
            t.HasCheckConstraint("ck_waybill_items_pieces", "\"Pieces\" IS NULL OR \"Pieces\" > 0");
            t.HasCheckConstraint("ck_waybill_items_measurements", "(\"Weight\" IS NULL OR \"Weight\" >= 0) AND (\"Length\" IS NULL OR \"Length\" >= 0) AND (\"Width\" IS NULL OR \"Width\" >= 0) AND (\"Height\" IS NULL OR \"Height\" >= 0) AND (\"DeclaredValue\" IS NULL OR \"DeclaredValue\" >= 0)");
        });
        item.HasKey(x => x.Id);
        item.Property(x => x.ItemType).HasMaxLength(100).IsRequired();
        item.Property(x => x.Contents).HasColumnType("text").IsRequired();
        item.Property(x => x.Quantity).HasPrecision(19, 4);
        item.Property(x => x.Weight).HasPrecision(19, 4);
        item.Property(x => x.Length).HasPrecision(19, 4);
        item.Property(x => x.Width).HasPrecision(19, 4);
        item.Property(x => x.Height).HasPrecision(19, 4);
        item.Property(x => x.DeclaredValue).HasPrecision(19, 4);
        item.Property(x => x.RiskFlagsJson).HasColumnType("jsonb").IsRequired();
        item.Property(x => x.Notes).HasMaxLength(1000);
        item.HasIndex(x => new { x.WaybillId, x.LineNo }).IsUnique();
        item.HasIndex(x => x.ItemType);
        item.HasOne(x => x.Waybill).WithMany(x => x.Items).HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Cascade);

        var seq = mb.Entity<NumberSequenceEntity>();
        seq.ToTable("number_sequences", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_number_sequences_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
            t.HasCheckConstraint("ck_number_sequences_next", "\"NextValue\" >= 1");
            t.HasCheckConstraint("ck_number_sequences_version", "\"Version\" >= 1");
        });
        seq.HasKey(x => x.Id);
        seq.Property(x => x.DocumentType).HasMaxLength(60).IsRequired();
        seq.Property(x => x.Prefix).HasMaxLength(30);
        seq.Property(x => x.ResetPolicy).HasMaxLength(40).IsRequired();
        seq.Property(x => x.Status).HasMaxLength(20).IsRequired();
        seq.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        seq.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        seq.Property(x => x.Version).IsConcurrencyToken();
        seq.HasIndex(x => new { x.CompanyId, x.DocumentType })
            .IsUnique()
            .HasFilter("\"BranchId\" IS NULL");
        seq.HasIndex(x => new { x.CompanyId, x.BranchId, x.DocumentType })
            .IsUnique()
            .HasFilter("\"BranchId\" IS NOT NULL");
        seq.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        seq.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);

        var reservation = mb.Entity<NumberReservationEntity>();
        reservation.ToTable("number_reservations", "transport_erp", t =>
            t.HasCheckConstraint("ck_number_reservations_state", "\"State\" IN ('RESERVED','COMMITTED','VOID')"));
        reservation.HasKey(x => x.Id);
        reservation.Property(x => x.IdempotencyKey).HasMaxLength(160).IsRequired();
        reservation.Property(x => x.RenderedNumber).HasMaxLength(60).IsRequired();
        reservation.Property(x => x.ReservedAt).HasColumnType("timestamptz");
        reservation.Property(x => x.CommittedAt).HasColumnType("timestamptz");
        reservation.Property(x => x.VoidedAt).HasColumnType("timestamptz");
        reservation.Property(x => x.VoidReason).HasMaxLength(500);
        reservation.Property(x => x.State).HasMaxLength(20).IsRequired();
        reservation.Property(x => x.LastTransitionKey).HasMaxLength(160);
        reservation.HasIndex(x => new { x.SequenceId, x.NumberValue }).IsUnique();
        reservation.HasIndex(x => new { x.CompanyId, x.IdempotencyKey }).IsUnique();
        reservation.HasIndex(x => new { x.CompanyId, x.RenderedNumber }).IsUnique();
        reservation.HasIndex(x => new { x.WaybillId, x.State });
        reservation.HasOne(x => x.Sequence).WithMany().HasForeignKey(x => x.SequenceId).OnDelete(DeleteBehavior.Restrict);
        reservation.HasOne(x => x.Waybill).WithMany().HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
        reservation.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        reservation.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}
