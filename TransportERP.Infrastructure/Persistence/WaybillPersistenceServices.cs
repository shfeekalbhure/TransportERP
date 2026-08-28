using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Infrastructure.Persistence;

public sealed class EfWaybillRepository(TransportErpDbContext db) : IWaybillRepository
{
    private DbSet<WaybillEntity> Waybills => db.Set<WaybillEntity>();
    private DbSet<WaybillPartyEntity> WaybillParties => db.Set<WaybillPartyEntity>();
    private DbSet<WaybillItemEntity> WaybillItems => db.Set<WaybillItemEntity>();
    private DbSet<NumberReservationEntity> NumberReservations => db.Set<NumberReservationEntity>();

    public async Task<WaybillAggregate?> GetAsync(Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken)
    {
        var entity = await Waybills.AsNoTracking()
            .Include(x => x.Parties)
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Id == waybillId && x.CompanyId == companyId && x.BranchId == branchId, cancellationToken);
        return entity is null ? null : ToAggregate(entity);
    }

    public async Task<WaybillAggregate?> GetByCreateOperationAsync(Guid companyId, Guid branchId, string clientOperationId, CancellationToken cancellationToken)
    {
        await LockIdempotencyKeyAsync($"waybill|{companyId}|{branchId}|{clientOperationId.Trim()}", cancellationToken);
        var entity = await Waybills.AsNoTracking()
            .Include(x => x.Parties)
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.CompanyId == companyId && x.BranchId == branchId && x.CreateClientOperationId == clientOperationId, cancellationToken);
        return entity is null ? null : ToAggregate(entity);
    }

    public Task<bool> WasLastOperationAsync(Guid companyId, Guid branchId, Guid waybillId, string clientOperationId, CancellationToken cancellationToken)
        => Waybills.AsNoTracking().AnyAsync(x => x.Id == waybillId && x.CompanyId == companyId && x.BranchId == branchId && x.LastClientOperationId == clientOperationId, cancellationToken);

    public async Task<WaybillAggregate> AddOrGetAsync(WaybillAggregate aggregate, string clientOperationId, CancellationToken cancellationToken)
    {
        var operationId = clientOperationId.Trim();
        var existing = await GetByCreateOperationAsync(aggregate.CompanyId, aggregate.BranchId, operationId, cancellationToken);
        if (existing is not null)
            return existing;

        var now = DateTimeOffset.UtcNow;
        var entity = new WaybillEntity
        {
            Id = aggregate.Id,
            CompanyId = aggregate.CompanyId,
            BranchId = aggregate.BranchId,
            DraftNo = aggregate.DraftNo,
            WaybillNo = aggregate.WaybillNo,
            WaybillDateTime = aggregate.WaybillDateTime,
            ServiceType = aggregate.ServiceType,
            Priority = aggregate.Priority,
            OriginId = aggregate.OriginId,
            DestinationId = aggregate.DestinationId,
            CurrencyId = aggregate.CurrencyId,
            ExchangeRate = aggregate.ExchangeRate,
            FreightTotal = aggregate.FreightTotal,
            DiscountTotal = aggregate.DiscountTotal,
            Status = ToStorageStatus(aggregate.Status),
            CreateClientOperationId = operationId,
            LastClientOperationId = operationId,
            Version = aggregate.Version,
            CreatedAt = now,
            UpdatedAt = now,
            Parties = aggregate.Parties.Select((x, i) => ToEntity(aggregate.Id, x, i + 1)).ToList(),
            Items = aggregate.Items.Select(x => ToEntity(aggregate.Id, x)).ToList()
        };

        var ambient = db.Database.CurrentTransaction;
        var savepoint = $"waybill_insert_{entity.Id:N}";
        if (ambient is not null)
            await ambient.CreateSavepointAsync(savepoint, cancellationToken);
        Waybills.Add(entity);
        try
        {
            await SaveWithConcurrencyMapping(cancellationToken);
            if (ambient is not null)
                await ambient.ReleaseSavepointAsync(savepoint, cancellationToken);
            return aggregate;
        }
        catch (WaybillPersistenceException ex) when (ex.Code == "DUPLICATE_OPERATION")
        {
            if (ambient is not null)
                await ambient.RollbackToSavepointAsync(savepoint, cancellationToken);
            db.ChangeTracker.Clear();
            existing = await GetByCreateOperationAsync(aggregate.CompanyId, aggregate.BranchId, operationId, cancellationToken);
            if (existing is not null)
            {
                if (ambient is not null)
                    await ambient.ReleaseSavepointAsync(savepoint, cancellationToken);
                return existing;
            }
            throw;
        }
    }

    public async Task SaveAsync(WaybillAggregate aggregate, long expectedVersion, string clientOperationId, CancellationToken cancellationToken)
    {
        var entity = await Waybills
            .Include(x => x.Parties)
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Id == aggregate.Id && x.CompanyId == aggregate.CompanyId && x.BranchId == aggregate.BranchId, cancellationToken)
            ?? throw new WaybillPersistenceException("NOT_FOUND");

        if (entity.Version != expectedVersion)
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");

        entity.WaybillNo = aggregate.WaybillNo;
        entity.WaybillDateTime = aggregate.WaybillDateTime;
        entity.ServiceType = aggregate.ServiceType;
        entity.Priority = aggregate.Priority;
        entity.OriginId = aggregate.OriginId;
        entity.DestinationId = aggregate.DestinationId;
        entity.CurrencyId = aggregate.CurrencyId;
        entity.ExchangeRate = aggregate.ExchangeRate;
        entity.FreightTotal = aggregate.FreightTotal;
        entity.DiscountTotal = aggregate.DiscountTotal;
        entity.Status = ToStorageStatus(aggregate.Status);
        entity.LastClientOperationId = clientOperationId;
        entity.Version = aggregate.Version;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        WaybillParties.RemoveRange(entity.Parties);
        entity.Parties = aggregate.Parties.Select((x, i) => ToEntity(aggregate.Id, x, i + 1)).ToList();

        var existingItems = entity.Items.ToDictionary(x => x.Id);
        var incomingIds = aggregate.Items.Select(x => x.Id).ToHashSet();
        foreach (var removed in entity.Items.Where(x => !incomingIds.Contains(x.Id)).ToList())
            WaybillItems.Remove(removed);
        foreach (var item in aggregate.Items)
        {
            if (existingItems.TryGetValue(item.Id, out var tracked))
                Copy(item, tracked);
            else
                entity.Items.Add(ToEntity(aggregate.Id, item));
        }

        await SaveWithConcurrencyMapping(cancellationToken);
    }

    public async Task LinkNumberReservationAsync(Guid companyId, Guid branchId, Guid waybillId, Guid reservationId, CancellationToken cancellationToken)
    {
        var reservation = await NumberReservations.SingleOrDefaultAsync(
            x => x.Id == reservationId && x.CompanyId == companyId && (x.BranchId == null || x.BranchId == branchId), cancellationToken)
            ?? throw new WaybillPersistenceException("NUMBERING_UNAVAILABLE");
        if (reservation.WaybillId.HasValue && reservation.WaybillId != waybillId)
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
        reservation.WaybillId = waybillId;
        await SaveWithConcurrencyMapping(cancellationToken);
    }

    private async Task SaveWithConcurrencyMapping(CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new WaybillPersistenceException("DUPLICATE_OPERATION", ex);
        }
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "23505" }) return true;
        return false;
    }

    private async Task LockIdempotencyKeyAsync(string key, CancellationToken cancellationToken)
    {
        if (db.Database.CurrentTransaction is null || !db.Database.IsNpgsql()) return;
        var lockKey = BitConverter.ToInt64(SHA256.HashData(Encoding.UTF8.GetBytes(key)), 0);
        await db.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock({lockKey})", cancellationToken);
    }

    private static WaybillAggregate ToAggregate(WaybillEntity x)
        => WaybillAggregate.Rehydrate(
            x.Id, x.CompanyId, x.BranchId, x.DraftNo, x.WaybillNo, x.WaybillDateTime,
            x.ServiceType, x.Priority, x.OriginId, x.DestinationId, x.CurrencyId, x.ExchangeRate,
            x.FreightTotal, x.DiscountTotal, FromStorageStatus(x.Status), x.Version,
            x.Parties.OrderBy(p => p.Sequence).Select(p => new WaybillPartyValue(
                Enum.Parse<WaybillPartyRole>(p.Role, true), p.OperationalPartyId, p.NameSnapshot,
                p.MobileSnapshot, p.IdentityTypeSnapshot, p.IdentityNoSnapshot,
                p.CountryId, p.GovernorateId, p.CityId, p.AreaId, p.AddressLineSnapshot)),
            x.Items.OrderBy(i => i.LineNo).Select(i => new WaybillItemValue(
                i.Id, i.LineNo, i.ItemType, i.Contents, i.Quantity, i.Pieces, i.Weight,
                i.Length, i.Width, i.Height, i.DeclaredValue, i.OriginCountryId, i.RiskFlagsJson, i.Notes, i.Volume)));

    private static WaybillPartyEntity ToEntity(Guid waybillId, WaybillPartyValue x, int sequence)
        => new()
        {
            Id = Guid.NewGuid(), WaybillId = waybillId, Sequence = sequence,
            Role = x.Role.ToString().ToUpperInvariant(), OperationalPartyId = x.OperationalPartyId,
            NameSnapshot = x.Name, MobileSnapshot = x.Mobile, IdentityTypeSnapshot = x.IdentityType,
            IdentityNoSnapshot = x.IdentityNo, CountryId = x.CountryId, GovernorateId = x.GovernorateId,
            CityId = x.CityId, AreaId = x.AreaId, AddressLineSnapshot = x.AddressText
        };

    private static WaybillItemEntity ToEntity(Guid waybillId, WaybillItemValue x)
    {
        var entity = new WaybillItemEntity { Id = x.Id, WaybillId = waybillId };
        Copy(x, entity);
        return entity;
    }

    private static void Copy(WaybillItemValue source, WaybillItemEntity target)
    {
        target.LineNo = source.LineNo;
        target.ItemType = source.ItemType;
        target.Contents = source.Contents;
        target.Quantity = source.Quantity;
        target.Pieces = source.Pieces;
        target.Weight = source.Weight;
        target.Length = source.Length;
        target.Width = source.Width;
        target.Height = source.Height;
        target.Volume = source.Volume;
        target.DeclaredValue = source.DeclaredValue;
        target.OriginCountryId = source.OriginCountryId;
        target.RiskFlagsJson = source.RiskFlagsJson;
        target.Notes = source.Notes;
    }

    private static string ToStorageStatus(WaybillStatus status) => status switch
    {
        WaybillStatus.Draft => "DRAFT",
        WaybillStatus.ReadyForApproval => "READY_FOR_APPROVAL",
        WaybillStatus.Approved => "APPROVED",
        WaybillStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

    private static WaybillStatus FromStorageStatus(string status) => status switch
    {
        "DRAFT" => WaybillStatus.Draft,
        "READY_FOR_APPROVAL" => WaybillStatus.ReadyForApproval,
        "APPROVED" => WaybillStatus.Approved,
        "CANCELLED" => WaybillStatus.Cancelled,
        _ => throw new WaybillPersistenceException("INVALID_STATE")
    };
}

