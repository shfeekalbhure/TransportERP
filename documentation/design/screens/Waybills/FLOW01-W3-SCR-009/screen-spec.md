# FLOW01-W3-SCR-009 — أمر التوصيل — Canonical Screen Specification

**Alias:** `DLV-001`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-02`

## Authority
- Current FLOW01 owner issuance `SRC-053`; historical generic `Transaction` Variant gate is superseded by current `HeaderLines` typed definition.
- W1: `DeliveryOrder`, `DeliveryTask`, `DispatchAssignment`.
- W2: `F01.DeliveryOrder.Create`, `Update`, `Cancel`, `Get/Search`.
- Permissions: `f01.delivery.order.create`, `f01.delivery.order.edit`, `f01.delivery.order.cancel`, `f01.delivery.order.view`.
- Typed definition: `FLOW01-W3-SCR-009_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-008` (issued; runtime not run).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-02_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain a delivery order for eligible ready items/packages, assign tasks within authorized scope, support controlled cancellation, and expose scoped read/search.

Fields:
- `deliveryOrderId` — رقم أمر التوصيل — UUID/read-only/server assigned.
- `itemOrPackageRefs` — البنود/الطرود — LookupMulti/UUID Collection — required — ready/no active order.
- `assigneeRef` — المكلف — Lookup/Reference — conditional — permitted scope.
- `deliveryState` — الحالة — Enum/read-only/server state.
- `taskLines` — مهام التوصيل — LineGrid/Collection — editable only in issued Draft/Assigned state.
- `cancelReason` — سبب الإلغاء — String conditional — required on cancel.
- `expectedVersion` — hidden Integer/read-only — update/cancel.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- DeliveryTasksGrid = `Fill`.
- actions/audit/validation/loading/error = shared CoreUI.
- tabs: `General | Tasks & Items | Audit`.
- no LocalException/local style.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Required | Edit policy | Width / editor |
|---:|---|---|---|---|---|---|
| 1 | `taskRef` | المهمة | Reference | No | ReadOnly/server identity | content/reference |
| 2 | `itemOrPackageRef` | البند/الطرد | Reference | Yes | Draft/Assigned | content / `TransportLookup` |
| 3 | `destination` | الوجهة | Reference | Yes | ReadOnly display | content/reference |
| 4 | `assignee` | المكلف | Reference | Conditional | Draft/Assigned | content / `TransportLookup` |
| 5 | `taskState` | حالة المهمة | Enum | Yes | ReadOnly/server state | content state |
| 6 | `scheduledAt` | الموعد | Instant | No | Draft/Assigned | content datetime |

Exact lookup providers/server sort keys remain `TBD-GATED` and nonblocking.

## UX — TEAM-D04 PASS
- create requires eligible ready item/package and no active order; UI does not infer availability authority.
- edit only in issued Draft/Assigned state with permission and `expectedVersion`.
- cancel requires issued cancel permission/state and reason; cancellation result comes from server.
- no local restoration formula for ready state.
- loading prevents duplicate state-changing submissions.
- concurrency conflict uses shared Reload/Refresh; no silent overwrite.
- permission/scope denial leaks no hidden item/task data.
- all writes online-only.

## VISUAL — TEAM-D05 PASS
Shared Transaction CoreUI visual system only: RTL/DPI, central typography/spacing/state styling, tabs, task grid, lookup editors, action/error/audit presenters. No raw/local style.

## Independent review — TEAM-D06 PASS
Review report: `documentation/design/reviews/2026-08-24_BATCH-02_INDEPENDENT_REVIEW.md`. Open design findings: `0`. Runtime `TAE-F01-008` not run.

## Acceptance criteria
1. current `HeaderLines` profile/variant is used.
2. six explicit task columns and single-row selection.
3. state/eligibility/cancel restoration remain server authority.
4. no API/DTO/Permission/DDL/offline invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`, `INDEPENDENT_REVIEW`.  
Final: `DESIGN_APPROVED`.
