using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Geo.Migrations;

/// <summary>Development/test migration only. Production deployment and rollback remain release decisions.</summary>
public partial class GeoHierarchy : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var countries = migrationBuilder.CreateTable(name: "countries", columns: table => new
        {
            id = table.Column<byte[]>(type: "binary(16)", nullable: false),
            code = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
            arabic_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
            english_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
            nationality_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
            is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
            version = table.Column<uint>(type: "int unsigned", nullable: false)
        }, constraints: table => table.PrimaryKey("PK_countries", x => x.id));
        countries.Annotation("MySql:CharSet", "utf8mb4");
        migrationBuilder.CreateIndex(name: "UX_countries_code", table: "countries", column: "code", unique: true);
        migrationBuilder.CreateIndex(name: "IX_countries_active_code", table: "countries", columns: new[] { "is_active", "code" });
        CreateChild(migrationBuilder, "governorates", "country_id", "countries", "PK_countries", "UX_governorates_country_code", "IX_governorates_parent_active_code");
        CreateChild(migrationBuilder, "directorates", "governorate_id", "governorates", "PK_governorates", "UX_directorates_governorate_code", "IX_directorates_parent_active_code");
        CreateChild(migrationBuilder, "cities", "directorate_id", "directorates", "PK_directorates", "UX_cities_directorate_code", "IX_cities_parent_active_code");
        CreateChild(migrationBuilder, "areas", "city_id", "cities", "PK_cities", "UX_areas_city_code", "IX_areas_parent_active_code");
    }
    protected override void Down(MigrationBuilder migrationBuilder) { migrationBuilder.DropTable("areas"); migrationBuilder.DropTable("cities"); migrationBuilder.DropTable("directorates"); migrationBuilder.DropTable("governorates"); migrationBuilder.DropTable("countries"); }
    private static void CreateChild(MigrationBuilder m, string tableName, string parentColumn, string parentTable, string parentKey, string unique, string lookup)
    {
        var child = m.CreateTable(name: tableName, columns: table => new { id=table.Column<byte[]>(type:"binary(16)",nullable:false), parent_id=table.Column<byte[]>(name:parentColumn,type:"binary(16)",nullable:false), code=table.Column<string>(type:"varchar(64)",maxLength:64,nullable:false), arabic_name=table.Column<string>(type:"varchar(200)",maxLength:200,nullable:false), english_name=table.Column<string>(type:"varchar(200)",maxLength:200,nullable:true), is_active=table.Column<bool>(type:"tinyint(1)",nullable:false,defaultValue:true), version=table.Column<uint>(type:"int unsigned",nullable:false) }, constraints: table => { table.PrimaryKey("PK_" + tableName, x => x.id); table.ForeignKey("FK_" + tableName + "_" + parentTable + "_" + parentColumn, x => x.parent_id, principalTable: parentTable, principalColumn: "id", onDelete: ReferentialAction.Restrict); });
        child.Annotation("MySql:CharSet", "utf8mb4");
        m.CreateIndex(name: unique, table: tableName, columns: new[] { parentColumn, "code" }, unique:true);
        m.CreateIndex(name: lookup, table: tableName, columns: new[] { parentColumn, "is_active", "code" });
    }
}
