# TransportERP — Screen Workflow and Team Handoff V1

## 1. Goal
Move screen work automatically between specialized teams through one shared queue. The owner does not manually relay instructions from team to team.

## 2. Workflow states
`BACKLOG → ANALYSIS → LAYOUT → FIELD_GRID → UX → VISUAL → INDEPENDENT_REVIEW → DESIGN_APPROVED`

Revision path:
`INDEPENDENT_REVIEW → NEEDS_REVISION → owning prior stage → INDEPENDENT_REVIEW`

Blocking path:
`ANY_STAGE → HOLD_AUTHORITY` when a missing governing decision, identity, API/permission authority or unresolved contradiction prevents safe design.

## 3. Handoff contract
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

## 4. Team transitions
- TEAM-D01 completes analysis → TEAM-D02.
- TEAM-D02 completes profile-compliant layout → TEAM-D03.
- TEAM-D03 completes fields/grids/lookups → TEAM-D04.
- TEAM-D04 completes flow/error/keyboard behavior → TEAM-D05.
- TEAM-D05 applies visual system → TEAM-D06.
- TEAM-D06 PASS → DESIGN-LEAD closes as `DESIGN_APPROVED`.

## 5. No manual copy rule
Teams do not receive separate copied briefs. They read:
1. the queue row;
2. the canonical screen spec;
3. the exact kurrasa/source references;
4. the frozen CoreUI/Profile documentation.

## 6. Review gates
Independent review must verify:
- no seventh ScreenProfile was invented;
- no local duplicate Toolbar/Grid/Pagination/Audit implementation was specified;
- RTL/DPI/resize rules are preserved;
- all screen-specific fields/grids/commands are explicit;
- permissions and online/offline behavior are classified, not guessed;
- any authority gap is HOLD, not silently designed around.

## 7. Batch execution
After the pilot screen passes, the Design Lead may release batches grouped by shared Profile/Variant. A batch does not bypass per-screen review.
