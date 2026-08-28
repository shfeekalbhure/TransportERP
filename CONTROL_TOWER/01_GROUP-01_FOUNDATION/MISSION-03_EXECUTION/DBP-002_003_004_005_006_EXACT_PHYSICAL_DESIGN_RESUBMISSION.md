# DBP-002/003/004/005/006 — Exact Greenfield Physical Design Resubmission

- Revision: `v1.0`
- Product baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Product tree: `00512125311306a43474638195d2cad97b76118e`
- Database baseline: `DB-BASELINE-001 = GREENFIELD / NEW / EMPTY`
- PostgreSQL target for rehearsal: `18.6`, isolated, disposable, non-Production
- Current lineage: the ten committed migrations, unchanged
- Requested decision: `APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`
- Current authority while this document is under review: `DESIGN ONLY — NO ENTITY/DBCONTEXT/MIGRATION/SCHEMA/SEED/DATA AUTHORITY`

This document replaces the incomplete physical portions of
`DBP-002_004_005_006_REVIEW_PREPARATION.md` and the legacy-target assumptions in
`DBP-003A_REHEARSAL_RESUBMISSION.md`. It does not erase those historical files.
It is an exact review specification, not executable DDL and not a self-approval.

## 1. Revalidated current physical state

The exact execution tree has one `TransportErpDbContext`, ten migration
implementations and no candidate DBP migration. The current schema contains
`companies`, `branches`, `users`, `roles`, `permissions`, legacy
`user_roles`/`user_permission_overrides`, accounting journals/vouchers,
`audit_events`, generic `sync_operations`, Waybill and Shipping tables.

The following required durable objects do not exist: explicit user membership,
security state, application sessions, device registry/assignment, proof keys,
replay ledger, Audit V2 stream heads, shared outbox, governed Settlement, typed
Offline inbox/result/claim/lease. `users.PasswordHash` exists as required
`varchar(500)`, but no generator/verifier or versioned format exists.

The legacy `users.CompanyId/BranchId`, `user_roles`,
`user_permission_overrides`, generic `sync_operations` and V1 audit fields stay
physically present through rehearsal. They are not request-time authority after
the new readers are activated. No destructive removal is in this bundle.

## 2. Dependency-ordered candidate migration units

Names below are reserved review identities. Timestamps are assigned only when
DB-GOV opens authoring.

| Order | Candidate migration unit | Proposal | Depends on |
|---:|---|---|---|
| 11 | `GreenfieldTenantMembershipIsolation` | DBP-002 | existing ten migrations |
| 12 | `GreenfieldAuditV2AndAtomicOutbox` | DBP-004 | DBP-002 |
| 13 | `GreenfieldLocalAuthSessions` | DBP-003A | DBP-002/004 |
| 14 | `GreenfieldDeviceRegistryAndProof` | DBP-003B/C | DBP-002/003A/004 |
| 15 | `GreenfieldTypedOfflineProtocol` | DBP-006 | DBP-002/003A/B/C/004 |
| 16 | `GreenfieldGovernedSettlement` | DBP-005 | DBP-002/004 |

Each unit is separately revertible before activation. Rehearsal applies all
units in the order above only if the independent decision approves the complete
bundle. A failed unit stops later units and preserves its logs/database image.

## 3. DBP-002 — tenant membership and grants

### 3.1 Authoritative scope

`Company` is the tenant root. A `Branch` belongs to exactly one Company. A user
may have multiple company memberships and multiple branch memberships. Every
request selects exactly one active membership server-side; client/JWT company,
branch, role and permission claims are selectors only and cannot widen it.

### 3.2 `user_memberships`

