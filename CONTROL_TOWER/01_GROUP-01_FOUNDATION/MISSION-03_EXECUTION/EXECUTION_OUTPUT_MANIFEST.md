# MISSION-03 EXECUTION OUTPUT MANIFEST

Package version: `MISSION-03-DBP002-POST-REHEARSAL-v1.2`
Package state: `STABILIZED FOR INDEPENDENT DB-GOV RE-REVIEW — NOT SEALED`
Prepared from Control Tower checkpoint: `63d8b291b63415e07c3a9f94a55227f2eae5c384`

## Frozen DBP-002 execution identity

- SHA: `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`
- Tree: `e828941817432bdc73f3e6fc31e74219e74fcf33`
- Parent: `f128d24dce7baf76a6ac8af4e62a331b80447311`
- Execution branch at run time: `codex/mission-03-execution-20260828`

This identity, not the later current tip of the execution branch, is the candidate governed by this package.

## Package files

| File | Role | Package status |
|---|---|---|
| `DBP-002_POST_REHEARSAL_EXECUTION_REPORT_2026-09-18.md` | Exact-head post-rehearsal report | CURRENT v1.2 |
| `EVIDENCE/DBP-002/POST_REHEARSAL_GITHUB_ACTIONS_RUN_EVIDENCE.md` | Run/job/hash/artifact evidence | CURRENT v1.2 |
| `EVIDENCE/DBP-002/POST_REHEARSAL_EVIDENCE_INDEX.md` | Evidence index | CURRENT v1.2 |
| `DBP-002_V2_V3_TECHNICAL_DISPOSITION_2026-09-18.md` | v2/v3 technical disposition | CURRENT v1.2 |
| `TEST_EXECUTION_REGISTER.md` | Existing register with appended DBP-002 exact-head evidence | CURRENT v1.2 |
| `EXECUTION_OUTPUT_MANIFEST.md` | This manifest | CURRENT v1.2 |
| `EXECUTION_OUTPUT_SHA256_v1.2.txt` | Detached SHA-256 register generated after stabilization | SIDE-CAR / FINAL PACKAGE COMMIT |

## Run bindings

- Full Rehearsal v3: 33222541097 / job 99019447103 / SUCCESS / artifact 9705722045 / digest `232fd71285c2675dc7fbcdf9ad207d6d97fbe407a222fe6ac98db8729732527c`
- W0: 33222541108 / jobs 99019515884 + 99019515734 / SUCCESS / artifacts 9705724131 + 9705704277 / digests `09f0265a...` + `a6af8b22...`
- W7: 33222541109 / job 99019447043 / SUCCESS / artifact 9705704957 / digest `6e2e6edc...`
- Full Rehearsal v2: 33222541073 / job 99019446824 / FAILURE RETAINED / artifact 9705708441 / digest `8653061f...`

## Technical disposition

v2 remains an immutable failed run. Its failure is at raw baseline catalog text reconciliation before candidate application. v3 is the corrected structural/semantic reconciliation path and completes the frozen candidate end-to-end. See `DBP-002_V2_V3_TECHNICAL_DISPOSITION_2026-09-18.md`.

## Detached hash rule

`EXECUTION_OUTPUT_SHA256_v1.2.txt` must be generated only after all non-sidecar package contents are stabilized. The sidecar hashes the exact UTF-8 bytes of the six content files listed above and does not hash itself. Creation of the sidecar must be a separate final package commit.

## Preservation / non-authority

- DBP-002 governance acceptance remains pending independent DB-GOV re-review.
- DBP-004 remains HOLD/STOP and is not accepted or modified by this package.
- `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` is not modified.
- PR #69 at `601f2d1cad61d62e590a6714ad84e307eb84fe5f` remains OPEN / DRAFT / UNMERGED and is not modified.
- No Production authority, final MISSION-03 seal, final handoff, or successor-mission readiness is asserted.
