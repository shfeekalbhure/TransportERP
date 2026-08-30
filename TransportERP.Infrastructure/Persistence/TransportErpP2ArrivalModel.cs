using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpP2ArrivalModel
{
    public static void Configure(ModelBuilder mb)
    {
        ConfigureReceipt(mb);
        ConfigureHolding(mb);
        ConfigureException(mb);
        ExtendMovementScope(mb);
    }

    private static void ConfigureReceipt(ModelBuilder mb)
    {
        var receipt = mb.Entity<ArrivalReceiptEntity>();
        receipt.ToTable("arrival_receipts", "transport_erp", t =>
            t.HasCheckConstraint("ck_arrival_receipt_status", "\"Status\" IN ('DRAFT','FINALIZED')"));
        receipt.HasKey(x => x.Id);
        receipt.Property(x => x.ReceivedAt).HasColumnType("timestamptz");
        receipt.Property(x => x.Status).HasMaxLength(20).IsRequired();
        receipt.Property(x => x.CreateClientOperationId).HasMaxLength(160).IsRequired();
        receipt.Property(x => x.LastClientOperationId).HasMaxLength(160).IsRequired();
        receipt.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        receipt.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        receipt.Property(x => x.Version).IsConcurrencyToken();
        receipt.HasIndex(x => new { x.TripId, x.LocationId });
        receipt.HasIndex(x => x.Status);
        receipt.HasIndex(x => new { x.CompanyId, x.ReceivingBranchId, x.CreateClientOperationId }).IsUnique();
        receipt.HasIndex(x => new { x.TripId, x.ManifestId, x.LocationId }).IsUnique();
        receipt.HasOne(x => x.Trip).WithMany().HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.Restrict);
        receipt.HasOne(x => x.Manifest).WithMany().HasForeignKey(x => x.ManifestId).OnDelete(DeleteBehavior.Restrict);

        var line = mb.Entity<ArrivalReceiptLineEntity>();
        line.ToTable("arrival_receipt_lines", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_arrival_line_quantities", "\"ExpectedQty\" > 0 AND \"ActualQty\" >= 0 AND \"ActualQty\" <= \"ExpectedQty\" AND \"DamageQty\" >= 0 AND \"DamageQty\" <= \"ActualQty\"");
            t.HasCheckConstraint("ck_arrival_line_difference", "\"DifferenceType\" IN ('UNVALIDATED','NONE','SHORT','DAMAGE','SHORT_AND_DAMAGE')");
        });
        line.HasKey(x => x.Id);
        line.Property(x => x.ExpectedQty).HasPrecision(19, 4);
        line.Property(x => x.ActualQty).HasPrecision(19, 4);
        line.Property(x => x.DamageQty).HasPrecision(19, 4);
        line.Property(x => x.DifferenceType).HasMaxLength(30).IsRequired();
        line.Property(x => x.Notes).HasMaxLength(1000);
        line.HasIndex(x => new { x.ArrivalReceiptId, x.ManifestLineId }).IsUnique();
        line.HasIndex(x => new { x.WaybillItemId, x.DifferenceType });
        line.HasOne(x => x.ArrivalReceipt).WithMany(x => x.Lines).HasForeignKey(x => x.ArrivalReceiptId).OnDelete(DeleteBehavior.Cascade);
        line.HasOne(x => x.ManifestLine).WithMany().HasForeignKey(x => x.ManifestLineId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne(x => x.WaybillItem).WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureHolding(ModelBuilder mb)
    {
        var holding = mb.Entity<WarehouseHoldingEntity>();
        holding.ToTable("warehouse_holdings", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_warehouse_holding_quantity", "\"Quantity\" >= 0");
            t.HasCheckConstraint("ck_warehouse_holding_type", "\"HoldingType\" IN ('TRANSIT','DESTINATION')");
            t.HasCheckConstraint("ck_warehouse_holding_status", "\"Status\" IN ('AVAILABLE','RESERVED','RELEASED','EXCEPTION')");
        });
        holding.HasKey(x => x.Id);
        holding.Property(x => x.Quantity).HasPrecision(19, 4);
        holding.Property(x => x.HoldingType).HasMaxLength(20).IsRequired();
        holding.Property(x => x.Status).HasMaxLength(20).IsRequired();
        holding.Property(x => x.SourceClientOperationId).HasMaxLength(160).IsRequired();
        holding.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        holding.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        holding.Property(x => x.Version).IsConcurrencyToken();
        holding.HasIndex(x => new { x.LocationId, x.Status });
        holding.HasIndex(x => x.WaybillItemId);
        holding.HasIndex(x => new { x.CompanyId, x.BranchId, x.WaybillItemId, x.LocationId, x.Status });
        holding.HasOne(x => x.WaybillItem).WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureException(ModelBuilder mb)
    {
        var exception = mb.Entity<ShipmentExceptionEntity>();
        exception.ToTable("shipment_exceptions", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_shipment_exception_severity", "\"Severity\" IN ('BLOCKING','WARNING','INFO')");
            t.HasCheckConstraint("ck_shipment_exception_status", "\"Status\" IN ('OPEN','RESOLVED')");
        });
        exception.HasKey(x => x.Id);
        exception.Property(x => x.ExceptionType).HasMaxLength(40).IsRequired();
        exception.Property(x => x.Severity).HasMaxLength(20).IsRequired();
        exception.Property(x => x.Status).HasMaxLength(20).IsRequired();
        exception.Property(x => x.ResolutionNotes).HasMaxLength(1000);
        exception.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        exception.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        exception.Property(x => x.Version).IsConcurrencyToken();
        exception.HasIndex(x => new { x.CompanyId, x.BranchId, x.TripId, x.Status });
        exception.HasIndex(x => new { x.TripId, x.Status });
    }

    private static void ExtendMovementScope(ModelBuilder mb)
    {
        mb.Entity<MovementEventEntity>().ToTable("movement_events", "transport_erp", t =>
            t.HasCheckConstraint("ck_movement_event_c_scope", "\"EventType\" IN ('LOAD','DEPART','ARRIVE','UNLOAD','REALLOCATE')"));
    }
}
