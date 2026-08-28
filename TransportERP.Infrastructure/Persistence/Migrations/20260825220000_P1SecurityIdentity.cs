using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260825220000_P1SecurityIdentity")]
public partial class P1SecurityIdentity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        DROP TRIGGER IF EXISTS trg_audit_events_append_only ON transport_erp.audit_events;
        ALTER TABLE transport_erp.audit_events ADD COLUMN "SequenceNo" bigint NULL;
        WITH ordered AS (
          SELECT "Id", row_number() OVER (
            ORDER BY "OccurredAt", "Id") AS sequence_no
          FROM transport_erp.audit_events)
        UPDATE transport_erp.audit_events a SET "SequenceNo"=o.sequence_no FROM ordered o WHERE o."Id"=a."Id";
        CREATE SEQUENCE transport_erp.audit_event_sequence_no_seq AS bigint START WITH 1;
        DO $$ DECLARE max_sequence bigint; BEGIN
          SELECT max("SequenceNo") INTO max_sequence FROM transport_erp.audit_events;
          IF max_sequence IS NULL THEN
            PERFORM setval('transport_erp.audit_event_sequence_no_seq', 1, false);
          ELSE
            PERFORM setval('transport_erp.audit_event_sequence_no_seq', max_sequence, true);
          END IF;
        END $$;
        ALTER SEQUENCE transport_erp.audit_event_sequence_no_seq OWNED BY transport_erp.audit_events."SequenceNo";
        ALTER TABLE transport_erp.audit_events ALTER COLUMN "SequenceNo" SET NOT NULL,
          ALTER COLUMN "SequenceNo" SET DEFAULT nextval('transport_erp.audit_event_sequence_no_seq');
        CREATE UNIQUE INDEX "IX_audit_events_SequenceNo" ON transport_erp.audit_events ("SequenceNo");
        CREATE TRIGGER trg_audit_events_append_only BEFORE UPDATE OR DELETE ON transport_erp.audit_events
          FOR EACH ROW EXECUTE FUNCTION transport_erp.prevent_audit_event_mutation();

        CREATE TABLE transport_erp.audit_stream_heads (
          "StreamKey" varchar(200) PRIMARY KEY,
          "LastHash" varchar(128) NULL,
          "UpdatedAt" timestamptz NOT NULL);
        INSERT INTO transport_erp.audit_stream_heads ("StreamKey", "LastHash", "UpdatedAt")
        SELECT DISTINCT ON (stream_key) stream_key, "Hash", "OccurredAt"
        FROM (
          SELECT COALESCE("CompanyId"::text, '') || '|' || COALESCE("BranchId"::text, '') || '|' ||
                   COALESCE(btrim("DeviceId"), '') AS stream_key,
                 "Hash", "OccurredAt", "SequenceNo"
          FROM transport_erp.audit_events
        ) existing
        ORDER BY stream_key, "SequenceNo" DESC;

        ALTER TABLE transport_erp.users
          ADD COLUMN "AccessFailedCount" integer NOT NULL DEFAULT 0,
          ADD COLUMN "AuthVersion" integer NOT NULL DEFAULT 1,
          ADD COLUMN "LockoutEnd" timestamptz NULL,
          ADD COLUMN "NormalizedEmail" varchar(320) NULL,
          ADD COLUMN "SecurityStamp" varchar(64) NOT NULL DEFAULT replace(gen_random_uuid()::text, '-', '');
        UPDATE transport_erp.users SET "SecurityStamp"=replace(gen_random_uuid()::text, '-', '')
          WHERE "SecurityStamp" IS NULL OR length("SecurityStamp") < 32;
        ALTER TABLE transport_erp.users ALTER COLUMN "SecurityStamp" DROP DEFAULT;
        UPDATE transport_erp.users SET "NormalizedEmail"=upper(trim("Email")) WHERE "Email" IS NOT NULL;

        DO $$ BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.users WHERE "NormalizedEmail" IS NOT NULL AND "DeletedAt" IS NULL
                     GROUP BY "NormalizedEmail", "CompanyId" HAVING count(*) > 1)
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: duplicate normalized email within company scope'; END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.users WHERE "DeletedAt" IS NULL
                     GROUP BY "NormalizedUserName", "CompanyId" HAVING count(*) > 1)
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: duplicate normalized username within company scope'; END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.users u LEFT JOIN transport_erp.branches b
                     ON b."Id"=u."BranchId" AND b."CompanyId"=u."CompanyId"
                     WHERE u."BranchId" IS NOT NULL AND b."Id" IS NULL)
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: cross-tenant user branch scope'; END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.role_permissions rp JOIN transport_erp.permissions p ON p."Id"=rp."PermissionId"
                     WHERE rp."ScopeType"<>p."ScopeType" OR
                       (rp."ScopeType"='PLATFORM' AND (rp."CompanyId" IS NOT NULL OR rp."BranchId" IS NOT NULL)) OR
                       (rp."ScopeType"='COMPANY' AND (rp."CompanyId" IS NULL OR rp."BranchId" IS NOT NULL)) OR
                       (rp."ScopeType"='BRANCH' AND (rp."CompanyId" IS NULL OR rp."BranchId" IS NULL)))
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: malformed role permission scope'; END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.user_roles WHERE "BranchId" IS NOT NULL AND "CompanyId" IS NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.user_permission_overrides WHERE "BranchId" IS NOT NULL AND "CompanyId" IS NULL)
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: branch assignment without company'; END IF;
          IF EXISTS (SELECT 1 FROM transport_erp.role_permissions x LEFT JOIN transport_erp.branches b
                     ON b."Id"=x."BranchId" AND b."CompanyId"=x."CompanyId"
                     WHERE x."BranchId" IS NOT NULL AND b."Id" IS NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.user_roles x LEFT JOIN transport_erp.branches b
                     ON b."Id"=x."BranchId" AND b."CompanyId"=x."CompanyId"
                     WHERE x."BranchId" IS NOT NULL AND b."Id" IS NULL)
             OR EXISTS (SELECT 1 FROM transport_erp.user_permission_overrides x LEFT JOIN transport_erp.branches b
                     ON b."Id"=x."BranchId" AND b."CompanyId"=x."CompanyId"
                     WHERE x."BranchId" IS NOT NULL AND b."Id" IS NULL)
            THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: cross-tenant RBAC branch scope'; END IF;
        END $$;

        DROP INDEX IF EXISTS transport_erp."IX_users_Email_CompanyId";
        DROP INDEX IF EXISTS transport_erp."IX_users_NormalizedUserName_CompanyId";
        CREATE UNIQUE INDEX "IX_users_NormalizedUserName_CompanyId" ON transport_erp.users
          ("NormalizedUserName", "CompanyId") NULLS NOT DISTINCT WHERE "DeletedAt" IS NULL;
        CREATE UNIQUE INDEX "IX_users_NormalizedEmail_CompanyId" ON transport_erp.users
          ("NormalizedEmail", "CompanyId") NULLS NOT DISTINCT
          WHERE "NormalizedEmail" IS NOT NULL AND "DeletedAt" IS NULL;
        ALTER TABLE transport_erp.users DROP CONSTRAINT IF EXISTS "FK_users_branches_BranchId";
        DROP INDEX IF EXISTS transport_erp."IX_users_BranchId";
        CREATE INDEX "IX_users_BranchId_CompanyId" ON transport_erp.users ("BranchId", "CompanyId");
        ALTER TABLE transport_erp.users ADD CONSTRAINT "FK_users_branches_BranchId_CompanyId"
          FOREIGN KEY ("BranchId", "CompanyId") REFERENCES transport_erp.branches ("Id", "CompanyId") ON DELETE RESTRICT;
        ALTER TABLE transport_erp.users ADD CONSTRAINT ck_users_security_stamp CHECK (length("SecurityStamp") >= 32);
        ALTER TABLE transport_erp.users ADD CONSTRAINT ck_users_auth_version CHECK ("AuthVersion" >= 1);
        ALTER TABLE transport_erp.users ADD CONSTRAINT ck_users_branch_company CHECK ("BranchId" IS NULL OR "CompanyId" IS NOT NULL);

        ALTER TABLE transport_erp.role_permissions ADD CONSTRAINT ck_role_permissions_scope_fields CHECK (
          ("ScopeType"='PLATFORM' AND "CompanyId" IS NULL AND "BranchId" IS NULL) OR
          ("ScopeType"='COMPANY' AND "CompanyId" IS NOT NULL AND "BranchId" IS NULL) OR
          ("ScopeType"='BRANCH' AND "CompanyId" IS NOT NULL AND "BranchId" IS NOT NULL));
        ALTER TABLE transport_erp.user_roles ADD CONSTRAINT ck_user_roles_scope_fields CHECK ("BranchId" IS NULL OR "CompanyId" IS NOT NULL);
        ALTER TABLE transport_erp.user_permission_overrides ADD CONSTRAINT ck_user_permission_overrides_scope_fields CHECK ("BranchId" IS NULL OR "CompanyId" IS NOT NULL);
        CREATE INDEX "IX_role_permissions_BranchId_CompanyId" ON transport_erp.role_permissions ("BranchId", "CompanyId");
        CREATE INDEX "IX_user_roles_BranchId_CompanyId" ON transport_erp.user_roles ("BranchId", "CompanyId");
        CREATE INDEX "IX_user_permission_overrides_BranchId_CompanyId" ON transport_erp.user_permission_overrides ("BranchId", "CompanyId");
        ALTER TABLE transport_erp.role_permissions ADD CONSTRAINT "FK_role_permissions_branches_BranchId_CompanyId"
          FOREIGN KEY ("BranchId", "CompanyId") REFERENCES transport_erp.branches ("Id", "CompanyId") ON DELETE RESTRICT;
        ALTER TABLE transport_erp.user_roles ADD CONSTRAINT "FK_user_roles_branches_BranchId_CompanyId"
          FOREIGN KEY ("BranchId", "CompanyId") REFERENCES transport_erp.branches ("Id", "CompanyId") ON DELETE RESTRICT;
        ALTER TABLE transport_erp.user_permission_overrides ADD CONSTRAINT "FK_user_permission_overrides_branches_BranchId_CompanyId"
          FOREIGN KEY ("BranchId", "CompanyId") REFERENCES transport_erp.branches ("Id", "CompanyId") ON DELETE RESTRICT;

        CREATE TABLE transport_erp.auth_sessions (
          "Id" uuid PRIMARY KEY, "UserId" uuid NOT NULL, "CompanyId" uuid NOT NULL, "BranchId" uuid NULL,
          "DeviceId" varchar(120) NOT NULL, "Mode" varchar(20) NOT NULL,
          "SecurityStampAtIssue" varchar(64) NOT NULL, "AuthVersionAtIssue" integer NOT NULL,
          "RefreshTokenHash" varchar(64) NOT NULL, "RefreshTokenFamilyId" uuid NOT NULL, "ReplacedBySessionId" uuid NULL,
          "IssuedAt" timestamptz NOT NULL, "AccessTokenExpiresAt" timestamptz NOT NULL, "RefreshTokenExpiresAt" timestamptz NOT NULL,
          "LastUsedAt" timestamptz NULL, "RevokedAt" timestamptz NULL, "RevokeReason" varchar(200) NULL,
          "CreatedAt" timestamptz NOT NULL, "UpdatedAt" timestamptz NOT NULL, "RowVersion" bytea NOT NULL,
          CONSTRAINT ck_auth_sessions_mode CHECK ("Mode"='LOCAL'),
          CONSTRAINT ck_auth_sessions_expiry CHECK ("AccessTokenExpiresAt" <= "RefreshTokenExpiresAt"),
          CONSTRAINT ck_auth_sessions_security_stamp CHECK (length("SecurityStampAtIssue") >= 32),
          CONSTRAINT ck_auth_sessions_auth_version CHECK ("AuthVersionAtIssue" >= 1),
          CONSTRAINT "FK_auth_sessions_users_UserId" FOREIGN KEY ("UserId") REFERENCES transport_erp.users ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_auth_sessions_companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES transport_erp.companies ("Id") ON DELETE RESTRICT,
          CONSTRAINT "FK_auth_sessions_branches_BranchId_CompanyId" FOREIGN KEY ("BranchId","CompanyId") REFERENCES transport_erp.branches ("Id","CompanyId") ON DELETE RESTRICT,
          CONSTRAINT "FK_auth_sessions_auth_sessions_ReplacedBySessionId" FOREIGN KEY ("ReplacedBySessionId") REFERENCES transport_erp.auth_sessions ("Id") ON DELETE RESTRICT);
        CREATE INDEX "IX_auth_sessions_BranchId_CompanyId" ON transport_erp.auth_sessions ("BranchId","CompanyId");
        CREATE INDEX "IX_auth_sessions_CompanyId_BranchId" ON transport_erp.auth_sessions ("CompanyId","BranchId");
        CREATE INDEX "IX_auth_sessions_DeviceId" ON transport_erp.auth_sessions ("DeviceId");
        CREATE INDEX "IX_auth_sessions_RefreshTokenFamilyId" ON transport_erp.auth_sessions ("RefreshTokenFamilyId");
        CREATE UNIQUE INDEX "IX_auth_sessions_RefreshTokenHash" ON transport_erp.auth_sessions ("RefreshTokenHash");
        CREATE INDEX "IX_auth_sessions_ReplacedBySessionId" ON transport_erp.auth_sessions ("ReplacedBySessionId");
        CREATE INDEX "IX_auth_sessions_UserId_RevokedAt_RefreshTokenExpiresAt" ON transport_erp.auth_sessions ("UserId","RevokedAt","RefreshTokenExpiresAt");

        INSERT INTO transport_erp.permissions
          ("Id","Code","NameAr","Resource","Action","ScopeType","IsSystem","Status","CreatedAt","UpdatedAt","RowVersion","DeletedAt")
        SELECT gen_random_uuid(), v.code, v.name_ar, v.resource, v.action, v.scope_type, true, 'ACTIVE',
               clock_timestamp(), clock_timestamp(), decode(md5(random()::text || clock_timestamp()::text), 'hex'), NULL
        FROM (VALUES
          ('auth.scope.select','اختيار نطاق التشغيل','auth.scope','select','PLATFORM'),
          ('sync.operations.execute','تنفيذ عمليات المزامنة','sync.operations','execute','BRANCH'),
          ('audit.events.read','قراءة أحداث التدقيق','audit.events','read','BRANCH'),
          ('waybill.view','عرض البوليصة','waybill','view','BRANCH'),
          ('waybill.create','إنشاء البوليصة','waybill','create','BRANCH'),
          ('waybill.edit','تعديل البوليصة','waybill','edit','BRANCH'),
          ('waybill.validate','فحص البوليصة','waybill','validate','BRANCH'),
          ('waybill.submit','إرسال البوليصة للاعتماد','waybill','submit','BRANCH'),
          ('waybill.approve','اعتماد البوليصة','waybill','approve','BRANCH'),
          ('waybill.approval.return','إرجاع البوليصة للتصحيح','waybill','return','BRANCH'),
          ('waybill.cancel','إلغاء البوليصة','waybill','cancel','BRANCH'),
          ('party.view','عرض الأطراف','party','view','BRANCH'),
          ('party.create','إنشاء طرف','party','create','BRANCH'),
          ('waybill.payment.plan','إدارة خطة الدفع','waybill.payment','plan','BRANCH'),
          ('waybill.collection.create','تسجيل التحصيل','waybill.collection','create','BRANCH'),
          ('waybill.collection.reverse','عكس التحصيل','waybill.collection','reverse','BRANCH'),
          ('waybill.release','صرف أصناف البوليصة','waybill.release','execute','BRANCH'),
          ('trip.create','إنشاء رحلة','trip','create','BRANCH'),
          ('waybill.allocate','تخصيص صنف للرحلة','waybill.allocate','execute','BRANCH'),
          ('waybill.unallocate','إلغاء تخصيص الصنف','waybill.unallocate','execute','BRANCH'),
          ('manifest.create','إنشاء كشف التحميل','manifest','create','BRANCH'),
          ('manifest.load','تحميل سطر الكشف','manifest','load','BRANCH'),
          ('manifest.finalize','إقفال كشف التحميل','manifest','finalize','BRANCH'),
          ('manifest.handover','تسليم كشف التحميل','manifest','handover','BRANCH'),
          ('trip.start','بدء الرحلة','trip','start','BRANCH')
        ) AS v(code,name_ar,resource,action,scope_type)
        ON CONFLICT ("Code") DO NOTHING;
        DO $$ BEGIN
          IF EXISTS (
            SELECT 1 FROM transport_erp.permissions p JOIN (VALUES
              ('auth.scope.select','auth.scope','select','PLATFORM'),
              ('sync.operations.execute','sync.operations','execute','BRANCH'),
              ('audit.events.read','audit.events','read','BRANCH'),
              ('waybill.view','waybill','view','BRANCH'),('waybill.create','waybill','create','BRANCH'),
              ('waybill.edit','waybill','edit','BRANCH'),('waybill.validate','waybill','validate','BRANCH'),
              ('waybill.submit','waybill','submit','BRANCH'),('waybill.approve','waybill','approve','BRANCH'),
              ('waybill.approval.return','waybill','return','BRANCH'),('waybill.cancel','waybill','cancel','BRANCH'),
              ('party.view','party','view','BRANCH'),('party.create','party','create','BRANCH'),
              ('waybill.payment.plan','waybill.payment','plan','BRANCH'),
              ('waybill.collection.create','waybill.collection','create','BRANCH'),
              ('waybill.collection.reverse','waybill.collection','reverse','BRANCH'),
              ('waybill.release','waybill.release','execute','BRANCH'),('trip.create','trip','create','BRANCH'),
              ('waybill.allocate','waybill.allocate','execute','BRANCH'),
              ('waybill.unallocate','waybill.unallocate','execute','BRANCH'),
              ('manifest.create','manifest','create','BRANCH'),('manifest.load','manifest','load','BRANCH'),
              ('manifest.finalize','manifest','finalize','BRANCH'),('manifest.handover','manifest','handover','BRANCH'),
              ('trip.start','trip','start','BRANCH')
            ) v(code,resource,action,scope_type) ON p."Code"=v.code
            WHERE p."DeletedAt" IS NOT NULL OR p."Status"<>'ACTIVE' OR NOT p."IsSystem" OR
                  p."Resource"<>v.resource OR p."Action"<>v.action OR p."ScopeType"<>v.scope_type)
          THEN RAISE EXCEPTION 'P1SecurityIdentity blocked: system permission catalog drift'; END IF;
        END $$;
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        DROP TABLE IF EXISTS transport_erp.auth_sessions;
        DROP TABLE IF EXISTS transport_erp.audit_stream_heads;
        ALTER TABLE transport_erp.role_permissions DROP CONSTRAINT IF EXISTS "FK_role_permissions_branches_BranchId_CompanyId", DROP CONSTRAINT IF EXISTS ck_role_permissions_scope_fields;
        ALTER TABLE transport_erp.user_roles DROP CONSTRAINT IF EXISTS "FK_user_roles_branches_BranchId_CompanyId", DROP CONSTRAINT IF EXISTS ck_user_roles_scope_fields;
        ALTER TABLE transport_erp.user_permission_overrides DROP CONSTRAINT IF EXISTS "FK_user_permission_overrides_branches_BranchId_CompanyId", DROP CONSTRAINT IF EXISTS ck_user_permission_overrides_scope_fields;
        DROP INDEX IF EXISTS transport_erp."IX_role_permissions_BranchId_CompanyId";
        DROP INDEX IF EXISTS transport_erp."IX_user_roles_BranchId_CompanyId";
        DROP INDEX IF EXISTS transport_erp."IX_user_permission_overrides_BranchId_CompanyId";
        ALTER TABLE transport_erp.users DROP CONSTRAINT IF EXISTS "FK_users_branches_BranchId_CompanyId", DROP CONSTRAINT IF EXISTS ck_users_security_stamp,
          DROP CONSTRAINT IF EXISTS ck_users_auth_version, DROP CONSTRAINT IF EXISTS ck_users_branch_company;
        DROP INDEX IF EXISTS transport_erp."IX_users_BranchId_CompanyId";
        DROP INDEX IF EXISTS transport_erp."IX_users_NormalizedEmail_CompanyId";
        DROP INDEX IF EXISTS transport_erp."IX_users_NormalizedUserName_CompanyId";
        CREATE UNIQUE INDEX "IX_users_NormalizedUserName_CompanyId" ON transport_erp.users
          ("NormalizedUserName", "CompanyId") WHERE "DeletedAt" IS NULL;
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_users_Email_CompanyId" ON transport_erp.users ("Email","CompanyId") WHERE "Email" IS NOT NULL AND "DeletedAt" IS NULL;
        CREATE INDEX IF NOT EXISTS "IX_users_BranchId" ON transport_erp.users ("BranchId");
        ALTER TABLE transport_erp.users ADD CONSTRAINT "FK_users_branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES transport_erp.branches ("Id") ON DELETE RESTRICT;
        ALTER TABLE transport_erp.users DROP COLUMN "AccessFailedCount", DROP COLUMN "AuthVersion", DROP COLUMN "LockoutEnd",
          DROP COLUMN "NormalizedEmail", DROP COLUMN "SecurityStamp";
        DROP INDEX IF EXISTS transport_erp."IX_audit_events_SequenceNo";
        ALTER TABLE transport_erp.audit_events ALTER COLUMN "SequenceNo" DROP DEFAULT;
        ALTER TABLE transport_erp.audit_events DROP COLUMN "SequenceNo";
        DROP SEQUENCE IF EXISTS transport_erp.audit_event_sequence_no_seq;
        """);
}
