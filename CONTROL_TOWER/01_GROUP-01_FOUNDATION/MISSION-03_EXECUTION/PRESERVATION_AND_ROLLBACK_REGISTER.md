# Preservation and Rollback Register

| PRES | W0 action/result | State |
|---|---|---|
| `PRES-001` | exact master/ref/tree and 378-entry tracked tree recorded | `SATISFIED FOR REPOSITORY BASELINE` |
| `PRES-002` | M02 parent/delivery and current governance lineage retained | `SATISFIED` |
| `PRES-003` | sealed M02 checksums passed; sealed predecessors not edited | `SATISFIED` |
| `PRES-004` | PR69 ref/tree/delta frozen; no merge/rebase/cherry-pick/copy | `SATISFIED` |
| `PRES-005` | 50 remote heads bundled; local refs/stash inventoried; an additive detached worktree verifies exact remote execution head `9c5b7a1...`; external workspaces remain unknown | `PARTIAL — EXTERNAL INVENTORY BLOCKED` |
| `PRES-006` | ordered migration hashes retained; model-drift check passed and all 10 committed migrations applied to an empty disposable PostgreSQL 18.6 DB; live applied history remains unavailable | `SATISFIED FOR EXECUTION / LIVE UNKNOWN` |
| `PRES-007` | no data access or mutation | `NOT ACTIVATED; LIVE IMPACT UNKNOWN` |
| `PRES-008` | fresh exact-head runs passed before and after REM-100: 124/124 then 125/125 | `SATISFIED FOR W0/W1` |
| `PRES-009` | audit bytes untouched | `PRESERVED / NOT ACTIVATED` |
| `PRES-010` | accounting history untouched | `PRESERVED / NOT ACTIVATED` |
| `PRES-011` | contracts untouched; only missing mapper assignment was added | `SATISFIED FOR REM-100` |
| `PRES-012` | Offline payload/history untouched | `PRESERVED / NOT ACTIVATED` |
| `PRES-013` | screen/client assets untouched | `PRESERVED / NOT ACTIVATED` |
| `PRES-014` | runtime execution retained 124 pre-change tests and added one focused regression; 125/125 pass | `SATISFIED FOR W0/W1` |
| `PRES-015` | no Production secret/data used; logs record tool absence and synthetic CI only | `SATISFIED FOR W0` |
| `PRES-W2-001` | current tenant predicates and permission codes retained; controls added at service boundary | `SATISFIED FOR W2-A1/B1/C1` |
| `PRES-W2-002` | no Entity/DbContext/Migration/Seed/Schema/data change; ten-migration lineage and model snapshot unchanged | `SATISFIED` |
| `PRES-W2-003` | Sync IDs, statuses, retry/conflict history and audit appends retained; only unauthorized ownership paths are denied | `SATISFIED` |
| `PRES-W2-004` | Waybill/Finance/Shipping routes and application contracts retained; duplicated claim-only checks replaced by one fail-closed request resolver | `SATISFIED FOR W2-A2/B2A` |
| `PRES-W2-005` | failed run 33184771338 stopped at compile before migration/test/API; disposable PostgreSQL was discarded; corrective head d740740 passed the full matrix | `RECOVERED — NO PARTIAL DB/PRODUCT MUTATION` |
| `PRES-W2-006` | Control Tower revalidated linear W1→W2 ancestry, exact 15-path diff, retained artifacts and normal-revert recovery; no merge/rebase/cherry-pick/force-push or candidate deletion | `SATISFIED FOR BOUNDED ADOPTION` |
| `PRES-W2-007` | B2B was added linearly from `9c5b7a1...` as three new code/test files; existing auth configuration, entities, DbContext, migrations, schema, data, routes and client projects untouched | `SATISFIED FOR CODE-ONLY B2B` |
| `PRES-W2-008` | raw passwords/tokens/signing keys/private keys are not logged or persisted; no test store is registered in API; local issuance remains fail-closed until durable DBP-003 adapters | `SATISFIED FOR CODE-ONLY B2B` |
| `PRES-W2-009` | exact-head run `33191269475` passed 146/146, ten existing migrations/no drift, API 401 and Desktop/Mobile probes | `SATISFIED FOR CODE-ONLY B2B` |

## Recovery evidence

- Bundle: `/workspace/scratch/2cc4cde701d9/MISSION03_W0_PRESERVATION_20260828.bundle`
- Size: `29,758,224` bytes.
- SHA-256: `aebcb2399f61295eb002a92c8a8392917d146a06159a47517b3338d52aa4428b`.
- `git bundle verify`: PASS; complete history; 54 refs.
- Recovery rehearsal: cloned bundle into a temporary repository; recovered authoritative commit/tree, PR69 commit/tree and governance head; `git fsck --no-dangling` passed.

## Rollback status

The entries below document recovery paths only. The candidate is adopted for bounded execution, but no rollback, deletion or history rewrite is currently requested or performed.

- W0 evidence harness rollback: revert `a48b68023072122c3f71941b861d8b9eeca82d34`; authoritative master is unchanged.
- REM-100 rollback: revert `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`, restoring the mapper and test files to the W0 tree. No schema/data rollback is required because no DB/data mutation exists.
- W2 A2/B2A rollback: revert `9c5b7a1...`, then `d740740...`, then `d1c0a2571bf3d240b9134e8614186acd70a6bd5d` to restore exact head `04a875a...`. The intermediate `d1c0a257...` is not a deployable rollback target because its core build failed.
- W2 A1/B1/C1 rollback: from `04a875a...`, revert `04a875a...` only to remove manual dispatch capability if required, then revert `a157c34d6767deeb5544adf456a2a36946a599a9` to restore the W1 code tree. No schema/data rollback exists. Stop and preserve evidence if rollback would re-open a security exposure in an environment where this branch has been deployed.
- W2-B2B code-only rollback: normally revert `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`, restoring exact adopted baseline `9c5b7a1...`. The API never registered the new lifecycle or a temporary store, so rollback has no token/session/data cleanup. If later DBP-003 work activates local sessions, use its feature-disable/family-revoke/forward-recovery path instead of destructive schema rollback.
- Recovery source remains authoritative master plus the verified preservation bundle. No merge, deletion, force-push or history rewrite was performed.

## v0.9 preparation preservation

- Product execution head/tree remained exactly `cc67ad2...` / `ea940e59...`;
  no Product or workflow file changed.
- No Entity, DbContext, Migration, schema, seed, data, Production configuration,
  credential or secret changed.
- PR #69, P2-D, legacy design identities, historical failures, current migration
  lineage and all client scaffolds were inspected only and retained.
- New SQL artifacts are explicitly read-only inventory/reconciliation templates
  under MISSION-03 governance. They were not run against any database.
- W8 was not entered; no move, rename, delete, branch cleanup or structural
  refactor occurred.
- Rollback for this checkpoint is a normal revert of the governance-only v0.9
  commit. Product/database rollback is not applicable.
