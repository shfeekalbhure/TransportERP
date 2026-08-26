using System.Text.Json;
using System.Text.Json.Serialization;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Application.Sync;

/// <summary>
/// Closed, typed routing for the governed sync-v1 actions. No reflection, service lookup,
/// or action-name-to-type convention is used: every executable action has an explicit case.
/// </summary>
public sealed class SyncBusinessDispatcher(
    ISyncWaybillBusinessAdapter waybills,
    ISyncFinanceBusinessAdapter finance,
    ISyncShippingBusinessAdapter shipping,
    ISyncBusinessDispatchAuditSink audit)
{
    private static readonly JsonSerializerOptions PayloadOptions = new()
    {
        PropertyNameCaseInsensitive = false,
        AllowTrailingCommas = false,
        ReadCommentHandling = JsonCommentHandling.Disallow,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 32
    };

    public async Task<SyncBusinessDispatchResult> DispatchAsync(
        SyncBusinessActorContext actor,
        SyncBusinessDispatchCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await DispatchCoreAsync(actor, command, cancellationToken);
        await audit.WriteAsync(new SyncBusinessDispatchAuditRecord(
            actor.CompanyId, actor.BranchId, actor.UserId, actor.RegisteredDeviceId,
            command.OperationCorrelationId, command.ClientOperationId, command.ActionCode,
            command.EntityId, command.BaseVersion, result.Status, result.ResultEntityId,
            result.ResultVersion, result.ErrorCode), cancellationToken);
        return result;
    }

    private async Task<SyncBusinessDispatchResult> DispatchCoreAsync(
        SyncBusinessActorContext actor,
        SyncBusinessDispatchCommand command,
        CancellationToken cancellationToken)
    {
        if (!HasCompleteActor(actor) || !HasMatchingScope(actor, command))
            return SyncBusinessDispatchResult.Rejected("SCOPE_DENIED");
        if (!string.Equals(command.ProtocolVersion, "sync-v1", StringComparison.Ordinal))
            return SyncBusinessDispatchResult.Rejected("PROTOCOL_VERSION_UNSUPPORTED");
        if (string.IsNullOrWhiteSpace(command.ClientOperationId) || command.ClientOperationId.Trim().Length > 160 ||
            command.OperationCorrelationId == Guid.Empty || string.IsNullOrWhiteSpace(command.PayloadJson) ||
            command.EntityId == Guid.Empty || command.BaseVersion is <= 0)
            return SyncBusinessDispatchResult.Rejected("ACTION_CONTRACT_MISMATCH");

        var shape = SyncActionCatalog.ValidateShape(
            command.ActionCode, command.OperationType, command.EntityType, command.EntityId, command.BaseVersion);
        if (shape.Definition is not null && !actor.Permissions.Contains(shape.Definition.RequiredPermission))
            return SyncBusinessDispatchResult.Rejected("SCOPE_DENIED");
        if (shape.ErrorCode is not null)
            return SyncBusinessDispatchResult.Rejected(shape.ErrorCode);
        var definition = shape.Definition!;
        if (definition.DispatcherSupport != SyncActionDispatcherSupport.Supported)
            return SyncBusinessDispatchResult.Rejected("ACTION_RUNTIME_UNAVAILABLE");

        var operationId = command.ClientOperationId.Trim();
        var execution = new SyncBusinessExecutionContext(
            new OperationContext(actor.UserId, actor.CompanyId, actor.BranchId, actor.CorrelationId),
            actor.RegisteredDeviceId, operationId, command.OperationCorrelationId);

        try
        {
            var result = definition.ActionCode switch
            {
                SyncActionCode.CreateWaybillDraft => await CreateWaybillDraft(execution, command, operationId, cancellationToken),
                SyncActionCode.UpdateWaybillDraft => await UpdateWaybillDraft(execution, command, operationId, cancellationToken),
                SyncActionCode.CreateOperationalParty => await CreateOperationalParty(execution, command, operationId, cancellationToken),
                SyncActionCode.RecordCollection => await RecordCollection(execution, command, operationId, cancellationToken),
                SyncActionCode.LoadAllocatedQuantity => await LoadAllocatedQuantity(execution, command, operationId, cancellationToken),
                _ => null
            };
            if (result is null)
                return SyncBusinessDispatchResult.Rejected("ACTION_RUNTIME_UNAVAILABLE");
            if (result.EntityId == Guid.Empty ||
                (definition.ResultVersionRequired && !result.Version.HasValue) ||
                (result.Version.HasValue && result.Version.Value <= 0))
                return SyncBusinessDispatchResult.Rejected("BUSINESS_RESULT_INVALID");
            return SyncBusinessDispatchResult.Succeeded(result.EntityId, result.Version);
        }
        catch (JsonException)
        {
            return SyncBusinessDispatchResult.Rejected("PAYLOAD_INVALID");
        }
        catch (NotSupportedException)
        {
            return SyncBusinessDispatchResult.Rejected("PAYLOAD_INVALID");
        }
        catch (SyncBusinessPayloadException exception)
        {
            return SyncBusinessDispatchResult.Rejected(exception.Code);
        }
    }

    private async Task<SyncBusinessActionResult> CreateWaybillDraft(
        SyncBusinessExecutionContext execution,
        SyncBusinessDispatchCommand command,
        string operationId,
        CancellationToken cancellationToken)
    {
        var payload = ReadPayload<CreateWaybillDraftRequest>(command.PayloadJson);
        EnsureOperation(payload.ClientOperationId, operationId);
        if (payload.BranchId != execution.Operation.BranchId)
            throw new SyncBusinessPayloadException("SCOPE_DENIED");
        return await waybills.CreateDraftAsync(execution, payload, cancellationToken);
    }

    private async Task<SyncBusinessActionResult> UpdateWaybillDraft(
        SyncBusinessExecutionContext execution,
        SyncBusinessDispatchCommand command,
        string operationId,
        CancellationToken cancellationToken)
    {
        var payload = ReadPayload<UpdateWaybillDraftRequest>(command.PayloadJson);
        if (payload.Parties is null || payload.Items is null)
            throw new SyncBusinessPayloadException("PAYLOAD_INVALID");
        EnsureOperation(payload.ClientOperationId, operationId);
        if (payload.ExpectedVersion != command.BaseVersion)
            throw new SyncBusinessPayloadException("ACTION_CONTRACT_MISMATCH");
        return await waybills.UpdateDraftAsync(execution, command.EntityId!.Value, payload, cancellationToken);
    }

    private async Task<SyncBusinessActionResult> CreateOperationalParty(
        SyncBusinessExecutionContext execution,
        SyncBusinessDispatchCommand command,
        string operationId,
        CancellationToken cancellationToken)
    {
        var payload = ReadPayload<OperationalPartyCreateRequest>(command.PayloadJson);
        if (payload.Address is null)
            throw new SyncBusinessPayloadException("PAYLOAD_INVALID");
        EnsureOperation(payload.ClientOperationId, operationId);
        return await waybills.CreateOperationalPartyAsync(execution, payload, cancellationToken);
    }

    private async Task<SyncBusinessActionResult> RecordCollection(
        SyncBusinessExecutionContext execution,
        SyncBusinessDispatchCommand command,
        string operationId,
        CancellationToken cancellationToken)
    {
        var payload = ReadPayload<RecordCollectionRequest>(command.PayloadJson);
        if (payload.Amount is null)
            throw new SyncBusinessPayloadException("PAYLOAD_INVALID");
        EnsureOperation(payload.ClientOperationId, operationId);
        return await finance.RecordCollectionAsync(execution, command.EntityId!.Value, payload, cancellationToken);
    }

    private async Task<SyncBusinessActionResult> LoadAllocatedQuantity(
        SyncBusinessExecutionContext execution,
        SyncBusinessDispatchCommand command,
        string operationId,
        CancellationToken cancellationToken)
    {
        var payload = ReadPayload<SyncLoadAllocatedQuantityPayload>(command.PayloadJson);
        if (payload.ManifestId == Guid.Empty || payload.Request is null)
            throw new SyncBusinessPayloadException("PAYLOAD_INVALID");
        EnsureOperation(payload.Request.ClientOperationId, operationId);
        return await shipping.LoadAllocatedQuantityAsync(
            execution, command.EntityId!.Value, payload, cancellationToken);
    }

    private static T ReadPayload<T>(string json) where T : class
        => JsonSerializer.Deserialize<T>(json, PayloadOptions)
           ?? throw new JsonException("The action payload cannot be null.");

    private static void EnsureOperation(string payloadOperationId, string commandOperationId)
    {
        if (!string.Equals(payloadOperationId?.Trim(), commandOperationId, StringComparison.Ordinal))
            throw new SyncBusinessPayloadException("IDEMPOTENCY_CONFLICT");
    }

    private static bool HasCompleteActor(SyncBusinessActorContext actor)
        => actor.CompanyId != Guid.Empty && actor.BranchId != Guid.Empty && actor.UserId != Guid.Empty &&
           actor.RegisteredDeviceId != Guid.Empty && actor.CorrelationId != Guid.Empty && actor.Permissions is not null;

    private static bool HasMatchingScope(SyncBusinessActorContext actor, SyncBusinessDispatchCommand command)
        => actor.CompanyId == command.CompanyId && actor.BranchId == command.BranchId &&
           actor.UserId == command.UserId && actor.RegisteredDeviceId == command.RegisteredDeviceId;
}

internal sealed class SyncBusinessPayloadException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
