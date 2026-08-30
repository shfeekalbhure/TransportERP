using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01DArrivalTransitWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_movement_event_c_scope",
                schema: "transport_erp",
                table: "movement_events");

            migrationBuilder.CreateTable(
                name: "arrival_receipts",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivingBranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManifestId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ReceivedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreateClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    LastClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arrival_receipts", x => x.Id);
                    table.CheckConstraint("ck_arrival_receipt_status", "\"Status\" IN ('DRAFT','FINALIZED')");
                    table.ForeignKey(
                        name: "FK_arrival_receipts_manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalSchema: "transport_erp",
                        principalTable: "manifests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_arrival_receipts_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_holdings",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    HoldingType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_holdings", x => x.Id);
                    table.CheckConstraint("ck_warehouse_holding_quantity", "\"Quantity\" >= 0");
                    table.CheckConstraint("ck_warehouse_holding_status", "\"Status\" IN ('AVAILABLE','RESERVED','RELEASED','EXCEPTION')");
                    table.CheckConstraint("ck_warehouse_holding_type", "\"HoldingType\" IN ('TRANSIT','DESTINATION')");
                    table.ForeignKey(
                        name: "FK_warehouse_holdings_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "arrival_receipt_lines",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArrivalReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManifestLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpectedQty = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    ActualQty = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    DifferenceType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DamageQty = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    EvidenceAttachmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arrival_receipt_lines", x => x.Id);
                    table.CheckConstraint("ck_arrival_line_difference", "\"DifferenceType\" IN ('UNVALIDATED','NONE','SHORT','DAMAGE','SHORT_AND_DAMAGE')");
                    table.CheckConstraint("ck_arrival_line_quantities", "\"ExpectedQty\" > 0 AND \"ActualQty\" >= 0 AND \"ActualQty\" <= \"ExpectedQty\" AND \"DamageQty\" >= 0 AND \"DamageQty\" <= \"ActualQty\"");
                    table.ForeignKey(
                        name: "FK_arrival_receipt_lines_arrival_receipts_ArrivalReceiptId",
                        column: x => x.ArrivalReceiptId,
                        principalSchema: "transport_erp",
                        principalTable: "arrival_receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_arrival_receipt_lines_manifest_lines_ManifestLineId",
                        column: x => x.ManifestLineId,
                        principalSchema: "transport_erp",
                        principalTable: "manifest_lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_arrival_receipt_lines_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddCheckConstraint(
                name: "ck_movement_event_c_scope",
                schema: "transport_erp",
                table: "movement_events",
                sql: "\"EventType\" IN ('LOAD','DEPART','ARRIVE','UNLOAD','REALLOCATE')");

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipt_lines_ArrivalReceiptId_ManifestLineId",
                schema: "transport_erp",
                table: "arrival_receipt_lines",
                columns: new[] { "ArrivalReceiptId", "ManifestLineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipt_lines_ManifestLineId",
                schema: "transport_erp",
                table: "arrival_receipt_lines",
                column: "ManifestLineId");

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipt_lines_WaybillItemId_DifferenceType",
                schema: "transport_erp",
                table: "arrival_receipt_lines",
                columns: new[] { "WaybillItemId", "DifferenceType" });

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipts_CompanyId_ReceivingBranchId_CreateClientOp~",
                schema: "transport_erp",
                table: "arrival_receipts",
                columns: new[] { "CompanyId", "ReceivingBranchId", "CreateClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipts_ManifestId",
                schema: "transport_erp",
                table: "arrival_receipts",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipts_Status",
                schema: "transport_erp",
                table: "arrival_receipts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipts_TripId_LocationId",
                schema: "transport_erp",
                table: "arrival_receipts",
                columns: new[] { "TripId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_arrival_receipts_TripId_ManifestId_LocationId",
                schema: "transport_erp",
                table: "arrival_receipts",
                columns: new[] { "TripId", "ManifestId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_holdings_CompanyId_BranchId_WaybillItemId_Locatio~",
                schema: "transport_erp",
                table: "warehouse_holdings",
                columns: new[] { "CompanyId", "BranchId", "WaybillItemId", "LocationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_holdings_LocationId_Status",
                schema: "transport_erp",
                table: "warehouse_holdings",
                columns: new[] { "LocationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_holdings_WaybillItemId",
                schema: "transport_erp",
                table: "warehouse_holdings",
                column: "WaybillItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arrival_receipt_lines",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "warehouse_holdings",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "arrival_receipts",
                schema: "transport_erp");

            migrationBuilder.DropCheckConstraint(
                name: "ck_movement_event_c_scope",
                schema: "transport_erp",
                table: "movement_events");

            migrationBuilder.AddCheckConstraint(
                name: "ck_movement_event_c_scope",
                schema: "transport_erp",
                table: "movement_events",
                sql: "\"EventType\" IN ('LOAD','DEPART')");
        }
    }
}
