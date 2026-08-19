using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class VoucherLifecyclePersistenceTests
{
    [Fact]
    public async Task Receipt_creation_is_idempotent_by_external_reference()
    {
        await using var db = CreateDb();
        var service = new VoucherLifecycleService(db);
        var command = ReceiptCommand("EXT-RECEIPT-001");

        var first = await service.CreateReceiptAsync(command);
        var second = await service.CreateReceiptAsync(command);

        Assert.Equal(first.Id, second.Id);
        Assert.Equal(1, await db.ReceiptVouchers.CountAsync());
        Assert.Equal("DRAFT", second.Status);
    }

    [Fact]
    public async Task Receipt_lifecycle_requires_order_and_blocks_posted_cancellation()
    {
        await using var db = CreateDb();
        var service = new VoucherLifecycleService(db);
        var receipt = await service.CreateReceiptAsync(ReceiptCommand("EXT-RECEIPT-002"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostReceiptAsync(receipt.CompanyId, receipt.Id, Guid.NewGuid()));
        await service.ApproveReceiptAsync(receipt.CompanyId, receipt.Id, Guid.NewGuid());
        await service.PostReceiptAsync(receipt.CompanyId, receipt.Id, Guid.NewGuid());
        var persisted = await db.ReceiptVouchers.SingleAsync(x => x.Id == receipt.Id);
        Assert.Equal("POSTED", persisted.Status);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CancelReceiptAsync(receipt.CompanyId, receipt.Id, "اختبار منع الإلغاء", Guid.NewGuid()));
    }

    [Fact]
    public async Task Payment_can_be_cancelled_from_draft_and_reason_is_retained()
    {
        await using var db = CreateDb();
        var service = new VoucherLifecycleService(db);
        var payment = await service.CreatePaymentAsync(PaymentCommand("EXT-PAYMENT-001"));

        var cancelled = await service.CancelPaymentAsync(payment.CompanyId, payment.Id, "سبب الإلغاء", Guid.NewGuid());

        Assert.Equal("CANCELLED", cancelled.Status);
        Assert.Contains("سبب الإلغاء", cancelled.Notes);
    }

    private static TransportErpDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase($"p1-persistence-{Guid.NewGuid():N}")
            .Options;
        return new TransportErpDbContext(options);
    }

    private static CreateReceiptVoucherCommand ReceiptCommand(string externalReference) => new(
        Guid.NewGuid(), Guid.NewGuid(), "RV-0001", DateTime.UtcNow, "عميل الاختبار", "ACCOUNT", null,
        "CASH", 100m, Guid.NewGuid(), Guid.NewGuid(), null, null, externalReference);

    private static CreatePaymentVoucherCommand PaymentCommand(string externalReference) => new(
        Guid.NewGuid(), Guid.NewGuid(), "PV-0001", DateTime.UtcNow, "مورد الاختبار", "EXPENSE", null,
        "BANK", 75m, Guid.NewGuid(), Guid.NewGuid(), null, null, externalReference);
}
