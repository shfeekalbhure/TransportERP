# CONTINUOUS MISSION DISPATCHER — GROUP-01

## Purpose

This file closes the governance handoff gap between `MISSION-02 → MISSION-03 → MISSION-04 → MISSION-05`.

The authoritative state remains under `CONTROL_TOWER/`. A transition is not complete merely because `CONTROL_TOWER_TASK_QUEUE.md` says `START`. Each mission must also have its own `CURRENT_DIRECTIVE.md` set to the matching state.

## Critical operating truth

A file-state change can authorize and dispatch the next mission, but it cannot by itself wake or create a stopped external chat/Codex session. Therefore Control Tower must distinguish:

- `START AUTHORIZED` = prerequisites verified and mission directive changed to START.
- `IN PROGRESS` = evidence exists that a worker session actually began work and wrote mission output.
- `WAITING FOR WORKER SESSION` = START is authorized but no active worker execution is evidenced yet.

Control Tower must never claim `IN PROGRESS` from a directive change alone.

## Automatic dispatch sequence

1. When MISSION-02 is sealed and handed off, verify its report, crosswalk, manifest, SHA-256, seal, handoff, preservation, DB-GOV and execution-wave prerequisites.
2. If valid, change `MISSION-03_EXECUTION/CURRENT_DIRECTIVE.md` to `START — EXECUTION UNDER SEALED MISSION-02 PLAN` and update Task Queue/Live Status/Handoff register in the same governance transition.
3. When MISSION-03 is sealed and handed off, verify exact execution SHAs, evidence, tests, DB-GOV compliance, preservation and rollback records.
4. If valid, change `MISSION-04_VERIFICATION/CURRENT_DIRECTIVE.md` to `START — INDEPENDENT VERIFICATION` and update central records.
5. When MISSION-04 is sealed and handed off, verify independent result package and unresolved/reopened findings.
6. If valid, change `MISSION-05_CLOSURE/CURRENT_DIRECTIVE.md` to `START — FINAL CLOSURE AND DELIVERY` and update central records.
7. When MISSION-05 is sealed, update GROUP-01 final state and owner-delivery package.

## No owner relay

The owner is not required to copy reports or manually rewrite start orders between missions. Control Tower performs the file-based dispatch automatically after prerequisite verification.

If the next mission has no active worker session, record exactly:

`START AUTHORIZED — WAITING FOR WORKER SESSION`

This is an execution-runtime limitation, not a governance blocker. Do not revert the mission to WAIT merely because the session is inactive.

## Mission-local directive requirement

Every mission from MISSION-02 through MISSION-05 must have a `CURRENT_DIRECTIVE.md` in its own official folder. The mission-local directive is the first executable instruction after governance/README review.

## Independence rule

MISSION-04 must remain independent from MISSION-03 execution. The same execution worker must not self-certify MISSION-04. File dispatch may be automatic; verification role separation remains mandatory.

## Safety

This dispatcher does not authorize destructive Git actions, Production changes, Database changes outside `DB-GOV-001`, or bypass of any sealed prerequisite. High-risk owner-reserved actions remain `OWNER DECISION REQUIRED`.
