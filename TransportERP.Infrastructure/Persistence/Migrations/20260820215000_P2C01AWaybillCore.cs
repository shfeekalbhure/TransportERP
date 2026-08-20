using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Infrastructure.Persistence;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260820215000_P2C01AWaybillCore")]
public partial class P2C01AWaybillCore : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "operational_parties",
            schema: "transport_erp",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                PartyNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                Mobile = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                IdentityType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                IdentityNo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                GovernorateId = table.Column<Guid>(type: "uuid", nullable: true),
                DirectorateId = table.Column<Guid>(type: "uuid", nullable: true),
                CityId = table.Column<Guid>(type: "uuid", nullable: true),
                AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                AddressLine = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_operational_parties", x => x.Id);
                table.ForeignKey("FK_operational_parties_companies_CompanyId", x => x.CompanyId, "companies", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_operational_parties_branches_BranchId", x => x.BranchId, "branches", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
            });

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
                NextValue = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                ResetPolicy = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_number_sequences", x => x.Id);
                table.ForeignKey("FK_number_sequences_companies_CompanyId", x => x.CompanyId, "companies", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_number_sequences_branches_BranchId", x => x.BranchId, "branches", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "waybills",
            schema: "transport_erp",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                DraftNo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                WaybillNo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                ServicePointId = table.Column<Guid>(type: "uuid", nullable: true),
                AgentId = table.Column<Guid>(type: "uuid", nullable: true),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                WaybillDateTime = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RequestDateTime = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                ExpectedArrivalAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                ServiceType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                Priority = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                OriginId = table.Column<Guid>(type: "uuid", nullable: false),
                DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                ExchangeRate = table.Column<decimal>(type: "numeric(20,8)", nullable: false),
                FreightTotal = table.Column<decimal>(type: "numeric(20,4)", nullable: false),
                DiscountTotal = table.Column<decimal>(type: "numeric(20,4)", nullable: false),
                NetAmount = table.Column<decimal>(type: "numeric(20,4)", nullable: false),
                OperationalStatus = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                FinancialStatus = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                LastReason = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                ApprovedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                CancelledAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_waybills", x => x.Id);
                table.ForeignKey("FK_waybills_companies_CompanyId", x => x.CompanyId, "companies", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_waybills_branches_BranchId", x => x.BranchId, "branches", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_waybills_currencies_CurrencyId", x => x.CurrencyId, "currencies", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_waybills_users_CreatedBy", x => x.CreatedBy, "users", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_waybills_users_ApprovedBy", x => x.ApprovedBy, "users", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "waybill_parties",
            schema: "transport_erp",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                Role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                OperationalPartyId = table.Column<Guid>(type: "uuid", nullable: true),
                NameSnapshot = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                MobileSnapshot = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                IdentityTypeSnapshot = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                IdentityNoSnapshot = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                GovernorateId = table.Column<Guid>(type: "uuid", nullable: true),
                DirectorateId = table.Column<Guid>(type: "uuid", nullable: true),
                CityId = table.Column<Guid>(type: "uuid", nullable: true),
                AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                AddressLine = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_waybill_parties", x => x.Id);
                table.ForeignKey("FK_waybill_parties_waybills_WaybillId", x => x.WaybillId, "waybills", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_waybill_parties_operational_parties_OperationalPartyId", x => x.OperationalPartyId, "operational_parties", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "waybill_items",
            schema: "transport_erp",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                WaybillId = table.Column<Guid>(type: "uuid", nullable: false),
                LineNo = table.Column<int>(type: "integer", nullable: false),
                ItemCode = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                ItemType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Contents = table.Column<string>(type: "text", nullable: false),
                Quantity = table.Column<decimal>(type: "numeric(20,4)", nullable: false),
                Pieces = table.Column<int>(type: "integer", nullable: true),
                Weight = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                Length = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                Width = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                Height = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                Volume = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                DeclaredValue = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                OriginCountryId = table.Column<Guid>(type: "uuid", nullable: true),
                ItemFreight = table.Column<decimal>(type: "numeric(20,4)", nullable: true),
                RiskFlagsJson = table.Column<string>(type: "jsonb", nullable: true),
                Notes = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_waybill_items", x => x.Id);
                table.ForeignKey("FK_waybill_items_waybills_WaybillId", x => x.WaybillId, "waybills", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Cascade);
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
                NumberValue = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                RenderedNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                ReservedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                CommittedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                VoidedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                VoidReason = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                State = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_number_reservations", x => x.Id);
                table.ForeignKey("FK_number_reservations_number_sequences_SequenceId", x => x.SequenceId, "number_sequences", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_number_reservations_waybills_WaybillId", x => x.WaybillId, "waybills", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_number_reservations_companies_CompanyId", x => x.CompanyId, "companies", "Id", principalSchema: "transport_erp", onDelete: ReferentialAction.Restrict);
                table.CheckConstraint("ck_number_reservations_state", "\"State\" IN ('RESERVED','COMMITTED','VOID')");
                table.CheckConstraint("ck_number_reservations_commit_link", "\"State\" <> 'COMMITTED' OR \"WaybillId\" IS NOT NULL");
            });

        migrationBuilder.CreateIndex("UX_operational_parties_company_party_no", "operational_parties", new[] { "CompanyId", "PartyNo" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("IX_operational_parties_company_mobile", "operational_parties", new[] { "CompanyId", "Mobile" }, schema: "transport_erp");
        migrationBuilder.CreateIndex("IX_operational_parties_identity", "operational_parties", "IdentityNo", schema: "transport_erp");
        migrationBuilder.CreateIndex("UX_number_sequences_scope_document", "number_sequences", new[] { "CompanyId", "BranchId", "DocumentType" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("UX_waybills_draft", "waybills", new[] { "CompanyId", "DraftNo" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("UX_waybills_official", "waybills", new[] { "CompanyId", "BranchId", "WaybillNo" }, schema: "transport_erp", unique: true, filter: "\"WaybillNo\" IS NOT NULL");
        migrationBuilder.CreateIndex("IX_waybills_status", "waybills", new[] { "CompanyId", "BranchId", "OperationalStatus" }, schema: "transport_erp");
        migrationBuilder.CreateIndex("UX_waybill_parties_role", "waybill_parties", new[] { "WaybillId", "Role" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("IX_waybill_parties_party", "waybill_parties", "OperationalPartyId", schema: "transport_erp");
        migrationBuilder.CreateIndex("UX_waybill_items_line", "waybill_items", new[] { "WaybillId", "LineNo" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("UX_number_reservations_sequence_value", "number_reservations", new[] { "SequenceId", "NumberValue" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("UX_number_reservations_idempotency", "number_reservations", new[] { "CompanyId", "IdempotencyKey" }, schema: "transport_erp", unique: true);
        migrationBuilder.CreateIndex("UX_number_reservations_waybill_committed", "number_reservations", new[] { "WaybillId", "State" }, schema: "transport_erp", unique: true, filter: "\"State\" = 'COMMITTED'");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "number_reservations", schema: "transport_erp");
        migrationBuilder.DropTable(name: "waybill_items", schema: "transport_erp");
        migrationBuilder.DropTable(name: "waybill_parties", schema: "transport_erp");
        migrationBuilder.DropTable(name: "number_sequences", schema: "transport_erp");
        migrationBuilder.DropTable(name: "waybills", schema: "transport_erp");
        migrationBuilder.DropTable(name: "operational_parties", schema: "transport_erp");
    }
}
