using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Infrastructure.Persistence;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260820215100_P2C01AIdempotencyColumns")]
public partial class P2C01AIdempotencyColumns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ClientOperationId",
            schema: "transport_erp",
            table: "operational_parties",
            type: "character varying(160)",
            maxLength: 160,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CreateOperationId",
            schema: "transport_erp",
            table: "waybills",
            type: "character varying(160)",
            maxLength: 160,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "UX_operational_parties_create_operation",
            schema: "transport_erp",
            table: "operational_parties",
            columns: new[] { "CompanyId", "ClientOperationId" },
            unique: true,
            filter: "\"ClientOperationId\" IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "UX_waybills_create_operation",
            schema: "transport_erp",
            table: "waybills",
            columns: new[] { "CompanyId", "CreateOperationId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "UX_operational_parties_create_operation", schema: "transport_erp", table: "operational_parties");
        migrationBuilder.DropIndex(name: "UX_waybills_create_operation", schema: "transport_erp", table: "waybills");
        migrationBuilder.DropColumn(name: "ClientOperationId", schema: "transport_erp", table: "operational_parties");
        migrationBuilder.DropColumn(name: "CreateOperationId", schema: "transport_erp", table: "waybills");
    }
}