| Column | PostgreSQL type / rule |
|---|---|
| `Id` | `uuid PRIMARY KEY` |
| `UserId` | `uuid NOT NULL REFERENCES users(Id) ON DELETE RESTRICT` |
| `CompanyId` | `uuid NOT NULL REFERENCES companies(Id) ON DELETE RESTRICT` |
| `BranchId` | `uuid NULL` |
| `ScopeType` | `varchar(12) NOT NULL`, `COMPANY` or `BRANCH` |
| `Status` | `varchar(12) NOT NULL`, `ACTIVE`, `SUSPENDED`, `REVOKED` |
| `SecurityVersion` | `bigint NOT NULL DEFAULT 1 CHECK >= 1` |
| `ValidFrom` | `timestamptz NOT NULL` |
| `ValidTo` | `timestamptz NULL` |
| `CreatedAt/UpdatedAt` | `timestamptz NOT NULL` |
| `CreatedBy/RevokedBy` | `uuid NULL REFERENCES users(Id) ON DELETE RESTRICT` |
| `RevokeReason` | `varchar(500) NULL` |
| `ConcurrencyVersion` | `bigint NOT NULL DEFAULT 1 CHECK >= 1` |

Constraints and indexes:

- composite FK `(BranchId, CompanyId) -> branches(Id, CompanyId)`;
- `ScopeType='COMPANY'` iff `BranchId IS NULL`; `BRANCH` iff non-null;
- `ValidTo IS NULL OR ValidTo >= ValidFrom`;
- revoked status requires `ValidTo`, `RevokedBy` and nonblank reason;
- unique `(Id, UserId, CompanyId)` for dependent composite FKs;
- unique `(UserId, CompanyId, BranchId) NULLS NOT DISTINCT`;
- indexes `(UserId, Status, CompanyId)`, `(CompanyId, BranchId, Status)` and
  `(CompanyId, UpdatedAt)`.

### 3.3 Persistent grants

`user_role_grants` columns are `Id`, `MembershipId`, `UserId`, `CompanyId`,
nullable `BranchId`, `RoleId`, `Status`, `ValidFrom`, nullable `ValidTo`,
`GrantedBy`, nullable `RevokedBy`, nullable `Reason`, timestamps and
`ConcurrencyVersion`. It has:

- composite FK `(MembershipId,UserId,CompanyId)` to `user_memberships`;
- branch/company composite FK;
- FK `RoleId -> roles(Id)`;
- unique active `(MembershipId,RoleId)`;
- the membership and grant branch must be `IS NOT DISTINCT FROM` one another;
- a company-owned role must match `CompanyId`; a system role has
  `roles.CompanyId IS NULL`. A named constraint trigger enforces this at commit.

`user_permission_grants` has the same membership/scope/audit columns plus
`PermissionId` and `Effect varchar(5) CHECK IN ('ALLOW','DENY')`. Unique active
key is `(MembershipId,PermissionId)`. An explicit DENY wins over role grants.
Existing `user_roles` and `user_permission_overrides` receive no application
write grant and are not read by the new resolver.

### 3.4 PostgreSQL roles and RLS-equivalent bootstrap

The rehearsal creates NOLOGIN group roles; no credential is stored:

- `transporterp_schema_owner`: owns schema/functions/tables, `NOLOGIN`, not
  granted to application principals;
- `transporterp_migrator`: `NOLOGIN`, may assume schema owner only in the
  isolated migration job;
- `transporterp_app`: `NOLOGIN NOBYPASSRLS` with bounded CRUD;
- `transporterp_worker`: `NOLOGIN NOBYPASSRLS` with bounded Offline/Outbox CRUD;
- `transporterp_readonly`: `NOLOGIN NOBYPASSRLS`, tenant-filtered SELECT only.

Deployment LOGIN roles are external and may only be granted one group role.
The connection initializer starts a transaction and uses `SET LOCAL` for
`app.user_id`, `app.membership_id`, `app.company_id`, `app.branch_id`,
`app.session_id`, `app.security_version` and `app.device_id`. Missing, malformed
or non-authoritative values produce no rows. Security-definer functions are
forbidden for business CRUD. Helper functions are `SECURITY INVOKER`, stable,
have an empty fixed `search_path`, and return null on a missing setting.

