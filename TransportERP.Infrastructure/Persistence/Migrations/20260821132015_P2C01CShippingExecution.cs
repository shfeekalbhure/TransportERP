using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01CShippingExecution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "item_releases",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ReleasedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReversalOfId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_releases", x => x.Id);
                    table.CheckConstraint("ck_item_release_quantity", "\"Quantity\" > 0");
                    table.CheckConstraint("ck_item_release_reversal_shape", "(\"Status\" = 'ACTIVE' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
                    table.CheckConstraint("ck_item_release_status", "\"Status\" IN ('ACTIVE','REVERSED')");
                    table.ForeignKey(
                        name: "FK_item_releases_item_releases_ReversalOfId",
                        column: x => x.ReversalOfId,
                        principalSchema: "transport_erp",
                        principalTable: "item_releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_releases_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripNo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedDepartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActualDepartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ActualArriveAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreateClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    LastClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.Id);
                    table.CheckConstraint("ck_trip_status", "\"Status\" IN ('DRAFT','READY','DEPARTED','ARRIVED','CLOSED','CANCELLED')");
                });

            migrationBuilder.CreateTable(
                name: "waybill_holds",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    HoldType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PlacedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    PlacedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ReleasedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waybill_holds", x => x.Id);
                    table.CheckConstraint("ck_waybill_hold_status", "\"Status\" IN ('ACTIVE','RELEASED')");
                    table.ForeignKey(
                        name: "FK_waybill_holds_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "manifests",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManifestNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HandoverAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    DriverAcceptedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreateClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    LastClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manifests", x => x.Id);
                    table.CheckConstraint("ck_manifest_status", "\"Status\" IN ('DRAFT','FINALIZED','HANDED_OVER','ACCEPTED','CLOSED')");
                    table.ForeignKey(
                        name: "FK_manifests_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trip_allocations",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    AllocatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ClientOperationId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReversalOfId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_allocations", x => x.Id);
                    table.CheckConstraint("ck_trip_allocation_quantity", "\"Quantity\" > 0");
                    table.CheckConstraint("ck_trip_allocation_reversal_shape", "(\"Status\" = 'ALLOCATED' AND \"ReversalOfId\" IS NULL) OR (\"Status\" = 'REVERSED' AND \"ReversalOfId\" IS NOT NULL)");
                    table.CheckConstraint("ck_trip_allocation_status", "\"Status\" IN ('ALLOCATED','REVERSED')");
                    table.ForeignKey(
                        name: "FK_trip_allocations_item_releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalSchema: "transport_erp",
                        principalTable: "item_releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_allocations_trip_allocations_ReversalOfId",
                        column: x => x.ReversalOfId,
                        principalSchema: "transport_erp",
                        principalTable: "trip_allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_allocations_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_allocations_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trip_stops",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    StopNo = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StopType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PlannedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ArrivedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    DepartedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_stops", x => x.Id);
                    table.CheckConstraint("ck_trip_stop_status", "\"Status\" IN ('PLANNED','ARRIVED','DEPARTED','SKIPPED')");
                    table.ForeignKey(
                        name: "FK_trip_stops_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "manifest_lines",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManifestId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    LoadedQuantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    Volume = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    LoadStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manifest_lines", x => x.Id);
                    table.CheckConstraint("ck_manifest_line_load_status", "\"LoadStatus\" IN ('PLANNED','PARTIAL','LOADED','CANCELLED')");
                    table.CheckConstraint("ck_manifest_line_quantities", "\"Quantity\" > 0 AND \"LoadedQuantity\" >= 0 AND \"LoadedQuantity\" <= \"Quantity\"");
                    table.ForeignKey(
                        name: "FK_manifest_lines_manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalSchema: "transport_erp",
                        principalTable: "manifests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_manifest_lines_trip_allocations_AllocationId",
                        column: x => x.AllocationId,
                        principalSchema: "transport_erp",
                        principalTable: "trip_allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_manifest_lines_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_manifest_lines_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movement_events",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    AllocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManifestLineId = table.Column<Guid>(type: "uuid", nullable: true),
                    EventType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManifestId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RecordedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RecordedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ReversesEventId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClientOperationId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movement_events", x => x.Id);
                    table.CheckConstraint("ck_movement_event_c_scope", "\"EventType\" IN ('LOAD','DEPART')");
                    table.CheckConstraint("ck_movement_event_quantity", "\"Quantity\" IS NULL OR \"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_movement_events_manifest_lines_ManifestLineId",
                        column: x => x.ManifestLineId,
                        principalSchema: "transport_erp",
                        principalTable: "manifest_lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_manifests_ManifestId",
                        column: x => x.ManifestId,
                        principalSchema: "transport_erp",
                        principalTable: "manifests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_movement_events_ReversesEventId",
                        column: x => x.ReversesEventId,
                        principalSchema: "transport_erp",
                        principalTable: "movement_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_trip_allocations_AllocationId",
                        column: x => x.AllocationId,
                        principalSchema: "transport_erp",
                        principalTable: "trip_allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_waybill_items_WaybillItemId",
                        column: x => x.WaybillItemId,
                        principalSchema: "transport_erp",
                        principalTable: "waybill_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_movement_events_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_releases_CompanyId_BranchId_ClientOperationId",
                schema: "transport_erp",
                table: "item_releases",
                columns: new[] { "CompanyId", "BranchId", "ClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_releases_ReversalOfId",
                schema: "transport_erp",
                table: "item_releases",
                column: "ReversalOfId",
                unique: true,
                filter: "\"ReversalOfId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_item_releases_WaybillItemId_Status",
                schema: "transport_erp",
                table: "item_releases",
                columns: new[] { "WaybillItemId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_manifest_lines_AllocationId",
                schema: "transport_erp",
                table: "manifest_lines",
                column: "AllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_manifest_lines_ManifestId_AllocationId",
                schema: "transport_erp",
                table: "manifest_lines",
                columns: new[] { "ManifestId", "AllocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_manifest_lines_WaybillId",
                schema: "transport_erp",
                table: "manifest_lines",
                column: "WaybillId");

            migrationBuilder.CreateIndex(
                name: "IX_manifest_lines_WaybillItemId",
                schema: "transport_erp",
                table: "manifest_lines",
                column: "WaybillItemId");

            migrationBuilder.CreateIndex(
                name: "IX_manifests_CompanyId_BranchId_CreateClientOperationId",
                schema: "transport_erp",
                table: "manifests",
                columns: new[] { "CompanyId", "BranchId", "CreateClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_manifests_TripId_ManifestNo",
                schema: "transport_erp",
                table: "manifests",
                columns: new[] { "TripId", "ManifestNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_manifests_TripId_Status",
                schema: "transport_erp",
                table: "manifests",
                columns: new[] { "TripId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_AllocationId",
                schema: "transport_erp",
                table: "movement_events",
                column: "AllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_CompanyId_ClientOperationId",
                schema: "transport_erp",
                table: "movement_events",
                columns: new[] { "CompanyId", "ClientOperationId" },
                unique: true,
                filter: "\"ClientOperationId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_ManifestId",
                schema: "transport_erp",
                table: "movement_events",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_ManifestLineId_EventType",
                schema: "transport_erp",
                table: "movement_events",
                columns: new[] { "ManifestLineId", "EventType" });

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_ReversesEventId",
                schema: "transport_erp",
                table: "movement_events",
                column: "ReversesEventId");

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_TripId",
                schema: "transport_erp",
                table: "movement_events",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_WaybillId_OccurredAt",
                schema: "transport_erp",
                table: "movement_events",
                columns: new[] { "WaybillId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_movement_events_WaybillItemId_OccurredAt",
                schema: "transport_erp",
                table: "movement_events",
                columns: new[] { "WaybillItemId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_allocations_CompanyId_BranchId_ClientOperationId",
                schema: "transport_erp",
                table: "trip_allocations",
                columns: new[] { "CompanyId", "BranchId", "ClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trip_allocations_ReleaseId_Status",
                schema: "transport_erp",
                table: "trip_allocations",
                columns: new[] { "ReleaseId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_allocations_ReversalOfId",
                schema: "transport_erp",
                table: "trip_allocations",
                column: "ReversalOfId",
                unique: true,
                filter: "\"ReversalOfId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_trip_allocations_TripId_Status",
                schema: "transport_erp",
                table: "trip_allocations",
                columns: new[] { "TripId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_allocations_WaybillItemId_Status",
                schema: "transport_erp",
                table: "trip_allocations",
                columns: new[] { "WaybillItemId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_stops_LocationId_Status",
                schema: "transport_erp",
                table: "trip_stops",
                columns: new[] { "LocationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_stops_TripId_StopNo",
                schema: "transport_erp",
                table: "trip_stops",
                columns: new[] { "TripId", "StopNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_CompanyId_BranchId_CreateClientOperationId",
                schema: "transport_erp",
                table: "trips",
                columns: new[] { "CompanyId", "BranchId", "CreateClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_CompanyId_TripNo",
                schema: "transport_erp",
                table: "trips",
                columns: new[] { "CompanyId", "TripNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_DriverId_Status",
                schema: "transport_erp",
                table: "trips",
                columns: new[] { "DriverId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_trips_VehicleId_Status",
                schema: "transport_erp",
                table: "trips",
                columns: new[] { "VehicleId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_waybill_holds_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "waybill_holds",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_waybill_holds_WaybillId_Status",
                schema: "transport_erp",
                table: "waybill_holds",
                columns: new[] { "WaybillId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movement_events",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "trip_stops",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "waybill_holds",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "manifest_lines",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "manifests",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "trip_allocations",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "item_releases",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "trips",
                schema: "transport_erp");
        }
    }
}
