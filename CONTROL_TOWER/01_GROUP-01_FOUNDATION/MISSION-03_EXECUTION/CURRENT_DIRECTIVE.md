# CURRENT DIRECTIVE — MISSION-03

`EXTERNAL EVIDENCE REQUIRED — ALL INTERNAL WORK EXHAUSTED; KEEP MISSION-03 OPEN`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch/baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`.
- Execution tree: `00512125311306a43474638195d2cad97b76118e`.
- W2 B2B code-only checkpoint: independently reverified; run `33191269475 = 146/146 PASS`; no new persistence Product change.
- PR #69 remains `UNMERGED EVIDENCE ONLY`.

## Owner decisions now binding

### AUTH-001

`AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`

### ACC-001

Decision file:
`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/ACC-001_ACCOUNTING_POSTING_AUTHORITY_2026-08-28.md`

`ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`

MISSION-03 must implement W3/accounting work against this model. Collection itself is an operational immutable event and need not have a pre-posted voucher. Settlement is the atomic accounting boundary for voucher+journal posting and source linkage. Configured account roles, FX/rounding, maker-checker (`SoD threshold = 0`) and period rules in the decision are authoritative.

### OFFLINE-001

Decision file:
`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/OFFLINE-001_PER_ACTION_AUTHORITY_2026-08-28.md`

`OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`

MISSION-03 must materialize the exact per-action matrix from canonical action registers and current Product code. Draft/replay-safe operational capture can be queued only with the full governed envelope. Security/admin/accounting posting/settlement/reversal/period/destructive/authority-changing actions remain online-authoritative. Any action not clearly allow-listed remains DENY. No additional owner approval is needed to perform this deterministic mapping.

### CLIENT-001

Decision file:
`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/CLIENT-001_DELIVERY_SIGNING_SCOPE_2026-08-28.md`

`CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS IS DEFERRED`

Targets:
- Windows x64 Desktop: `TransportERP.Desktop`.
- Android Admin: `com.altayer.transporterp.admin`.
- Android Customer: `com.altayer.transporterp.customer`.
- Android Driver: `com.altayer.transporterp.driver`.

Current library/scaffold builds are not release evidence. W5 must produce executable/runtime proof for every target. Signing private material remains external protected custody; no key is authorized to be committed.

## DB-GOV state

Owner decisions do not override DB-GOV.

- `DBP-003 = HOLD AT REHEARSAL ENTRY` remains binding until independently released.
- `DBP-003A` revised proposal may continue through review/evidence preparation only unless DB-GOV explicitly opens disposable/safe-copy rehearsal.
- `DBP-003B/C` remain dependent on DBP-002/006.
- DBP-002/003/004/005/006 and later proposals authorize no Entity/DbContext/Migration/Schema/Data mutation unless their exact DB-GOV gate is opened.

## Remaining authorized external evidence

The owner-decision gate is closed. Remaining true external inputs are limited to the evidence listed in `MISSION03_COMPLETION_GATE_ASSESSMENT.md`, including:

- authorized sanitized PasswordHash/verifier/legacy policy evidence;
- named non-Production safe copy with migration/roles/RLS/data-shape/backup/restore/reconciliation proof;
- sanitized legacy audit/accounting reconciliation samples;
- latest canonical Kurrasa/Ticketing/post-departure Shipping authority;
- deploy/recovery/RPO-RTO/signing custody/dependency/privacy/KMS evidence;
- complete external workspace/stash/local-only inventory before W8 cleanup.

Do not fabricate any missing external evidence.

## Execution direction

MISSION-03 must resume automatically from `cc67ad2...` and continue all work enabled by the resolved owner decisions without returning for another command:

1. Rebind W3 accounting/UoW/audit packages to ACC-001 and complete every code/design/test item whose dependencies are satisfied.
2. Materialize OFFLINE-001 into the exact W4 per-action matrix and implement/test every independently permitted queue/replay path; leave ambiguous or prohibited actions DENY.
3. Rebind W5 to CLIENT-001 and convert target scaffolds to real executables/runtimes where the sealed plan permits and no external signing secret is required; unsigned test builds do not satisfy final release proof.
4. Continue DBP proposal revision/re-submission and all non-destructive safe-copy tooling/evidence preparation.
5. Continue W6/W7 preparation and any executable package whose canonical authority is available.
6. Enter W8 only after W7 stability and preservation inventory are proven.

A blocked package must not stop unrelated satisfied packages.

## Prohibitions

Do not merge to master. Do not rebase, cherry-pick, force-push or rewrite history. Do not mutate Production. Do not commit signing/private secrets. Do not cross a DB-GOV gate. Do not start MISSION-04 before a valid final MISSION-03 seal/handoff.

Before every material Product commit, re-fetch this file and the governance branch head.

## v1.0 exhaustion directive

All work that can lawfully be completed from repository source, reachable
history, current CI and disposable synthetic evidence has now been executed or
prepared. Preserve `5d1352b...` as the bounded execution head. Do not open a
new Product package until its named external evidence and independent DB-GOV
decision are available. Do not seal MISSION-03 and do not start MISSION-04.

Return to the owner only for a genuinely new owner-reserved decision not covered by AUTH-001/ACC-001/OFFLINE-001/CLIENT-001, or an external evidence source that cannot be obtained through authorized project resources. Otherwise continue toward full MISSION-03 completion.
