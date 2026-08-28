using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826090000_P1Stage4SyncRetentionRedaction")]
public partial class P1Stage4SyncRetentionRedaction : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations,
                   transport_erp.conflict_cases
          IN ACCESS EXCLUSIVE MODE;

        ALTER TABLE transport_erp.sync_operations
          ADD COLUMN "RedactedAt" timestamptz NULL,
          ADD CONSTRAINT ck_sync_payload_redaction_shape CHECK (
            "RedactedAt" IS NULL OR
            ("PayloadJson"='{}' AND "Status" IN ('SUCCEEDED','REJECTED','RESOLVED') AND
             "RedactedAt">="UpdatedAt"+INTERVAL '90 days')
          );
        CREATE INDEX ix_sync_operation_retention_cleanup
          ON transport_erp.sync_operations ("RedactedAt","Status","UpdatedAt");

        ALTER TABLE transport_erp.conflict_cases
          ADD COLUMN "RedactedAt" timestamptz NULL,
          ADD CONSTRAINT ck_conflict_snapshot_redaction_shape CHECK (
            "RedactedAt" IS NULL OR
            ("DeviceSnapshot"='{}' AND "ServerSnapshot"='{}' AND "Status"='RESOLVED' AND
             "ResolvedAt" IS NOT NULL AND "RedactedAt">="ResolvedAt"+INTERVAL '90 days')
          );
        CREATE INDEX ix_sync_conflict_retention_cleanup
          ON transport_erp.conflict_cases ("RedactedAt","Status","ResolvedAt");

        CREATE OR REPLACE FUNCTION transport_erp.enforce_sync_operation_device_binding()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE
          redaction_allowed boolean := false;
        BEGIN
          IF TG_OP='UPDATE' THEN
            IF OLD."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') AND
               (OLD."Status" IS DISTINCT FROM NEW."Status" OR
                OLD."UpdatedAt" IS DISTINCT FROM NEW."UpdatedAt") THEN
              RAISE EXCEPTION 'sync operation terminal retention timestamp is immutable';
            END IF;
            IF OLD."RedactedAt" IS DISTINCT FROM NEW."RedactedAt" THEN
              redaction_allowed :=
                OLD."RedactedAt" IS NULL AND NEW."RedactedAt" IS NOT NULL AND
                NEW."PayloadJson"='{}' AND
                OLD."Status"=NEW."Status" AND OLD."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') AND
                clock_timestamp()>=OLD."UpdatedAt"+INTERVAL '90 days' AND
                NEW."RedactedAt">=OLD."UpdatedAt"+INTERVAL '90 days' AND
                NEW."RedactedAt"<=clock_timestamp() AND
                (to_jsonb(OLD)-ARRAY['PayloadJson','RedactedAt'])=
                (to_jsonb(NEW)-ARRAY['PayloadJson','RedactedAt']);
              IF NOT redaction_allowed THEN
                RAISE EXCEPTION 'sync operation redaction transition denied';
              END IF;
            END IF;

            IF (OLD."PayloadJson" IS DISTINCT FROM NEW."PayloadJson" AND NOT redaction_allowed) OR
               OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR
               OLD."BranchId" IS DISTINCT FROM NEW."BranchId" OR
               OLD."DeviceId" IS DISTINCT FROM NEW."DeviceId" OR
               OLD."RegisteredDeviceId" IS DISTINCT FROM NEW."RegisteredDeviceId" OR
               OLD."RegisteredDeviceCredentialVersion" IS DISTINCT FROM NEW."RegisteredDeviceCredentialVersion" OR
               OLD."UserId" IS DISTINCT FROM NEW."UserId" OR
               OLD."ActionCode" IS DISTINCT FROM NEW."ActionCode" OR
               OLD."ProtocolVersion" IS DISTINCT FROM NEW."ProtocolVersion" OR
               OLD."OperationType" IS DISTINCT FROM NEW."OperationType" OR
               OLD."EntityType" IS DISTINCT FROM NEW."EntityType" OR
               OLD."EntityId" IS DISTINCT FROM NEW."EntityId" OR
               OLD."ClientOperationId" IS DISTINCT FROM NEW."ClientOperationId" OR
               OLD."PayloadHash" IS DISTINCT FROM NEW."PayloadHash" OR
               OLD."ClientOccurredAt" IS DISTINCT FROM NEW."ClientOccurredAt" OR
               OLD."BaseVersion" IS DISTINCT FROM NEW."BaseVersion" OR
               OLD."OperationCorrelationId" IS DISTINCT FROM NEW."OperationCorrelationId" OR
               OLD."RequestFingerprintVersion" IS DISTINCT FROM NEW."RequestFingerprintVersion" OR
               OLD."RequestFingerprintHash" IS DISTINCT FROM NEW."RequestFingerprintHash" OR
               OLD."ProofKeyVersion" IS DISTINCT FROM NEW."ProofKeyVersion" OR
               OLD."ProofKeyThumbprint" IS DISTINCT FROM NEW."ProofKeyThumbprint" OR
               OLD."AcceptedProofReplayId" IS DISTINCT FROM NEW."AcceptedProofReplayId" THEN
              RAISE EXCEPTION 'sync operation provenance is immutable';
            END IF;
            RETURN NEW;
          END IF;

          IF NEW."RedactedAt" IS NOT NULL THEN
            RAISE EXCEPTION 'new sync operation cannot be pre-redacted';
          END IF;
          IF NEW."RequestFingerprintVersion" IS NULL THEN
            RAISE EXCEPTION 'new sync operation requires accepted Stage4 proof replay';
          END IF;

          IF EXISTS (
            SELECT 1 FROM transport_erp.sync_operations o
            WHERE o."CompanyId"=NEW."CompanyId" AND o."DeviceId"=NEW."DeviceId"
              AND o."ClientOperationId"=NEW."ClientOperationId"
              AND o."RequestFingerprintVersion" IS NULL
          ) THEN
            RAISE unique_violation USING
              MESSAGE='sync operation Stage4 idempotency key collides with legacy row',
              CONSTRAINT='ux_sync_op_legacy_company_device_client';
          END IF;

          PERFORM 1
          FROM transport_erp.sync_proof_replays r
          WHERE r."Id"=NEW."AcceptedProofReplayId"
            AND r."CompanyId"=NEW."CompanyId"
            AND r."RegisteredDeviceId"=NEW."RegisteredDeviceId"
            AND r."DeviceId"=NEW."DeviceId"
            AND r."UserId"=NEW."UserId"
            AND r."BranchId"=NEW."BranchId"
            AND r."ProofKeyVersion"=NEW."ProofKeyVersion"
            AND r."ProofKeyThumbprint"=NEW."ProofKeyThumbprint";
          IF NOT FOUND THEN
            RAISE EXCEPTION 'sync operation accepted proof replay scope mismatch';
          END IF;
          RETURN NEW;
        END $body$;

        CREATE FUNCTION transport_erp.enforce_sync_conflict_redaction()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE
          redaction_allowed boolean := false;
        BEGIN
          IF TG_OP='INSERT' THEN
            IF NEW."RedactedAt" IS NOT NULL THEN
              RAISE EXCEPTION 'new sync conflict cannot be pre-redacted';
            END IF;
            RETURN NEW;
          END IF;

          IF OLD."Status"='RESOLVED' AND
             (OLD."Status" IS DISTINCT FROM NEW."Status" OR
              OLD."ResolvedAt" IS DISTINCT FROM NEW."ResolvedAt") THEN
            RAISE EXCEPTION 'sync conflict resolution retention timestamp is immutable';
          END IF;

          IF OLD."RedactedAt" IS DISTINCT FROM NEW."RedactedAt" OR
             OLD."DeviceSnapshot" IS DISTINCT FROM NEW."DeviceSnapshot" OR
             OLD."ServerSnapshot" IS DISTINCT FROM NEW."ServerSnapshot" THEN
            redaction_allowed :=
              OLD."RedactedAt" IS NULL AND NEW."RedactedAt" IS NOT NULL AND
              NEW."DeviceSnapshot"='{}' AND NEW."ServerSnapshot"='{}' AND
              OLD."Status"=NEW."Status" AND OLD."Status"='RESOLVED' AND
              OLD."ResolvedAt" IS NOT NULL AND NEW."ResolvedAt"=OLD."ResolvedAt" AND
              clock_timestamp()>=OLD."ResolvedAt"+INTERVAL '90 days' AND
              NEW."RedactedAt">=OLD."ResolvedAt"+INTERVAL '90 days' AND
              NEW."RedactedAt"<=clock_timestamp() AND
              EXISTS (
                SELECT 1 FROM transport_erp.sync_operations o
                WHERE o."Id"=OLD."SyncOperationId"
                  AND o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED')
                  AND clock_timestamp()>=o."UpdatedAt"+INTERVAL '90 days'
              ) AND
              (to_jsonb(OLD)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt'])=
              (to_jsonb(NEW)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt']);
            IF NOT redaction_allowed THEN
              RAISE EXCEPTION 'sync conflict redaction transition denied';
            END IF;
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_sync_conflict_redaction
          BEFORE INSERT OR UPDATE ON transport_erp.conflict_cases
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_conflict_redaction();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations,
                   transport_erp.conflict_cases
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations WHERE "RedactedAt" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.conflict_cases WHERE "RedactedAt" IS NOT NULL) THEN
            RAISE EXCEPTION 'STAGE4_RETENTION_DOWN_BLOCKED_REDACTED_DATA';
          END IF;
        END $body$;

        DROP TRIGGER trg_sync_conflict_redaction ON transport_erp.conflict_cases;
        DROP FUNCTION transport_erp.enforce_sync_conflict_redaction();

        CREATE OR REPLACE FUNCTION transport_erp.enforce_sync_operation_device_binding()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          IF TG_OP='UPDATE' THEN
            IF OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR
               OLD."BranchId" IS DISTINCT FROM NEW."BranchId" OR
               OLD."DeviceId" IS DISTINCT FROM NEW."DeviceId" OR
               OLD."RegisteredDeviceId" IS DISTINCT FROM NEW."RegisteredDeviceId" OR
               OLD."RegisteredDeviceCredentialVersion" IS DISTINCT FROM NEW."RegisteredDeviceCredentialVersion" OR
               OLD."UserId" IS DISTINCT FROM NEW."UserId" OR
               OLD."ActionCode" IS DISTINCT FROM NEW."ActionCode" OR
               OLD."ProtocolVersion" IS DISTINCT FROM NEW."ProtocolVersion" OR
               OLD."OperationType" IS DISTINCT FROM NEW."OperationType" OR
               OLD."EntityType" IS DISTINCT FROM NEW."EntityType" OR
               OLD."EntityId" IS DISTINCT FROM NEW."EntityId" OR
               OLD."ClientOperationId" IS DISTINCT FROM NEW."ClientOperationId" OR
               OLD."PayloadJson" IS DISTINCT FROM NEW."PayloadJson" OR
               OLD."PayloadHash" IS DISTINCT FROM NEW."PayloadHash" OR
               OLD."ClientOccurredAt" IS DISTINCT FROM NEW."ClientOccurredAt" OR
               OLD."BaseVersion" IS DISTINCT FROM NEW."BaseVersion" OR
               OLD."OperationCorrelationId" IS DISTINCT FROM NEW."OperationCorrelationId" OR
               OLD."RequestFingerprintVersion" IS DISTINCT FROM NEW."RequestFingerprintVersion" OR
               OLD."RequestFingerprintHash" IS DISTINCT FROM NEW."RequestFingerprintHash" OR
               OLD."ProofKeyVersion" IS DISTINCT FROM NEW."ProofKeyVersion" OR
               OLD."ProofKeyThumbprint" IS DISTINCT FROM NEW."ProofKeyThumbprint" OR
               OLD."AcceptedProofReplayId" IS DISTINCT FROM NEW."AcceptedProofReplayId" THEN
              RAISE EXCEPTION 'sync operation provenance is immutable';
            END IF;
            RETURN NEW;
          END IF;

          IF NEW."RequestFingerprintVersion" IS NULL THEN
            RAISE EXCEPTION 'new sync operation requires accepted Stage4 proof replay';
          END IF;

          IF EXISTS (
            SELECT 1 FROM transport_erp.sync_operations o
            WHERE o."CompanyId"=NEW."CompanyId" AND o."DeviceId"=NEW."DeviceId"
              AND o."ClientOperationId"=NEW."ClientOperationId"
              AND o."RequestFingerprintVersion" IS NULL
          ) THEN
            RAISE unique_violation USING
              MESSAGE='sync operation Stage4 idempotency key collides with legacy row',
              CONSTRAINT='ux_sync_op_legacy_company_device_client';
          END IF;

          PERFORM 1
          FROM transport_erp.sync_proof_replays r
          WHERE r."Id"=NEW."AcceptedProofReplayId"
            AND r."CompanyId"=NEW."CompanyId"
            AND r."RegisteredDeviceId"=NEW."RegisteredDeviceId"
            AND r."DeviceId"=NEW."DeviceId"
            AND r."UserId"=NEW."UserId"
            AND r."BranchId"=NEW."BranchId"
            AND r."ProofKeyVersion"=NEW."ProofKeyVersion"
            AND r."ProofKeyThumbprint"=NEW."ProofKeyThumbprint";
          IF NOT FOUND THEN
            RAISE EXCEPTION 'sync operation accepted proof replay scope mismatch';
          END IF;
          RETURN NEW;
        END $body$;

        DROP INDEX transport_erp.ix_sync_conflict_retention_cleanup;
        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT ck_conflict_snapshot_redaction_shape,
          DROP COLUMN "RedactedAt";
        DROP INDEX transport_erp.ix_sync_operation_retention_cleanup;
        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT ck_sync_payload_redaction_shape,
          DROP COLUMN "RedactedAt";
        """);
}
