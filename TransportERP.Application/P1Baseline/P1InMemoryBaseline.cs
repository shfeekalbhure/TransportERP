using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

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
public enum P1VoucherState { Draft, Approved, Posted, Cancelled }
public sealed record P1Voucher(
    string Id,
    string CompanyId,
    string? BranchId,
    decimal Amount,
    string PaymentMethod,
    string Reference,
    bool Receipt,
    long Version = 1,
    P1VoucherState Status = P1VoucherState.Draft,
    string? CancellationReason = null);
public sealed record P1AuditEvent(
    string Id,
    string Action,
    string EntityId,
    string ActorId,
    string CompanyId,
    string? BranchId,
    string CorrelationId,
    DateTimeOffset At,
    string Outcome,
    string Hash = "",
    string? PreviousHash = null,
    string? Reason = null);
public enum P1SyncStatus { Queued, Sending, Succeeded, Failed, Conflict, Rejected, Resolved }
public sealed record P1SyncOperation(
    string DeviceId,
    string ClientOperationId,
    string PayloadHash,
    string Action,
    string CompanyId,
    string? BranchId,
    string BaseVersion,
    P1SyncStatus Status = P1SyncStatus.Queued,
    int RetryCount = 0,
    DateTimeOffset? NextRetryAt = null,
    string? ErrorCode = null,
    string? ConflictCaseId = null,
    long Version = 1);
public sealed record P1SyncResult(string ClientOperationId, string Status, string? ErrorCode = null);
public enum P1ScreenPhase { Loading, Online, Offline, Ready, Empty, Error, Editing, DraftEditing, Saved, Conflict, Closed, Checked, Approved, Posted, Reversed, Cancelled, Detail, Retrying, Resolved }
public sealed record P1ScreenState(string ScreenId, P1ScreenPhase Phase, string? ErrorCode = null, bool IsOffline = false, long Version = 1);
public sealed record P1Role(string Id, string Name, IReadOnlySet<string> Permissions, long Version = 1)
{
    public P1Role(string id, string name, long version = 1) : this(id, name, new HashSet<string>(), version) { }
}
public sealed record P1Permission(string Code);
public sealed record P1Setting(string Key, string Value, long Version = 1);
public sealed record P1ScopedSetting(string ScopeType, string ScopeId, string Key, string Value, long Version = 1);

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
    internal ConcurrentDictionary<string, P1Role> Roles { get; } = new();
    internal ConcurrentDictionary<string, HashSet<string>> UserRoles { get; } = new();
    internal ConcurrentDictionary<string, P1Setting> GlobalSettings { get; } = new();
    internal ConcurrentDictionary<string, P1ScopedSetting> ScopedSettings { get; } = new();
    internal ConcurrentBag<P1AuditEvent> AuditEvents { get; } = new();
    internal List<P1AuditEvent> AuditSequence { get; } = new();
    internal ConcurrentDictionary<string, P1ScreenState> ScreenStates { get; } = new();
}

public sealed class P1InMemoryService
{
    private readonly P1InMemoryStore _store;
    private readonly object _auditLock = new();

    public P1InMemoryService(P1InMemoryStore? store = null) => _store = store ?? new P1InMemoryStore();

    public P1ScreenState InitializeScreen(string screenId)
    {
        ValidateScreenId(screenId);
        var state = new P1ScreenState(screenId, P1ScreenPhase.Loading);
        _store.ScreenStates[screenId] = state;
        return state;
    }

    public P1ScreenState TransitionScreen(string screenId, P1ScreenPhase newPhase, string? errorCode = null, bool isOffline = false, string actorId = "screen")
    {
        ValidateScreenId(screenId);
        if (!_store.ScreenStates.TryGetValue(screenId, out var current))
            throw new P1RuleException("SCREEN_NOT_INITIALIZED", screenId);
        if (!IsAllowedScreenTransition(screenId, current.Phase, newPhase))
            throw new P1RuleException("INVALID_STATE_TRANSITION", screenId);
        if (newPhase == P1ScreenPhase.Error && string.IsNullOrWhiteSpace(errorCode))
            throw new P1RuleException("ERROR_CODE_REQUIRED", screenId);
        var updated = current with { Phase = newPhase, ErrorCode = errorCode, IsOffline = isOffline, Version = current.Version + 1 };
        _store.ScreenStates[screenId] = updated;
        return updated;
    }

