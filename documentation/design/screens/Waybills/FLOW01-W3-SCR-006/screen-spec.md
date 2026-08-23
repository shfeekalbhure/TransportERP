# FLOW01-W3-SCR-006 — الترانزيت وتسليم الحيازة — Canonical Screen Specification

**Alias:** `WHS-003`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `FIELD_GRID`  
**OwnerTeam:** `TEAM-D03`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `CurrentCustodyOwner`, `CustodyHandoff`, `PackageHandlingEvent`.
- W2: `F01.CustodyHandoff.Confirm`, `F01.HubHandling.Get`.
- Typed definition: `FLOW01-W3-SCR-006_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-005` (issued; runtime PASS not claimed).

## ANALYSIS — TEAM-D01 PASS
Purpose: confirm a custody handoff for one or more packages and display immutable handling history inside authorized scope.

### Capabilities
| Capability | Action | Permission | State rule |
|---|---|---|---|
| تأكيد انتقال الحيازة | `F01.CustodyHandoff.Confirm` | `f01.custody.handoff.confirm` | source/target + receipt confirmation |
| عرض تعامل الطرد | `F01.HubHandling.Get` | `f01.hub.handling.view` | scoped read |

### Fields
- `packageRefs` — الطرود — LookupMulti / UUID Collection — required — one current owner.
- `sourceCustodyRef` — الحيازة المصدر — Reference/read-only — server resolved.
- `targetCustodyRef` — الحيازة المستهدفة — Lookup/Reference — required — allowed scope.
- `receiptConfirmedBy` — تأكيد المستلم — SignatureRef/Reference — required — distinct confirmation.
- `handoffReason` — سبب الانتقال — TextArea/String — required/nonempty.
- `handlingHistory` — سجل التعامل — ReadGrid/Collection — immutable display.

## LAYOUT — TEAM-D02 PASS
Current shared Transaction profile governs historical sizing shorthand:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- Handoff/History grid workspace = `Fill`.
- Actions/Audit = shared CoreUI regions.
- No LocalException; RTL/DPI/shared styling remain CoreUI-owned.

Functional tabs retained: `Handoff | Handling History | Audit`.

## FIELD_GRID — TEAM-D03 IN PROGRESS
Typed columns:
`packageRef, sourceCustody, targetCustody, confirmedAt, receiver, reason, eventState`.

Known rules:
- `AutoGenerateColumns=false`.
- paging is issued.
- history is immutable.
- custody source is server-resolved; client must not assert current owner.

Required TEAM-D03 closure: explicit labels/types/read-only-edit rules/order/CoreUI width policy/selection policy and reference-editor presentation. Exact provider identifiers remain gated unless issued.

## Non-inventions
No local custody transition formula, API/DTO/permission, DDL, offline handoff, or hidden package relationship is created.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`.
- Current: `FIELD_GRID`.
- Next after PASS: `TEAM-D04 / UX`.
