# TransportERP — Master Remediation Plan

## 1. Document control

- Mission: `MISSION-02 — REMEDIATION PLANNING ONLY`
- State: `FINAL PLANNING PACKAGE — NO IMPLEMENTATION AUTHORITY`
- Authoritative product: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Product tree: `516247dd320cfc0ef71607cd3d8e7946fe9375ab`
- Planning branch baseline: `governance/control-tower-20260828@f2fc5a73bd4ffa30836b51b8187df0322eaceddf`
- PR #69 comparison: `601f2d1cad61d62e590a6714ad84e307eb84fe5f` / `OPEN / DRAFT / UNMERGED`
- Database control: `DB-GOV-001 — BINDING`

This plan changes no Source, Tests, Migrations, Database, Production configuration, or product ref. A MISSION-03 execution team must recheck all refs and entry gates before acting.

## 2. Governing inputs and direct revalidation

MISSION-01 MASTER/GATE v2.0 was independently hash-checked (`14/14 OK`). TEAM-D v1.1's `64` rows are the trace population. TEAM-E v1.1's `39/39` P0/P1 review and full `8/8` P2/P3 census are advisory inputs. Current facts were rechecked against the exact master object, not accepted merely because a report stated them.

Verified current inventory: `378` tracked files, `10` projects, `1` solution, `10` migration implementations, `1` snapshot, `22` C# test files, `1` test project, and `7` workflows. `dotnet` is unavailable in the planning runtime, so no new runtime PASS is claimed.

## 3. Remediation inventory

| Remediation ID | Governing findings / blockers | Priority | Objective | Primary scope | PR69 disposition | Wave |
|---|---|---:|---|---|---|---:|
| `REM-000` | `A-PRES-001`, `TB-F-016`, `M02-BLK-006` | P0 | Freeze every current, sealed, unmerged and local-only asset before change | Git/workspaces/evidence | `PRESERVE AS CANDIDATE`; no merge | 0 |
| `REM-001` | `A-QA-001`, `A-CI-001`, `TB-F-011`, `M02-BLK-001` | P1 gate | Produce exact-SHA build/test/migrate/boot baseline | CI/test/runtime | verify patterns only | 0 |
| `REM-100` | `A-ARCH-002`, `M02-BLK-002` | P0 | Stop `Volume` loss and determine affected data safely | Waybill mapper/DB/data | `REIMPLEMENT`; candidate retains defect | 1 |
| `REM-200` | `A-SEC-001`, `TB-F-002` | P1 | One authoritative session/RBAC/revocation pipeline | API/Identity/RBAC | selective adopt after verification | 2 |
| `REM-210` | `A-SEC-002`, `A-DB-003`, `A-DB-004`, `TB-F-003`, `TB-F-012` | P1 | Server-derived tenant scope plus DB defense | API/EF/PostgreSQL | selective adopt; DB-GOV required | 2 |
| `REM-220` | `A-OFF-002`, `TB-F-004`, `D-SEC-SYNC-001`, `M02-BLK-003`, `M02-BLK-010` | P1 | Bind every Sync lifecycle mutation to user/device or audited override | Sync/API/device | verify/rework; candidate lifecycle gap remains | 2 |
| `REM-300` | `E-BLK-013`, `C1-PROB-001/002/003` | P1 gate | Approve transaction/UoW and module-ownership ADR | architecture/DB | reimplement from approved ADR | 3 |
| `REM-310` | `A-ACCDB-007`, `A-BIZ-005`, `TB-F-005`, `M02-BLK-007` | P1 | Make `POSTED` imply balanced immutable ledger and audit | Accounting/Waybill finance | not closed; reimplement | 3 |
| `REM-320` | `A-AUD-006`, `A-DB-005`, `TB-F-013` | P1/P2 | Version audit hash and enforce append-only/atomic writes | Audit/DB | selective verification; not closed | 3 |
| `REM-400` | `A-OFF-001`, `A-OFF-002`, `TB-F-004`, `A-KUR-002`, `M02-BLK-009`, `M02-BLK-010` | P1 | Typed authorized offline protocol with replay/conflict guarantees | Offline/Sync | candidate evidence only; operation-level adoption | 4 |
| `REM-500` | `A-RUNTIME-001/002`, `TB-F-001`, `A-ARCH-005`, `TB-F-015` | P1/P2 | Executable Desktop and separately scoped Mobile clients | Desktop/Mobile/API | verify Desktop/Driver; Admin/Customer incomplete | 5 |
| `REM-600` | `A-BIZ-001`, `TB-F-007` | P1 | Complete custody lifecycle after departure in governed increments | Shipping/accounting/offline | partial/verify only | 6 |
| `REM-610` | `A-BIZ-002`, `TB-F-006` | P1 | Implement canonical Ticketing as a separate bounded module | Ticketing | not addressed; reimplement after authority | 6 |
| `REM-620` | `A-SCR-001`, `TB-F-010`, `M02-BLK-005` | P1 gate | Bind canonical Kurrasa/screens to code/data/tests | requirements/UI | not addressed | 6 |
| `REM-700` | `A-QA-002`, `A-QA-005`, `A-CI-001`, `TB-F-011` | P1/P2 | Full SHA-bound acceptance and retained evidence matrix | QA/CI | adopt patterns, rerun on chosen SHA | 7 |
| `REM-710` | `A-SUPPLY-001`, `TB-F-014`, `C1-PROB-011` | P1 | Reproducible dependency graph, SBOM/SCA/license/provenance | supply chain | not sufficiently addressed | 7 |
| `REM-720` | `A-RELEASE-001`, `TB-F-009`, `M02-BLK-004` | P1 | Artifact→install→upgrade→rollback→restore chain | release/operations | candidate CI is insufficient | 7 |
| `REM-730` | `A-PRIV-008`, `TB-F-008` | P1 | PII classification, minimization, crypto, retention and export controls | privacy/all clients | selective evidence; Production unknown | 7 |
| `REM-800` | `A-ARCH-006/012`, `C1-PROB-004/006..012`, `TB-F-017/021` | P2/P3 | Reduce structural debt after parity gates | architecture/UI/tests | selective; no big-bang adoption | 8 |
| `REM-900` | `TB-F-018/019/020`, `C1-CORR-001`, `BLK-B-001` | governance | Preserve provenance, supersession and fail-closed EF tooling | governance/tooling | no product adoption | 0/7 |

