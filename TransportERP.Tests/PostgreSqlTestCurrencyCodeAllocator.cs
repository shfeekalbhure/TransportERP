using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

internal static class PostgreSqlTestCurrencyCodeAllocator
{
    private const int CodeSpace = 36 * 36 * 36;
    private const long InitializerLockId = 8_104_202_608_260_001L;
    private const string SequenceName = "public.transport_erp_test_currency_code_seq";
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static async Task<string> NextAsync(
        TransportErpDbContext db,
        CancellationToken cancellationToken = default)
    {
        await EnsureSequenceAsync(db, cancellationToken);

        for (var attempt = 0; attempt < CodeSpace; attempt++)
        {
            var ordinal = await NextOrdinalAsync(db, cancellationToken);
            var code = ToBase36Code(ordinal % CodeSpace);
            if (!await db.Currencies.AsNoTracking().AnyAsync(x => x.Code == code, cancellationToken))
                return code;
        }

        throw new InvalidOperationException("The three-character test currency code space is exhausted.");
    }

    private static async Task EnsureSequenceAsync(TransportErpDbContext db, CancellationToken cancellationToken)
    {
        var connection = db.Database.GetDbConnection();
        var openedHere = connection.State != ConnectionState.Open;
        if (openedHere)
            await connection.OpenAsync(cancellationToken);

        DbTransaction? ownedTransaction = null;
        try
        {
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();
            if (transaction is null)
            {
                ownedTransaction = await connection.BeginTransactionAsync(cancellationToken);
                transaction = ownedTransaction;
            }

            try
            {
                await ExecuteScalarAsync(connection, transaction,
                    $"SELECT pg_advisory_xact_lock({InitializerLockId})", cancellationToken);
                await ExecuteNonQueryAsync(connection, transaction,
                    $"CREATE SEQUENCE IF NOT EXISTS {SequenceName} AS bigint MINVALUE 0 START WITH 0 NO MAXVALUE NO CYCLE",
                    cancellationToken);
                await ExecuteNonQueryAsync(connection, transaction,
                    $"ALTER SEQUENCE {SequenceName} NO MAXVALUE NO CYCLE", cancellationToken);

                if (ownedTransaction is not null)
                    await ownedTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                if (ownedTransaction?.Connection is not null)
                {
                    try
                    {
                        await ownedTransaction.RollbackAsync(CancellationToken.None);
                    }
                    catch
                    {
                        // Preserve the initialization failure; disposal/connection close still releases the xact lock.
                    }
                }
                throw;
            }
        }
        finally
        {
            if (ownedTransaction is not null)
                await ownedTransaction.DisposeAsync();
            if (openedHere)
                await connection.CloseAsync();
        }
    }

    private static async Task<long> NextOrdinalAsync(TransportErpDbContext db, CancellationToken cancellationToken)
    {
        var connection = db.Database.GetDbConnection();
        var openedHere = connection.State != ConnectionState.Open;
        if (openedHere)
            await connection.OpenAsync(cancellationToken);

        try
        {
            var transaction = db.Database.CurrentTransaction?.GetDbTransaction();
            var value = await ExecuteScalarAsync(connection, transaction,
                $"SELECT nextval('{SequenceName}')", cancellationToken);
            return Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        finally
        {
            if (openedHere)
                await connection.CloseAsync();
        }
    }

    private static async Task<object> ExecuteScalarAsync(
        DbConnection connection,
        DbTransaction? transaction,
        string commandText,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = commandText;
        return await command.ExecuteScalarAsync(cancellationToken)
            ?? throw new InvalidOperationException($"PostgreSQL returned no value for: {commandText}");
    }

    private static async Task ExecuteNonQueryAsync(
        DbConnection connection,
        DbTransaction transaction,
        string commandText,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = commandText;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string ToBase36Code(long value)
    {
        Span<char> result = stackalloc char[3];
        for (var index = result.Length - 1; index >= 0; index--)
        {
            result[index] = Alphabet[(int)(value % 36)];
            value /= 36;
        }

        return new string(result);
    }
}
