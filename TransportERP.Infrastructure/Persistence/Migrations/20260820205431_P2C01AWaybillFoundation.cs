using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P2C01AWaybillFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "number_sequences",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    DocumentType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    NextValue = table.Column<long>(type: "bigint", nullable: false),
                    ResetPolicy = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_number_sequences", x => x.Id);
                    table.CheckConstraint("ck_number_sequences_next", "\"NextValue\" >= 1");
                    table.CheckConstraint("ck_number_sequences_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                    table.CheckConstraint("ck_number_sequences_version", "\"Version\" >= 1");
                    table.ForeignKey(
                        name: "FK_number_sequences_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_number_sequences_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operational_parties",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    PartyNo = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Mobile = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IdentityType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    IdentityNo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    GovernorateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressLine = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClientOperationId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_parties", x => x.Id);
                    table.CheckConstraint("ck_operational_parties_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                    table.CheckConstraint("ck_operational_parties_version", "\"Version\" >= 1");
                    table.ForeignKey(
                        name: "FK_operational_parties_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operational_parties_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "waybills",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    DraftNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    WaybillNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    WaybillDateTime = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ServiceType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Priority = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    OriginId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(19,8)", precision: 19, scale: 8, nullable: false),
                    FreightTotal = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    DiscountTotal = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreateClientOperationId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    LastClientOperationId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waybills", x => x.Id);
                    table.CheckConstraint("ck_waybills_amounts", "\"FreightTotal\" >= 0 AND \"DiscountTotal\" >= 0 AND \"DiscountTotal\" <= \"FreightTotal\"");
                    table.CheckConstraint("ck_waybills_exchange_rate", "\"ExchangeRate\" > 0");
                    table.CheckConstraint("ck_waybills_number_state", "(\"Status\" = 'APPROVED' AND \"WaybillNo\" IS NOT NULL) OR (\"Status\" <> 'APPROVED')");
                    table.CheckConstraint("ck_waybills_status", "\"Status\" IN ('DRAFT','READY_FOR_APPROVAL','APPROVED','CANCELLED')");
                    table.CheckConstraint("ck_waybills_version", "\"Version\" >= 1");
                    table.ForeignKey(
                        name: "FK_waybills_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_waybills_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_waybills_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "number_reservations",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SequenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    NumberValue = table.Column<long>(type: "bigint", nullable: false),
                    RenderedNumber = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ReservedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CommittedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    VoidedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    VoidReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    State = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastTransitionKey = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_number_reservations", x => x.Id);
                    table.CheckConstraint("ck_number_reservations_state", "\"State\" IN ('RESERVED','COMMITTED','VOID')");
                    table.ForeignKey(
                        name: "FK_number_reservations_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_number_reservations_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_number_reservations_number_sequences_SequenceId",
                        column: x => x.SequenceId,
                        principalSchema: "transport_erp",
                        principalTable: "number_sequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_number_reservations_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "waybill_items",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNo = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Contents = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    Pieces = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    Length = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    Width = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    Height = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    DeclaredValue = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: true),
                    OriginCountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    RiskFlagsJson = table.Column<string>(type: "jsonb", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waybill_items", x => x.Id);
                    table.CheckConstraint("ck_waybill_items_measurements", "(\"Weight\" IS NULL OR \"Weight\" >= 0) AND (\"Length\" IS NULL OR \"Length\" >= 0) AND (\"Width\" IS NULL OR \"Width\" >= 0) AND (\"Height\" IS NULL OR \"Height\" >= 0) AND (\"DeclaredValue\" IS NULL OR \"DeclaredValue\" >= 0)");
                    table.CheckConstraint("ck_waybill_items_pieces", "\"Pieces\" IS NULL OR \"Pieces\" > 0");
                    table.CheckConstraint("ck_waybill_items_quantity", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_waybill_items_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "waybill_parties",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OperationalPartyId = table.Column<Guid>(type: "uuid", nullable: true),
                    NameSnapshot = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    MobileSnapshot = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IdentityTypeSnapshot = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    IdentityNoSnapshot = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    GovernorateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CityId = table.Column<Guid>(type: "uuid", nullable: true),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressLineSnapshot = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waybill_parties", x => x.Id);
                    table.CheckConstraint("ck_waybill_parties_role", "\"Role\" IN ('SENDER','RECEIVER','PAYER')");
                    table.ForeignKey(
                        name: "FK_waybill_parties_operational_parties_OperationalPartyId",
                        column: x => x.OperationalPartyId,
                        principalSchema: "transport_erp",
                        principalTable: "operational_parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_waybill_parties_waybills_WaybillId",
                        column: x => x.WaybillId,
                        principalSchema: "transport_erp",
                        principalTable: "waybills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_number_reservations_BranchId",
                schema: "transport_erp",
                table: "number_reservations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_number_reservations_CompanyId_IdempotencyKey",
                schema: "transport_erp",
                table: "number_reservations",
                columns: new[] { "CompanyId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_number_reservations_CompanyId_RenderedNumber",
                schema: "transport_erp",
                table: "number_reservations",
                columns: new[] { "CompanyId", "RenderedNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_number_reservations_SequenceId_NumberValue",
                schema: "transport_erp",
                table: "number_reservations",
                columns: new[] { "SequenceId", "NumberValue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_number_reservations_WaybillId_State",
                schema: "transport_erp",
                table: "number_reservations",
                columns: new[] { "WaybillId", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_number_sequences_BranchId",
                schema: "transport_erp",
                table: "number_sequences",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_number_sequences_CompanyId_BranchId_DocumentType",
                schema: "transport_erp",
                table: "number_sequences",
                columns: new[] { "CompanyId", "BranchId", "DocumentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_BranchId",
                schema: "transport_erp",
                table: "operational_parties",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "operational_parties",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_CompanyId_ClientOperationId",
                schema: "transport_erp",
                table: "operational_parties",
                columns: new[] { "CompanyId", "ClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_CompanyId_IdentityNo",
                schema: "transport_erp",
                table: "operational_parties",
                columns: new[] { "CompanyId", "IdentityNo" },
                filter: "\"IdentityNo\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_CompanyId_Mobile",
                schema: "transport_erp",
                table: "operational_parties",
                columns: new[] { "CompanyId", "Mobile" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_parties_CompanyId_PartyNo",
                schema: "transport_erp",
                table: "operational_parties",
                columns: new[] { "CompanyId", "PartyNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waybill_items_ItemType",
                schema: "transport_erp",
                table: "waybill_items",
                column: "ItemType");

            migrationBuilder.CreateIndex(
                name: "IX_waybill_items_WaybillId_LineNo",
                schema: "transport_erp",
                table: "waybill_items",
                columns: new[] { "WaybillId", "LineNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waybill_parties_OperationalPartyId",
                schema: "transport_erp",
                table: "waybill_parties",
                column: "OperationalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_waybill_parties_WaybillId_Role",
                schema: "transport_erp",
                table: "waybill_parties",
                columns: new[] { "WaybillId", "Role" });

            migrationBuilder.CreateIndex(
                name: "IX_waybill_parties_WaybillId_Sequence",
                schema: "transport_erp",
                table: "waybill_parties",
                columns: new[] { "WaybillId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waybills_BranchId",
                schema: "transport_erp",
                table: "waybills",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CompanyId_BranchId_CreateClientOperationId",
                schema: "transport_erp",
                table: "waybills",
                columns: new[] { "CompanyId", "BranchId", "CreateClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CompanyId_BranchId_Status_WaybillDateTime",
                schema: "transport_erp",
                table: "waybills",
                columns: new[] { "CompanyId", "BranchId", "Status", "WaybillDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CompanyId_DraftNo",
                schema: "transport_erp",
                table: "waybills",
                columns: new[] { "CompanyId", "DraftNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CompanyId_WaybillNo",
                schema: "transport_erp",
                table: "waybills",
                columns: new[] { "CompanyId", "WaybillNo" },
                unique: true,
                filter: "\"WaybillNo\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_waybills_CurrencyId",
                schema: "transport_erp",
                table: "waybills",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "number_reservations",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "waybill_items",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "waybill_parties",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "number_sequences",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "operational_parties",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "waybills",
                schema: "transport_erp");
        }
    }
}
