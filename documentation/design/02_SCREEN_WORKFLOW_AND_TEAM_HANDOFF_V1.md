# TransportERP — Screen Workflow and Team Handoff V1

## 1. Goal
Move screen work automatically between specialized logical teams through one shared queue. The owner does not manually relay instructions from team to team.

## 2. Workflow states
`BACKLOG → ANALYSIS → LAYOUT → FIELD_GRID → UX → VISUAL → INDEPENDENT_REVIEW → DESIGN_APPROVED`

Revision path:
`INDEPENDENT_REVIEW → NEEDS_REVISION → owning prior stage → INDEPENDENT_REVIEW`

Blocking path:
`ANY_STAGE → HOLD_AUTHORITY` when a missing governing decision, identity, API/permission authority or unresolved contradiction prevents safe design.

## 3. Orchestrator pre-check
Before assigning or advancing any stage, `DESIGN-LEAD / ORCHESTRATOR` verifies that the assigned role can read:
1. the current live queue row;
2. the canonical `screen-spec.md`;
3. exact governing kurrasa/current-design references cited by the spec;
4. relevant frozen/current CoreUI and ScreenProfile references;
5. current repository implementation/evidence for the screen/domain when it exists.

If a blocking input is missing or contradictory, the screen does not advance. It moves to `HOLD_AUTHORITY` with the blocker recorded.

## 4. Handoff contract
Each stage must update the live queue with:
- `ScreenCode`
- `CurrentState`
- `OwnerTeam`
- `InputVersion`
- `OutputVersion`
- `BlockingIssue`
- `NextTeam`
- `EvidencePath`
- `ReviewedBy`
- `UpdatedAt`

The next team begins only when `NextTeam` matches its team and required evidence is present.

## 5. Team transitions
- TEAM-D01 completes analysis → TEAM-D02.
- TEAM-D02 completes profile-compliant layout → TEAM-D03.
- TEAM-D03 completes fields/grids/lookups → TEAM-D04.
- TEAM-D04 completes flow/error/keyboard behavior → TEAM-D05.
- TEAM-D05 applies visual system → TEAM-D06.
- TEAM-D06 PASS → DESIGN-LEAD closes as `DESIGN_APPROVED`.

`DESIGN-LEAD / ORCHESTRATOR` performs the routing check; specialist ownership of evidence remains with the assigned team.

## 6. No manual copy / no separate authority rule
Teams do not receive separate copied briefs and do not depend on separate conversation histories. They read:
1. the queue row;
2. the canonical screen spec;
3. the exact kurrasa/source references;
4. the frozen/current CoreUI/Profile documentation;
5. the repository evidence cited for the screen.

A separate human team conversation may exist for discussion, but it cannot become an authority or handoff source independent of the repository.

## 7. No-guess behavior
When an exact fact cannot be established from the required sources:
- do not infer it from naming, current UI, prior chat, or convenience;
- use `TBD-GATED` when the unknown does not block the current stage;
- use `HOLD_AUTHORITY` when it does;
- record the required decision/evidence precisely.

## 8. Review gates
Independent review must verify:
- no seventh ScreenProfile was invented;
- no local duplicate Toolbar/Grid/Pagination/Audit implementation was specified;
- RTL/DPI/resize rules are preserved;
- all screen-specific fields/grids/commands are explicit;
- permissions and online/offline behavior are classified, not guessed;
- all material design claims are traceable to cited authority/evidence;
- any authority gap is HOLD, not silently designed around.

## 9. Batch execution
After the pilot screen passes, the Design Lead may release batches grouped by shared Profile/Variant. A batch does not bypass per-screen review.

## 10. Orchestration authority
The detailed central coordination protocol is `06_DESIGN_ORCHESTRATOR_PROTOCOL_V1.md`. This workflow creates no programming/API/DDL/Permission/Offline-write authority.
