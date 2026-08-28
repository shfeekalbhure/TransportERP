# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must read its own section here before starting or resuming work. Only Control Tower changes a `CURRENT DIRECTIVE`. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

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
- `CURRENT DIRECTIVE`: `HOLD — OWNER DECISION REQUIRED`
- Prerequisite status: TEAM-A, TEAM-B, and TEAM-C1 are sealed and centrally received.
- Hold reason: `AUTHORITATIVE CURRENT LINE FOR THIS AUDIT` is unresolved; TEAM-A reports governing P0 findings, including valuable-work preservation risk, while TEAM-B reports zero confirmed P0.
- Next permitted action: Read this directive only and wait. Do not start reconciliation until Control Tower records `START`.

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
