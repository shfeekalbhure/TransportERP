namespace TransportERP.Contracts.Attachments;

public enum AttachmentOwnerKind
{
    Generic,
    Waybill,
    WaybillItem,
    Delivery,
    ShipmentException
}

/// <summary>
/// Provider-neutral attachment metadata. Binary storage remains outside the contract layer.
/// </summary>
public sealed record AttachmentDescriptor(
    Guid AttachmentId,
    AttachmentOwnerKind OwnerKind,
    Guid OwnerId,
    string AttachmentType,
    string StorageRef,
    string ContentHash,
    long ContentLength,
    string MediaType,
    Guid AddedBy,
    DateTimeOffset AddedAt)
{
    public void EnsureComplete()
    {
        if (AttachmentId == Guid.Empty || OwnerId == Guid.Empty || AddedBy == Guid.Empty)
        {
            throw new ArgumentException("Attachment, owner, and actor identities are required.");
        }
        if (!Enum.IsDefined(OwnerKind))
        {
            throw new ArgumentException("A defined attachment owner kind is required.", nameof(OwnerKind));
        }
        if (string.IsNullOrWhiteSpace(AttachmentType) || string.IsNullOrWhiteSpace(StorageRef) ||
            string.IsNullOrWhiteSpace(ContentHash) || string.IsNullOrWhiteSpace(MediaType))
        {
            throw new ArgumentException("Attachment type, storage reference, hash, and media type are required.");
        }
        if (ContentLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ContentLength));
        }
        if (AddedAt == default)
        {
            throw new ArgumentException("Attachment timestamp is required.", nameof(AddedAt));
        }
    }
}
