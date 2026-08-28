# DBP-002/003/004/005/006 — Independent Greenfield DB-GOV Review Decision

Review authority: `CONTROL TOWER / DB-GOV-001`
Decision date: `2026-08-28`
Owner database authority: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / DATA`
Execution baseline reviewed: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
Execution tree: `00512125311306a43474638195d2cad97b76118e`
Current governed lineage: `10 existing migrations`
Production authorization: `NONE`

## Controlling decision

`DBP-002/003/004/005/006 = APPROVED FOR COORDINATED DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`

This decision opens candidate Entity/DbContext/Migration/persistent-adapter authoring **only on the isolated MISSION-03 execution branch** and application **only to disposable/new non-Production PostgreSQL 18.6 rehearsal databases**.

It does not authorize Production, master merge, live credentials, real customer/user/accounting data, destructive migration, legacy backfill, or final DB acceptance.

## Greenfield basis

The target database has no pre-existing tables/data/users/password hashes/accounting rows/audit rows. The repository's ten committed migrations remain the immutable bootstrap lineage for rehearsal. Therefore legacy backfill, legacy PasswordHash discovery, legacy audit/accounting data reconciliation and copying an existing target database are not rehearsal prerequisites.

The rehearsal sequence is:

1. create an empty PostgreSQL 18.6 database;
2. apply the current ten migrations unchanged;
3. capture exact migration/catalog baseline and backup digest;
4. restore baseline to a second fresh disposable database;
5. apply only the authorized candidate migrations in the ordered package sequence below;
6. run proposal-specific negative/concurrency/failure tests;
7. run full existing regression and EF model-drift checks;
8. backup/restore the candidate state and reconcile migration/catalog invariants;
9. dispose rehearsal databases and retain evidence.

## DBP-002 — tenant memberships and physical consistency

Decision: `APPROVED FOR GREENFIELD REHEARSAL`.

Allowed candidate scope:

- additive explicit membership persistence implementing ADR-W2-001: User -> 0..N active memberships; each membership binds exactly one Company and optional explicit company-wide Branch scope;
- tenant-consistent Branch/Company composite FKs and checks;
- membership-scoped grants or explicit scope tuples that cannot infer authority from null;
- indexes/uniqueness required for deterministic membership selection;
- raw-SQL constraints proving wrong-company branch and cross-tenant relationships fail;
- reviewed RLS/equivalent policies may be rehearsed, but application authorization remains mandatory.

Greenfield-specific rule: no legacy membership backfill/dual-read migration is required for target deployment because the target has no user rows. Existing `User.CompanyId/BranchId` columns may be retained temporarily for code compatibility during rehearsal, but they are not the target authority and must not be treated as null-as-wildcard.

Prohibited: destructive drop/rename of existing columns in this rehearsal package; inferred platform access; synthetic Production roles/grants.

## DBP-004 — audit integrity and atomic enlistment

Decision: `APPROVED FOR GREENFIELD REHEARSAL`.

Allowed candidate scope:

- caller-owned transaction enlistment for audit append;
- additive `HashVersion` plus deterministic stream sequence/ordering control;
- a V2 canonicalizer that covers every persisted semantic audit field with explicit field order/null/length/time/number encoding;
- append-only database controls and raw-SQL UPDATE/DELETE denial;
- atomic business/session/settlement + audit tests and failure injection.

Because the target is empty, no Production V1 audit rows require migration or rehash. Existing V1 code/tests remain regression evidence only. Candidate deployment must not rewrite any synthetic pre-candidate rows; migration design must be forward-only.

## DBP-003A — session/security-version persistence

Decision: `APPROVED FOR GREENFIELD REHEARSAL`.

The physical design in `DBP-003A_REHEARSAL_RESUBMISSION.md` is accepted for candidate authoring/rehearsal, including:

- `user_security_state`;
- `auth_sessions`;
- one-successor invariant;
- unique refresh digest and generation/family constraints;
- serializable lock/re-read rotation;
- family revoke on reuse;
- atomic audit enlistment through the DBP-004-compatible transaction boundary;
- retry/ambiguous-commit/failure-injection behavior.

Greenfield removes the prior legacy PasswordHash evidence blocker. However:

`LOGIN ACTIVATION = BLOCKED UNTIL NEW-SYSTEM PASSWORD HASH / VERIFY / LOCKOUT POLICY IS DEFINED AND TESTED`.

The session persistence schema and refresh-family rehearsal may use synthetic identities without activating a Production login endpoint. No raw refresh token/signing secret/private key may be persisted or logged.

## DBP-003B/C — device registry / assignment / PoP / nonce / replay

Decision: `APPROVED FOR COORDINATED GREENFIELD REHEARSAL AFTER DBP-002 + DBP-006 CANDIDATE BASELINE PASSES`.

Allowed scope:

- registered device logical identity and status;
- membership-bound device assignment;
- public-key/proof metadata only, never private key material;
- nonce/JTI/replay uniqueness and revocation/quarantine state;
- synthetic key/proof fixtures for rehearsal;
- revoke/replay/two-device/cross-tenant negative tests.

Production MDM/attestation/key-custody evidence remains a Production-readiness gate, not a Greenfield schema-rehearsal blocker.

## DBP-006 — typed Offline/Sync persistence

Decision: `APPROVED FOR GREENFIELD REHEARSAL`.

Allowed candidate scope:

- typed `ActionCode` and protocol/version fields;
- immutable operation fingerprint and company/device/client-operation uniqueness;
- user/company/branch/session/security-version/registered-device provenance;
- claim/lease/attempt/result/inbox/outbox states with deterministic restart recovery;
- nonce/replay linkage to DBP-003C;
- fail-closed quarantine after revoke;
- retention fields must be configurable; no unapproved Production retention duration may be hard-coded;
- only OFFLINE-001 allow-class actions may become execution-eligible; all other actions remain deny/online-authoritative.

Greenfield means no generic legacy SyncOperation rows require migration/backfill. Existing schema remains bootstrap history; candidate migration may introduce the typed target without inventing provenance for nonexistent rows.

## DBP-005 — governed Settlement/accounting integrity

Decision: `APPROVED FOR GREENFIELD REHEARSAL`.

Allowed candidate scope under ACC-001:

- immutable Settlement header/source collection links;
- atomic voucher + balanced journal + source link + audit + outbox Unit of Work;
- idempotency/source uniqueness and reversal lineage;
- maker/checker identities and open-period checks;
- configured account-role references and captured FX/rounding snapshots;
- database-level balance/posted-history protections;
- cross-tenant and closed-period negative tests.

Rehearsal must use synthetic configuration values and test Chart-of-Account/currency/period fixtures. No Production account IDs, balances or accounting data are required or permitted. Account-role identifiers must remain configuration-driven and never hard-coded.

## Ordered candidate sequence

To avoid circular or stale assumptions, MISSION-03 must author/test in this order, with independent evidence at each checkpoint:

1. `DBP-002` membership/tenant physical core.
2. `DBP-004` transaction-aware audit/V2 core.
3. `DBP-003A` session/security-version persistence.
4. `DBP-006` typed Offline/Sync core.
5. `DBP-003B/C` device/PoP/nonce/replay on the passed 002+006 basis.
6. `DBP-005` governed Settlement/accounting persistence on the passed 002+004 basis.

A failure in one package stops that package and its dependents only; independent packages may continue.

## Candidate migration rules

- Existing ten migrations are immutable: no edit/delete/rename/squash.
- New migrations are additive/forward-only for rehearsal.
- No Production data, credentials, signing keys, peppers or device private keys.
- No Seed that creates real accounts/users/roles/tokens/devices. Test-only synthetic fixtures belong in tests/rehearsal setup, not Product Production seed.
- Every candidate migration must bind exact parent SHA/tree, migration name/hash, EF snapshot diff, generated SQL and changed files.
- `dotnet ef migrations has-pending-model-changes` must be clean after candidate generation.
- Full regression must run on exact candidate head.
- Candidate backup/restore and catalog/migration reconciliation must pass before package exit.
- No down migration is relied upon for recovery; rehearsal recovery is fresh baseline restore or forward correction.

## Rehearsal exit criteria

Each package may advance only if:

- current ten-migration baseline applies cleanly to empty PostgreSQL 18.6;
- candidate migration applies cleanly after that lineage;
- no unintended schema/model delta exists;
- proposal-specific concurrency/security/tenant/accounting/audit/replay negatives pass;
- existing regression remains green;
- exact evidence/artifact digests are retained;
- backup/restore of the candidate state succeeds;
- no secret/PII/Production connection appears in artifacts;
- an independent DB-GOV post-rehearsal review accepts the evidence.

## Production boundary

This decision is **not** Production approval. After successful rehearsal, DB-GOV must separately decide whether each proposal is fit for the actual Greenfield Production bootstrap/release candidate. Production signing, KMS, recovery/RPO/RTO, privacy/retention and executable-client gates remain separate.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`. MISSION-04 remains `WAITING`.
