using System.Collections.Concurrent;

namespace TransportERP.Application.P1Baseline;

public sealed record P1Scope(string CompanyId, string? BranchId = null);
public sealed record P1User(string Id, string CompanyId, string? BranchId, string UserName, string Password, bool Active = true, long Version = 1);
public sealed record P1Session(string Id, string UserId, P1Scope Scope, DateTimeOffset IssuedAt);
public sealed record P1Company(string Id, string Code, string Name, bool Active = true, long Version = 1);
public sealed record P1Branch(string Id, string CompanyId, string Code, string Name, bool Active = true, long Version = 1);
public sealed record P1Account(string Id, string CompanyId, string Code, string Name, string? ParentId, bool IsPosting, long Version = 1);
public enum P1PeriodState { Open, SoftClosed, Closed }
public sealed record P1FiscalPeriod(string Id, string CompanyId, DateOnly Start, DateOnly End, P1PeriodState State = P1PeriodState.Open, long Version = 1);
public enum P1JournalState { Draft, Checked, Posted, Reversed }
public sealed record P1JournalLine(string AccountId, decimal Debit, decimal Credit, string? Dimension = null);
public sealed record P1JournalEntry(string Id, string CompanyId, string? BranchId, string ClientOperationId, string Reference, string PeriodId, IReadOnlyList<P1JournalLine> Lines, P1JournalState State = P1JournalState.Draft, string? ReversalOf = null, long Version = 1);
public sealed record P1Voucher(string Id, string CompanyId, string? BranchId, decimal Amount, string PaymentMethod, string Reference, bool Receipt, long Version = 1);
public sealed record P1AuditEvent(string Id, string Action, string EntityId, string ActorId, string CompanyId, string? BranchId, string CorrelationId, DateTimeOffset At, string Outcome);
public sealed record P1SyncOperation(string DeviceId, string ClientOperationId, string PayloadHash, string Action, string CompanyId, string? BranchId, string BaseVersion);
public sealed record P1SyncResult(string ClientOperationId, string Status, string? ErrorCode = null);

public sealed class P1RuleException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public sealed class P1InMemoryStore
{
    internal ConcurrentDictionary<string, P1User> Users { get; } = new();
    internal ConcurrentDictionary<string, P1Session> Sessions { get; } = new();
    internal ConcurrentDictionary<string, P1Company> Companies { get; } = new();
    internal ConcurrentDictionary<string, P1Branch> Branches { get; } = new();
    internal ConcurrentDictionary<string, P1Account> Accounts { get; } = new();
    internal ConcurrentDictionary<string, P1FiscalPeriod> Periods { get; } = new();
    internal ConcurrentDictionary<string, P1JournalEntry> Journals { get; } = new();
    internal ConcurrentDictionary<string, P1Voucher> Vouchers { get; } = new();
    internal ConcurrentDictionary<string, P1SyncOperation> SyncOperations { get; } = new();
    internal ConcurrentBag<P1AuditEvent> AuditEvents { get; } = new();
}

public sealed class P1InMemoryService
{
    private readonly P1InMemoryStore _store;

    public P1InMemoryService(P1InMemoryStore? store = null) => _store = store ?? new P1InMemoryStore();

    public P1Company CreateCompany(string id, string code, string name, string actorId = "system")
    {
        if (!_store.Companies.TryAdd(id, new P1Company(id, code, name)))
            throw new P1RuleException("COMPANY_DUPLICATE", id);
        Audit("ManageOrganizations", id, actorId, id, null, "SUCCESS");
        return _store.Companies[id];
    }

    public P1Branch CreateBranch(string companyId, string id, string code, string name, string actorId = "system")
    {
        RequireCompany(companyId);
        if (_store.Branches.Values.Any(x => x.CompanyId == companyId && x.Code == code))
            throw new P1RuleException("BRANCH_DUPLICATE", code);
        var branch = new P1Branch(id, companyId, code, name);
        _store.Branches[id] = branch;
        Audit("ManageOrganizations", id, actorId, companyId, id, "SUCCESS");
        return branch;
    }

    public P1User CreateUser(string companyId, string? branchId, string id, string userName, string password, string actorId = "system")
    {
        RequireCompany(companyId);
        if (_store.Users.Values.Any(x => x.UserName == userName))
            throw new P1RuleException("USER_DUPLICATE", userName);
        if (branchId is not null) RequireBranch(companyId, branchId);
        var user = new P1User(id, companyId, branchId, userName, password);
        _store.Users[id] = user;
        Audit("ManageUsers", id, actorId, companyId, branchId, "SUCCESS");
        return user;
    }

