using TransportERP.Api.Sync;
using TransportERP.Application.Sync;

namespace TransportERP.Tests;

public sealed class Stage4SyncActionCatalogTests
{
    public static IEnumerable<object[]> GovernedActions()
    {
        yield return Action("CreateJournalEntry", "CREATE", "JournalEntry", false, false);
        yield return Action("CreateReceiptVoucher", "CREATE", "ReceiptVoucher", false, false);
        yield return Action("CreatePaymentVoucher", "CREATE", "PaymentVoucher", false, false);
        yield return Action("CreateWaybillDraft", "CREATE", "Waybill", false, false);
        yield return Action("UpdateWaybillDraft", "UPDATE", "Waybill", true, true);
        yield return Action("CreateOperationalParty", "CREATE", "OperationalParty", false, false);
        yield return Action("AddWaybillAttachment", "CREATE", "Waybill", true, false);
        yield return Action("RecordCollection", "COMMAND", "Waybill", true, false);
        yield return Action("LoadAllocatedQuantity", "COMMAND", "ManifestLine", true, false);
        yield return Action("RecordArrival", "COMMAND", "Trip", true, false);
        yield return Action("RecordUnload", "COMMAND", "ArrivalReceipt", true, false);
        yield return Action("DeliverQuantity", "COMMAND", "Waybill", true, false);
        yield return Action("RecordProofOfDelivery", "CREATE", "Delivery", true, false);
        yield return Action("CreateShipmentException", "COMMAND", "Waybill", true, false);
    }

    [Fact]
    public void Catalog_contains_exactly_the_fourteen_governed_actions_and_keeps_runtime_closed()
    {
        var expected = GovernedActions().Select(x => Assert.IsType<string>(x[0])).ToArray();

        Assert.Equal(expected, SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue));
        Assert.Equal(14, SyncActionCatalog.Definitions.Count);
        Assert.Equal(14, SyncActionCatalog.Definitions.Select(x => x.ActionCode).Distinct().Count());
        Assert.All(SyncActionCatalog.Definitions, definition =>
            Assert.Equal(SyncActionRuntimeAvailability.Unavailable, definition.RuntimeAvailability));
    }

    [Theory]
    [MemberData(nameof(GovernedActions))]
    public void Every_governed_action_has_an_exact_typed_shape_and_explicit_unavailable_result(
        string actionCode,
        string operationType,
        string entityType,
        bool entityRequired,
        bool baseVersionRequired)
    {
        var entityId = entityRequired ? Guid.NewGuid() : null;
        var baseVersion = baseVersionRequired ? 7L : null;

        var result = SyncActionCatalog.Validate(actionCode, operationType, entityType, entityId, baseVersion);

        Assert.NotNull(result.Definition);
        Assert.Equal(actionCode, result.Definition!.ActionCodeValue);
        Assert.Equal(operationType, result.Definition.OperationTypeValue);
        Assert.Equal(entityType, result.Definition.EntityTypeValue);
        Assert.Equal(entityRequired ? SyncValueRequirement.Required : SyncValueRequirement.Optional,
            result.Definition.EntityId);
        Assert.Equal(baseVersionRequired ? SyncValueRequirement.Required : SyncValueRequirement.Forbidden,
            result.Definition.BaseVersion);
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", result.ErrorCode);
        Assert.False(result.IsAcceptedForExecution);
    }

    [Theory]
    [MemberData(nameof(GovernedActions))]
    public void Every_governed_action_rejects_a_case_drifted_operation_type_before_runtime_dispatch(
        string actionCode,
        string operationType,
        string entityType,
        bool entityRequired,
        bool baseVersionRequired)
    {
        var result = SyncActionCatalog.Validate(actionCode, operationType.ToLowerInvariant(), entityType,
            entityRequired ? Guid.NewGuid() : null, baseVersionRequired ? 1L : null);

        Assert.Equal("ACTION_CONTRACT_MISMATCH", result.ErrorCode);
    }

    [Fact]
    public void Unknown_actions_and_generic_delete_remain_online_only()
    {
        Assert.Equal("ONLINE_REQUIRED",
            SyncActionCatalog.Validate("UnknownAction", "CREATE", "Waybill", null, null).ErrorCode);
        Assert.Equal("ONLINE_REQUIRED",
            SyncActionCatalog.Validate("CreateWaybillDraft", "DELETE", "Waybill", null, null).ErrorCode);
    }

    [Theory]
    [InlineData(0, "BATCH_SIZE_INVALID")]
    [InlineData(1, null)]
    [InlineData(100, null)]
    [InlineData(101, "BATCH_SIZE_INVALID")]
    public void Batch_boundaries_have_the_governed_error_code(int operationCount, string? expectedError)
    {
        var request = Request("sync-v1", operationCount);

        Assert.Equal(expectedError,
            SyncBatchEnvelopeContract.Validate(request, "device-1", SyncApiModule.MaximumBatchOperations));
    }

    [Theory]
    [InlineData("sync-v0")]
    [InlineData("SYNC-V1")]
    [InlineData("")]
    public void Unsupported_protocol_has_its_governed_error_code(string protocolVersion)
    {
        Assert.Equal("PROTOCOL_VERSION_UNSUPPORTED",
            SyncBatchEnvelopeContract.Validate(Request(protocolVersion, 1), "device-1",
                SyncApiModule.MaximumBatchOperations));
    }

    private static object[] Action(
        string actionCode,
        string operationType,
        string entityType,
        bool entityRequired,
        bool baseVersionRequired)
        => [actionCode, operationType, entityType, entityRequired, baseVersionRequired];

    private static SyncBatchRequest Request(string protocolVersion, int operationCount)
        => new("device-1", protocolVersion,
            Enumerable.Range(0, operationCount).Select(_ => (SyncBatchOperationRequest?)null).ToArray());
}
