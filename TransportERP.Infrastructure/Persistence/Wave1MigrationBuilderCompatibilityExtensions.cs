using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace TransportERP.Infrastructure.Persistence;

internal static class Wave1MigrationBuilderCompatibilityExtensions
{
    public static void CreateTable<TColumns>(
        this MigrationBuilder migrationBuilder,
        string name,
        string schema,
        Func<ColumnsBuilder, TColumns> columns,
        Action<CreateTableBuilder<TColumns>> constraints)
    {
        migrationBuilder.CreateTable(
            name: name,
            columns: columns,
            schema: schema,
            constraints: constraints);
    }

    public static void CreateIndex(
        this MigrationBuilder migrationBuilder,
        string name,
        string schema,
        string table,
        string[] columns,
        bool unique = false,
        string? filter = null)
    {
        migrationBuilder.CreateIndex(
            name: name,
            table: table,
            columns: columns,
            schema: schema,
            unique: unique,
            filter: filter);
    }
}
