using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01DShipmentException : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shipment_exceptions",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: true),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExceptionType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Severity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ResolutionNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_exceptions", x => x.Id);
                    table.CheckConstraint("ck_shipment_exception_severity", "\"Severity\" IN ('BLOCKING','WARNING','INFO')");
                    table.CheckConstraint("ck_shipment_exception_status", "\"Status\" IN ('OPEN','RESOLVED')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_exceptions_CompanyId_BranchId_TripId_Status",
                schema: "transport_erp",
                table: "shipment_exceptions",
                columns: new[] { "CompanyId", "BranchId", "TripId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_shipment_exceptions_TripId_Status",
                schema: "transport_erp",
                table: "shipment_exceptions",
                columns: new[] { "TripId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipment_exceptions",
                schema: "transport_erp");
        }
    }
}
