using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed record CreateReceiptVoucherCommand(
    Guid CompanyId,
    Guid BranchId,
    string VoucherNo,
    DateTime VoucherDate,
    string PayerName,
    string ReferenceType,
    Guid? ReferenceId,
    string PaymentMethodCode,
    decimal Amount,
    Guid CurrencyId,
    Guid CollectedBy,
    Guid? CashBoxId,
    string? Notes,
    string? ExternalReference);

public sealed record CreatePaymentVoucherCommand(
    Guid CompanyId,
    Guid BranchId,
    string VoucherNo,
    DateTime VoucherDate,
    string PayeeName,
    string ReferenceType,
    Guid? ReferenceId,
    string PaymentMethodCode,
    decimal Amount,
    Guid CurrencyId,
    Guid PaidBy,
    Guid? CashBoxId,
    string? Notes,
    string? ExternalReference);

public sealed class VoucherLifecycleService(TransportErpDbContext db)
{
    public async Task<ReceiptVoucher> CreateReceiptAsync(CreateReceiptVoucherCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await db.ReceiptVouchers.SingleOrDefaultAsync(x =>
            x.CompanyId == command.CompanyId && x.ExternalReference == command.ExternalReference && command.ExternalReference != null,
            cancellationToken);
        if (existing is not null) return existing;

        var voucher = new ReceiptVoucher
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            BranchId = command.BranchId,
            VoucherNo = command.VoucherNo,
            VoucherDate = command.VoucherDate,
            PayerName = command.PayerName,
            ReferenceType = command.ReferenceType,
            ReferenceId = command.ReferenceId,
            PaymentMethodCode = command.PaymentMethodCode,
            Amount = command.Amount,
            CurrencyId = command.CurrencyId,
            CollectedBy = command.CollectedBy,
            CashBoxId = command.CashBoxId,
            Notes = command.Notes,
            ExternalReference = command.ExternalReference,
            Status = "DRAFT",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = NewVersion()
        };
        db.ReceiptVouchers.Add(voucher);
        await db.SaveChangesAsync(cancellationToken);
        return voucher;
    }

    public async Task<PaymentVoucher> CreatePaymentAsync(CreatePaymentVoucherCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await db.PaymentVouchers.SingleOrDefaultAsync(x =>
            x.CompanyId == command.CompanyId && x.ExternalReference == command.ExternalReference && command.ExternalReference != null,
            cancellationToken);
        if (existing is not null) return existing;

        var voucher = new PaymentVoucher
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            BranchId = command.BranchId,
            VoucherNo = command.VoucherNo,
            VoucherDate = command.VoucherDate,
            PayeeName = command.PayeeName,
            ReferenceType = command.ReferenceType,
            ReferenceId = command.ReferenceId,
            PaymentMethodCode = command.PaymentMethodCode,
            Amount = command.Amount,
            CurrencyId = command.CurrencyId,
            PaidBy = command.PaidBy,
            CashBoxId = command.CashBoxId,
            Notes = command.Notes,
            ExternalReference = command.ExternalReference,
            Status = "DRAFT",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = NewVersion()
        };
        db.PaymentVouchers.Add(voucher);
        await db.SaveChangesAsync(cancellationToken);
        return voucher;
    }

    public Task<ReceiptVoucher> ApproveReceiptAsync(Guid companyId, Guid voucherId, Guid actorId, CancellationToken cancellationToken = default) =>
        TransitionAsync(db.ReceiptVouchers, companyId, voucherId, "DRAFT", "APPROVED", cancellationToken);

    public Task<ReceiptVoucher> PostReceiptAsync(Guid companyId, Guid voucherId, Guid actorId, CancellationToken cancellationToken = default) =>
        TransitionAsync(db.ReceiptVouchers, companyId, voucherId, "APPROVED", "POSTED", cancellationToken);

    public Task<ReceiptVoucher> CancelReceiptAsync(Guid companyId, Guid voucherId, string reason, Guid actorId, CancellationToken cancellationToken = default) =>
        CancelAsync(db.ReceiptVouchers, companyId, voucherId, reason, cancellationToken);

    public Task<PaymentVoucher> ApprovePaymentAsync(Guid companyId, Guid voucherId, Guid actorId, CancellationToken cancellationToken = default) =>
        TransitionAsync(db.PaymentVouchers, companyId, voucherId, "DRAFT", "APPROVED", cancellationToken);

    public Task<PaymentVoucher> PostPaymentAsync(Guid companyId, Guid voucherId, Guid actorId, CancellationToken cancellationToken = default) =>
        TransitionAsync(db.PaymentVouchers, companyId, voucherId, "APPROVED", "POSTED", cancellationToken);

    public Task<PaymentVoucher> CancelPaymentAsync(Guid companyId, Guid voucherId, string reason, Guid actorId, CancellationToken cancellationToken = default) =>
        CancelAsync(db.PaymentVouchers, companyId, voucherId, reason, cancellationToken);

    private async Task<TEntity> TransitionAsync<TEntity>(DbSet<TEntity> set, Guid companyId, Guid voucherId, string expected, string next, CancellationToken cancellationToken)
        where TEntity : P1Entity, IP1Voucher
    {
        var voucher = await set.SingleOrDefaultAsync(x => x.Id == voucherId && x.CompanyId == companyId, cancellationToken)
            ?? throw new KeyNotFoundException($"Voucher {voucherId} was not found in company {companyId}.");
        if (!string.Equals(voucher.Status, expected, StringComparison.Ordinal))
            throw new InvalidOperationException($"Voucher transition {voucher.Status}->{next} is not allowed; expected {expected}.");
        voucher.Status = next;
        voucher.UpdatedAt = DateTimeOffset.UtcNow;
        voucher.RowVersion = NewVersion();
        await db.SaveChangesAsync(cancellationToken);
        return voucher;
    }

    private async Task<TEntity> CancelAsync<TEntity>(DbSet<TEntity> set, Guid companyId, Guid voucherId, string reason, CancellationToken cancellationToken)
        where TEntity : P1Entity, IP1Voucher
    {
        var voucher = await set.SingleOrDefaultAsync(x => x.Id == voucherId && x.CompanyId == companyId, cancellationToken)
            ?? throw new KeyNotFoundException($"Voucher {voucherId} was not found in company {companyId}.");
        if (string.Equals(voucher.Status, "POSTED", StringComparison.Ordinal))
            throw new InvalidOperationException("A posted voucher cannot be cancelled.");
        if (string.Equals(voucher.Status, "CANCELLED", StringComparison.Ordinal))
            return voucher;
        voucher.Status = "CANCELLED";
        voucher.Notes = string.IsNullOrWhiteSpace(voucher.Notes) ? reason : $"{voucher.Notes}\n{reason}";
        voucher.UpdatedAt = DateTimeOffset.UtcNow;
        voucher.RowVersion = NewVersion();
        await db.SaveChangesAsync(cancellationToken);
        return voucher;
    }

    private static byte[] NewVersion() => Guid.NewGuid().ToByteArray();
}
