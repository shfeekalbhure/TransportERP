using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Geo.Migrations;

/// <summary>Development/test migration only. Production deployment remains a release decision.</summary>
public partial class BusinessAuditEvents : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var audits = migrationBuilder.CreateTable(name: "business_audit_events", columns: table => new
        {
            event_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            actor_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            occurred_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            company_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            branch_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            entity_type = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
            record_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            action = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
            correlation_id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
            before_state = table.Column<string>(type: "json", nullable: true),
            after_state = table.Column<string>(type: "json", nullable: true)
        }, constraints: table => table.PrimaryKey("PK_business_audit_events", x => x.event_id));
        audits.Annotation("MySql:CharSet", "utf8mb4");
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("business_audit_events");
}