### 3.1 Work-package contracts

The following matrix completes the execution contract. `DBP-*`, `T-*`, `PRES-*`, `DEP-*`, and Wave rows contain the normative detailed procedures.

| REM | Expected files/components | Impact profile | Dependencies / preconditions / blocker | Completion and closure evidence |
|---|---|---|---|---|
| `REM-000` | refs, worktrees, stashes, bundles, all sealed packages | no runtime/DB; preservation-critical | none; external inventory may stay unknown | PRES-001..015 manifest, hashes, owners, recovery test |
| `REM-001` | solution/projects/workflows/test/migration/API/client targets | no intended functional change | REM-000; disposable .NET/PostgreSQL/Windows/Android environments | T-000 logs and immutable exact-SHA artifacts |
| `REM-100` | Waybill mapper/domain/contract/entity, integration tests; conditional data script | DB/data + API/Shipping; no intended client contract change | REM-001, DBP-001; blocked by live-row ambiguity for repair only | T-100 and impact/repair evidence; Volume survives exactly |
| `REM-200` | API Program/Identity/Security/RBAC/session services and tests | API/Security/clients; DBP-003 likely; Offline trust dependency | DEP-005, IdP/auth-mode decision | T-200, revocation/session evidence, permission parity |
| `REM-210` | security context, EF mappings, tenant-bearing entities/queries/roles | DB/API/Security/Accounting/Offline/Reporting | tenant cardinality ADR, DBP-002, safe baseline | T-210 and migration/restore evidence; zero cross-tenant access |
| `REM-220` | Sync lifecycle methods/routes/callers/device registry/override audit | Security/Device/API/Offline; DBP-003/006 possible | REM-200/210, caller inventory, override policy | T-220; every non-owner denied or explicitly audited override |
| `REM-300` | ADR for module ownership, single DbContext UoW, outbox/audit | Architecture/DB/Accounting/Offline; no code before approval | REM-210, canonical ownership; M02-BLK-008 | approved ADR with failure semantics and no circular ownership |
| `REM-310` | VoucherLifecycle, journal/source links, WaybillFinance, reporting | DB/API/Accounting; client-visible status; Offline action dependency | REM-300, accounting authority, DBP-005 | T-300; POSTED iff balanced immutable journal/audit/outbox |
| `REM-320` | AuditEvent service/model/hash, append-only DB controls | DB/Audit/Privacy/all writers | REM-300, DBP-004, legacy event sample | T-320; legacy/new chains verify; raw-SQL mutation denied |
| `REM-400` | Sync catalog/API/worker, Offline store/transport, client producers | DB/API/Desktop/Mobile/Offline/Security/Accounting | REM-220, REM-300/310 for affected actions, per-action authority, DBP-006 | T-400; exactly-once/idempotent authorized effects only |
| `REM-500` | Desktop host/client/session/navigation; scoped Mobile apps, signing/local stores | Desktop/Mobile/API/Security/Offline; local DB possible | REM-200/220/400, screen registry, delivery scope | T-500 signed executable/E2E/RTL/secure-storage evidence |
| `REM-600` | Shipping API/application/store/contracts/entities/screens | DB/API/Desktop/Mobile/Offline/Accounting/Security | REM-210/300/310, canonical lifecycle, DBP-007 | T-600 numbered custody/quantity/accounting acceptance |
| `REM-610` | new Ticketing bounded module/contracts/entities/screens/tests | DB/API/Desktop/Mobile/Accounting/Security; Offline only if authorized | canonical TRV authority, REM-210/300/310, DBP-008 | T-610 seat/booking/payment/refund trace and E2E |
| `REM-620` | authority/supersession and screen/route/permission/test registries | governance/UI; no DB by itself | latest Kurrasa and owner authority | immutable crosswalk with zero unresolved ID collisions |
| `REM-700` | test project topology, workflows, coverage/artifact retention | CI/QA/all platforms; no intended functional change | stable targets and environments | T-700 all required exact-head jobs/artifacts PASS |
| `REM-710` | global SDK/package/source/lock policy, SBOM/SCA/license/provenance | build/supply/release; dependency graph may affect all projects | capture current graph first; policy approvals | T-710 deterministic approved graph and signed provenance |
| `REM-720` | packaging/signing/install/deploy/upgrade/rollback/restore runbooks | release/Production/DB/clients | REM-700/710/730, stable candidate, operations authority | T-720 drill within approved RPO/RTO; artifact identity verified |
| `REM-730` | classification, logging/redaction, crypto/keying, retention/export/local cache | Privacy/Security/DB/API/clients/backups | legal/policy and Production evidence | T-730 approved results and clean secret/PII scan |
| `REM-800` | solution folders, stores, DTO placement, package ownership, tests/UI splits | all layers; DB physical split prohibited by default | all behavior/data gates stable; consumer inventory | T-800 exact parity and approved removal dispositions |
| `REM-900` | governance provenance, supersession, EF factory/tooling evidence | governance/DB tooling only | synthetic non-Production config; independent reviewers | lineage retained; EF fails closed and controlled tooling result recorded |

