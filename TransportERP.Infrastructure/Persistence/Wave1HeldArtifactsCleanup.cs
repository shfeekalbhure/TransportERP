using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Removes WAVE-1 persistence artifacts that are not backed by a current governing
/// physical contract. The corresponding runtime routes are withheld while their
/// authority gaps remain open.
/// </summary>
[DbContext(typeof(Wave1ReferenceDbContext))]
[Migration("20260822172500_Wave1HeldArtifactsCleanup")]
public sealed class Wave1HeldArtifactsCleanup : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TABLE IF EXISTS transport_erp.accounting_open_items CASCADE;");
        migrationBuilder.Sql("DROP TABLE IF EXISTS transport_erp.account_classifications CASCADE;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => throw new NotSupportedException(
            "Held WAVE-1 persistence artifacts must not be recreated without a current governing physical contract.");
}
