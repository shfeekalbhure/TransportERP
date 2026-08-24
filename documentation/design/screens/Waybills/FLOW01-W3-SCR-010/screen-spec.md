# FLOW01-W3-SCR-010 — إثبات التسليم — Canonical Screen Specification

**Alias:** `DLV-002`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-02`

## Authority
- Current FLOW01 owner issuance `SRC-053`; historical generic Variant gate is superseded by current `Transaction / HeaderLines` typed definition.
- W1: `DeliveryOutcome`.
- W2: `F01.DeliveryOutcome.Record`, `F01.DeliveryOutcome.Get`.
- Permissions: `f01.delivery.outcome.record`, `f01.delivery.outcome.view`.
- Typed definition: `FLOW01-W3-SCR-010_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-009` (issued; runtime not run).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-02_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: record an authorized delivery outcome with required evidence/exception context and display the resulting immutable outcome inside scope.

Fields:
- `deliveryOrderRef` — أمر التوصيل — Lookup/UUID — required/read-only assigned context.
- `taskRef` — المهمة — Lookup/UUID — required — belongs to order.
- `outcomeType` — النتيجة — Enum lookup — required — allowed transition.
- `receiverRef` — المستلم — Reference — conditional on delivered.
- `occurredAt` — وقت النتيجة — Instant — required/audited.
- `evidenceRefs` — أدلة التسليم — AttachmentList/Reference Collection — conditional by outcome policy.
- `exceptionReason` — سبب الاستثناء — String — conditional on failure.
- `expectedVersion` — hidden Integer/read-only — required on record.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- EvidenceGrid = `Fill`.
- shared actions/audit/validation/loading/error = CoreUI.
- tabs: `Outcome | Evidence | Exception | Audit`.
- no LocalException/local sizing.

## FIELD_GRID — TEAM-D03 PASS
The evidence list is a display/result surface; capture is through issued evidence inputs. `GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Edit policy | Width policy |
|---:|---|---|---|---|---|
| 1 | `evidenceRef` | الدليل | Reference | ReadOnly | primary Fill |
| 2 | `evidenceType` | نوع الدليل | Enum | ReadOnly | content state |
| 3 | `capturedAt` | وقت الالتقاط | Instant | ReadOnly | content datetime |
| 4 | `capturedBy` | ملتقط الدليل | Reference | ReadOnly | content/reference |
| 5 | `verificationState` | حالة التحقق | Enum | ReadOnly | content state |

Provider/sort identifiers remain `TBD-GATED`; no attachment API is invented.

## UX — TEAM-D04 PASS
- record is available only in issued Assigned/OutForDelivery state with permission.
- delivered outcome requires the issued recipient/evidence conditions; failed outcome requires the issued exception reason condition.
- UI does not invent additional outcome values or transition edges.
- loading prevents double-submit; `expectedVersion` conflict uses shared Reload/Refresh.
- evidence shown after response is server-authoritative; no local verification result is invented.
- permission/scope denial leaks no hidden order/task evidence.
- online-only; no offline proof queue.

## VISUAL — TEAM-D05 PASS
Shared Transaction/CoreUI only: RTL/DPI, attachment/evidence state presentation, central typography/spacing, read-only evidence grid, tabs, validation/loading/error/audit. No local style or evidence-specific color semantics.

## Acceptance criteria
1. outcome recording remains state/permission bound.
2. five explicit immutable evidence columns.
3. conditional delivered/failed requirements are preserved without new business rules.
4. no API/DTO/Permission/DDL/offline invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.  
Current: `INDEPENDENT_REVIEW` — `TEAM-D06`.
