using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class P1InitialPostgreSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transport_erp");

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MinorUnit = table.Column<int>(type: "integer", nullable: false),
                    IsBase = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencies", x => x.Id);
                    table.CheckConstraint("ck_currencies_minor_unit", "\"MinorUnit\" BETWEEN 0 AND 6");
                    table.CheckConstraint("ck_currencies_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                });

            migrationBuilder.CreateTable(
                name: "global_settings",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    Key = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    ValueJson = table.Column<string>(type: "text", nullable: false),
                    ValueType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_global_settings", x => x.Id);
                    table.CheckConstraint("ck_global_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Resource = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Action = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ScopeType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                    table.CheckConstraint("ck_permissions_scope", "\"ScopeType\" IN ('PLATFORM','COMPANY','BRANCH')");
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                    table.CheckConstraint("ck_roles_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                });

            migrationBuilder.CreateTable(
                name: "companies",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    LegalNameAr = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    LegalNameEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TaxIdentifier = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BaseCurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultCalendarId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                    table.CheckConstraint("ck_companies_status", "\"Status\" IN ('DRAFT','ACTIVE','SUSPENDED','CLOSED')");
                    table.ForeignKey(
                        name: "FK_companies_currencies_BaseCurrencyId",
                        column: x => x.BaseCurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "transport_erp",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopeType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.RoleId, x.PermissionId, x.ScopeType });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "transport_erp",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "transport_erp",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branches",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BranchType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Timezone = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branches", x => x.Id);
                    table.UniqueConstraint("AK_branches_Id_CompanyId", x => new { x.Id, x.CompanyId });
                    table.CheckConstraint("ck_branches_status", "\"Status\" IN ('DRAFT','ACTIVE','INACTIVE')");
                    table.ForeignKey(
                        name: "FK_branches_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "chart_of_accounts",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PostingAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chart_of_accounts", x => x.Id);
                    table.CheckConstraint("ck_chart_accounts_type", "\"AccountType\" IN ('ASSET','LIABILITY','EQUITY','REVENUE','EXPENSE')");
                    table.ForeignKey(
                        name: "FK_chart_of_accounts_chart_of_accounts_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "transport_erp",
                        principalTable: "chart_of_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chart_of_accounts_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chart_of_accounts_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_settings",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    Key = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    ValueJson = table.Column<string>(type: "text", nullable: false),
                    ValueType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_settings", x => x.Id);
                    table.CheckConstraint("ck_company_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                    table.ForeignKey(
                        name: "FK_company_settings_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "financial_dimensions",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DimensionCode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ValueCode = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ValueNameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_dimensions", x => x.Id);
                    table.CheckConstraint("ck_financial_dimensions_dates", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
                    table.ForeignKey(
                        name: "FK_financial_dimensions_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_financial_dimensions_financial_dimensions_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "transport_erp",
                        principalTable: "financial_dimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "branch_settings",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    Key = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    ValueJson = table.Column<string>(type: "text", nullable: false),
                    ValueType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_settings", x => x.Id);
                    table.CheckConstraint("ck_branch_settings_status", "\"Status\" IN ('ACTIVE','INACTIVE')");
                    table.ForeignKey(
                        name: "FK_branch_settings_branches_BranchId_CompanyId",
                        columns: x => new { x.BranchId, x.CompanyId },
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumns: new[] { "Id", "CompanyId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_branch_settings_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.CheckConstraint("ck_users_status", "\"Status\" IN ('ACTIVE','LOCKED','DISABLED')");
                    table.ForeignKey(
                        name: "FK_users_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActorUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    BeforeJson = table.Column<string>(type: "text", nullable: true),
                    AfterJson = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ip = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PreviousHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_events_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audit_events_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audit_events_users_ActorUserId",
                        column: x => x.ActorUserId,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fiscal_periods",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiscal_periods", x => x.Id);
                    table.CheckConstraint("ck_fiscal_periods_range", "\"EndDate\" >= \"StartDate\"");
                    table.CheckConstraint("ck_fiscal_periods_status", "\"Status\" IN ('OPEN','SOFT_CLOSED','CLOSED')");
                    table.ForeignKey(
                        name: "FK_fiscal_periods_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fiscal_periods_users_ClosedBy",
                        column: x => x.ClosedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payment_vouchers",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoucherNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PayeeName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMethodCode = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PaidBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CashBoxId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExternalReference = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_vouchers", x => x.Id);
                    table.CheckConstraint("ck_payment_vouchers_amount", "\"Amount\" > 0");
                    table.CheckConstraint("ck_payments_status", "\"Status\" IN ('DRAFT','APPROVED','POSTED','CANCELLED')");
                    table.ForeignKey(
                        name: "FK_payment_vouchers_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_vouchers_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_vouchers_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_vouchers_users_PaidBy",
                        column: x => x.PaidBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receipt_vouchers",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoucherNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PayerName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMethodCode = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CollectedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CashBoxId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExternalReference = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_vouchers", x => x.Id);
                    table.CheckConstraint("ck_receipt_vouchers_amount", "\"Amount\" > 0");
                    table.CheckConstraint("ck_receipts_status", "\"Status\" IN ('DRAFT','APPROVED','POSTED','CANCELLED')");
                    table.ForeignKey(
                        name: "FK_receipt_vouchers_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receipt_vouchers_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receipt_vouchers_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receipt_vouchers_users_CollectedBy",
                        column: x => x.CollectedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sync_operations",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    OperationType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientOperationId = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    PayloadHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientOccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ServerReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BaseVersion = table.Column<long>(type: "bigint", nullable: true),
                    ResultVersion = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    NextRetryAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ErrorCode = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ConflictCaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sync_operations", x => x.Id);
                    table.CheckConstraint("ck_sync_operation_type", "\"OperationType\" IN ('CREATE','UPDATE','DELETE','COMMAND')");
                    table.CheckConstraint("ck_sync_retry_count", "\"RetryCount\" >= 0");
                    table.CheckConstraint("ck_sync_status", "\"Status\" IN ('QUEUED','SENDING','SUCCEEDED','FAILED','CONFLICT','REJECTED','RESOLVED')");
                    table.ForeignKey(
                        name: "FK_sync_operations_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sync_operations_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sync_operations_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_permission_overrides",
                schema: "transport_erp",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permission_overrides", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_user_permission_overrides_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "transport_erp",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_permission_overrides_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "transport_erp",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "transport_erp",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_roles_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "journal_entries",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentNo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    FiscalPeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalDebit = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(19,8)", precision: 19, scale: 8, nullable: false),
                    ReversalOfId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_journal_entries", x => x.Id);
                    table.CheckConstraint("ck_journal_entries_amounts", "\"TotalDebit\" >= 0 AND \"TotalCredit\" >= 0");
                    table.CheckConstraint("ck_journal_entries_status", "\"Status\" IN ('DRAFT','CHECKED','APPROVED','POSTED','REVERSED')");
                    table.ForeignKey(
                        name: "FK_journal_entries_branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "transport_erp",
                        principalTable: "branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entries_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "transport_erp",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entries_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entries_fiscal_periods_FiscalPeriodId",
                        column: x => x.FiscalPeriodId,
                        principalSchema: "transport_erp",
                        principalTable: "fiscal_periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entries_journal_entries_ReversalOfId",
                        column: x => x.ReversalOfId,
                        principalSchema: "transport_erp",
                        principalTable: "journal_entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "journal_entry_lines",
                schema: "transport_erp",
                columns: table => new
                {
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNo = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FinancialDimensionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Debit = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    Credit = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    ForeignAmount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_journal_entry_lines", x => new { x.JournalEntryId, x.LineNo });
                    table.CheckConstraint("ck_journal_lines_amounts", "\"Debit\" >= 0 AND \"Credit\" >= 0 AND (\"Debit\" > 0 OR \"Credit\" > 0) AND NOT (\"Debit\" > 0 AND \"Credit\" > 0)");
                    table.ForeignKey(
                        name: "FK_journal_entry_lines_chart_of_accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "transport_erp",
                        principalTable: "chart_of_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entry_lines_currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "transport_erp",
                        principalTable: "currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entry_lines_financial_dimensions_FinancialDimension~",
                        column: x => x.FinancialDimensionId,
                        principalSchema: "transport_erp",
                        principalTable: "financial_dimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_journal_entry_lines_journal_entries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "transport_erp",
                        principalTable: "journal_entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_ActorUserId",
                schema: "transport_erp",
                table: "audit_events",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_BranchId",
                schema: "transport_erp",
                table: "audit_events",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_CompanyId_OccurredAt",
                schema: "transport_erp",
                table: "audit_events",
                columns: new[] { "CompanyId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_CorrelationId",
                schema: "transport_erp",
                table: "audit_events",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_EntityType_EntityId_OccurredAt",
                schema: "transport_erp",
                table: "audit_events",
                columns: new[] { "EntityType", "EntityId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_Hash",
                schema: "transport_erp",
                table: "audit_events",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_branch_settings_BranchId_CompanyId",
                schema: "transport_erp",
                table: "branch_settings",
                columns: new[] { "BranchId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_branch_settings_BranchId_Key",
                schema: "transport_erp",
                table: "branch_settings",
                columns: new[] { "BranchId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_branch_settings_CompanyId_BranchId_Status",
                schema: "transport_erp",
                table: "branch_settings",
                columns: new[] { "CompanyId", "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_branches_CompanyId_Code",
                schema: "transport_erp",
                table: "branches",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_branches_CompanyId_Status",
                schema: "transport_erp",
                table: "branches",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_chart_of_accounts_CompanyId_AccountType_Status",
                schema: "transport_erp",
                table: "chart_of_accounts",
                columns: new[] { "CompanyId", "AccountType", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_chart_of_accounts_CompanyId_Code",
                schema: "transport_erp",
                table: "chart_of_accounts",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_chart_of_accounts_CompanyId_ParentId",
                schema: "transport_erp",
                table: "chart_of_accounts",
                columns: new[] { "CompanyId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_chart_of_accounts_CurrencyId",
                schema: "transport_erp",
                table: "chart_of_accounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_chart_of_accounts_ParentId",
                schema: "transport_erp",
                table: "chart_of_accounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_BaseCurrencyId",
                schema: "transport_erp",
                table: "companies",
                column: "BaseCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_Code",
                schema: "transport_erp",
                table: "companies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_Status",
                schema: "transport_erp",
                table: "companies",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_companies_TaxIdentifier",
                schema: "transport_erp",
                table: "companies",
                column: "TaxIdentifier",
                unique: true,
                filter: "\"TaxIdentifier\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_company_settings_CompanyId_Key",
                schema: "transport_erp",
                table: "company_settings",
                columns: new[] { "CompanyId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_settings_CompanyId_Status",
                schema: "transport_erp",
                table: "company_settings",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_currencies_Code",
                schema: "transport_erp",
                table: "currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_currencies_Status",
                schema: "transport_erp",
                table: "currencies",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_financial_dimensions_CompanyId_DimensionCode_ValueCode",
                schema: "transport_erp",
                table: "financial_dimensions",
                columns: new[] { "CompanyId", "DimensionCode", "ValueCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_financial_dimensions_CompanyId_ParentId_Status",
                schema: "transport_erp",
                table: "financial_dimensions",
                columns: new[] { "CompanyId", "ParentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_financial_dimensions_CompanyId_ValidFrom_ValidTo",
                schema: "transport_erp",
                table: "financial_dimensions",
                columns: new[] { "CompanyId", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_financial_dimensions_ParentId",
                schema: "transport_erp",
                table: "financial_dimensions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_periods_ClosedBy",
                schema: "transport_erp",
                table: "fiscal_periods",
                column: "ClosedBy");

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_periods_CompanyId_Code",
                schema: "transport_erp",
                table: "fiscal_periods",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_periods_CompanyId_StartDate_EndDate",
                schema: "transport_erp",
                table: "fiscal_periods",
                columns: new[] { "CompanyId", "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_periods_CompanyId_Status",
                schema: "transport_erp",
                table: "fiscal_periods",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_global_settings_Key",
                schema: "transport_erp",
                table: "global_settings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_global_settings_Status",
                schema: "transport_erp",
                table: "global_settings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_BranchId_EntryDate",
                schema: "transport_erp",
                table: "journal_entries",
                columns: new[] { "BranchId", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_CompanyId_DocumentNo",
                schema: "transport_erp",
                table: "journal_entries",
                columns: new[] { "CompanyId", "DocumentNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_CompanyId_FiscalPeriodId_Status",
                schema: "transport_erp",
                table: "journal_entries",
                columns: new[] { "CompanyId", "FiscalPeriodId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_CurrencyId",
                schema: "transport_erp",
                table: "journal_entries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_FiscalPeriodId",
                schema: "transport_erp",
                table: "journal_entries",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_ReversalOfId",
                schema: "transport_erp",
                table: "journal_entries",
                column: "ReversalOfId");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_SourceType_SourceId",
                schema: "transport_erp",
                table: "journal_entries",
                columns: new[] { "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_journal_entry_lines_AccountId",
                schema: "transport_erp",
                table: "journal_entry_lines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entry_lines_CurrencyId",
                schema: "transport_erp",
                table: "journal_entry_lines",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entry_lines_FinancialDimensionId",
                schema: "transport_erp",
                table: "journal_entry_lines",
                column: "FinancialDimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_vouchers_BranchId_VoucherDate_Status",
                schema: "transport_erp",
                table: "payment_vouchers",
                columns: new[] { "BranchId", "VoucherDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_vouchers_CompanyId_ReferenceType_ReferenceId",
                schema: "transport_erp",
                table: "payment_vouchers",
                columns: new[] { "CompanyId", "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_vouchers_CompanyId_VoucherNo",
                schema: "transport_erp",
                table: "payment_vouchers",
                columns: new[] { "CompanyId", "VoucherNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_vouchers_CurrencyId",
                schema: "transport_erp",
                table: "payment_vouchers",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_vouchers_PaidBy",
                schema: "transport_erp",
                table: "payment_vouchers",
                column: "PaidBy");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_Code",
                schema: "transport_erp",
                table: "permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_Resource_Action",
                schema: "transport_erp",
                table: "permissions",
                columns: new[] { "Resource", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_BranchId_VoucherDate_Status",
                schema: "transport_erp",
                table: "receipt_vouchers",
                columns: new[] { "BranchId", "VoucherDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_CollectedBy",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "CollectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_CompanyId_ReferenceType_ReferenceId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                columns: new[] { "CompanyId", "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_CompanyId_VoucherNo",
                schema: "transport_erp",
                table: "receipt_vouchers",
                columns: new[] { "CompanyId", "VoucherNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_CurrencyId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_PermissionId",
                schema: "transport_erp",
                table: "role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Code_CompanyId",
                schema: "transport_erp",
                table: "roles",
                columns: new[] { "Code", "CompanyId" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_roles_CompanyId_Status",
                schema: "transport_erp",
                table: "roles",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_BranchId",
                schema: "transport_erp",
                table: "sync_operations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_CompanyId_Status_NextRetryAt",
                schema: "transport_erp",
                table: "sync_operations",
                columns: new[] { "CompanyId", "Status", "NextRetryAt" });

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_DeviceId_ClientOperationId",
                schema: "transport_erp",
                table: "sync_operations",
                columns: new[] { "DeviceId", "ClientOperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_DeviceId_CreatedAt",
                schema: "transport_erp",
                table: "sync_operations",
                columns: new[] { "DeviceId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_EntityType_EntityId_CreatedAt",
                schema: "transport_erp",
                table: "sync_operations",
                columns: new[] { "EntityType", "EntityId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_PayloadHash",
                schema: "transport_erp",
                table: "sync_operations",
                column: "PayloadHash");

            migrationBuilder.CreateIndex(
                name: "IX_sync_operations_UserId",
                schema: "transport_erp",
                table: "sync_operations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_overrides_PermissionId",
                schema: "transport_erp",
                table: "user_permission_overrides",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleId",
                schema: "transport_erp",
                table: "user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_users_BranchId_Status",
                schema: "transport_erp",
                table: "users",
                columns: new[] { "BranchId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_users_CompanyId",
                schema: "transport_erp",
                table: "users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email_CompanyId",
                schema: "transport_erp",
                table: "users",
                columns: new[] { "Email", "CompanyId" },
                unique: true,
                filter: "\"Email\" IS NOT NULL AND \"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_NormalizedUserName_CompanyId",
                schema: "transport_erp",
                table: "users",
                columns: new[] { "NormalizedUserName", "CompanyId" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_events",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "branch_settings",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "company_settings",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "global_settings",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "journal_entry_lines",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "payment_vouchers",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "receipt_vouchers",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "sync_operations",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "user_permission_overrides",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "chart_of_accounts",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "financial_dimensions",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "journal_entries",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "fiscal_periods",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "users",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "branches",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "companies",
                schema: "transport_erp");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "transport_erp");
        }
    }
}