Every tenant table is `ENABLE ROW LEVEL SECURITY` and `FORCE ROW LEVEL
SECURITY`. Policies require `CompanyId = current_company_id()`. A row with a
non-null `BranchId` additionally requires either the authoritative membership
is company-scoped or `BranchId = current_branch_id()`. Membership/grant/session/
device/offline/accounting tables also require the row membership/user/device
selectors to equal the transaction-local authoritative values. `WITH CHECK`
equals `USING`; cross-tenant inserts cannot rely on later validation.

The migration owner is different from runtime roles. PUBLIC receives no schema
CREATE, table, sequence or function EXECUTE privilege. Default privileges are
revoked before grants. Raw SQL A-to-B/B-to-A SELECT/INSERT/UPDATE/DELETE and
missing-context tests are mandatory.

## 4. DBP-003A — password, security version and sessions

### 4.1 New-system password policy

There is no legacy compatibility requirement. The new format is an ASCII,
versioned envelope stored in existing `users.PasswordHash`:

`$terp$pbkdf2-sha256$v1$i=600000,l=32$<base64-32-byte-salt>$<base64-32-byte-derived-key>`

- PBKDF2-HMAC-SHA-256, exactly 600,000 iterations, 32 random salt bytes and 32
  derived bytes;
- salt/token entropy comes only from the OS CSPRNG;
- verifier parses bounded lengths/iteration values, rejects malformed or
  unknown envelopes without exception leakage and uses constant-time compare;
- UTF-8 input is normalized as Unicode NFC; minimum 12 Unicode scalar values,
  maximum 256 scalars and maximum 1024 UTF-8 bytes;
- no password is logged, audited, queued, returned or retained in managed
  caches; no reversible encryption or plaintext fallback exists;
- parameters are stored in the envelope. A higher approved policy marks a
  successful login for rehash in the same caller-owned transaction;
- a Greenfield bootstrap user must arrive through a one-time reset/enrollment
  flow. No default password or seed user is allowed.

Lockout policy: five failed verifications inside a rolling 15-minute window
locks the account for 15 minutes. A further failure after expiry starts a new
window. Successful verification clears count/window. Disabled users always
deny without password timing distinction. All responses use the same public
`INVALID_CREDENTIALS` result. Administrative unlock requires permission,
reason and Audit V2; it increments user `SecurityVersion` and revokes every
session family. Password reset does the same.

### 4.2 `user_security_state`

Primary/FK `UserId`; `SecurityVersion bigint >=1`; `AccessFailedCount integer
>=0`; nullable `FailureWindowStartedAt`, `LastFailedAt`, `LockoutEnd`;
`PasswordChangedAt`; `PasswordHashVersion smallint DEFAULT 1 CHECK = 1`;
timestamps and `ConcurrencyVersion bigint >=1`. Shape checks require a zero
count to have null failure window and a positive count to have a window. The
row is created atomically with every user.

`password_reset_tokens` stores `Id`, `UserId`, `TokenDigest bytea(32)`,
`Purpose`, `IssuedAt`, `ExpiresAt`, nullable `ConsumedAt`, `IssuedBy`,
`CreatedByOperationId` and `ConcurrencyVersion`. Raw token storage is forbidden.
Digest and operation ID are globally unique; one active token per user/purpose
is enforced by a partial unique index. Expiry is at most 30 minutes. Consumption
and password/security-version/session-family mutation are one transaction.

### 4.3 `auth_sessions`

Columns: `Id uuid PK`, `FamilyId uuid`, `Generation integer >=0`, `UserId`,
`MembershipId`, `CompanyId`, nullable `BranchId`, nullable `RegisteredDeviceId`,
`DeviceSelector varchar(120)`, `SecurityVersionAtIssue bigint >=1`,
`MembershipVersionAtIssue bigint >=1`, `RefreshTokenDigest bytea(32)`,
`LifecycleState varchar(12)`, `IssuedAt`, `AccessExpiresAt`, `RefreshExpiresAt`,
nullable `ConsumedAt/RevokedAt`, nullable `RevokeReason`, nullable
`PredecessorSessionId`, `CreatedByOperationId`, timestamps and
`ConcurrencyVersion bigint >=1`.

