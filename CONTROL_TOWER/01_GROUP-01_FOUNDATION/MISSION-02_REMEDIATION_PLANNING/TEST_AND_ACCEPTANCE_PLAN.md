# Test and Acceptance Plan

## Evidence rules

All evidence must name commit/tree, environment, command, exit code, test counts, tool/runtime versions and artifact SHA-256. No skipped/conditional job is a PASS. No result transfers between master and PR69. Logs must contain neither Production secrets nor unnecessary personal data.

| Test ID | Scope / findings | Required test set | Acceptance criteria | Evidence |
|---|---|---|---|---|
| `T-000` | W0 baseline | restore, build, test discovery, all tests, fresh migrate, pending-model check, API boot, Desktop/Mobile target probes | deterministic exit results; every failure classified; no silent skip | logs/TRX/inventory/artifact hashes |
| `T-100` | `A-ARCH-002` | explicit non-null/null Volume create→update→reload→allocation; concurrency/idempotency; affected-row query | exact value survives; no other item field regresses; impact population accounted for | PostgreSQL logs + before/after rows on disposable copy |
| `T-200` | identity/RBAC | login/refresh/revoke, inactive user, stamp/version change, permission grant/revoke, stale token, rate limits | every stale/mismatched session fails closed; allowed session retains exact scope | API + DB negative matrix |
| `T-210` | tenant | A→B and B→A company/branch/user/device direct ID, query, worker, export, report and raw SQL | zero cross-tenant read/write; migration/app roles least privilege | request/response + SQL assertions |
| `T-220` | device/Sync owner | unregistered/revoked/expired/wrong user/device/branch, PoP replay, every transition/retry/conflict path and privileged override | non-owner denied; override is least-privilege, reasoned and immutable-audited | API/service/PostgreSQL tests |
| `T-300` | UoW/accounting | balanced/unbalanced, duplicate/idempotent retry, period closed, currency/rounding, SoD, journal source uniqueness, reversal, injected failure | source cannot be POSTED without linked balanced journal/audit/outbox; rollback leaves none or all | transaction/failure logs + reconciliation query |
| `T-320` | audit/append-only | legacy/new hash, omitted fields, mutation via EF and raw SQL, concurrent append, export/redaction | old and new chains verify; mutation denied; failed business write has no orphan audit | chain report + SQL denial evidence |
| `T-400` | Offline | action allowlist, unsupported/DELETE, duplicate/reorder/replay/restart, conflict, Base/ResultVersion, retention, revocation, schema compatibility | only authorized action executes once; rejected actions stay effect-free; restart converges | client/server store snapshots + E2E logs |
| `T-500` | Desktop/Mobile | build/install/sign, startup/config failure, auth/revoke, secure storage, API TLS, offline/online, RTL/accessibility, upgrade local cache | executable artifact launches; invalid config fails closed; no secret leakage; canonical screen routes pass | signed artifact hashes, UI automation, screenshots |
| `T-600` | Shipping | every state/quantity/custody transition, partial/full transfer, arrival/unload/delivery/POD/return/claim/customs, settlement | state machine, quantity conservation, audit/accounting/idempotency all pass | numbered acceptance cases |
| `T-610` | Ticketing | booking/seat capacity, duplicate booking, payment modes, driver variance, transfer/cancel/refund, tenant/offline policy | canonical cases pass; no inferred behavior; accounting/reversal trace complete | requirement-case crosswalk + E2E |
| `T-620` | Reporting | authorization, tenant, as-of, currency, reconciliation, redaction, performance | totals reconcile to source; read-only and tenant-safe; documented freshness | query plans + signed report samples |
| `T-700` | CI/coverage | required jobs for server/DB/Desktop/Mobile/Offline/E2E, coverage thresholds, artifact upload/retention | all required jobs PASS on exact head; coverage meets approved thresholds; artifacts retrievable | workflow URLs, job logs, TRX/coverage hashes |
| `T-710` | supply chain | locked restore, approved sources, SCA, SBOM, licenses, provenance/signature | deterministic graph; no unaccepted critical advisory/license; provenance validates | lock/SBOM/SCA/license/signature reports |
| `T-720` | release/recovery | artifact install, upgrade representative DB/client, rollback, backup/restore to new instance, RPO/RTO drill | identical artifact identity; restore counts/hashes/invariants; operator runbook succeeds | drill report + artifact/backup hashes |
| `T-730` | privacy | classification, minimization, log redaction, crypto/key failure, retention/legal hold, export/delete/anonymize, offline cache | policy-approved results; no secrets/PII leakage; legal hold wins where required | privacy test report + sanitized samples |
| `T-800` | refactor parity | dependencies, public contracts, test discovery/count, runtime behavior, migrations and evidence before/after | no unintended delta; all consumers migrated; removal approved | comparative manifests/diffs/tests |

Acceptance is fail-closed. A single unauthorized cross-tenant, accounting imbalance, lost field, incompatible queue, unsigned artifact, or failed restore blocks the affected Wave exit.
