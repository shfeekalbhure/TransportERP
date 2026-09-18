# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, this file, its mission order, its mission-local `CURRENT_DIRECTIVE.md`, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. START does not equal IN PROGRESS; worker execution must be evidenced in the repository first.

## Governing owner decisions now in force

- Authoritative current product line: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- PR #69 `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED REMEDIATION / FINAL CANDIDATE`, not CURRENT. No merge is authorized.
- Target database authority: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- `DB-GOV-001` remains binding.

## MISSION-01

All MISSION-01 teams and the Master/Gate are `STOP — SEALED — DELIVERED`; historical superseded versions remain preserved. Master/Gate v2.0 remains `READY FOR REMEDIATION PLANNING`.

## MISSION-02

- `CURRENT DIRECTIVE`: `STOP`.
- `MISSION-02-v1.2 = SEALED — DELIVERED TO CONTROL TOWER — READY FOR MISSION-03`.
- Product modification authority exercised by MISSION-02: `NONE`.

## MISSION-03

- `CURRENT DIRECTIVE`: `CONTINUE — DBP-002 ACCEPTED AT FROZEN EXACT HEAD; DBP-004 START AUTHORIZED FROM ACCEPTED DBP-002 BOUNDARY — WAITING FOR WORKER SESSION`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Accepted DBP-002 exact head: `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Current observed execution branch remains later at preserved, unaccepted DBP-004 evidence head `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Governing DB sequence

The post-correction pre-authoring DB-GOV PASS remains valid and authorizes bounded non-Production authoring/rehearsal only in this order:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Each DBP requires its own post-rehearsal acceptance before releasing the next gated DBP.

### DBP-002 — accepted

Control Tower independently verified package `MISSION-03-DBP002-POST-REHEARSAL-v1.2`, including exact-head report, repository-local evidence/index, explicit v2/v3 disposition, manifest, test-register entry, and detached SHA-256 sidecar published after stabilization.

Independent exact-head run evidence:

- Full Rehearsal v3 `33222541097 = SUCCESS`.
- W0 `33222541108 = SUCCESS`.
- W7 disposable recovery `33222541109 = SUCCESS`.
- Full Rehearsal v2 `33222541073 = FAILURE — RETAINED`.

The v2 failure occurs before candidate application at raw textual baseline catalog-deparse comparison. The corrected v3 structural/semantic path runs the complete candidate and passes PostgreSQL 18.6 reconciliation, RLS/fail-closed checks, `155/155` tests and final recovery on the same frozen exact head. The v2 result is retained and is not rewritten to PASS.

Formal state:

`DBP-002 POST-REHEARSAL DB-GOV = PASS`

`DBP-002 = ACCEPTED — EXACT-HEAD ACCEPTANCE ONLY`

Decision record: `MISSION-03_EXECUTION/DBP-002_POST_REHEARSAL_DB_GOV_ACCEPTANCE_DECISION_2026-09-18.md`.

### DBP-004 — released, not yet in progress

`DBP-004 START AUTHORIZED — WAITING FOR WORKER SESSION`

Do not mark DBP-004 IN PROGRESS until repository evidence shows a worker actually starts producing outputs under this authorization.

Fresh DBP-004 work must begin from accepted DBP-002 boundary `ffdf1087...` on a non-destructive worker line. Historical early DBP-004 commits remain preserved unaccepted evidence only:

- `1750fe82e39107de36129cb0420adc622829dc9e`
- `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`

They receive no retroactive acceptance. They may be inspected as read-only comparative evidence only. Do not merge, cherry-pick, rebase, squash, revert, force-push, delete, or rewrite them as part of supervision.

DBP-004 must generate its own fresh exact-head report + evidence + manifest + detached SHA-256 and pass its own DB-GOV review before DBP-003B/C can be released.

### Prohibitions

No Production database/data/configuration/credentials or Production secrets. No destructive migration/down-migration reliance. No merge to master or PR #69. No rebase, cherry-pick, force-push or history rewrite as part of supervision. Control Tower itself does not modify Product Source, Tests, Migrations, production configuration or databases.

No `OWNER DECISION REQUIRED` is active.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- MISSION-03 is open/not sealed; MISSION-04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be conclusively sealed and handed off.
