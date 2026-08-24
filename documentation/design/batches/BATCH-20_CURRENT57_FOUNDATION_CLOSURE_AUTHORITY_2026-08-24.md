# BATCH-20 — Current57 Foundation Closure — Design Authority

**Screens:** `GEN-013`, `ACC-036`  
**Date:** 2026-08-24  
**State:** `INDEPENDENT_REVIEW`

## Authority
- Current57 baseline / Final Execution Register V1.0.
- Current W2 routes and permissions.
- Owner Wave-1 decision `OWNER-WAVE1-20260823`.
- `ORG-OD-004` numbering semantics and controlled resolution R-008.
- CoreUI Settings and MasterData foundations.

## Owner closures consumed
### GEN-013
- Numbering is server-side and atomic; no `MAX+1`.
- Lifecycle is `Reserve → Commit | Cancel`.
- No reuse of cancelled numbers; durable history retained.
- Scope dimensions are Company / Branch / Fiscal Year / Document Type as governed by the sequence.
- `Last Number` is a protected/read-only business concept derived from durable committed allocation, not silently mapped to legacy `NextValue`.
- Protected reset/change-last-number uses `GEN013.Override`, reason, permission, ExpectedVersion and approval when policy requires.
- Legacy `NextValue → LastNumber` migration/backfill remains an implementation/runtime gate only.

### ACC-036
- Current owner decision requires separate `AccountGroup` + `AccountType` implementation with a discriminated DTO.
- Legacy merged classification persistence is excluded from governing runtime.
- Screen remains `MasterData / Standard`, using only `View/Create/Edit/Disable`.
- Exact physical field mapping remains implementation-owned where not closed; no merged classification storage is invented by design.

## Design boundary
This batch may define UI field presentation, edit/read-only semantics, layout and shared component use from current governed screen content. It does not create/change W1 columns, migrations, DTO fields, routes, permissions, accounting formulas, numbering algorithms, or offline-final-write authority.

`TEAM-D01..D05 = PASS`; next gate: `TEAM-D06 INDEPENDENT_REVIEW`.
