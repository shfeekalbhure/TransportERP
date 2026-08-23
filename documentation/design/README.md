# TransportERP — Design Workspace

**Status:** Approved operating structure for design work. Documentation/design only; no application code, DDL, API or permission authority is created here.

## Purpose
This workspace coordinates screen design work without copying instructions manually between teams. It reuses the existing governed CoreUI architecture and the six existing screen profile families.

The owner may direct design through one central conversation/session. The repository—not separate team conversations—is the handoff and source-of-truth layer.

## Governing design chain
`CoreUI Shared Definitions → ScreenProfile → Variant → Capabilities → ScreenDefinition → Local Exception`

## Six governed screen profiles
- `MasterData`
- `TreeMaster`
- `Transaction`
- `ControlApproval`
- `ReportInquiry`
- `Settings`

No seventh profile may be introduced by a screen-design team without an explicit governed change.

## Files
- `00_DESIGN_OPERATING_MODEL_V1.md` — operating model and team responsibilities.
- `01_REPOSITORY_LAYOUT_AND_OWNERSHIP_V1.md` — where design artifacts live vs. code.
- `02_SCREEN_WORKFLOW_AND_TEAM_HANDOFF_V1.md` — automatic hand-off state machine.
- `03_SCREEN_SPECIFICATION_TEMPLATE_V1.md` — canonical per-screen specification template.
- `04_SCREEN_WORK_QUEUE.csv` — single live queue for design work.
- `05_COREUI_ADOPTION_RULES_V1.md` — rules preventing local duplication of common UI behavior.
- `06_DESIGN_ORCHESTRATOR_PROTOCOL_V1.md` — central coordinator, mandatory pre-read, automatic routing and no-guess protocol.

## Operating rule
Every stage starts from current repository state and exact cited authority/evidence. Missing or contradictory facts become `TBD-GATED` or `HOLD_AUTHORITY`; they are never filled from chat memory or assumption.

## Authority boundary
The official kurrasa remains external governing authority. This folder references it; it does not replace or silently update it.
