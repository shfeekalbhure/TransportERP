# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## TEAM-A

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`
- Reason: Package, manifest hashes, seal, and handoff were verified and centrally received.
- Next permitted action: None unless Control Tower records `REOPEN`.

## TEAM-B

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`
- Reason: Package, detached hashes, seal, and handoff were verified and centrally received.
- Governing carry-forward: `SINGLE-SESSION TEAM-B — MULTI-REVIEWER ASSURANCE LIMITATION RECORDED` (`BLK-B-001`).
- Next permitted action: None unless Control Tower records `REOPEN`. TEAM-B may not begin a later stage itself.

## TEAM-C1

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`
- Reason: All nine sealed outputs, seal, manifest, and handoff were verified and centrally received.
- Next permitted action: None unless Control Tower records `REOPEN`.

## TEAM-D

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Prerequisite status: TEAM-A, TEAM-B, and TEAM-C1 are sealed and centrally received.
- Required scope: Create the complete Finding-by-Finding Crosswalk; independently reverify agreements, conflicts, and single-team findings; preserve all original IDs and temporal classifications.
- Governing limitation: `AUTHORITATIVE CURRENT LINE FOR THIS AUDIT` remains `UNKNOWN — REQUIRES VERIFICATION`. TEAM-D may reconcile evidence and classified refs, but must not infer or designate the authoritative current line or issue a final CURRENT-state/gate judgment.
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`; 13 detached hashes and the 62-record Crosswalk verified by Control Tower.
- Next permitted action: None unless Control Tower records `REOPEN`.

## TEAM-C2

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `CONTINUE`
- Prerequisite: TEAM-D must be sealed and handed off.
- Required scope: Build the target architecture proposal from the sealed TEAM-D reconciliation and predecessor packages without treating proposals as implemented state.
- Governing limitations: `AUTHORITATIVE CURRENT LINE` remains `UNKNOWN`; confirmed P0 risks and `DB-GOV-001` must be preserved as design constraints, not silently remediated.
- Recorded disposition: `IN PROGRESS — TEAM-C2 SESSION STARTED 2026-08-28T02:02:53Z`.
- Next permitted action: Continue TEAM-C2 target-architecture work inside `05_TEAM-C2/` only and produce the complete report/register/manifest/seal/handoff package.

## TEAM-E

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-C2 must be sealed and handed off.
- Next permitted action: Wait for a Control Tower `START` directive.

## MASTER REPORT + RECONCILIATION GATE

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-E must be sealed and handed off; all governing assurance conditions must be addressed.
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
