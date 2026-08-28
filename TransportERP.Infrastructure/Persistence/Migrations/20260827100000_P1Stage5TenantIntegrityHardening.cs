using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260827100000_P1Stage5TenantIntegrityHardening")]
public partial class P1Stage5TenantIntegrityHardening : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.conflict_cases,
                   transport_erp.sync_operations,
                   transport_erp.registered_devices,
                   transport_erp.registered_device_proof_key_challenges,
                   transport_erp.registered_device_proof_key_changes
          IN ACCESS EXCLUSIVE MODE;

        ALTER TABLE transport_erp.sync_operations
          ADD COLUMN "LegalHold" boolean NOT NULL DEFAULT false,
          ADD COLUMN "RetentionDaysApplied" integer NULL;
        UPDATE transport_erp.sync_operations
          SET "RetentionDaysApplied"=90
          WHERE "RedactedAt" IS NOT NULL;
        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT ck_sync_payload_redaction_shape,
          ADD CONSTRAINT ck_sync_payload_redaction_shape CHECK (
            ("RedactedAt" IS NULL AND "RetentionDaysApplied" IS NULL) OR
            (NOT "LegalHold" AND "PayloadJson"='{}' AND
             ("Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
              ("Status"='FAILED' AND "NextRetryAt" IS NULL)) AND
             "RetentionDaysApplied" IS NOT NULL AND
             "RetentionDaysApplied" BETWEEN 1 AND 90 AND
             "RedactedAt">="UpdatedAt"+make_interval(days => "RetentionDaysApplied"))
          );
        DROP INDEX transport_erp.ix_sync_operation_retention_cleanup;
        CREATE INDEX ix_sync_operation_retention_cleanup
          ON transport_erp.sync_operations ("LegalHold","RedactedAt","Status","UpdatedAt");

        ALTER TABLE transport_erp.conflict_cases
          ADD COLUMN "LegalHold" boolean NOT NULL DEFAULT false,
          ADD COLUMN "RetentionDaysApplied" integer NULL;
        UPDATE transport_erp.conflict_cases
          SET "RetentionDaysApplied"=90
          WHERE "RedactedAt" IS NOT NULL;
        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT ck_conflict_snapshot_redaction_shape,
          ADD CONSTRAINT ck_conflict_snapshot_redaction_shape CHECK (
            ("RedactedAt" IS NULL AND "RetentionDaysApplied" IS NULL) OR
            (NOT "LegalHold" AND "DeviceSnapshot"='{}' AND "ServerSnapshot"='{}' AND
             "Status"='RESOLVED' AND "ResolvedAt" IS NOT NULL AND
             "RetentionDaysApplied" IS NOT NULL AND
             "RetentionDaysApplied" BETWEEN 1 AND 90 AND
             "RedactedAt">="ResolvedAt"+make_interval(days => "RetentionDaysApplied"))
          );
        DROP INDEX transport_erp.ix_sync_conflict_retention_cleanup;
        CREATE INDEX ix_sync_conflict_retention_cleanup
          ON transport_erp.conflict_cases ("LegalHold","RedactedAt","Status","ResolvedAt");

        ALTER FUNCTION transport_erp.enforce_sync_operation_device_binding()
          RENAME TO enforce_sync_operation_device_binding_stage4_backup;
        CREATE FUNCTION transport_erp.enforce_sync_operation_device_binding()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE redaction_allowed boolean := false;
        BEGIN
          IF TG_OP='UPDATE' THEN
            IF OLD."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') AND
               (OLD."Status" IS DISTINCT FROM NEW."Status" OR
                OLD."UpdatedAt" IS DISTINCT FROM NEW."UpdatedAt") THEN
              RAISE EXCEPTION 'sync operation terminal retention timestamp is immutable';
            END IF;
            IF OLD."RedactedAt" IS DISTINCT FROM NEW."RedactedAt" OR
               OLD."RetentionDaysApplied" IS DISTINCT FROM NEW."RetentionDaysApplied" OR
               OLD."PayloadJson" IS DISTINCT FROM NEW."PayloadJson" THEN
              redaction_allowed :=
                OLD."RedactedAt" IS NULL AND NEW."RedactedAt" IS NOT NULL AND
                NOT OLD."LegalHold" AND NOT NEW."LegalHold" AND
                NEW."PayloadJson"='{}' AND OLD."Status"=NEW."Status" AND
                (OLD."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                 (OLD."Status"='FAILED' AND OLD."NextRetryAt" IS NULL)) AND
                OLD."RetentionDaysApplied" IS NULL AND NEW."RetentionDaysApplied" BETWEEN 1 AND 90 AND
                clock_timestamp()>=OLD."UpdatedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
                NEW."RedactedAt">=OLD."UpdatedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
                NEW."RedactedAt"<=clock_timestamp() AND
                (to_jsonb(OLD)-ARRAY['PayloadJson','RedactedAt','RetentionDaysApplied'])=
                (to_jsonb(NEW)-ARRAY['PayloadJson','RedactedAt','RetentionDaysApplied']);
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

          IF NEW."RedactedAt" IS NOT NULL OR NEW."RetentionDaysApplied" IS NOT NULL THEN
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
          PERFORM 1 FROM transport_erp.sync_proof_replays r
          WHERE r."Id"=NEW."AcceptedProofReplayId" AND r."CompanyId"=NEW."CompanyId"
            AND r."RegisteredDeviceId"=NEW."RegisteredDeviceId" AND r."DeviceId"=NEW."DeviceId"
            AND r."UserId"=NEW."UserId" AND r."BranchId"=NEW."BranchId"
            AND r."ProofKeyVersion"=NEW."ProofKeyVersion"
            AND r."ProofKeyThumbprint"=NEW."ProofKeyThumbprint";
          IF NOT FOUND THEN RAISE EXCEPTION 'sync operation accepted proof replay scope mismatch'; END IF;
          RETURN NEW;
        END $body$;
        DROP TRIGGER trg_sync_operations_device_binding ON transport_erp.sync_operations;
        CREATE TRIGGER trg_sync_operations_device_binding
          BEFORE INSERT OR UPDATE ON transport_erp.sync_operations
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_operation_device_binding();

        ALTER FUNCTION transport_erp.enforce_sync_conflict_redaction()
          RENAME TO enforce_sync_conflict_redaction_stage4_backup;
        CREATE FUNCTION transport_erp.enforce_sync_conflict_redaction()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE redaction_allowed boolean := false;
        BEGIN
          IF TG_OP='INSERT' THEN
            IF NEW."RedactedAt" IS NOT NULL OR NEW."RetentionDaysApplied" IS NOT NULL THEN
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
             OLD."RetentionDaysApplied" IS DISTINCT FROM NEW."RetentionDaysApplied" OR
             OLD."DeviceSnapshot" IS DISTINCT FROM NEW."DeviceSnapshot" OR
             OLD."ServerSnapshot" IS DISTINCT FROM NEW."ServerSnapshot" THEN
            redaction_allowed :=
              OLD."RedactedAt" IS NULL AND NEW."RedactedAt" IS NOT NULL AND
              NOT OLD."LegalHold" AND NOT NEW."LegalHold" AND
              NEW."DeviceSnapshot"='{}' AND NEW."ServerSnapshot"='{}' AND
              OLD."Status"=NEW."Status" AND OLD."Status"='RESOLVED' AND
              OLD."ResolvedAt" IS NOT NULL AND NEW."ResolvedAt"=OLD."ResolvedAt" AND
              OLD."RetentionDaysApplied" IS NULL AND NEW."RetentionDaysApplied" BETWEEN 1 AND 90 AND
              clock_timestamp()>=OLD."ResolvedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
              NEW."RedactedAt">=OLD."ResolvedAt"+make_interval(days => NEW."RetentionDaysApplied") AND
              NEW."RedactedAt"<=clock_timestamp() AND
              EXISTS (SELECT 1 FROM transport_erp.sync_operations o
                WHERE o."Id"=OLD."SyncOperationId" AND NOT o."LegalHold"
                  AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                       (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
                  AND clock_timestamp()>=o."UpdatedAt"+make_interval(days => NEW."RetentionDaysApplied")) AND
              (to_jsonb(OLD)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt','RetentionDaysApplied'])=
              (to_jsonb(NEW)-ARRAY['DeviceSnapshot','ServerSnapshot','RedactedAt','RetentionDaysApplied']);
            IF NOT redaction_allowed THEN RAISE EXCEPTION 'sync conflict redaction transition denied'; END IF;
          END IF;
          RETURN NEW;
        END $body$;
        DROP TRIGGER trg_sync_conflict_redaction ON transport_erp.conflict_cases;
        CREATE TRIGGER trg_sync_conflict_redaction
          BEFORE INSERT OR UPDATE ON transport_erp.conflict_cases
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_conflict_redaction();

        DO $body$
        BEGIN
          IF EXISTS (
            SELECT 1
            FROM transport_erp.conflict_cases c
            LEFT JOIN transport_erp.sync_operations original
              ON original."Id"=c."SyncOperationId"
             AND original."CompanyId"=c."CompanyId"
             AND original."BranchId" IS NOT DISTINCT FROM c."BranchId"
            LEFT JOIN transport_erp.branches b
              ON b."Id"=c."BranchId" AND b."CompanyId"=c."CompanyId"
            LEFT JOIN transport_erp.sync_operations replacement
              ON replacement."Id"=c."ReplacedByOperationId"
             AND replacement."CompanyId"=c."CompanyId"
             AND replacement."BranchId" IS NOT DISTINCT FROM c."BranchId"
            WHERE original."Id" IS NULL
               OR (c."BranchId" IS NOT NULL AND b."Id" IS NULL)
               OR (c."ReplacedByOperationId" IS NOT NULL AND replacement."Id" IS NULL)
          ) THEN
            RAISE EXCEPTION 'P1_STAGE5_CONFLICT_TENANT_HARDENING_BLOCKED_INVALID_DATA';
          END IF;
        END $body$;

        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT "FK_conflict_cases_branches_BranchId",
          DROP CONSTRAINT "FK_conflict_cases_sync_operations_SyncOperationId",
          DROP CONSTRAINT "FK_conflict_cases_sync_operations_ReplacedByOperationId";

        ALTER TABLE transport_erp.sync_operations
          ADD CONSTRAINT "AK_sync_operations_Id_CompanyId" UNIQUE ("Id","CompanyId");

        ALTER TABLE transport_erp.conflict_cases
          ADD CONSTRAINT "FK_conflict_cases_branches_BranchId_CompanyId"
            FOREIGN KEY ("BranchId","CompanyId")
            REFERENCES transport_erp.branches ("Id","CompanyId") ON DELETE RESTRICT,
          ADD CONSTRAINT "FK_conflict_cases_sync_operations_SyncOperationId_CompanyId"
            FOREIGN KEY ("SyncOperationId","CompanyId")
            REFERENCES transport_erp.sync_operations ("Id","CompanyId") ON DELETE RESTRICT,
          ADD CONSTRAINT "FK_conflict_cases_sync_operations_ReplacedByOperationId_CompanyId"
            FOREIGN KEY ("ReplacedByOperationId","CompanyId")
            REFERENCES transport_erp.sync_operations ("Id","CompanyId") ON DELETE RESTRICT;

        DROP INDEX transport_erp."IX_conflict_cases_BranchId";
        CREATE INDEX "IX_conflict_cases_BranchId_CompanyId"
          ON transport_erp.conflict_cases ("BranchId","CompanyId");
        DROP INDEX transport_erp."IX_conflict_cases_ReplacedByOperationId";
        CREATE INDEX "IX_conflict_cases_ReplacedByOperationId_CompanyId"
          ON transport_erp.conflict_cases ("ReplacedByOperationId","CompanyId");
        DROP INDEX transport_erp."IX_conflict_cases_SyncOperationId";
        CREATE UNIQUE INDEX "IX_conflict_cases_SyncOperationId_CompanyId"
          ON transport_erp.conflict_cases ("SyncOperationId","CompanyId");

        CREATE FUNCTION transport_erp.fn_conflict_case_tenant_scope_guard()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM 1 FROM transport_erp.sync_operations o
          WHERE o."Id"=NEW."SyncOperationId" AND o."CompanyId"=NEW."CompanyId"
            AND o."BranchId" IS NOT DISTINCT FROM NEW."BranchId";
          IF NOT FOUND THEN
            RAISE EXCEPTION 'sync conflict original operation tenant scope mismatch';
          END IF;
          IF NEW."ReplacedByOperationId" IS NOT NULL THEN
            PERFORM 1 FROM transport_erp.sync_operations o
            WHERE o."Id"=NEW."ReplacedByOperationId" AND o."CompanyId"=NEW."CompanyId"
              AND o."BranchId" IS NOT DISTINCT FROM NEW."BranchId";
            IF NOT FOUND THEN
              RAISE EXCEPTION 'sync conflict replacement operation tenant scope mismatch';
            END IF;
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_conflict_case_tenant_scope_guard
          BEFORE INSERT OR UPDATE OF "SyncOperationId","ReplacedByOperationId","CompanyId","BranchId"
          ON transport_erp.conflict_cases FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_conflict_case_tenant_scope_guard();

        CREATE FUNCTION transport_erp.fn_key_challenge_material_reuse_guard()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE current_thumbprint varchar(43);
        BEGIN
          IF NEW."ChangeType" NOT IN ('ROTATE','RECOVER') THEN
            RETURN NEW;
          END IF;

          SELECT d."ProofKeyThumbprint" INTO current_thumbprint
          FROM transport_erp.registered_devices d
          WHERE d."Id"=NEW."RegisteredDeviceId"
            AND d."CompanyId"=NEW."CompanyId"
            AND d."DeviceId"=NEW."DeviceId"
          FOR UPDATE;
          IF NOT FOUND THEN
            RAISE EXCEPTION 'proof key challenge device scope mismatch';
          END IF;

          IF NEW."NewProofKeyThumbprint" IS NOT DISTINCT FROM current_thumbprint
             OR EXISTS (
               SELECT 1 FROM transport_erp.registered_device_proof_key_changes h
               WHERE h."RegisteredDeviceId"=NEW."RegisteredDeviceId"
                 AND h."CompanyId"=NEW."CompanyId"
                 AND (h."NewProofKeyThumbprint"=NEW."NewProofKeyThumbprint"
                      OR h."PreviousProofKeyThumbprint"=NEW."NewProofKeyThumbprint"))
             OR EXISTS (
               SELECT 1 FROM transport_erp.registered_device_proof_key_challenges pending
               WHERE pending."RegisteredDeviceId"=NEW."RegisteredDeviceId"
                 AND pending."CompanyId"=NEW."CompanyId"
                 AND pending."Id"<>NEW."Id"
                 AND pending."NewProofKeyThumbprint"=NEW."NewProofKeyThumbprint"
                 AND pending."ConsumedAt" IS NULL
                 AND pending."ExpiresAt">clock_timestamp()) THEN
            RAISE EXCEPTION 'proof key material reuse denied';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_key_challenge_material_reuse_guard
          BEFORE INSERT ON transport_erp.registered_device_proof_key_challenges
          FOR EACH ROW EXECUTE FUNCTION transport_erp.fn_key_challenge_material_reuse_guard();

        CREATE FUNCTION transport_erp.fn_key_change_material_reuse_guard()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE current_thumbprint varchar(43);
        BEGIN
          IF NEW."ChangeType" NOT IN ('ROTATE','RECOVER') THEN
            RETURN NEW;
          END IF;

          SELECT d."ProofKeyThumbprint" INTO current_thumbprint
          FROM transport_erp.registered_devices d
          WHERE d."Id"=NEW."RegisteredDeviceId"
            AND d."CompanyId"=NEW."CompanyId"
            AND d."DeviceId"=NEW."DeviceId"
          FOR UPDATE;
          IF NOT FOUND THEN
            RAISE EXCEPTION 'proof key change device scope mismatch';
          END IF;

          IF NEW."NewProofKeyThumbprint" IS NOT DISTINCT FROM current_thumbprint
             OR NEW."NewProofKeyThumbprint" IS NOT DISTINCT FROM NEW."PreviousProofKeyThumbprint"
             OR EXISTS (
               SELECT 1 FROM transport_erp.registered_device_proof_key_changes h
               WHERE h."RegisteredDeviceId"=NEW."RegisteredDeviceId"
                 AND h."CompanyId"=NEW."CompanyId"
                 AND (h."NewProofKeyThumbprint"=NEW."NewProofKeyThumbprint"
                      OR h."PreviousProofKeyThumbprint"=NEW."NewProofKeyThumbprint")) THEN
            RAISE EXCEPTION 'proof key material reuse denied';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_key_change_material_reuse_guard
          BEFORE INSERT ON transport_erp.registered_device_proof_key_changes
          FOR EACH ROW EXECUTE FUNCTION transport_erp.fn_key_change_material_reuse_guard();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.conflict_cases,
                   transport_erp.sync_operations,
                   transport_erp.registered_device_proof_key_challenges,
                   transport_erp.registered_device_proof_key_changes
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations
                     WHERE "RedactedAt" IS NOT NULL OR "LegalHold" OR "RetentionDaysApplied" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.conflict_cases
                        WHERE "RedactedAt" IS NOT NULL OR "LegalHold" OR "RetentionDaysApplied" IS NOT NULL) THEN
            RAISE EXCEPTION 'P1_STAGE5_HARDENING_DOWN_BLOCKED_RETENTION_DATA';
          END IF;
        END $body$;

        DROP TRIGGER trg_key_change_material_reuse_guard
          ON transport_erp.registered_device_proof_key_changes;
        DROP FUNCTION transport_erp.fn_key_change_material_reuse_guard();
        DROP TRIGGER trg_key_challenge_material_reuse_guard
          ON transport_erp.registered_device_proof_key_challenges;
        DROP FUNCTION transport_erp.fn_key_challenge_material_reuse_guard();

        DROP TRIGGER trg_conflict_case_tenant_scope_guard ON transport_erp.conflict_cases;
        DROP FUNCTION transport_erp.fn_conflict_case_tenant_scope_guard();

        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT "FK_conflict_cases_sync_operations_ReplacedByOperationId_CompanyId",
          DROP CONSTRAINT "FK_conflict_cases_sync_operations_SyncOperationId_CompanyId",
          DROP CONSTRAINT "FK_conflict_cases_branches_BranchId_CompanyId";
        DROP INDEX transport_erp."IX_conflict_cases_ReplacedByOperationId_CompanyId";
        DROP INDEX transport_erp."IX_conflict_cases_SyncOperationId_CompanyId";
        CREATE UNIQUE INDEX "IX_conflict_cases_SyncOperationId"
          ON transport_erp.conflict_cases ("SyncOperationId");
        DROP INDEX transport_erp."IX_conflict_cases_BranchId_CompanyId";
        CREATE INDEX "IX_conflict_cases_BranchId"
          ON transport_erp.conflict_cases ("BranchId");
        CREATE INDEX "IX_conflict_cases_ReplacedByOperationId"
          ON transport_erp.conflict_cases ("ReplacedByOperationId");

        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT "AK_sync_operations_Id_CompanyId";

        ALTER TABLE transport_erp.conflict_cases
          ADD CONSTRAINT "FK_conflict_cases_branches_BranchId"
            FOREIGN KEY ("BranchId") REFERENCES transport_erp.branches ("Id") ON DELETE RESTRICT,
          ADD CONSTRAINT "FK_conflict_cases_sync_operations_SyncOperationId"
            FOREIGN KEY ("SyncOperationId") REFERENCES transport_erp.sync_operations ("Id") ON DELETE RESTRICT,
          ADD CONSTRAINT "FK_conflict_cases_sync_operations_ReplacedByOperationId"
            FOREIGN KEY ("ReplacedByOperationId") REFERENCES transport_erp.sync_operations ("Id") ON DELETE RESTRICT;

        DROP TRIGGER trg_sync_conflict_redaction ON transport_erp.conflict_cases;
        DROP FUNCTION transport_erp.enforce_sync_conflict_redaction();
        ALTER FUNCTION transport_erp.enforce_sync_conflict_redaction_stage4_backup()
          RENAME TO enforce_sync_conflict_redaction;
        CREATE TRIGGER trg_sync_conflict_redaction
          BEFORE INSERT OR UPDATE ON transport_erp.conflict_cases
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_conflict_redaction();

        DROP TRIGGER trg_sync_operations_device_binding ON transport_erp.sync_operations;
        DROP FUNCTION transport_erp.enforce_sync_operation_device_binding();
        ALTER FUNCTION transport_erp.enforce_sync_operation_device_binding_stage4_backup()
          RENAME TO enforce_sync_operation_device_binding;
        CREATE TRIGGER trg_sync_operations_device_binding
          BEFORE INSERT OR UPDATE ON transport_erp.sync_operations
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_operation_device_binding();

        DROP INDEX transport_erp.ix_sync_conflict_retention_cleanup;
        ALTER TABLE transport_erp.conflict_cases
          DROP CONSTRAINT ck_conflict_snapshot_redaction_shape,
          ADD CONSTRAINT ck_conflict_snapshot_redaction_shape CHECK (
            "RedactedAt" IS NULL OR
            ("DeviceSnapshot"='{}' AND "ServerSnapshot"='{}' AND "Status"='RESOLVED' AND
             "ResolvedAt" IS NOT NULL AND "RedactedAt">="ResolvedAt"+INTERVAL '90 days')
          ),
          DROP COLUMN "RetentionDaysApplied",
          DROP COLUMN "LegalHold";
        CREATE INDEX ix_sync_conflict_retention_cleanup
          ON transport_erp.conflict_cases ("RedactedAt","Status","ResolvedAt");

        DROP INDEX transport_erp.ix_sync_operation_retention_cleanup;
        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT ck_sync_payload_redaction_shape,
          ADD CONSTRAINT ck_sync_payload_redaction_shape CHECK (
            "RedactedAt" IS NULL OR
            ("PayloadJson"='{}' AND "Status" IN ('SUCCEEDED','REJECTED','RESOLVED') AND
             "RedactedAt">="UpdatedAt"+INTERVAL '90 days')
          ),
          DROP COLUMN "RetentionDaysApplied",
          DROP COLUMN "LegalHold";
        CREATE INDEX ix_sync_operation_retention_cleanup
          ON transport_erp.sync_operations ("RedactedAt","Status","UpdatedAt");
        """);
}
