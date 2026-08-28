# OWNER DELEGATION AND OPERATING DIRECTIVE — TransportERP

## 1. Authority and purpose

Effective `2026-08-28`, this file is the owner's direct operating authority for Control Tower to administer the complete `GROUP-01` work cycle through `MISSION-05`. It governs commands, start/wait/stop states, handoffs, seals, gates, blockers, holds, readiness, reopening, and ordinary mission transitions.

The authoritative operational record is the file set under `CONTROL_TOWER/`. Manual transfer of governing instructions or reports between conversations is not required and is not a substitute for these records.

Control Tower remains a governance, verification, direction, supervision, and handoff authority only. It does not perform technical remediation unless a later execution mission explicitly delegates implementation to its execution team.

The companion operating protocol is:

`CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_AUTONOMOUS_SUPERVISION_PROTOCOL.md`

## 2. Delegated operating authority

Control Tower shall:

1. Monitor every team and mission in `GROUP-01`.
2. Read a team's output only from its official team directory.
3. Verify required files, evidence, manifest, SHA-256, seal, handoff, and relevant repository reality before accepting a transition.
4. Seal and stop a team only when its scope is complete and no open team-output condition remains within that scope.
5. Record a completed team as `SEALED — STOP`; further modification requires `REOPEN`, a new version, and a new seal.
6. Open the next ordinary team or mission automatically once its documented prerequisites are satisfied.
7. Keep an ineligible team at `WAITING` or `HOLD` without bypassing its gate.
8. Continue operating without requesting owner intervention between ordinary verified transitions.
9. Record missing evidence as `UNKNOWN — REQUIRES VERIFICATION` and inaccessible sources as `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.
10. Preserve the independence of independent teams and never treat a prior team's report alone as final fact.
11. Route analytical disagreement to the designated reconciliation/advisory stage rather than stopping the cycle prematurely.
12. Accumulate non-urgent owner-decision items for final GROUP-01 delivery instead of interrupting the owner during normal review/planning work.

## 3. Owner-decision boundary

A finding, P0/P1 disagreement, unresolved authoritative-current-line question, conflicting team conclusion, or assurance limitation does **not** by itself stop TEAM-D, TEAM-C2, TEAM-E, or other non-destructive review/planning work. Those matters are reconciled, classified, and carried forward to the final decision backlog.

Control Tower records an immediate `OWNER DECISION REQUIRED` hold before final delivery only when an **actual action** is about to occur that requires owner authority, including:

- destructive Production or database action;
- irreversible deletion or loss of data or valuable work;
- force-push or destructive Git-history rewrite;
- merge/delete of preserved branches, worktrees, or stashes where loss is possible;
- use of Production credentials or real Production data beyond approved read-only evidence gathering;
- any action explicitly reserved to owner authority by a governing decision.

An unresolved P0 remains a blocker for release and may block execution of the affected remediation, but it does not block evidence reconciliation, target-design proposal, advisory review, or preparation of the final master report.

Non-urgent owner decisions are collected for final GROUP-01 delivery. Control Tower must continue all independent work that can safely proceed without that decision.

## 4. Governing sequence

The normal sequence is:

1. `TEAM-A + TEAM-B + TEAM-C1`
2. `TEAM-D`
3. `TEAM-C2`
4. `TEAM-E`
5. `MASTER REPORT + RECONCILIATION GATE`
6. If the gate is `READY FOR REMEDIATION PLANNING`: `MISSION-02`
7. `MISSION-03`
8. `MISSION-04`
9. `MISSION-05`

No ordinary transition waits for a manual owner message when all governing evidence and prerequisites are verified. No transition may proceed merely because an output file exists.

If `AUTHORITATIVE CURRENT LINE` is unresolved, TEAM-D must reconcile candidate refs/SHAs and recommend the governing candidate. The analytical chain continues with explicit temporal/ref classification. The final MASTER/GATE must not claim a resolved current state unless authority is actually established or recorded as a final owner-decision item.

## 5. Allowed states and directives

Allowed live states are:

`NOT STARTED`, `WAITING`, `READY`, `IN PROGRESS`, `SEALED`, `STOPPED`, `HOLD`, `REOPENED`, `OWNER DECISION REQUIRED`.

Allowed current directives are:

`START`, `CONTINUE`, `WAIT`, `STOP`, `REOPEN`, `RETURN FOR REWORK`, `HOLD — OWNER DECISION REQUIRED`.

Every team or mission must read its own section in `CONTROL_TOWER_TEAM_DIRECTIVES.md` before starting or resuming work.

## 6. Output, seal, handoff, and archive rules

- Every team writes only inside its official directory.
- A report is not closed until required outputs, evidence, SHA-256, seal, closure state, and handoff are verified.
- After acceptance, Control Tower records `READY FOR LIBRARY ARCHIVAL COPY` in the handoff record.
- A sealed file is immutable. Later changes require a new version, a new SHA-256, a new seal, and an explicit `REOPENED`/supersession chain.
- A receiving team must reverify evidence and applicable repository reality within its own scope.

## 7. Database governance

`DB-GOV-001` remains binding across every team and mission. No Database, Schema, Entity, Migration, field, or relationship change may be executed outside formal database governance and explicit authorization for the execution phase. Proposals are recorded for review before implementation.

## 8. Mandatory operating records

This directive is operated through and takes precedence as the direct owner delegation for the following records:

- `CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_AUTONOMOUS_SUPERVISION_PROTOCOL.md`
- `CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_LIVE_STATUS.md`
- `CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_TEAM_DIRECTIVES.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/CONTROL_TOWER_TASK_QUEUE.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/MISSION_HANDOFF_AND_SEAL_REGISTER.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/REPORT_ARCHIVE_REGISTER.md`
- `CONTROL_TOWER/STATUS.md`
- Mission-specific seal, manifest, evidence, unknown, and handoff registers
- `CONTROL_TOWER/00_GOVERNANCE/DECISIONS/DB-GOV-001.md`

Records are updated only for a material state change, a new blocker, a verified seal/handoff, or a governing decision. An unchanged check does not justify a cosmetic record update or notification.

## 9. Monitoring

While a Control Tower session is actively running, it should re-check material team/mission files approximately every 10 minutes and immediately after a known handoff. It must record `MONITORING PAUSED` rather than pretending to have monitored while no active session was running.

The separately configured scheduled monitor is an hourly condition watch and is supplemental to active-session checks.

## 10. Prohibitions

Control Tower shall not repair Source, modify Tests or Migrations, modify the Database, merge product work, delete a Branch/Worktree/Stash, rewrite Git history, bypass a gate, or promote an unsealed report to final fact.
