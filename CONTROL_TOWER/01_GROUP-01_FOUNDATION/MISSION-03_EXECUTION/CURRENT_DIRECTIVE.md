# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DB REHEARSAL ENTRY HOLD; RESOLVE DBP-003B/C ↔ DBP-006 ORDER CONFLICT; KEEP MISSION-03 OPEN`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch/head: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`.
- Execution tree: `00512125311306a43474638195d2cad97b76118e`.
- PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f — OPEN / DRAFT / UNMERGED — EVIDENCE ONLY`.
- Exact internal baseline evidence remains: run `33201720896 = 153/153 PASS`; PostgreSQL 18.6; ten existing migrations; no model drift; API HTTP 401; client build probes only. Recovery run `33201720878 = PASS` on disposable PostgreSQL.

## Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — TARGET DATABASE IS GREENFIELD / NEW / EMPTY / NO LEGACY TABLES OR DATA`.

## Post-resubmission DB-GOV revalidation

The exact v1.0 Greenfield physical design and acceptance specification now exist in repository evidence. A mission-local DB-GOV decision file nominally approves coordinated disposable/Greenfield non-Production rehearsal, but repository chronology proves that decision already existed at governance `fc2e28f86b297203be9f857f507d40629d9bbb35`, before the exact v1.0 resubmission was committed in `8b97d99e481ed2b6f4a7e90a5d4790ebdcac8219`.

Control Tower therefore independently revalidated the current repository package and recorded the controlling result in:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_RESUBMISSION_REVALIDATION_2026-08-29.md`

Result:

`HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — POST-RESUBMISSION DB-GOV REVALIDATION REQUIRED`

### Evidence-bound dependency conflict

The exact physical design orders:

`DBP-002 → DBP-004 → DBP-003A → DBP-003B/C → DBP-006 → DBP-005`

and makes DBP-006 depend on DBP-003B/C device/proof persistence.

The earlier review decision orders:

`DBP-002 → DBP-004 → DBP-003A → DBP-006 → DBP-003B/C → DBP-005`

and conditions DBP-003B/C on a passed DBP-006 candidate baseline.

Both orders cannot govern the same exact physical package. No candidate Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring or candidate migration application is authorized while this contradiction remains unresolved.

## Authorized next work

MISSION-03 must continue automatically with non-destructive work and must **not** stop the analytical chain:

1. Resolve the DBP-003B/C ↔ DBP-006 physical dependency by either:
   - retaining DBP-003B/C before DBP-006 and correcting the coordinated decision; or
   - splitting DBP-006 into a physically independent pre-device core plus a later device/proof-bound extension.
2. Bind the corrected order to the exact current design revision, execution parent SHA/tree, proposed candidate-unit identities, FK/index dependencies, and the Greenfield acceptance matrix.
3. Submit the corrected package for a fresh independent DB-GOV review **after** the corrected repository package exists.
4. Do not author Entity/DbContext/Migration/Schema/Seed/persistent adapters and do not apply candidate migrations until that post-resubmission review opens bounded rehearsal authority.
5. Continue unrelated non-destructive W5/W6/W7 preparation where existing gates permit.
6. Keep W8 last; no destructive/global cleanup before its preservation gate is satisfied.
7. The next worker checkpoint must issue a new manifest and detached SHA-256 set; `MISSION-03-GREENFIELD-DBP-RESUBMISSION-v1.1` is now historical because Control Tower governance changed after that checkpoint.

## Remaining non-DB / external gates

The Greenfield decision does not remove:

- canonical programming authority for post-DEPART Shipping, Ticketing and governed screen routes;
- real Windows/Android executable runtime and secure-store proof;
- protected Production signing custody;
- Production recovery/RPO/RTO, privacy/retention, KMS/key custody and dependency/license/provenance approvals;
- complete Git worktree/stash/local-only preservation inventory before any W8 destructive/global cleanup.

## Mission transition boundary

MISSION-03 is not sealed. No final exact-head acceptance package/seal/handoff exists. MISSION-04 remains:

`WAIT — NOT STARTED`

Do not change MISSION-04 to START until MISSION-03 is conclusively sealed and handed off with exact SHAs, evidence, manifest, detached SHA-256, seal, preservation/rollback and DB-GOV compliance verified.

## Prohibitions

No merge to master, rebase, cherry-pick, force-push, history rewrite, Production mutation, signing-secret commit, Entity/DbContext/Migration/Schema/Seed/Data change or unauthorized database action.

No `OWNER DECISION REQUIRED` is active. The current next action is delegated non-destructive DB-GOV correction/revalidation.
