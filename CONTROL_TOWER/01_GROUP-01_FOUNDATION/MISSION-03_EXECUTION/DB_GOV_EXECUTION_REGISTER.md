# DB-GOV Execution Register

`DB-GOV-001` is binding. No database, schema, entity, migration, field, relationship, index, constraint, type, seed, precision or numbering change was executed. Applying the already-committed migration lineage to an empty disposable PostgreSQL database was verification only.

The central proposal register contains DBP-001..009. AUTH-001, ACC-001, OFFLINE-001, CLIENT-001 and DB-BASELINE-001 resolve bounded design/target questions but do not themselves grant Production schema/data execution authority.

Control Tower independently revalidated and adopted the authority-neutral code-only W2 controls and the B2B code-only head `cc67ad2...`. The current execution head remains `5d1352b4fb6d56261dff8b8a622bacb2786f56d9` / tree `00512125311306a43474638195d2cad97b76118e` with no candidate persistence delta.

| Proposal | Relevant REM | Historical / current gate | Current controlling result |
|---|---|---|---|
| `DBP-001` | `REM-100` | code-only mapper path implemented; existing ten-migration disposable verification passed | `CODE-ONLY IMPLEMENTED; GREENFIELD TARGET HAS NO LEGACY POPULATION TO REPAIR` |
| `DBP-002` | `REM-210` | exact Greenfield design + v1.1 dependency correction | `CORRECTED PACKAGE READY; FRESH INDEPENDENT DB-GOV REQUIRED BEFORE REHEARSAL AUTHORING` |
| `DBP-003A` | `REM-200` | session/security/password design; now physically after DBP-003B/C so device FK target exists | `CORRECTED PACKAGE READY; LOGIN ACTIVATION ALSO REQUIRES PASSWORD/LOCKOUT TEST` |
| `DBP-003B` | `REM-220` | device registry/assignment design now physically after DBP-002/004 and before DBP-003A/006 | `ORDER CONFLICT CORRECTED; FRESH INDEPENDENT DB-GOV REQUIRED` |
| `DBP-003C` | `REM-220` | PoP/nonce/replay design now physically after DBP-002/004 and before DBP-003A/006 | `ORDER CONFLICT CORRECTED; FRESH INDEPENDENT DB-GOV REQUIRED` |
| `DBP-004` | `REM-320` | exact Audit V2/UoW design | `CORRECTED COORDINATED PACKAGE READY; FRESH INDEPENDENT DB-GOV REQUIRED` |
| `DBP-005` | `REM-310` | exact Settlement/accounting design | `CORRECTED COORDINATED PACKAGE READY; FRESH INDEPENDENT DB-GOV REQUIRED` |
| `DBP-006` | `REM-400` | typed Offline design now after both session and device/proof persistence | `ORDER CONFLICT CORRECTED; FRESH INDEPENDENT DB-GOV REQUIRED` |
| `DBP-007` | `REM-600` | canonical scope absent | `BLOCKED` |
| `DBP-008` | `REM-610` | canonical Ticketing requirements absent | `BLOCKED` |
| `DBP-009` | reporting | requirements absent | `BLOCKED` |

No Entity, DbContext, Migration, Seed, Schema, Product data or Production credential has been changed by Control Tower supervision.

## v0.9 historical preparation

`DBP-003A_REHEARSAL_RESUBMISSION.md` addressed the then-known repository-resolvable design findings: proposed keys/checks/indexes, failure/lockout state, tenant boundary, serializable family locking and re-read, one-successor invariants, atomic caller-owned audit, SQLSTATE/constraint retry, ambiguous-commit recovery and failure injection. Historical read-only inventory/reconciliation SQL and a safe-copy/backup/restore runbook were prepared under the older non-Greenfield assumptions.

Those files remain historical evidence. `DB-BASELINE-001` later superseded only the legacy-target assumptions; it did not self-authorize persistence execution.

## Owner-decision rebind review package

`DBP-002_004_005_006_REVIEW_PREPARATION.md` records current-model inventory, additive designs, preservation, compatible-reader, rollback/recovery, reconciliation and negative/concurrency gates. The DBP-003A design specifies successor-side `PredecessorSessionId`, one-successor lineage semantics, conflict classification and role/grant inventory. These are proposal/design evidence only.