## 4. Technical ordering

The execution sequence is governed by dependency, not convenience:

1. Freeze evidence/assets and establish an executable exact-SHA baseline.
2. Close `Volume` P0 code loss; assess data separately through DB-GOV.
3. Establish identity, tenant and device trust before exposing Offline or clients.
4. Approve the transaction/UoW ADR, then implement accounting/audit invariants.
5. Reconcile offline-write authority, then adopt only approved PR69 actions.
6. Compose executable clients against stable security/accounting/offline contracts.
7. Complete Shipping and Ticketing in canonical, separately accepted increments.
8. Close acceptance, supply-chain, release, recovery and privacy gates.
9. Perform lower-risk structural cleanup only after behavior parity.

## 5. Cross-impact controls

- Database: no Entity/schema/migration/data/role/RLS/trigger work outside `DB_GOV_REMEDIATION_REGISTER.md` and central DB-GOV review.
- API: compatibility inventory and negative authorization tests precede contract changes.
- Desktop/Mobile: screen IDs, RTL behavior, signing, secure storage and offline policy are preserved.
- Accounting: no source document may be `POSTED` without a linked balanced immutable journal in the governed atomic boundary.
- Offline: no generic executor; only versioned, typed, explicitly authorized operations.
- Security: tenant/user/device are server-derived and independently validated at API, service and DB layers.

## 6. Blocking and owner-decision boundaries

Unknown evidence is carried in `UNKNOWN_AND_BLOCKERS_REGISTER.md`. It blocks its affected execution/release gate, not this plan. Owner authority is required only for Production change, data repair, destructive migration, merge/delete/force-push, disposal of preserved assets, irreversible change, or a canonical product/accounting/offline decision explicitly reserved to the owner.

## 7. Definition of ready for MISSION-03

MISSION-03 may receive this package but may start only after Control Tower confirms the seal and revalidates refs. Each Wave starts independently only when its Entry Criteria are satisfied. Any mismatch between this plan and current evidence is `STOP — REPLAN`, never permission to guess.
