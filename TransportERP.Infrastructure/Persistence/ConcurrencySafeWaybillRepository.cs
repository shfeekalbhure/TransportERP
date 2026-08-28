using System.Data;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Production repository wrapper for P2-C01-A. Header updates are guarded by an explicit
/// Company/Branch/Id/Version predicate, so a stale ExpectedVersion can never overwrite a newer row.
/// Child snapshots are replaced in the same transaction; later movement/allocation tables are out of A scope.
/// </summary>
public sealed class ConcurrencySafeWaybillRepository(TransportErpDbContext db) : IWaybillRepository
{
    private readonly EfWaybillRepository inner = new(db);

    public Task<WaybillAggregate?> GetAsync(Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken)
        => inner.GetAsync(companyId, branchId, waybillId, cancellationToken);

    public Task<WaybillAggregate?> GetByCreateOperationAsync(Guid companyId, Guid branchId, string clientOperationId, CancellationToken cancellationToken)
        => inner.GetByCreateOperationAsync(companyId, branchId, clientOperationId, cancellationToken);

    public Task<bool> WasLastOperationAsync(Guid companyId, Guid branchId, Guid waybillId, string clientOperationId, CancellationToken cancellationToken)
        => inner.WasLastOperationAsync(companyId, branchId, waybillId, clientOperationId, cancellationToken);

    public Task<WaybillAggregate> AddOrGetAsync(WaybillAggregate aggregate, string clientOperationId, CancellationToken cancellationToken)
        => inner.AddOrGetAsync(aggregate, clientOperationId, cancellationToken);

    public Task LinkNumberReservationAsync(Guid companyId, Guid branchId, Guid waybillId, Guid reservationId, CancellationToken cancellationToken)
        => inner.LinkNumberReservationAsync(companyId, branchId, waybillId, reservationId, cancellationToken);

    public async Task SaveAsync(
        WaybillAggregate aggregate,
        long expectedVersion,
        string clientOperationId,
        CancellationToken cancellationToken)
    {
        if (expectedVersion < 1 || aggregate.Version <= expectedVersion)
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");

        var ownsTransaction = db.Database.CurrentTransaction is null;
        await using var transaction = ownsTransaction
            ? await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken)
            : null;

        try
        {
            // Do not allow stale tracked instances to influence an explicit compare-and-swap update.
            db.ChangeTracker.Clear();

            var affected = await db.Set<WaybillEntity>()
                .Where(x => x.Id == aggregate.Id &&
                            x.CompanyId == aggregate.CompanyId &&
                            x.BranchId == aggregate.BranchId &&
                            x.Version == expectedVersion)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.WaybillNo, aggregate.WaybillNo)
                    .SetProperty(x => x.WaybillDateTime, aggregate.WaybillDateTime)
                    .SetProperty(x => x.ServiceType, aggregate.ServiceType)
                    .SetProperty(x => x.Priority, aggregate.Priority)
                    .SetProperty(x => x.OriginId, aggregate.OriginId)
                    .SetProperty(x => x.DestinationId, aggregate.DestinationId)
                    .SetProperty(x => x.CurrencyId, aggregate.CurrencyId)
                    .SetProperty(x => x.ExchangeRate, aggregate.ExchangeRate)
                    .SetProperty(x => x.FreightTotal, aggregate.FreightTotal)
                    .SetProperty(x => x.DiscountTotal, aggregate.DiscountTotal)
                    .SetProperty(x => x.Status, ToStorageStatus(aggregate.Status))
                    .SetProperty(x => x.LastClientOperationId, clientOperationId.Trim())
                    .SetProperty(x => x.Version, aggregate.Version)
                    .SetProperty(x => x.UpdatedAt, DateTimeOffset.UtcNow),
                    cancellationToken);

            if (affected != 1)
                throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");

            await db.Set<WaybillPartyEntity>()
                .Where(x => x.WaybillId == aggregate.Id)
                .ExecuteDeleteAsync(cancellationToken);
            await db.Set<WaybillItemEntity>()
                .Where(x => x.WaybillId == aggregate.Id)
                .ExecuteDeleteAsync(cancellationToken);

            db.Set<WaybillPartyEntity>().AddRange(
                aggregate.Parties.Select((x, i) => ToPartyEntity(aggregate.Id, x, i + 1)));
            db.Set<WaybillItemEntity>().AddRange(
                aggregate.Items.Select(x => ToItemEntity(aggregate.Id, x)));
            await db.SaveChangesAsync(cancellationToken);

            if (transaction is not null)
                await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            if (transaction is not null)
                await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static WaybillPartyEntity ToPartyEntity(Guid waybillId, WaybillPartyValue x, int sequence)
        => new()
        {
            Id = Guid.NewGuid(),
            WaybillId = waybillId,
            Sequence = sequence,
            Role = x.Role.ToString().ToUpperInvariant(),
            OperationalPartyId = x.OperationalPartyId,
            NameSnapshot = x.Name,
            MobileSnapshot = x.Mobile,
            IdentityTypeSnapshot = x.IdentityType,
            IdentityNoSnapshot = x.IdentityNo,
            CountryId = x.CountryId,
            GovernorateId = x.GovernorateId,
            CityId = x.CityId,
            AreaId = x.AreaId,
            AddressLineSnapshot = x.AddressText
        };

    private static WaybillItemEntity ToItemEntity(Guid waybillId, WaybillItemValue x)
        => new()
        {
            Id = x.Id,
            WaybillId = waybillId,
            LineNo = x.LineNo,
            ItemType = x.ItemType,
            Contents = x.Contents,
            Quantity = x.Quantity,
            Pieces = x.Pieces,
            Weight = x.Weight,
            Length = x.Length,
            Width = x.Width,
            Height = x.Height,
            Volume = x.Volume,
            DeclaredValue = x.DeclaredValue,
            OriginCountryId = x.OriginCountryId,
            RiskFlagsJson = x.RiskFlagsJson,
            Notes = x.Notes
        };

    private static string ToStorageStatus(WaybillStatus status) => status switch
    {
        WaybillStatus.Draft => "DRAFT",
        WaybillStatus.ReadyForApproval => "READY_FOR_APPROVAL",
        WaybillStatus.Approved => "APPROVED",
        WaybillStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };
}
