using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;

namespace TransportERP.Infrastructure.Persistence;

public sealed class ReceiptAttachment
{
    public Guid Id { get; set; }
    public Guid ReceiptId { get; set; }
    public Guid AddedBy { get; set; }
    public DateTimeOffset AddedAt { get; set; }
    public string FileName { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string Hash { get; set; } = "";
    public byte[] Content { get; set; } = [];
}

public sealed partial class ReceiptWorkspaceService
{
    public const int MaxAttachmentBytes = 5 * 1024 * 1024;
    public async Task<ReceiptDocument> AddAttachmentAsync(OperationContext scope, Guid receiptId, ReceiptAttachmentUpload upload, CancellationToken ct = default)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var receipt = await Find(scope, receiptId, ct);
        if (upload.Id == Guid.Empty || string.IsNullOrWhiteSpace(upload.FileName) || upload.FileName.Length > 200 ||
            upload.FileName.IndexOfAny(['/', '\\', ':', '\0', '\r', '\n']) >= 0 || upload.Content is not { Length: > 0 and <= MaxAttachmentBytes })
            throw new InvalidOperationException("اسم المرفق أو حجمه غير صالح؛ الحد 5 ميجابايت.");
        string media = AttachmentMedia(upload.FileName, upload.Content);
        string hash = Convert.ToHexString(SHA256.HashData(upload.Content));
        var existing = await db.Set<ReceiptAttachment>().SingleOrDefaultAsync(a => a.Id == upload.Id, ct);
        if (existing != null)
        {
            if (existing.ReceiptId == receiptId && existing.Hash == hash && existing.FileName == upload.FileName) return await GetAsync(scope, receiptId, ct);
            throw new InvalidOperationException("معرف المرفق مستخدم بالفعل.");
        }
        Version(receipt, upload.Version);
        if (receipt.Status != "DRAFT") throw new InvalidOperationException("إضافة المرفقات متاحة للمسودة المحفوظة فقط.");
        if (await db.Set<ReceiptAttachment>().CountAsync(a => a.ReceiptId == receiptId, ct) >= 20)
            throw new InvalidOperationException("الحد الأقصى 20 مرفقًا لكل سند.");
        db.Add(new ReceiptAttachment { Id = upload.Id, ReceiptId = receiptId, FileName = upload.FileName, MediaType = media,
            Content = upload.Content, Hash = hash, AddedBy = scope.UserId, AddedAt = DateTimeOffset.UtcNow });
        Stamp(receipt); await Audit(scope, receipt, "AttachReceipt", ct, upload.FileName);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct);
        return await GetAsync(scope, receiptId, ct);
    }

    public async Task<ReceiptAttachmentDownload> DownloadAttachmentAsync(OperationContext scope, Guid receiptId, Guid attachmentId, CancellationToken ct = default)
    {
        await Find(scope, receiptId, ct);
        var a = await db.Set<ReceiptAttachment>().AsNoTracking().SingleOrDefaultAsync(a => a.Id == attachmentId && a.ReceiptId == receiptId, ct)
            ?? throw new KeyNotFoundException();
        return new(new(a.Id, a.FileName, a.MediaType, a.Content.LongLength, a.Hash, a.AddedAt), a.Content);
    }

    private static string AttachmentMedia(string name, byte[] data)
    {
        bool Starts(params byte[] prefix) => data.AsSpan().StartsWith(prefix);
        switch (Path.GetExtension(name).ToLowerInvariant())
        {
            case ".pdf" when Starts(37, 80, 68, 70, 45): return "application/pdf";
            case ".png" when Starts(137, 80, 78, 71, 13, 10, 26, 10): return "image/png";
            case ".jpg" or ".jpeg" when Starts(255, 216, 255): return "image/jpeg";
            case ".txt":
                try { _ = new UTF8Encoding(false, true).GetString(data); if (!data.Contains((byte)0)) return "text/plain"; }
                catch (DecoderFallbackException) { }
                break;
        }
        throw new InvalidOperationException("المرفقات المدعومة: PDF أو PNG أو JPEG أو نص UTF-8 مطابق لنوع الملف.");
    }
}
