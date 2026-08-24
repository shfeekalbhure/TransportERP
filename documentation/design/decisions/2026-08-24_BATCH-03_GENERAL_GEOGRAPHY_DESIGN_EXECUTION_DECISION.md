# BATCH-03 — General Geography Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY

## Scope
Proceed with canonical design workflow for:
- GEN-003 — الدول / Countries
- GEN-004 — المحافظات / Governorates
- GEN-005 — المديريات / Directorates
- GEN-006 — المدن / Cities
- GEN-007 — المناطق / Areas

## Governing authority
- GEN-003: SRC-055 full W1/W2/W3 specification-design authority.
- GEN-004..GEN-007: SRC-056 full W1/W2/W3 specification-design authority.
- Current CoreUI / MasterData / Standard contracts remain shared architecture authority.
- SET-001 and SET-002 remain composite/navigation lineage only and do not replace the five current identities.

## Design boundary
Design teams may consume already-issued fields, filters, grid columns, capabilities, lookup bindings, paging and validation presentation. They must not invent API routes, DTOs, permissions, business rules, DDL, offline write, extra capabilities, or official-kurrasa content.

## Important capability boundary
- GEN-003 includes Print because current W2/W3 issues it.
- GEN-004..GEN-007 do not gain Print, Export, Delete, Enable, Activate, Move, Offline or Queue capabilities.
- Geography parent reference is supplied on Create and is immutable afterward under the current contract.

No application code or official kurrasa is modified by this decision.
