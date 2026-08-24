# BATCH-20 — Current57 Foundation Closure — Final Design Closure

**Screens:** `GEN-013`, `ACC-036`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`  
**Independent review:** `TEAM-D06 PASS / 0 open design findings`

## Authority
- Current57 baseline / Final Execution Register V1.0.
- Current W2 routes and permissions.
- Owner Wave-1 decision `OWNER-WAVE1-20260823`.
- `ORG-OD-004` numbering semantics and controlled resolution R-008.
- CoreUI Settings and MasterData foundations.

## Final states
### GEN-013 — الترقيم العام
- `Settings / NumberingControlled`
- 10 governing fields, 4 tabs.
- Exact executable surface: `View | Edit | Reserve | Commit | Cancel | Override`.
- Number reservation is server-side/atomic; no `MAX+1` or client numbering authority.
- `Last Number` is read-only and derived from durable committed allocation.
- Protected reset/change-last-number remains `Override` with permission, reason, ExpectedVersion and approval when required.
- Legacy `NextValue → LastNumber` migration/backfill remains an implementation/runtime/release gate only.

### ACC-036 — مجموعات وأنواع الحسابات
- `MasterData / Standard`
- 8 governing fields, 3 tabs, no screen-specific local grid-column contract.
- Exact executable surface: `View | Create | Edit | Disable`.
- `AccountGroup` and `AccountType` remain separate governed record kinds under the current discriminated contract.
- Legacy merged classification persistence is not promoted.
- Exact discriminator/DTO/physical mappings remain implementation-owned where not issued.

## Completed stages
`TEAM-D01 ANALYSIS = PASS`  
`TEAM-D02 LAYOUT = PASS`  
`TEAM-D03 FIELD_GRID = PASS`  
`TEAM-D04 UX = PASS`  
`TEAM-D05 VISUAL = PASS`  
`TEAM-D06 INDEPENDENT_REVIEW = PASS`

Independent review evidence: `documentation/design/batches/BATCH-20_INDEPENDENT_REVIEW_2026-08-24.md`.

## Design boundary
No application code, official Kurrasa, W1/DDL, API, DTO, permission, migration or offline-final-write authority was modified by this closure. Runtime/release evidence remains separate.
