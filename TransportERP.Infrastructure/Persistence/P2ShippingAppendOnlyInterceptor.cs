using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using TransportERP.Domain.Waybills;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// P2-C01-C accepted quantity and movement ledger rows are append-only.
/// Corrections are represented by dedicated reversal rows/events rather than UPDATE/DELETE.
/// Manifest-line physical measures are normalized from the authoritative WaybillItem line snapshot
/// so a split allocation cannot duplicate the full line weight/volume on every trip.
/// </summary>
public sealed class P2ShippingAppendOnlyInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        RejectMutation(eventData.Context);
        NormalizeAddedManifestLinesAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        RejectMutation(eventData.Context);
        await NormalizeAddedManifestLinesAsync(eventData.Context, cancellationToken);
        return result;
    }

    private static void RejectMutation(DbContext? context)
    {
        if (context is null) return;
        Reject<ItemReleaseEntity>(context, "ItemRelease");
        Reject<TripAllocationEntity>(context, "TripAllocation");
        Reject<MovementEventEntity>(context, "MovementEvent");
    }

    private static void Reject<TEntity>(DbContext context, string label) where TEntity : class
    {
        var ids = context.ChangeTracker.Entries<TEntity>()
            .Where(x => x.State is EntityState.Modified or EntityState.Deleted)
            .Select(x => x.Property("Id").CurrentValue?.ToString() ?? "unknown")
            .ToArray();
        if (ids.Length > 0)
            throw new InvalidOperationException($"{label} is append-only; mutation denied: {string.Join(',', ids)}");
    }

    private static async Task NormalizeAddedManifestLinesAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null) return;
        var lines = context.ChangeTracker.Entries<ManifestLineEntity>()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .ToArray();
        if (lines.Length == 0) return;

        var connectionString = context.Database.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Manifest-line physical measure normalization requires a configured PostgreSQL connection.");

        var itemIds = lines.Select(x => x.WaybillItemId).Distinct().ToArray();
        var items = new Dictionary<Guid, ItemMeasureSnapshot>();

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT "Id", "Quantity", "Weight", "Length", "Width", "Height"
            FROM transport_erp.waybill_items
            WHERE "Id" = ANY(@ids)
            """;
        command.Parameters.AddWithValue("ids", itemIds);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            items[reader.GetGuid(0)] = new ItemMeasureSnapshot(
                reader.GetDecimal(1),
                reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                reader.IsDBNull(5) ? null : reader.GetDecimal(5));
        }

        foreach (var line in lines)
        {
            if (!items.TryGetValue(line.WaybillItemId, out var item))
                throw new InvalidOperationException($"WaybillItem {line.WaybillItemId} is required to normalize ManifestLine physical measures.");

            var (weight, volume) = ShippingExecutionRules.AllocatePhysicalMeasures(
                item.Quantity, line.Quantity, item.Weight, item.Length, item.Width, item.Height);
            line.Weight = weight;
            line.Volume = volume;
        }
    }

    private sealed record ItemMeasureSnapshot(
        decimal Quantity,
        decimal? Weight,
        decimal? Length,
        decimal? Width,
        decimal? Height);
}
