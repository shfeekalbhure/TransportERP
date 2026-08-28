# HOURLY SUPERVISION SCHEDULE — TransportERP Control Tower

## Purpose

This file defines the scheduled supervision cadence for `CONTROL_TOWER/` and complements, but does not replace, the active-session 10-minute checks defined by the governing supervision protocol.

## A. Active-session cadence

While the Control Tower session is active and capable of monitoring, it performs a material-state check every `10 minutes`.

Each 10-minute check reviews only what can materially change the workflow:

- `CONTROL_TOWER_TEAM_DIRECTIVES.md`
- `CONTROL_TOWER_LIVE_STATUS.md`
- `CONTROL_TOWER_TASK_QUEUE.md`
- `MISSION_HANDOFF_AND_SEAL_REGISTER.md`
- relevant team/mission output directories
- new reports, evidence registers, manifests, SHA-256 records, seals, handoffs, blockers, and reopen records

If nothing material changed, do not rewrite records cosmetically and do not invent a decision.

## B. Hourly external supervision

A scheduled external supervision pass runs once every hour, at minute `10` in the configured user timezone.

The hourly pass must:

1. Read `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`.
2. Read `CONTROL_TOWER_AUTONOMOUS_SUPERVISION_PROTOCOL.md`.
3. Read `CONTROL_TOWER_TEAM_DIRECTIVES.md`.
4. Read both live-status records and the central task queue.
5. Inspect the active/ready team's official directory.
6. Verify any newly claimed completion against report + evidence + manifest + SHA-256 + seal + handoff.
7. Update governance files only when a material state change is proven.
8. Advance ordinary review/planning transitions automatically when prerequisites are satisfied.
9. Carry analytical disagreements to TEAM-D / TEAM-C2 / TEAM-E instead of stopping the chain prematurely.
10. Record `OWNER DECISION REQUIRED` immediately only when an actual next action is destructive, Production-affecting, irreversible, risks preserved work/data, rewrites Git history, or is explicitly reserved to owner authority.
11. Never modify product Source, Tests, Migrations, production configuration, or database state as part of supervision.

## C. Hourly task matrix

At every hourly pass, apply the first matching row only:

| Priority | Condition | Hourly action |
|---:|---|---|
| 1 | A sealed team was modified without `REOPEN` | Record governance breach; stop affected transition; require new version/seal |
| 2 | Current active team has produced a complete sealed package | Verify hashes/handoff; mark `SEALED — DELIVERED TO CONTROL TOWER — STOP`; open next prerequisite-satisfied stage |
| 3 | Current active team has partial outputs only | Keep `IN PROGRESS`; record only real blockers or new evidence |
| 4 | Current team is `READY` but not started | Keep/start directive according to governing sequence; do not skip stages |
| 5 | Upstream prerequisites not satisfied | Keep downstream teams `WAIT` |
| 6 | Analytical conflict exists | Route to assigned reconciliation/advisory stage; do not escalate to owner merely because teams disagree |
| 7 | Destructive/Production/irreversible action is actually pending | Record `OWNER DECISION REQUIRED` for that affected gate |
| 8 | No material change | No governance rewrite and no notification |

## D. Governing sequence

`TEAM-A + TEAM-B + TEAM-C1 → TEAM-D → TEAM-C2 → TEAM-E → MASTER REPORT + RECONCILIATION GATE → MISSION-02 → MISSION-03 → MISSION-04 → MISSION-05`

A stage advances only after its documented prerequisites and seal/handoff requirements are verified.

## E. Current adopted state

At issuance of this schedule:

- TEAM-A = `SEALED — STOP`
- TEAM-B = `SEALED — STOP`
- TEAM-C1 = `SEALED — STOP`
- TEAM-D = `READY / START AUTHORIZED`
- TEAM-C2 = `WAIT`
- TEAM-E = `WAIT`
- MASTER = `WAIT`
- MISSION-02–05 = `WAIT`

The unresolved authoritative-current-line question must be reconciled and carried as an evidence/authority item; it does not block TEAM-D's read-only reconciliation work.

## F. Record discipline

The hourly supervisor updates only the affected official records. It does not create duplicate truth stores and does not use chat transcripts as governing state.

`CONTROL_TOWER/` remains the authoritative operational record.
