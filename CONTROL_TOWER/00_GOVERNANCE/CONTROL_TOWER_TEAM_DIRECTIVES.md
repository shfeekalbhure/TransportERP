# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## Governing owner decision now in force

Owner authority designates the authoritative current product line as:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

Decision record:

`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/AUTHORITATIVE_PRODUCT_LINE_DECISION_2026-08-28.md`

PR #69 `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED REMEDIATION / FINAL CANDIDATE`, not CURRENT. No merge is authorized.

## TEAM-A

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`
- Reason: Package, manifest hashes, seal, and handoff were verified and centrally received.
- Next permitted action: None unless new evidence requires controlled `REOPEN`.

## TEAM-B

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `SEALED — DELIVERED TO CONTROL TOWER — STOP`
- Governing carry-forward: `SINGLE-SESSION TEAM-B — MULTI-REVIEWER ASSURANCE LIMITATION RECORDED` (`BLK-B-001`).
- Next permitted action: None unless new evidence requires controlled `REOPEN`.

## TEAM-C1

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `v1.1 SEALED — DELIVERED TO CONTROL TOWER — STOP`; v1.0 remains preserved and superseded for downstream use.
- Next permitted action: None unless new evidence causes `REOPEN REQUIRED`.

## TEAM-D

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `v1.1 SEALED — DELIVERED TO CONTROL TOWER — STOP`; v1.0 remains preserved and superseded for downstream use.
- Historical limitation at seal time: authoritative current line was unknown. That authority question is now resolved by the owner decision above; TEAM-D's sealed historical package is not silently rewritten.
- Next permitted action: None unless MASTER/GATE revalidation proves a specific TEAM-D correction is required.

## TEAM-C2

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `v1.1 SEALED — DELIVERED TO CONTROL TOWER — STOP`; v1.0 remains preserved and superseded for downstream use.
- Next permitted action: None unless MASTER/GATE revalidation proves reassessment is required.

## TEAM-E

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `STOP`
- Recorded disposition: `v1.1 SEALED — DELIVERED TO CONTROL TOWER — STOP`; v1.0 remains preserved and rejected for downstream use.
- Governing assurance limitation `BLK-B-001` remains in the final narrative.
- Next permitted action: None unless MASTER/GATE revalidation proves advisory reassessment is required.

## MASTER REPORT + RECONCILIATION GATE

- Mission: `MISSION-01`
- `CURRENT DIRECTIVE`: `REOPEN`
- Reopen basis: owner/repository authority has designated the authoritative product ref and full SHA.
- Required target: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Required action: revalidate all affected current-state claims, findings, blockers, counts, preservation classifications, and every mandatory gate condition on the exact SHA; preserve the sealed v1.0 package; create a new version, hashes, seal, and handoff.
- Do not convert the gate to READY merely because the authority blocker is resolved. Other critical evidence gaps must be independently resolved or remain explicit blockers.
- Next permitted action: perform read-only MASTER/GATE revalidation only.

## MISSION-02

- `CURRENT DIRECTIVE`: `WAIT — PENDING REVALIDATED MISSION-01 GATE`
- Owner-decision blocker for authoritative line: `RESOLVED`.
- Prerequisite: the reopened MISSION-01 gate must be resealed and formally state `READY FOR REMEDIATION PLANNING`.
- If the revalidated gate is READY, Control Tower shall change MISSION-02 to `START` automatically without another owner message.
- If the revalidated gate remains NOT READY, retain WAIT/HOLD only for the evidence conditions explicitly identified by the new gate.

## MISSION-03

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-02 must be sealed and handed off.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-03 must be sealed and handed off.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No team may execute a Database, Schema, Entity, Migration, field, or relationship change without the required governance and execution authorization.
