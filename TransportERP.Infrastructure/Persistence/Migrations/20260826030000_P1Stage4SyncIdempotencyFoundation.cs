using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826030000_P1Stage4SyncIdempotencyFoundation")]
public partial class P1Stage4SyncIdempotencyFoundation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF NOT EXISTS (
            SELECT 1
            FROM pg_class i
            JOIN pg_namespace n ON n.oid=i.relnamespace
            JOIN pg_index x ON x.indexrelid=i.oid
            WHERE n.nspname='transport_erp'
              AND i.relname='IX_sync_operations_DeviceId_ClientOperationId'
              AND x.indrelid='transport_erp.sync_operations'::regclass
              AND x.indisunique
              AND x.indisvalid
              AND x.indisready
              AND x.indpred IS NULL
              AND x.indnkeyatts=2
              AND (
                SELECT array_agg(a.attname ORDER BY k.ordinality)
                FROM unnest(x.indkey::smallint[]) WITH ORDINALITY AS k(attnum, ordinality)
                JOIN pg_attribute a ON a.attrelid=x.indrelid AND a.attnum=k.attnum
              )=ARRAY['DeviceId','ClientOperationId']::name[]
          ) THEN
            RAISE EXCEPTION 'STAGE4_UP_LEGACY_INDEX_MISSING_OR_DRIFT';
          END IF;
        END $body$;

        ALTER TABLE transport_erp.registered_device_assignments
          ADD CONSTRAINT ux_device_assignment_proof_scope
          UNIQUE ("Id","RegisteredDeviceId","CompanyId","UserId","BranchId");

        ALTER TABLE transport_erp.audit_events
          ADD COLUMN "OperationCorrelationId" uuid NULL;
        CREATE INDEX ix_audit_event_operation_correlation
          ON transport_erp.audit_events ("OperationCorrelationId");

        ALTER TABLE transport_erp.sync_operations
          ALTER COLUMN "EntityId" DROP NOT NULL,
          ADD COLUMN "ResultEntityId" uuid NULL,
          ADD COLUMN "ActionCode" varchar(120) NULL,
          ADD COLUMN "ProtocolVersion" varchar(20) NULL,
          ADD COLUMN "OperationCorrelationId" uuid NULL,
          ADD COLUMN "RequestFingerprintVersion" varchar(16) NULL,
          ADD COLUMN "RequestFingerprintHash" bytea NULL,
          ADD COLUMN "ProofKeyVersion" integer NULL,
          ADD COLUMN "ProofKeyThumbprint" varchar(43) NULL,
          ADD COLUMN "AcceptedProofReplayId" uuid NULL,
          ADD CONSTRAINT ck_sync_stage4_contract_bundle CHECK (
            ("ActionCode" IS NULL AND "ProtocolVersion" IS NULL AND "OperationCorrelationId" IS NULL AND
             "RequestFingerprintVersion" IS NULL AND "RequestFingerprintHash" IS NULL AND
             "ProofKeyVersion" IS NULL AND "ProofKeyThumbprint" IS NULL AND "AcceptedProofReplayId" IS NULL)
            OR
            ("RequestFingerprintVersion" IS NOT NULL AND "RequestFingerprintVersion"='fp-v1' AND
             "ProtocolVersion" IS NOT NULL AND "ProtocolVersion"='sync-v1' AND
             "RegisteredDeviceId" IS NOT NULL AND "BranchId" IS NOT NULL AND "ActionCode" IS NOT NULL AND
             "OperationCorrelationId" IS NOT NULL AND
             "OperationCorrelationId"<>'00000000-0000-0000-0000-000000000000'::uuid AND
             "RequestFingerprintHash" IS NOT NULL AND octet_length("RequestFingerprintHash")=32 AND
             "ProofKeyVersion" IS NOT NULL AND "ProofKeyVersion">=1 AND
             "ProofKeyThumbprint" IS NOT NULL AND length("ProofKeyThumbprint")=43 AND
             "AcceptedProofReplayId" IS NOT NULL)
          );

        CREATE TABLE transport_erp.sync_proof_nonces (
          "Id" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "RegisteredDeviceId" uuid NOT NULL,
          "DeviceId" varchar(120) NOT NULL,
          "ProofKeyVersion" integer NOT NULL,
          "NonceHash" bytea NOT NULL,
          "IssuedAt" timestamptz NOT NULL,
          "ExpiresAt" timestamptz NOT NULL,
          CONSTRAINT pk_sync_proof_nonces PRIMARY KEY ("Id"),
          CONSTRAINT ux_sync_nonce_scope UNIQUE
            ("Id","CompanyId","RegisteredDeviceId","DeviceId","ProofKeyVersion"),
          CONSTRAINT fk_sync_nonce_registered_device
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT,
          CONSTRAINT ck_sync_nonce_key_version CHECK ("ProofKeyVersion">=1),
          CONSTRAINT ck_sync_nonce_hash_len CHECK (octet_length("NonceHash")=32),
          CONSTRAINT ck_sync_nonce_window CHECK ("ExpiresAt">"IssuedAt")
        );
        CREATE UNIQUE INDEX ux_sync_nonce_hash ON transport_erp.sync_proof_nonces ("NonceHash");
        CREATE INDEX ix_sync_nonce_device_key_expiry
          ON transport_erp.sync_proof_nonces ("RegisteredDeviceId","ProofKeyVersion","ExpiresAt");
        CREATE INDEX ix_sync_nonce_expiry ON transport_erp.sync_proof_nonces ("ExpiresAt");
        CREATE INDEX "IX_sync_proof_nonces_RegisteredDeviceId_CompanyId_DeviceId"
          ON transport_erp.sync_proof_nonces ("RegisteredDeviceId","CompanyId","DeviceId");

        CREATE TABLE transport_erp.sync_proof_replays (
          "Id" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "RegisteredDeviceId" uuid NOT NULL,
          "DeviceId" varchar(120) NOT NULL,
          "DeviceAssignmentId" uuid NOT NULL,
          "UserId" uuid NOT NULL,
          "BranchId" uuid NOT NULL,
          "ProofKeyVersion" integer NOT NULL,
          "ProofKeyThumbprint" varchar(43) NOT NULL,
          "JtiHash" bytea NOT NULL,
          "HtuHash" bytea NOT NULL,
          "HttpMethod" varchar(8) NOT NULL,
          "NonceRecordId" uuid NOT NULL,
          "IssuedAt" timestamptz NOT NULL,
          "FirstSeenAt" timestamptz NOT NULL,
          "ExpiresAt" timestamptz NOT NULL,
          "AttemptCorrelationId" uuid NOT NULL,
          CONSTRAINT pk_sync_proof_replays PRIMARY KEY ("Id"),
          CONSTRAINT fk_sync_replay_registered_device
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT,
          CONSTRAINT fk_sync_replay_assignment_scope
            FOREIGN KEY ("DeviceAssignmentId","RegisteredDeviceId","CompanyId","UserId","BranchId")
            REFERENCES transport_erp.registered_device_assignments
              ("Id","RegisteredDeviceId","CompanyId","UserId","BranchId") ON DELETE RESTRICT,
          CONSTRAINT fk_sync_replay_nonce_scope
            FOREIGN KEY ("NonceRecordId","CompanyId","RegisteredDeviceId","DeviceId","ProofKeyVersion")
            REFERENCES transport_erp.sync_proof_nonces
              ("Id","CompanyId","RegisteredDeviceId","DeviceId","ProofKeyVersion") ON DELETE RESTRICT,
          CONSTRAINT ck_sync_replay_key_version CHECK ("ProofKeyVersion">=1),
          CONSTRAINT ck_sync_replay_hash_len CHECK
            (octet_length("JtiHash")=32 AND octet_length("HtuHash")=32 AND
             char_length("ProofKeyThumbprint")=43),
          CONSTRAINT ck_sync_replay_method CHECK ("HttpMethod"='POST'),
          CONSTRAINT ck_sync_replay_window CHECK
            ("ExpiresAt">"FirstSeenAt" AND
             "FirstSeenAt">="IssuedAt"-INTERVAL '30 seconds' AND
             "FirstSeenAt"<="IssuedAt"+INTERVAL '120 seconds')
        );
        CREATE UNIQUE INDEX ux_sync_replay_device_key_jti
          ON transport_erp.sync_proof_replays ("RegisteredDeviceId","ProofKeyVersion","JtiHash");
        CREATE INDEX ix_sync_replay_expiry ON transport_erp.sync_proof_replays ("ExpiresAt");
        CREATE INDEX ix_sync_replay_nonce ON transport_erp.sync_proof_replays ("NonceRecordId");
        CREATE INDEX "IX_sync_proof_replays_RegisteredDeviceId_CompanyId_DeviceId"
          ON transport_erp.sync_proof_replays ("RegisteredDeviceId","CompanyId","DeviceId");
        CREATE INDEX "IX_sync_proof_replays_DeviceAssignmentId_RegisteredDeviceId_CompanyId_UserId_BranchId"
          ON transport_erp.sync_proof_replays
            ("DeviceAssignmentId","RegisteredDeviceId","CompanyId","UserId","BranchId");
        CREATE INDEX "IX_sync_proof_replays_NonceRecordId_CompanyId_RegisteredDeviceId_DeviceId_ProofKeyVersion"
          ON transport_erp.sync_proof_replays
            ("NonceRecordId","CompanyId","RegisteredDeviceId","DeviceId","ProofKeyVersion");

        CREATE FUNCTION transport_erp.fn_sync_replay_append_only() RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          RAISE EXCEPTION 'sync proof replay is append-only';
        END $body$;
        CREATE TRIGGER trg_sync_replay_append_only
          BEFORE UPDATE ON transport_erp.sync_proof_replays
          FOR EACH ROW EXECUTE FUNCTION transport_erp.fn_sync_replay_append_only();

        DROP INDEX transport_erp."IX_sync_operations_DeviceId_ClientOperationId";
        CREATE UNIQUE INDEX ux_sync_op_registered_device_client
          ON transport_erp.sync_operations ("CompanyId","RegisteredDeviceId","ClientOperationId")
          WHERE "RegisteredDeviceId" IS NOT NULL AND "RequestFingerprintVersion"='fp-v1';
        CREATE UNIQUE INDEX ux_sync_op_legacy_company_device_client
          ON transport_erp.sync_operations ("CompanyId","DeviceId","ClientOperationId")
          WHERE "RequestFingerprintVersion" IS NULL;
        CREATE INDEX ix_sync_op_accepted_proof
          ON transport_erp.sync_operations ("AcceptedProofReplayId");

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
        DROP TRIGGER IF EXISTS trg_sync_operations_user_scope ON transport_erp.sync_operations;
        CREATE TRIGGER trg_sync_operations_user_scope
          BEFORE INSERT OR UPDATE OF "UserId","CompanyId","BranchId"
          ON transport_erp.sync_operations FOR EACH ROW
          EXECUTE FUNCTION transport_erp.enforce_sync_operation_user_scope();
        DROP TRIGGER IF EXISTS trg_sync_operations_device_binding ON transport_erp.sync_operations;
        CREATE TRIGGER trg_sync_operations_device_binding
          BEFORE INSERT OR UPDATE ON transport_erp.sync_operations FOR EACH ROW
          EXECUTE FUNCTION transport_erp.enforce_sync_operation_device_binding();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations,
                   transport_erp.sync_proof_replays,
                   transport_erp.sync_proof_nonces,
                   transport_erp.audit_events,
                   transport_erp.registered_device_assignments
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations
                     WHERE "RequestFingerprintVersion" IS NOT NULL OR "AcceptedProofReplayId" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.sync_proof_nonces)
             OR EXISTS (SELECT 1 FROM transport_erp.sync_proof_replays) THEN
            RAISE EXCEPTION 'STAGE4_DOWN_BLOCKED_DATA_PRESENT';
          END IF;

          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations
                     WHERE "EntityId" IS NULL OR "ResultEntityId" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.audit_events
                        WHERE "OperationCorrelationId" IS NOT NULL)
             OR EXISTS (
                  SELECT 1 FROM transport_erp.sync_operations
                  GROUP BY "DeviceId","ClientOperationId" HAVING count(*)>1
             ) THEN
            RAISE EXCEPTION 'STAGE4_DOWN_LEGACY_SHAPE_CONFLICT';
          END IF;
        END $body$;

        CREATE OR REPLACE FUNCTION transport_erp.enforce_sync_operation_device_binding()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          IF TG_OP='UPDATE' THEN
            IF OLD."RegisteredDeviceId" IS DISTINCT FROM NEW."RegisteredDeviceId" OR
               OLD."RegisteredDeviceCredentialVersion" IS DISTINCT FROM NEW."RegisteredDeviceCredentialVersion" OR
               OLD."DeviceId" IS DISTINCT FROM NEW."DeviceId" OR
               OLD."UserId" IS DISTINCT FROM NEW."UserId" OR
               OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR
               OLD."BranchId" IS DISTINCT FROM NEW."BranchId" THEN
              RAISE EXCEPTION 'sync operation provenance is immutable';
            END IF;
            RETURN NEW;
          END IF;
          IF NEW."RegisteredDeviceId" IS NULL OR NEW."RegisteredDeviceCredentialVersion" IS NULL THEN
            RAISE EXCEPTION 'new sync operation requires registered device provenance';
          END IF;
          PERFORM 1 FROM transport_erp.registered_devices d
            WHERE d."Id"=NEW."RegisteredDeviceId" AND d."CompanyId"=NEW."CompanyId"
              AND d."DeviceId"=NEW."DeviceId" AND d."Status"='ACTIVE'
              AND d."CredentialVersion"=NEW."RegisteredDeviceCredentialVersion"
              AND (d."ExpiresAt" IS NULL OR d."ExpiresAt">clock_timestamp())
              AND COALESCE(d."LastSeenAt",d."ApprovedAt",d."CreatedAt")>clock_timestamp()-interval '90 days'
            FOR SHARE;
          IF NOT FOUND OR NOT EXISTS (
            SELECT 1 FROM transport_erp.registered_device_assignments a
            WHERE a."RegisteredDeviceId"=NEW."RegisteredDeviceId" AND a."CompanyId"=NEW."CompanyId"
              AND a."UserId"=NEW."UserId" AND a."BranchId"=NEW."BranchId" AND a."Status"='ACTIVE') THEN
            RAISE EXCEPTION 'sync operation registered device binding is not active';
          END IF;
          RETURN NEW;
        END $body$;

        DROP INDEX transport_erp.ix_sync_op_accepted_proof;
        DROP INDEX transport_erp.ux_sync_op_registered_device_client;
        DROP INDEX transport_erp.ux_sync_op_legacy_company_device_client;
        DROP TRIGGER trg_sync_replay_append_only ON transport_erp.sync_proof_replays;
        DROP FUNCTION transport_erp.fn_sync_replay_append_only();
        DROP TABLE transport_erp.sync_proof_replays;
        DROP TABLE transport_erp.sync_proof_nonces;
        ALTER TABLE transport_erp.registered_device_assignments
          DROP CONSTRAINT ux_device_assignment_proof_scope;
        DROP INDEX transport_erp.ix_audit_event_operation_correlation;
        ALTER TABLE transport_erp.audit_events DROP COLUMN "OperationCorrelationId";
        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT ck_sync_stage4_contract_bundle,
          DROP COLUMN "AcceptedProofReplayId",
          DROP COLUMN "ProofKeyThumbprint",
          DROP COLUMN "ProofKeyVersion",
          DROP COLUMN "RequestFingerprintHash",
          DROP COLUMN "RequestFingerprintVersion",
          DROP COLUMN "OperationCorrelationId",
          DROP COLUMN "ProtocolVersion",
          DROP COLUMN "ActionCode",
          DROP COLUMN "ResultEntityId",
          ALTER COLUMN "EntityId" SET NOT NULL;
        CREATE UNIQUE INDEX "IX_sync_operations_DeviceId_ClientOperationId"
          ON transport_erp.sync_operations ("DeviceId","ClientOperationId");
        """);
}
