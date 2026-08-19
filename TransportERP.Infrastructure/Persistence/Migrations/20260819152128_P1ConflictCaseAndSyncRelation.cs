using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P1ConflictCaseAndSyncRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM transport_erp.sync_operations
        WHERE ""ConflictCaseId"" IS NOT NULL
    ) THEN
        RAISE EXCEPTION 'Cannot drop legacy ConflictCaseId values before an approved data migration.';
    END IF;
END $$;");

            migrationBuilder.DropColumn(
                name: "ConflictCaseId",
                schema: "transport_erp",
                table: "sync_operations");

            migrationBuilder.CreateTable(
                name: "conflict_cases",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SyncOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseVersion = table.Column<long>(type: "bigint", nullable: true),
                    DeviceSnapshot = table.Column<string>(type: "text", nullable: false),
                    ServerSnapshot = table.Column<string>(type: "text", nullable: false),
                    ConflictReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Resolution = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ResolvedBy = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReplacedByOperationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conflict_cases", x => x.Id);
                    table.CheckConstraint("ck_conflict_case_status", "\"Status\" IN ('OPEN','RESOLVED')");
                    table.ForeignKey(
                        name: "FK_conflict_cases_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_conflict_cases_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_conflict_cases_sync_operations_ReplacedByOperationId",
                        column: x => x.ReplacedByOperationId,
                        principalSchema: "transport_erp",
                        principalTable: "sync_operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_conflict_cases_sync_operations_SyncOperationId",
                        column: x => x.SyncOperationId,
                        principalSchema: "transport_erp",
                        principalTable: "sync_operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conflict_cases_BranchId",
                schema: "transport_erp",
                table: "conflict_cases",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_conflict_cases_CompanyId_BranchId_Status_CreatedAt",
                schema: "transport_erp",
                table: "conflict_cases",
                columns: new[] { "CompanyId", "BranchId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_conflict_cases_ReplacedByOperationId",
                schema: "transport_erp",
                table: "conflict_cases",
                column: "ReplacedByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_conflict_cases_SyncOperationId",
                schema: "transport_erp",
                table: "conflict_cases",
                column: "SyncOperationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conflict_cases",
                schema: "transport_erp");

            migrationBuilder.AddColumn<Guid>(
                name: "ConflictCaseId",
                schema: "transport_erp",
                table: "sync_operations",
                type: "uuid",
                nullable: true);
        }
    }
}
