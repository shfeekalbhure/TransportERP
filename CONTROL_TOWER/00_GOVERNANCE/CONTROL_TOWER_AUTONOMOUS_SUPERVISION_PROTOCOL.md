# CONTROL TOWER AUTONOMOUS SUPERVISION PROTOCOL

## Purpose

This protocol keeps `GROUP-01` moving through file-based governance without requiring the owner to relay reports or ordinary transition messages between sessions.

## Operating model

- `CONTROL_TOWER/` is the sole authoritative operational record.
- Teams read `CONTROL_TOWER/README.md`, the owner directive, their current section in `CONTROL_TOWER_TEAM_DIRECTIVES.md`, their mission order, and required sealed predecessors before work.
- Teams write only inside their official mission/team directory.
- Control Tower verifies completeness, evidence, manifests, SHA-256, seals, handoffs, and prerequisites before changing a directive.
- Ordinary analytical/review/planning disagreements are routed to the designated reconciliation or advisory stage, not treated as permission to interrupt the owner or bypass a gate.

## Owner-decision deferral rule

During non-destructive MISSION-01 review and MISSION-02 planning, an analytical finding, priority disagreement, P0/P1 classification conflict, unresolved authoritative-current-line question, or conflicting team conclusion does not by itself stop the analytical chain. It is routed to the designated reconciliation/advisory stage and carried into the final decision backlog.

An immediate `OWNER DECISION REQUIRED` hold is recorded when an actual proposed action requires owner authority, including:

- destructive Production or database action;
- irreversible deletion or loss of data or valuable work;
- force-push or destructive Git-history rewrite;
- merge/delete of preserved branches, worktrees, or stashes where loss is possible;
- use of Production credentials or real Production data beyond approved read-only evidence gathering;
- any action explicitly reserved to owner authority by a governing decision.

A discovered or claimed P0 remains a potential governing blocker for release/execution. It does not prevent TEAM-D, TEAM-C2, or TEAM-E from completing authorized read-only analysis, reconciliation, design proposal, and advisory work.

## Authoritative current line rule

If the authoritative product line is unresolved, TEAM-D reconciles candidate refs/SHAs and evidence, classifies each line (`CURRENT CANDIDATE / UNMERGED / HISTORICAL / LOCAL-ONLY / UNKNOWN`), and records a recommendation without guessing. The MASTER/GATE must not claim a final CURRENT state or readiness until the authority question is resolved or explicitly recorded as a final owner-decision item.

## Monitoring cadence

While a Control Tower session is actively running and able to monitor, it rechecks material team/mission files every `10 minutes` and immediately after a known handoff. It must not claim monitoring during periods without an active session.

The checked scope includes team directories, new reports, Evidence registers, Seal registers, Handoff files, Task Queue, blockers, and mission states. Records change only for a material state change, new blocker, new seal/handoff, reopening, or governing decision.

Before active monitoring ends, Control Tower updates `CONTROL_TOWER/04_CONTROL_TOWER_OPERATIONS/CONTROL_TOWER_LIVE_STATUS.md`. When continued monitoring is not possible, the state is `MONITORING PAUSED — REQUIRES RESUME`.

Any separately enabled scheduled condition watch supplements this active-session cadence but does not replace it and must not be claimed active without current evidence.

## Automatic sequence

1. TEAM-A + TEAM-B + TEAM-C1
2. TEAM-D
3. TEAM-C2
4. TEAM-E
5. MASTER REPORT + RECONCILIATION GATE
6. MISSION-02 if the gate permits planning
7. MISSION-03 according to the sealed remediation plan and execution authority
8. MISSION-04
9. MISSION-05

A completed team is `SEALED — DELIVERED TO CONTROL TOWER — STOP`. A later correction requires `REOPEN`, a new version, a new SHA-256, and a new seal.

## No-guessing and database rules

`REPORT SAYS SO = FACT` is prohibited. Receiving teams recheck governing evidence within scope. Unsupported claims remain `UNKNOWN — REQUIRES VERIFICATION`; inaccessible sources remain `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.

`DB-GOV-001` remains binding. This protocol does not authorize database modification.
