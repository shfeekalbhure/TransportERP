# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 RETURN FOR EXACT-HEAD POST-REHEARSAL EVIDENCE PACKAGING; INDEPENDENT ACCEPTANCE BLOCKED UNTIL PACKAGE EXISTS; DBP-004 HOLD/STOP — NO FURTHER DBP-004 PRODUCT MODIFICATION`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Frozen DBP-002 post-rehearsal target: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Current observed execution-branch head: `codex/mission-03-execution-20260828@c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, tree `74caed5d25a99efd13ceb86a79adc71f938f5bda`, parent `1750fe82e39107de36129cb0420adc622829dc9e`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY` at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`.

## Governing authority

The fresh post-correction pre-authoring DB-GOV decision remains `DB-GOV VERDICT = PASS` for bounded candidate authoring/rehearsal in this order only:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

That PASS authorizes bounded work; it does not pre-accept a generated candidate. DBP-002 requires independent post-rehearsal acceptance before DBP-004 may be released.

## Independent DBP-002 intake result

Control Tower independently reverified the frozen exact-head technical run identities:

- Full Rehearsal v3 `33222541097 = SUCCESS` at `ffdf1087...`.
- W0 `33222541108 = SUCCESS` at `ffdf1087...`.
- W7 disposable recovery `33222541109 = SUCCESS` at `ffdf1087...`.
- Full Rehearsal v2 `33222541073 = FAILURE` at `ffdf1087...`; the failing step is `Baseline catalog backup restore reconciliation`, with later candidate/RLS/regression/recovery steps skipped in that workflow.

However, the authoritative repository package required for independent acceptance does not exist at the frozen SHA. `EXECUTION_OUTPUT_MANIFEST.md`, `EXECUTION_OUTPUT_SHA256_v1.1.txt`, `TEST_EXECUTION_REGISTER.md`, and `TRANSPORTERP_MASTER_REMEDIATION_EXECUTION_REPORT.md` remain bound to the earlier `5d1352b4...` / `005121...` checkpoint, and `EVIDENCE/` contains no DBP-002 post-rehearsal exact-head package.

Formal intake decision:

`DBP-002 POST-REHEARSAL DB-GOV INTAKE = FAIL / RETURN FOR EVIDENCE PACKAGING`

Decision record:

`CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-002_POST_REHEARSAL_DB_GOV_INTAKE_DECISION_2026-09-07.md`

This is an evidence-integrity/intake failure, not a technical rejection of the frozen candidate.

## Binding disposition

`DBP-002 = FROZEN TECHNICAL CANDIDATE AT ffdf1087... — TECHNICAL RUNS RETAINED — GOVERNANCE ACCEPTANCE BLOCKED — NOT ACCEPTED`

`DBP-002 EVIDENCE PACKAGING = START AUTHORIZED — WAITING FOR WORKER SESSION`

`DBP-004 = HOLD/STOP — UNAUTHORIZED EARLY EXECUTION DETECTED — PRESERVE COMMITS AS UNACCEPTED CANDIDATE EVIDENCE — NO FURTHER PRODUCT MODIFICATION`

The early DBP-004 commits `1750fe82...` and `c3f2b7b4...` must be preserved. Do not delete, revert, squash, rebase, cherry-pick, force-push, or rewrite them as part of supervision. They are not accepted baselines and must not be merged or treated as accepted work.

## Required next action

The only DB-sequenced worker action authorized now is non-destructive evidence packaging for the frozen DBP-002 checkpoint. MISSION-03 must publish in its own mission directory, without owner copy/paste:

1. an exact-head post-rehearsal DBP-002 report bound to `ffdf1087...` / tree `e8289418...` / parent `f128d24d...`;
2. repository-local evidence/index entries for v3, W0, W7 and the retained v2 failure, including exact run/job identities and required candidate/source/migration/generated-SQL/artifact hashes;
3. an explicit v2-versus-v3 technical disposition;
4. a new versioned post-rehearsal manifest bound to the frozen exact head;
5. a new detached SHA-256 list generated only after package stabilization.

Do not mark this evidence-packaging action `IN PROGRESS` merely because START is written here. It becomes in progress only when repository evidence shows a worker actually began producing the required outputs.

After the package exists, Control Tower must independently verify report + evidence + manifest + detached SHA-256 + exact SHA/tree/parent and then issue a fresh DBP-002 `PASS` or `FAIL`.

Only after a valid independent DBP-002 PASS is recorded may Control Tower explicitly release DBP-004. Existing early DBP-004 commits gain no retroactive acceptance merely because they exist.

## Parallel and later work

Continue unrelated W5/W6/W7 work only where its own gates remain independent and where it does not modify or rely on the unauthorized DBP-004 state. W8 stays last and no destructive/global cleanup is authorized before preservation gates.

## Prohibitions

No further DBP-004 Product Source, Tests, Entities, DbContext, Migrations, schema, seed, persistent-adapter or production-configuration changes until Control Tower records DBP-002 independent acceptance and explicitly releases DBP-004.

No Production database/data/configuration/credentials. No Production secrets. No edit/delete/squash of the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

Control Tower supervision does not modify Product Source, Tests, Migrations, production configuration or databases.

## Return rule

Do not return to the owner after each DBP or Wave. Continue automatically through all enabled MISSION-03 work.

Return only for:

1. `MISSION-03 = COMPLETE — SEALED — DELIVERED TO CONTROL TOWER`; or
2. a genuinely new owner-reserved decision not already covered by current decisions; or
3. a true external-access blocker after all internally permitted work is exhausted.

No `OWNER DECISION REQUIRED` is active. MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 report + evidence + manifest + detached SHA-256 + seal + handoff exists.
