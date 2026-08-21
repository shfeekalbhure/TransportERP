using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Enforces the P2-C01-B append-only boundary in the production DbContext pipeline.
/// Accepted collections and their reversal rows are immutable after insertion; FinancialLink is append-only too.
/// Corrections must be represented by new reversal/link records rather than UPDATE or DELETE.
/// </summary>
public sealed class P2FinanceAppendOnlyInterceptor : SaveChangesInterceptor
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

        var collectionIds = context.ChangeTracker.Entries<CollectionTransactionEntity>()
            .Where(x => x.State is EntityState.Modified or EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .ToArray();
        if (collectionIds.Length > 0)
            throw new InvalidOperationException(
                $"CollectionTransaction is append-only; mutation denied: {string.Join(',', collectionIds)}");

        var linkIds = context.ChangeTracker.Entries<FinancialLinkEntity>()
            .Where(x => x.State is EntityState.Modified or EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .ToArray();
        if (linkIds.Length > 0)
            throw new InvalidOperationException(
                $"FinancialLink is append-only; mutation denied: {string.Join(',', linkIds)}");
    }
}
