using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

internal static class TransportErpGreenfieldDbp004Model
{
    internal static void Configure(ModelBuilder mb)
    {
        var stream = mb.Entity<AuditStreamHead>();
        stream.ToTable("audit_stream_heads", t =>
        {
            t.HasCheckConstraint("ck_audit_stream_heads_sequence", "\"LastSequence\" >= 0");
            t.HasCheckConstraint("ck_audit_stream_heads_hash_length", "\"LastHashV2\" IS NULL OR octet_length(\"LastHashV2\") = 32");
            t.HasCheckConstraint("ck_audit_stream_heads_shape", "(\"LastSequence\" = 0 AND \"LastHashV2\" IS NULL) OR (\"LastSequence\" > 0 AND \"LastHashV2\" IS NOT NULL)");
            t.HasCheckConstraint("ck_audit_stream_heads_concurrency", "\"ConcurrencyVersion\" >= 1");
        });
        stream.HasKey(x => x.Id);
        stream.Property(x => x.StreamKey).HasMaxLength(300).IsRequired();
        stream.Property(x => x.LastHashV2).HasColumnType("bytea");
        stream.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        stream.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        stream.HasIndex(x => new { x.CompanyId, x.StreamKey }).IsUnique();
        stream.HasIndex(x => new { x.CompanyId, x.BranchId });
        stream.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        stream.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);

        var audit = mb.Entity<AuditEvent>();
        audit.ToTable("audit_events", t =>
        {
            t.HasCheckConstraint("ck_audit_events_v2_versions", "\"HashVersion\" = 2 AND \"CanonicalizerVersion\" = 1");
            t.HasCheckConstraint("ck_audit_events_stream_sequence", "\"StreamSequence\" >= 1");
            t.HasCheckConstraint("ck_audit_events_previous_hash_v2_length", "\"PreviousHashV2\" IS NULL OR octet_length(\"PreviousHashV2\") = 32");
            t.HasCheckConstraint("ck_audit_events_hash_v2_length", "octet_length(\"HashV2\") = 32");
            t.HasCheckConstraint("ck_audit_events_payload_digest_length", "octet_length(\"PayloadDigest\") = 32");
            t.HasCheckConstraint("ck_audit_events_retention_class", "btrim(\"RetentionClass\") <> ''");
        });
        audit.Property<short>("HashVersion").HasDefaultValue((short)2).IsRequired();
        audit.Property<short>("CanonicalizerVersion").HasDefaultValue((short)1).IsRequired();
        audit.Property<Guid>("StreamHeadId").IsRequired();
        audit.Property<long>("StreamSequence").IsRequired();
        audit.Property<byte[]?>("PreviousHashV2").HasColumnType("bytea");
        audit.Property<byte[]>("HashV2").HasColumnType("bytea").IsRequired();
        audit.Property<byte[]>("PayloadDigest").HasColumnType("bytea").IsRequired();
        audit.Property<Guid>("OperationId").IsRequired();
        audit.Property<string>("RetentionClass").HasMaxLength(30).IsRequired();
        audit.HasIndex("StreamHeadId", "StreamSequence").IsUnique();
        audit.HasIndex("HashV2").IsUnique();
        audit.HasIndex("StreamHeadId", "OperationId").IsUnique();
        audit.HasOne<AuditStreamHead>().WithMany().HasForeignKey("StreamHeadId").OnDelete(DeleteBehavior.Restrict);

        var outbox = mb.Entity<IntegrationOutbox>();
        outbox.ToTable("integration_outbox", t =>
        {
            t.HasCheckConstraint("ck_integration_outbox_contract_version", "\"ContractVersion\" >= 1");
            t.HasCheckConstraint("ck_integration_outbox_payload_hash", "octet_length(\"PayloadSha256\") = 32");
            t.HasCheckConstraint("ck_integration_outbox_attempt_count", "\"AttemptCount\" >= 0");
            t.HasCheckConstraint("ck_integration_outbox_status", "\"Status\" IN ('PENDING','LEASED','PUBLISHED','FAILED')");
            t.HasCheckConstraint("ck_integration_outbox_available_time", "\"AvailableAt\" >= \"OccurredAt\"");
            t.HasCheckConstraint("ck_integration_outbox_lease_shape", "(\"Status\" = 'LEASED' AND \"LeaseOwner\" IS NOT NULL AND \"LeaseToken\" IS NOT NULL AND \"LeaseExpiresAt\" IS NOT NULL) OR (\"Status\" <> 'LEASED' AND \"LeaseOwner\" IS NULL AND \"LeaseToken\" IS NULL AND \"LeaseExpiresAt\" IS NULL)");
            t.HasCheckConstraint("ck_integration_outbox_publish_shape", "(\"Status\" = 'PUBLISHED' AND \"PublishedAt\" IS NOT NULL) OR (\"Status\" <> 'PUBLISHED' AND \"PublishedAt\" IS NULL)");
            t.HasCheckConstraint("ck_integration_outbox_concurrency", "\"ConcurrencyVersion\" >= 1");
        });
        outbox.HasKey(x => x.Id);
        outbox.Property(x => x.Topic).HasMaxLength(160).IsRequired();
        outbox.Property(x => x.PayloadJson).HasColumnType("text").IsRequired();
        outbox.Property(x => x.PayloadSha256).HasColumnType("bytea").IsRequired();
        outbox.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        outbox.Property(x => x.AvailableAt).HasColumnType("timestamptz");
        outbox.Property(x => x.Status).HasMaxLength(20).IsRequired();
        outbox.Property(x => x.LeaseOwner).HasMaxLength(120);
        outbox.Property(x => x.LeaseExpiresAt).HasColumnType("timestamptz");
        outbox.Property(x => x.PublishedAt).HasColumnType("timestamptz");
        outbox.Property(x => x.LastError).HasMaxLength(1000);
        outbox.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        outbox.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        outbox.HasIndex(x => new { x.CompanyId, x.OperationId, x.Topic }).IsUnique();
        outbox.HasIndex(x => new { x.Status, x.AvailableAt });
        outbox.HasIndex(x => new { x.Status, x.LeaseExpiresAt });
        outbox.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        outbox.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        outbox.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId })
            .HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
    }
}
