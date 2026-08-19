using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P1AuditAppendOnlyAndOutcome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Outcome",
                schema: "transport_erp",
                table: "audit_events",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION transport_erp.prevent_audit_event_mutation()
                RETURNS trigger
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RAISE EXCEPTION 'audit_events are append-only; UPDATE and DELETE are forbidden';
                END;
                $$;

                DROP TRIGGER IF EXISTS trg_audit_events_append_only ON transport_erp.audit_events;
                CREATE TRIGGER trg_audit_events_append_only
                BEFORE UPDATE OR DELETE ON transport_erp.audit_events
                FOR EACH ROW
                EXECUTE FUNCTION transport_erp.prevent_audit_event_mutation();
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS trg_audit_events_append_only ON transport_erp.audit_events;
                DROP FUNCTION IF EXISTS transport_erp.prevent_audit_event_mutation();
                """);

            migrationBuilder.DropColumn(
                name: "Outcome",
                schema: "transport_erp",
                table: "audit_events");
        }
    }
}
