# DBP-003 — Independent DB-GOV Review Decision

- Review authority: `CONTROL TOWER / DB-GOV-001`
- Review time UTC: `2026-08-28T17:17:41Z`
- Authoritative product: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Previous accepted execution baseline: `9c5b7a12e59d2c42e682717b8e90c491f8699b96`
- Reviewed execution head: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- Reviewed execution tree: `ea940e592cb11f5fff736e68055ebf77d2eece88`
- Raw CI: [GitHub Actions run 33191269475](https://github.com/shfeekalbhure/TransportERP/actions/runs/33191269475)
- PR #69: [open Draft PR — unmerged candidate evidence](https://github.com/shfeekalbhure/TransportERP/pull/69)
- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`

## Controlling decision

`DBP-003 = HOLD AT REHEARSAL ENTRY`

| Package | DB-GOV decision | Execution authority |
|---|---|---|
| `DBP-003A — session/security-version persistence` | `REVISE BEFORE REHEARSAL` | no Entity/DbContext/Migration/schema/persistent-adapter authoring yet |
| `DBP-003B — device registry/assignment` | `DEFERRED — DEPENDS ON DBP-002/006` | none |
| `DBP-003C — PoP/nonce/replay` | `DEFERRED — DEPENDS ON DBP-002/006` | none |

No DBP-003 package is opened by this review. This is not a general stop: the independently verified code-only head is adopted as the next bounded MISSION-03 baseline, and proposal/evidence revision may continue without database mutation.

## Independent B2B code-only finding

The exact diff `9c5b7a1...cc67ad2...` contains three additions and no modification:

| Path | Classification |
|---|---|
| `TransportERP.Api/Identity/LocalSessionLifecycle.cs` | storage-neutral session lifecycle and interfaces |
| `TransportERP.Contracts/Identity/LocalSessionContracts.cs` | request/response contracts |
| `TransportERP.Tests/LocalSessionLifecycleTests.cs` | 18 test-only lifecycle cases and an in-memory locked store |

- Diff size: `3 files / 992 insertions`.
- Tracked tree: `382 → 385` files.
- Projects/solution: unchanged at `10 .csproj + 1 .sln`.
- Test source files: `23 → 24`; `[Fact]` methods: `105 → 123`; `[Theory]`: unchanged at `2`.
- Migration lineage: unchanged at `10` implementation migrations, `9` designer files and `1` model snapshot (`20` migration-directory files total).
- No Entity, DbContext model, ModelSnapshot, Migration, schema, seed, Production configuration, committed data repair or workflow change exists in the diff.

Therefore:

`NO NEW PERSISTENCE CHANGE`

Run 33191269475 separately applied the already-committed ten migrations and executed integration tests against its disposable PostgreSQL service:

`DISPOSABLE TEST DATABASE MUTATION OCCURRED AS PART OF VALIDATION`

That disposable mutation is not a Product diff and is not a Production change.

## Raw CI verification

Control Tower read the decoded raw logs for jobs `98917044706` and `98917044568` and inspected both retained artifacts.

| Claim | Independent result |
|---|---|
| exact SHA | `PASS — cc67ad2bd491ed3ab23c3144f11dff955353c3a4` |
| exact tree | `PASS — ea940e592cb11f5fff736e68055ebf77d2eece88` |
| parent | `PASS — 9c5b7a12e59d2c42e682717b8e90c491f8699b96` |
| PostgreSQL | `PASS — 18.6 (Debian 18.6-1.pgdg12+2)` |
| migrations | `PASS — all 10 current migrations applied to the empty disposable DB` |
| EF drift | `PASS — No changes have been made to the model since the last migration` |
| tests | `PASS — 146/146; 0 failed; 0 skipped` |
| API boundary | `PASS — expected HTTP 401` |
| Desktop build/probe | build `PASS`; probe truth is `net10.0-windows / Library / HasDesktopEntryPoint=false` |
| Mobile Admin build/probe | build `PASS`; probe truth is `net10.0 / Library / MauiRuntimeReady=false` |
| Mobile Customer build/probe | build `PASS`; probe truth is `net10.0 / Library / MauiRuntimeReady=false` |
| Mobile Driver build/probe | build `PASS`; probe truth is `net10.0 / Library / MauiRuntimeReady=false` |

The client jobs validate the current scaffold/contract build surfaces; they do not prove executable Desktop or Mobile session behavior.

Warnings remain evidence, not hidden failures:

- four `xUnit2031` warnings, including two in `LocalSessionLifecycleTests.cs` lines 194–195;
- Desktop `CS8602` at `Waybills/ShippingExecutionForms.cs:656`;
- GitHub Actions Node 20 deprecation notices.

The raw PostgreSQL `ERROR` lines are accounted for:

- missing `__EFMigrationsHistory` occurred during inspection of the intentionally empty disposable database before initial apply;
- unique-constraint and serialization errors correspond to negative/idempotency/concurrency paths whose enclosing suite completed `146/146`;
- item-release, trip-allocation, movement-event and audit append-only errors correspond to explicit raw-mutation denial tests.

They are expected-test evidence for this run, not unhandled job failures. They do not constitute PostgreSQL evidence for the proposed session store, because DBP-003 had no durable adapter or schema in the run.

## Existing migration lineage captured

The ordered implementation lineage and snapshot at `cc67ad2...` are fixed below. Git blob and SHA-256 both bind the content.

| Migration / snapshot | Git blob | SHA-256 |
|---|---|---|
| `20260819032151_P1InitialPostgreSql.cs` | `9030eae65df98784636c24438b7c5902bf4d2b4c` | `ca35536b8ac503f41dfd5e163580ee69609c88cf4bd0cdeb30c0c87701fbc05e` |
| `20260819151242_P1AuditAppendOnlyAndOutcome.cs` | `50c3ec7a97fabb3e62a31eb794557685b6358dad` | `5e6591db6686cd02b6e12382c827dc485041ac6d8bfd42b03acb07c108a03e69` |
| `20260819152128_P1ConflictCaseAndSyncRelation.cs` | `820e6ad455dd206a1470fd4012b033127d65a768` | `90ead061d5537d3df34dec78cc3e96bf2d9a33c0655e990509f51ac947084099` |
| `20260820205431_P2C01AWaybillFoundation.cs` | `dd947a0e7ccc13e9aa965df7ee9f279292f0e9bf` | `0a39ef48032d3d938db1cfba667be5f5939f0160169869d00892989643985c4b` |
| `20260820205853_P2C01AWaybillFoundationHardening.cs` | `49cdd7bde74b0e6cdef3d4579ccff0d6d365a93f` | `678ebda4878f0783f2475d72fffab001307e9bd6c9f74d8fb42e8d9437b3050f` |
| `20260821004516_P2C01BFinance.cs` | `ed6dc3c6b2dd2c45e396923c16019393dd4334af` | `c4a471087354503c3569ceca4c968de8c5887388c11feb75fb36f48137fe4cd8` |
| `20260821132015_P2C01CShippingExecution.cs` | `5227830b64314c7791bced2806967918236285d0` | `791f333b98aabfd951c5eb4d5d074933ce43570a622d8594d6f5141271dd57df` |
| `20260821141529_P2C01CShippingExecutionHardening.cs` | `8e46046049f744f69e06ed07ac8d850b9072c2cf` | `53d72ffd87d2bd13e16702c9d2df5cfaf84a321ecbb6bd4d4f4e13b18d8ed464` |
| `20260821170000_P2C01CTeam03PostgreSqlHardening.cs` | `a9324a19b9de3cffab94e99901982b361b452abf` | `a2f570f38a14c19d777417f66dc81b2989779137696788fad3bee92397ddb51f` |
| `20260821191039_P2C01CWaybillVolumeContract.cs` | `6b5e1de3dac73ae24593b7d2fdcfe3a296d8415a` | `462608390c489bd8c9133fc0382714ce5167e1276622356d1bfc6d3a422b1282` |
| `TransportErpDbContextModelSnapshot.cs` | `e017fc0d9357de76d73d6dc735dfc1adccacfdde` | `d6cec8f72aeddf4b39e8030900412efb27065aee33c603084ab04ae35182b677` |

## Object decisions

| Proposed object | Decision | Evidence and required correction |
|---|---|---|
| `user_security_state` | `REVISE` | persistent invalidation version is required, but the proposed shape omits the failure/lockout fields claimed by its login semantics and does not select a PostgreSQL-safe concurrency mechanism; specify columns, checks, initialization, increment semantics and concurrency token behavior |
| `auth_sessions` | `REVISE` | durable rotating families are required, but the proposal does not provide executable PostgreSQL transaction/locking/audit design, a database-enforced single-successor invariant, complete tenant-consistent FKs, or rollback/retry behavior |
| `registered_devices` | `DEFER` | required for the eventual C2 trust target, but not required for session-table rehearsal; device identity, enrollment authority, public-key metadata and DBP-006 boundary remain unsettled |
| `registered_device_assignments` | `DEFER` | depends on explicit membership/cardinality and tenant-consistent keys in DBP-002 plus device lifecycle rules in DBP-006 |
| `device_proof_nonces` | `DEFER` | replay store choice, nonce/JTI uniqueness scope, retention, legal hold and cleanup behavior depend on DBP-006; a PostgreSQL table is not yet proved to be the correct persistence object |

## Keys, relationships and PostgreSQL transaction review

The current source proves `Branch` has alternate key `(Id, CompanyId)`, but `User` has only PK `Id`; its nullable `CompanyId` and `BranchId` are independent FKs. A current user row can therefore point at a branch from a different company. DBP-003 must not claim tenant consistency from application intent.

Before DBP-003A can enter rehearsal, its revised physical design must specify and test:

1. `user_security_state(UserId)` as PK/FK, `SecurityVersion >= 1`, failure count/lockout shape, initialization race behavior and a PostgreSQL-effective concurrency token.
2. `auth_sessions` PK, immutable `FamilyId`, predecessor/successor relation, globally unique fixed-length refresh digest, expiry/status checks, and indexes supporting active session, family revoke, user/security-version and expiry cleanup queries.
3. company FK plus composite `(BranchId, CompanyId) -> branches(Id, CompanyId)` when BranchId is present.
4. an explicit statement that session-to-user tenant consistency is either deferred to DBP-002 composite keys or implemented through a separately reviewed invariant; no silent alternate-key/cardinality mutation.
5. a PostgreSQL transaction that locks and re-reads the presented generation/family, permits one successful rotation maximum, prevents two active successors, handles serialization/unique conflicts deterministically, revokes the complete family on detected reuse, and commits the audit event atomically with the decision.
6. failure injection proving that rollback cannot resurrect a consumed token or commit a successor without its predecessor/audit state.

The current `ILocalSessionStore` tests do not satisfy items 5–6. They use a process-local `lock`; `FindByRefreshTokenHashAsync` precedes `RotateAsync`; some `RevokeFamilyAsync` calls are separate store operations; and the store contract carries no audit record/unit-of-work boundary.

## DBP-002 / DBP-003 staging

| Scope | Dependency result |
|---|---|
| security-version concept and one-to-one User FK | logically independent of DBP-002, but current object shape still requires revision before authoring |
| base session generation/family concept | can preserve current singular User cardinality, but PostgreSQL concurrency/audit design must be revised first |
| Branch/Company consistency | composite Branch key already exists and can be used without changing User cardinality |
| Session/User/Company/Branch database consistency | depends on DBP-002 or a separately reviewed invariant because User lacks the required composite alternate key |
| device assignment to membership | requires DBP-002 before physical implementation |
| device registry/proof/nonce/replay | requires DBP-006 evidence; assignment also requires DBP-002 |

## Password-hash gate

The exact source contains only `User.PasswordHash` as required `varchar(500)` and test fixtures containing literal `test-only`. No password verifier, algorithm/version marker, salt format, legacy-format inventory, rehash policy, failed-login counter or lockout implementation exists in the reviewed tree.

`PASSWORD-HASH BASELINE = UNKNOWN — BLOCKS LOGIN PERSISTENCE ACTIVATION`

Required non-destructive resolution: inventory authorized sanitized/current hash format samples or authoritative identity documentation; bind algorithm, encoded format/version, per-password salt behavior, any pepper custody, legacy verification, opportunistic rehash, invalid-format response, failure counters, lockout duration/reset and concurrency tests. No algorithm may be inferred from the column name or length.

## Secrets and token custody

- The code generates 32 random bytes for refresh tokens and stores only their SHA-256 digest in `LocalSessionRecord`; the raw value crosses the response boundary only.
- It does not persist passwords, signing private secrets or device private keys.
- The local JWT signing value is injected through options; the diff adds no committed Production value or configuration.
- Production signing/encryption/pepper custody, rotation, overlap, revocation and recovery remain unproved. This blocks Production activation, not proposal revision or existing disposable baseline verification.

## Disposable / safe-copy gate

The proposal names the necessary stages, and this review captured the exact current migration lineage. Entry is still denied because no operationally bound rehearsal package exists for DBP-003A:

| Required gate evidence | Current state |
|---|---|
| exact lineage | `SATISFIED — captured above` |
| pre-change schema snapshot procedure/output | `MISSING` |
| sanitized data-shape inventory, including null/mismatched scope cases | `MISSING` |
| recoverable backup/snapshot identity and digest | `MISSING` |
| restore test before candidate migration | `MISSING` |
| pre/post row counts and reconciliation queries | `MISSING` |
| FK/index/check reconciliation queries | `MISSING` |
| candidate EF model drift check | `MISSING — no candidate authorized` |
| application boot and existing regression suite | `PLANNED; baseline proof exists, candidate proof absent` |
| durable PostgreSQL session/concurrency/audit tests | `MISSING` |
| rollback/forward-recovery failure-injection evidence | `MISSING` |

## Exhaustively verified blockers and next actions

| ID | Evidence | Why it blocks | Can current repository/CI settle it now? | Required non-destructive action |
|---|---|---|---|---|
| `DBP003-BLK-001` | proposal text plus code/test store boundary | no PostgreSQL-proof one-rotation/family-revoke/atomic-audit guarantee | partially; the deficiency is source-visible, proof requires a revised design and later authorized adapter | revise transaction, constraints, audit UoW, retries and failure-injection test specification; resubmit DB-GOV review |
| `DBP003-BLK-002` | only `PasswordHash varchar(500)` and `test-only` fixtures found | login activation could reject or weaken unknown current hashes and lockout behavior | no | produce authorized sanitized format inventory and approved verify/rehash/lockout policy |
| `DBP003-BLK-003` | no snapshot, data-shape, restore or reconciliation artifact | safe-copy migration cannot be shown recoverable or preservation-safe | no; empty CI is insufficient | prepare named non-Production safe copy, schema/data inventory, backup digest, pre-apply restore proof and reconciliation scripts |
| `DBP003-BLK-004` | User Company/Branch FKs are independent; no User composite alternate key | full session/assignment tenant consistency would silently depend on DBP-002 | yes for design classification, not execution | split DBP-003A/B dependency and obtain DBP-002 physical-key decision before dependent FKs |
| `DBP003-BLK-005` | DBP-006 retention/MDM/attestation evidence absent | device assignment and nonce/replay physical model may be wrong or retain data indefinitely | no | keep 003B/C deferred; settle lifecycle, storage, uniqueness and retention under DBP-006 |
| `DBP003-BLK-006` | no Production key-custody/rotation/recovery evidence | Production token issuance cannot be operated safely | no; external operational evidence required | provide approved secret-store ownership, rotation/overlap/revocation and recovery drill; does not block disposable design work |

None of these remaining actions is destructive or requires a Production mutation. No `OWNER DECISION REQUIRED` is raised by this review.

## MISSION-03 direction

- New bounded code-only baseline: `codex/mission-03-execution-20260828@cc67ad2bd491ed3ab23c3144f11dff955353c3a4`, tree `ea940e592cb11f5fff736e68055ebf77d2eece88`.
- `DBP-003A`: revise proposal/evidence only; no Entity/DbContext/Migration/schema/persistent adapter.
- `DBP-003B/C`: deferred; no authoring.
- Permitted database environment for a new DBP-003 migration: `NONE AT THIS CHECKPOINT`.
- Existing ten-migration disposable baseline verification may be repeated; it grants no DBP-003 authority.
- After blocker closure, any later authority can be at most `APPROVED FOR DISPOSABLE/SAFE-COPY REHEARSAL ONLY`; Production DB/credentials/data repair/destructive migration/drop/rename/history rewrite/master merge remain prohibited.
- Required exit evidence is exact-head clean+upgrade PostgreSQL apply, pre/post preservation reconciliation, restore proof, EF drift, application boot, full regression, password-hash matrix, durable one-success refresh race, reuse-family revoke, atomic audit, rollback/failure injection, cross-tenant direct-SQL denials and client credential-clearing behavior.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT`.
