using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260827110000_P1Stage5ParentLegalHoldGuard")]
public partial class P1Stage5ParentLegalHoldGuard : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations,
                   transport_erp.conflict_cases
          IN ACCESS EXCLUSIVE MODE;

        ALTER TABLE transport_erp.conflict_cases
          ADD COLUMN "ParentLegalHold" boolean NOT NULL DEFAULT false;
        UPDATE transport_erp.conflict_cases c
          SET "ParentLegalHold"=o."LegalHold"
          FROM transport_erp.sync_operations o
          WHERE o."Id"=c."SyncOperationId" AND o."CompanyId"=c."CompanyId";

        DO $body$
        BEGIN
          IF EXISTS (
            SELECT 1
            FROM transport_erp.conflict_cases c
            WHERE c."ParentLegalHold" AND c."RedactedAt" IS NOT NULL
          ) THEN
            RAISE EXCEPTION
              'P1_STAGE5_PARENT_LEGAL_HOLD_UP_BLOCKED_LEGACY_REDACTED_CONFLICT';
          END IF;
        END $body$;

        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT ck_conflict_snapshot_redaction_shape,
          ADD CONSTRAINT ck_conflict_snapshot_redaction_shape CHECK (
            ("RedactedAt" IS NULL AND "RetentionDaysApplied" IS NULL) OR
            (NOT "LegalHold" AND NOT "ParentLegalHold" AND
             "DeviceSnapshot"='{}' AND "ServerSnapshot"='{}' AND
             "Status"='RESOLVED' AND "ResolvedAt" IS NOT NULL AND
             "RetentionDaysApplied" IS NOT NULL AND
             "RetentionDaysApplied" BETWEEN 1 AND 90 AND
             "RedactedAt">="ResolvedAt"+make_interval(days => "RetentionDaysApplied"))
          );
        DROP INDEX transport_erp.ix_sync_conflict_retention_cleanup;
        CREATE INDEX ix_sync_conflict_retention_cleanup
          ON transport_erp.conflict_cases
             ("LegalHold","ParentLegalHold","RedactedAt","Status","ResolvedAt");

        ALTER FUNCTION transport_erp.enforce_sync_conflict_redaction()
          RENAME TO enforce_sync_conflict_redaction_stage5_parent_hold_backup;
        CREATE FUNCTION transport_erp.enforce_sync_conflict_redaction()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE
          redaction_allowed boolean := false;
          current_parent_hold boolean;
        BEGIN
          IF TG_OP='INSERT' THEN
            SELECT o."LegalHold" INTO current_parent_hold
            FROM transport_erp.sync_operations o
            WHERE o."Id"=NEW."SyncOperationId" AND o."CompanyId"=NEW."CompanyId"
            FOR UPDATE;
            IF NOT FOUND THEN
              RAISE EXCEPTION 'sync conflict parent operation scope mismatch';
            END IF;
            NEW."ParentLegalHold" := current_parent_hold;
            IF NEW."RedactedAt" IS NOT NULL OR NEW."RetentionDaysApplied" IS NOT NULL THEN
              RAISE EXCEPTION 'new sync conflict cannot be pre-redacted';
            END IF;
            RETURN NEW;
          END IF;

          IF OLD."SyncOperationId" IS DISTINCT FROM NEW."SyncOperationId" OR
             OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR
             OLD."BranchId" IS DISTINCT FROM NEW."BranchId" THEN
            RAISE EXCEPTION 'sync conflict parent operation scope is immutable';
          END IF;

          IF OLD."ParentLegalHold" IS DISTINCT FROM NEW."ParentLegalHold" THEN
            SELECT o."LegalHold" INTO current_parent_hold
            FROM transport_erp.sync_operations o
            WHERE o."Id"=NEW."SyncOperationId" AND o."CompanyId"=NEW."CompanyId";
            IF NOT FOUND OR NEW."ParentLegalHold" IS DISTINCT FROM current_parent_hold THEN
              RAISE EXCEPTION 'sync conflict parent legal hold is derived';
            END IF;
          END IF;

          IF OLD."Status"='RESOLVED' AND
             (OLD."Status" IS DISTINCT FROM NEW."Status" OR
              OLD."ResolvedAt" IS DISTINCT FROM NEW."ResolvedAt") THEN
            RAISE EXCEPTION 'sync conflict resolution retention timestamp is immutable';
          END IF;
          IF OLD."RedactedAt" IS DISTINCT FROM NEW."RedactedAt" OR
             OLD."RetentionDaysApplied" IS DISTINCT FROM NEW."RetentionDaysApplied" OR
             OLD."DeviceSnapshot" IS DISTINCT FROM NEW."DeviceSnapshot" OR
             OLD."ServerSnapshot" IS DISTINCT FROM NEW."ServerSnapshot" THEN
            redaction_allowed :=
              OLD."RedactedAt" IS NULL AND NEW."RedactedAt" IS NOT NULL AND
              NOT OLD."LegalHold" AND NOT NEW."LegalHold" AND
              NOT OLD."ParentLegalHold" AND NOT NEW."ParentLegalHold" AND
              NEW."DeviceSnapshot"='{}' AND NEW."ServerSnapshot"='{}' AND
              OLD."Status"=NEW."Status" AND OLD."Status"='RESOLVED' AND
              OLD."ResolvedAt" IS NOT NULL AND NEW."ResolvedAt"=OLD."ResolvedAt" AND
              OLD."RetentionDaysApplied" IS NULL AND NEW."RetentionDaysApplied" BETWEEN 1 AND 90 AND
              clock_timestamp()>=OLD."ResolvedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
              NEW."RedactedAt">=OLD."ResolvedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
              NEW."RedactedAt"<=clock_timestamp() AND
              EXISTS (SELECT 1 FROM transport_erp.sync_operations o
                WHERE o."Id"=OLD."SyncOperationId"
                  AND o."CompanyId"=OLD."CompanyId"
                  AND o."BranchId"=OLD."BranchId"
                  AND NOT o."LegalHold"
                  AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                       (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
                  AND clock_timestamp()>=o."UpdatedAt"+make_interval(days => NEW."RetentionDaysApplied")) AND
              (to_jsonb(OLD)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt','RetentionDaysApplied'])=
              (to_jsonb(NEW)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt','RetentionDaysApplied']);
            IF NOT redaction_allowed THEN
              RAISE EXCEPTION 'sync conflict redaction transition denied';
            END IF;
          END IF;
          RETURN NEW;
        END $body$;
        DROP TRIGGER trg_sync_conflict_redaction ON transport_erp.conflict_cases;
        CREATE TRIGGER trg_sync_conflict_redaction
          BEFORE INSERT OR UPDATE ON transport_erp.conflict_cases
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_conflict_redaction();

        CREATE FUNCTION transport_erp.propagate_sync_operation_legal_hold()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          UPDATE transport_erp.conflict_cases c
          SET "ParentLegalHold"=NEW."LegalHold"
          WHERE c."Id" IN (
            SELECT candidate."Id"
            FROM transport_erp.conflict_cases candidate
            WHERE candidate."SyncOperationId"=NEW."Id"
              AND candidate."CompanyId"=NEW."CompanyId"
            ORDER BY candidate."Id"
            FOR UPDATE
          );
          RETURN NULL;
        END $body$;
        CREATE TRIGGER trg_sync_operation_propagate_legal_hold
          AFTER UPDATE OF "LegalHold" ON transport_erp.sync_operations
          FOR EACH ROW
          WHEN (OLD."LegalHold" IS DISTINCT FROM NEW."LegalHold")
          EXECUTE FUNCTION transport_erp.propagate_sync_operation_legal_hold();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations,
                   transport_erp.conflict_cases
          IN ACCESS EXCLUSIVE MODE;

        DROP TRIGGER trg_sync_operation_propagate_legal_hold
          ON transport_erp.sync_operations;
        DROP FUNCTION transport_erp.propagate_sync_operation_legal_hold();

        DROP TRIGGER trg_sync_conflict_redaction ON transport_erp.conflict_cases;
        DROP FUNCTION transport_erp.enforce_sync_conflict_redaction();
        ALTER FUNCTION transport_erp.enforce_sync_conflict_redaction_stage5_parent_hold_backup()
          RENAME TO enforce_sync_conflict_redaction;
        CREATE TRIGGER trg_sync_conflict_redaction
          BEFORE INSERT OR UPDATE ON transport_erp.conflict_cases
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_conflict_redaction();

        DROP INDEX transport_erp.ix_sync_conflict_retention_cleanup;
        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT ck_conflict_snapshot_redaction_shape,
          DROP COLUMN "ParentLegalHold",
          ADD CONSTRAINT ck_conflict_snapshot_redaction_shape CHECK (
            ("RedactedAt" IS NULL AND "RetentionDaysApplied" IS NULL) OR
            (NOT "LegalHold" AND "DeviceSnapshot"='{}' AND "ServerSnapshot"='{}' AND
             "Status"='RESOLVED' AND "ResolvedAt" IS NOT NULL AND
             "RetentionDaysApplied" IS NOT NULL AND
             "RetentionDaysApplied" BETWEEN 1 AND 90 AND
             "RedactedAt">="ResolvedAt"+make_interval(days => "RetentionDaysApplied"))
          );
        CREATE INDEX ix_sync_conflict_retention_cleanup
          ON transport_erp.conflict_cases
             ("LegalHold","RedactedAt","Status","ResolvedAt");
        """);
}
