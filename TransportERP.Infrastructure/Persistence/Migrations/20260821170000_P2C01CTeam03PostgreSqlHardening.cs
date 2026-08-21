using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260821170000_P2C01CTeam03PostgreSqlHardening")]
public sealed class P2C01CTeam03PostgreSqlHardening : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS transport_erp.shipping_command_outcomes
            (
                "Id" uuid NOT NULL,
                "CompanyId" uuid NOT NULL,
                "BranchId" uuid NOT NULL,
                "Action" character varying(40) NOT NULL,
                "ClientOperationId" character varying(160) NOT NULL,
                "AggregateType" character varying(40) NOT NULL,
                "AggregateId" uuid NOT NULL,
                "RequestFingerprint" character(64) NOT NULL,
                "ResponseJson" text NOT NULL,
                "CreatedAt" timestamptz NOT NULL,
                CONSTRAINT "PK_shipping_command_outcomes" PRIMARY KEY ("Id")
            );

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_shipping_command_outcomes_scope_action_operation"
                ON transport_erp.shipping_command_outcomes
                ("CompanyId", "BranchId", "Action", "ClientOperationId");

            CREATE INDEX IF NOT EXISTS "IX_shipping_command_outcomes_scope_aggregate"
                ON transport_erp.shipping_command_outcomes
                ("CompanyId", "BranchId", "AggregateType", "AggregateId");

            CREATE OR REPLACE FUNCTION transport_erp.p2_c01_c_reject_append_only_mutation()
            RETURNS trigger
            LANGUAGE plpgsql
            AS $$
            BEGIN
                RAISE EXCEPTION USING
                    ERRCODE = '55000',
                    MESSAGE = TG_TABLE_SCHEMA || '.' || TG_TABLE_NAME || ' is append-only';
            END;
            $$;

            CREATE TRIGGER trg_item_releases_append_only
                BEFORE UPDATE OR DELETE ON transport_erp.item_releases
                FOR EACH ROW EXECUTE FUNCTION transport_erp.p2_c01_c_reject_append_only_mutation();

            CREATE TRIGGER trg_trip_allocations_append_only
                BEFORE UPDATE OR DELETE ON transport_erp.trip_allocations
                FOR EACH ROW EXECUTE FUNCTION transport_erp.p2_c01_c_reject_append_only_mutation();

            CREATE TRIGGER trg_movement_events_append_only
                BEFORE UPDATE OR DELETE ON transport_erp.movement_events
                FOR EACH ROW EXECUTE FUNCTION transport_erp.p2_c01_c_reject_append_only_mutation();

            CREATE TRIGGER trg_shipping_command_outcomes_append_only
                BEFORE UPDATE OR DELETE ON transport_erp.shipping_command_outcomes
                FOR EACH ROW EXECUTE FUNCTION transport_erp.p2_c01_c_reject_append_only_mutation();
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TRIGGER IF EXISTS trg_shipping_command_outcomes_append_only
                ON transport_erp.shipping_command_outcomes;
            DROP TRIGGER IF EXISTS trg_movement_events_append_only
                ON transport_erp.movement_events;
            DROP TRIGGER IF EXISTS trg_trip_allocations_append_only
                ON transport_erp.trip_allocations;
            DROP TRIGGER IF EXISTS trg_item_releases_append_only
                ON transport_erp.item_releases;
            DROP FUNCTION IF EXISTS transport_erp.p2_c01_c_reject_append_only_mutation();
            DROP TABLE IF EXISTS transport_erp.shipping_command_outcomes;
            """);
    }
}
