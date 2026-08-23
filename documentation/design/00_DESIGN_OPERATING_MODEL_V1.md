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

### DESIGN-LEAD / ORCHESTRATOR — Coordinator
Owns queue orchestration, ownership assignment, dependency resolution, stage gating and escalation. It does not rewrite team evidence silently and does not invent missing authority.

## 4. Control-plane model
The owner may use one central conversation/session for direction and approvals. TEAM-D01..TEAM-D06 are logical workflow roles whose authoritative handoff occurs through repository artifacts, not through separate conversation histories.

Conversation memory is never a governing source. Before every stage, the assigned role must read the queue row, canonical screen spec, exact cited kurrasa/design authority, relevant frozen/current CoreUI/Profile references, and current repository implementation/evidence where it exists.

`06_DESIGN_ORCHESTRATOR_PROTOCOL_V1.md` defines the mandatory orchestration and no-guess protocol.

## 5. Single-source rule
There is one live work queue and one canonical specification package per screen. Teams add evidence and status to that package; they do not create competing “final” copies.

## 6. Batch policy
First validate the operating model on one representative Transaction screen. After PASS, move to controlled batches by common ScreenProfile/Variant, not arbitrary groups. Batch size is decided by dependency and review capacity; all screens in a batch must share the same frozen CoreUI baseline.

## 7. Required preconditions before a screen enters design
- Canonical Screen ID known or explicitly gate-bound.
- Governing kurrasa/source reference recorded.
- ScreenProfile selected from the six governed families.
- Variant/Capabilities identified or explicitly `TBD-GATED`.
- Current repository evidence identified when implementation already exists.
- No team invents API/DTO/Permission/DDL from UI need alone.

## 8. No-guess rule
A missing, contradictory, stale or authority-unsupported fact is never filled from assumption. Use `TBD-GATED` for a non-blocking unknown or `HOLD_AUTHORITY` when the stage cannot safely continue. Record the exact blocker and required evidence.

## 9. Design completion definition
A screen is `DESIGN_READY_FOR_REVIEW` only when it has: profile, variant, capabilities, fields, grids, tabs, filters, commands, state/permission conditions, layout roles, lookup semantics, validation/error states, RTL/DPI notes, offline behavior classification, and acceptance criteria.

It becomes `DESIGN_APPROVED` only after TEAM-D06 PASS and Design Lead closure.
