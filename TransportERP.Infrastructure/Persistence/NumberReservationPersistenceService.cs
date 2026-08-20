using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Numbering;

namespace TransportERP.Infrastructure.Persistence;

public sealed class NumberReservationPersistenceService(TransportErpDbContext db) : INumberReservationService
{
    public async ValueTask<NumberReservationDto> ReserveAsync(
        OperationContext context,
        NumberReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        request.EnsureValid();
        return await InTransactionAsync(async (connection, transaction) =>
        {
            var existing = await FindByIdempotencyAsync(connection, transaction, context.CompanyId, request.IdempotencyKey, cancellationToken);
            if (existing is not null)
            {
                if (existing.SequenceId != request.SequenceId)
                    throw new NumberingRuleException("IDEMPOTENCY_CONFLICT");
                return existing.ToDto();
            }

            await using var sequenceCommand = new NpgsqlCommand("""
                SELECT "NextValue", "Prefix", "BranchId", "Status"
                FROM transport_erp.number_sequences
                WHERE "Id" = @sequenceId AND "CompanyId" = @companyId
                FOR UPDATE
                """, connection, transaction);
            sequenceCommand.Parameters.AddWithValue("sequenceId", request.SequenceId);
            sequenceCommand.Parameters.AddWithValue("companyId", context.CompanyId);
            await using var reader = await sequenceCommand.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
                throw new NumberingRuleException("NUMBERING_UNAVAILABLE");
            var nextDecimal = reader.GetDecimal(0);
            var prefix = reader.IsDBNull(1) ? null : reader.GetString(1);
            var branchId = reader.IsDBNull(2) ? (Guid?)null : reader.GetGuid(2);
            var status = reader.GetString(3);
            await reader.CloseAsync();

            if (!string.Equals(status, "ACTIVE", StringComparison.Ordinal))
                throw new NumberingRuleException("NUMBERING_UNAVAILABLE");
            if (branchId.HasValue && branchId.Value != context.BranchId)
                throw new NumberingRuleException("SCOPE_DENIED");
            if (nextDecimal < 1 || nextDecimal > ulong.MaxValue)
                throw new NumberingRuleException("NUMBERING_UNAVAILABLE");

            var next = decimal.ToUInt64(nextDecimal);
            var rendered = string.IsNullOrWhiteSpace(prefix) ? next.ToString() : $"{prefix}{next}";
            var now = DateTimeOffset.UtcNow;
            var rowVersion = Guid.NewGuid().ToByteArray();

            await using (var update = new NpgsqlCommand("""
                UPDATE transport_erp.number_sequences
                SET "NextValue" = @nextValue, "UpdatedAt" = @now, "RowVersion" = @rowVersion
                WHERE "Id" = @sequenceId AND "CompanyId" = @companyId
                """, connection, transaction))
            {
                update.Parameters.AddWithValue("nextValue", nextDecimal + 1m);
                update.Parameters.AddWithValue("now", now);
                update.Parameters.AddWithValue("rowVersion", rowVersion);
                update.Parameters.AddWithValue("sequenceId", request.SequenceId);
                update.Parameters.AddWithValue("companyId", context.CompanyId);
                if (await update.ExecuteNonQueryAsync(cancellationToken) != 1)
                    throw new NumberingRuleException("NUMBERING_UNAVAILABLE");
            }

            var id = Guid.NewGuid();
            await using (var insert = new NpgsqlCommand("""
                INSERT INTO transport_erp.number_reservations
                ("Id","SequenceId","WaybillId","CompanyId","BranchId","IdempotencyKey","NumberValue","RenderedNumber",
                 "ReservedAt","CommittedAt","VoidedAt","VoidReason","State","CreatedAt","UpdatedAt","RowVersion")
                VALUES
                (@id,@sequenceId,NULL,@companyId,@branchId,@key,@numberValue,@rendered,
                 @now,NULL,NULL,NULL,'RESERVED',@now,@now,@rowVersion)
                """, connection, transaction))
            {
                insert.Parameters.AddWithValue("id", id);
                insert.Parameters.AddWithValue("sequenceId", request.SequenceId);
                insert.Parameters.AddWithValue("companyId", context.CompanyId);
                insert.Parameters.AddWithValue("branchId", context.BranchId);
                insert.Parameters.AddWithValue("key", request.IdempotencyKey.Trim());
                insert.Parameters.AddWithValue("numberValue", nextDecimal);
                insert.Parameters.AddWithValue("rendered", rendered);
                insert.Parameters.AddWithValue("now", now);
                insert.Parameters.AddWithValue("rowVersion", Guid.NewGuid().ToByteArray());
                await insert.ExecuteNonQueryAsync(cancellationToken);
            }

            return new NumberReservationDto(id, request.SequenceId, next, rendered, NumberReservationStates.Reserved);
        }, cancellationToken);
    }

