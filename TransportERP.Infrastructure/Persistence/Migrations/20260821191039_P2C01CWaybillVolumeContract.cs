using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01CWaybillVolumeContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_waybill_items_measurements",
                schema: "transport_erp",
                table: "waybill_items");

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                schema: "transport_erp",
                table: "waybill_items",
                type: "numeric(19,4)",
                precision: 19,
                scale: 4,
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_waybill_items_measurements",
                schema: "transport_erp",
                table: "waybill_items",
                sql: "(\"Weight\" IS NULL OR \"Weight\" >= 0) AND (\"Length\" IS NULL OR \"Length\" >= 0) AND (\"Width\" IS NULL OR \"Width\" >= 0) AND (\"Height\" IS NULL OR \"Height\" >= 0) AND (\"Volume\" IS NULL OR \"Volume\" >= 0) AND (\"DeclaredValue\" IS NULL OR \"DeclaredValue\" >= 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_waybill_items_measurements",
                schema: "transport_erp",
                table: "waybill_items");

            migrationBuilder.DropColumn(
                name: "Volume",
                schema: "transport_erp",
                table: "waybill_items");

            migrationBuilder.AddCheckConstraint(
                name: "ck_waybill_items_measurements",
                schema: "transport_erp",
                table: "waybill_items",
                sql: "(\"Weight\" IS NULL OR \"Weight\" >= 0) AND (\"Length\" IS NULL OR \"Length\" >= 0) AND (\"Width\" IS NULL OR \"Width\" >= 0) AND (\"Height\" IS NULL OR \"Height\" >= 0) AND (\"DeclaredValue\" IS NULL OR \"DeclaredValue\" >= 0)");
        }
    }
}
