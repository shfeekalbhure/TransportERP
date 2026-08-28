# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — CORRECTED DEPENDENCY PACKAGE EXISTS; FRESH INDEPENDENT DB-GOV REVIEW REQUIRED; KEEP MISSION-03 OPEN`

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

## Post-resubmission DB-GOV state

The repository contains the exact v1.0 Greenfield physical design and acceptance specification. Control Tower revalidation proved the earlier coordinated DB-GOV decision predated that exact resubmission and also contained an incompatible DBP-003B/C ↔ DBP-006 sequence.

The MISSION-03 correction now exists in:

`CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-003BC_003A_006_PHYSICAL_DEPENDENCY_CORRECTION_v1.1.md`

Correction commit:

`20608494998e671892ee35abd415158e399c9036`

The correction also resolves a second physical FK-order issue discovered during dependency binding: `auth_sessions.RegisteredDeviceId` cannot create its composite FK before `registered_devices` exists.

The only candidate-unit order submitted for the next independent review is therefore:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Physical/activation boundary:

- DBP-003B/C durable device/proof objects may physically precede session and Offline persistence.
- Device lifecycle operations that require session-family revocation remain disabled until DBP-003A exists and passes.
- Device lifecycle operations that require Offline quarantine remain disabled until DBP-006 exists and passes.
- DBP-003A creates its device FK only after `registered_devices` exists.
- DBP-006 remains after both session and device/proof persistence.

## Controlling gate

`HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — FRESH POST-CORRECTION INDEPENDENT DB-GOV REVIEW REQUIRED`

The correction is not a self-approval. No candidate Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring or candidate migration application is authorized until an independent DB-GOV decision is recorded after the correction package exists.

## Authorized next work

MISSION-03 must continue automatically with non-destructive work and must not stop the analytical chain:

1. Submit the v1.1 correction package plus v1.0 exact design and `GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md` to a fresh independent DB-GOV review.
2. The independent review must bind the exact execution parent SHA/tree, corrected candidate identities/order, composite FKs/indexes, activation boundaries and acceptance matrix.
3. Do not author Entity/DbContext/Migration/Schema/Seed/persistent adapters and do not apply candidate migrations until that review explicitly opens bounded rehearsal authority.
4. Continue unrelated non-destructive W5/W6/W7 preparation where existing gates permit.
5. Keep W8 last; no destructive/global cleanup before its preservation gate is satisfied.
6. Issue a new manifest/checkpoint and detached SHA-256 set after this corrected package is stabilized; prior `MISSION-03-GREENFIELD-DBP-RESUBMISSION-v1.1` hashes remain historical.

## Remaining non-DB / external gates

The Greenfield correction does not remove:

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

No `OWNER DECISION REQUIRED` is active. The current next gate is a fresh independent DB-GOV review of the post-correction package.
