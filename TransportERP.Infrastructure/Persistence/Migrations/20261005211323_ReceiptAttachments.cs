using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "receipt_attachments",
                schema: "transport_erp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MediaType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_receipt_attachments_receipt_vouchers_ReceiptId",
                        column: x => x.ReceiptId,
                        principalSchema: "transport_erp",
                        principalTable: "receipt_vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_receipt_attachments_users_AddedBy",
                        column: x => x.AddedBy,
                        principalSchema: "transport_erp",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_receipt_attachments_AddedBy",
                schema: "transport_erp",
                table: "receipt_attachments",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_receipt_attachments_ReceiptId",
                schema: "transport_erp",
                table: "receipt_attachments",
                column: "ReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt_attachments",
                schema: "transport_erp");
        }
    }
}
