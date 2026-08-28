# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## TEAM-A

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `REOPEN`
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
- `CURRENT DIRECTIVE`: `REOPEN`
- Recorded disposition: `REOPENED — LIMITED v1.1 CORRECTION REQUIRED`.
- Reason: TEAM-E proved v1.0 incorrectly describes a source-coded design-time connection fallback; source fails closed when `TRANSPORTERP_DESIGN_CONNSTR` is absent. Preserve v1.0.
- Next permitted action: Issue complete corrected v1.1 under `03_TEAM-C1/v1.1/` with truthful source claim and complete required registers/seal chain.

## TEAM-D

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `REOPEN`
- Prerequisite status: TEAM-A, TEAM-B, and TEAM-C1 are sealed and centrally received.
- Required scope: Create the complete Finding-by-Finding Crosswalk; independently reverify agreements, conflicts, and single-team findings; preserve all original IDs and temporal classifications.
- Governing limitation: `AUTHORITATIVE CURRENT LINE FOR THIS AUDIT` remains `UNKNOWN — REQUIRES VERIFICATION`. TEAM-D may reconcile evidence and classified refs, but must not infer or designate the authoritative current line or issue a final CURRENT-state/gate judgment.
- Recorded disposition: `REOPENED — v1.1 REQUIRED AFTER TEAM-C1 v1.1`; v1.0 remains preserved.
- Reopen basis: Seal chronology defect; §34 Crosswalk field omissions; newly observed Sync lifecycle user/device ownership scope; mandatory-register contract recheck.
- Next permitted action: After C1 v1.1 handoff, issue complete D v1.1 under `04_TEAM-D/v1.1/`.

## TEAM-C2

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: TEAM-D must be sealed and handed off.
- Required scope: Build the target architecture proposal from the sealed TEAM-D reconciliation and predecessor packages without treating proposals as implemented state.
- Governing limitations: `AUTHORITATIVE CURRENT LINE` remains `UNKNOWN`; confirmed P0 risks and `DB-GOV-001` must be preserved as design constraints, not silently remediated.
- Recorded disposition: `REOPENED — RETURN FOR REWORK`; v1.0 hashes/content were complete, but its internal evidence-collection end time `02:19:00Z` postdates its declared closure `02:12:51Z`.
- Next permitted action: Preserve v1.0 and its unsealed v1.1 draft; wait for accepted C1/D v1.1, then reassess and seal a complete C2 v1.1.

## TEAM-E

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `HOLD`
- Prerequisite: TEAM-C2 must be sealed and handed off.
- Required scope: Multidisciplinary advisory review of sealed TEAM-D reconciliation and TEAM-C2 target proposal; review every P0/P1 and a justified P2/P3 sample; preserve assurance and authority limitations.
- Recorded disposition: `HOLD FINAL SEAL — REOPEN CHAIN C1 → D → C2 REQUIRED`; current review evidence is preserved.
- Next permitted action: Resume/re-review after accepted C1 v1.1, D v1.1, and C2 v1.1 handoffs; do not seal now.

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
