using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptWorkspaceDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Register receipt-specific capabilities without granting them to any role or user.
            var capabilities = new[] { ("ACC043.View", "عرض سند القبض"), ("ACC043.Create", "إنشاء سند القبض"),
                ("ACC043.Edit", "تعديل سند القبض"), ("ACC043.Post", "ترحيل سند القبض"),
                ("ACC043.Cancel", "إلغاء سند القبض"), ("ACC043.Reverse", "عكس سند القبض"),
                ("accounting.receipts.approve", "اعتماد سند القبض"), ("accounting.receipts.configure", "إعدادات سند القبض") };
            foreach (var (code, label) in capabilities)
            {
                var id = Guid.NewGuid();
                migrationBuilder.Sql($"""
                    INSERT INTO transport_erp.permissions
                    ("Id", "Code", "NameAr", "Resource", "Action", "ScopeType", "IsSystem", "Status", "CreatedAt", "UpdatedAt", "RowVersion")
                    VALUES ('{id}', '{code}', '{label}', 'ReceiptVoucher', '{code}', 'BRANCH', TRUE, 'ACTIVE', NOW(), NOW(), decode(replace('{id}', '-', ''), 'hex'))
                    ON CONFLICT ("Code") DO NOTHING;
                    """);
            }
            migrationBuilder.AddColumn<string>(
                name: "DocumentJson",
                schema: "transport_erp",
                table: "receipt_vouchers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingPolicyJson",
                schema: "transport_erp",
                table: "receipt_vouchers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Keep catalog capabilities: role assignments may exist and must not be removed by a schema rollback.
            migrationBuilder.DropColumn(
                name: "DocumentJson",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropColumn(
                name: "PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropColumn(
                name: "PostingPolicyJson",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropColumn(
                name: "ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");
        }
    }
}