Exact constraints/indexes:

- membership composite FK and branch/company composite FK;
- device FK `(RegisteredDeviceId,CompanyId)` when present;
- unique digest; unique `(FamilyId,Generation)`; unique non-null predecessor;
- unique operation ID; partial unique `(FamilyId)` where `ACTIVE`;
- `IssuedAt < AccessExpiresAt <= RefreshExpiresAt`;
- state/timestamp shape checks for `ACTIVE`, `ROTATED`, `REVOKED`;
- indexes `(FamilyId,Id)`, `(UserId,SecurityVersionAtIssue)`,
  `(MembershipId,LifecycleState,RefreshExpiresAt)`,
  `(CompanyId,BranchId,UserId)` and `(LifecycleState,RefreshExpiresAt)`.

A deferred constraint trigger verifies successor identity/scope/device equality
and generation `n+1`. Rotation is PostgreSQL `SERIALIZABLE`: lock security row,
membership row, family rows ordered by Id and Audit stream head; re-read the
digest; rotate once or revoke the complete family; append Audit V2 and Outbox
in the same caller-owned transaction. `40001/40P01` retries the entire command
with a new DbContext. Named uniqueness conflict becomes reuse/race handling.
Ambiguous commit reconciles by operation ID; if delivery is uncertain the
family is revoked and reauthentication is required.

Access tokens live 10 minutes. Refresh families live at most 14 days. Every
request re-reads active session, security version, membership version/status,
device assignment/proof status and persistent permissions. Claims never widen
those records.

## 5. DBP-003B/C — device registry, assignment and proof

`registered_devices`: `Id`, `CompanyId`, stable random `DeviceSelector`
(`varchar(120)`), `Platform` (`WINDOWS`/`ANDROID`), `PackageId`,
`InstallationIdDigest bytea(32)`, `Status` (`PENDING/ACTIVE/LOST/REVOKED/
REPLACED`), nullable `ReplacedByDeviceId`, `EnrolledAt/ActivatedAt/RevokedAt`,
`ActivatedBy/RevokedBy`, nullable `Reason`, timestamps and concurrency version.
Unique `(CompanyId,DeviceSelector)`, `(CompanyId,InstallationIdDigest)` and
`(Id,CompanyId)`; replacement must be same company and cannot form a cycle.

`device_assignments`: `Id`, `RegisteredDeviceId`, `MembershipId`, `UserId`,
`CompanyId`, nullable `BranchId`, `Status` (`ACTIVE/REVOKED/TRANSFERRED`),
`AssignedAt/AssignedBy`, nullable `RevokedAt/RevokedBy/Reason`, and version.
Composite membership/device/branch FKs; one active assignment per device and
one active `(device,membership)`; transfer is revoke-old plus create-new plus
Audit V2 atomically. Lost/revoke/replacement revokes assignments, proof keys,
sessions and quarantines pending Offline work in one UoW.

`device_proof_keys`: `Id`, `RegisteredDeviceId`, `CompanyId`, `KeyVersion
integer >=1`, `Algorithm='ECDSA_P256_SHA256'`, `PublicKeySpki bytea`,
`PublicKeySha256 bytea(32)`, `Status` (`ACTIVE/RETIRED/REVOKED`), valid-from/to,
actor/reason and version. No private key enters server storage. Unique device/
version and global public-key digest; one active key per device.

`device_replay_nonces`: `RegisteredDeviceId`, `ProofKeyId`, `NonceDigest
bytea(32)`, `RequestJti uuid`, `RequestTimestamp`, `ObservedAt`, `ExpiresAt`,
`OperationId`; primary key `(RegisteredDeviceId,ProofKeyId,NonceDigest)` and
unique `(RegisteredDeviceId,RequestJti)`. The signed request canonical form is
length-framed UTF-8 of method, normalized path/query, company, membership,
session, device, UTC epoch seconds, JTI, nonce and SHA-256 body digest. Allowed
clock skew is five minutes. Verification and nonce insert precede business
mutation in the same transaction; a duplicate fails closed with no effect.

