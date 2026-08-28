# DBP-002/004/005/006 — Review Preparation

> Superseded for current Greenfield physical-design review by
> `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md v1.0` and
> `GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`. This file remains preserved as
> the pre-Greenfield preparation record and grants no execution authority.

- Bound governance: `e8d443dc5cefb6a1ea131311cfb7b2ded569b8df`
- Source baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `READY FOR INDEPENDENT DB-GOV DESIGN REVIEW — NO REHEARSAL OR EXECUTION AUTHORITY`
- Entity/DbContext/Migration/schema/seed/data mutation: `NONE`

This package completes repository-resolvable inventory and proposed additive
boundaries. It intentionally contains no executable DDL and grants itself no
DB-GOV approval.

## DBP-002 — tenant consistency and membership

Current evidence:

- Company is the tenant root; Branch belongs to exactly one Company.
- User currently carries independent `CompanyId` and nullable `BranchId` FKs.
- Branch has alternate key `(Id, CompanyId)`, but User does not use a composite
  branch/company FK.
- `UserRole(UserId, RoleId)` and `UserPermissionOverride(UserId, PermissionId)`
  cannot represent the same grant independently across explicit memberships.

Proposed additive staging:

1. inventory all user/company/branch mismatches and role/RLS/grants read-only;
2. add explicit membership identity and tenant-consistent composite keys only
   after safe-copy review;
3. copy only rows whose source scope is unambiguous; quarantine every null,
   mismatch or duplicate rather than infer authority;
4. dual-read/compare, then dual-write under a feature gate;
5. retain legacy `User.CompanyId/BranchId` until parity, rollback rehearsal and
   every API/worker/client consumer are verified;
6. cross-tenant negative queries and raw-SQL FK/check tests are mandatory.

Recovery is compatible-reader plus forward correction or verified safe-copy
restore. No destructive column removal is proposed.

## DBP-004 — audit V2 and atomic enlistment

Preserve every V1 byte/hash and verifier. Proposed additive V2 requires
`HashVersion`, immutable stream key/sequence, predecessor hash, a canonicalizer
covering every persisted semantic field, and an append boundary that enlists in
the caller-owned transaction. Mixed V1/V2 verification cannot rehash history.

Rehearsal gates: sanitized fixed V1 vectors, live ordering/duplicate inventory,
roles/triggers/RLS, backup/restore, failure at each caller stage, and independent
approval. Privacy minimization cannot rewrite historical audit evidence.

## DBP-005 — governed Settlement persistence

ACC-001 fixes the boundary: operational Collection never posts GL; Settlement
atomically creates/posts voucher, balanced journal, collection/source links,
audit and outbox. Proposed additive design needs:

- immutable Settlement header and collection links;
- configured account-role and FX/rounding snapshot references;
- maker/checker identities and open-period evidence;
- unique source/idempotency and unique reversal lineage;
- database-enforced balanced POSTED journal and immutable posted history;
- tenant-consistent FKs from DBP-002 and transaction-aware audit from DBP-004.

Current `CollectionTransaction.AccountingReferenceId` is append-only and cannot
be safely filled later, so it must not be repurposed through an ungoverned
update. Exact mappings/roles/FX values, legacy reconciliation and safe-copy
backup/restore remain external gates.

## DBP-006 — typed Offline/Sync persistence

OFFLINE-001 resolves action authority but current persistence omits
`ProtocolVersion`, `ActionCode`, session/security version, registered-device/PoP
identity, canonical fingerprint, action permission/policy version, worker
claim/lease, result/outbox and nonce replay keys.

Proposed additive design:

- company-scoped `(RegisteredDeviceId, ClientOperationId)` idempotency plus
  immutable fingerprint; same key/different fingerprint is conflict;
- explicit action/protocol/version and tenant/user/device/session provenance;
- claim/lease/attempt/result/outbox state with deterministic recovery;
- unique nonce per registered device/key and quarantine after revoke;
- old generic rows remain readable but `ExecutionEligible=false`; no guessed
  ActionCode/provenance backfill;
- client encrypted SQLite outbox is a separate governed schema.

Activation requires DBP-002, DBP-003B/C, compatible reader, authorized safe
copy, backup/restore, retention/legal-hold, platform key/attestation evidence,
two-worker/concurrency/failure tests and independent DB-GOV approval.

## Shared safe-copy entry gate

The named non-Production copy must provide exact migration history, PostgreSQL
version, non-secret roles/memberships/grants/default privileges, extensions,
constraints/indexes/triggers/RLS/policies, sanitized row-shape aggregates,
schema/custom backup digests, restore to a new disposable instance and
pre/post reconciliation. Any mismatch stops only the affected DBP.
