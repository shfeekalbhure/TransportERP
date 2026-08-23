# FLOW01-W3-SCR-001 — Layout Authority Decision

Date: 2026-08-24
Decision owner: Project owner / user approval
Scope: Design-stage layout authority only for `FLOW01-W3-SCR-001 — إدخال البوليصة`

## Decision

For this screen, the current approved shared CoreUI / Transaction profile governs vertical layout composition:

- `Header / MainData = Content`
- `Tabs / Workspace = Fill`
- `Lines / Grid = Fill`
- shared Toolbar / Grid / Pagination / Audit / RTL / DPI behavior remains inherited from CoreUI

The earlier `SRC-053` typed text `Header(Fixed) -> Tabs(Content)` is retained as historical issued evidence but is **not** used to create a screen-local layout exception.

No `LocalException` is created. The screen stays `Transaction / HeaderLines` and must conform to the approved shared profile.

## Effect

- The prior `HOLD_AUTHORITY` blocking TEAM-D02 layout is resolved.
- `FLOW01-W3-SCR-001` may resume at `LAYOUT` under `TEAM-D02`.
- TEAM-D02 must not invent additional layout deviations.
- Existing `TBD-GATED` items unrelated to this contradiction remain gated.

## Non-effects

This decision does not modify the official kurrasa, API, DTO, DDL, permissions, runtime behavior, offline-write authority, business lifecycle, or application code.
