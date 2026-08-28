# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must read its own section here before starting or resuming work. Only Control Tower changes a `CURRENT DIRECTIVE`. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

During an active Control Tower session, teams should re-check this file approximately every 10 minutes and immediately after a known upstream handoff. If no active session is running, no continuous monitoring may be assumed.

## TEAM-A

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — STOP`
- Reason: Package, manifest hashes, seal, and handoff were verified and centrally received.
- Next permitted action: None unless Control Tower records `REOPEN`.

## TEAM-B

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — STOP`
- Reason: Package, detached hashes, seal, and handoff were verified and centrally received.
- Governing carry-forward: `BLK-B-001 — SINGLE-SESSION TEAM-B — MULTI-REVIEWER ASSURANCE NOT SATISFIED`.
- Next permitted action: None unless Control Tower records `REOPEN`. TEAM-B may not begin a later stage itself.

## TEAM-C1

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — STOP`
- Reason: All nine sealed outputs, seal, manifest, and handoff were verified and centrally received.
- Next permitted action: None unless Control Tower records `REOPEN`.

## TEAM-D

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `START`
- Prerequisite status: `SATISFIED — TEAM-A, TEAM-B, TEAM-C1 SEALED AND HASH-VERIFIED`.
- Required work: Perform full Finding-by-Finding reconciliation, including agreement, disagreement, single-team findings, P0/P1 classification conflicts, preservation findings, assurance limitations, and authoritative-line candidates.
- Special instruction: The unresolved `AUTHORITATIVE CURRENT LINE` and TEAM-A/TEAM-B P0 disagreement are **inputs to reconciliation**, not owner holds at this stage.
- Authoritative-line task: classify candidate refs/SHAs, recommend the governing candidate, and explicitly mark anything unresolved. Do not guess.
- Required output: `TEAM-D_EVIDENCE_RECONCILIATION_REPORT.md` plus crosswalk/evidence/unknown registers required by the governing command.
- Next permitted action after completion: Seal and hand off to Control Tower. Do not start TEAM-C2 yourself.

## TEAM-C2

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-D must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

## TEAM-E

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-C2 must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

## MASTER REPORT + RECONCILIATION GATE

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-E must be sealed and handed off; all governing assurance conditions must be addressed or explicitly carried as final decision items.
- Next permitted action: Wait for a Control Tower `START` directive.

## MISSION-02

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-01 gate must be formally `READY FOR REMEDIATION PLANNING`.
- Next permitted action: Wait for a Control Tower `START` directive.

## MISSION-03

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-02 must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-03 must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-04 must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

`DB-GOV-001` is binding throughout. No team may execute a Database, Schema, Entity, Migration, field, or relationship change without the required governance and execution authorization.
