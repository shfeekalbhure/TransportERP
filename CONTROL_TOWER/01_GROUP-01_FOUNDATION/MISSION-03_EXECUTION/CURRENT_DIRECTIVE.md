# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 ACCEPTED AT FROZEN EXACT HEAD; DBP-004 START AUTHORIZED FROM ACCEPTED DBP-002 BOUNDARY — WAITING FOR WORKER SESSION`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Accepted DBP-002 exact head: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Current observed execution-branch head remains the later preserved DBP-004 evidence head: `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, tree `74caed5d25a99efd13ceb86a79adc71f938f5bda`, parent `1750fe82e39107de36129cb0420adc622829dc9e`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY` at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`.

## Governing DB order

The post-correction pre-authoring DB-GOV PASS remains binding and the only authorized physical order is:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

## DBP-002 acceptance

Control Tower independently revalidated package `MISSION-03-DBP002-POST-REHEARSAL-v1.2` and issued:

`DBP-002 POST-REHEARSAL DB-GOV = PASS`

`DBP-002 = ACCEPTED — EXACT-HEAD ACCEPTANCE ONLY`

Decision record:

`CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-002_POST_REHEARSAL_DB_GOV_ACCEPTANCE_DECISION_2026-09-18.md`

Accepted identity:

`ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce / e828941817432bdc73f3e6fc31e74219e74fcf33 / f128d24dce7baf76a6ac8af4e62a331b80447311`

The authoritative mission package now contains the exact-head report, repository-local evidence/index, explicit v2/v3 disposition, versioned manifest, test-register entry, and detached `EXECUTION_OUTPUT_SHA256_v1.2.txt` sidecar published after package stabilization.

Independent run verification retained:

- Full Rehearsal v3 `33222541097 = SUCCESS` on the accepted exact head; PostgreSQL 18.6, structural/semantic reconciliation, RLS/fail-closed negatives, `155/155` tests, and final candidate recovery all pass.
- W0 `33222541108 = SUCCESS`; all 11 migrations apply, no EF model drift, `155/155`, API HTTP 401 expected, client build/probe matrix passes.
- W7 `33222541109 = SUCCESS`; source migrations 11, restored migrations 11, `restore_result=PASS`.
- Full Rehearsal v2 `33222541073 = FAILURE` remains retained. It fails on raw textual baseline catalog-deparse reconciliation before candidate execution. It is recorded as `RETAINED FAILURE / HARNESS-SPECIFIC FALSE NEGATIVE`; it is not rewritten to PASS.

The prior `FAIL / RETURN FOR EVIDENCE PACKAGING` intake decision remains historical and is superseded for current operation by the 2026-09-18 acceptance decision.

## DBP-004 release

DBP-002 acceptance clears the sequence gate for DBP-004.

`DBP-004 START AUTHORIZED — WAITING FOR WORKER SESSION`

This START does not mean DBP-004 is IN PROGRESS. Mark it IN PROGRESS only when repository evidence shows a worker actually begins producing authorized DBP-004 outputs.

### Required worker boundary

Fresh DBP-004 work must begin from the accepted DBP-002 exact boundary `ffdf1087...` on a non-destructive worker line. The earlier unauthorized DBP-004 commits remain preserved evidence only:

- `1750fe82e39107de36129cb0420adc622829dc9e`
- `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`

Those commits receive no retroactive acceptance. They may be inspected as read-only comparative evidence, but must not be merged, cherry-picked, rebased, squashed, reverted, force-pushed, or rewritten by supervision. Their failed generator/gate evidence remains retained.

The authorized DBP-004 worker must produce a fresh report/evidence/manifest/SHA-256 chain and satisfy its own DB-GOV acceptance gates before any DBP-003B/C release.

## Parallel and later work

Continue unrelated W5/W6/W7 work only where its own gates remain independent and where it does not rely on unaccepted later DBP state. W8 stays last and no destructive/global cleanup is authorized before preservation gates.

## Prohibitions

No Production database/data/configuration/credentials. No Production secrets. No edit/delete/squash of the existing ten historical migrations. No destructive migration/down-migration reliance. No merge to master, no PR #69 merge, no rebase, cherry-pick, force-push or history rewrite as part of supervision.

Control Tower supervision does not modify Product Source, Tests, Migrations, production configuration or databases.

## Return rule

Do not return to the owner after each DBP or Wave. Continue automatically through all enabled MISSION-03 work.

Return only for:

1. `MISSION-03 = COMPLETE — SEALED — DELIVERED TO CONTROL TOWER`; or
2. a genuinely new owner-reserved decision not already covered by current decisions; or
3. a true external-access blocker after all internally permitted work is exhausted.

No `OWNER DECISION REQUIRED` is active. MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 report + evidence + manifest + detached SHA-256 + seal + handoff exists.
