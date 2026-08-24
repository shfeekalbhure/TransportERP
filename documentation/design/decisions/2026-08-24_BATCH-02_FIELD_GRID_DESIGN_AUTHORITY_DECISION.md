# BATCH-02 — FIELD_GRID Design Authority Decision

**Date:** 2026-08-24  
**Authority:** Project Owner approval in central design orchestration  
**Scope:** `FLOW01-W3-SCR-004`, `009`, `010`, `011`, `012`, `013`  
**Status:** APPROVED — DESIGN AUTHORITY ONLY

## Decision
TEAM-D03 is authorized to complete screen-specific UI `GridColumnDefinition` metadata for BATCH-02 within the current approved CoreUI/Profile contracts and the already-issued FLOW01 W1/W2/W3 semantics.

For issued grid semantics TEAM-D03 may define:
- Arabic presentation label;
- UI ValueType consistent with the issued semantic;
- Required / ReadOnly / editable presentation rule;
- column order;
- CoreUI-compatible semantic width policy;
- editor/presentation type where the semantic already requires one;
- selection policy when no bulk capability exists.

## Hard boundaries
This decision does **not** authorize creation or modification of:
- API route/action/endpoint/provider identifiers;
- DTO fields beyond current governing contracts;
- Permission codes, scope or authorization rules;
- DDL, tables, columns, migrations or indexes;
- domain formulas, lifecycle graphs, accounting/posting rules or custody authority;
- offline-write, queue, outbox or synchronization authority;
- official Kurrasa content.

Unissued provider identifiers, exact server sort-key mappings or other technical bindings remain `TBD-GATED` and nonblocking for design unless they prevent a safe user interaction contract.

## Shared UI constraints
- `AutoGenerateColumns=false` for concrete grids.
- Shared CoreUI owns RTL, DPI, typography, spacing, grid/header/row styling, loading, validation, error, pagination and audit presentation.
- No local CoreUI clone or LocalException is created by this decision.
- Sensitive commands remain bound to the exact issued W2 Permission + state/scope predicates; the server remains authoritative.
- FLOW01 issued actions remain `ONLINE_ONLY / OFFLINE_WRITE=0 / Can Queue=NO`.

## Handoff
TEAM-D03 may proceed without further owner approval for BATCH-02 FIELD_GRID metadata while remaining inside the boundaries above.
