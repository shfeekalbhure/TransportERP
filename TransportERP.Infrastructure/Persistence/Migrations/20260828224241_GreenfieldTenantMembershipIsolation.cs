using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GreenfieldTenantMembershipIsolation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_memberships",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScopeType = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Status = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SecurityVersion = table.Column<long>(type: "bigint", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RevokedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RevokeReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ConcurrencyVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_memberships", x => x.Id);
                    table.UniqueConstraint("AK_user_memberships_Id_UserId_CompanyId", x => new { x.Id, x.UserId, x.CompanyId });
                    table.CheckConstraint("ck_user_memberships_concurrency", "\"ConcurrencyVersion\" >= 1");
                    table.CheckConstraint("ck_user_memberships_revoked_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"RevokeReason\", '')) <> '')");
                    table.CheckConstraint("ck_user_memberships_scope", "(\"ScopeType\" = 'COMPANY' AND \"BranchId\" IS NULL) OR (\"ScopeType\" = 'BRANCH' AND \"BranchId\" IS NOT NULL)");
                    table.CheckConstraint("ck_user_memberships_security_version", "\"SecurityVersion\" >= 1");
                    table.CheckConstraint("ck_user_memberships_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
                    table.CheckConstraint("ck_user_memberships_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
                    table.ForeignKey(
                        name: "FK_user_memberships_branches_BranchId_CompanyId",
                        columns: x => new { x.BranchId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumns: new[] { "Id", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_memberships_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_memberships_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_memberships_users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_memberships_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_permission_grants",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Effect = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Status = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    GrantedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    RevokedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ConcurrencyVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permission_grants", x => x.Id);
                    table.CheckConstraint("ck_user_permission_grants_concurrency", "\"ConcurrencyVersion\" >= 1");
                    table.CheckConstraint("ck_user_permission_grants_effect", "\"Effect\" IN ('ALLOW','DENY')");
                    table.CheckConstraint("ck_user_permission_grants_revoke_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"Reason\", '')) <> '')");
                    table.CheckConstraint("ck_user_permission_grants_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
                    table.CheckConstraint("ck_user_permission_grants_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
                    table.ForeignKey(
                        name: "FK_user_permission_grants_branches_BranchId_CompanyId",
                        columns: x => new { x.BranchId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumns: new[] { "Id", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_permission_grants_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "transport_erp",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_permission_grants_user_memberships_MembershipId_UserId~",
                        columns: x => new { x.MembershipId, x.UserId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "user_memberships",
                        principalColumns: new[] { "Id", "UserId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_permission_grants_users_GrantedBy",
                        column: x => x.GrantedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_permission_grants_users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_role_grants",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    GrantedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    RevokedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ConcurrencyVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role_grants", x => x.Id);
                    table.CheckConstraint("ck_user_role_grants_concurrency", "\"ConcurrencyVersion\" >= 1");
                    table.CheckConstraint("ck_user_role_grants_revoke_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"Reason\", '')) <> '')");
                    table.CheckConstraint("ck_user_role_grants_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
                    table.CheckConstraint("ck_user_role_grants_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
                    table.ForeignKey(
                        name: "FK_user_role_grants_branches_BranchId_CompanyId",
                        columns: x => new { x.BranchId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumns: new[] { "Id", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_grants_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "transport_erp",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_grants_user_memberships_MembershipId_UserId_Compa~",
                        columns: x => new { x.MembershipId, x.UserId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "user_memberships",
                        principalColumns: new[] { "Id", "UserId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_grants_users_GrantedBy",
                        column: x => x.GrantedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_grants_users_RevokedBy",
                        column: x => x.RevokedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_BranchId_CompanyId",
                schema: "transport_erp",
                table: "user_memberships",
                columns: new[] { "BranchId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "user_memberships",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_CompanyId_UpdatedAt",
                schema: "transport_erp",
                table: "user_memberships",
                columns: new[] { "CompanyId", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_CreatedBy",
                schema: "transport_erp",
                table: "user_memberships",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_RevokedBy",
                schema: "transport_erp",
                table: "user_memberships",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_UserId_CompanyId_BranchId",
                schema: "transport_erp",
                table: "user_memberships",
                columns: new[] { "UserId", "CompanyId", "BranchId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_user_memberships_UserId_Status_CompanyId",
                schema: "transport_erp",
                table: "user_memberships",
                columns: new[] { "UserId", "Status", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_BranchId_CompanyId",
                schema: "transport_erp",
                table: "user_permission_grants",
                columns: new[] { "BranchId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "user_permission_grants",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_GrantedBy",
                schema: "transport_erp",
                table: "user_permission_grants",
                column: "GrantedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_MembershipId_PermissionId",
                schema: "transport_erp",
                table: "user_permission_grants",
                columns: new[] { "MembershipId", "PermissionId" },
                unique: true,
                filter: "\"Status\" = 'ACTIVE'");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_MembershipId_UserId_CompanyId",
                schema: "transport_erp",
                table: "user_permission_grants",
                columns: new[] { "MembershipId", "UserId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_PermissionId",
                schema: "transport_erp",
                table: "user_permission_grants",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_RevokedBy",
                schema: "transport_erp",
                table: "user_permission_grants",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_grants_UserId_Status",
                schema: "transport_erp",
                table: "user_permission_grants",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_BranchId_CompanyId",
                schema: "transport_erp",
                table: "user_role_grants",
                columns: new[] { "BranchId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "user_role_grants",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_GrantedBy",
                schema: "transport_erp",
                table: "user_role_grants",
                column: "GrantedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_MembershipId_RoleId",
                schema: "transport_erp",
                table: "user_role_grants",
                columns: new[] { "MembershipId", "RoleId" },
                unique: true,
                filter: "\"Status\" = 'ACTIVE'");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_MembershipId_UserId_CompanyId",
                schema: "transport_erp",
                table: "user_role_grants",
                columns: new[] { "MembershipId", "UserId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_RevokedBy",
                schema: "transport_erp",
                table: "user_role_grants",
                column: "RevokedBy");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_RoleId",
                schema: "transport_erp",
                table: "user_role_grants",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_grants_UserId_Status",
                schema: "transport_erp",
                table: "user_role_grants",
                columns: new[] { "UserId", "Status" });
            migrationBuilder.Sql(GreenfieldDbp002PhysicalSql.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(GreenfieldDbp002PhysicalSql.Down);

            migrationBuilder.DropTable(
                name: "user_permission_grants",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "user_role_grants",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "user_memberships",
                schema: "transport_erp");
        }
    }
}
