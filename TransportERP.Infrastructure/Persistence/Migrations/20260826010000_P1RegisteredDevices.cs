using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826010000_P1RegisteredDevices")]
public partial class P1RegisteredDevices : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        CREATE TABLE transport_erp.registered_devices (
          "Id" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "DeviceId" varchar(120) NOT NULL,
          "DisplayName" varchar(200) NOT NULL,
          "Platform" varchar(40) NOT NULL,
          "AppVersion" varchar(40) NOT NULL,
          "DeviceModel" varchar(120) NULL,
          "OsVersion" varchar(80) NULL,
          "RegistrationRequestId" varchar(120) NOT NULL,
          "CredentialHash" varchar(64) NOT NULL,
          "CredentialVersion" integer NOT NULL,
          "Status" varchar(20) NOT NULL,
          "RegisteredByUserId" uuid NOT NULL,
          "ApprovedByUserId" uuid NULL,
          "ApprovedAt" timestamptz NULL,
          "SuspendedAt" timestamptz NULL,
          "RevokedAt" timestamptz NULL,
          "ExpiresAt" timestamptz NULL,
          "LastSeenAt" timestamptz NULL,
          "CreatedAt" timestamptz NOT NULL,
          "UpdatedAt" timestamptz NOT NULL,
          "RowVersion" bytea NOT NULL,
          CONSTRAINT "PK_registered_devices" PRIMARY KEY ("Id"),
          CONSTRAINT "AK_registered_devices_Id_CompanyId" UNIQUE ("Id","CompanyId"),
          CONSTRAINT "AK_registered_devices_Id_CompanyId_DeviceId" UNIQUE ("Id","CompanyId","DeviceId"),
          CONSTRAINT ck_registered_devices_status CHECK ("Status" IN ('PENDING','ACTIVE','SUSPENDED','REVOKED','EXPIRED')),
          CONSTRAINT ck_registered_devices_credential_version CHECK ("CredentialVersion" >= 1),
          CONSTRAINT ck_registered_devices_credential_hash CHECK (length("CredentialHash") = 64),
          CONSTRAINT "FK_registered_devices_companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES transport_erp.companies ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_devices_users_RegisteredByUserId" FOREIGN KEY ("RegisteredByUserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_devices_users_ApprovedByUserId" FOREIGN KEY ("ApprovedByUserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT
        );
        CREATE UNIQUE INDEX "IX_registered_devices_CompanyId_DeviceId" ON transport_erp.registered_devices ("CompanyId","DeviceId");
        CREATE UNIQUE INDEX "IX_registered_devices_CompanyId_RegistrationRequestId" ON transport_erp.registered_devices ("CompanyId","RegistrationRequestId");
        CREATE INDEX "IX_registered_devices_CompanyId_Status" ON transport_erp.registered_devices ("CompanyId","Status");
        CREATE INDEX "IX_registered_devices_RegisteredByUserId" ON transport_erp.registered_devices ("RegisteredByUserId");
        CREATE INDEX "IX_registered_devices_ApprovedByUserId" ON transport_erp.registered_devices ("ApprovedByUserId");

        CREATE TABLE transport_erp.registered_device_assignments (
          "Id" uuid NOT NULL,
          "RegisteredDeviceId" uuid NOT NULL,
          "UserId" uuid NOT NULL,
          "CompanyId" uuid NOT NULL,
          "BranchId" uuid NOT NULL,
          "Status" varchar(20) NOT NULL,
          "AssignedByUserId" uuid NOT NULL,
          "RemovedByUserId" uuid NULL,
          "AssignedAt" timestamptz NOT NULL,
          "RemovedAt" timestamptz NULL,
          "CreatedAt" timestamptz NOT NULL,
          "UpdatedAt" timestamptz NOT NULL,
          "RowVersion" bytea NOT NULL,
          CONSTRAINT "PK_registered_device_assignments" PRIMARY KEY ("Id"),
          CONSTRAINT ck_registered_device_assignments_status CHECK ("Status" IN ('ACTIVE','REVOKED')),
          CONSTRAINT "FK_registered_device_assignments_registered_devices_RegisteredDeviceId_CompanyId" FOREIGN KEY ("RegisteredDeviceId","CompanyId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_device_assignments_users_UserId" FOREIGN KEY ("UserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_device_assignments_branches_BranchId_CompanyId" FOREIGN KEY ("BranchId","CompanyId")
            REFERENCES transport_erp.branches ("Id","CompanyId") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_device_assignments_users_AssignedByUserId" FOREIGN KEY ("AssignedByUserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_registered_device_assignments_users_RemovedByUserId" FOREIGN KEY ("RemovedByUserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT
        );
        CREATE INDEX "IX_registered_device_assignments_RegisteredDeviceId_CompanyId" ON transport_erp.registered_device_assignments ("RegisteredDeviceId","CompanyId");
        CREATE INDEX "IX_registered_device_assignments_BranchId_CompanyId" ON transport_erp.registered_device_assignments ("BranchId","CompanyId");
        CREATE INDEX "IX_registered_device_assignments_UserId_CompanyId_BranchId_Status" ON transport_erp.registered_device_assignments ("UserId","CompanyId","BranchId","Status");
        CREATE UNIQUE INDEX "IX_registered_device_assignments_active" ON transport_erp.registered_device_assignments
          ("RegisteredDeviceId","UserId","BranchId") WHERE "Status"='ACTIVE';
        CREATE INDEX "IX_registered_device_assignments_AssignedByUserId" ON transport_erp.registered_device_assignments ("AssignedByUserId");
        CREATE INDEX "IX_registered_device_assignments_RemovedByUserId" ON transport_erp.registered_device_assignments ("RemovedByUserId");

        CREATE FUNCTION transport_erp.enforce_registered_device_user_scope() RETURNS trigger LANGUAGE plpgsql AS $body$
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
            IF NOT FOUND THEN
              RAISE EXCEPTION 'registered device actor scope mismatch';
            END IF;
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
            IF NOT FOUND THEN
              RAISE EXCEPTION 'device assignment user scope mismatch';
            END IF;
            PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."AssignedByUserId"
              AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
            IF NOT FOUND THEN
              RAISE EXCEPTION 'device assignment actor scope mismatch';
            END IF;
            IF NEW."RemovedByUserId" IS NOT NULL THEN
              PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."RemovedByUserId"
                AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId");
              IF NOT FOUND THEN RAISE EXCEPTION 'device assignment remover scope mismatch'; END IF;
            END IF;
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_registered_devices_user_scope BEFORE INSERT OR UPDATE ON transport_erp.registered_devices
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_registered_device_user_scope();
        CREATE TRIGGER trg_registered_device_assignments_user_scope BEFORE INSERT OR UPDATE ON transport_erp.registered_device_assignments
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_registered_device_user_scope();

        CREATE FUNCTION transport_erp.enforce_auth_session_user_scope() RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."UserId"::text, 0));
          PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."UserId"
            AND (u."CompanyId" IS NULL OR u."CompanyId"=NEW."CompanyId")
            AND (u."BranchId" IS NULL OR u."BranchId"=NEW."BranchId");
          IF NOT FOUND THEN
            RAISE EXCEPTION 'auth session user scope mismatch';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_auth_sessions_user_scope BEFORE INSERT OR UPDATE OF "UserId","CompanyId","BranchId"
          ON transport_erp.auth_sessions FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_auth_session_user_scope();

        ALTER TABLE transport_erp.auth_sessions
          ADD COLUMN "RegisteredDeviceId" uuid NULL,
          ADD COLUMN "DeviceCredentialVersion" integer NULL,
          ADD CONSTRAINT ck_auth_sessions_registered_device_binding CHECK (
            ("RegisteredDeviceId" IS NULL AND "DeviceCredentialVersion" IS NULL) OR
            ("RegisteredDeviceId" IS NOT NULL AND "DeviceCredentialVersion" >= 1 AND "BranchId" IS NOT NULL)),
          ADD CONSTRAINT "FK_auth_sessions_registered_devices_RegisteredDeviceId_CompanyId_DeviceId"
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT;
        CREATE INDEX "IX_auth_sessions_RegisteredDeviceId_CompanyId_DeviceId" ON transport_erp.auth_sessions ("RegisteredDeviceId","CompanyId","DeviceId");

        ALTER TABLE transport_erp.sync_operations
          ADD COLUMN "RegisteredDeviceId" uuid NULL,
          ADD COLUMN "RegisteredDeviceCredentialVersion" integer NULL,
          ADD CONSTRAINT ck_sync_registered_device_binding CHECK (
            ("RegisteredDeviceId" IS NULL AND "RegisteredDeviceCredentialVersion" IS NULL) OR
            ("RegisteredDeviceId" IS NOT NULL AND "RegisteredDeviceCredentialVersion" >= 1 AND "BranchId" IS NOT NULL)),
          ADD CONSTRAINT "FK_sync_operations_registered_devices_RegisteredDeviceId_CompanyId_DeviceId"
            FOREIGN KEY ("RegisteredDeviceId","CompanyId","DeviceId")
            REFERENCES transport_erp.registered_devices ("Id","CompanyId","DeviceId") ON DELETE RESTRICT;
        CREATE INDEX "IX_sync_operations_RegisteredDeviceId_CompanyId_DeviceId" ON transport_erp.sync_operations ("RegisteredDeviceId","CompanyId","DeviceId");
        DO $$ BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations o
                     LEFT JOIN transport_erp.branches b ON b."Id"=o."BranchId" AND b."CompanyId"=o."CompanyId"
                     WHERE o."BranchId" IS NOT NULL AND b."Id" IS NULL) THEN
            RAISE EXCEPTION 'P1RegisteredDevices blocked: historical sync operation branch/company mismatch';
          END IF;
        END $$;
        ALTER TABLE transport_erp.sync_operations DROP CONSTRAINT IF EXISTS "FK_sync_operations_branches_BranchId";
        DROP INDEX IF EXISTS transport_erp."IX_sync_operations_BranchId";
        CREATE INDEX "IX_sync_operations_BranchId_CompanyId" ON transport_erp.sync_operations ("BranchId","CompanyId");
        ALTER TABLE transport_erp.sync_operations ADD CONSTRAINT "FK_sync_operations_branches_BranchId_CompanyId"
          FOREIGN KEY ("BranchId","CompanyId") REFERENCES transport_erp.branches ("Id","CompanyId") ON DELETE RESTRICT;

        DO $$ BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.auth_sessions s JOIN transport_erp.users u ON u."Id"=s."UserId"
                     WHERE NOT ((u."CompanyId" IS NULL OR u."CompanyId"=s."CompanyId") AND
                                (u."BranchId" IS NULL OR u."BranchId"=s."BranchId"))) THEN
            RAISE EXCEPTION 'P1RegisteredDevices blocked: historical auth session user scope mismatch';
          END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.sync_operations o JOIN transport_erp.users u ON u."Id"=o."UserId"
                     WHERE u."CompanyId" IS NULL OR u."CompanyId"<>o."CompanyId" OR
                           (u."BranchId" IS NOT NULL AND u."BranchId" IS DISTINCT FROM o."BranchId")) THEN
            RAISE EXCEPTION 'P1RegisteredDevices blocked: historical sync operation user scope mismatch';
          END IF;
        END $$;

        CREATE FUNCTION transport_erp.enforce_sync_operation_user_scope() RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM pg_advisory_xact_lock(hashtextextended('user-scope|' || NEW."UserId"::text, 0));
          PERFORM 1 FROM transport_erp.users u WHERE u."Id"=NEW."UserId"
            AND u."CompanyId"=NEW."CompanyId" AND (u."BranchId" IS NULL OR u."BranchId"=NEW."BranchId");
          IF NOT FOUND THEN
            RAISE EXCEPTION 'sync operation user scope mismatch';
          END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_sync_operations_user_scope BEFORE INSERT OR UPDATE OF "UserId","CompanyId","BranchId"
          ON transport_erp.sync_operations FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_operation_user_scope();

        CREATE FUNCTION transport_erp.enforce_sync_operation_device_binding() RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          IF TG_OP='UPDATE' THEN
            IF OLD."RegisteredDeviceId" IS DISTINCT FROM NEW."RegisteredDeviceId" OR
               OLD."RegisteredDeviceCredentialVersion" IS DISTINCT FROM NEW."RegisteredDeviceCredentialVersion" OR
               OLD."DeviceId" IS DISTINCT FROM NEW."DeviceId" OR OLD."UserId" IS DISTINCT FROM NEW."UserId" OR
               OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR OLD."BranchId" IS DISTINCT FROM NEW."BranchId" THEN
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
        CREATE TRIGGER trg_sync_operations_device_binding BEFORE INSERT OR UPDATE ON transport_erp.sync_operations
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_sync_operation_device_binding();

        CREATE FUNCTION transport_erp.prevent_user_scope_reference_drift() RETURNS trigger LANGUAGE plpgsql AS $body$
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
        CREATE TRIGGER trg_users_prevent_scope_reference_drift BEFORE UPDATE OF "CompanyId","BranchId"
          ON transport_erp.users FOR EACH ROW
          WHEN (OLD."CompanyId" IS DISTINCT FROM NEW."CompanyId" OR OLD."BranchId" IS DISTINCT FROM NEW."BranchId")
          EXECUTE FUNCTION transport_erp.prevent_user_scope_reference_drift();

        DO $$ BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.permissions
                     WHERE "Code" IN ('devices.register','devices.read','devices.manage') OR "Id" IN
                       ('d1000000-0000-4000-8000-000000000001'::uuid,'d1000000-0000-4000-8000-000000000002'::uuid,
                        'd1000000-0000-4000-8000-000000000003'::uuid)) THEN
            RAISE EXCEPTION 'P1RegisteredDevices blocked: device permission codes already exist and are not migration-owned';
          END IF;
        END $$;
        INSERT INTO transport_erp.permissions
          ("Id","Code","NameAr","Resource","Action","ScopeType","IsSystem","Status","CreatedAt","UpdatedAt","RowVersion","DeletedAt")
        SELECT v.id, v.code, v.name_ar, 'devices', v.action, 'COMPANY', true, 'ACTIVE',
               clock_timestamp(), clock_timestamp(), decode(md5(random()::text || clock_timestamp()::text), 'hex'), NULL
        FROM (VALUES ('d1000000-0000-4000-8000-000000000001'::uuid,'devices.register','تسجيل جهاز','register'),
                     ('d1000000-0000-4000-8000-000000000002'::uuid,'devices.read','عرض الأجهزة','read'),
                     ('d1000000-0000-4000-8000-000000000003'::uuid,'devices.manage','إدارة الأجهزة','manage'))
             v(id,code,name_ar,action);
        DO $$ BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.permissions WHERE "Code" IN ('devices.register','devices.read','devices.manage')
                     AND ("DeletedAt" IS NOT NULL OR "Status"<>'ACTIVE' OR NOT "IsSystem" OR
                          "Resource"<>'devices' OR "ScopeType"<>'COMPANY' OR
                          "Action"<>split_part("Code",'.',2) OR "NameAr"<>CASE "Code"
                            WHEN 'devices.register' THEN 'تسجيل جهاز' WHEN 'devices.read' THEN 'عرض الأجهزة'
                            WHEN 'devices.manage' THEN 'إدارة الأجهزة' END)) THEN
            RAISE EXCEPTION 'P1RegisteredDevices blocked: device permission catalog drift';
          END IF;
        END $$;
        INSERT INTO transport_erp.role_permissions
          ("RoleId","PermissionId","ScopeType","CompanyId","BranchId","CreatedAt","UpdatedAt","RowVersion")
        SELECT r."Id", p."Id", 'COMPANY', r."CompanyId", NULL, clock_timestamp(), clock_timestamp(),
               decode(md5(random()::text || clock_timestamp()::text), 'hex')
        FROM transport_erp.roles r CROSS JOIN transport_erp.permissions p
        WHERE r."Code"='SYSTEM_ADMIN' AND r."CompanyId" IS NOT NULL AND r."Status"='ACTIVE' AND
              p."Code" IN ('devices.register','devices.read','devices.manage')
        ON CONFLICT DO NOTHING;
        DO $$ BEGIN
          IF EXISTS (
            SELECT 1 FROM transport_erp.roles r CROSS JOIN transport_erp.permissions p
            LEFT JOIN transport_erp.role_permissions rp ON rp."RoleId"=r."Id" AND rp."PermissionId"=p."Id"
            WHERE r."Code"='SYSTEM_ADMIN' AND r."CompanyId" IS NOT NULL AND r."Status"='ACTIVE'
              AND p."Code" IN ('devices.register','devices.read','devices.manage')
              AND (rp."RoleId" IS NULL OR rp."ScopeType"<>'COMPANY' OR rp."CompanyId"<>r."CompanyId" OR rp."BranchId" IS NOT NULL)
          ) THEN RAISE EXCEPTION 'P1RegisteredDevices blocked: SYSTEM_ADMIN device grant drift'; END IF;
        END $$;
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.registered_devices,
                   transport_erp.registered_device_assignments,
                   transport_erp.auth_sessions,
                   transport_erp.sync_operations,
                   transport_erp.role_permissions,
                   transport_erp.user_permission_overrides
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.registered_devices)
             OR EXISTS (SELECT 1 FROM transport_erp.registered_device_assignments)
             OR EXISTS (SELECT 1 FROM transport_erp.auth_sessions
                        WHERE "RegisteredDeviceId" IS NOT NULL OR "DeviceCredentialVersion" IS NOT NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.sync_operations
                        WHERE "RegisteredDeviceId" IS NOT NULL OR "RegisteredDeviceCredentialVersion" IS NOT NULL)
             OR EXISTS (
                  SELECT 1 FROM transport_erp.role_permissions rp
                  JOIN transport_erp.roles r ON r."Id"=rp."RoleId"
                  WHERE rp."PermissionId" IN
                    ('d1000000-0000-4000-8000-000000000001'::uuid,
                     'd1000000-0000-4000-8000-000000000002'::uuid,
                     'd1000000-0000-4000-8000-000000000003'::uuid)
                    AND NOT (r."Code"='SYSTEM_ADMIN' AND r."CompanyId" IS NOT NULL
                             AND rp."ScopeType"='COMPANY' AND rp."CompanyId"=r."CompanyId"
                             AND rp."BranchId" IS NULL))
             OR EXISTS (SELECT 1 FROM transport_erp.user_permission_overrides WHERE "PermissionId" IN
                          ('d1000000-0000-4000-8000-000000000001'::uuid,
                           'd1000000-0000-4000-8000-000000000002'::uuid,
                           'd1000000-0000-4000-8000-000000000003'::uuid)) THEN
            RAISE EXCEPTION 'P1_REGISTERED_DEVICES_DOWN_BLOCKED_OPERATIONAL_DATA';
          END IF;
        END $body$;

        DELETE FROM transport_erp.user_permission_overrides WHERE "PermissionId" IN
          ('d1000000-0000-4000-8000-000000000001'::uuid,'d1000000-0000-4000-8000-000000000002'::uuid,
           'd1000000-0000-4000-8000-000000000003'::uuid);
        DELETE FROM transport_erp.role_permissions WHERE "PermissionId" IN
          ('d1000000-0000-4000-8000-000000000001'::uuid,'d1000000-0000-4000-8000-000000000002'::uuid,
           'd1000000-0000-4000-8000-000000000003'::uuid);
        DELETE FROM transport_erp.permissions WHERE "Id" IN
          ('d1000000-0000-4000-8000-000000000001'::uuid,'d1000000-0000-4000-8000-000000000002'::uuid,
           'd1000000-0000-4000-8000-000000000003'::uuid);
        ALTER TABLE transport_erp.sync_operations DROP CONSTRAINT IF EXISTS "FK_sync_operations_registered_devices_RegisteredDeviceId_CompanyId_DeviceId",
          DROP CONSTRAINT IF EXISTS "FK_sync_operations_branches_BranchId_CompanyId", DROP CONSTRAINT IF EXISTS ck_sync_registered_device_binding;
        DROP INDEX IF EXISTS transport_erp."IX_sync_operations_RegisteredDeviceId_CompanyId_DeviceId";
        DROP INDEX IF EXISTS transport_erp."IX_sync_operations_BranchId_CompanyId";
        DROP TRIGGER IF EXISTS trg_sync_operations_user_scope ON transport_erp.sync_operations;
        DROP FUNCTION IF EXISTS transport_erp.enforce_sync_operation_user_scope();
        DROP TRIGGER IF EXISTS trg_sync_operations_device_binding ON transport_erp.sync_operations;
        DROP FUNCTION IF EXISTS transport_erp.enforce_sync_operation_device_binding();
        DROP TRIGGER IF EXISTS trg_users_prevent_scope_reference_drift ON transport_erp.users;
        DROP FUNCTION IF EXISTS transport_erp.prevent_user_scope_reference_drift();
        CREATE INDEX "IX_sync_operations_BranchId" ON transport_erp.sync_operations ("BranchId");
        ALTER TABLE transport_erp.sync_operations ADD CONSTRAINT "FK_sync_operations_branches_BranchId"
          FOREIGN KEY ("BranchId") REFERENCES transport_erp.branches ("Id") ON DELETE RESTRICT;
        ALTER TABLE transport_erp.sync_operations DROP COLUMN "RegisteredDeviceId", DROP COLUMN "RegisteredDeviceCredentialVersion";
        ALTER TABLE transport_erp.auth_sessions DROP CONSTRAINT IF EXISTS "FK_auth_sessions_registered_devices_RegisteredDeviceId_CompanyId_DeviceId",
          DROP CONSTRAINT IF EXISTS ck_auth_sessions_registered_device_binding;
        DROP TRIGGER IF EXISTS trg_auth_sessions_user_scope ON transport_erp.auth_sessions;
        DROP FUNCTION IF EXISTS transport_erp.enforce_auth_session_user_scope();
        DROP INDEX IF EXISTS transport_erp."IX_auth_sessions_RegisteredDeviceId_CompanyId_DeviceId";
        ALTER TABLE transport_erp.auth_sessions DROP COLUMN "RegisteredDeviceId", DROP COLUMN "DeviceCredentialVersion";
        DROP TABLE transport_erp.registered_device_assignments;
        DROP TABLE transport_erp.registered_devices;
        DROP FUNCTION transport_erp.enforce_registered_device_user_scope();
        """);
}
