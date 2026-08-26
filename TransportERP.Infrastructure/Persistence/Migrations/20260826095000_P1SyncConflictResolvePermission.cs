using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826095000_P1SyncConflictResolvePermission")]
public partial class P1SyncConflictResolvePermission : Migration
{
    private const string PermissionId = "d1000000-0000-4000-8000-000000000004";

    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql($$"""
        LOCK TABLE transport_erp.permissions,
                   transport_erp.role_permissions,
                   transport_erp.user_permission_overrides
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (
            SELECT 1 FROM transport_erp.permissions
            WHERE "Id"='{{PermissionId}}'::uuid OR "Code"='sync.conflicts.resolve'
          ) THEN
            RAISE EXCEPTION 'P1SyncConflictResolvePermission blocked: permission identity already exists';
          END IF;
        END
        $body$;

        INSERT INTO transport_erp.permissions
          ("Id","Code","NameAr","Resource","Action","ScopeType","IsSystem","Status",
           "CreatedAt","UpdatedAt","RowVersion","DeletedAt")
        VALUES
          ('{{PermissionId}}'::uuid,'sync.conflicts.resolve','حل تعارضات المزامنة',
           'sync.conflicts','resolve','BRANCH',true,'ACTIVE',clock_timestamp(),clock_timestamp(),
           decode(md5(random()::text || clock_timestamp()::text),'hex'),NULL);

        DO $body$
        BEGIN
          IF NOT EXISTS (
            SELECT 1 FROM transport_erp.permissions
            WHERE "Id"='{{PermissionId}}'::uuid AND "Code"='sync.conflicts.resolve'
              AND "NameAr"='حل تعارضات المزامنة' AND "Resource"='sync.conflicts'
              AND "Action"='resolve' AND "ScopeType"='BRANCH' AND "IsSystem"
              AND "Status"='ACTIVE' AND "DeletedAt" IS NULL
          ) THEN
            RAISE EXCEPTION 'P1SyncConflictResolvePermission blocked: inserted permission catalog drift';
          END IF;
        END
        $body$;
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql($$"""
        LOCK TABLE transport_erp.permissions,
                   transport_erp.role_permissions,
                   transport_erp.user_permission_overrides
          IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF NOT EXISTS (
            SELECT 1 FROM transport_erp.permissions
            WHERE "Id"='{{PermissionId}}'::uuid AND "Code"='sync.conflicts.resolve'
              AND "NameAr"='حل تعارضات المزامنة' AND "Resource"='sync.conflicts'
              AND "Action"='resolve' AND "ScopeType"='BRANCH' AND "IsSystem"
              AND "Status"='ACTIVE' AND "DeletedAt" IS NULL
          ) THEN
            RAISE EXCEPTION 'P1SyncConflictResolvePermission down blocked: owned permission missing or drifted';
          END IF;
          IF EXISTS (
            SELECT 1 FROM transport_erp.role_permissions
            WHERE "PermissionId"='{{PermissionId}}'::uuid
          ) OR EXISTS (
            SELECT 1 FROM transport_erp.user_permission_overrides
            WHERE "PermissionId"='{{PermissionId}}'::uuid
          ) THEN
            RAISE EXCEPTION 'P1SyncConflictResolvePermission down blocked: permission references exist';
          END IF;

          DELETE FROM transport_erp.permissions
          WHERE "Id"='{{PermissionId}}'::uuid AND "Code"='sync.conflicts.resolve';
        END
        $body$;
        """);
}
