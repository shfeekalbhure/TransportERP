# DB-GOV Post-Correction Independent Review Request — 2026-08-29

Requesting mission: `MISSION-03 — Execution & Remediation`
Requested authority: `CONTROL TOWER / DB-GOV-001 — INDEPENDENT REVIEW`
Review mode: repository/evidence review only; no Product/DB mutation

## Exact execution basis

- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- MISSION-03 execution head: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Execution tree: `00512125311306a43474638195d2cad97b76118e`
- Database baseline: `DB-BASELINE-001 = GREENFIELD / NEW / EMPTY / NO LEGACY TABLES OR DATA`
- Rehearsal target if later approved: isolated disposable PostgreSQL `18.6`

## Review inputs

The independent reviewer must read together:

1. `CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_RESUBMISSION_REVALIDATION_2026-08-29.md`
2. `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md`
3. `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-003BC_003A_006_PHYSICAL_DEPENDENCY_CORRECTION_v1.1.md`
4. `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`
5. `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DB_GOV_EXECUTION_REGISTER.md`
6. the historical `DBP-002_003_004_005_006_GREENFIELD_DB_GOV_REVIEW_DECISION.md` only as prior evidence, not as current authority.

## Corrected candidate order submitted for decision

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Reserved candidate migration identities:

1. `GreenfieldTenantMembershipIsolation`
2. `GreenfieldAuditV2AndAtomicOutbox`
3. `GreenfieldDeviceRegistryAndProof`
4. `GreenfieldLocalAuthSessions`
5. `GreenfieldTypedOfflineProtocol`
6. `GreenfieldGovernedSettlement`

## Independent questions to decide

The reviewer must independently determine whether:

- DBP-003B/C may physically depend on DBP-002/004 only while deferring session/offline-coupled lifecycle activation until later units exist;
- `registered_devices` and unique `(Id,CompanyId)` exist before DBP-003A creates the session-device composite FK;
- DBP-003A has no remaining physical dependency on objects introduced after it;
- DBP-006 begins only after membership, Audit V2, device/proof and session durable dependencies exist;
- the acceptance specification contains sufficient FK/RLS/cross-tenant/concurrency/failure/backup-restore gates for the corrected order;
- each unit has a valid stop/dependency boundary and no candidate authoring must occur before review approval.

## Requested decision vocabulary

Return exactly one controlling outcome for the coordinated bundle:

- `APPROVED FOR BOUNDED DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL AUTHORING/APPLICATION`; or
- `REVISE — CORRECTIONS REQUIRED BEFORE REHEARSAL`; or
- `HOLD — INSUFFICIENT AUTHORITY/EVIDENCE`.

If approval is granted, it must explicitly bind the exact execution parent SHA/tree, corrected order, candidate-unit identities, permitted non-Production scope, and prohibitions.

## Prohibitions during review

No Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring, candidate migration application, Production access, master merge, rebase, cherry-pick, force-push, history rewrite, credential/signing material or real data mutation is authorized by this request.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT — NOT STARTED` until a later conclusive MISSION-03 seal/handoff.
