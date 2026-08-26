# TransportERP — PR #69 Full Execution Mechanism

> **Execution update — 2026-08-27:** This mechanism is the historical plan. The owner later delegated G4/G5 and Ready authority while still prohibiting merge. Stages 4 and 5 have since been implemented and exact-SHA verified; current status is in `PR69_EXECUTION_CHECKPOINT.md` and `PR69_FULL_EXECUTION_AND_COMPLETION_REPORT.md`.

**Record:** `PR69-EXEC-MECH-20260826-01`  
**Prepared:** 2026-08-26, Asia/Riyadh  
**Repository:** `shfeekalbhure/TransportERP`  
**Branch:** `codex/p1-security-device-sync-offline-20260825`  
**Starting exact SHA:** `0274672bae3f072c0039fa96da03203e6759cbd9`  
**Authority:** Full implementation on the PR branch. Merge, Ready-for-review, auto-merge, production deployment, production migrations, production secrets, production Offline enablement, and owner G4/G5 approval remain prohibited.

## 1. Starting fingerprint

| Item | Verified state at start |
|---|---|
| PR | `OPEN / DRAFT / UNMERGED`, mergeable at the observation time |
| Base | `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` |
| Head | `0274672bae3f072c0039fa96da03203e6759cbd9` |
| Change set | 22 commits, 70 changed files, +14,732 / -665 |
| Reviews | No submitted GitHub reviews on the starting head |
| Commit statuses | Empty combined-status collection; it is not evidence of failure or success |
| GitHub Actions | CI run `32994091571` succeeded on the exact starting SHA; Core/PostgreSQL/HTTP job `98258686808` reported `291 passed / 0 failed / 0 skipped`; Desktop job `98258687185` succeeded. Foundation and W0-3 validators succeeded; path-filtered A/B/C/W0-5 workflows were skipped and are not counted as PASS. |
| Runtime gate | Production registration is `ClosedSyncRuntimeGate`; Offline remains closed |
| Local execution environment | Repository is available, but this worker has no local .NET/PostgreSQL runtime. Python validators and static checks run locally; .NET/PostgreSQL/Windows verification is fail-closed in GitHub Actions. |

Evidence recorded against an earlier SHA becomes `STALE` for changed executable paths. Historical failures remain preserved and are never rewritten as successes.

## 2. Actual stage state

| Stage | Implemented now | Missing or not proven |
|---|---|---|
| 1 — identity, sessions, RBAC, audit | Substantial server runtime, PostgreSQL schema, HTTP and persistence tests; CI green | Exact-head independent review and owner approval |
| 2 — tenant/branch isolation and atomic audit | Broad scoped services and negative tests | `OperationalPartyId` is not scope-validated during waybill update; the database FK proves existence only. A complete client-controlled reference sweep is required. |
| 3 — registered device trust | Device registration/assignment/lifecycle, credential version binding, tests and migrations | Exact-head independent review; no authority to treat the Stage 3 credential as request PoP |
| 4A — Sync-PoP transport security | Key lifecycle, nonce, proof validation, replay claim, request/body limits, `sync-v1`, enqueue/idempotency foundation, proof cleanup, tests | These controls do not execute approved business actions |
| 4B — commercial sync runtime | Action contract table exists | Every allowed write action currently returns `ACTION_RUNTIME_UNAVAILABLE`; no bounded business dispatcher, atomic execution claim worker, complete conflict endpoint/runtime, or 90-day payload/snapshot redaction worker |
| 4C — G4 evidence | 291-test CI is green on the starting SHA | The 203-row acceptance register is still `SPECIFIED_NOT_EXECUTED` unless individually linked; T-SYNC-010 is client-side; no full per-action evidence matrix or final independent review |
| 5 — Offline client | Project shells and legacy screen-contract surface only | No encrypted durable local outbox, secure key storage, retry/nonce/PoP client, recovery, sync operations UI, or E2E evidence |

## 3. Priorities and blockers

### P0

1. Cross-company/cross-branch `OperationalPartyId` linkage during waybill mutation.
2. Sweep every client-controlled UUID/FK in the PR delta for unscoped lookup or existence leakage.
3. Commercial idempotency and atomic execution claim before any dispatcher can create business effects.
4. Database migration safety, especially fail-closed downgrade with Stage 4/5 data.
5. Proof/token/nonce/credential and redacted-payload leakage controls.

### P1

