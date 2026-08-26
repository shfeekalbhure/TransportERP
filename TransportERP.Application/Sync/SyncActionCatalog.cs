using TransportERP.Contracts.Waybills;

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

public enum SyncActionDispatcherSupport
{
    Unavailable,
    Supported
}

public sealed record SyncActionDefinition(
    SyncActionCode ActionCode,
    SyncOperationKind OperationKind,
    SyncEntityKind EntityKind,
    SyncValueRequirement EntityId,
    SyncValueRequirement BaseVersion,
    string RequiredPermission,
    bool ResultVersionRequired,
    SyncActionDispatcherSupport DispatcherSupport,
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
/// The single typed catalog for sync-v1 write actions. Runtime availability is explicit per action:
/// only actions backed by the typed dispatcher and server executor are available. This decision is
/// separate from the owner-controlled Offline/HTTP gate and worker activation setting.
/// </summary>
public static class SyncActionCatalog
{
    private static readonly IReadOnlyList<SyncActionDefinition> Catalog = Array.AsReadOnly<SyncActionDefinition>(
    [
        Define(SyncActionCode.CreateJournalEntry, SyncOperationKind.Create, SyncEntityKind.JournalEntry,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden, "accounting.journal.create"),
        Define(SyncActionCode.CreateReceiptVoucher, SyncOperationKind.Create, SyncEntityKind.ReceiptVoucher,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden, "accounting.receipts.create"),
        Define(SyncActionCode.CreatePaymentVoucher, SyncOperationKind.Create, SyncEntityKind.PaymentVoucher,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden, "accounting.payments.create"),
        Define(SyncActionCode.CreateWaybillDraft, SyncOperationKind.Create, SyncEntityKind.Waybill,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden, WaybillPermissionCodes.Create, supported: true),
        Define(SyncActionCode.UpdateWaybillDraft, SyncOperationKind.Update, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Required, WaybillPermissionCodes.Edit, supported: true),
        Define(SyncActionCode.CreateOperationalParty, SyncOperationKind.Create, SyncEntityKind.OperationalParty,
            SyncValueRequirement.Optional, SyncValueRequirement.Forbidden, WaybillPermissionCodes.PartyCreate, supported: true),
        Define(SyncActionCode.AddWaybillAttachment, SyncOperationKind.Create, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "waybill.attachment.add"),
        Define(SyncActionCode.RecordCollection, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, WaybillFinancePermissionCodes.CollectionCreate, supported: true),
        Define(SyncActionCode.LoadAllocatedQuantity, SyncOperationKind.Command, SyncEntityKind.ManifestLine,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, ShippingExecutionPermissionCodes.ManifestLoad,
            supported: true, resultVersionRequired: false),
        Define(SyncActionCode.RecordArrival, SyncOperationKind.Command, SyncEntityKind.Trip,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "arrival.record"),
        Define(SyncActionCode.RecordUnload, SyncOperationKind.Command, SyncEntityKind.ArrivalReceipt,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "arrival.unload"),
        Define(SyncActionCode.DeliverQuantity, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "waybill.deliver"),
        Define(SyncActionCode.RecordProofOfDelivery, SyncOperationKind.Create, SyncEntityKind.Delivery,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "waybill.pod.capture"),
        Define(SyncActionCode.CreateShipmentException, SyncOperationKind.Command, SyncEntityKind.Waybill,
            SyncValueRequirement.Required, SyncValueRequirement.Forbidden, "waybill.exception.create")
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
        var shape = ValidateShape(actionCode, operationType, entityType, entityId, baseVersion);
        if (shape.ErrorCode is not null) return shape;
        var definition = shape.Definition!;

        return definition.RuntimeAvailability switch
        {
            SyncActionRuntimeAvailability.Available => new(definition, null),
            SyncActionRuntimeAvailability.Unavailable => new(definition, "ACTION_RUNTIME_UNAVAILABLE"),
            _ => new(definition, "ACTION_RUNTIME_UNAVAILABLE")
        };
    }

    public static SyncActionValidationResult ValidateShape(
        string? actionCode,
        string? operationType,
        string? entityType,
        Guid? entityId,
        long? baseVersion)
    {
        if (string.Equals(operationType, "DELETE", StringComparison.Ordinal) ||
            actionCode is null || !ByCode.TryGetValue(actionCode, out var definition))
            return new(null, "ONLINE_REQUIRED");

        if (!string.Equals(operationType, definition.OperationTypeValue, StringComparison.Ordinal) ||
            !string.Equals(entityType, definition.EntityTypeValue, StringComparison.Ordinal) ||
            !Satisfies(definition.EntityId, entityId.HasValue) ||
            !Satisfies(definition.BaseVersion, baseVersion.HasValue))
            return new(definition, "ACTION_CONTRACT_MISMATCH");

        return new(definition, null);
    }

    private static SyncActionDefinition Define(
        SyncActionCode actionCode,
        SyncOperationKind operationKind,
        SyncEntityKind entityKind,
        SyncValueRequirement entityId,
        SyncValueRequirement baseVersion,
        string requiredPermission,
        bool supported = false,
        bool resultVersionRequired = true)
        => new(actionCode, operationKind, entityKind, entityId, baseVersion,
            requiredPermission,
            resultVersionRequired,
            supported ? SyncActionDispatcherSupport.Supported : SyncActionDispatcherSupport.Unavailable,
            supported ? SyncActionRuntimeAvailability.Available : SyncActionRuntimeAvailability.Unavailable);

    private static bool Satisfies(SyncValueRequirement requirement, bool isPresent) => requirement switch
    {
        SyncValueRequirement.Optional => true,
        SyncValueRequirement.Required => isPresent,
        SyncValueRequirement.Forbidden => !isPresent,
        _ => false
    };
}
