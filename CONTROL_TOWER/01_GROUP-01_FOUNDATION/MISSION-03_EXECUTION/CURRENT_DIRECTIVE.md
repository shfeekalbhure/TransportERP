# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — PRESERVE VERIFIED W1; W2 HOLD RETAINED AFTER CONTROL TOWER REVALIDATION`

## Accepted execution basis

- MISSION-02 package: `MISSION-02-v1.2 — SEALED — DELIVERED TO CONTROL TOWER`.
- Governing product baseline: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- PR #69: `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f — UNMERGED REMEDIATION / FINAL CANDIDATE — EVIDENCE ONLY`.
- Accepted MISSION-03 W1 checkpoint: `MISSION-03-W1-CHECKPOINT-v0.2` at execution SHA `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- W1 `REM-100` remains independently verified and preserved; exact-head run `33181376288` remains accepted technical evidence.
- Latest received worker checkpoint: `MISSION-03-W2-REVALIDATION-HOLD-CHECKPOINT-v0.5` at `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`.

## Control Tower revalidation decision

Control Tower independently rechecked the v0.5 checkpoint against repository reality and the sealed MISSION-02 contract.

The following technical facts are accepted as evidence:

- the execution branch head is exactly `9c5b7a12e59d2c42e682717b8e90c491f8699b96`;
- it is five commits ahead of the accepted W1 checkpoint;
- the compare changes one evidence workflow plus API/Security/Sync source and tests and contains no Entity, DbContext, Migration, Seed, schema, data or Production configuration change;
- exact-head run `33185419917` completed successfully at `9c5b7a1...`;
- both workflow jobs succeeded and retained artifact digests were independently rechecked;
- the mission test register records `128/128 PASS`, ten existing PostgreSQL 18.6 migrations with no model drift, expected API HTTP 401 boundary, Desktop PASS and Mobile Admin/Customer/Driver probes PASS;
- failed intermediate run `33184771338` remains retained as historical evidence.

These facts establish a technically successful isolated candidate. They do not establish execution authority or retroactive adoption.

Governing decision:

`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/MISSION_03_W2_REVALIDATION_DECISION_2026-08-28.md`

Candidate disposition:

`9c5b7a12e59d2c42e682717b8e90c491f8699b96 — PRESERVED TECHNICAL CANDIDATE — NOT ADOPTED AS EXECUTION BASELINE`

## Why the W2 hold remains

The sealed MISSION-02 dependency contract has not been fully satisfied:

- `DEP-005` depends on canonical authority **and live-role evidence**. Canonical authority is resolved, but authoritative live user/role/RLS evidence remains unavailable/unknown.
- `DEP-006` depends on IdP mode/config and DEP-005. The Production issuer/session authority mode is not established by repository evidence and DEP-005 is not released.
- `DEP-007` depends on DEP-005/006, so it is not released as a W2 execution gate.
- The sealed W2 entry criteria require IdP/tenant-cardinality evidence and `DBP-002/003` review state. The current MISSION-03 DB-GOV register still records both entry gates as unsatisfied/blocked.
- The preserved candidate still relies on cardinality semantics that are not proven from authoritative live evidence: `CurrentRequestSecurityResolver` accepts a stored active user with `BranchId = null` for a claimed active branch within the same company. ADR-W2-001 itself requires explicit governed company-wide membership semantics rather than an inferred wildcard, while the live population and intended null-scope meaning remain unknown.

Therefore:

`HOLD — NO FURTHER W2 PRODUCT MODIFICATION — REVALIDATION COMPLETE / GATE NOT RELEASED`

ADRs `TENANT_CARDINALITY_ADR.md`, `IDENTITY_RBAC_SESSION_ADR.md`, and `DEVICE_LIFECYCLE_POP_ADR.md` are preserved as substantive candidate design evidence, but they do not release DEP-005/006/007 at this checkpoint.

Packages `W2-A1/A2/B1/B2A/C1/F1` remain `PRESERVED TECHNICAL CANDIDATE — NOT ADOPTED`.

Packages `W2-B2B/C2/D/E/F2` remain blocked by their recorded owner-authority, live-baseline, upstream dependency and/or DB-GOV conditions.

## Permitted next work

MISSION-03 may continue only with non-destructive prerequisite work that does not modify Product:

1. obtain authorized read-only/live-role and tenant-cardinality evidence sufficient to settle DEP-005, including null/company-wide scope semantics and mismatch counts;
2. obtain/register authoritative non-secret IdP mode/config evidence sufficient for DEP-006 or carry the exact owner-reserved authority choice to the correct gate;
3. complete DBP-002/003 impact, preservation, forward-migration and recovery evidence required by DB-GOV-001 before any database execution request;
4. revise/rebind ADRs and W2 package gates against those facts;
5. preserve all post-W1 commits, failed/successful runs and artifacts without merge, reset, delete, rewrite, force-push, cherry-pick or silent adoption.

No Product Source, Tests, Migrations, Production configuration, database/data mutation, PR merge or destructive Git action is authorized by this directive.

## Owner-decision boundary

Bounded owner-authority items such as `AUTH-001` remain carried forward. No immediate global `OWNER DECISION REQUIRED` hold is issued because the actual next permitted work is non-destructive prerequisite/evidence reconciliation rather than issuer-specific Product/Production execution.

If an owner-reserved choice becomes the actual next execution step after the other prerequisites are satisfied, stop only that affected gate and escalate it then.

## MISSION-04

MISSION-03 remains `IN PROGRESS — NOT SEALED`.

MISSION-04 remains:

`WAIT — PENDING SEALED MISSION-03 EXECUTION OUTPUTS`

MISSION-04 must remain independent from MISSION-03 execution.

## Checkpoint hash note

`EXECUTION_OUTPUT_SHA256.txt` for `MISSION-03-W2-REVALIDATION-HOLD-CHECKPOINT-v0.5` binds the worker checkpoint and the prior Control Tower directive snapshot that existed when that provisional package was generated. This directive intentionally supersedes that operational instruction after Control Tower revalidation. Do not rewrite the historical v0.5 hash register to conceal the supersession. Any later checkpoint or final MISSION-03 package must regenerate its manifest and detached hashes.
