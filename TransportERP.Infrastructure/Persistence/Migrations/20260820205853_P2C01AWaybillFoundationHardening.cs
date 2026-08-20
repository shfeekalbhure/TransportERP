using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01AWaybillFoundationHardening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_waybills_number_state",
                schema: "transport_erp",
                table: "waybills");

            migrationBuilder.DropIndex(
                name: "IX_number_sequences_CompanyId_BranchId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences");

            migrationBuilder.AddCheckConstraint(
                name: "ck_waybills_number_state",
                schema: "transport_erp",
                table: "waybills",
                sql: "(\"Status\" = 'APPROVED' AND \"WaybillNo\" IS NOT NULL) OR (\"Status\" <> 'APPROVED' AND \"WaybillNo\" IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_number_sequences_CompanyId_BranchId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences",
                columns: new[] { "CompanyId", "BranchId", "DocumentType" },
                unique: true,
                filter: "\"BranchId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_number_sequences_CompanyId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences",
                columns: new[] { "CompanyId", "DocumentType" },
                unique: true,
                filter: "\"BranchId\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_waybills_number_state",
                schema: "transport_erp",
                table: "waybills");

            migrationBuilder.DropIndex(
                name: "IX_number_sequences_CompanyId_BranchId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences");

            migrationBuilder.DropIndex(
                name: "IX_number_sequences_CompanyId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences");

            migrationBuilder.AddCheckConstraint(
                name: "ck_waybills_number_state",
                schema: "transport_erp",
                table: "waybills",
                sql: "(\"Status\" = 'APPROVED' AND \"WaybillNo\" IS NOT NULL) OR (\"Status\" <> 'APPROVED')");

            migrationBuilder.CreateIndex(
                name: "IX_number_sequences_CompanyId_BranchId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences",
                columns: new[] { "CompanyId", "BranchId", "DocumentType" },
                unique: true);
        }
    }
}
