# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, its mission-local `CURRENT_DIRECTIVE.md`, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## Governing owner decisions now in force

Authoritative current product line:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

PR #69 `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED REMEDIATION / FINAL CANDIDATE`, not CURRENT. No merge is authorized.

Target database authority:

`DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.

## MISSION-01 TEAMS

- TEAM-A: `STOP — SEALED — DELIVERED TO CONTROL TOWER`.
- TEAM-B: `STOP — SEALED — DELIVERED TO CONTROL TOWER`; `BLK-B-001` retained.
- TEAM-C1: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-D: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-C2: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-E: `STOP — v1.1 SEALED`; v1.0 preserved/rejected for downstream use.
- MASTER/GATE: `STOP — v2.0 SEALED — READY FOR REMEDIATION PLANNING`; v1.0 preserved as historical sealed evidence.

## MISSION-02

- `CURRENT DIRECTIVE`: `STOP`.
- Recorded disposition: `MISSION-02-v1.2 — SEALED — DELIVERED TO CONTROL TOWER — READY FOR MISSION-03`.
- Accepted planning scope: 64/64 findings; both P0s; all governing P1s; 8/8 workstreams `PLANNED`; 20 remediation packages; waves `W0–W8`; all proposed DB changes gated through `DB-GOV-001`.
- Product modification authority exercised by MISSION-02: `NONE`.

## MISSION-03

- `CURRENT DIRECTIVE`: `CONTINUE — DBP-002 INDEPENDENT POST-REHEARSAL DB-GOV REVIEW ONLY; DBP-004 EARLY EXECUTION DETECTED — STOP/HOLD DBP-004 — NO FURTHER DBP-004 PRODUCT MODIFICATION`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Frozen DBP-002 post-rehearsal review target: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Current observed execution-branch head: `codex/mission-03-execution-20260828@c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, tree `74caed5d25a99efd13ceb86a79adc71f938f5bda`, parent `1750fe82e39107de36129cb0420adc622829dc9e`.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Governing DB-GOV decision

The fresh post-correction pre-authoring decision remains `DB-GOV VERDICT = PASS` and authorizes bounded non-Production authoring/rehearsal in this single order only:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes work; it does not pre-accept any generated candidate. The immediately preceding Control Tower directive required DBP-002 independent post-rehearsal acceptance before DBP-004 could start.

### DBP-002 frozen review target

The immutable `ffdf1087...` checkpoint has the previously verified successful technical evidence:

- `MISSION-03 DBP-002 Full Rehearsal v3` run `33222541097 = SUCCESS`.
- W0 run `33222541108 = SUCCESS`.
- W7 PostgreSQL backup/restore run `33222541109 = SUCCESS`.
- Legacy Full Rehearsal v2 run `33222541073 = FAIL` at baseline catalog backup/restore reconciliation and still requires explicit independent disposition.

DBP-002 remains **not independently accepted**.

`DBP-002 POST-REHEARSAL DB-GOV REVIEW = START AUTHORIZED — WAITING FOR INDEPENDENT REVIEW EVIDENCE`.

The independent reviewer must verify the frozen `ffdf1087...` report + evidence + manifest + SHA-256 package, exact SHA/tree/parent, original-ten migration preservation, generated SQL/hashes, PostgreSQL 18.6 apply, RLS/catalog/roles/ACL/FK, negative isolation/fail-closed behavior, W0/full regression and recovery evidence, and then issue a fresh independent `PASS` or `FAIL`.

### DBP-004 gate violation and stop

Before any independent DBP-002 acceptance was recorded, the execution branch advanced through two explicit DBP-004 commits:

- `1750fe82e39107de36129cb0420adc622829dc9e`, parent `ffdf1087...`: DBP-004 Audit V2 product-source authoring, 556 additions.
- `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, parent `1750fe82...`: adds the DBP-004 PostgreSQL 18.6 EF candidate-generator workflow.

The generator run `33223141635 = FAILURE`: the authoring-head build failed and migration generation was skipped. Automatic gates on the new branch head are also red: DBP-002 Full Rehearsal v3 `33223141626 = FAILURE`, W0 `33223141611 = FAILURE`, and W7 `33223141566 = FAILURE`.

`DBP-004 = HOLD/STOP — UNAUTHORIZED EARLY EXECUTION DETECTED — PRESERVE COMMITS AS UNACCEPTED CANDIDATE EVIDENCE — NO FURTHER PRODUCT MODIFICATION`.

Do not delete, revert, squash, rebase, cherry-pick, force-push or rewrite the early DBP-004 commits. They are preserved evidence only, not accepted baselines. Only after a fresh independent DBP-002 PASS is recorded may Control Tower explicitly release DBP-004 and determine whether the preserved early candidate can be inspected under the newly authorized boundary.

Continue unrelated W5/W6/W7 work only where its own gates remain independent and it does not modify or rely on the unauthorized DBP-004 state. W8 stays last; no destructive/global cleanup before preservation gates.

### Prohibitions

No further DBP-004 Product Source, Tests, Entities, DbContext, Migrations, schema, seed, persistent-adapter or production-configuration changes until explicit Control Tower release after DBP-002 independent acceptance.

No Production database/data/configuration/credentials. No Production secrets. Do not edit/delete/squash the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

No `OWNER DECISION REQUIRED` is active; the next action is the already-authorized independent DBP-002 governance review on the isolated frozen checkpoint.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Production Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit Production authority.
