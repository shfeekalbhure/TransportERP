using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826080000_P1Stage4SyncExecutionClaimFoundation")]
public partial class P1Stage4SyncExecutionClaimFoundation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (
            SELECT 1 FROM transport_erp.sync_operations WHERE "Status"='SENDING'
          ) THEN
            RAISE EXCEPTION 'STAGE4_EXECUTION_CLAIM_PREFLIGHT_SENDING_PRESENT';
          END IF;
        END $body$;

        ALTER TABLE transport_erp.sync_operations
          ADD COLUMN "ExecutionClaimToken" uuid NULL,
          ADD COLUMN "ExecutionAttemptStartedAt" timestamptz NULL,
          ADD COLUMN "ExecutionLeaseExpiresAt" timestamptz NULL,
          ADD CONSTRAINT ck_sync_execution_claim_bundle CHECK (
            ("Status"='SENDING' AND
             "ExecutionClaimToken" IS NOT NULL AND
             "ExecutionClaimToken"<>'00000000-0000-0000-0000-000000000000'::uuid AND
             "ExecutionAttemptStartedAt" IS NOT NULL AND
             "ExecutionLeaseExpiresAt" IS NOT NULL AND
             "ExecutionLeaseExpiresAt">"ExecutionAttemptStartedAt")
            OR
            ("Status"<>'SENDING' AND
             "ExecutionClaimToken" IS NULL AND
             "ExecutionAttemptStartedAt" IS NULL AND
             "ExecutionLeaseExpiresAt" IS NULL)
          );

        CREATE UNIQUE INDEX ux_sync_operation_execution_claim
          ON transport_erp.sync_operations ("ExecutionClaimToken")
          WHERE "ExecutionClaimToken" IS NOT NULL;

        CREATE INDEX ix_sync_operation_execution_queue
          ON transport_erp.sync_operations
            ("Status", "NextRetryAt", "ExecutionLeaseExpiresAt", "CreatedAt");
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.sync_operations IN ACCESS EXCLUSIVE MODE;

        DO $body$
        BEGIN
          IF EXISTS (
            SELECT 1
            FROM transport_erp.sync_operations
            WHERE "Status"='SENDING'
               OR "ExecutionClaimToken" IS NOT NULL
               OR "ExecutionAttemptStartedAt" IS NOT NULL
               OR "ExecutionLeaseExpiresAt" IS NOT NULL
          ) THEN
            RAISE EXCEPTION 'STAGE4_EXECUTION_CLAIM_DOWN_BLOCKED_ACTIVE_CLAIM';
          END IF;
        END $body$;

        DROP INDEX transport_erp.ix_sync_operation_execution_queue;
        DROP INDEX transport_erp.ux_sync_operation_execution_claim;
        ALTER TABLE transport_erp.sync_operations
          DROP CONSTRAINT ck_sync_execution_claim_bundle,
          DROP COLUMN "ExecutionLeaseExpiresAt",
          DROP COLUMN "ExecutionAttemptStartedAt",
          DROP COLUMN "ExecutionClaimToken";
        """);
}
