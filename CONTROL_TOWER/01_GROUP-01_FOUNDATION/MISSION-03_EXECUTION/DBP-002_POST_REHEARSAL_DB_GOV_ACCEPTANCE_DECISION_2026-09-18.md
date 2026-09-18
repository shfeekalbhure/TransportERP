# DBP-002 POST-REHEARSAL DB-GOV ACCEPTANCE DECISION

Date: 2026-09-18
Decision authority: Control Tower independent supervision
Mission: MISSION-03
Decision: `PASS — DBP-002 ACCEPTED AT FROZEN EXACT HEAD`

## Accepted exact identity

- Execution branch at evidence time: `codex/mission-03-execution-20260828`
- SHA: `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`
- Tree: `e828941817432bdc73f3e6fc31e74219e74fcf33`
- Parent: `f128d24dce7baf76a6ac8af4e62a331b80447311`
- Package: `MISSION-03-DBP002-POST-REHEARSAL-v1.2`

This acceptance is bound only to that exact SHA/tree/parent. It does not accept later execution-branch commits.

## Repository package verification

The previously missing acceptance package is now present under the authoritative `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/` record and was published by worker evidence commits directly on top of the prior Control Tower head:

- `dc5ac50c199283eb2a3a8860b12cb32768ee65c2` — stabilized v1.2 report/evidence/manifest/test-register package.
- `7df63e059721a9b925a55cad6596a0c54ea193bc` — detached SHA-256 register added after stabilization.

Verified package components:

1. `DBP-002_POST_REHEARSAL_EXECUTION_REPORT_2026-09-18.md` binds the evidence to the frozen SHA/tree/parent and retains all four relevant runs.
2. `EVIDENCE/DBP-002/POST_REHEARSAL_GITHUB_ACTIONS_RUN_EVIDENCE.md` and `POST_REHEARSAL_EVIDENCE_INDEX.md` provide repository-local run/artifact evidence.
3. `DBP-002_V2_V3_TECHNICAL_DISPOSITION_2026-09-18.md` explicitly retains the v2 red result and explains why it is a pre-candidate raw textual catalog-deparse false negative rather than a candidate defect.
4. `EXECUTION_OUTPUT_MANIFEST.md` is version `MISSION-03-DBP002-POST-REHEARSAL-v1.2`, stabilized for independent re-review and bound to the frozen exact identity.
5. `TEST_EXECUTION_REGISTER.md` records the exact-head post-rehearsal evidence.
6. `EXECUTION_OUTPUT_SHA256_v1.2.txt` is detached in the subsequent commit and enumerates SHA-256 values for the six stabilized package files. The sidecar was not included in its own hash set, avoiding self-reference.

The earlier intake failure is therefore superseded for current operation by this complete package. The intake failure remains historical evidence and is not deleted or rewritten.

## Independent technical revalidation

Control Tower independently re-read GitHub Actions metadata and exact-head logs rather than relying only on the worker report.

### Full Rehearsal v3 — run 33222541097 / job 99019447103

- GitHub binds the run to exact head `ffdf1087...` and tree `e8289418...`.
- PostgreSQL 18.6.
- Original ten migrations are proven unchanged relative to the pre-authoring baseline.
- Build succeeds and EF reports no pending model changes.
- Migration 11 is generated and applied.
- Baseline backup/restore is structurally reconciled.
- Generated-SQL and EF candidate physical state are reconciled semantically/structurally.
- RLS/ACL/scope/fail-closed/negative-path checks complete.
- Full regression: `155 passed / 0 failed / 0 skipped`.
- Final candidate backup/restore structural reconciliation completes.
- Terminal marker: `DBP-002 TECHNICAL REHEARSAL PASS`.

### W0 — run 33222541108

- Exact frozen SHA/tree/parent verified.
- PostgreSQL 18.6 candidate database applies all 11 migrations.
- EF reports no pending model changes.
- Full regression: `155 passed / 0 failed / 0 skipped`.
- API protected-boundary probe returns expected HTTP 401.
- Mobile probes/builds and Windows Desktop job complete successfully.

### W7 — run 33222541109 / job 99019447043

- Exact frozen SHA/tree/parent verified.
- EF reports no pending model changes.
- Source migrations: 11; restored migrations: 11.
- `restore_result=PASS`.
- Disposable backup/restore artifacts and SHA-256 digests are retained.

### Retained v2 failure — run 33222541073

The v2 result remains `FAILURE`. It fails at baseline catalog backup/restore reconciliation before candidate SQL, Migration 11, RLS checks, full regression, or candidate recovery executes. The exact v2 workflow gates on raw `pg_get_*` textual catalog representations. The corrected v3 path uses structural/semantic reconciliation and then completes the entire candidate path on the same frozen exact head. Accordingly:

`v2 = RETAINED FAILURE / HARNESS-SPECIFIC FALSE NEGATIVE`

`v3 = CORRECTED SUPERSEDING REHEARSAL PATH FOR DBP-002 TECHNICAL ACCEPTANCE`

The v2 failure is not converted to PASS and remains in the evidence record.

## Acceptance scope

`DBP-002 = ACCEPTED — EXACT-HEAD ACCEPTANCE ONLY`

This acceptance authorizes the next DB-GOV sequence gate only. It does not authorize Production deployment, Production data/configuration, migration of any live database, or acceptance of later DBP work.

The governing DB sequence remains:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Therefore DBP-004 may now be released for bounded non-Production authoring/rehearsal.

## DBP-004 release treatment

The historical early DBP-004 commits remain preserved, unaccepted evidence:

- `1750fe82e39107de36129cb0420adc622829dc9e`
- `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0`

They do not gain retroactive acceptance from this decision. Their failed generator/gate runs remain retained evidence.

Fresh authorized DBP-004 work must begin from the accepted DBP-002 boundary `ffdf1087...`, on a non-destructive worker line. The preserved early commits may be inspected as read-only comparative evidence only; do not merge, cherry-pick, rebase, squash, revert, force-push, or rewrite them as part of supervision.

Formal next state:

`DBP-004 START AUTHORIZED — WAITING FOR WORKER SESSION`

Do not mark DBP-004 `IN PROGRESS` until repository evidence shows a worker actually begins producing DBP-004 outputs under this authorized boundary.

## Mission state

- `MISSION-03 = IN PROGRESS — OPEN — NOT SEALED`.
- `MISSION-04 = WAIT — NOT STARTED`.
- No final MISSION-03 seal or successor handoff is issued by this decision.
- `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` remains the authoritative current product line.
- PR #69 at `601f2d1cad61d62e590a6714ad84e307eb84fe5f` remains OPEN / DRAFT / UNMERGED and is not CURRENT.
- No `OWNER DECISION REQUIRED` is active.
