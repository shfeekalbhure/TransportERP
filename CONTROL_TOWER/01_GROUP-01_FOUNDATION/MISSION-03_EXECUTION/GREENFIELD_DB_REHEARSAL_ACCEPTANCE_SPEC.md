# Greenfield DB Rehearsal Acceptance Specification

- Bound design: `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md v1.0` + `DBP-003BC_003A_006_PHYSICAL_DEPENDENCY_CORRECTION_v1.1.md`
- Corrected physical order: `DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`
- Baseline: `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Environment: isolated PostgreSQL `18.6`, empty, non-Production
- State: `TEST/RECOVERY SPECIFICATION — FRESH INDEPENDENT DB-GOV REVIEW REQUIRED; REHEARSAL NOT YET AUTHORIZED`

## Entry gate

1. Independent DB-GOV records the exact DBP units approved for disposable rehearsal after the v1.1 physical dependency correction exists.
2. The Product parent SHA/tree and candidate migration file hashes are fixed.
3. `pg_dump --schema-only` and custom-format backups of the base-ten state are hashed and restore successfully to a second empty instance.
4. Runtime/migrator role separation and no-secret connection injection are evidenced.
5. No Production endpoint, data, role or credential is reachable.

## Ordered run

1. Restore/build/tests on the exact candidate head.
2. Create empty database; apply the ten existing migrations; require `10/10`.
3. Capture catalog, extensions, roles, grants, default privileges, RLS/policies, constraints, indexes and migration history.
4. Apply authorized candidate units only in this order: `DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`, preserving per-unit logs and stopping dependents after any failed unit.
5. Require EF pending-model check = none and snapshot/catalog reconciliation.
6. Run all proposal suites below.
7. Boot API and require unauthenticated boundary HTTP 401.
8. Backup candidate database, restore to a new PostgreSQL 18.6 instance, rerun invariants and compare catalog/history/count/hash manifests.
9. Exercise forward correction and discard/recreate recovery. Do not treat migration `Down()` as recovery proof.

## DBP-002 suite

- company and branch membership selection, multi-company and multi-branch;
- invalid branch/company composite FK;
- duplicate/null-distinct membership;
- expired/revoked/suspended membership;
- company role mismatch and deny-over-allow permission;
- application, worker, readonly and missing-context raw SQL A↔B in both directions for SELECT/INSERT/UPDATE/DELETE;
- attempt to disable/bypass RLS or use unqualified/search-path object;
- API and worker claims cannot widen persistent membership/grants.

Acceptance: zero cross-tenant rows/effects and every missing/malformed context fails closed.

## DBP-003 suite

- DBP-003B/C physical creation after DBP-002/004, including `registered_devices` unique `(Id,CompanyId)` before DBP-003A session-device FK creation;
- device activation/assignment/transfer/lost/replaced/revoke, wrong owner, revoked key, signature tamper, clock skew, nonce/JTI replay and override audit;
- device lifecycle commands that require session-family revocation remain disabled until DBP-003A passes;
- device lifecycle commands that require Offline quarantine remain disabled until DBP-006 passes;
- valid enrollment/login; invalid/malformed hash; disabled and locked user;
- five failures/window, concurrent failure counter, timed unlock, admin unlock audit, reset token one-time/expiry and password reset family revoke;
- issued access accepted; expired, revoked, stale security/membership version, wrong tenant/device denied;
- refresh rotation, one successor, reuse family revoke, logout/current/family revoke and client-clear contract;
- 50 concurrent same-token refresh calls: at most one successor, family ends revoked when reuse is observed;
- failure before/after predecessor, successor, audit, save and commit;
- SQLSTATE `40001/40P01`, digest/operation/lineage uniqueness and ambiguous commit reconciliation.

Acceptance: no successor/session/device mutation without matching Audit V2; `auth_sessions.RegisteredDeviceId` is tenant-consistent and FK-backed only after `registered_devices` exists; raw refresh/private proof values never appear in DB/log/audit.

## DBP-004 suite

- fixed canonicalizer vectors for null/empty, Unicode NFC, UUID/time/decimal, JSON property order, arrays and malformed JSON;
- every persisted semantic field independently changes HashV2;
- concurrent 100-event append to one stream produces contiguous unique sequence;
- raw SQL UPDATE/DELETE/TRUNCATE and stream-head rewind denied;
- caller failure before audit, after audit add, after outbox add, after save and before commit leaves none or all;
- restore and verify every stream/hash.

Acceptance: immutable, gap-free committed stream and no orphan audit/outbox.

## DBP-005 suite

- configured debit/credit/FX/rounding profile and missing/invalid configuration;
- unbalanced, zero, wrong currency/company/branch/account, closed period, maker=checker and permission denial;
- one Collection included once, concurrent same source/operation, duplicate document/source, and failure at every UoW stage;
- successful Settlement creates Settlement + voucher + balanced posted journal + links + Audit V2 + Outbox atomically;
- reversal creates one exact linked inverse and leaves original immutable;
- raw SQL posted header/line update/delete denied.

Acceptance: `POSTED` iff a balanced immutable linked journal exists; none or all of the atomic boundary survives failure.

## DBP-006 suite

- verify all membership/session/device/proof durable dependencies already exist before Offline candidate creation;
- unknown protocol/action, DELETE and online-only action denied effect-free;
- payload size/schema/hash/fingerprint tamper;
- A↔B, user/device/session/membership/key mismatch;
- same key/same hash returns stored result; same key/different hash conflicts;
- two workers, lease expiry/steal, crash before/after business mutation, reordered delivery, retry exhaustion and lost response;
- revoke between intake/claim/commit quarantines with zero business effect;
- Offline result/business/Audit/Outbox atomicity;
- retention cleanup respects active rows, legal holds and tombstone periods.

Acceptance: one authorized effect at most, deterministic stored result, no authority widening and restart convergence.

## Recovery and evidence bundle

Retain exact SHA/tree/parent, changed files, SDK/runtime/container images, commands, raw logs/TRX, failed then corrected runs, base/candidate/restored database identifiers, migration/catalog/policy manifests, backup hashes, artifact hashes and rollback/forward-recovery outcome. Sanitize connection strings and never retain secrets or real personal data.

Final rehearsal result for each DBP is one of `PASS`, `FAIL`, or `BLOCKED`. `PASS` is evidence for a later DB-GOV decision; it is not Production authority.
