using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptPostingLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "PostingJournalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipt_vouchers_ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "ReversalJournalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_receipt_vouchers_journal_entries_PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "PostingJournalId",
                principalSchema: "transport_erp",
                principalTable: "journal_entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_receipt_vouchers_journal_entries_ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers",
                column: "ReversalJournalId",
                principalSchema: "transport_erp",
                principalTable: "journal_entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_receipt_vouchers_journal_entries_PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_receipt_vouchers_journal_entries_ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropIndex(
                name: "IX_receipt_vouchers_PostingJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");

            migrationBuilder.DropIndex(
                name: "IX_receipt_vouchers_ReversalJournalId",
                schema: "transport_erp",
                table: "receipt_vouchers");
        }
    }
}
