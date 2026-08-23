# FLOW01-W3-SCR-006 — الترانزيت وتسليم الحيازة — Canonical Screen Specification

**Alias:** `WHS-003`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `CurrentCustodyOwner`, `CustodyHandoff`, `PackageHandlingEvent`.
- W2: `F01.CustodyHandoff.Confirm`, `F01.HubHandling.Get`.
- Permissions: `f01.custody.handoff.confirm`, `f01.hub.handling.view`.
- Typed definition: `FLOW01-W3-SCR-006_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-005` (issued; runtime PASS not claimed).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-01_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: confirm a custody handoff for one or more packages and display immutable handling history inside authorized scope.

Fields:
- `packageRefs` — الطرود — LookupMulti / UUID Collection — required — one current owner.
- `sourceCustodyRef` — الحيازة المصدر — Reference/read-only — server resolved.
- `targetCustodyRef` — الحيازة المستهدفة — Lookup/Reference — required — allowed scope.
- `receiptConfirmedBy` — تأكيد المستلم — SignatureRef/Reference — required — distinct confirmation.
- `handoffReason` — سبب الانتقال — TextArea/String — required/nonempty.
- `handlingHistory` — سجل التعامل — ReadGrid/Collection — immutable display.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- history/handoff grid workspace = `Fill`.
- actions/audit/shared presenters = CoreUI.
- tabs: `Handoff | Handling History | Audit`.
- no LocalException or local sizing/style.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines/Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true` as issued, `SelectionPolicy=SingleRow`. The history grid is immutable/read-only.

| # | Key | Arabic Header | UI Type | Edit | Width policy |
|---:|---|---|---|---|---|
| 1 | `packageRef` | الطرد | Reference | ReadOnly | content/reference |
| 2 | `sourceCustody` | الحيازة المصدر | Reference | ReadOnly | content/reference |
| 3 | `targetCustody` | الحيازة المستهدفة | Reference | ReadOnly | content/reference |
| 4 | `confirmedAt` | وقت التأكيد | Instant | ReadOnly | content datetime |
| 5 | `receiver` | المستلم | Reference | ReadOnly | content/reference |
| 6 | `reason` | السبب | String | ReadOnly | primary Fill |
| 7 | `eventState` | حالة الحدث | Enum | ReadOnly | content state |

Input interaction remains in the issued header fields (`packageRefs`, `targetCustodyRef`, `receiptConfirmedBy`, `handoffReason`); the history grid does not become an edit surface.

## UX — TEAM-D04 PASS
- source custody is displayed from the server and never asserted or recalculated by the client.
- confirm is available only with issued permission/state, selected subjects/target, distinct receipt confirmation and nonempty reason.
- while confirming, shared loading state prevents duplicate submit/conflicting mutation.
- client does not change visible current custody until the successful server response is returned.
- validation uses shared presenter; permission/scope denial leaks no hidden custody/package data.
- handling history remains immutable; refresh reloads server-authoritative history.
- no offline handoff/queue is exposed.

## VISUAL — TEAM-D05 PASS
- CoreUI Transaction visual tokens only: RTL, DPI, central typography/spacing, lookup/signature field states, shared grid/history rendering, validation/loading/error/audit.
- no local colors, heights, fonts, widths or handoff-specific toolbar clone.

## Acceptance criteria
1. one current custody owner remains server authority.
2. separate receiver confirmation is required by the issued contract.
3. source custody is read-only/server resolved.
4. history grid has seven explicit read-only columns and paging.
5. no silent local custody transfer, API/DTO/Permission/DDL/offline invention.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.
- Current: `INDEPENDENT_REVIEW`.
- Reviewer: `TEAM-D06`.