    public P1Session Authenticate(string userName, string password, P1Scope scope)
    {
        var user = _store.Users.Values.SingleOrDefault(x => x.UserName == userName);
        if (user is null || user.Password != password) throw new P1RuleException("AUTH_INVALID_CREDENTIALS", userName);
        if (!user.Active) throw new P1RuleException("AUTH_LOCKED", userName);
        RequireCompany(scope.CompanyId);
        if (user.CompanyId != scope.CompanyId || (scope.BranchId is not null && user.BranchId is not null && user.BranchId != scope.BranchId))
            throw new P1RuleException("AUTH_SCOPE_REQUIRED", userName);
        var session = new P1Session(Guid.NewGuid().ToString("N"), user.Id, scope, DateTimeOffset.UtcNow);
        _store.Sessions[session.Id] = session;
        Audit("Authenticate", session.Id, user.Id, scope.CompanyId, scope.BranchId, "SUCCESS");
        return session;
    }

    public P1Account CreateAccount(string companyId, string id, string code, string name, string? parentId, bool isPosting, string actorId = "system")
    {
        RequireCompany(companyId);
        if (_store.Accounts.Values.Any(x => x.CompanyId == companyId && x.Code == code))
            throw new P1RuleException("ACCOUNT_CODE_DUPLICATE", code);
        if (parentId is not null && (!_store.Accounts.TryGetValue(parentId, out var parent) || parent.CompanyId != companyId))
            throw new P1RuleException("PARENT_INVALID", parentId);
        var account = new P1Account(id, companyId, code, name, parentId, isPosting);
        _store.Accounts[id] = account;
        Audit("CreateChartOfAccounts", id, actorId, companyId, null, "SUCCESS");
        return account;
    }

    public P1FiscalPeriod CreatePeriod(string companyId, string id, DateOnly start, DateOnly end, string actorId = "system")
    {
        RequireCompany(companyId);
        if (start >= end) throw new P1RuleException("PERIOD_OVERLAP", id);
        var period = new P1FiscalPeriod(id, companyId, start, end);
        _store.Periods[id] = period;
        Audit("ManageFiscalPeriod", id, actorId, companyId, null, "SUCCESS");
        return period;
    }

    public P1FiscalPeriod SetPeriodState(string companyId, string periodId, P1PeriodState state, string actorId = "system")
    {
        var period = RequirePeriod(companyId, periodId);
        if (period.State == P1PeriodState.Closed && state != P1PeriodState.Open)
            throw new P1RuleException("INVALID_STATE_TRANSITION", periodId);
        if (state == P1PeriodState.Closed && _store.Journals.Values.Any(x => x.PeriodId == periodId && x.State is P1JournalState.Draft or P1JournalState.Checked))
            throw new P1RuleException("PERIOD_HAS_BLOCKERS", periodId);
        var updated = period with { State = state, Version = period.Version + 1 };
        _store.Periods[periodId] = updated;
        Audit("ManageFiscalPeriod", periodId, actorId, companyId, null, "SUCCESS");
        return updated;
    }

    public P1JournalEntry CreateJournal(string companyId, string? branchId, string periodId, string entryId, string clientOperationId, string reference, IReadOnlyList<P1JournalLine> lines, string actorId = "system")
    {
        var period = RequirePeriod(companyId, periodId);
        if (period.State == P1PeriodState.Closed) throw new P1RuleException("PERIOD_CLOSED", periodId);
        if (lines.Count == 0 || lines.Sum(x => x.Debit) != lines.Sum(x => x.Credit)) throw new P1RuleException("ENTRY_UNBALANCED", reference);
        if (lines.Any(x => !_store.Accounts.TryGetValue(x.AccountId, out var a) || a.CompanyId != companyId || !a.IsPosting))
            throw new P1RuleException("ACCOUNT_NOT_POSTABLE", reference);
        if (_store.Journals.Values.Any(x => x.ClientOperationId == clientOperationId))
            return _store.Journals.Values.Single(x => x.ClientOperationId == clientOperationId);
        var entry = new P1JournalEntry(entryId, companyId, branchId, clientOperationId, reference, periodId, lines, P1JournalState.Checked);
        _store.Journals[entryId] = entry;
        Audit("CreateJournalEntry", entryId, actorId, companyId, branchId, "SUCCESS");
        return entry;
    }

    public P1JournalEntry PostJournal(string companyId, string entryId, string actorId = "system")
    {
        if (!_store.Journals.TryGetValue(entryId, out var entry) || entry.CompanyId != companyId)
            throw new P1RuleException("ENTRY_NOT_FOUND", entryId);
        var period = RequirePeriod(companyId, entry.PeriodId);
        if (period.State == P1PeriodState.Closed) throw new P1RuleException("PERIOD_CLOSED", entry.PeriodId);
        if (entry.State != P1JournalState.Checked) throw new P1RuleException("INVALID_STATE_TRANSITION", entryId);
        var posted = entry with { State = P1JournalState.Posted, Version = entry.Version + 1 };
        _store.Journals[entryId] = posted;
        Audit("PostJournalEntry", entryId, actorId, companyId, entry.BranchId, "SUCCESS");
        return posted;
    }