1. Bounded business dispatcher and per-action adapters.
2. Execution/retry worker with atomic claim and restart recovery.
3. Conflict resolution endpoint and replacement-operation transaction.
4. Server retention/redaction and effective settings hierarchy.
5. Desktop Offline client first, followed by the approved Mobile scope.
6. End-to-end interruption/recovery/concurrency tests and exact-SHA evidence.

### P2

Performance tuning, non-risky duplication cleanup, diagnostics, and non-blocking UX improvements. P2 cannot delay a P0/P1 correction but cannot be mixed into its security commit.

## 4. Dependency map

1. `P0 reference isolation` is a prerequisite for dispatcher adapters that mutate waybills.
2. `Action contract + dispatcher adapters` is a prerequisite for the execution worker.
3. `Atomic operation claim + business idempotency` is a prerequisite for retries and multi-instance tests.
4. `Conflict creation` depends on dispatcher version checks; conflict resolution depends on atomic replacement creation.
5. `Server runtime stable` is a prerequisite for the Desktop outbox protocol implementation.
6. `Desktop durable queue + server runtime` is a prerequisite for T-SYNC-010 and G4 end-to-end evidence.
7. `All implementation and exact-SHA CI` precede the independent review.
8. Independent review findings must be closed before the owner receives the G5 decision package.

## 5. Work waves, ownership, tests, and commits

| Wave | Owner | Exclusive primary paths | Required result | Commit boundary | Acceptance gate |
|---|---|---|---|---|---|
| W0 — mechanism/baseline | Execution lead | this record; governance index only | Exact-head fingerprint, stale-evidence rule, dependencies and gates | `docs(governance): record PR69 execution mechanism` | Repository diff clean; validators pass; no runtime claim |
| W1 — P0 reference isolation | Security/isolation team | `WaybillApplicationService.cs`, `WaybillPersistenceServices.cs`, relevant waybill model/migration/tests | Application and persistence scope validation plus PostgreSQL enforcement for `OperationalPartyId`; no existence leak | `fix(security): isolate waybill operational party references` | Negative same-company/wrong-branch and cross-company HTTP/PostgreSQL/replay tests pass |
| W2 — remaining reference sweep | Security/isolation team | API/application/persistence files identified by the sweep; dedicated negative tests | All client-controlled references are scoped or explicitly classified | One logical commit per domain, never one bulk cleanup | Tenant A cannot infer or mutate Tenant B records; `SCOPE_DENIED` contract is uniform |
| W3 — dispatcher foundation | Stage 4 runtime team | new bounded sync application/runtime files, `SyncApiModule.cs`, DI, adapter tests | Exact table-driven ActionCode registry; no reflection/generic dispatch; unavailable actions explicit | `feat(sync): add bounded business dispatcher` | Per-action table tests; required permission/scope/entity/version/result contract verified |
| W4 — execution/retry worker | Stage 4 runtime team | sync execution service/worker, sync entities/model/migration, worker tests | Atomic claim, lease/recovery, independent server retry budget, transition audit | `feat(sync): add atomic execution retry worker` | Two-worker race yields one business effect; restart and exhaustion tests pass |
| W5 — conflict runtime | Stage 4 runtime team | conflict endpoint/service/migration/tests | `KEEP_SERVER_AND_REJECT_LOCAL` and `REAPPLY_AS_NEW`; separate permission; single-resolution guarantee | `feat(sync): complete conflict resolution runtime` | Concurrent resolution/idempotency/scope/audit tests pass |
| W6 — retention/settings | Stage 4 runtime team | retention worker/service, configuration validation, migrations/tests | 24h/7d client policy contract, 90d server redaction, metadata preserved, effective hierarchy fail-closed | Separate retention and settings commits | Rerunnable concurrent cleanup; API/audit/log non-disclosure; invalid override closes scope |
| W7 — G4-SERVER | QA/evidence team | tests, CI workflow where necessary, acceptance register/evidence | Server test matrix executed and linked to exact SHA | `test(sync): close G4 server evidence gaps`, then evidence-only commit | No Critical/High; PostgreSQL Up/Down/Up and fail-closed Down; multi-instance/replay matrix complete |
| W8 — Desktop Offline client | Stage 5 client team | `TransportERP.Desktop/Offline/**`, project references, client tests | Encrypted durable outbox, OS secure key, nonce/PoP per attempt, independent retry, recovery, cache separation, UI | Small commits: storage, protocol, worker, UI | Offline production gate unchanged; queue survives restart; no duplicate effects |
| W9 — approved Mobile scope | Stage 5 client team | Mobile shared/client paths only | Reuse protocol contracts without weakening secure storage or tenant isolation | Per client surface | Build and platform-safe storage tests pass; unsupported platforms fail closed |
| W10 — E2E and independent review | QA plus reviewer who wrote no target code | E2E tests and final review/evidence records | Interruption, timeout-after-commit, conflict, rotation/suspension/session/scope, partial batch, retries and retention all evidenced | Test/evidence commits only | CI green on final exact SHA; independent findings classified and no Critical/High open |