    public P1ScreenState GetScreenState(string screenId)
    {
        ValidateScreenId(screenId);
        if (!_store.ScreenStates.TryGetValue(screenId, out var state)) throw new P1RuleException("SCREEN_NOT_INITIALIZED", screenId);
        return state;
    }

    public void RequireScreenPermission(string screenId, string action, IReadOnlySet<string> permissions, bool online)
    {
        ValidateScreenId(screenId);
        var required = (screenId, action) switch
        {
            ("W3-P1-008", "manage") => new[] { "accounting.period.manage", "accounting.reference.manage" },
            ("W3-P1-009", "create") => new[] { "accounting.journal.create" },
            ("W3-P1-009", "post") => new[] { "accounting.journal.post" },
            ("W3-P1-009", "reverse") => new[] { "accounting.journal.reverse" },
            ("W3-P1-010", "receipt") => new[] { "accounting.receipts.create" },
            ("W3-P1-010", "payment") => new[] { "accounting.payments.create" },
            ("W3-P1-011", "read") => new[] { "audit.events.read" },
            ("W3-P1-012", "retry") => new[] { "sync.operations.execute" },
            ("W3-P1-012", "resolve") => new[] { "sync.conflicts.resolve" },
            _ => Array.Empty<string>()
        };
        if (required.Length == 0) throw new P1RuleException("ACTION_NOT_DEFINED", $"{screenId}:{action}");
        if (!required.Any(permissions.Contains)) throw new P1RuleException("PERMISSION_DENIED", $"{screenId}:{action}");
        if (!online && screenId is "W3-P1-008" or "W3-P1-011" or "W3-P1-012" && action is "manage" or "read" or "retry" or "resolve")
            throw new P1RuleException("ONLINE_REQUIRED", $"{screenId}:{action}");
        if (!online && screenId is "W3-P1-009" or "W3-P1-010" && action is "post" or "reverse" or "receipt" or "payment")
            throw new P1RuleException("OFFLINE_DRAFT_ONLY", $"{screenId}:{action}");
    }

    private static void ValidateScreenId(string screenId)
    {
        if (screenId is not ("W3-P1-008" or "W3-P1-009" or "W3-P1-010" or "W3-P1-011" or "W3-P1-012"))
            throw new P1RuleException("SCREEN_NOT_DEFINED", screenId);
    }

    private static bool IsAllowedScreenTransition(string screenId, P1ScreenPhase from, P1ScreenPhase to) =>
        screenId switch
        {
            "W3-P1-008" => (from, to) is (P1ScreenPhase.Loading, P1ScreenPhase.Ready or P1ScreenPhase.Empty or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.Editing) or (P1ScreenPhase.Editing, P1ScreenPhase.Saved or P1ScreenPhase.Conflict)
                or (P1ScreenPhase.Saved, P1ScreenPhase.Closed),
            "W3-P1-009" => (from, to) is (P1ScreenPhase.Loading, P1ScreenPhase.Ready or P1ScreenPhase.Empty or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.DraftEditing) or (P1ScreenPhase.DraftEditing, P1ScreenPhase.Checked)
                or (P1ScreenPhase.Checked, P1ScreenPhase.Approved) or (P1ScreenPhase.Approved, P1ScreenPhase.Posted)
                or (P1ScreenPhase.Posted, P1ScreenPhase.Reversed),
            "W3-P1-010" => (from, to) is (P1ScreenPhase.Loading, P1ScreenPhase.Ready or P1ScreenPhase.Empty or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.DraftEditing) or (P1ScreenPhase.DraftEditing, P1ScreenPhase.Approved)
                or (P1ScreenPhase.Approved, P1ScreenPhase.Posted or P1ScreenPhase.Cancelled),
            "W3-P1-011" => (from, to) is (P1ScreenPhase.Loading, P1ScreenPhase.Ready or P1ScreenPhase.Empty or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.Detail),
            "W3-P1-012" => (from, to) is (P1ScreenPhase.Loading, P1ScreenPhase.Online or P1ScreenPhase.Offline)
                or (P1ScreenPhase.Online or P1ScreenPhase.Offline, P1ScreenPhase.Ready or P1ScreenPhase.Empty or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.Retrying) or (P1ScreenPhase.Retrying, P1ScreenPhase.Resolved or P1ScreenPhase.Error)
                or (P1ScreenPhase.Ready, P1ScreenPhase.Conflict) or (P1ScreenPhase.Conflict, P1ScreenPhase.Resolved),
            _ => false
        };

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