Only the assigned user may use the device. Activation, transfer, revoke,
replacement and recovery require explicit persistent permissions; transfer,
replacement and recovery additionally require maker/checker separation and an
immutable reasoned audit event. There is no admin bypass that skips audit.

## 6. DBP-004 — Audit V2 and caller-owned Unit of Work

### 6.1 Physical objects

`audit_stream_heads`: `Id uuid PK`, `CompanyId`, nullable `BranchId`,
`StreamKey varchar(300)`, `LastSequence bigint >=0`, nullable `LastHashV2
bytea(32)`, timestamps and version. Unique `(CompanyId,StreamKey)` and tenant
FKs/RLS.

Existing `audit_events` gains: `HashVersion smallint NOT NULL DEFAULT 2`,
`CanonicalizerVersion smallint NOT NULL DEFAULT 1`, `StreamHeadId uuid`,
`StreamSequence bigint`, `PreviousHashV2 bytea(32)`, `HashV2 bytea(32)`,
`PayloadDigest bytea(32)`, `OperationId uuid`, and `RetentionClass varchar(30)`.
For the empty Greenfield target all rows are V2; existing V1 columns remain for
reader compatibility. Unique `(StreamHeadId,StreamSequence)`, unique `HashV2`,
unique `(StreamHeadId,OperationId)` and exact 32-byte checks apply.

`integration_outbox`: `Id`, `CompanyId`, nullable `BranchId`, `OperationId`,
`Topic`, `ContractVersion`, `PayloadJson`, `PayloadSha256 bytea(32)`,
`OccurredAt`, `AvailableAt`, `Status`, `AttemptCount`, nullable lease fields,
nullable published/error fields and version. Unique `(CompanyId,OperationId,
Topic)`; statuses and lease shape are checked. Business code inserts it in the
same transaction; publishing never performs business mutation.

### 6.2 Canonicalizer V2

Hash input starts with ASCII `TransportERP-Audit-V2` and one zero byte. Every
field is encoded in fixed order as signed 32-bit big-endian byte length followed
by bytes; `-1` is null and `0` is empty. UUID is 16 RFC-4122 network-order
bytes. Timestamps are UTC Unix epoch microseconds. Integers are signed
big-endian. Text is UTF-8 NFC. JSON is parsed, object keys sorted ordinal by
UTF-8 bytes, arrays retained, numbers emitted as minimal invariant decimal,
and insignificant whitespace removed; invalid JSON is rejected. Field order:
hash/canonicalizer version, stream key/sequence, previous hash, event ID,
occurred time, actor, company, branch, action, outcome, entity type/id,
correlation, operation, device, before JSON, after JSON, reason, IP,
retention class and payload digest. SHA-256 produces `HashV2`.

### 6.3 Append-only and transaction contract

The audit appender receives the caller's DbContext/transaction and performs no
Begin/Commit/SaveChanges. It locks one stream head, increments sequence,
computes the hash, adds the event/outbox and returns; the outer orchestration
performs one SaveChanges and one commit. No nested/autonomous audit UoW exists.

Runtime roles have SELECT/INSERT only. BEFORE UPDATE/DELETE triggers raise
`AUDIT_APPEND_ONLY`; TRUNCATE is revoked; stream sequence/hash/event fields are
immutable. Only the isolated migrator can alter objects. Raw SQL update/delete/
truncate, orphan audit, duplicate sequence and failure-before/after each caller
stage are mandatory negatives.

## 7. DBP-005 — governed Settlement and accounting integrity

`accounting_posting_profiles`: `Id`, `CompanyId`, `Code`, `CurrencyId`,
`RoundingMode` (`TO_EVEN/AWAY_FROM_ZERO`), `MinorUnit`, `FxSourceCode`,
`EffectiveFrom/To`, `Status`, maker/checker, timestamps/version. Unique active
`(CompanyId,Code,EffectiveFrom)`.

