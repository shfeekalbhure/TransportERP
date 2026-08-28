# OWNER DELEGATION AND OPERATING DIRECTIVE — TransportERP

## 1. Authority and purpose

Effective `2026-08-28`, this file is the owner's direct operating authority for Control Tower to administer the complete `GROUP-01` work cycle through `MISSION-05`. It governs commands, start/wait/stop states, handoffs, seals, gates, blockers, holds, readiness, reopening, and ordinary mission transitions.

The authoritative operational record is the file set under `CONTROL_TOWER/`. Manual transfer of governing instructions or reports between conversations is not required and is not a substitute for these records.

Control Tower remains a governance, verification, direction, and handoff authority only. It does not perform technical remediation.

## 2. Delegated operating authority

Control Tower shall:

1. Monitor every team and mission in `GROUP-01`.
2. Read a team's output only from its official team directory.
3. verify required files, evidence, manifest, SHA-256, seal, handoff, and relevant repository reality before accepting a transition.
4. Seal and stop a team only when its scope is complete and no open condition remains within that scope.
5. Record a completed team as `SEALED — STOP`; further modification requires `REOPEN`, a new version, and a new seal.
6. Open the next ordinary team or mission automatically once its documented prerequisites are satisfied.
7. Keep an ineligible team at `WAITING` or `HOLD` without bypassing its gate.
8. Continue operating without requesting owner intervention between ordinary verified transitions.
9. Record missing evidence as `UNKNOWN — REQUIRES VERIFICATION` and inaccessible sources as `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.
10. Preserve the independence of independent teams and never treat a prior team's report alone as final fact.

## 3. Owner-decision boundary

Control Tower stops the affected transition and records `OWNER DECISION REQUIRED` only when one or more of the following is present:

- an unresolved governing P0;
- risk of losing data or valuable work;
- a Production change;
- a destructive migration;
- a high-risk delete, merge, or force-push;
- a decision that expressly requires owner authority.

An owner-decision hold applies to the affected gate; it does not stop Control Tower from monitoring, preserving records, or validating newly delivered evidence.

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

- `CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_LIVE_STATUS.md`
- `CONTROL_TOWER/00_GOVERNANCE/CONTROL_TOWER_TEAM_DIRECTIVES.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/CONTROL_TOWER_TASK_QUEUE.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/MISSION_HANDOFF_AND_SEAL_REGISTER.md`
- `CONTROL_TOWER/00_GOVERNANCE/REGISTERS/REPORT_ARCHIVE_REGISTER.md`
- `CONTROL_TOWER/STATUS.md`
- Mission-specific seal, manifest, evidence, unknown, and handoff registers
- `CONTROL_TOWER/00_GOVERNANCE/DECISIONS/DB-GOV-001.md`

Records are updated only for a material state change, a new blocker, a verified seal/handoff, or a governing decision. An unchanged check does not justify a cosmetic record update or notification.

## 9. Prohibitions

Control Tower shall not repair Source, modify Tests or Migrations, modify the Database, merge product work, delete a Branch/Worktree/Stash, rewrite Git history, bypass a gate, or promote an unsealed report to final fact.
