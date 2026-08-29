# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 INDEPENDENT POST-REHEARSAL DB-GOV REVIEW ONLY; DBP-004 EARLY EXECUTION DETECTED — STOP/HOLD DBP-004 — NO FURTHER DBP-004 PRODUCT MODIFICATION`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Frozen DBP-002 post-rehearsal review target: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Current execution-branch head now observed: `codex/mission-03-execution-20260828@c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, tree `74caed5d25a99efd13ceb86a79adc71f938f5bda`, parent `1750fe82e39107de36129cb0420adc622829dc9e`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY` at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`.

## Governing authority

The fresh post-correction pre-authoring DB-GOV decision remains:

`DB-GOV VERDICT = PASS`

It authorizes bounded candidate authoring/rehearsal only in this order:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes the workstream; it does not pre-accept a generated candidate. The immediately preceding Control Tower directive explicitly required DBP-002 independent post-rehearsal acceptance before DBP-004 could start.

## Material gate violation now verified

DBP-002 has **not** received an independent post-rehearsal DB-GOV acceptance in the authoritative Control Tower record. Nevertheless the execution branch advanced beyond the frozen DBP-002 review target:

1. `1750fe82e39107de36129cb0420adc622829dc9e`, parent `ffdf1087...`, is explicitly a `MISSION-03 DBP-004` product-source commit. It adds the Audit V2 model/canonicalization/appender foundation under `TransportERP.Infrastructure/Persistence` and changes 556 lines of product source.
2. `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`, parent `1750fe82...`, adds `.github/workflows/mission-03-dbp004-generator.yml` to generate the proposed DBP-004 migration candidate.
3. The DBP-004 generator run `33223141635` on `c3f2b7b4...` completed `FAILURE`; `Build authoring head` failed, migration-generation was skipped, and no successful candidate-generation result exists.
4. The previously green DBP-002 Full Rehearsal v3 and W0 are also red when automatically rerun on the contaminated branch head: v3 run `33223141626 = FAILURE`; W0 run `33223141611 = FAILURE`; W7 run `33223141566 = FAILURE`.

These later failures do not retroactively invalidate the immutable `ffdf1087...` evidence package, but they prove that the current branch head is **not** an accepted DBP-002 or DBP-004 checkpoint and that the ordering gate was crossed before Control Tower authorization.

## Binding disposition

`DBP-002 = FROZEN TECHNICAL CANDIDATE AT ffdf1087... — AWAITING INDEPENDENT POST-REHEARSAL DB-GOV ACCEPTANCE — NOT ACCEPTED`

`DBP-002 POST-REHEARSAL DB-GOV REVIEW = START AUTHORIZED — WAITING FOR INDEPENDENT REVIEW EVIDENCE`

`DBP-004 = HOLD/STOP — UNAUTHORIZED EARLY EXECUTION DETECTED — PRESERVE COMMITS AS UNACCEPTED CANDIDATE EVIDENCE — NO FURTHER PRODUCT MODIFICATION`

The commits `1750fe82...` and `c3f2b7b4...` must be preserved. Do not delete, revert, squash, rebase, cherry-pick, force-push, or rewrite them as part of supervision. They are not authorized baselines and must not be merged or treated as accepted work.

## Required next action

The only DB-sequenced action now authorized is the already-dispatched independent DBP-002 post-rehearsal DB-GOV review against the immutable `ffdf1087...` package. The independent reviewer must:

1. verify and bind the DBP-002 report + evidence + manifest + SHA-256 to `ffdf1087...` / tree `e8289418...` / parent `f128d24d...`;
2. determine whether v3 is the valid corrected/superseding rehearsal path and explicitly disposition the red v2 result;
3. verify original-ten migration preservation, generated SQL/hashes, PostgreSQL 18.6 apply, RLS/ACL/catalog/FK, fail-closed/cross-tenant negatives, W0/full regression, backup/restore and recovery evidence at the frozen DBP-002 checkpoint;
4. issue a fresh independent `PASS` or `FAIL` with exact blockers.

Only after a valid independent DBP-002 PASS is recorded by Control Tower may DBP-004 be re-dispatched. At that point the early DBP-004 commits may be independently inspected as preserved candidate evidence, but they receive no retroactive acceptance merely because they already exist.

No alternate ordering is authorized.

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

MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 report + evidence + manifest + detached SHA-256 + seal + handoff exists.
