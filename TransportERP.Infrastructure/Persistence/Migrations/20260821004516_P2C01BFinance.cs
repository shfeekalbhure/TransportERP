using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01BFinance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinancialStatus",
                schema: "transport_erp",
                table: "waybills",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "UNPAID");

            migrationBuilder.CreateTable(
                name: "collection_transactions",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PartyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMethodCode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(19,8)", precision: 19, scale: 8, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CollectedByType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CollectedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccountingReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReversalOfId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReversalReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_transactions", x => x.Id);
                    table.CheckConstraint("ck_collection_amount", "\"Amount\" > 0 AND \"ExchangeRate\" > 0");
                    table.CheckConstraint("ck_collection_reversal_shape", "(\"Status\" = 'ACCEPTED' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
                    table.CheckConstraint("ck_collection_status", "\"Status\" IN ('ACCEPTED','REVERSED')");
                    table.ForeignKey(
                        name: "FK_collection_transactions_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_collection_transactions_collection_transactions_ReversalOfId",
                        column: x => x.ReversalOfId,
                        principalSchema: "transport_erp",
                        principalTable: "collection_transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_collection_transactions_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_collection_transactions_operational_parties_PartyId",
                        column: x => x.PartyId,
                        principalSchema: "transport_erp",
                        principalTable: "operational_parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_collection_transactions_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payment_plan_lines",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNo = table.Column<int>(type: "integer", nullable: false),
                    PayerRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PartyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMethodCode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    AmountCurrencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    Percent = table.Column<decimal>(type: "numeric(9,4)", precision: 9, scale: 4, nullable: true),
                    DueTrigger = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    DueAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_plan_lines", x => x.Id);
                    table.CheckConstraint("ck_payment_plan_amount", "\"Amount\" IS NULL OR \"Amount\" > 0");
                    table.CheckConstraint("ck_payment_plan_mode", "(\"Amount\" IS NOT NULL AND \"Percent\" IS NULL) OR (\"Amount\" IS NULL AND \"Percent\" IS NOT NULL)");
                    table.CheckConstraint("ck_payment_plan_percent", "\"Percent\" IS NULL OR (\"Percent\" > 0 AND \"Percent\" <= 100)");
                    table.CheckConstraint("ck_payment_plan_status", "\"Status\" IN ('DRAFT','ACTIVE','SATISFIED','CANCELLED')");
                    table.ForeignKey(
                        name: "FK_payment_plan_lines_currencies_AmountCurrencyId",
                        column: x => x.AmountCurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_plan_lines_operational_parties_PartyId",
                        column: x => x.PartyId,
                        principalSchema: "transport_erp",
                        principalTable: "operational_parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_plan_lines_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "waybill_financial_links",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waybill_financial_links", x => x.Id);
                    table.CheckConstraint("ck_financial_link_status", "\"Status\" IN ('ACTIVE','REVERSED')");
                    table.ForeignKey(
                        name: "FK_waybill_financial_links_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_waybill_financial_links_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CompanyId_BranchId_FinancialStatus",
                schema: "transport_erp",
                table: "waybills",
                columns: new[] { "CompanyId", "BranchId", "FinancialStatus" });

            migrationBuilder.AddCheckConstraint(
                name: "ck_waybills_financial_status",
                schema: "transport_erp",
                table: "waybills",
                sql: "\"FinancialStatus\" IN ('UNPAID','PARTIAL','PAID','OVERPAID')");

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_BranchId",
                schema: "transport_erp",
                table: "collection_transactions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_CollectedById_CollectedAt",
                schema: "transport_erp",
                table: "collection_transactions",
                columns: new[] { "CollectedById", "CollectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_CompanyId_ClientOperationId",
                schema: "transport_erp",
                table: "collection_transactions",
                columns: new[] { "CompanyId", "ClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_CurrencyId",
                schema: "transport_erp",
                table: "collection_transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_PartyId",
                schema: "transport_erp",
                table: "collection_transactions",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_ReversalOfId",
                schema: "transport_erp",
                table: "collection_transactions",
                column: "ReversalOfId",
                unique: true,
                filter: "\"ReversalOfId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_collection_transactions_WaybillId_Status",
                schema: "transport_erp",
                table: "collection_transactions",
                columns: new[] { "WaybillId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_plan_lines_AmountCurrencyId",
                schema: "transport_erp",
                table: "payment_plan_lines",
                column: "AmountCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_plan_lines_PartyId",
                schema: "transport_erp",
                table: "payment_plan_lines",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_plan_lines_WaybillId_LineNo",
                schema: "transport_erp",
                table: "payment_plan_lines",
                columns: new[] { "WaybillId", "LineNo" },
                unique: true,
                filter: "\"Status\" = 'ACTIVE'");

            migrationBuilder.CreateIndex(
                name: "IX_payment_plan_lines_WaybillId_Status",
                schema: "transport_erp",
                table: "payment_plan_lines",
                columns: new[] { "WaybillId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_waybill_financial_links_CurrencyId",
                schema: "transport_erp",
                table: "waybill_financial_links",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_waybill_financial_links_DocumentId",
                schema: "transport_erp",
                table: "waybill_financial_links",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_waybill_financial_links_WaybillId_DocumentType_DocumentId_L~",
                schema: "transport_erp",
                table: "waybill_financial_links",
                columns: new[] { "WaybillId", "DocumentType", "DocumentId", "LinkType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collection_transactions",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "payment_plan_lines",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "waybill_financial_links",
                schema: "transport_erp");

            migrationBuilder.DropIndex(
                name: "IX_waybills_CompanyId_BranchId_FinancialStatus",
                schema: "transport_erp",
                table: "waybills");

            migrationBuilder.DropCheckConstraint(
                name: "ck_waybills_financial_status",
                schema: "transport_erp",
                table: "waybills");

            migrationBuilder.DropColumn(
                name: "FinancialStatus",
                schema: "transport_erp",
                table: "waybills");
        }
    }
}