    public P1Branch UpdateBranch(string companyId, string branchId, string code, string name, bool active, long expectedVersion, string actorId = "system")
    {
        RequireCompany(companyId);
        if (!_store.Branches.TryGetValue(branchId, out var current) || current.CompanyId != companyId)
            throw new P1RuleException("BRANCH_NOT_FOUND", branchId);
        if (current.Version != expectedVersion)
            throw new P1RuleException("CONCURRENCY_CONFLICT", branchId);
        if (_store.Branches.Values.Any(x => x.Id != branchId && x.CompanyId == companyId && x.Code == code))
            throw new P1RuleException("BRANCH_DUPLICATE", code);
        var updated = current with { Code = code, Name = name, Active = active, Version = current.Version + 1 };
        _store.Branches[branchId] = updated;
        Audit("ManageOrganizations", branchId, actorId, companyId, branchId, "SUCCESS");
        return updated;
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

    public P1User UpdateUser(string companyId, string userId, string userName, bool active, long expectedVersion, string actorId = "system")
    {
        if (!_store.Users.TryGetValue(userId, out var current) || current.CompanyId != companyId)
            throw new P1RuleException("USER_NOT_FOUND", userId);
        if (current.Version != expectedVersion)
            throw new P1RuleException("CONCURRENCY_CONFLICT", userId);
        if (_store.Users.Values.Any(x => x.Id != userId && x.UserName == userName))
            throw new P1RuleException("USER_DUPLICATE", userName);
        var updated = current with { UserName = userName, Active = active, Version = current.Version + 1 };
        _store.Users[userId] = updated;
        Audit("ManageUsers", userId, actorId, companyId, current.BranchId, "SUCCESS");
        return updated;
    }

    public P1Role RegisterRole(string roleId, string name, IEnumerable<string>? permissions = null, string actorId = "system")
    {
        if (string.IsNullOrWhiteSpace(name)) throw new P1RuleException("VALIDATION_ERROR", roleId);
        if (!_store.Roles.TryAdd(roleId, new P1Role(roleId, name, (permissions ?? Array.Empty<string>()).ToHashSet())))
            throw new P1RuleException("ROLE_DUPLICATE", roleId);
        Audit("AssignPermissions", roleId, actorId, "platform", null, "SUCCESS");
        return _store.Roles[roleId];
    }

    public IReadOnlyCollection<string> AssignRoles(string companyId, string userId, IEnumerable<string> roleIds, string actorId = "system", long? expectedUserVersion = null)
    {
        if (!_store.Users.TryGetValue(userId, out var user) || user.CompanyId != companyId)
            throw new P1RuleException("USER_NOT_FOUND", userId);
        if (expectedUserVersion is not null && user.Version != expectedUserVersion.Value)
            throw new P1RuleException("CONCURRENCY_CONFLICT", userId);
        var roles = roleIds.Distinct().ToHashSet();
        if (roles.Any(x => !_store.Roles.ContainsKey(x)))
            throw new P1RuleException("ROLE_NOT_FOUND", roles.First(x => !_store.Roles.ContainsKey(x)));
        _store.UserRoles[userId] = roles;
        Audit("AssignPermissions", userId, actorId, companyId, user.BranchId, "SUCCESS");
        return roles.ToArray();
    }

    public P1Setting SaveGlobalSetting(string key, string value, string actorId = "system")
        => SaveGlobalSetting(key, value, null, actorId);

    public P1Setting SaveGlobalSetting(string key, string value, long? expectedVersion, string actorId = "system")
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            throw new P1RuleException("SETTING_SCHEMA_INVALID", key);
        if (_store.GlobalSettings.TryGetValue(key, out var current) && expectedVersion is not null && current.Version != expectedVersion.Value)
            throw new P1RuleException("CONCURRENCY_CONFLICT", key);
        var saved = _store.GlobalSettings.AddOrUpdate(key,
            _ => new P1Setting(key, value),
            (_, old) => old with { Value = value, Version = old.Version + 1 });
        Audit("SaveGlobalSettings", key, actorId, "platform", null, "SUCCESS");
        return saved;
    }