    public async ValueTask<NumberReservationDto> CommitAsync(
        OperationContext context,
        NumberReservationTransitionRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        request.EnsureValid();
        return await InTransactionAsync(async (connection, transaction) =>
        {
            var current = await FindByIdAsync(connection, transaction, context.CompanyId, request.ReservationId, true, cancellationToken)
                ?? throw new NumberingRuleException("NUMBERING_RESERVATION_NOT_FOUND");
            EnsureTransitionKey(current, request.IdempotencyKey);
            if (current.State == NumberReservationStates.Committed) return current.ToDto();
            if (current.State == NumberReservationStates.Void) throw new NumberingRuleException("NUMBERING_RESERVATION_VOID");
            if (!current.WaybillId.HasValue) throw new NumberingRuleException("NUMBERING_DOCUMENT_LINK_REQUIRED");

            var now = DateTimeOffset.UtcNow;
            await using var command = new NpgsqlCommand("""
                UPDATE transport_erp.number_reservations
                SET "State"='COMMITTED', "CommittedAt"=@now, "UpdatedAt"=@now, "RowVersion"=@rowVersion
                WHERE "Id"=@id AND "CompanyId"=@companyId AND "State"='RESERVED'
                """, connection, transaction);
            command.Parameters.AddWithValue("now", now);
            command.Parameters.AddWithValue("rowVersion", Guid.NewGuid().ToByteArray());
            command.Parameters.AddWithValue("id", current.Id);
            command.Parameters.AddWithValue("companyId", context.CompanyId);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
                throw new NumberingRuleException("NUMBERING_STATE_CONFLICT");
            return current with { State = NumberReservationStates.Committed }.ToDto();
        }, cancellationToken);
    }

    public async ValueTask<NumberReservationDto> VoidAsync(
        OperationContext context,
        NumberReservationTransitionRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        request.EnsureValid();
        return await InTransactionAsync(async (connection, transaction) =>
        {
            var current = await FindByIdAsync(connection, transaction, context.CompanyId, request.ReservationId, true, cancellationToken)
                ?? throw new NumberingRuleException("NUMBERING_RESERVATION_NOT_FOUND");
            EnsureTransitionKey(current, request.IdempotencyKey);
            if (current.State == NumberReservationStates.Void) return current.ToDto();
            if (current.State == NumberReservationStates.Committed) throw new NumberingRuleException("NUMBERING_COMMITTED_CANNOT_VOID");
            var now = DateTimeOffset.UtcNow;
            await using var command = new NpgsqlCommand("""
                UPDATE transport_erp.number_reservations
                SET "State"='VOID', "VoidedAt"=@now, "VoidReason"=@reason, "UpdatedAt"=@now, "RowVersion"=@rowVersion
                WHERE "Id"=@id AND "CompanyId"=@companyId AND "State"='RESERVED'
                """, connection, transaction);
            command.Parameters.AddWithValue("now", now);
            command.Parameters.AddWithValue("reason", (object?)request.Reason ?? DBNull.Value);
            command.Parameters.AddWithValue("rowVersion", Guid.NewGuid().ToByteArray());
            command.Parameters.AddWithValue("id", current.Id);
            command.Parameters.AddWithValue("companyId", context.CompanyId);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
                throw new NumberingRuleException("NUMBERING_STATE_CONFLICT");
            return current with { State = NumberReservationStates.Void }.ToDto();
        }, cancellationToken);
    }