    public P1JournalEntry ReverseJournal(string companyId, string entryId, string reason, string actorId = "system")
    {
        if (string.IsNullOrWhiteSpace(reason)) throw new P1RuleException("REASON_REQUIRED", entryId);
        if (!_store.Journals.TryGetValue(entryId, out var original) || original.CompanyId != companyId)
            throw new P1RuleException("ENTRY_NOT_FOUND", entryId);
        if (original.State != P1JournalState.Posted) throw new P1RuleException("NOT_POSTED", entryId);
        var reversalId = $"REV-{entryId}";
        var reversal = original with { Id = reversalId, Reference = $"REVERSAL:{entryId}:{reason}", State = P1JournalState.Posted, ReversalOf = entryId, Version = 1 };
        _store.Journals[entryId] = original with { State = P1JournalState.Reversed, Version = original.Version + 1 };
        _store.Journals[reversalId] = reversal;
        Audit("ReverseJournalEntry", entryId, actorId, companyId, original.BranchId, "SUCCESS");
        return reversal;
    }

    public P1Voucher CreateVoucher(string companyId, string? branchId, string id, decimal amount, string paymentMethod, string reference, bool receipt, string actorId = "system")
    {
        RequireCompany(companyId);
        if (amount <= 0) throw new P1RuleException("AMOUNT_INVALID", id);
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new P1RuleException("PAYMENT_METHOD_INVALID", id);
        if (_store.Vouchers.Values.Any(x => x.Reference == reference)) throw new P1RuleException("DUPLICATE_REFERENCE", reference);
        var voucher = new P1Voucher(id, companyId, branchId, amount, paymentMethod, reference, receipt);
        _store.Vouchers[id] = voucher;
        Audit(receipt ? "CreateReceiptVoucher" : "CreatePaymentVoucher", id, actorId, companyId, branchId, "SUCCESS");
        return voucher;
    }

    public IReadOnlyList<P1AuditEvent> ReadAuditEvents(string companyId) => _store.AuditEvents.Where(x => x.CompanyId == companyId).OrderBy(x => x.At).ToArray();

    public IReadOnlyList<P1SyncResult> SyncBatch(IEnumerable<P1SyncOperation> operations, string actorId = "sync")
    {
        var results = new List<P1SyncResult>();
        foreach (var op in operations)
        {
            if (string.IsNullOrWhiteSpace(op.DeviceId) || string.IsNullOrWhiteSpace(op.ClientOperationId) || string.IsNullOrWhiteSpace(op.PayloadHash))
            {
                results.Add(new P1SyncResult(op.ClientOperationId, "REJECTED", "PAYLOAD_INVALID"));
                continue;
            }
            if (_store.SyncOperations.TryGetValue(op.ClientOperationId, out var prior))
            {
                results.Add(new P1SyncResult(op.ClientOperationId, prior.PayloadHash == op.PayloadHash ? "DUPLICATE_ACCEPTED" : "CONFLICT", prior.PayloadHash == op.PayloadHash ? null : "CONFLICT"));
                continue;
            }
            _store.SyncOperations[op.ClientOperationId] = op;
            Audit("SyncP1Operations", op.ClientOperationId, actorId, op.CompanyId, op.BranchId, "SUCCESS");
            results.Add(new P1SyncResult(op.ClientOperationId, "ACCEPTED"));
        }
        return results;
    }

    private void RequireCompany(string companyId)
    {
        if (!_store.Companies.TryGetValue(companyId, out var company) || !company.Active)
            throw new P1RuleException("COMPANY_NOT_FOUND", companyId);
    }

    private void RequireBranch(string companyId, string branchId)
    {
        if (!_store.Branches.TryGetValue(branchId, out var branch) || branch.CompanyId != companyId || !branch.Active)
            throw new P1RuleException("BRANCH_NOT_FOUND", branchId);
    }

    private P1FiscalPeriod RequirePeriod(string companyId, string periodId)
    {
        if (!_store.Periods.TryGetValue(periodId, out var period) || period.CompanyId != companyId)
            throw new P1RuleException("PERIOD_NOT_FOUND", periodId);
        return period;
    }

    private void Audit(string action, string entityId, string actorId, string companyId, string? branchId, string outcome)
    {
        _store.AuditEvents.Add(new P1AuditEvent(Guid.NewGuid().ToString("N"), action, entityId, actorId, companyId, branchId, Guid.NewGuid().ToString("N"), DateTimeOffset.UtcNow, outcome));
    }
}
