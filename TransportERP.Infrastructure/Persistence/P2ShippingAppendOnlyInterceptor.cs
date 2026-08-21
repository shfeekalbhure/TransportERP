using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// P2-C01-C accepted quantity and movement ledger rows are append-only.
/// Corrections are represented by dedicated reversal rows/events rather than UPDATE/DELETE.
/// </summary>
public sealed class P2ShippingAppendOnlyInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        RejectMutation(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        RejectMutation(eventData.Context);
        return ValueTask.FromResult(result);
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
}