Agents remain read-only until the execution lead assigns a wave and an exclusive path set. Cross-owner files (`Program.cs`, DbContext, model snapshot, CI and governance index) are changed serially by the execution lead or through a declared handoff.

## 6. Detailed Stage 4 acceptance sequence

1. Preserve the current PoP/enqueue behavior and production `ClosedSyncRuntimeGate`.
2. Convert the action table from validation-only to a registry with an explicit runtime availability flag and typed handler.
3. Validate action, scope, permission, `EntityId`, and `BaseVersion` before the operation becomes executable.
4. Claim one due operation atomically with a lease/claim token; a second worker must receive no claim.
5. Execute the typed business service in a transaction that records the business idempotency key and result mapping.
6. Persist `ResultEntityId` and `ResultVersion`, then append the transition AuditEvent atomically.
7. Classify retryable failure without incrementing counters for proof replay or duplicate enqueue.
8. Create a conflict with snapshots only when the action contract permits it.
9. Resolve conflicts through a separate permissioned endpoint; replacement creation and supersession are one transaction.
10. Redact terminal payloads/snapshots after the governing retention period without deleting audit metadata or hashes.

`G4-SERVER` requires the server portions of T-SYNC-001..009 plus the full Stage 4 security/migration matrix. `G4-END-TO-END` also requires the Stage 5 client and T-SYNC-010. Neither label is owner `G4 PASS`.

## 7. CI failure protocol

1. Pin the failing run, job, step and exact SHA; never replace it with only the later green run.
2. Reproduce locally where the environment supports it; otherwise add the narrowest diagnostic assertion and use the required CI environment.
3. Determine whether failure is product code, test defect, environment, migration or flaky concurrency.
4. Fix the cause in a new logical commit. Governing assertions are not weakened unless the contract is proven wrong and separately changed under authority.
5. Re-run the complete required CI, not only the formerly failing test.
6. Mark evidence from the failed SHA historical and evidence from earlier changed code stale.

## 8. Rollback protocol

- Each wave is a small fast-forward commit and is reverted by a normal revert commit; history is never rewritten and force-push is prohibited.
- Schema changes use additive migrations. A Down path must be fail-closed when removing it could orphan Stage 4/5 data.
- Workers and dispatch adapters remain unreachable through the production gate until owner G5.
- Risky background processing uses a disabled-by-default registration/configuration boundary until its exact migration and concurrency tests pass.
- No rollback deletes audit events, accepted proofs, business idempotency records or owner evidence.

## 9. Evidence contract per exact SHA

For every acceptance row retain: exact SHA and tree, test name, command/job, result, relevant sanitized request/response, database effect, AuditEvent, CI URL, defect/history note, reviewer identity and classification. Test totals are supporting evidence only and never replace row-level mapping.

## 10. Transition and final closure criteria

### Stage 4 to Stage 5 implementation

Stage 5 implementation may start with production Offline closed only when P0 isolation is closed, the server dispatcher/worker/conflict/retention runtime is stable, its migrations are safe, and the server CI slice is green. This task is the owner's explicit authorization to build Stage 5 while keeping production Offline disabled; it is not G4/G5 approval.

### Final handoff

The execution team stops at `IMPLEMENTATION COMPLETE — CI GREEN — INDEPENDENT REVIEW COMPLETE — OWNER DECISION REQUIRED` only after:

- all P0 findings are closed;
- server and client runtime plus E2E tests are complete;
- final exact-SHA CI is green;
- evidence rows are linked and an independent reviewer reports no open Critical/High;
- PR remains Draft/unmerged and Offline production remains closed.

The final report is `PR69_FULL_EXECUTION_AND_COMPLETION_REPORT` and may state only `PASS — IMPLEMENTATION AND EVIDENCE COMPLETE — OWNER G5 DECISION REQUIRED` or `FAIL — BLOCKERS REMAIN`.