    public P1ScopedSetting SaveScopedSetting(string scopeType, string scopeId, string key, string value, string companyId, string actorId = "system")
        => SaveScopedSetting(scopeType, scopeId, key, value, companyId, null, actorId);

    public P1ScopedSetting SaveScopedSetting(string scopeType, string scopeId, string key, string value, string companyId, long? expectedVersion, string actorId = "system")
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            throw new P1RuleException("SETTING_SCHEMA_INVALID", key);
        if (scopeType.Equals("branch", StringComparison.OrdinalIgnoreCase))
            RequireBranch(companyId, scopeId);
        else if (scopeType.Equals("company", StringComparison.OrdinalIgnoreCase))
            RequireCompany(scopeId);
        else
            throw new P1RuleException("SETTING_SCOPE_INVALID", scopeType);
        var id = $"{scopeType}:{scopeId}:{key}";
        if (_store.ScopedSettings.TryGetValue(id, out var current) && expectedVersion is not null && current.Version != expectedVersion.Value)
            throw new P1RuleException("CONCURRENCY_CONFLICT", id);
        var saved = _store.ScopedSettings.AddOrUpdate(id,
            _ => new P1ScopedSetting(scopeType, scopeId, key, value),
            (_, old) => old with { Value = value, Version = old.Version + 1 });
        Audit("SaveScopedSettings", id, actorId, companyId, scopeType.Equals("branch", StringComparison.OrdinalIgnoreCase) ? scopeId : null, "SUCCESS");
        return saved;
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
        if (branchId is null) throw new P1RuleException("BRANCH_REQUIRED", id);
        RequireBranch(companyId, branchId);
        if (amount <= 0) throw new P1RuleException("AMOUNT_INVALID", id);
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new P1RuleException("PAYMENT_METHOD_INVALID", id);
        if (string.IsNullOrWhiteSpace(reference)) throw new P1RuleException("REFERENCE_REQUIRED", id);
        if (_store.Vouchers.Values.Any(x => x.CompanyId == companyId && x.Reference == reference))
            throw new P1RuleException("DUPLICATE_REFERENCE", reference);
        var voucher = new P1Voucher(id, companyId, branchId, amount, paymentMethod, reference, receipt);
        _store.Vouchers[id] = voucher;
        Audit(receipt ? "CreateReceiptVoucher" : "CreatePaymentVoucher", id, actorId, companyId, branchId, "SUCCESS");
        return voucher;
    }

    public P1Voucher CreateVoucherIdempotent(string companyId, string? branchId, string id, decimal amount, string paymentMethod, string reference, bool receipt, string actorId = "system")
    {
        var existing = _store.Vouchers.Values.SingleOrDefault(x => x.CompanyId == companyId && x.Reference == reference);
        if (existing is not null)
        {
            if (existing.Receipt != receipt || existing.Amount != amount || !string.Equals(existing.PaymentMethod, paymentMethod, StringComparison.OrdinalIgnoreCase))
                throw new P1RuleException("IDEMPOTENCY_PAYLOAD_MISMATCH", reference);
            return existing;
        }
        return CreateVoucher(companyId, branchId, id, amount, paymentMethod, reference, receipt, actorId);
    }

    public P1Voucher UpdateVoucherDraft(string companyId, string voucherId, decimal amount, string paymentMethod, long expectedVersion, string actorId = "system")
    {
        var current = RequireVoucher(companyId, voucherId);
        if (current.Status != P1VoucherState.Draft) throw new P1RuleException("VOUCHER_IMMUTABLE", voucherId);
        if (current.Version != expectedVersion) throw new P1RuleException("CONCURRENCY_CONFLICT", voucherId);
        if (amount <= 0) throw new P1RuleException("AMOUNT_INVALID", voucherId);
        if (string.IsNullOrWhiteSpace(paymentMethod)) throw new P1RuleException("PAYMENT_METHOD_INVALID", voucherId);
        var updated = current with { Amount = amount, PaymentMethod = paymentMethod, Version = current.Version + 1 };
        _store.Vouchers[voucherId] = updated;
        Audit(current.Receipt ? "UpdateReceiptVoucher" : "UpdatePaymentVoucher", voucherId, actorId, companyId, current.BranchId, "SUCCESS");
        return updated;
    }

    public P1Voucher ApproveVoucher(string companyId, string voucherId, string actorId = "system")
    {
        var current = RequireVoucher(companyId, voucherId);
        if (current.Status != P1VoucherState.Draft) throw new P1RuleException("INVALID_STATE_TRANSITION", voucherId);
        var updated = current with { Status = P1VoucherState.Approved, Version = current.Version + 1 };
        _store.Vouchers[voucherId] = updated;
        Audit(current.Receipt ? "ApproveReceiptVoucher" : "ApprovePaymentVoucher", voucherId, actorId, companyId, current.BranchId, "SUCCESS");
        return updated;
    }

    public P1Voucher PostVoucher(string companyId, string voucherId, string actorId = "system")
    {
        var current = RequireVoucher(companyId, voucherId);
        if (current.Status != P1VoucherState.Approved) throw new P1RuleException("INVALID_STATE_TRANSITION", voucherId);
        var updated = current with { Status = P1VoucherState.Posted, Version = current.Version + 1 };
        _store.Vouchers[voucherId] = updated;
        Audit(current.Receipt ? "PostReceiptVoucher" : "PostPaymentVoucher", voucherId, actorId, companyId, current.BranchId, "SUCCESS");
        return updated;
    }

    public P1Voucher CancelVoucher(string companyId, string voucherId, string reason, string actorId = "system")
    {
        if (string.IsNullOrWhiteSpace(reason)) throw new P1RuleException("REASON_REQUIRED", voucherId);
        var current = RequireVoucher(companyId, voucherId);
        if (current.Status == P1VoucherState.Posted) throw new P1RuleException("POSTED_IMMUTABLE", voucherId);
        if (current.Status == P1VoucherState.Cancelled) throw new P1RuleException("INVALID_STATE_TRANSITION", voucherId);
        var updated = current with { Status = P1VoucherState.Cancelled, CancellationReason = reason, Version = current.Version + 1 };
        _store.Vouchers[voucherId] = updated;
        Audit(current.Receipt ? "CancelReceiptVoucher" : "CancelPaymentVoucher", voucherId, actorId, companyId, current.BranchId, "SUCCESS");
        return updated;
    }

    private P1Voucher RequireVoucher(string companyId, string voucherId)
    {
        if (!_store.Vouchers.TryGetValue(voucherId, out var voucher) || voucher.CompanyId != companyId)
            throw new P1RuleException("VOUCHER_NOT_FOUND", voucherId);
        return voucher;
    }

    public IReadOnlyList<P1AuditEvent> ReadAuditEvents(string companyId) => ReadAuditEvents(companyId, null, null, 0, int.MaxValue);

    public IReadOnlyList<P1AuditEvent> ReadAuditEvents(string companyId, string? action, string? branchId, int skip = 0, int take = 100)
    {
        if (skip < 0 || take < 0) throw new P1RuleException("INVALID_FILTER", "paging");
        IEnumerable<P1AuditEvent> query;
        lock (_auditLock) query = _store.AuditSequence.Where(x => x.CompanyId == companyId).ToArray();
        if (!string.IsNullOrWhiteSpace(action)) query = query.Where(x => x.Action == action);
        if (!string.IsNullOrWhiteSpace(branchId)) query = query.Where(x => x.BranchId == branchId);
        return query.Skip(skip).Take(take).ToArray();
    }

    public bool VerifyAuditHashChain(string? companyId = null)
    {
        P1AuditEvent[] events;
        lock (_auditLock) events = _store.AuditSequence.ToArray();
        string? previousHash = null;
        foreach (var auditEvent in events)
        {
            if (!string.Equals(auditEvent.PreviousHash, previousHash, StringComparison.Ordinal)) return false;
            if (!string.Equals(auditEvent.Hash, ComputeAuditHash(auditEvent, previousHash), StringComparison.Ordinal)) return false;
            previousHash = auditEvent.Hash;
        }
        return companyId is null || events.Any(x => x.CompanyId == companyId);
    }

    public IReadOnlyList<P1AuditEvent> ExportAuditEvents(string companyId, int skip = 0, int take = 100)
        => ReadAuditEvents(companyId, null, null, skip, take);

    public P1SyncOperation EnqueueSyncOperation(P1SyncOperation operation, string actorId = "sync")
    {
        ValidateSyncOperation(operation);
        var queued = operation with { Status = P1SyncStatus.Queued, ErrorCode = null, Version = 1 };
        if (!_store.SyncOperations.TryAdd(SyncKey(operation), queued))
            throw new P1RuleException("DUPLICATE_OPERATION", operation.ClientOperationId);
        Audit("SyncOperationQueued", operation.ClientOperationId, actorId, operation.CompanyId, operation.BranchId, "SUCCESS");
        return queued;
    }

    public P1SyncOperation TransitionSyncOperation(string clientOperationId, P1SyncStatus newStatus, string actorId = "sync")
        => TransitionSyncOperation(FindSyncOperation(clientOperationId), newStatus, actorId);

    public P1SyncOperation TransitionSyncOperation(string deviceId, string clientOperationId, P1SyncStatus newStatus, string actorId = "sync")
    {
        if (!_store.SyncOperations.TryGetValue($"{deviceId}:{clientOperationId}", out var current))
            throw new P1RuleException("OPERATION_NOT_FOUND", clientOperationId);
        return TransitionSyncOperation(current, newStatus, actorId);
    }

    private P1SyncOperation TransitionSyncOperation(P1SyncOperation current, P1SyncStatus newStatus, string actorId)
    {
        if (current.Status == newStatus) return current;
        if (!IsAllowedSyncTransition(current.Status, newStatus))
            throw new P1RuleException("INVALID_STATE_TRANSITION", current.ClientOperationId);
        var updated = current with { Status = newStatus, Version = current.Version + 1, ErrorCode = newStatus == P1SyncStatus.Succeeded ? null : current.ErrorCode };
        _store.SyncOperations[SyncKey(current)] = updated;
        Audit("SyncOperationTransition", current.ClientOperationId, actorId, current.CompanyId, current.BranchId, "SUCCESS", newStatus.ToString().ToUpperInvariant());
        return updated;
    }

    public P1SyncOperation RetrySyncOperation(string clientOperationId, string actorId = "sync")
    {
        var current = FindSyncOperation(clientOperationId);
        if (current.Status != P1SyncStatus.Failed && current.Status != P1SyncStatus.Conflict)
            throw new P1RuleException("RETRY_NOT_ALLOWED", clientOperationId);
        var updated = current with
        {
            Status = P1SyncStatus.Sending,
            RetryCount = current.RetryCount + 1,
            NextRetryAt = DateTimeOffset.UtcNow,
            ErrorCode = null,
            Version = current.Version + 1
        };
        _store.SyncOperations[SyncKey(current)] = updated;
        Audit("SyncOperationRetry", clientOperationId, actorId, current.CompanyId, current.BranchId, "SUCCESS");
        return updated;
    }

    public P1SyncOperation ResolveSyncConflict(string clientOperationId, string resolution, string actorId = "sync")
    {
        if (string.IsNullOrWhiteSpace(resolution)) throw new P1RuleException("RESOLUTION_REQUIRED", clientOperationId);
        var current = FindSyncOperation(clientOperationId);
        if (current.Status != P1SyncStatus.Conflict) throw new P1RuleException("CONFLICT_NOT_FOUND", clientOperationId);
        var updated = current with { Status = P1SyncStatus.Resolved, ErrorCode = null, Version = current.Version + 1 };
        _store.SyncOperations[SyncKey(current)] = updated;
        Audit("SyncOperationConflictResolved", clientOperationId, actorId, current.CompanyId, current.BranchId, "SUCCESS", resolution);
        return updated;
    }

    public P1SyncOperation GetSyncOperation(string clientOperationId) => FindSyncOperation(clientOperationId);

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
            var key = SyncKey(op);
            if (_store.SyncOperations.TryGetValue(key, out var prior))
            {
                if (prior.PayloadHash == op.PayloadHash)
                {
                    results.Add(new P1SyncResult(op.ClientOperationId, "DUPLICATE_ACCEPTED"));
                    continue;
                }
                var conflicted = prior with { Status = P1SyncStatus.Conflict, ErrorCode = "HASH_MISMATCH", Version = prior.Version + 1 };
                _store.SyncOperations[key] = conflicted;
                Audit("SyncOperationConflict", op.ClientOperationId, actorId, prior.CompanyId, prior.BranchId, "CONFLICT", "HASH_MISMATCH");
                results.Add(new P1SyncResult(op.ClientOperationId, "CONFLICT", "CONFLICT"));
                continue;
            }
            var queued = EnqueueSyncOperation(op, actorId);
            var sending = TransitionSyncOperation(queued.DeviceId, queued.ClientOperationId, P1SyncStatus.Sending, actorId);
            _ = TransitionSyncOperation(sending.DeviceId, sending.ClientOperationId, P1SyncStatus.Succeeded, actorId);
            results.Add(new P1SyncResult(op.ClientOperationId, "ACCEPTED"));
        }
        return results;
    }

    private static string SyncKey(P1SyncOperation operation) => $"{operation.DeviceId}:{operation.ClientOperationId}";

    private P1SyncOperation FindSyncOperation(string clientOperationId)
    {
        var matches = _store.SyncOperations.Values.Where(x => x.ClientOperationId == clientOperationId).ToArray();
        if (matches.Length == 0) throw new P1RuleException("OPERATION_NOT_FOUND", clientOperationId);
        if (matches.Length > 1) throw new P1RuleException("OPERATION_DEVICE_REQUIRED", clientOperationId);
        return matches[0];
    }

    private static void ValidateSyncOperation(P1SyncOperation operation)
    {
        if (string.IsNullOrWhiteSpace(operation.DeviceId) || string.IsNullOrWhiteSpace(operation.ClientOperationId) || string.IsNullOrWhiteSpace(operation.PayloadHash))
            throw new P1RuleException("PAYLOAD_INVALID", operation.ClientOperationId);
        if (string.IsNullOrWhiteSpace(operation.CompanyId)) throw new P1RuleException("SCOPE_DENIED", operation.ClientOperationId);
    }

    private static bool IsAllowedSyncTransition(P1SyncStatus from, P1SyncStatus to) =>
        (from, to) switch
        {
            (P1SyncStatus.Queued, P1SyncStatus.Sending) => true,
            (P1SyncStatus.Sending, P1SyncStatus.Succeeded or P1SyncStatus.Failed or P1SyncStatus.Conflict or P1SyncStatus.Rejected) => true,
            (P1SyncStatus.Failed, P1SyncStatus.Sending) => true,
            (P1SyncStatus.Conflict, P1SyncStatus.Resolved) => true,
            _ => false
        };

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

    private void Audit(string action, string entityId, string actorId, string companyId, string? branchId, string outcome, string? reason = null)
    {
        lock (_auditLock)
        {
            var previousHash = _store.AuditSequence.LastOrDefault()?.Hash;
            var auditEvent = new P1AuditEvent(Guid.NewGuid().ToString("N"), action, entityId, actorId, companyId, branchId, Guid.NewGuid().ToString("N"), DateTimeOffset.UtcNow, outcome, "", previousHash, reason);
            var hashed = auditEvent with { Hash = ComputeAuditHash(auditEvent, previousHash) };
            _store.AuditSequence.Add(hashed);
            _store.AuditEvents.Add(hashed);
        }
    }

    private static string ComputeAuditHash(P1AuditEvent auditEvent, string? previousHash)
    {
        var canonical = string.Join("|", auditEvent.Id, auditEvent.Action, auditEvent.EntityId, auditEvent.ActorId,
            auditEvent.CompanyId, auditEvent.BranchId ?? "", auditEvent.CorrelationId, auditEvent.At.ToUniversalTime().ToString("O"),
            auditEvent.Outcome, auditEvent.Reason ?? "", previousHash ?? "");
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();
    }
}