`accounting_posting_profile_lines`: `(ProfileId,RoleCode)` PK, `AccountId`,
nullable dimension, `Side` (`DEBIT/CREDIT`), `AmountBasis`, `SortOrder`.
Required roles for Collection Settlement are `CASH_OR_BANK`,
`COLLECTION_CLEARING` and optional `ROUNDING_GAIN/ROUNDING_LOSS`; account and
dimension company consistency is enforced. No account ID, FX rate or rounding
value is hard-coded.

`settlements`: `Id`, `CompanyId`, `BranchId`, `SettlementNo`, `OperationId`,
`PostingProfileId`, `FiscalPeriodId`, `CurrencyId`, `ExchangeRate numeric(19,8)`,
`RoundingMode`, `MinorUnit`, `TotalAmount numeric(19,4)`, `Status`
(`DRAFT/APPROVED/POSTED/REVERSED`), `MakerUserId`, nullable `CheckerUserId`,
nullable `ApprovedAt/PostedAt`, nullable `JournalEntryId`, nullable
`ReversalOfSettlementId`, `Reason`, timestamps/version. Checks require positive
amount/rate, maker != checker for APPROVED/POSTED, open period at posting, and
state/timestamp/link shape. Unique company number, operation ID, journal link,
and non-null reversal target. Tenant composite FKs apply.

`settlement_collections`: `SettlementId`, `CollectionId`, `CompanyId`,
`BranchId`, `AppliedAmount numeric(19,4)`, `CurrencyId`, `SourceExchangeRate`,
`SourcePayloadSha256 bytea(32)`, primary key `(SettlementId,CollectionId)`.
Collection tenant/currency/value is snapshotted and composite-validated. A
partial unique index prevents one accepted collection from belonging to more
than one non-reversal posting.

`journal_entries` gains tenant-consistent composite FKs, `SettlementId`,
`OperationId`, `PostedAt`, `PostedBy` and immutable status enforcement. A
deferred posting constraint trigger requires: at least two lines; sum debit =
sum credit = header totals > 0 at configured scale; every line account belongs
to the company and permits posting; period is OPEN at commit; source type is
SETTLEMENT; exactly one matching Settlement; every linked collection is
accounted once. Posted headers/lines can only be superseded by a separately
linked inverse journal; UPDATE/DELETE of posted history is denied.

The caller-owned serializable UoW locks collections, period/profile, Settlement
idempotency key, journal source key and Audit stream in stable order. It creates
Settlement, voucher, balanced journal/lines, source links, Audit V2 and Outbox
atomically. Reversal creates an inverse Settlement/journal and never edits the
original. Concurrent same operation/source yields one posting. A failure at
every stage leaves none of the objects committed.

## 8. DBP-006 — typed Offline inbox/queue/result/lease

### 8.1 Physical objects

`offline_inbox`: `Id`, `CompanyId`, nullable `BranchId`, `MembershipId`,
`UserId`, `RegisteredDeviceId`, `SessionId`, security/membership/key versions,
`ProtocolVersion smallint`, `ActionCode varchar(100)`, `ClientOperationId uuid`,
`PayloadJson`, `PayloadSha256 bytea(32)`, `FingerprintSha256 bytea(32)`,
nullable `BaseVersion`, `ClientOccurredAt`, `ReceivedAt`, `AuthorityPolicyVersion`,
`RequiredPermissionCode`, `Status`, nullable quarantine/reject fields,
timestamps/version. Unique `(CompanyId,RegisteredDeviceId,ClientOperationId)`;
same key/same fingerprint returns the stored result, same key/different
fingerprint is immutable conflict. Protocol is exactly `1` for this migration.
Unknown action/version, DELETE and online-only actions fail before insert or are
recorded as effect-free rejection according to API contract.

