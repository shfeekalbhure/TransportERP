# BATCH-08 — Account and Cost Center Tree Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY CONTINUATION

Scope:
- ACC-035 — دليل الحسابات / Account Tree
- ACC-037 — مراكز التكلفة / Cost Center Tree

Authority:
- `SRC-061 / OWNER-ACCOUNT-COSTCENTER-TREE-W1-W2-W3-ISSUANCE-001`.
- Current `TreeMaster / Standard` and CoreUI contracts.

Boundaries:
- Both screens are real TreeMaster surfaces with lazy paged children, SingleRow selection, Create/Edit/Disable/Move and server paging.
- CompanyId is Create-only and immutable afterward.
- Parent is optional on Create, immutable through Update and changed only by Move; null means root.
- Move requires expectedVersion; self/descendant move is `HIERARCHY_CYCLE`, invalid parent is `INVALID_PARENT`.
- ACC-035 AccountGroup/AccountType selectors consume existing ACC036 reference authority only; no new lookup route or ACC036 contract is invented.
- ACC-035 `IsPostable` is the issued boolean only; no leaf/posting semantic is inferred. `CurrencyPolicy` is hidden/null-only; no currency control.
- ACC-035 has no AccountBranchScope tab/grid/action.
- ACC-037 BranchId is optional Create/Edit and validated by current reference/scope only; no unissued cross-company compatibility rule.
- No CostCenter statement/JournalLine/posting UX is created.
- No Delete/Enable/Print/Export/Attachments/Offline/Queue; no numeric default page size/cap invention.

No application code, DDL, API/DTO/permission contract, or official kurrasa is modified.
