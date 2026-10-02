using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Infrastructure.Persistence;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(TransportErpDbContext))]
    [Migration("20261001001000_P2C01CTripSegments")]
    public partial class P2C01CTripSegments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trip_segments",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    SegmentNo = table.Column<int>(type: "integer", nullable: false),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStopId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToStopId = table.Column<Guid>(type: "uuid", nullable: true),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedDepartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    PlannedArriveAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ActualDepartAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ActualArriveAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CustodyStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_segments", x => x.Id);
                    table.CheckConstraint("ck_trip_segment_dates", "\"PlannedArriveAt\" IS NULL OR \"PlannedDepartAt\" IS NULL OR \"PlannedArriveAt\" >= \"PlannedDepartAt\"");
                    table.CheckConstraint("ck_trip_segment_locations", "\"FromLocationId\" <> \"ToLocationId\"");
                    table.CheckConstraint("ck_trip_segment_number", "\"SegmentNo\" > 0");
                    table.CheckConstraint("ck_trip_segment_status", "\"CustodyStatus\" IN ('PLANNED','IN_CUSTODY','ARRIVED','CLOSED','CANCELLED')");
                    table.ForeignKey(
                        name: "FK_trip_segments_trip_stops_FromStopId",
                        column: x => x.FromStopId,
                        principalSchema: "transport_erp",
                        principalTable: "trip_stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_segments_trip_stops_ToStopId",
                        column: x => x.ToStopId,
                        principalSchema: "transport_erp",
                        principalTable: "trip_stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_segments_trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "transport_erp",
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_DriverId_CustodyStatus",
                schema: "transport_erp",
                table: "trip_segments",
                columns: new[] { "DriverId", "CustodyStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_FromStopId",
                schema: "transport_erp",
                table: "trip_segments",
                column: "FromStopId");

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_ToStopId",
                schema: "transport_erp",
                table: "trip_segments",
                column: "ToStopId");

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_TripId_FromLocationId_ToLocationId",
                schema: "transport_erp",
                table: "trip_segments",
                columns: new[] { "TripId", "FromLocationId", "ToLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_TripId_SegmentNo",
                schema: "transport_erp",
                table: "trip_segments",
                columns: new[] { "TripId", "SegmentNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trip_segments_VehicleId_CustodyStatus",
                schema: "transport_erp",
                table: "trip_segments",
                columns: new[] { "VehicleId", "CustodyStatus" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trip_segments",
                schema: "transport_erp");
        }
    }
}
