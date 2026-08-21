using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public static class TransportErpP2ShippingModel
{
    public static void Configure(ModelBuilder mb)
    {
        ConfigureRelease(mb);
        ConfigureTrip(mb);
        ConfigureAllocation(mb);
        ConfigureManifest(mb);
        ConfigureMovement(mb);
        ConfigureHold(mb);
    }

    private static void ConfigureRelease(ModelBuilder mb)
    {
        var release = mb.Entity<ItemReleaseEntity>();
        release.ToTable("item_releases", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_item_release_quantity", "\"Quantity\" > 0");
            t.HasCheckConstraint("ck_item_release_status", "\"Status\" IN ('ACTIVE','REVERSED')");
            t.HasCheckConstraint("ck_item_release_reversal_shape", "(\"Status\" = 'ACTIVE' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
        });
        release.HasKey(x => x.Id);
        release.Property(x => x.Quantity).HasPrecision(19, 4);
        release.Property(x => x.ReleasedAt).HasColumnType("timestamptz");
        release.Property(x => x.ClientOperationId).HasMaxLength(160).IsRequired();
        release.Property(x => x.Status).HasMaxLength(20).IsRequired();
        release.Property(x => x.Reason).HasMaxLength(500);
        release.HasIndex(x => new { x.CompanyId, x.BranchId, x.ClientOperationId }).IsUnique();
        release.HasIndex(x => new { x.WaybillItemId, x.Status });
        release.HasIndex(x => x.ReversalOfId).IsUnique().HasFilter("\"ReversalOfId\" IS NOT NULL");
        release.HasOne(x => x.WaybillItem).WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
        release.HasOne(x => x.ReversalOf).WithMany().HasForeignKey(x => x.ReversalOfId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureTrip(ModelBuilder mb)
    {
        var trip = mb.Entity<TripEntity>();
        trip.ToTable("trips", "transport_erp", t =>
            t.HasCheckConstraint("ck_trip_status", "\"Status\" IN ('DRAFT','READY','DEPARTED','ARRIVED','CLOSED','CANCELLED')"));
        trip.HasKey(x => x.Id);
        trip.Property(x => x.TripNo).HasMaxLength(80).IsRequired();
        trip.Property(x => x.PlannedDepartAt).HasColumnType("timestamptz");
        trip.Property(x => x.ActualDepartAt).HasColumnType("timestamptz");
        trip.Property(x => x.ActualArriveAt).HasColumnType("timestamptz");
        trip.Property(x => x.Status).HasMaxLength(20).IsRequired();
        trip.Property(x => x.CreateClientOperationId).HasMaxLength(160).IsRequired();
        trip.Property(x => x.LastClientOperationId).HasMaxLength(160).IsRequired();
        trip.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        trip.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        trip.Property(x => x.Version).IsConcurrencyToken();
        trip.HasIndex(x => new { x.CompanyId, x.TripNo }).IsUnique();
        trip.HasIndex(x => new { x.CompanyId, x.BranchId, x.CreateClientOperationId }).IsUnique();
        trip.HasIndex(x => new { x.DriverId, x.Status });
        trip.HasIndex(x => new { x.VehicleId, x.Status });

        var stop = mb.Entity<TripStopEntity>();
        stop.ToTable("trip_stops", "transport_erp", t =>
            t.HasCheckConstraint("ck_trip_stop_status", "\"Status\" IN ('PLANNED','ARRIVED','DEPARTED','SKIPPED')"));
        stop.HasKey(x => x.Id);
        stop.Property(x => x.StopType).HasMaxLength(40).IsRequired();
        stop.Property(x => x.PlannedAt).HasColumnType("timestamptz");
        stop.Property(x => x.ArrivedAt).HasColumnType("timestamptz");
        stop.Property(x => x.DepartedAt).HasColumnType("timestamptz");
        stop.Property(x => x.Status).HasMaxLength(20).IsRequired();
        stop.HasIndex(x => new { x.TripId, x.StopNo }).IsUnique();
        stop.HasIndex(x => new { x.LocationId, x.Status });
        stop.HasOne(x => x.Trip).WithMany(x => x.Stops).HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAllocation(ModelBuilder mb)
    {
        var allocation = mb.Entity<TripAllocationEntity>();
        allocation.ToTable("trip_allocations", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_trip_allocation_quantity", "\"Quantity\" > 0");
            t.HasCheckConstraint("ck_trip_allocation_status", "\"Status\" IN ('ALLOCATED','REVERSED')");
            t.HasCheckConstraint("ck_trip_allocation_reversal_shape", "(\"Status\" = 'ALLOCATED' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
        });
        allocation.HasKey(x => x.Id);
        allocation.Property(x => x.Quantity).HasPrecision(19, 4);
        allocation.Property(x => x.AllocatedAt).HasColumnType("timestamptz");
        allocation.Property(x => x.ClientOperationId).HasMaxLength(160).IsRequired();
        allocation.Property(x => x.Status).HasMaxLength(20).IsRequired();
        allocation.Property(x => x.Reason).HasMaxLength(500);
        allocation.HasIndex(x => new { x.CompanyId, x.BranchId, x.ClientOperationId }).IsUnique();
        allocation.HasIndex(x => new { x.WaybillItemId, x.Status });
        allocation.HasIndex(x => new { x.TripId, x.Status });
        allocation.HasIndex(x => new { x.ReleaseId, x.Status });
        allocation.HasIndex(x => x.ReversalOfId).IsUnique().HasFilter("\"ReversalOfId\" IS NOT NULL");
        allocation.HasOne(x => x.WaybillItem).WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
        allocation.HasOne(x => x.Release).WithMany().HasForeignKey(x => x.ReleaseId).OnDelete(DeleteBehavior.Restrict);
        allocation.HasOne(x => x.Trip).WithMany(x => x.Allocations).HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.Restrict);
        allocation.HasOne(x => x.ReversalOf).WithMany().HasForeignKey(x => x.ReversalOfId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureManifest(ModelBuilder mb)
    {
        var manifest = mb.Entity<ManifestEntity>();
        manifest.ToTable("manifests", "transport_erp", t =>
            t.HasCheckConstraint("ck_manifest_status", "\"Status\" IN ('DRAFT','FINALIZED','HANDED_OVER','ACCEPTED','CLOSED')"));
        manifest.HasKey(x => x.Id);
        manifest.Property(x => x.ManifestNo).HasMaxLength(100).IsRequired();
        manifest.Property(x => x.HandoverAt).HasColumnType("timestamptz");
        manifest.Property(x => x.DriverAcceptedAt).HasColumnType("timestamptz");
        manifest.Property(x => x.Status).HasMaxLength(20).IsRequired();
        manifest.Property(x => x.CreateClientOperationId).HasMaxLength(160).IsRequired();
        manifest.Property(x => x.LastClientOperationId).HasMaxLength(160).IsRequired();
        manifest.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        manifest.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        manifest.Property(x => x.Version).IsConcurrencyToken();
        // C intentionally governs one manifest lifecycle per Trip. Without this invariant,
        // finalizing one draft can move the Trip to READY while another draft remains stranded.
        manifest.HasIndex(x => x.TripId).IsUnique();
        manifest.HasIndex(x => new { x.TripId, x.ManifestNo }).IsUnique();
        manifest.HasIndex(x => new { x.CompanyId, x.BranchId, x.CreateClientOperationId }).IsUnique();
        manifest.HasIndex(x => new { x.TripId, x.Status });
        manifest.HasOne(x => x.Trip).WithMany(x => x.Manifests).HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.Restrict);

        var line = mb.Entity<ManifestLineEntity>();
        line.ToTable("manifest_lines", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_manifest_line_quantities", "\"Quantity\" > 0 AND \"LoadedQuantity\" >= 0 AND \"LoadedQuantity\" <= \"Quantity\"");
            t.HasCheckConstraint("ck_manifest_line_load_status", "\"LoadStatus\" IN ('PLANNED','PARTIAL','LOADED','CANCELLED')");
        });
        line.HasKey(x => x.Id);
        line.Property(x => x.Quantity).HasPrecision(19, 4);
        line.Property(x => x.LoadedQuantity).HasPrecision(19, 4);
        line.Property(x => x.Weight).HasPrecision(19, 4);
        line.Property(x => x.Volume).HasPrecision(19, 4);
        line.Property(x => x.LoadStatus).HasMaxLength(20).IsRequired();
        line.HasIndex(x => new { x.ManifestId, x.AllocationId }).IsUnique();
        line.HasIndex(x => x.WaybillId);
        line.HasIndex(x => x.WaybillItemId);
        line.HasOne(x => x.Manifest).WithMany(x => x.Lines).HasForeignKey(x => x.ManifestId).OnDelete(DeleteBehavior.Cascade);
        line.HasOne(x => x.Allocation).WithMany().HasForeignKey(x => x.AllocationId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<WaybillEntity>().WithMany().HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
        line.HasOne<WaybillItemEntity>().WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureMovement(ModelBuilder mb)
    {
        var movement = mb.Entity<MovementEventEntity>();
        movement.ToTable("movement_events", "transport_erp", t =>
        {
            t.HasCheckConstraint("ck_movement_event_c_scope", "\"EventType\" IN ('LOAD','DEPART')");
            t.HasCheckConstraint("ck_movement_event_quantity", "\"Quantity\" IS NULL OR \"Quantity\" > 0");
        });
        movement.HasKey(x => x.Id);
        movement.Property(x => x.EventType).HasMaxLength(40).IsRequired();
        movement.Property(x => x.Quantity).HasPrecision(19, 4);
        movement.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        movement.Property(x => x.RecordedAt).HasColumnType("timestamptz");
        movement.Property(x => x.ReasonCode).HasMaxLength(80);
        movement.Property(x => x.ClientOperationId).HasMaxLength(200);
        movement.HasIndex(x => new { x.CompanyId, x.ClientOperationId }).IsUnique().HasFilter("\"ClientOperationId\" IS NOT NULL");
        movement.HasIndex(x => new { x.WaybillId, x.OccurredAt });
        movement.HasIndex(x => new { x.WaybillItemId, x.OccurredAt });
        movement.HasIndex(x => new { x.ManifestLineId, x.EventType });
        movement.HasOne<WaybillEntity>().WithMany().HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<WaybillItemEntity>().WithMany().HasForeignKey(x => x.WaybillItemId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<TripEntity>().WithMany().HasForeignKey(x => x.TripId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<ManifestEntity>().WithMany().HasForeignKey(x => x.ManifestId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<TripAllocationEntity>().WithMany().HasForeignKey(x => x.AllocationId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<ManifestLineEntity>().WithMany().HasForeignKey(x => x.ManifestLineId).OnDelete(DeleteBehavior.Restrict);
        movement.HasOne<MovementEventEntity>().WithMany().HasForeignKey(x => x.ReversesEventId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureHold(ModelBuilder mb)
    {
        var hold = mb.Entity<WaybillHoldEntity>();
        hold.ToTable("waybill_holds", "transport_erp", t =>
            t.HasCheckConstraint("ck_waybill_hold_status", "\"Status\" IN ('ACTIVE','RELEASED')"));
        hold.HasKey(x => x.Id);
        hold.Property(x => x.HoldType).HasMaxLength(80).IsRequired();
        hold.Property(x => x.Reason).HasMaxLength(500).IsRequired();
        hold.Property(x => x.PlacedAt).HasColumnType("timestamptz");
        hold.Property(x => x.ReleasedAt).HasColumnType("timestamptz");
        hold.Property(x => x.Status).HasMaxLength(20).IsRequired();
        hold.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        hold.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        hold.Property(x => x.Version).IsConcurrencyToken();
        hold.HasIndex(x => new { x.WaybillId, x.Status });
        hold.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        hold.HasOne<WaybillEntity>().WithMany().HasForeignKey(x => x.WaybillId).OnDelete(DeleteBehavior.Restrict);
    }
}
