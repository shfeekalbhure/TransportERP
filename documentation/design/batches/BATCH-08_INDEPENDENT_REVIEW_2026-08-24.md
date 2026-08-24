# BATCH-08 — Independent Design Review

Date: 2026-08-24
Reviewer: TEAM-D06
Verdict: PASS
Open design findings: 0
Runtime: NOT RUN

Reviewed:
- ACC-035 — دليل الحسابات
- ACC-037 — مراكز التكلفة

## Review gates
1. Both are `TreeMaster / Standard`, not flat MasterData grids.
2. Shared TreeMaster host, SingleRow, lazy children and server paging are preserved; no local tree/grid/pagination/audit clone.
3. CompanyId is Create-only and immutable afterward.
4. Parent is Create-time optional/null-root; Update cannot change parent; Move is the only reparent action and requires expectedVersion.
5. Self/descendant move maps to `HIERARCHY_CYCLE`; invalid parent maps to `INVALID_PARENT`; server remains authority.
6. ACC-035 consumes existing AccountGroup/AccountType selector authority only; no ACC036 route/DTO is invented.
7. ACC-035 IsPostable stays a simple issued boolean without leaf/posting inference; CurrencyPolicy remains hidden/null-only; no AccountBranchScope surface.
8. ACC-037 BranchId is optional and governed only by current reference/scope validation; no extra cross-company compatibility rule is invented.
9. No CostCenter statement, JournalLine/posting UX, Delete/Enable/Print/Export/Attachments/Offline/Queue.
10. Disable uses reason + expectedVersion; shared concurrency/error presenters preserved.

Acceptance tests confirm root/child create, lazy children, Move-to-root/reparent, cycle/invalid-parent rejection and security boundaries. Design PASS does not claim runtime/API/database PASS.
