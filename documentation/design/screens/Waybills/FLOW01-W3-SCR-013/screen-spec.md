# FLOW01-W3-SCR-013 — تسوية التحصيل — Canonical Screen Specification

**Alias:** `COD-003`  
**Profile / Variant:** `Transaction / Reconciliation`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-02`

## Authority
- Current FLOW01 owner issuance `SRC-053`; historical generic Variant gate is superseded by current `Transaction / Reconciliation` typed definition.
- W1: `DriverSettlement`, `ReversalSettlementReference`, `FinancialSourceLink`.
- W2: `F01.DriverSettlement.Submit`, `Approve`, `Reverse`, `Get/Search`.
- Permissions: `f01.cash.settlement.submit`, `f01.cash.settlement.approve`, `f01.cash.settlement.reverse`, `f01.cash.settlement.view`.
- SoD: submitter cannot approve own settlement.
- Typed definition: `FLOW01-W3-SCR-013_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-012` (issued; runtime not run).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-02_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: reconcile expected versus actual driver settlement lines, require the issued financial-document source link, support submit/approve/reverse under SoD and controlled reversal rules, and expose scoped read/search without posting accounting entries from this screen.

Fields:
- `settlementId` — رقم التسوية — UUID/read-only/server assigned.
- `driverRef` — السائق — Lookup/Reference — required/scope valid.
- `financialDocumentRef` — المرجع المالي — Lookup/Reference — required prerequisite; no posting here.
- `settlementLines` — عناصر المطابقة — ReconciliationGrid/Collection — required.
- `expectedAmount` — المتوقع — Decimal Currency — required/read-only/server calculated.
- `actualAmount` — الفعلي — Decimal Currency — required/editable under issued pre-approval state.
- `varianceAmount` — الفرق — Decimal Currency — required/read-only/server calculated.
- `approvalState` — حالة الاعتماد — Enum/read-only/server state.
- `reason` — السبب — String conditional for variance/reversal.
- `expectedVersion` — hidden Integer/read-only — submit/approve/reverse.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / Reconciliation` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- SettlementLinesGrid = `Fill`.
- expected/actual/variance summary = shared `Content` region.
- approval/reversal action/reason/audit regions = shared CoreUI Content/Fixed behavior.
- tabs: `Reconciliation | Financial Link | Exceptions | Audit`.
- no LocalException/local style.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines`, `GridVariant=Reconciliation`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Required | Edit policy | Width / editor |
|---:|---|---|---|---|---|---|
| 1 | `reference` | المرجع | Reference | Yes | ReadOnly/server source | primary Fill |
| 2 | `expectedAmount` | المتوقع | Decimal | Yes | ReadOnly/server calculated | content numeric |
| 3 | `actualAmount` | الفعلي | Decimal | Yes | Editable before controlled submit/approval boundary | content numeric |
| 4 | `variance` | الفرق | Decimal | Yes | ReadOnly/server calculated | content numeric |
| 5 | `disposition` | المعالجة | Enum | Conditional | Editable when issued reconciliation state permits | content / enum editor |
| 6 | `evidenceRef` | الدليل | Reference | No | Editable when evidence is supplied for the issued reconciliation context | content/reference |

Exact provider/sort identifiers remain `TBD-GATED`; no financial formula or posting contract is invented.

## UX — TEAM-D04 PASS
- submit requires issued permission, financial document reference and valid settlement data.
- approve requires separate approver permission and server-enforced `submitter ≠ approver` SoD.
- reverse is controlled and reason/audit bound; no delete or silent rollback.
- expected/variance values remain server-authoritative; client does not recompute financial authority.
- loading prevents duplicate submit/approve/reverse.
- `expectedVersion` conflict uses shared Reload/Refresh and never silently overwrites.
- permission/scope/SoD denial reveals no hidden financial data.
- no journal posting, revenue recognition or accounting write is implied by settlement approval.
- all writes online-only.

## VISUAL — TEAM-D05 PASS
Shared Transaction/Reconciliation CoreUI only: RTL/DPI, central currency/state/variance presentation, reconciliation grid, shared actions/reason/validation/loading/error/audit. No local financial colors, raw dimensions or custom approval toolbar.

## Independent review — TEAM-D06 PASS
Review report: `documentation/design/reviews/2026-08-24_BATCH-02_INDEPENDENT_REVIEW.md`. Open design findings: `0`. Runtime `TAE-F01-012` not run.

## Acceptance criteria
1. current Variant is `Reconciliation`.
2. six explicit reconciliation columns; expected/variance remain read-only server results.
3. submit/approve/reverse are permission/state/version bound and SoD is preserved.
4. no posting, API/DTO/Permission/DDL/offline invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`, `INDEPENDENT_REVIEW`.  
Final: `DESIGN_APPROVED`.
