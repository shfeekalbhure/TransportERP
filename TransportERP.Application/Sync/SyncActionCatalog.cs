namespace TransportERP.Application.Sync;

public enum SyncActionCode
{
    CreateJournalEntry,
    CreateReceiptVoucher,
    CreatePaymentVoucher,
    CreateWaybillDraft,
    UpdateWaybillDraft,
    CreateOperationalParty,
    AddWaybillAttachment,
    RecordCollection,
    LoadAllocatedQuantity,
    RecordArrival,
    RecordUnload,
    DeliverQuantity,
    RecordProofOfDelivery,
    CreateShipmentException
}

public enum SyncOperationKind
{
    Create,
    Update,
    Command
}

public enum SyncEntityKind
{
    JournalEntry,
    ReceiptVoucher,
    PaymentVoucher,
    Waybill,
    OperationalParty,
    ManifestLine,
    Trip,
    ArrivalReceipt,
    Delivery
}

public enum SyncValueRequirement
{
    Optional,
    Required,
    Forbidden
}

public enum SyncActionRuntimeAvailability
{
    Unavailable,
    Available
}

public sealed record SyncActionDefinition(
    SyncActionCode ActionCode,
    SyncOperationKind OperationKind,
    SyncEntityKind EntityKind,
    SyncValueRequirement EntityId,
    SyncValueRequirement BaseVersion,
    SyncActionRuntimeAvailability RuntimeAvailability)
{
    public string ActionCodeValue => ActionCode.ToString();
    public string OperationTypeValue => OperationKind switch
    {
        SyncOperationKind.Create => "CREATE",
        SyncOperationKind.Update => "UPDATE",
        SyncOperationKind.Command => "COMMAND",
        _ => throw new InvalidOperationException($"Unsupported sync operation kind: {OperationKind}.")
    };
    public string EntityTypeValue => EntityKind.ToString();
}

public sealed record SyncActionValidationResult(
    SyncActionDefinition? Definition,
    string? ErrorCode)
{
    public bool IsAcceptedForExecution => Definition is not null && ErrorCode is null;
}

/// <summary>
/// The single typed catalog for sync-v1 write actions. Runtime availability is
/// deliberately explicit and remains closed until a separately reviewed
/// dispatcher is installed for an action.
/// </summary>
public static class SyncActionCatalog
{
    private static readonly IReadOnlyList<SyncActionDefinition> Catalog = Array.AsReadOnly<SyncActionDefinition>(
    [
        Define(SyncActionCode.CreateJournalEntry, SyncOperationKind.Create, SyncEntityKind.JournalEntry,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.CreateReceiptVoucher, SyncOperationKind.Create, SyncEntityKind.ReceiptVoucher,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.CreatePaymentVoucher, SyncOperationKind.Create, SyncEntityKind.PaymentVoucher,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.CreateWaybillDraft, SyncOperationKind.Create, SyncEntityKind.Waybill,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.UpdateWaybillDraft, SyncOperationKind.Update, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Required),
        Define(SyncActionCode.CreateOperationalParty, SyncOperationKind.Create, SyncEntityKind.OperationalParty,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.AddWaybillAttachment, SyncOperationKind.Create, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.RecordCollection, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.LoadAllocatedQuantity, SyncOperationKind.Command, SyncEntityKind.ManifestLine,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.RecordArrival, SyncOperationKind.Command, SyncEntityKind.Trip,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.RecordUnload, SyncOperationKind.Command, SyncEntityKind.ArrivalReceipt,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.DeliverQuantity, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.RecordProofOfDelivery, SyncOperationKind.Create, SyncEntityKind.Delivery,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden),
        Define(SyncActionCode.CreateShipmentException, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden)
    ]);

    private static readonly IReadOnlyDictionary<string, SyncActionDefinition> ByCode =
        Catalog.ToDictionary(x => x.ActionCodeValue, StringComparer.Ordinal);

    public static IReadOnlyList<SyncActionDefinition> Definitions => Catalog;

    public static SyncActionValidationResult Validate(
        string? actionCode,
        string? operationType,
        string? entityType,
        Guid? entityId,
        long? baseVersion)
    {
        if (string.Equals(operationType, "DELETE", StringComparison.Ordinal) ||
            actionCode is null ||
            !ByCode.TryGetValue(actionCode, out var definition))
            return new(null, "ONLINE_REQUIRED");

        if (!string.Equals(operationType, definition.OperationTypeValue, StringComparison.Ordinal) ||
            !string.Equals(entityType, definition.EntityTypeValue, StringComparison.Ordinal) ||
            !Satisfies(definition.EntityId, entityId.HasValue) ||
            !Satisfies(definition.BaseVersion, baseVersion.HasValue))
            return new(definition, "ACTION_CONTRACT_MISMATCH");

        return definition.RuntimeAvailability switch
        {
            SyncActionRuntimeAvailability.Available => new(definition, null),
            SyncActionRuntimeAvailability.Unavailable => new(definition, "ACTION_RUNTIME_UNAVAILABLE"),
            _ => new(definition, "ACTION_RUNTIME_UNAVAILABLE")
        };
    }

    private static SyncActionDefinition Define(
        SyncActionCode actionCode,
        SyncOperationKind operationKind,
        SyncEntityKind entityKind,
        SyncValueRequirement entityId,
        SyncValueRequirement baseVersion)
        => new(actionCode, operationKind, entityKind, entityId, baseVersion,
            SyncActionRuntimeAvailability.Unavailable);

    private static bool Satisfies(SyncValueRequirement requirement, bool isPresent) => requirement switch
    {
        SyncValueRequirement.Optional => true,
        SyncValueRequirement.Required => isPresent,
        SyncValueRequirement.Forbidden => !isPresent,
        _ => false
    };
}
