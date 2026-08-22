using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1AccountingAuthorityModelCustomizer(ModelCustomizerDependencies dependencies)
    : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);
        var audit = modelBuilder.Entity<AuditEvent>();
        audit.ToTable("audit_events", "transport_erp");
        audit.HasKey(x => x.Id);
        audit.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        audit.Property(x => x.Action).HasMaxLength(120).IsRequired();
        audit.Property(x => x.Outcome).HasMaxLength(40).IsRequired();
        audit.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        audit.Property(x => x.DeviceId).HasMaxLength(120);
        audit.Property(x => x.Reason).HasMaxLength(500);
        audit.Property(x => x.Ip).HasMaxLength(64);
        audit.Property(x => x.Hash).HasMaxLength(128).IsRequired();
        audit.Property(x => x.PreviousHash).HasMaxLength(128);
    }
}
