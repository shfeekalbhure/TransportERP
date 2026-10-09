using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GeneralLedgerWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Catalog only: no roles, users, configuration values or existing documents are changed.
            foreach (var (id, code, label, scope) in new[] {
                ("63f8e2d4-3256-4e58-8a65-1b291011caf1", "accounting.general-ledger.view", "عرض إعدادات الأستاذ العام", "COMPANY"),
                ("63f8e2d4-3256-4e58-8a65-1b291011caf2", "accounting.general-ledger.configure", "إعداد سياسة الأستاذ العام للشركة", "COMPANY"),
                ("63f8e2d4-3256-4e58-8a65-1b291011caf3", "accounting.receipts.review", "مراجعة سند القبض", "BRANCH") })
                migrationBuilder.Sql($"""
                    INSERT INTO transport_erp.permissions
                    ("Id", "Code", "NameAr", "Resource", "Action", "ScopeType", "IsSystem", "Status", "CreatedAt", "UpdatedAt", "RowVersion")
                    VALUES ('{id}', '{code}', '{label}', 'Accounting', '{code}', '{scope}', TRUE, 'ACTIVE', NOW(), NOW(), decode(replace('{id}', '-', ''), 'hex'))
                    ON CONFLICT ("Code") DO NOTHING;
                    """);
            migrationBuilder.DropCheckConstraint(
                name: "ck_receipts_status",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.CreateTable(
                name: "accounting_workflow_snapshots",
                schema: "transport_erp",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyJson = table.Column<string>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounting_workflow_snapshots", x => new { x.DocumentType, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_accounting_workflow_snapshots_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_accounting_workflow_snapshots_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddCheckConstraint(
                name: "ck_receipts_status",
                schema: "transport_erp",
                table: "receipt_vouchers",
                sql: "\"Status\" IN ('DRAFT','REVIEWED','APPROVED','POSTED','CANCELLED')");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_workflow_snapshots_BranchId",
                schema: "transport_erp",
                table: "accounting_workflow_snapshots",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_accounting_workflow_snapshots_CompanyId",
                schema: "transport_erp",
                table: "accounting_workflow_snapshots",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$ BEGIN
                  IF EXISTS (SELECT 1 FROM transport_erp.accounting_workflow_snapshots) THEN
                    RAISE EXCEPTION 'Cannot remove frozen document workflow policies. Preserve/migrate document history before downgrade.';
                  END IF;
                END $$;
                """);
            migrationBuilder.DropTable(
                name: "accounting_workflow_snapshots",
                schema: "transport_erp");

            migrationBuilder.DropCheckConstraint(
                name: "ck_receipts_status",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.AddCheckConstraint(
                name: "ck_receipts_status",
                schema: "transport_erp",
                table: "receipt_vouchers",
                sql: "\"Status\" IN ('DRAFT','APPROVED','POSTED','CANCELLED')");
        }
    }
}
