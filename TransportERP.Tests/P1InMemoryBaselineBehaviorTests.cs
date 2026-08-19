using TransportERP.Application.P1Baseline;
using Xunit;

namespace TransportERP.Tests;

public sealed class P1InMemoryBaselineBehaviorTests
{
    private static (P1InMemoryService Service, string CompanyId, string BranchId, string PeriodId, string CashId, string RevenueId) Seed()
    {
        var service = new P1InMemoryService();
        var company = service.CreateCompany("C-001", "C001", "شركة الاختبار");
        var branch = service.CreateBranch(company.Id, "B-001", "B001", "الفرع الرئيسي");
        service.CreateUser(company.Id, branch.Id, "U-001", "owner", "secret");
        var cash = service.CreateAccount(company.Id, "A-001", "1110", "الصندوق", null, true);
        var revenue = service.CreateAccount(company.Id, "A-002", "4110", "الإيراد", null, true);
        var period = service.CreatePeriod(company.Id, "P-001", new DateOnly(2026, 1, 1), new DateOnly(2027, 1, 1));
        return (service, company.Id, branch.Id, period.Id, cash.Id, revenue.Id);
    }

    [Fact]
    public void Authenticate_creates_scoped_session_and_audit_event()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var session = service.Authenticate("owner", "secret", new P1Scope(companyId, branchId));
        Assert.Equal(companyId, session.Scope.CompanyId);
        Assert.Equal(branchId, session.Scope.BranchId);
        Assert.Contains(service.ReadAuditEvents(companyId), e => e.Action == "Authenticate" && e.Outcome == "SUCCESS");
    }

    [Fact]
    public void Journal_requires_balanced_lines_and_postable_accounts()
    {
        var (service, companyId, branchId, periodId, cashId, revenueId) = Seed();
        var entry = service.CreateJournal(companyId, branchId, periodId, "J-001", "op-001", "INV-001", new[]
        {
            new P1JournalLine(cashId, 100m, 0m),
            new P1JournalLine(revenueId, 0m, 100m)
        });
        Assert.Equal(P1JournalState.Checked, entry.State);
        Assert.Throws<P1RuleException>(() => service.CreateJournal(companyId, branchId, periodId, "J-002", "op-002", "INV-002", new[] { new P1JournalLine(cashId, 100m, 0m) }));
    }

    [Fact]
    public void Journal_client_operation_is_idempotent()
    {
        var (service, companyId, branchId, periodId, cashId, revenueId) = Seed();
        var lines = new[] { new P1JournalLine(cashId, 50m, 0m), new P1JournalLine(revenueId, 0m, 50m) };
        var first = service.CreateJournal(companyId, branchId, periodId, "J-001", "same-op", "INV-001", lines);
        var second = service.CreateJournal(companyId, branchId, periodId, "J-002", "same-op", "INV-001", lines);
        Assert.Equal(first.Id, second.Id);
    }

    [Fact]
    public void Posted_journal_is_immutable_until_reversal()
    {
        var (service, companyId, branchId, periodId, cashId, revenueId) = Seed();
        service.CreateJournal(companyId, branchId, periodId, "J-001", "op-001", "INV-001", new[] { new P1JournalLine(cashId, 100m, 0m), new P1JournalLine(revenueId, 0m, 100m) });
        var posted = service.PostJournal(companyId, "J-001");
        Assert.Equal(P1JournalState.Posted, posted.State);
        var reversal = service.ReverseJournal(companyId, "J-001", "تصحيح القيد");
        Assert.Equal("J-001", reversal.ReversalOf);
        Assert.Contains(service.ReadAuditEvents(companyId), e => e.Action == "ReverseJournalEntry");
    }

    [Fact]
    public void Closing_period_with_draft_journal_is_blocked()
    {
        var (service, companyId, branchId, periodId, cashId, revenueId) = Seed();
        service.CreateJournal(companyId, branchId, periodId, "J-001", "op-001", "INV-001", new[] { new P1JournalLine(cashId, 100m, 0m), new P1JournalLine(revenueId, 0m, 100m) });
        Assert.Throws<P1RuleException>(() => service.SetPeriodState(companyId, periodId, P1PeriodState.Closed));
    }

    [Fact]
    public void Sync_accepts_first_operation_repeats_same_hash_and_flags_conflict()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var op = new P1SyncOperation("D-001", "client-001", "hash-a", "CreateJournalEntry", companyId, branchId, "0");
        Assert.Equal("ACCEPTED", service.SyncBatch(new[] { op })[0].Status);
        Assert.Equal("DUPLICATE_ACCEPTED", service.SyncBatch(new[] { op })[0].Status);
        var changed = op with { PayloadHash = "hash-b" };
        Assert.Equal("CONFLICT", service.SyncBatch(new[] { changed })[0].Status);
    }

    [Fact]
    public void User_update_requires_matching_version_and_role_assignment_requires_existing_role()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var updated = service.UpdateUser(companyId, "U-001", "owner-renamed", true, 1);
        Assert.Equal(2, updated.Version);
        Assert.Throws<P1RuleException>(() => service.UpdateUser(companyId, "U-001", "owner-again", true, 1));
        service.RegisterRole("R-ADMIN", "مدير النظام");
        Assert.Contains("R-ADMIN", service.AssignRoles(companyId, "U-001", new[] { "R-ADMIN" }));
        Assert.Throws<P1RuleException>(() => service.AssignRoles(companyId, "U-001", new[] { "R-MISSING" }));
    }

    [Fact]
    public void Organization_role_permissions_and_version_conflicts_are_enforced()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var updatedBranch = service.UpdateBranch(companyId, branchId, "B001-UPDATED", "فرع محدث", true, 1);
        Assert.Equal(2, updatedBranch.Version);
        Assert.Throws<P1RuleException>(() => service.UpdateBranch(companyId, branchId, "B001-STALE", "فرع قديم", true, 1));
        var role = service.RegisterRole("R-OPS", "مشغل", new[] { "settings.write", "audit.read" });
        Assert.Contains("settings.write", role.Permissions);
        var user = service.UpdateUser(companyId, "U-001", "owner-renamed", true, 1);
        Assert.Contains("R-OPS", service.AssignRoles(companyId, "U-001", new[] { "R-OPS" }, "system", user.Version));
        Assert.Throws<P1RuleException>(() => service.AssignRoles(companyId, "U-001", new[] { "R-OPS" }, "system", 1));
    }

    [Fact]
    public void Settings_require_matching_versions_for_updates()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var global = service.SaveGlobalSetting("default.currency", "YER");
        var globalUpdated = service.SaveGlobalSetting("default.currency", "USD", global.Version);
        Assert.Equal(2, globalUpdated.Version);
        Assert.Throws<P1RuleException>(() => service.SaveGlobalSetting("default.currency", "SAR", global.Version));
        var scoped = service.SaveScopedSetting("branch", branchId, "cash.policy", "CASH", companyId);
        var scopedUpdated = service.SaveScopedSetting("branch", branchId, "cash.policy", "BANK", companyId, scoped.Version);
        Assert.Equal(2, scopedUpdated.Version);
        Assert.Throws<P1RuleException>(() => service.SaveScopedSetting("branch", branchId, "cash.policy", "CREDIT", companyId, scoped.Version));
    }

    [Fact]
    public void Audit_query_supports_action_branch_and_paging_filters()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        service.SaveGlobalSetting("default.currency", "YER", "actor-a");
        service.SaveScopedSetting("branch", branchId, "cash.policy", "CASH", companyId, "actor-b");
        var settings = service.ReadAuditEvents(companyId, "SaveScopedSettings", branchId, 0, 10);
        Assert.Single(settings);
        Assert.Equal("actor-b", settings[0].ActorId);
        Assert.Empty(service.ReadAuditEvents(companyId, "SaveGlobalSettings", branchId, 0, 10));
        Assert.Throws<P1RuleException>(() => service.ReadAuditEvents(companyId, null, null, -1, 10));
    }

    [Fact]
    public void Settings_support_global_and_company_or_branch_scope_and_reject_invalid_scope()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var global = service.SaveGlobalSetting("default.currency", "YER");
        Assert.Equal("YER", global.Value);
        var company = service.SaveScopedSetting("company", companyId, "timezone", "Asia/Aden", companyId);
        Assert.Equal(companyId, company.ScopeId);
        var branch = service.SaveScopedSetting("branch", branchId, "cash.policy", "CASH", companyId);
        Assert.Equal(branchId, branch.ScopeId);
        Assert.Throws<P1RuleException>(() => service.SaveScopedSetting("tenant", companyId, "x", "y", companyId));
    }

    [Fact]
    public void Voucher_rejects_non_positive_amount_and_duplicate_reference()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        Assert.Throws<P1RuleException>(() => service.CreateVoucher(companyId, branchId, "R-001", 0m, "CASH", "REF-001", true));
        service.CreateVoucher(companyId, branchId, "R-001", 25m, "CASH", "REF-001", true);
        Assert.Throws<P1RuleException>(() => service.CreateVoucher(companyId, branchId, "R-002", 25m, "CASH", "REF-001", true));
    }

    [Fact]
    public void Voucher_lifecycle_is_draft_approved_posted_and_posted_is_immutable()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var draft = service.CreateVoucher(companyId, branchId, "R-010", 100m, "CASH", "EXT-010", true);
        Assert.Equal(P1VoucherState.Draft, draft.Status);
        var approved = service.ApproveVoucher(companyId, draft.Id);
        Assert.Equal(P1VoucherState.Approved, approved.Status);
        var posted = service.PostVoucher(companyId, draft.Id);
        Assert.Equal(P1VoucherState.Posted, posted.Status);
        Assert.Throws<P1RuleException>(() => service.UpdateVoucherDraft(companyId, draft.Id, 120m, "BANK", posted.Version));
        Assert.Throws<P1RuleException>(() => service.CancelVoucher(companyId, draft.Id, "محاولة إلغاء مرحّل"));
    }

    [Fact]
    public void Voucher_idempotency_returns_same_draft_and_rejects_changed_payload()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var first = service.CreateVoucherIdempotent(companyId, branchId, "R-011", 75m, "CASH", "EXT-011", false);
        var repeated = service.CreateVoucherIdempotent(companyId, branchId, "R-012", 75m, "CASH", "EXT-011", false);
        Assert.Equal(first.Id, repeated.Id);
        Assert.Throws<P1RuleException>(() => service.CreateVoucherIdempotent(companyId, branchId, "R-013", 76m, "CASH", "EXT-011", false));
    }

    [Fact]
    public void Audit_events_are_append_only_and_hash_chain_verifies()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        service.SaveGlobalSetting("audit.test", "one");
        service.SaveScopedSetting("branch", branchId, "audit.test", "two", companyId);
        var events = service.ExportAuditEvents(companyId);
        Assert.NotEmpty(events);
        Assert.All(events, e => Assert.False(string.IsNullOrWhiteSpace(e.Hash)));
        Assert.True(service.VerifyAuditHashChain(companyId));
    }

    [Fact]
    public void Sync_operation_supports_lifecycle_retry_and_conflict_resolution()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var queued = service.EnqueueSyncOperation(new P1SyncOperation("D-002", "client-002", "hash-002", "UPDATE", companyId, branchId, "1"));
        Assert.Equal(P1SyncStatus.Queued, queued.Status);
        var sending = service.TransitionSyncOperation(queued.ClientOperationId, P1SyncStatus.Sending);
        var failed = service.TransitionSyncOperation(sending.ClientOperationId, P1SyncStatus.Failed);
        var retried = service.RetrySyncOperation(failed.ClientOperationId);
        Assert.Equal(1, retried.RetryCount);
        var conflict = service.TransitionSyncOperation(retried.ClientOperationId, P1SyncStatus.Conflict);
        var resolved = service.ResolveSyncConflict(conflict.ClientOperationId, "USE_SERVER_VERSION");
        Assert.Equal(P1SyncStatus.Resolved, resolved.Status);
    }

    [Fact]
    public void Sync_operation_rejects_invalid_transitions()
    {
        var (service, companyId, branchId, _, _, _) = Seed();
        var queued = service.EnqueueSyncOperation(new P1SyncOperation("D-003", "client-003", "hash-003", "UPDATE", companyId, branchId, "1"));
        Assert.Throws<P1RuleException>(() => service.TransitionSyncOperation(queued.ClientOperationId, P1SyncStatus.Succeeded));
    }

    [Fact]
    public void W3_journal_screen_state_enforces_workflow_and_permission()
    {
        var (service, _, _, _, _, _) = Seed();
        Assert.Equal(P1ScreenPhase.Loading, service.InitializeScreen("W3-P1-009").Phase);
        service.TransitionScreen("W3-P1-009", P1ScreenPhase.Ready);
        service.RequireScreenPermission("W3-P1-009", "create", new HashSet<string> { "accounting.journal.create" }, true);
        service.TransitionScreen("W3-P1-009", P1ScreenPhase.DraftEditing);
        service.TransitionScreen("W3-P1-009", P1ScreenPhase.Checked);
        service.TransitionScreen("W3-P1-009", P1ScreenPhase.Approved);
        service.TransitionScreen("W3-P1-009", P1ScreenPhase.Posted);
        Assert.Throws<P1RuleException>(() => service.TransitionScreen("W3-P1-009", P1ScreenPhase.DraftEditing));
        Assert.Throws<P1RuleException>(() => service.RequireScreenPermission("W3-P1-009", "post", new HashSet<string> { "accounting.journal.post" }, false));
    }

    [Fact]
    public void W3_sync_screen_state_supports_offline_retry_resolution_and_permission()
    {
        var (service, _, _, _, _, _) = Seed();
        service.InitializeScreen("W3-P1-012");
        service.TransitionScreen("W3-P1-012", P1ScreenPhase.Offline, isOffline: true);
        service.TransitionScreen("W3-P1-012", P1ScreenPhase.Ready, isOffline: true);
        service.RequireScreenPermission("W3-P1-012", "retry", new HashSet<string> { "sync.operations.execute" }, true);
        service.TransitionScreen("W3-P1-012", P1ScreenPhase.Retrying);
        var resolved = service.TransitionScreen("W3-P1-012", P1ScreenPhase.Resolved);
        Assert.Equal(P1ScreenPhase.Resolved, resolved.Phase);
        Assert.Throws<P1RuleException>(() => service.RequireScreenPermission("W3-P1-012", "resolve", new HashSet<string> { "sync.conflicts.resolve" }, false));
    }
}
