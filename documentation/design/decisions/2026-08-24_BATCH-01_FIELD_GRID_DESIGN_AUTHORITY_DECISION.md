# BATCH-01 — FIELD_GRID Design Authority Decision

**Date:** 2026-08-24  
**Authority:** Project Owner approval  
**Scope:** `FLOW01-W3-SCR-002`, `FLOW01-W3-SCR-003`, `FLOW01-W3-SCR-006`, `FLOW01-W3-SCR-007`, `FLOW01-W3-SCR-008`  
**Stage:** TEAM-D03 / FIELD_GRID

## Decision
TEAM-D03 is authorized to define **screen-design metadata only** for the concrete grids already issued by the current FLOW01 Typed ScreenDefinitions. The team may define, within issued business semantics and current CoreUI contracts:
- Arabic column label;
- UI ValueType / presentation type;
- Required / ReadOnly / edit policy;
- display order;
- CoreUI-compatible semantic width policy;
- editor / lookup presentation where the issued semantic is a reference/input;
- selection policy.

## Boundaries
This authority does **not** create or change:
- API routes/actions;
- DTO payload contracts or storage fields;
- permission codes or authorization scope;
- DDL/tables/columns/indexes;
- business calculations, lifecycle graphs, custody rules, capacity formulas or accounting behavior;
- offline-write authority;
- official kurrasa content.

Exact provider/endpoint identifiers, server sort-key mappings, or other technical bindings not already issued remain `TBD-GATED` and are nonblocking for design unless a later stage proves otherwise.

## CoreUI constraints
- `AutoGenerateColumns=false` for every concrete grid.
- Shared grid rendering, RTL, DPI, row/header styling, validation/loading/error states and pagination implementation remain CoreUI-owned.
- No fixed local pixel widths or screen-local grid styling.
- Sensitive commands remain Capability + Permission + State bound and server-authoritative.

## Effect
The common `HOLD_AUTHORITY` for BATCH-01 FIELD_GRID metadata is resolved. TEAM-D03 may complete the five screen grid contracts and hand off to TEAM-D04 when each canonical `screen-spec.md` is complete.
