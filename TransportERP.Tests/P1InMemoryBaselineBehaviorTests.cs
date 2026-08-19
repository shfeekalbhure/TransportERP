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
}