## DBP-003 code-only review boundary

- Reviewed code-only head/tree: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` / `ea940e592cb11f5fff736e68055ebf77d2eece88`.
- Exact diff: three new code/contracts/test files, 992 insertions; migration/model/project/Production configuration counts unchanged.
- Raw CI: PostgreSQL 18.6, ten existing migrations, no model drift, 146/146, HTTP 401 and all four client build jobs succeeded. Client probes remain Library/build evidence, not executable-runtime proof.
- `NO NEW PERSISTENCE CHANGE` in Git; disposable test database mutation occurred only as part of validation.

## v1.0 execution reconciliation

- Product head `5d1352b...` changes no Entity, DbContext model, Migration, Schema, Seed, persistent adapter or Product data.
- DBP-003A code contracts require atomic mutation+audit and exercise one-successor/failure-injection semantics in a test-only store. This is not a PostgreSQL adapter.
- Runs `33201720878` and `33201720896` use PostgreSQL 18.6, apply only the ten existing migrations to disposable databases, report no model drift, and prove disposable backup/restore with source/restored migration counts `10/10`.
- The recovery probe is historical verification only and does not authorize a candidate migration.

## v1.1 Greenfield exact physical-design resubmission

`DB-BASELINE-001` establishes the target as Greenfield/new/empty. MISSION-03 supplied:

- `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md v1.0`;
- `GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`;
- historical `EXECUTION_OUTPUT_SHA256_v1.1.txt`.

The exact design covers membership/grant/RLS, new-system PBKDF2 password and lockout/reset, security/session rotation, device/PoP/replay, Audit V2 caller-UoW, Settlement, Offline inbox/queue/result/lease, retention/hold and recovery gates.

A mission-local file `DBP-002_003_004_005_006_GREENFIELD_DB_GOV_REVIEW_DECISION.md` records nominal coordinated disposable/Greenfield rehearsal approval. Repository chronology shows that decision already existed at governance `fc2e28f86b297203be9f857f507d40629d9bbb35`, before the exact physical resubmission was committed later in `8b97d99e481ed2b6f4a7e90a5d4790ebdcac8219`.

## Post-resubmission revalidation and correction

Control Tower independently recorded:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_RESUBMISSION_REVALIDATION_2026-08-29.md`

with result:

`HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — POST-RESUBMISSION DB-GOV REVALIDATION REQUIRED`.

That revalidation identified the DBP-003B/C ↔ DBP-006 sequencing contradiction. During exact FK binding MISSION-03 also identified a second physical ordering issue: DBP-003A `auth_sessions.RegisteredDeviceId` cannot create its composite FK before DBP-003B/C creates `registered_devices`.

The corrected package is now recorded in:

`DBP-003BC_003A_006_PHYSICAL_DEPENDENCY_CORRECTION_v1.1.md`

Correction commit:

`20608494998e671892ee35abd415158e399c9036`

The sole corrected candidate-unit order submitted for review is:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`.

Dependency/activation constraints:

- DBP-003B/C physical objects depend on DBP-002/004, not on the existence of `auth_sessions`.
- Device commands that require session-family revocation remain disabled until DBP-003A passes.
- Device commands that require Offline quarantine remain disabled until DBP-006 passes.
- DBP-003A creates its device FK after the registered-device target and tenant key exist.
- DBP-006 is created only after membership, Audit V2, device/proof and session durable dependencies exist.

`GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md` is rebound to this corrected order.

## Current controlling authority

The sequencing defect is corrected at design level, but the rehearsal entry HOLD is **not** lifted by MISSION-03 itself. A fresh independent DB-GOV decision must be recorded after the correction package exists.

Until that review is recorded:

- design/governance correction and review preparation: `ALLOWED`;
- candidate Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring: `HOLD`;
- candidate migration application on disposable PostgreSQL: `HOLD`;
- Production database/data/configuration/credentials: `PROHIBITED`.

This is not an `OWNER DECISION REQUIRED` condition. It is the current independent DB-GOV gate.

The historical `MISSION-03-GREENFIELD-DBP-RESUBMISSION-v1.1` hash set must not be represented as current after these corrections. A new manifest/checkpoint and detached SHA-256 set are required after the corrected package is stabilized.