public sealed class EfOperationalPartyRepository(TransportErpDbContext db) : IOperationalPartyRepository
{
    private DbSet<OperationalPartyEntity> OperationalParties => db.Set<OperationalPartyEntity>();

    public async Task<(IReadOnlyList<OperationalPartyRecord> Items, long Total)> SearchAsync(
        Guid companyId, Guid branchId, string? query, int skip, int take, CancellationToken cancellationToken)
    {
        IQueryable<OperationalPartyEntity> q = OperationalParties.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "ACTIVE" && (x.BranchId == null || x.BranchId == branchId));
        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(x => EF.Functions.ILike(x.PartyNo, pattern) || EF.Functions.ILike(x.Name, pattern) ||
                             EF.Functions.ILike(x.Mobile, pattern) || (x.IdentityNo != null && EF.Functions.ILike(x.IdentityNo, pattern)));
        }
        var total = await q.LongCountAsync(cancellationToken);
        var rows = await q.OrderBy(x => x.Name).ThenBy(x => x.PartyNo).Skip(skip).Take(take).ToListAsync(cancellationToken);
        return (rows.Select(ToRecord).ToList(), total);
    }

    public async Task<OperationalPartyRecord?> GetByClientOperationAsync(
        Guid companyId, Guid branchId, string clientOperationId, CancellationToken cancellationToken)
    {
        if (db.Database.CurrentTransaction is not null && db.Database.IsNpgsql())
        {
            var key = $"party|{companyId}|{branchId}|{clientOperationId.Trim()}";
            var lockKey = BitConverter.ToInt64(SHA256.HashData(Encoding.UTF8.GetBytes(key)), 0);
            await db.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock({lockKey})", cancellationToken);
        }
        var entity = await OperationalParties.AsNoTracking()
            .SingleOrDefaultAsync(x => x.CompanyId == companyId && x.BranchId == branchId &&
                x.ClientOperationId == clientOperationId, cancellationToken);
        return entity is null ? null : ToRecord(entity);
    }

    public async Task EnsureUsableAsync(
        Guid companyId,
        Guid branchId,
        IReadOnlyCollection<Guid> operationalPartyIds,
        CancellationToken cancellationToken)
    {
        var ids = operationalPartyIds.Where(x => x != Guid.Empty).Distinct().ToArray();
        if (ids.Length != operationalPartyIds.Distinct().Count())
            throw new WaybillPersistenceException("SCOPE_DENIED");
        if (ids.Length == 0) return;

        var usable = await OperationalParties.AsNoTracking().CountAsync(x =>
            ids.Contains(x.Id) && x.CompanyId == companyId && x.Status == "ACTIVE" &&
            (x.BranchId == null || x.BranchId == branchId), cancellationToken);
        if (usable != ids.Length)
            throw new WaybillPersistenceException("SCOPE_DENIED");
    }

    public async Task<OperationalPartyRecord> CreateAsync(
        Guid companyId, Guid branchId, string partyNo, OperationalPartyCreateRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var entity = new OperationalPartyEntity
        {
            Id = Guid.NewGuid(), CompanyId = companyId, BranchId = branchId, PartyNo = partyNo,
            Name = request.Name.Trim(), Mobile = request.Mobile.Trim(), IdentityType = NullIfWhite(request.IdentityType),
            IdentityNo = NullIfWhite(request.IdentityNo), CountryId = request.Address.CountryId,
            GovernorateId = request.Address.GovernorateId, CityId = request.Address.CityId, AreaId = request.Address.AreaId, AddressLine = NullIfWhite(request.Address.AddressLine),
            Status = "ACTIVE", ClientOperationId = request.ClientOperationId.Trim(), Version = 1, CreatedAt = now, UpdatedAt = now
        };
        OperationalParties.Add(entity);
        try { await db.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateException ex) { throw new WaybillPersistenceException("PARTY_DUPLICATE_WARNING", ex); }
        return ToRecord(entity);
    }

    private static OperationalPartyRecord ToRecord(OperationalPartyEntity x)
        => new(x.Id, x.CompanyId, x.BranchId, x.PartyNo, x.Name, x.Mobile, x.IdentityType, x.IdentityNo,
            new GeoAddressSnapshot(x.CountryId, x.GovernorateId, x.CityId, x.AreaId, x.AddressLine),
            x.Status, x.Version);

    private static string? NullIfWhite(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public sealed class EfNumberReservationService(TransportErpDbContext db) : INumberReservationService
{
    private DbSet<NumberReservationEntity> Reservations => db.Set<NumberReservationEntity>();
    private DbSet<NumberSequenceEntity> Sequences => db.Set<NumberSequenceEntity>();

    public async ValueTask<NumberReservationDto> ReserveAsync(
        OperationContext context, NumberReservationRequest request, CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        request.EnsureValid();
        var key = request.IdempotencyKey.Trim();
        var existing = await Reservations.SingleOrDefaultAsync(
            x => x.CompanyId == context.CompanyId && x.IdempotencyKey == key, cancellationToken);
        if (existing is not null)
        {
            if (existing.SequenceId != request.SequenceId)
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return ToDto(existing);
        }

        var sequence = await Sequences.SingleOrDefaultAsync(
            x => x.Id == request.SequenceId && x.CompanyId == context.CompanyId &&
                 (x.BranchId == null || x.BranchId == context.BranchId) && x.Status == "ACTIVE", cancellationToken)
            ?? throw new WaybillPersistenceException("NUMBERING_UNAVAILABLE");

        var value = sequence.NextValue;
        if (value < 1) throw new WaybillPersistenceException("NUMBERING_UNAVAILABLE");
        sequence.NextValue++;
        sequence.Version++;
        sequence.UpdatedAt = DateTimeOffset.UtcNow;
        var rendered = $"{sequence.Prefix}{value:D8}";
        var reservation = new NumberReservationEntity
        {
            Id = Guid.NewGuid(), SequenceId = sequence.Id, CompanyId = context.CompanyId,
            BranchId = sequence.BranchId ?? context.BranchId, IdempotencyKey = key, NumberValue = value,
            RenderedNumber = rendered, ReservedAt = DateTimeOffset.UtcNow, State = NumberReservationStates.Reserved
        };
        Reservations.Add(reservation);
        try { await db.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateConcurrencyException ex) { throw new WaybillPersistenceException("NUMBERING_CONCURRENCY", ex); }
        catch (DbUpdateException ex) { throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex); }
        return ToDto(reservation);
    }

    public async ValueTask<NumberReservationDto> CommitAsync(
        OperationContext context, NumberReservationTransitionRequest request, CancellationToken cancellationToken = default)
    {
        context.EnsureComplete(); request.EnsureValid();
        var entity = await ScopedReservation(context, request.ReservationId, cancellationToken);
        if (entity.State == NumberReservationStates.Committed) return ToDto(entity);
        if (entity.State == NumberReservationStates.Void) throw new WaybillPersistenceException("NUMBER_ALREADY_VOID");
        if (!entity.WaybillId.HasValue) throw new WaybillPersistenceException("NUMBER_RESERVATION_UNLINKED");
        entity.State = NumberReservationStates.Committed;
        entity.CommittedAt = DateTimeOffset.UtcNow;
        entity.LastTransitionKey = request.IdempotencyKey.Trim();
        await db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async ValueTask<NumberReservationDto> VoidAsync(
        OperationContext context, NumberReservationTransitionRequest request, CancellationToken cancellationToken = default)
    {
        context.EnsureComplete(); request.EnsureValid();
        var entity = await ScopedReservation(context, request.ReservationId, cancellationToken);
        if (entity.State == NumberReservationStates.Void) return ToDto(entity);
        if (entity.State == NumberReservationStates.Committed) throw new WaybillPersistenceException("COMMITTED_NUMBER_CANNOT_VOID");
        entity.State = NumberReservationStates.Void;
        entity.VoidedAt = DateTimeOffset.UtcNow;
        entity.VoidReason = request.Reason;
        entity.LastTransitionKey = request.IdempotencyKey.Trim();
        await db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    private async Task<NumberReservationEntity> ScopedReservation(OperationContext context, Guid id, CancellationToken ct)
        => await Reservations.SingleOrDefaultAsync(
            x => x.Id == id && x.CompanyId == context.CompanyId && (x.BranchId == null || x.BranchId == context.BranchId), ct)
            ?? throw new WaybillPersistenceException("NUMBERING_UNAVAILABLE");

    private static NumberReservationDto ToDto(NumberReservationEntity x)
    {
        if (x.NumberValue < 0) throw new WaybillPersistenceException("NUMBERING_UNAVAILABLE");
        return new NumberReservationDto(x.Id, x.SequenceId, checked((ulong)x.NumberValue), x.RenderedNumber, x.State);
    }
}

public sealed class EfWaybillUnitOfWork(TransportErpDbContext db) : IWaybillUnitOfWork
{
    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        if (db.Database.CurrentTransaction is not null)
            return await action(cancellationToken);
        for (var attempt = 1; ; attempt++)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var result = await action(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception ex) when (attempt < 4 && RetryWholeUnitOfWork(ex))
            {
                await transaction.RollbackAsync(cancellationToken);
                db.ChangeTracker.Clear();
                await Task.Delay(TimeSpan.FromMilliseconds(20 * attempt), cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    private static bool RetryWholeUnitOfWork(Exception exception)
    {
        if (exception is WaybillPersistenceException { Code: "DUPLICATE_OPERATION" or "PARTY_DUPLICATE_WARNING" })
            return true;
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "40001" or "40P01" }) return true;
        return false;
    }
}

public sealed class EfWaybillAuditSink(TransportErpDbContext _, AuditEventService auditService) : IWaybillAuditSink
{
    public async Task WriteAsync(
        OperationContext context, string action, string outcome, string entityType, Guid entityId,
        string? beforeJson, string? afterJson, string? reason, CancellationToken cancellationToken)
    {
        await auditService.AppendAuditEventAsync(new AuditEventDraft(
            action, outcome, entityType, entityId, context.UserId, context.CompanyId, context.BranchId,
            context.CorrelationId, BeforeJson: beforeJson, AfterJson: afterJson, Reason: reason), cancellationToken);
    }
}

public sealed class WaybillPersistenceException : InvalidOperationException
{
    public WaybillPersistenceException(string code) : base(code) => Code = code;
    public WaybillPersistenceException(string code, Exception inner) : base(code, inner) => Code = code;
    public string Code { get; }
}