    public async Task LinkToWaybillAsync(
        OperationContext context,
        Guid reservationId,
        Guid waybillId,
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        if (reservationId == Guid.Empty || waybillId == Guid.Empty) throw new ArgumentException("Reservation and waybill are required.");
        await InTransactionAsync(async (connection, transaction) =>
        {
            var current = await FindByIdAsync(connection, transaction, context.CompanyId, reservationId, true, cancellationToken)
                ?? throw new NumberingRuleException("NUMBERING_RESERVATION_NOT_FOUND");
            EnsureTransitionKey(current, idempotencyKey);
            if (current.WaybillId.HasValue && current.WaybillId.Value != waybillId)
                throw new NumberingRuleException("IDEMPOTENCY_CONFLICT");
            if (current.State != NumberReservationStates.Reserved)
            {
                if (current.State == NumberReservationStates.Committed && current.WaybillId == waybillId) return 0;
                throw new NumberingRuleException("NUMBERING_STATE_CONFLICT");
            }
            await using var command = new NpgsqlCommand("""
                UPDATE transport_erp.number_reservations
                SET "WaybillId"=@waybillId, "UpdatedAt"=@now, "RowVersion"=@rowVersion
                WHERE "Id"=@id AND "CompanyId"=@companyId AND "State"='RESERVED'
                """, connection, transaction);
            command.Parameters.AddWithValue("waybillId", waybillId);
            command.Parameters.AddWithValue("now", DateTimeOffset.UtcNow);
            command.Parameters.AddWithValue("rowVersion", Guid.NewGuid().ToByteArray());
            command.Parameters.AddWithValue("id", reservationId);
            command.Parameters.AddWithValue("companyId", context.CompanyId);
            if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
                throw new NumberingRuleException("NUMBERING_STATE_CONFLICT");
            return 0;
        }, cancellationToken);
    }

    private async Task<T> InTransactionAsync<T>(Func<NpgsqlConnection, NpgsqlTransaction, Task<T>> work, CancellationToken cancellationToken)
    {
        var existing = db.Database.CurrentTransaction;
        if (existing is not null)
        {
            var connection = (NpgsqlConnection)db.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);
            return await work(connection, (NpgsqlTransaction)existing.GetDbTransaction());
        }

        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var connection = (NpgsqlConnection)db.Database.GetDbConnection();
            var result = await work(connection, (NpgsqlTransaction)tx.GetDbTransaction());
            await tx.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ReservationRow?> FindByIdempotencyAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid companyId, string key, CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand("""
            SELECT "Id","SequenceId","WaybillId","IdempotencyKey","NumberValue","RenderedNumber","State"
            FROM transport_erp.number_reservations
            WHERE "CompanyId"=@companyId AND "IdempotencyKey"=@key
            """, connection, transaction);
        command.Parameters.AddWithValue("companyId", companyId);
        command.Parameters.AddWithValue("key", key.Trim());
        return await ReadReservationAsync(command, cancellationToken);
    }

    private async Task<ReservationRow?> FindByIdAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, Guid companyId, Guid id, bool forUpdate, CancellationToken cancellationToken)
    {
        var sql = """
            SELECT "Id","SequenceId","WaybillId","IdempotencyKey","NumberValue","RenderedNumber","State"
            FROM transport_erp.number_reservations
            WHERE "CompanyId"=@companyId AND "Id"=@id
            """ + (forUpdate ? " FOR UPDATE" : string.Empty);
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("companyId", companyId);
        command.Parameters.AddWithValue("id", id);
        return await ReadReservationAsync(command, cancellationToken);
    }

    private static async Task<ReservationRow?> ReadReservationAsync(NpgsqlCommand command, CancellationToken cancellationToken)
    {
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken)) return null;
        return new ReservationRow(
            reader.GetGuid(0), reader.GetGuid(1), reader.IsDBNull(2) ? null : reader.GetGuid(2), reader.GetString(3),
            decimal.ToUInt64(reader.GetDecimal(4)), reader.GetString(5), reader.GetString(6));
    }

    private static void EnsureTransitionKey(ReservationRow row, string key)
    {
        if (!string.Equals(row.IdempotencyKey, key.Trim(), StringComparison.Ordinal))
            throw new NumberingRuleException("IDEMPOTENCY_CONFLICT");
    }

    private sealed record ReservationRow(Guid Id, Guid SequenceId, Guid? WaybillId, string IdempotencyKey, ulong NumberValue, string RenderedNumber, string State)
    {
        public NumberReservationDto ToDto() => new(Id, SequenceId, NumberValue, RenderedNumber, State);
    }
}

public sealed class NumberingRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
