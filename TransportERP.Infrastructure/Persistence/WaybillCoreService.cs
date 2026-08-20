using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Infrastructure.Persistence;

public sealed class WaybillCoreService(
    TransportErpDbContext db,
    NumberReservationPersistenceService numbering)
{
    public async Task<OperationalPartyResponse> CreateOperationalPartyAsync(
        OperationContext context,
        CreateOperationalPartyRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        ValidateOperationalPartyInput(request.Name, request.Mobile, request.IdentityType, request.IdentityNo, request.Address);

        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();

        if (!string.IsNullOrWhiteSpace(request.ClientOperationId))
        {
            var existing = await FindOperationalPartyByOperationAsync(
                connection, transaction, context, request.ClientOperationId.Trim(), cancellationToken);
            if (existing is not null)
            {
                await tx.CommitAsync(cancellationToken);
                return existing;
            }
        }

        var duplicate = await FindOperationalPartyDuplicateAsync(
            connection, transaction, context, request.Mobile.Trim(), request.IdentityNo, cancellationToken);
        if (duplicate is not null)
            throw new WaybillCoreRuleException("PARTY_DUPLICATE_WARNING");

        var id = Guid.NewGuid();
        var partyNo = $"P-{Guid.NewGuid():N}"[..14].ToUpperInvariant();
        var now = DateTimeOffset.UtcNow;
        var rowVersion = NewVersionBytes();

        await using (var command = new NpgsqlCommand("""
            INSERT INTO transport_erp.operational_parties
            ("Id","CompanyId","BranchId","PartyNo","Name","Mobile","IdentityType","IdentityNo",
             "CountryId","GovernorateId","DirectorateId","CityId","AreaId","AddressLine","ClientOperationId","Status",
             "CreatedAt","UpdatedAt","RowVersion")
            VALUES
            (@id,@companyId,@branchId,@partyNo,@name,@mobile,@identityType,@identityNo,
             @countryId,@governorateId,@directorateId,@cityId,@areaId,@addressLine,@operationId,'ACTIVE',
             @now,@now,@rowVersion)
            """, connection, transaction))
        {
            Add(command, "id", id);
            Add(command, "companyId", context.CompanyId);
            Add(command, "branchId", context.BranchId);
            Add(command, "partyNo", partyNo);
            Add(command, "name", request.Name.Trim());
            Add(command, "mobile", request.Mobile.Trim());
            Add(command, "identityType", NullIfWhiteSpace(request.IdentityType));
            Add(command, "identityNo", NullIfWhiteSpace(request.IdentityNo));
            AddAddress(command, request.Address);
            Add(command, "operationId", NullIfWhiteSpace(request.ClientOperationId));
            Add(command, "now", now);
            Add(command, "rowVersion", rowVersion);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await AppendAuditWithinTransactionAsync(
            "OperationalPartyCreated", nameof(OperationalParty), id, context, null,
            JsonSerializer.Serialize(new { partyNo, Name = request.Name.Trim(), Mobile = request.Mobile.Trim() }), null,
            cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return new OperationalPartyResponse(id, partyNo, request.Name.Trim(), request.Mobile.Trim(),
            NullIfWhiteSpace(request.IdentityType), NullIfWhiteSpace(request.IdentityNo), request.Address,
            "ACTIVE", Version(rowVersion));
    }

    public async Task<PagedOperationalPartyResponse> SearchOperationalPartiesAsync(
        OperationContext context,
        PartySearchRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        if (request.Skip < 0 || request.Take is < 1 or > 200)
            throw new WaybillCoreRuleException("INVALID_FILTER");

        var connection = await OpenConnectionAsync(cancellationToken);
        var q = NullIfWhiteSpace(request.Query);
        var pattern = q is null ? null : $"%{q}%";

        await using var count = new NpgsqlCommand("""
            SELECT COUNT(*)
            FROM transport_erp.operational_parties
            WHERE "CompanyId"=@companyId
              AND ("BranchId" IS NULL OR "BranchId"=@branchId)
              AND "Status"='ACTIVE'
              AND (@q IS NULL OR "Name" ILIKE @pattern OR "Mobile" ILIKE @pattern OR "IdentityNo" ILIKE @pattern OR "PartyNo" ILIKE @pattern)
            """, connection);
        Add(count, "companyId", context.CompanyId);
        Add(count, "branchId", context.BranchId);
        Add(count, "q", q);
        Add(count, "pattern", pattern);
        var total = Convert.ToInt32(await count.ExecuteScalarAsync(cancellationToken));

        await using var command = new NpgsqlCommand("""
            SELECT "Id","PartyNo","Name","Mobile","IdentityType","IdentityNo",
                   "CountryId","GovernorateId","DirectorateId","CityId","AreaId","AddressLine","Status","RowVersion"
            FROM transport_erp.operational_parties
            WHERE "CompanyId"=@companyId
              AND ("BranchId" IS NULL OR "BranchId"=@branchId)
              AND "Status"='ACTIVE'
              AND (@q IS NULL OR "Name" ILIKE @pattern OR "Mobile" ILIKE @pattern OR "IdentityNo" ILIKE @pattern OR "PartyNo" ILIKE @pattern)
            ORDER BY "Name", "PartyNo"
            OFFSET @skip LIMIT @take
            """, connection);
        Add(command, "companyId", context.CompanyId);
        Add(command, "branchId", context.BranchId);
        Add(command, "q", q);
        Add(command, "pattern", pattern);
        Add(command, "skip", request.Skip);
        Add(command, "take", request.Take);

        var items = new List<OperationalPartyResponse>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            items.Add(ReadOperationalParty(reader));

        return new PagedOperationalPartyResponse(items, total, request.Skip, request.Take);
    }

    public async Task<WaybillDraftResponse> CreateWaybillDraftAsync(
        OperationContext context,
        CreateWaybillDraftRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        ValidateDraftHeader(request.OriginId, request.DestinationId, request.CurrencyId, request.ExchangeRate,
            request.FreightTotal, request.DiscountTotal, request.ServiceType, request.Priority);
        if (string.IsNullOrWhiteSpace(request.ClientOperationId))
            throw new WaybillCoreRuleException("CLIENT_OPERATION_ID_REQUIRED");
        ValidatePartyInputs(request.Parties, allowMissingSenderReceiver: true);
        ValidateItemInputs(request.Items, allowEmpty: true);

        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();

        var existingId = await FindWaybillIdByCreateOperationAsync(
            connection, transaction, context.CompanyId, request.ClientOperationId.Trim(), cancellationToken);
        if (existingId.HasValue)
        {
            var existing = await LoadWaybillAsync(connection, transaction, context, existingId.Value, false, cancellationToken)
                ?? throw new WaybillCoreRuleException("NOT_FOUND");
            await tx.CommitAsync(cancellationToken);
            return ToDraft(existing);
        }

        var id = Guid.NewGuid();
        var draftNo = $"D-{Guid.NewGuid():N}"[..18].ToUpperInvariant();
        var now = DateTimeOffset.UtcNow;
        var version = NewVersionBytes();
        var net = request.FreightTotal - request.DiscountTotal;

        await InsertWaybillHeaderAsync(connection, transaction, id, context, draftNo, request, net, version, now, cancellationToken);
        await ReplacePartiesAsync(connection, transaction, id, request.Parties, cancellationToken);
        await ReplaceItemsAsync(connection, transaction, id, request.Items, cancellationToken);
        await AppendAuditWithinTransactionAsync(
            "WaybillDraftCreated", nameof(Waybill), id, context, null,
            JsonSerializer.Serialize(new { DraftNo = draftNo, request.OriginId, request.DestinationId, Items = request.Items.Count }),
            null, cancellationToken);
        await tx.CommitAsync(cancellationToken);

        var created = await GetWaybillAsync(context, id, cancellationToken);
        return ToDraft(created);
    }

    public async Task<WaybillDraftResponse> UpdateWaybillDraftAsync(
        OperationContext context,
        Guid waybillId,
        UpdateWaybillDraftRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        ValidateDraftHeader(request.OriginId, request.DestinationId, request.CurrencyId, request.ExchangeRate,
            request.FreightTotal, request.DiscountTotal, request.ServiceType, request.Priority);
        ValidatePartyInputs(request.Parties, allowMissingSenderReceiver: true);
        ValidateItemInputs(request.Items, allowEmpty: true);
        var expected = ParseVersion(request.ExpectedVersion);

        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();
        var current = await LoadWaybillAsync(connection, transaction, context, waybillId, true, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");
        EnsureState(current, WaybillOperationalStatuses.Draft);
        EnsureVersion(current.Version, expected);

        var newVersion = NewVersionBytes();
        var now = DateTimeOffset.UtcNow;
        await using (var command = new NpgsqlCommand("""
            UPDATE transport_erp.waybills SET
              "ServicePointId"=@servicePointId,"AgentId"=@agentId,"WaybillDateTime"=@waybillDateTime,
              "RequestDateTime"=@requestDateTime,"ExpectedArrivalAt"=@expectedArrivalAt,"ServiceType"=@serviceType,
              "Priority"=@priority,"OriginId"=@originId,"DestinationId"=@destinationId,"CurrencyId"=@currencyId,
              "ExchangeRate"=@exchangeRate,"FreightTotal"=@freightTotal,"DiscountTotal"=@discountTotal,"NetAmount"=@netAmount,
              "UpdatedAt"=@now,"RowVersion"=@newVersion
            WHERE "Id"=@id AND "CompanyId"=@companyId AND "BranchId"=@branchId AND "OperationalStatus"='DRAFT' AND "RowVersion"=@expectedVersion
            """, connection, transaction))
        {
            AddHeaderParameters(command, request.ServicePointId, request.AgentId, request.WaybillDateTime,
                request.RequestDateTime, request.ExpectedArrivalAt, request.ServiceType, request.Priority, request.OriginId,
                request.DestinationId, request.CurrencyId, request.ExchangeRate, request.FreightTotal, request.DiscountTotal,
                request.FreightTotal - request.DiscountTotal);
            Add(command, "now", now); Add(command, "newVersion", newVersion); Add(command, "id", waybillId);
            Add(command, "companyId", context.CompanyId); Add(command, "branchId", context.BranchId); Add(command, "expectedVersion", expected);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
                throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT");
        }

        await ReplacePartiesAsync(connection, transaction, waybillId, request.Parties, cancellationToken);
        await ReplaceItemsAsync(connection, transaction, waybillId, request.Items, cancellationToken);
        await AppendAuditWithinTransactionAsync(
            "WaybillDraftUpdated", nameof(Waybill), waybillId, context,
            JsonSerializer.Serialize(new { current.OriginId, current.DestinationId, current.FreightTotal, current.DiscountTotal }),
            JsonSerializer.Serialize(new { request.OriginId, request.DestinationId, request.FreightTotal, request.DiscountTotal }),
            null, cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return ToDraft(await GetWaybillAsync(context, waybillId, cancellationToken));
    }

    public async Task<WaybillResponse> GetWaybillAsync(
        OperationContext context,
        Guid waybillId,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        var connection = await OpenConnectionAsync(cancellationToken);
        return await LoadWaybillAsync(connection, null, context, waybillId, false, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");
    }

    public async Task<WaybillValidationResponse> ValidateWaybillAsync(
        OperationContext context,
        Guid waybillId,
        ValidateWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        var waybill = await GetWaybillAsync(context, waybillId, cancellationToken);
        if (!string.IsNullOrWhiteSpace(request.ExpectedVersion))
            EnsureVersion(waybill.Version, ParseVersion(request.ExpectedVersion!));
        var issues = ValidateForApproval(waybill);
        return new WaybillValidationResponse(waybill.Id, issues.All(x => !x.Blocking), issues, waybill.Version);
    }

    public async Task<WaybillResponse> SubmitWaybillForApprovalAsync(
        OperationContext context,
        Guid waybillId,
        SubmitWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        var expected = ParseVersion(request.ExpectedVersion);
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();
        var current = await LoadWaybillAsync(connection, transaction, context, waybillId, true, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");
        if (current.OperationalStatus == WaybillOperationalStatuses.ReadyForApproval)
        {
            await tx.CommitAsync(cancellationToken);
            return current;
        }
        EnsureState(current, WaybillOperationalStatuses.Draft);
        EnsureVersion(current.Version, expected);
        var issues = ValidateForApproval(current);
        if (issues.Any(x => x.Blocking))
            throw new WaybillValidationException(issues);

        var newVersion = NewVersionBytes();
        await UpdateStateAsync(connection, transaction, context, waybillId, WaybillOperationalStatuses.ReadyForApproval,
            null, expected, newVersion, cancellationToken);
        await AppendAuditWithinTransactionAsync("WaybillSubmitted", nameof(Waybill), waybillId, context,
            JsonSerializer.Serialize(new { Status = WaybillOperationalStatuses.Draft }),
            JsonSerializer.Serialize(new { Status = WaybillOperationalStatuses.ReadyForApproval }), null, cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return await GetWaybillAsync(context, waybillId, cancellationToken);
    }

    public async Task<WaybillResponse> ReturnWaybillForCorrectionAsync(
        OperationContext context,
        Guid waybillId,
        ReturnWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new WaybillCoreRuleException("REASON_REQUIRED");
        context.EnsureComplete();
        var expected = ParseVersion(request.ExpectedVersion);
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();
        var current = await LoadWaybillAsync(connection, transaction, context, waybillId, true, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");
        if (current.OperationalStatus == WaybillOperationalStatuses.Draft)
        {
            await tx.CommitAsync(cancellationToken);
            return current;
        }
        EnsureState(current, WaybillOperationalStatuses.ReadyForApproval);
        EnsureVersion(current.Version, expected);
        var newVersion = NewVersionBytes();
        await UpdateStateAsync(connection, transaction, context, waybillId, WaybillOperationalStatuses.Draft,
            request.Reason.Trim(), expected, newVersion, cancellationToken);
        await AppendAuditWithinTransactionAsync("WaybillReturned", nameof(Waybill), waybillId, context,
            JsonSerializer.Serialize(new { Status = current.OperationalStatus }),
            JsonSerializer.Serialize(new { Status = WaybillOperationalStatuses.Draft }), request.Reason.Trim(), cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return await GetWaybillAsync(context, waybillId, cancellationToken);
    }

    public async Task<WaybillResponse> CancelWaybillAsync(
        OperationContext context,
        Guid waybillId,
        CancelWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new WaybillCoreRuleException("REASON_REQUIRED");
        context.EnsureComplete();
        var expected = ParseVersion(request.ExpectedVersion);
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();
        var current = await LoadWaybillAsync(connection, transaction, context, waybillId, true, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");
        if (current.OperationalStatus == WaybillOperationalStatuses.Cancelled)
        {
            await tx.CommitAsync(cancellationToken);
            return current;
        }
        if (current.OperationalStatus is not (WaybillOperationalStatuses.Draft or WaybillOperationalStatuses.ReadyForApproval or WaybillOperationalStatuses.Approved))
            throw new WaybillCoreRuleException("CANCEL_BLOCKED");
        EnsureVersion(current.Version, expected);

        var newVersion = NewVersionBytes();
        var now = DateTimeOffset.UtcNow;
        await using (var command = new NpgsqlCommand("""
            UPDATE transport_erp.waybills
            SET "OperationalStatus"='CANCELLED',"LastReason"=@reason,"CancelledAt"=@now,"UpdatedAt"=@now,"RowVersion"=@newVersion
            WHERE "Id"=@id AND "CompanyId"=@companyId AND "BranchId"=@branchId AND "RowVersion"=@expectedVersion
            """, connection, transaction))
        {
            Add(command, "reason", request.Reason.Trim()); Add(command, "now", now); Add(command, "newVersion", newVersion);
            Add(command, "id", waybillId); Add(command, "companyId", context.CompanyId); Add(command, "branchId", context.BranchId); Add(command, "expectedVersion", expected);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1) throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT");
        }
        await AppendAuditWithinTransactionAsync("WaybillCancelled", nameof(Waybill), waybillId, context,
            JsonSerializer.Serialize(new { Status = current.OperationalStatus, current.WaybillNo }),
            JsonSerializer.Serialize(new { Status = WaybillOperationalStatuses.Cancelled, current.WaybillNo }),
            request.Reason.Trim(), cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return await GetWaybillAsync(context, waybillId, cancellationToken);
    }

    public async Task<ApprovedWaybillResponse> ApproveWaybillAsync(
        OperationContext context,
        Guid waybillId,
        ApproveWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        if (request.SequenceId == Guid.Empty) throw new WaybillCoreRuleException("NUMBERING_UNAVAILABLE");
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey)) throw new WaybillCoreRuleException("IDEMPOTENCY_KEY_REQUIRED");
        var expected = ParseVersion(request.ExpectedVersion);

        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        var transaction = (NpgsqlTransaction)tx.GetDbTransaction();
        var current = await LoadWaybillAsync(connection, transaction, context, waybillId, true, cancellationToken)
            ?? throw new WaybillCoreRuleException("NOT_FOUND");

        if (current.OperationalStatus == WaybillOperationalStatuses.Approved)
        {
            var committed = await FindCommittedReservationAsync(connection, transaction, context.CompanyId, waybillId, cancellationToken);
            if (committed is null || !string.Equals(committed.IdempotencyKey, request.IdempotencyKey.Trim(), StringComparison.Ordinal))
                throw new WaybillCoreRuleException("INVALID_STATE");
            await tx.CommitAsync(cancellationToken);
            return ToApproved(current, committed.ReservationId);
        }

        EnsureState(current, WaybillOperationalStatuses.ReadyForApproval);
        EnsureVersion(current.Version, expected);
        var issues = ValidateForApproval(current);
        if (issues.Any(x => x.Blocking)) throw new WaybillValidationException(issues);

        var reserved = await numbering.ReserveAsync(context,
            new NumberReservationRequest(request.SequenceId, request.IdempotencyKey.Trim(), "WAYBILL_APPROVAL"), cancellationToken);
        await numbering.LinkToWaybillAsync(context, reserved.Id, waybillId, request.IdempotencyKey.Trim(), cancellationToken);

        var newVersion = NewVersionBytes();
        var now = DateTimeOffset.UtcNow;
        await using (var command = new NpgsqlCommand("""
            UPDATE transport_erp.waybills
            SET "WaybillNo"=@waybillNo,"OperationalStatus"='APPROVED',"ApprovedBy"=@approvedBy,"ApprovedAt"=@now,
                "UpdatedAt"=@now,"RowVersion"=@newVersion
            WHERE "Id"=@id AND "CompanyId"=@companyId AND "BranchId"=@branchId AND "OperationalStatus"='READY_FOR_APPROVAL' AND "RowVersion"=@expectedVersion
            """, connection, transaction))
        {
            Add(command, "waybillNo", reserved.RenderedNumber); Add(command, "approvedBy", context.UserId); Add(command, "now", now);
            Add(command, "newVersion", newVersion); Add(command, "id", waybillId); Add(command, "companyId", context.CompanyId);
            Add(command, "branchId", context.BranchId); Add(command, "expectedVersion", expected);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1) throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT");
        }

        var committedReservation = await numbering.CommitAsync(context,
            new NumberReservationTransitionRequest(reserved.Id, request.IdempotencyKey.Trim(), "WAYBILL_APPROVAL_COMMIT"), cancellationToken);
        await AppendAuditWithinTransactionAsync("WaybillApproved", nameof(Waybill), waybillId, context,
            JsonSerializer.Serialize(new { Status = current.OperationalStatus, WaybillNo = (string?)null }),
            JsonSerializer.Serialize(new { Status = WaybillOperationalStatuses.Approved, WaybillNo = committedReservation.RenderedNumber, ReservationId = committedReservation.Id }),
            null, cancellationToken);
        await tx.CommitAsync(cancellationToken);

        var approved = await GetWaybillAsync(context, waybillId, cancellationToken);
        return ToApproved(approved, committedReservation.Id);
    }

    private async Task InsertWaybillHeaderAsync(
        NpgsqlConnection connection, NpgsqlTransaction transaction, Guid id, OperationContext context,
        string draftNo, CreateWaybillDraftRequest request, decimal netAmount, byte[] rowVersion, DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            INSERT INTO transport_erp.waybills
            ("Id","CompanyId","BranchId","DraftNo","WaybillNo","CreateOperationId","ServicePointId","AgentId","CreatedBy","ApprovedBy",
             "WaybillDateTime","RequestDateTime","ExpectedArrivalAt","ServiceType","Priority","OriginId","DestinationId","CurrencyId",
             "ExchangeRate","FreightTotal","DiscountTotal","NetAmount","OperationalStatus","FinancialStatus","LastReason","ApprovedAt","CancelledAt",
             "CreatedAt","UpdatedAt","RowVersion")
            VALUES
            (@id,@companyId,@branchId,@draftNo,NULL,@createOperationId,@servicePointId,@agentId,@createdBy,NULL,
             @waybillDateTime,@requestDateTime,@expectedArrivalAt,@serviceType,@priority,@originId,@destinationId,@currencyId,
             @exchangeRate,@freightTotal,@discountTotal,@netAmount,'DRAFT','UNPAID',NULL,NULL,NULL,@now,@now,@rowVersion)
            """, connection, transaction);
        Add(command, "id", id); Add(command, "companyId", context.CompanyId); Add(command, "branchId", context.BranchId);
        Add(command, "draftNo", draftNo); Add(command, "createOperationId", request.ClientOperationId.Trim());
        Add(command, "createdBy", context.UserId); AddHeaderParameters(command, request.ServicePointId, request.AgentId,
            request.WaybillDateTime, request.RequestDateTime, request.ExpectedArrivalAt, request.ServiceType, request.Priority,
            request.OriginId, request.DestinationId, request.CurrencyId, request.ExchangeRate, request.FreightTotal, request.DiscountTotal, netAmount);
        Add(command, "now", now); Add(command, "rowVersion", rowVersion);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task ReplacePartiesAsync(
        NpgsqlConnection connection, NpgsqlTransaction transaction, Guid waybillId,
        IReadOnlyList<WaybillPartyInput> parties, CancellationToken cancellationToken)
    {
        await using (var delete = new NpgsqlCommand("DELETE FROM transport_erp.waybill_parties WHERE \"WaybillId\"=@waybillId", connection, transaction))
        {
            Add(delete, "waybillId", waybillId);
            await delete.ExecuteNonQueryAsync(cancellationToken);
        }
        foreach (var party in parties)
        {
            await using var command = new NpgsqlCommand("""
                INSERT INTO transport_erp.waybill_parties
                ("Id","WaybillId","Role","OperationalPartyId","NameSnapshot","MobileSnapshot","IdentityTypeSnapshot","IdentityNoSnapshot",
                 "CountryId","GovernorateId","DirectorateId","CityId","AreaId","AddressLine")
                VALUES (@id,@waybillId,@role,@partyId,@name,@mobile,@identityType,@identityNo,@countryId,@governorateId,@directorateId,@cityId,@areaId,@addressLine)
                """, connection, transaction);
            Add(command, "id", Guid.NewGuid()); Add(command, "waybillId", waybillId); Add(command, "role", party.Role.Trim().ToUpperInvariant());
            Add(command, "partyId", party.OperationalPartyId); Add(command, "name", party.Name.Trim()); Add(command, "mobile", party.Mobile.Trim());
            Add(command, "identityType", NullIfWhiteSpace(party.IdentityType)); Add(command, "identityNo", NullIfWhiteSpace(party.IdentityNo));
            AddAddress(command, party.Address);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private async Task ReplaceItemsAsync(
        NpgsqlConnection connection, NpgsqlTransaction transaction, Guid waybillId,
        IReadOnlyList<WaybillItemInput> items, CancellationToken cancellationToken)
    {
        await using (var delete = new NpgsqlCommand("DELETE FROM transport_erp.waybill_items WHERE \"WaybillId\"=@waybillId", connection, transaction))
        {
            Add(delete, "waybillId", waybillId);
            await delete.ExecuteNonQueryAsync(cancellationToken);
        }
        var now = DateTimeOffset.UtcNow;
        foreach (var item in items)
        {
            await using var command = new NpgsqlCommand("""
                INSERT INTO transport_erp.waybill_items
                ("Id","WaybillId","LineNo","ItemCode","ItemType","Contents","Quantity","Pieces","Weight","Length","Width","Height","Volume",
                 "DeclaredValue","OriginCountryId","ItemFreight","RiskFlagsJson","Notes","CreatedAt","UpdatedAt","RowVersion")
                VALUES (@id,@waybillId,@lineNo,@itemCode,@itemType,@contents,@quantity,@pieces,@weight,@length,@width,@height,@volume,
                        @declaredValue,@originCountryId,@itemFreight,CAST(@riskFlags AS jsonb),@notes,@now,@now,@rowVersion)
                """, connection, transaction);
            Add(command, "id", Guid.NewGuid()); Add(command, "waybillId", waybillId); Add(command, "lineNo", item.LineNo);
            Add(command, "itemCode", NullIfWhiteSpace(item.ItemCode)); Add(command, "itemType", item.ItemType.Trim()); Add(command, "contents", item.Contents.Trim());
            Add(command, "quantity", item.Quantity); Add(command, "pieces", item.Pieces); Add(command, "weight", item.Weight); Add(command, "length", item.Length);
            Add(command, "width", item.Width); Add(command, "height", item.Height); Add(command, "volume", item.Volume); Add(command, "declaredValue", item.DeclaredValue);
            Add(command, "originCountryId", item.OriginCountryId); Add(command, "itemFreight", item.ItemFreight); Add(command, "riskFlags", item.RiskFlagsJson ?? "{}");
            Add(command, "notes", NullIfWhiteSpace(item.Notes)); Add(command, "now", now); Add(command, "rowVersion", NewVersionBytes());
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private async Task<WaybillResponse?> LoadWaybillAsync(
        NpgsqlConnection connection, NpgsqlTransaction? transaction, OperationContext context, Guid waybillId,
        bool forUpdate, CancellationToken cancellationToken)
    {
        var sql = """
            SELECT "Id","DraftNo","WaybillNo","CompanyId","BranchId","ServicePointId","AgentId","WaybillDateTime","RequestDateTime",
                   "ExpectedArrivalAt","ServiceType","Priority","OriginId","DestinationId","CurrencyId","ExchangeRate","FreightTotal","DiscountTotal",
                   "NetAmount","OperationalStatus","FinancialStatus","RowVersion"
            FROM transport_erp.waybills
            WHERE "Id"=@id AND "CompanyId"=@companyId AND "BranchId"=@branchId
            """ + (forUpdate ? " FOR UPDATE" : string.Empty);
        await using var command = Command(sql, connection, transaction);
        Add(command, "id", waybillId); Add(command, "companyId", context.CompanyId); Add(command, "branchId", context.BranchId);
        WaybillHeader? header = null;
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            if (await reader.ReadAsync(cancellationToken))
            {
                header = new WaybillHeader(
                    reader.GetGuid(0), reader.GetString(1), reader.IsDBNull(2) ? null : reader.GetString(2), reader.GetGuid(3), reader.GetGuid(4),
                    reader.IsDBNull(5) ? null : reader.GetGuid(5), reader.IsDBNull(6) ? null : reader.GetGuid(6), reader.GetFieldValue<DateTimeOffset>(7),
                    reader.IsDBNull(8) ? null : reader.GetFieldValue<DateTimeOffset>(8), reader.IsDBNull(9) ? null : reader.GetFieldValue<DateTimeOffset>(9),
                    reader.GetString(10), reader.GetString(11), reader.GetGuid(12), reader.GetGuid(13), reader.GetGuid(14), reader.GetDecimal(15),
                    reader.GetDecimal(16), reader.GetDecimal(17), reader.GetDecimal(18), reader.GetString(19), reader.GetString(20), (byte[])reader[21]);
            }
        }
        if (header is null) return null;

        var parties = new List<WaybillPartyResponse>();
        await using (var partyCommand = Command("""
            SELECT "Id","Role","OperationalPartyId","NameSnapshot","MobileSnapshot","IdentityTypeSnapshot","IdentityNoSnapshot",
                   "CountryId","GovernorateId","DirectorateId","CityId","AreaId","AddressLine"
            FROM transport_erp.waybill_parties WHERE "WaybillId"=@waybillId ORDER BY "Role"
            """, connection, transaction))
        {
            Add(partyCommand, "waybillId", waybillId);
            await using var reader = await partyCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
                parties.Add(new WaybillPartyResponse(reader.GetGuid(0), reader.GetString(1), reader.IsDBNull(2) ? null : reader.GetGuid(2),
                    reader.GetString(3), reader.GetString(4), reader.IsDBNull(5) ? null : reader.GetString(5), reader.IsDBNull(6) ? null : reader.GetString(6),
                    ReadAddress(reader, 7)));
        }

        var items = new List<WaybillItemResponse>();
        await using (var itemCommand = Command("""
            SELECT "Id","LineNo","ItemCode","ItemType","Contents","Quantity","Pieces","Weight","Length","Width","Height","Volume",
                   "DeclaredValue","OriginCountryId","ItemFreight","RiskFlagsJson"::text,"Notes"
            FROM transport_erp.waybill_items WHERE "WaybillId"=@waybillId ORDER BY "LineNo"
            """, connection, transaction))
        {
            Add(itemCommand, "waybillId", waybillId);
            await using var reader = await itemCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
                items.Add(new WaybillItemResponse(reader.GetGuid(0), reader.GetInt32(1), reader.IsDBNull(2) ? null : reader.GetString(2),
                    reader.GetString(3), reader.GetString(4), reader.GetDecimal(5), reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    DecimalOrNull(reader, 7), DecimalOrNull(reader, 8), DecimalOrNull(reader, 9), DecimalOrNull(reader, 10), DecimalOrNull(reader, 11),
                    DecimalOrNull(reader, 12), reader.IsDBNull(13) ? null : reader.GetGuid(13), DecimalOrNull(reader, 14),
                    reader.IsDBNull(15) ? null : reader.GetString(15), reader.IsDBNull(16) ? null : reader.GetString(16)));
        }

        return new WaybillResponse(header.Id, header.DraftNo, header.WaybillNo, header.CompanyId, header.BranchId, header.ServicePointId,
            header.AgentId, header.WaybillDateTime, header.RequestDateTime, header.ExpectedArrivalAt, header.ServiceType, header.Priority,
            header.OriginId, header.DestinationId, header.CurrencyId, header.ExchangeRate, header.FreightTotal, header.DiscountTotal, header.NetAmount,
            header.OperationalStatus, header.FinancialStatus, parties, items, Version(header.RowVersion));
    }

    private static IReadOnlyList<WaybillValidationIssue> ValidateForApproval(WaybillResponse waybill)
    {
        var issues = new List<WaybillValidationIssue>();
        if (waybill.CompanyId == Guid.Empty) issues.Add(Block("COMPANY_REQUIRED", "CompanyId", "Company is required."));
        if (waybill.BranchId == Guid.Empty) issues.Add(Block("BRANCH_REQUIRED", "BranchId", "Branch is required."));
        if (waybill.OriginId == Guid.Empty) issues.Add(Block("ORIGIN_REQUIRED", "OriginId", "Origin is required."));
        if (waybill.DestinationId == Guid.Empty) issues.Add(Block("DESTINATION_REQUIRED", "DestinationId", "Destination is required."));
        if (waybill.CurrencyId == Guid.Empty) issues.Add(Block("CURRENCY_REQUIRED", "CurrencyId", "Currency is required."));
        if (waybill.ExchangeRate <= 0m) issues.Add(Block("EXCHANGE_RATE_INVALID", "ExchangeRate", "Exchange rate must be positive."));
        if (waybill.FreightTotal < 0m || waybill.DiscountTotal < 0m || waybill.NetAmount < 0m)
            issues.Add(Block("AMOUNT_INVALID", "NetAmount", "Amounts cannot be negative."));
        if (!waybill.Parties.Any(x => x.Role == "SENDER")) issues.Add(Block("SENDER_REQUIRED", "Parties", "Sender is required."));
        if (!waybill.Parties.Any(x => x.Role == "RECEIVER")) issues.Add(Block("RECEIVER_REQUIRED", "Parties", "Receiver is required."));
        if (waybill.Items.Count == 0) issues.Add(Block("ITEM_REQUIRED", "Items", "At least one item is required."));
        foreach (var item in waybill.Items)
        {
            if (item.Quantity <= 0m) issues.Add(Block("QUANTITY_INVALID", $"Items[{item.LineNo}].Quantity", "Quantity must be positive."));
            if (Flag(item.RiskFlagsJson, "Prohibited")) issues.Add(Block("PROHIBITED_ITEM", $"Items[{item.LineNo}].RiskFlags", "Prohibited item blocks approval."));
            if (Flag(item.RiskFlagsJson, "PermitRequired")) issues.Add(Block("PERMIT_REQUIRED", $"Items[{item.LineNo}].RiskFlags", "Permit-required item cannot be approved in P2-C01-A without the later governed permit workflow."));
            if (Flag(item.RiskFlagsJson, "CustomsRequired")) issues.Add(new WaybillValidationIssue("CUSTOMS_REQUIRED", $"Items[{item.LineNo}].RiskFlags", "Customs readiness will be handled by the governed CUS boundary.", false));
        }
        return issues;
    }

    private async Task AppendAuditWithinTransactionAsync(
        string action, string entityType, Guid entityId, OperationContext context,
        string? beforeJson, string? afterJson, string? reason, CancellationToken cancellationToken)
    {
        var previousHash = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash)
            .FirstOrDefaultAsync(cancellationToken);
        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.UserId, CompanyId = context.CompanyId,
            BranchId = context.BranchId, Action = action, Outcome = "SUCCESS", EntityType = entityType, EntityId = entityId,
            CorrelationId = context.CorrelationId, BeforeJson = beforeJson, AfterJson = afterJson, Reason = reason, PreviousHash = previousHash
        };
        audit.Hash = AuditEventService.ComputeHash(audit);
        db.AuditEvents.Add(audit);
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateStateAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, OperationContext context,
        Guid waybillId, string status, string? reason, byte[] expectedVersion, byte[] newVersion, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            UPDATE transport_erp.waybills SET "OperationalStatus"=@status,"LastReason"=@reason,"UpdatedAt"=@now,"RowVersion"=@newVersion
            WHERE "Id"=@id AND "CompanyId"=@companyId AND "BranchId"=@branchId AND "RowVersion"=@expectedVersion
            """, connection, transaction);
        Add(command, "status", status); Add(command, "reason", NullIfWhiteSpace(reason)); Add(command, "now", DateTimeOffset.UtcNow);
        Add(command, "newVersion", newVersion); Add(command, "id", waybillId); Add(command, "companyId", context.CompanyId);
        Add(command, "branchId", context.BranchId); Add(command, "expectedVersion", expectedVersion);
        if (await command.ExecuteNonQueryAsync(cancellationToken) != 1) throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT");
    }

    private async Task<Guid?> FindWaybillIdByCreateOperationAsync(NpgsqlConnection connection, NpgsqlTransaction transaction,
        Guid companyId, string operationId, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("SELECT \"Id\" FROM transport_erp.waybills WHERE \"CompanyId\"=@companyId AND \"CreateOperationId\"=@operationId", connection, transaction);
        Add(command, "companyId", companyId); Add(command, "operationId", operationId);
        var value = await command.ExecuteScalarAsync(cancellationToken);
        return value is Guid id ? id : null;
    }

    private async Task<OperationalPartyResponse?> FindOperationalPartyByOperationAsync(NpgsqlConnection connection, NpgsqlTransaction transaction,
        OperationContext context, string operationId, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            SELECT "Id","PartyNo","Name","Mobile","IdentityType","IdentityNo","CountryId","GovernorateId","DirectorateId","CityId","AreaId","AddressLine","Status","RowVersion"
            FROM transport_erp.operational_parties WHERE "CompanyId"=@companyId AND "ClientOperationId"=@operationId
            """, connection, transaction);
        Add(command, "companyId", context.CompanyId); Add(command, "operationId", operationId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadOperationalParty(reader) : null;
    }

    private async Task<Guid?> FindOperationalPartyDuplicateAsync(NpgsqlConnection connection, NpgsqlTransaction transaction,
        OperationContext context, string mobile, string? identityNo, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            SELECT "Id" FROM transport_erp.operational_parties
            WHERE "CompanyId"=@companyId AND "Status"='ACTIVE'
              AND ((@identityNo IS NOT NULL AND "IdentityNo"=@identityNo) OR "Mobile"=@mobile)
            LIMIT 1
            """, connection, transaction);
        Add(command, "companyId", context.CompanyId); Add(command, "identityNo", NullIfWhiteSpace(identityNo)); Add(command, "mobile", mobile);
        var value = await command.ExecuteScalarAsync(cancellationToken);
        return value is Guid id ? id : null;
    }

    private async Task<CommittedReservation?> FindCommittedReservationAsync(NpgsqlConnection connection, NpgsqlTransaction transaction,
        Guid companyId, Guid waybillId, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            SELECT "Id","IdempotencyKey" FROM transport_erp.number_reservations
            WHERE "CompanyId"=@companyId AND "WaybillId"=@waybillId AND "State"='COMMITTED'
            """, connection, transaction);
        Add(command, "companyId", companyId); Add(command, "waybillId", waybillId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? new CommittedReservation(reader.GetGuid(0), reader.GetString(1)) : null;
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = (NpgsqlConnection)db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private static NpgsqlCommand Command(string sql, NpgsqlConnection connection, NpgsqlTransaction? transaction)
    {
        var command = new NpgsqlCommand(sql, connection);
        if (transaction is not null) command.Transaction = transaction;
        return command;
    }

    private static OperationalPartyResponse ReadOperationalParty(NpgsqlDataReader reader)
        => new(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
            reader.IsDBNull(4) ? null : reader.GetString(4), reader.IsDBNull(5) ? null : reader.GetString(5),
            ReadAddress(reader, 6), reader.GetString(12), Version((byte[])reader[13]));

    private static GeoAddressSnapshot ReadAddress(NpgsqlDataReader reader, int start)
        => new(reader.IsDBNull(start) ? null : reader.GetGuid(start), reader.IsDBNull(start + 1) ? null : reader.GetGuid(start + 1),
            reader.IsDBNull(start + 2) ? null : reader.GetGuid(start + 2), reader.IsDBNull(start + 3) ? null : reader.GetGuid(start + 3),
            reader.IsDBNull(start + 4) ? null : reader.GetGuid(start + 4), reader.IsDBNull(start + 5) ? null : reader.GetString(start + 5));

    private static decimal? DecimalOrNull(NpgsqlDataReader reader, int index) => reader.IsDBNull(index) ? null : reader.GetDecimal(index);

    private static void ValidateDraftHeader(Guid originId, Guid destinationId, Guid currencyId, decimal exchangeRate,
        decimal freightTotal, decimal discountTotal, string serviceType, string priority)
    {
        if (originId == Guid.Empty || destinationId == Guid.Empty || currencyId == Guid.Empty) throw new WaybillCoreRuleException("VALIDATION_ERROR");
        if (originId == destinationId) throw new WaybillCoreRuleException("ROUTE_INVALID");
        if (exchangeRate <= 0m || freightTotal < 0m || discountTotal < 0m || discountTotal > freightTotal) throw new WaybillCoreRuleException("AMOUNT_INVALID");
        if (string.IsNullOrWhiteSpace(serviceType) || string.IsNullOrWhiteSpace(priority)) throw new WaybillCoreRuleException("VALIDATION_ERROR");
    }

    private static void ValidateOperationalPartyInput(string name, string mobile, string? identityType, string? identityNo, GeoAddressSnapshot address)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(mobile)) throw new WaybillCoreRuleException("VALIDATION_ERROR");
        if (!string.IsNullOrWhiteSpace(identityNo) && string.IsNullOrWhiteSpace(identityType)) throw new WaybillCoreRuleException("IDENTITY_TYPE_REQUIRED");
        ValidateAddressIfPresent(address);
    }

    private static void ValidatePartyInputs(IReadOnlyList<WaybillPartyInput> parties, bool allowMissingSenderReceiver)
    {
        var roles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var party in parties)
        {
            var role = party.Role.Trim().ToUpperInvariant();
            if (role is not ("SENDER" or "RECEIVER" or "PAYER")) throw new WaybillCoreRuleException("PARTY_ROLE_INVALID");
            if (!roles.Add(role)) throw new WaybillCoreRuleException("PARTY_ROLE_DUPLICATE");
            ValidateOperationalPartyInput(party.Name, party.Mobile, party.IdentityType, party.IdentityNo, party.Address);
        }
        if (!allowMissingSenderReceiver && (!roles.Contains("SENDER") || !roles.Contains("RECEIVER"))) throw new WaybillCoreRuleException("PARTY_REQUIRED");
    }

    private static void ValidateItemInputs(IReadOnlyList<WaybillItemInput> items, bool allowEmpty)
    {
        if (!allowEmpty && items.Count == 0) throw new WaybillCoreRuleException("ITEM_REQUIRED");
        var lines = new HashSet<int>();
        foreach (var item in items)
        {
            if (item.LineNo <= 0 || !lines.Add(item.LineNo) || string.IsNullOrWhiteSpace(item.ItemType) || string.IsNullOrWhiteSpace(item.Contents) || item.Quantity <= 0m)
                throw new WaybillCoreRuleException("VALIDATION_ERROR");
            foreach (var value in new[] { item.Weight, item.Length, item.Width, item.Height, item.Volume, item.DeclaredValue, item.ItemFreight })
                if (value.HasValue && value.Value < 0m) throw new WaybillCoreRuleException("VALIDATION_ERROR");
            if (!string.IsNullOrWhiteSpace(item.RiskFlagsJson))
            {
                try { using var _ = JsonDocument.Parse(item.RiskFlagsJson); }
                catch (JsonException) { throw new WaybillCoreRuleException("RISK_FLAGS_INVALID"); }
            }
        }
    }

    private static void ValidateAddressIfPresent(GeoAddressSnapshot address)
    {
        if (address.HasStructuredLocation || !string.IsNullOrWhiteSpace(address.AddressLine)) address.EnsureUsable();
    }

    private static bool Flag(string? json, string name)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Object) return false;
            foreach (var p in doc.RootElement.EnumerateObject())
                if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind is JsonValueKind.True or JsonValueKind.False)
                    return p.Value.GetBoolean();
        }
        catch (JsonException) { }
        return false;
    }

    private static WaybillValidationIssue Block(string code, string field, string message) => new(code, field, message, true);
    private static void EnsureState(WaybillResponse current, string expected)
    {
        if (!string.Equals(current.OperationalStatus, expected, StringComparison.Ordinal)) throw new WaybillCoreRuleException("INVALID_STATE");
    }
    private static void EnsureVersion(string current, byte[] expected)
    {
        if (!Convert.FromBase64String(current).SequenceEqual(expected)) throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT");
    }
    private static byte[] ParseVersion(string value)
    {
        try { return Convert.FromBase64String(value); }
        catch (FormatException) { throw new WaybillCoreRuleException("CONCURRENCY_CONFLICT"); }
    }
    private static byte[] NewVersionBytes() => Guid.NewGuid().ToByteArray();
    private static string Version(byte[] value) => Convert.ToBase64String(value);
    private static string? NullIfWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static void Add(NpgsqlCommand command, string name, object? value) => command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    private static void AddAddress(NpgsqlCommand command, GeoAddressSnapshot address)
    {
        Add(command, "countryId", address.CountryId); Add(command, "governorateId", address.GovernorateId);
        Add(command, "directorateId", address.DirectorateId); Add(command, "cityId", address.CityId); Add(command, "areaId", address.AreaId);
        Add(command, "addressLine", NullIfWhiteSpace(address.AddressLine));
    }
    private static void AddHeaderParameters(NpgsqlCommand command, Guid? servicePointId, Guid? agentId, DateTimeOffset waybillDateTime,
        DateTimeOffset? requestDateTime, DateTimeOffset? expectedArrivalAt, string serviceType, string priority, Guid originId, Guid destinationId,
        Guid currencyId, decimal exchangeRate, decimal freightTotal, decimal discountTotal, decimal netAmount)
    {
        Add(command, "servicePointId", servicePointId); Add(command, "agentId", agentId); Add(command, "waybillDateTime", waybillDateTime);
        Add(command, "requestDateTime", requestDateTime); Add(command, "expectedArrivalAt", expectedArrivalAt); Add(command, "serviceType", serviceType.Trim());
        Add(command, "priority", priority.Trim().ToUpperInvariant()); Add(command, "originId", originId); Add(command, "destinationId", destinationId);
        Add(command, "currencyId", currencyId); Add(command, "exchangeRate", exchangeRate); Add(command, "freightTotal", freightTotal);
        Add(command, "discountTotal", discountTotal); Add(command, "netAmount", netAmount);
    }

    private static WaybillDraftResponse ToDraft(WaybillResponse x) => new(x.Id, x.DraftNo, x.WaybillNo, x.CompanyId, x.BranchId,
        x.ServicePointId, x.AgentId, x.WaybillDateTime, x.RequestDateTime, x.ExpectedArrivalAt, x.ServiceType, x.Priority, x.OriginId,
        x.DestinationId, x.CurrencyId, x.ExchangeRate, x.FreightTotal, x.DiscountTotal, x.NetAmount, x.OperationalStatus, x.FinancialStatus,
        x.Parties, x.Items, x.Version);
    private static ApprovedWaybillResponse ToApproved(WaybillResponse x, Guid reservationId) => new(x.Id, x.DraftNo, x.WaybillNo,
        x.CompanyId, x.BranchId, x.ServicePointId, x.AgentId, x.WaybillDateTime, x.RequestDateTime, x.ExpectedArrivalAt, x.ServiceType, x.Priority,
        x.OriginId, x.DestinationId, x.CurrencyId, x.ExchangeRate, x.FreightTotal, x.DiscountTotal, x.NetAmount, x.OperationalStatus,
        x.FinancialStatus, x.Parties, x.Items, x.Version, reservationId);

    private sealed record WaybillHeader(Guid Id, string DraftNo, string? WaybillNo, Guid CompanyId, Guid BranchId, Guid? ServicePointId,
        Guid? AgentId, DateTimeOffset WaybillDateTime, DateTimeOffset? RequestDateTime, DateTimeOffset? ExpectedArrivalAt, string ServiceType,
        string Priority, Guid OriginId, Guid DestinationId, Guid CurrencyId, decimal ExchangeRate, decimal FreightTotal, decimal DiscountTotal,
        decimal NetAmount, string OperationalStatus, string FinancialStatus, byte[] RowVersion);
    private sealed record CommittedReservation(Guid ReservationId, string IdempotencyKey);
}

public sealed class WaybillCoreRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class WaybillValidationException(IReadOnlyList<WaybillValidationIssue> issues) : InvalidOperationException("VALIDATION_ERROR")
{
    public string Code => "VALIDATION_ERROR";
    public IReadOnlyList<WaybillValidationIssue> Issues { get; } = issues;
}
