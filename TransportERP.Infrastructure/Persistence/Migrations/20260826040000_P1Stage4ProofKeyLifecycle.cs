using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826040000_P1Stage4ProofKeyLifecycle")]
public partial class P1Stage4ProofKeyLifecycle : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.registered_devices, transport_erp.users IN ACCESS EXCLUSIVE MODE;

        ALTER TABLE transport_erp.registered_devices
          ADD COLUMN "ProofPublicJwkCanonicalJson" varchar(512) NULL,
          ADD COLUMN "ProofKeyThumbprint" varchar(43) NULL,
          ADD COLUMN "ProofKeyVersion" integer NULL,
          ADD COLUMN "ProofKeyChangedAt" timestamptz NULL,
          ADD COLUMN "ProofKeyChangedByUserId" uuid NULL,
          ADD CONSTRAINT ck_reg_device_proof_key_bundle CHECK (
            ("ProofPublicJwkCanonicalJson" IS NULL AND "ProofKeyThumbprint" IS NULL AND
             "ProofKeyVersion" IS NULL AND "ProofKeyChangedAt" IS NULL AND "ProofKeyChangedByUserId" IS NULL)
            OR
            ("ProofPublicJwkCanonicalJson" IS NOT NULL AND "ProofKeyThumbprint" IS NOT NULL AND
             "ProofKeyVersion">=1 AND "ProofKeyChangedAt" IS NOT NULL AND "ProofKeyChangedByUserId" IS NOT NULL AND
             char_length("ProofKeyThumbprint")=43)
          ),
          ADD CONSTRAINT fk_reg_device_proof_changed_by FOREIGN KEY ("ProofKeyChangedByUserId")
            REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT;
        CREATE UNIQUE INDEX ux_registered_device_proof_thumbprint
          ON transport_erp.registered_devices ("ProofKeyThumbprint") WHERE "ProofKeyThumbprint" IS NOT NULL;
        CREATE INDEX ix_reg_device_proof_changed_by
          ON transport_erp.registered_devices ("ProofKeyChangedByUserId");

        CREATE TABLE transport_erp.registered_device_proof_key_challenges (
          "Id" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "RegisteredDeviceId" uuid NOT NULL,
          "DeviceId" varchar(120) NOT NULL,
          "ChangeRequestId" uuid NOT NULL,
          "ChangeType" varchar(8) NOT NULL,
          "ExpectedProofKeyVersion" integer NULL,
          "NewProofKeyThumbprint" varchar(43) NOT NULL,
          "ChallengeHash" bytea NOT NULL,
          "IssuedAt" timestamptz NOT NULL,
          "ExpiresAt" timestamptz NOT NULL,
          "ConsumedAt" timestamptz NULL,
          "CreatedByUserId" uuid NOT NULL,
          CONSTRAINT pk_device_key_challenges PRIMARY KEY ("Id"),
          CONSTRAINT ux_key_challenge_change_scope UNIQUE
            ("Id","CompanyId","RegisteredDeviceId","DeviceId","ChangeRequestId","ChangeType","NewProofKeyThumbprint"),
          CONSTRAINT fk_key_challenge_registered_device
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT,
          CONSTRAINT fk_key_challenge_created_by FOREIGN KEY ("CreatedByUserId")
            REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT ck_key_challenge_type CHECK ("ChangeType" IN ('BIND','ROTATE','RECOVER')),
          CONSTRAINT ck_key_challenge_expected_version CHECK (
            ("ChangeType"='BIND' AND "ExpectedProofKeyVersion" IS NULL) OR
            ("ChangeType" IN ('ROTATE','RECOVER') AND "ExpectedProofKeyVersion" IS NOT NULL AND
             "ExpectedProofKeyVersion">=1)
          ),
          CONSTRAINT ck_key_challenge_hash_len CHECK
            (octet_length("ChallengeHash")=32 AND char_length("NewProofKeyThumbprint")=43),
          CONSTRAINT ck_key_challenge_window CHECK
            ("ExpiresAt">"IssuedAt" AND ("ConsumedAt" IS NULL OR
             ("ConsumedAt">="IssuedAt" AND "ConsumedAt"<"ExpiresAt")))
        );
        CREATE UNIQUE INDEX ux_device_key_challenge_request
          ON transport_erp.registered_device_proof_key_challenges ("RegisteredDeviceId","ChangeRequestId");
        CREATE INDEX ix_device_key_challenge_expiry
          ON transport_erp.registered_device_proof_key_challenges ("ExpiresAt");
        CREATE INDEX ix_key_challenge_device_scope
          ON transport_erp.registered_device_proof_key_challenges ("RegisteredDeviceId","CompanyId","DeviceId");
        CREATE INDEX ix_key_challenge_created_by
          ON transport_erp.registered_device_proof_key_challenges ("CreatedByUserId");

        CREATE TABLE transport_erp.registered_device_proof_key_changes (
          "Id" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "RegisteredDeviceId" uuid NOT NULL,
          "DeviceId" varchar(120) NOT NULL,
          "ChangeRequestId" uuid NOT NULL,
          "ChallengeId" uuid NOT NULL,
          "ChangeType" varchar(8) NOT NULL,
          "ExpectedProofKeyVersion" integer NULL,
          "PreviousProofKeyThumbprint" varchar(43) NULL,
          "NewProofKeyThumbprint" varchar(43) NOT NULL,
          "ResultProofKeyVersion" integer NOT NULL,
          "ChangedByUserId" uuid NOT NULL,
          "Reason" varchar(500) NULL,
          "ChangedAt" timestamptz NOT NULL,
          CONSTRAINT pk_device_key_changes PRIMARY KEY ("Id"),
          CONSTRAINT fk_key_change_registered_device
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT,
          CONSTRAINT fk_key_change_challenge_scope
            FOREIGN KEY ("ChallengeId","CompanyId","RegisteredDeviceId","DeviceId","ChangeRequestId","ChangeType","NewProofKeyThumbprint")
            REFERENCES transport_erp.registered_device_proof_key_challenges
              ("Id","CompanyId","RegisteredDeviceId","DeviceId","ChangeRequestId","ChangeType","NewProofKeyThumbprint")
            ON DELETE RESTRICT,
          CONSTRAINT fk_key_change_changed_by FOREIGN KEY ("ChangedByUserId")
            REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT ck_key_change_type CHECK ("ChangeType" IN ('BIND','ROTATE','RECOVER')),
          CONSTRAINT ck_key_change_version_shape CHECK (
            ("ChangeType"='BIND' AND "ExpectedProofKeyVersion" IS NULL AND
             "PreviousProofKeyThumbprint" IS NULL AND "ResultProofKeyVersion"=1)
            OR
            ("ChangeType" IN ('ROTATE','RECOVER') AND "ExpectedProofKeyVersion" IS NOT NULL AND
             "ExpectedProofKeyVersion">=1 AND "PreviousProofKeyThumbprint" IS NOT NULL AND
             char_length("PreviousProofKeyThumbprint")=43 AND
             "ResultProofKeyVersion"="ExpectedProofKeyVersion"+1)
          ),
          CONSTRAINT ck_key_change_recovery_reason CHECK
            ("ChangeType"<>'RECOVER' OR ("Reason" IS NOT NULL AND char_length(trim("Reason"))>0))
        );
        CREATE UNIQUE INDEX ux_device_key_change_request
          ON transport_erp.registered_device_proof_key_changes ("RegisteredDeviceId","ChangeRequestId");
        CREATE INDEX ix_key_change_device_scope
          ON transport_erp.registered_device_proof_key_changes ("RegisteredDeviceId","CompanyId","DeviceId");
        CREATE INDEX ix_key_change_challenge_scope
          ON transport_erp.registered_device_proof_key_changes
            ("ChallengeId","CompanyId","RegisteredDeviceId","DeviceId","ChangeRequestId","ChangeType","NewProofKeyThumbprint");
        CREATE INDEX ix_key_change_changed_by
          ON transport_erp.registered_device_proof_key_changes ("ChangedByUserId");

        CREATE OR REPLACE FUNCTION transport_erp.enforce_registered_device_user_scope()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE scope_user_id uuid;
        BEGIN
          IF TG_TABLE_NAME='registered_devices' THEN
            FOR scope_user_id IN
              SELECT ids.id FROM (SELECT NEW."RegisteredByUserId" AS id
                UNION SELECT NEW."ApprovedByUserId" WHERE NEW."ApprovedByUserId" IS NOT NULL
                UNION SELECT NEW."ProofKeyChangedByUserId" WHERE NEW."ProofKeyChangedByUserId" IS NOT NULL) ids
              ORDER BY ids.id
            LOOP
              PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || scope_user_id::text, 0));
            END LOOP;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."RegisteredByUserId"
              AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
            IF NOT FOUND THEN RAISE EXCEPTION 'registered device actor scope mismatch'; END IF;
            IF NEW."ApprovedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."ApprovedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'registered device approver scope mismatch'; END IF;
            END IF;
            IF NEW."ProofKeyChangedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."ProofKeyChangedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'registered device proof actor scope mismatch'; END IF;
            END IF;
          ELSE
            FOR scope_user_id IN
              SELECT ids.id FROM (SELECT NEW."UserId" AS id UNION SELECT NEW."AssignedByUserId"
                UNION SELECT NEW."RemovedByUserId" WHERE NEW."RemovedByUserId" IS NOT NULL) ids ORDER BY ids.id
            LOOP
              PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || scope_user_id::text, 0));
            END LOOP;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."UserId"
              AND u."CompanyId"=NEW."CompanyId" AND (u."BranchId" IS NULL OR u."BranchId"=NEW."BranchId");
            IF NOT FOUND THEN RAISE EXCEPTION 'device assignment user scope mismatch'; END IF;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."AssignedByUserId"
              AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
            IF NOT FOUND THEN RAISE EXCEPTION 'device assignment actor scope mismatch'; END IF;
            IF NEW."RemovedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."RemovedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'device assignment remover scope mismatch'; END IF;
            END IF;
          END IF;
          RETURN NEW;
        END $body$;

        CREATE OR REPLACE FUNCTION transport_erp.prevent_user_scope_reference_drift()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."Id"::text, 0));
          IF EXISTS (SELECT 1 FROM transport_erp.registered_devices d
                     WHERE (d."RegisteredByUserId"=NEW."Id" OR d."ApprovedByUserId"=NEW."Id" OR
                            d."ProofKeyChangedByUserId"=NEW."Id")
                       AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=d."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_assignments a
                        WHERE a."UserId"=NEW."Id" AND (NEW."CompanyId" IS NULL OR
                          NEW."CompanyId"<>a."CompanyId" OR
                          (NEW."BranchId" IS NOT NULL AND NEW."BranchId"<>a."BranchId")))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_assignments a
                        WHERE (a."AssignedByUserId"=NEW."Id" OR a."RemovedByUserId"=NEW."Id")
                          AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=a."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_proof_key_challenges c
                        WHERE c."CreatedByUserId"=NEW."Id"
                          AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=c."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_proof_key_changes c
                        WHERE c."ChangedByUserId"=NEW."Id"
                          AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=c."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.auth_sessions s WHERE s."UserId"=NEW."Id" AND NOT (
                          (NEW."CompanyId" IS NULL OR NEW."CompanyId"=s."CompanyId") AND
                          (NEW."BranchId" IS NULL OR NEW."BranchId"=s."BranchId")))
             OR EXISTS (SELECT 1 FROM transport_erp.sync_operations o WHERE o."UserId"=NEW."Id" AND
                          (NEW."CompanyId" IS NULL OR NEW."CompanyId"<>o."CompanyId" OR
                           (NEW."BranchId" IS NOT NULL AND NEW."BranchId" IS DISTINCT FROM o."BranchId"))) THEN
            RAISE EXCEPTION 'user scope change would strand tenant-scoped references';
          END IF;
          RETURN NEW;
        END $body$;

        CREATE FUNCTION transport_erp.fn_reg_device_proof_key_transition()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE old_bound boolean; new_bound boolean;
        BEGIN
          old_bound := OLD."ProofKeyVersion" IS NOT NULL;
          new_bound := NEW."ProofKeyVersion" IS NOT NULL;
          IF NOT old_bound AND new_bound AND NEW."ProofKeyVersion"<>1 THEN
            RAISE EXCEPTION 'registered device proof key binding must start at version 1';
          END IF;
          IF old_bound AND NOT new_bound THEN
            RAISE EXCEPTION 'registered device proof key cannot be cleared';
          END IF;
          IF old_bound AND new_bound AND (
               OLD."ProofPublicJwkCanonicalJson" IS DISTINCT FROM NEW."ProofPublicJwkCanonicalJson" OR
               OLD."ProofKeyThumbprint" IS DISTINCT FROM NEW."ProofKeyThumbprint" OR
               OLD."ProofKeyVersion" IS DISTINCT FROM NEW."ProofKeyVersion" OR
               OLD."ProofKeyChangedAt" IS DISTINCT FROM NEW."ProofKeyChangedAt" OR
               OLD."ProofKeyChangedByUserId" IS DISTINCT FROM NEW."ProofKeyChangedByUserId") AND
             NEW."ProofKeyVersion"<>OLD."ProofKeyVersion"+1 THEN
            RAISE EXCEPTION 'registered device proof key version must increment exactly once';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_reg_device_proof_key_transition
          BEFORE UPDATE OF "ProofPublicJwkCanonicalJson","ProofKeyThumbprint","ProofKeyVersion",
                           "ProofKeyChangedAt","ProofKeyChangedByUserId"
          ON transport_erp.registered_devices FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_reg_device_proof_key_transition();

        CREATE FUNCTION transport_erp.fn_key_challenge_user_scope()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE device_row record;
        BEGIN
          SELECT * INTO device_row FROM transport_erp.registered_devices d
            WHERE d."Id"=NEW."RegisteredDeviceId" AND d."CompanyId"=NEW."CompanyId"
              AND d."DeviceId"=NEW."DeviceId" FOR UPDATE;
          IF NOT FOUND THEN RAISE EXCEPTION 'proof key challenge device scope mismatch'; END IF;
          IF NEW."ChangeType"='BIND' THEN
            IF device_row."Status" NOT IN ('PENDING','ACTIVE') OR device_row."ProofKeyVersion" IS NOT NULL OR
               NEW."ExpectedProofKeyVersion" IS NOT NULL THEN
              RAISE EXCEPTION 'proof key challenge device state mismatch';
            END IF;
          ELSIF NEW."ChangeType"='ROTATE' THEN
            IF device_row."Status"<>'ACTIVE' OR
               device_row."ProofKeyVersion" IS DISTINCT FROM NEW."ExpectedProofKeyVersion" THEN
              RAISE EXCEPTION 'proof key challenge device state mismatch';
            END IF;
          ELSIF NEW."ChangeType"='RECOVER' THEN
            IF device_row."Status" NOT IN ('ACTIVE','SUSPENDED','EXPIRED') OR
               device_row."ProofKeyVersion" IS DISTINCT FROM NEW."ExpectedProofKeyVersion" THEN
              RAISE EXCEPTION 'proof key challenge device state mismatch';
            END IF;
          ELSE
            RAISE EXCEPTION 'proof key challenge type invalid';
          END IF;
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."CreatedByUserId"::text, 0));
          PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."CreatedByUserId"
            AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
          IF NOT FOUND THEN RAISE EXCEPTION 'proof key challenge actor scope mismatch'; END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_key_challenge_user_scope
          BEFORE INSERT OR UPDATE OF "CreatedByUserId","CompanyId"
          ON transport_erp.registered_device_proof_key_challenges FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_key_challenge_user_scope();

        CREATE FUNCTION transport_erp.fn_key_challenge_update_guard()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          IF OLD."Id" IS DISTINCT FROM NEW."Id" OR
             OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR
             OLD."RegisteredDeviceId" IS DISTINCT FROM NEW."RegisteredDeviceId" OR
             OLD."DeviceId" IS DISTINCT FROM NEW."DeviceId" OR
             OLD."ChangeRequestId" IS DISTINCT FROM NEW."ChangeRequestId" OR
             OLD."ChangeType" IS DISTINCT FROM NEW."ChangeType" OR
             OLD."ExpectedProofKeyVersion" IS DISTINCT FROM NEW."ExpectedProofKeyVersion" OR
             OLD."NewProofKeyThumbprint" IS DISTINCT FROM NEW."NewProofKeyThumbprint" OR
             OLD."ChallengeHash" IS DISTINCT FROM NEW."ChallengeHash" OR
             OLD."IssuedAt" IS DISTINCT FROM NEW."IssuedAt" OR
             OLD."ExpiresAt" IS DISTINCT FROM NEW."ExpiresAt" OR
             OLD."CreatedByUserId" IS DISTINCT FROM NEW."CreatedByUserId" THEN
            RAISE EXCEPTION 'registered device proof key challenge is immutable';
          END IF;
          IF OLD."ConsumedAt" IS NOT NULL AND OLD."ConsumedAt" IS DISTINCT FROM NEW."ConsumedAt" THEN
            RAISE EXCEPTION 'registered device proof key challenge consumption is irreversible';
          END IF;
          IF OLD."ConsumedAt" IS NULL AND NEW."ConsumedAt" IS NOT NULL AND NOT EXISTS (
            SELECT 1 FROM transport_erp.registered_device_proof_key_changes c
             WHERE c."ChallengeId"=NEW."Id" AND c."CompanyId"=NEW."CompanyId"
               AND c."RegisteredDeviceId"=NEW."RegisteredDeviceId" AND c."DeviceId"=NEW."DeviceId"
               AND c."ChangeRequestId"=NEW."ChangeRequestId" AND c."ChangeType"=NEW."ChangeType"
               AND c."NewProofKeyThumbprint"=NEW."NewProofKeyThumbprint") THEN
            RAISE EXCEPTION 'proof key challenge consumption requires matching change ledger';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_key_challenge_update_guard
          BEFORE UPDATE ON transport_erp.registered_device_proof_key_challenges FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_key_challenge_update_guard();

        CREATE FUNCTION transport_erp.fn_key_change_insert_guard()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE device_row record;
        BEGIN
          SELECT * INTO device_row FROM transport_erp.registered_devices d
            WHERE d."Id"=NEW."RegisteredDeviceId" AND d."CompanyId"=NEW."CompanyId"
              AND d."DeviceId"=NEW."DeviceId" FOR UPDATE;
          IF NOT FOUND THEN RAISE EXCEPTION 'proof key change device scope mismatch'; END IF;
          IF NEW."ChangeType"='BIND' THEN
            IF device_row."Status" NOT IN ('PENDING','ACTIVE') OR device_row."ProofKeyVersion" IS NOT NULL OR
               NEW."ExpectedProofKeyVersion" IS NOT NULL OR NEW."PreviousProofKeyThumbprint" IS NOT NULL OR
               NEW."ResultProofKeyVersion"<>1 THEN
              RAISE EXCEPTION 'proof key change device state mismatch';
            END IF;
          ELSIF NEW."ChangeType"='ROTATE' THEN
            IF device_row."Status"<>'ACTIVE' OR
               device_row."ProofKeyVersion" IS DISTINCT FROM NEW."ExpectedProofKeyVersion" OR
               device_row."ProofKeyThumbprint" IS DISTINCT FROM NEW."PreviousProofKeyThumbprint" OR
               NEW."ResultProofKeyVersion"<>NEW."ExpectedProofKeyVersion"+1 THEN
              RAISE EXCEPTION 'proof key change device state mismatch';
            END IF;
          ELSIF NEW."ChangeType"='RECOVER' THEN
            IF device_row."Status" NOT IN ('ACTIVE','SUSPENDED','EXPIRED') OR
               device_row."ProofKeyVersion" IS DISTINCT FROM NEW."ExpectedProofKeyVersion" OR
               device_row."ProofKeyThumbprint" IS DISTINCT FROM NEW."PreviousProofKeyThumbprint" OR
               NEW."ResultProofKeyVersion"<>NEW."ExpectedProofKeyVersion"+1 THEN
              RAISE EXCEPTION 'proof key change device state mismatch';
            END IF;
          ELSE
            RAISE EXCEPTION 'proof key change type invalid';
          END IF;
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."ChangedByUserId"::text, 0));
          PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."ChangedByUserId"
            AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
          IF NOT FOUND THEN RAISE EXCEPTION 'proof key change actor scope mismatch'; END IF;
          PERFORM 1 FROM transport_erp.registered_device_proof_key_challenges c
            WHERE c."Id"=NEW."ChallengeId" AND c."CompanyId"=NEW."CompanyId"
              AND c."RegisteredDeviceId"=NEW."RegisteredDeviceId" AND c."DeviceId"=NEW."DeviceId"
              AND c."ChangeRequestId"=NEW."ChangeRequestId" AND c."ChangeType"=NEW."ChangeType"
              AND c."NewProofKeyThumbprint"=NEW."NewProofKeyThumbprint"
              AND c."ExpectedProofKeyVersion" IS NOT DISTINCT FROM NEW."ExpectedProofKeyVersion"
              AND c."ConsumedAt" IS NULL AND c."ExpiresAt">clock_timestamp()
            FOR UPDATE;
          IF NOT FOUND THEN RAISE EXCEPTION 'proof key change challenge mismatch'; END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_key_change_insert_guard
          BEFORE INSERT ON transport_erp.registered_device_proof_key_changes FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_key_change_insert_guard();

        CREATE FUNCTION transport_erp.fn_device_key_change_append_only()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          RAISE EXCEPTION 'registered device proof key change is append-only';
        END $body$;
        CREATE TRIGGER trg_device_key_change_append_only
          BEFORE UPDATE OR DELETE ON transport_erp.registered_device_proof_key_changes FOR EACH ROW
          EXECUTE FUNCTION transport_erp.fn_device_key_change_append_only();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.registered_device_proof_key_changes,
                   transport_erp.registered_device_proof_key_challenges,
                   transport_erp.registered_devices,
                   transport_erp.users
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.registered_devices WHERE "ProofKeyVersion" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_proof_key_challenges)
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_proof_key_changes) THEN
            RAISE EXCEPTION 'STAGE4_DOWN_BLOCKED_DATA_PRESENT';
          END IF;
        END $body$;

        DROP TRIGGER trg_device_key_change_append_only ON transport_erp.registered_device_proof_key_changes;
        DROP FUNCTION transport_erp.fn_device_key_change_append_only();
        DROP TRIGGER trg_key_change_insert_guard ON transport_erp.registered_device_proof_key_changes;
        DROP FUNCTION transport_erp.fn_key_change_insert_guard();
        DROP TRIGGER trg_key_challenge_update_guard ON transport_erp.registered_device_proof_key_challenges;
        DROP FUNCTION transport_erp.fn_key_challenge_update_guard();
        DROP TRIGGER trg_key_challenge_user_scope ON transport_erp.registered_device_proof_key_challenges;
        DROP FUNCTION transport_erp.fn_key_challenge_user_scope();
        DROP TRIGGER trg_reg_device_proof_key_transition ON transport_erp.registered_devices;
        DROP FUNCTION transport_erp.fn_reg_device_proof_key_transition();
        DROP TABLE transport_erp.registered_device_proof_key_changes;
        DROP TABLE transport_erp.registered_device_proof_key_challenges;
        DROP INDEX transport_erp.ux_registered_device_proof_thumbprint;
        DROP INDEX transport_erp.ix_reg_device_proof_changed_by;
        ALTER TABLE transport_erp.registered_devices
          DROP CONSTRAINT fk_reg_device_proof_changed_by,
          DROP CONSTRAINT ck_reg_device_proof_key_bundle,
          DROP COLUMN "ProofKeyChangedByUserId",
          DROP COLUMN "ProofKeyChangedAt",
          DROP COLUMN "ProofKeyVersion",
          DROP COLUMN "ProofKeyThumbprint",
          DROP COLUMN "ProofPublicJwkCanonicalJson";

        CREATE OR REPLACE FUNCTION transport_erp.enforce_registered_device_user_scope()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE scope_user_id uuid;
        BEGIN
          IF TG_TABLE_NAME='registered_devices' THEN
            FOR scope_user_id IN
              SELECT ids.id FROM (SELECT NEW."RegisteredByUserId" AS id
                UNION SELECT NEW."ApprovedByUserId" WHERE NEW."ApprovedByUserId" IS NOT NULL) ids ORDER BY ids.id
            LOOP
              PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || scope_user_id::text, 0));
            END LOOP;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."RegisteredByUserId"
              AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
            IF NOT FOUND THEN RAISE EXCEPTION 'registered device actor scope mismatch'; END IF;
            IF NEW."ApprovedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."ApprovedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'registered device approver scope mismatch'; END IF;
            END IF;
          ELSE
            FOR scope_user_id IN
              SELECT ids.id FROM (SELECT NEW."UserId" AS id UNION SELECT NEW."AssignedByUserId"
                UNION SELECT NEW."RemovedByUserId" WHERE NEW."RemovedByUserId" IS NOT NULL) ids ORDER BY ids.id
            LOOP
              PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || scope_user_id::text, 0));
            END LOOP;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."UserId"
              AND u."CompanyId"=NEW."CompanyId" AND (u."BranchId" IS NULL OR u."BranchId"=NEW."BranchId");
            IF NOT FOUND THEN RAISE EXCEPTION 'device assignment user scope mismatch'; END IF;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."AssignedByUserId"
              AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
            IF NOT FOUND THEN RAISE EXCEPTION 'device assignment actor scope mismatch'; END IF;
            IF NEW."RemovedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."RemovedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'device assignment remover scope mismatch'; END IF;
            END IF;
          END IF;
          RETURN NEW;
        END $body$;

        CREATE OR REPLACE FUNCTION transport_erp.prevent_user_scope_reference_drift()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."Id"::text, 0));
          IF EXISTS (SELECT 1 FROM transport_erp.registered_devices d
                     WHERE (d."RegisteredByUserId"=NEW."Id" OR d."ApprovedByUserId"=NEW."Id")
                       AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=d."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_assignments a
                        WHERE a."UserId"=NEW."Id" AND (NEW."CompanyId" IS NULL OR
                          NEW."CompanyId"<>a."CompanyId" OR
                          (NEW."BranchId" IS NOT NULL AND NEW."BranchId"<>a."BranchId")))
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_assignments a
                        WHERE (a."AssignedByUserId"=NEW."Id" OR a."RemovedByUserId"=NEW."Id")
                          AND NOT (NEW."CompanyId" IS NULL OR NEW."CompanyId"=a."CompanyId"))
             OR EXISTS (SELECT 1 FROM transport_erp.auth_sessions s WHERE s."UserId"=NEW."Id" AND NOT (
                          (NEW."CompanyId" IS NULL OR NEW."CompanyId"=s."CompanyId") AND
                          (NEW."BranchId" IS NULL OR NEW."BranchId"=s."BranchId")))
             OR EXISTS (SELECT 1 FROM transport_erp.sync_operations o WHERE o."UserId"=NEW."Id" AND
                          (NEW."CompanyId" IS NULL OR NEW."CompanyId"<>o."CompanyId" OR
                           (NEW."BranchId" IS NOT NULL AND NEW."BranchId" IS DISTINCT FROM o."BranchId"))) THEN
            RAISE EXCEPTION 'user scope change would strand tenant-scoped references';
          END IF;
          RETURN NEW;
        END $body$;
        """);
}
