using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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
        var operationCount = migrationBuilder.Operations.Count;
        migrationBuilder.CreateTable(
            name: name,
            columns: columns,
            schema: schema,
            constraints: constraints);

        var tableOperation = migrationBuilder.Operations
            .Skip(operationCount)
            .OfType<CreateTableOperation>()
            .Single();

        // Earlier WAVE-1 hand-written migrations used the compact positional
        // ForeignKey(name, column, schema, table, column) form. EF Core's actual
        // positional order is (name, column, principalTable, principalColumn,
        // principalSchema), which produces malformed PostgreSQL such as
        // REFERENCES "Id".transport_erp (companies). Normalize only that exact
        // legacy signature; correctly constructed foreign keys are untouched.
        foreach (var foreignKey in tableOperation.ForeignKeys)
        {
            if (!string.Equals(foreignKey.PrincipalTable, schema, StringComparison.Ordinal) ||
                !string.Equals(foreignKey.PrincipalSchema, "Id", StringComparison.Ordinal) ||
                foreignKey.PrincipalColumns.Length != 1 ||
                string.Equals(foreignKey.PrincipalColumns[0], "Id", StringComparison.Ordinal))
                continue;

            var actualPrincipalTable = foreignKey.PrincipalColumns[0];
            foreignKey.PrincipalTable = actualPrincipalTable;
            foreignKey.PrincipalSchema = schema;
            foreignKey.PrincipalColumns = ["Id"];
        }
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
