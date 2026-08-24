# BATCH-11 — Accounting Financial Masters — Owner Design Authority Decision

**Screens:** `ACC-038`, `ACC-039`, `ACC-040`  
**Date:** 2026-08-24  
**Decision state:** `APPROVED — DESIGN-ONLY`  
**Owner:** `DESIGN-LEAD / ORCHESTRATOR`

## Owner decision
The owner approved the recommended design-only authority decision on 2026-08-24.

For `ACC-038 / ACC-039 / ACC-040`, the current V1.3 screen field/tab inventory is the governing **UI design contract**. `TEAM-D03` may define only UI-design metadata:
- UI semantic / ValueType;
- required / optional / read-only / editable presentation policy;
- field order and tab grouping;
- CoreUI width policy;
- lookup presentation/selection semantics;
- list/grid presentation, including a bounded `ACC-040` list projection where the baseline had `Columns=0`.

All unresolved W1 / DTO / API / permission / DDL / physical lookup-provider bindings remain explicit `TBD-GATED` implementation blockers and are not promoted by this decision.

## Explicitly NOT authorized
- W1 columns/tables/DDL/migrations;
- API routes or DTO properties;
- new permissions/security scope;
- accounting formulas or financial limit enforcement not already issued;
- lookup provider identifiers/endpoints not already issued;
- application code;
- official Kurrasa modification;
- offline write/queue/outbox authority.

## Governing identities / W2 surface
- `ACC-038 — الصناديق` = `MasterData / Tabbed`; exact W2 surface: List/Get/Create/Update/Disable; `ACC038.View/Create/Edit/Disable`.
- `ACC-039 — الحسابات البنكية` = `MasterData / Tabbed`; exact W2 surface: List/Get/Create/Update/Disable; `ACC039.View/Create/Edit/Disable`.
- `ACC-040 — طرق الدفع` = `MasterData / Standard`; exact W2 surface: List/Get/Create/Update/Disable; `ACC040.View/Create/Edit/Disable`.

## Stage disposition after approval
- `TEAM-D01 ANALYSIS = PASS`
- `TEAM-D02 LAYOUT = PASS`
- `TEAM-D03 FIELD_GRID = PASS — owner design-only authority applied`
- `TEAM-D04 UX = PASS`
- `TEAM-D05 VISUAL = PASS`
- next gate: `TEAM-D06 INDEPENDENT_REVIEW`

## Technical gates retained
### ACC-038
Unresolved implementation bindings include Cashbox code/name, GL Account lookup provider, default cashier lookup provider, balance/transaction limit persistence, shift-close persistence and related field-level audit/offline binding.

### ACC-039
Unresolved implementation bindings include bank-account code/name fields, Account Number vs IBAN physical semantics, SWIFT/BIC storage, GL/fees/reconciliation-account lookup providers, withdrawal/transfer limit persistence and field-level audit/offline binding.

### ACC-040
Unresolved implementation bindings include PaymentMethod physical schema for code/name/type/booleans, Clearing Account lookup provider/storage and field-level audit/offline binding.

These are implementation gates only after this owner decision; they do not authorize invention and they do not by themselves block the UI design record from independent review.
