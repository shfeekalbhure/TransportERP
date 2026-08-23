# TransportERP — Design Operating Model V1

## 1. Objective
Create a self-running screen-design workflow where teams read the same live queue and the same governed references, update their own deliverables, and hand work forward without the owner manually copying instructions between teams.

## 2. Design-only scope
This operating model covers analysis, UX, UI, ScreenDefinition, visual evidence, review and acceptance preparation. It does **not** authorize production code, API changes, DDL, permissions, accounting postings or offline writes.

## 3. Team model
### TEAM-D01 — Screen Analysis
Owns: purpose, actors, scenarios, fields, grids, commands, states, permissions-needed, validation, alerts, acceptance inputs.

### TEAM-D02 — Layout / Wireframe
Owns: profile-compliant screen composition, container roles, fill/content/fixed ownership, responsive/DPI behavior, RTL composition.

### TEAM-D03 — Fields / Grids / Lookups
Owns: field semantics, data-entry ergonomics, lookup behavior, grid columns, search/filter/paging semantics, keyboard-first paths.

### TEAM-D04 — UX Flow
Owns: user path, shortcut behavior, minimum-click patterns, errors, confirmation, loading/empty/conflict states.

### TEAM-D05 — Visual UI
Owns: final visual application of existing CoreUI tokens/components only. It may not create local toolbar/grid/pagination/audit styling.

### TEAM-D06 — Independent Review
Owns: independent conformity review against kurrasa, CoreUI, ScreenProfile, accessibility/RTL, and the screen contract. Returns PASS / NEEDS_REVISION / HOLD.

### DESIGN-LEAD — Coordinator
Owns queue orchestration, ownership assignment, dependency resolution and escalation. Does not rewrite team evidence silently.

## 4. Single-source rule
There is one live work queue and one canonical specification package per screen. Teams add evidence and status to that package; they do not create competing “final” copies.

## 5. Batch policy
First validate the operating model on one representative Transaction screen. After PASS, move to controlled batches by common ScreenProfile/Variant, not arbitrary groups. Batch size is decided by dependency and review capacity; all screens in a batch must share the same frozen CoreUI baseline.

## 6. Required preconditions before a screen enters design
- Canonical Screen ID known or explicitly gate-bound.
- Governing kurrasa/source reference recorded.
- ScreenProfile selected from the six governed families.
- Variant/Capabilities identified or explicitly `TBD-GATED`.
- No team invents API/DTO/Permission/DDL from UI need alone.

## 7. Design completion definition
A screen is `DESIGN_READY_FOR_REVIEW` only when it has: profile, variant, capabilities, fields, grids, tabs, filters, commands, state/permission conditions, layout roles, lookup semantics, validation/error states, RTL/DPI notes, offline behavior classification, and acceptance criteria.

It becomes `DESIGN_APPROVED` only after TEAM-D06 PASS and Design Lead closure.