`offline_work_queue`: `InboxId PK/FK`, `Status` (`READY/LEASED/RETRY/
SUCCEEDED/REJECTED/QUARANTINED/DEAD`), `AttemptCount`, `NextAttemptAt`, nullable
`LeaseOwner`, `LeaseToken uuid`, `LeaseAcquiredAt`, `LeaseExpiresAt`,
`LastErrorCode`, version. Lease shape checks and `(Status,NextAttemptAt)` index.
Claim uses `FOR UPDATE SKIP LOCKED`; completion requires matching unexpired
lease token and checked version.

`offline_operation_results`: `InboxId PK/FK`, `ResultCode`, nullable
`ResultVersion`, `ResultJson`, `ResultSha256 bytea(32)`, `CompletedAt`,
`BusinessEntityType/Id`, `AuditEventId`, `OutboxId`. Result, business effect,
audit and outbox commit in one UoW.

`offline_quarantine`: `InboxId PK/FK`, `ReasonCode`, `QuarantinedAt`, actor,
immutable evidence digest and release disposition. Release requires current
membership/session/device/proof/permission reauthorization and reasoned Audit;
revoked security work cannot be resumed under old provenance.

Existing `sync_operations` stays readable but receives no worker execution
grant and is classified `ExecutionEligible=false`. There is no guessed backfill
to typed actions.

### 8.2 Authority and execution

OFFLINE-001 default-deny catalog is a versioned server table/configuration
artifact whose available handlers are only the five currently implemented
operational captures. Settlement, posting, period, permission, security and
device administration remain online-only. The worker re-resolves membership,
session/security version, device assignment/proof and permission immediately
before commit. Any revoke quarantines the operation with no business effect.

Retry is bounded exponential delay with jitter and max 10 attempts. A lost
response converges through the immutable key/fingerprint/result. Two workers,
expired lease, crash-before/after effect, replay, reordered delivery and
same-key/different-payload tests are mandatory.

## 9. Retention, legal hold, cleanup and recovery

`retention_holds` contains `Id`, `CompanyId`, `ResourceType`, nullable
`ResourceId`, `Reason`, `PlacedAt/By`, nullable `ReleasedAt/By`, immutable audit
links. Cleanup queries anti-join active holds and write a cleanup Audit V2 event
plus digest/count report.

Rehearsal defaults (Production activation still requires legal approval):

| Resource | Minimum retained after terminal state |
|---|---:|
| Audit V2, Settlement, journal/source/reversal links | 10 years |
| device registry/assignment/proof-key lifecycle | 365 days |
| terminal session family and refresh digest | 180 days |
| consumed password reset token | 90 days |
| online PoP nonce/JTI | 30 days |
| successful Offline payload/result/outbox | 90 days |
| rejected/conflict/quarantined Offline payload | 365 days |
| Offline idempotency tombstone/fingerprint | 400 days |

Cleanup is partition/batch bounded, uses `SKIP LOCKED`, never deletes an active
row, legal hold or accounting/audit history, and retains idempotency tombstones
after payload minimization. Key rotation retires public keys; it never rewrites
proof/audit history. Revocation is forward state, not deletion.

Greenfield recovery is: immutable schema/custom-format backup after the ten
base migrations; apply one candidate unit; run reconciliation and backup;
restore to a new PostgreSQL 18.6 instance; compare migration history, catalog,
constraints/indexes/policies/row counts and invariant queries. Before activation
a failed candidate may be reverted by dropping the disposable database. After
activation, recovery is forward correction or verified restore; destructive
Down is not relied upon.

## 10. Shared acceptance and stop conditions

Mandatory evidence is defined in
`GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`. Any missing RLS policy, cross-
tenant row, partial UoW, duplicate successor/posting/effect, mutable audit/
posted history, model drift, restore mismatch or unclassified PostgreSQL error
stops the affected proposal. Production access, data, credentials and signing
secrets are prohibited.

## 11. Independent decision request

The proposal-specific design gaps named by the controlling Greenfield re-review
are addressed in this document. MISSION-03 requests one decision per DBP and a
bundle order decision. Until that independent decision is recorded centrally,
the authorized material execution remains `NONE`.
