# OFFLINE-001 — Per-Action Offline Authority

Decision date: 2026-08-28
Owner decision: `APPROVED — RESOLVED`
Owner approval record: `EXPLICIT OWNER APPROVAL — 2026-08-28 — "اعتمد"`

## Decision

`OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`

The owner approves a fail-closed Offline policy. Every action is classified by this decision; anything not explicitly in an allowed class remains `OFFLINE_WRITE=0`.

## Offline write = ALLOW TO QUEUE

The client may queue only these classes, and server acceptance remains authoritative at replay:

1. Draft creation/update of operational documents where the action does not approve, post, settle, close a period, grant authority or allocate an irreversible official number.
2. Append-only operational events and quantity/custody movements that have a stable `ClientOperationId`, immutable provenance and server conflict validation.
3. Operational shipment/trip/manifest/arrival/delivery capture when the action is reversible or replay-safe under the governing state machine.
4. Collection capture as an operational `PENDING` client command only. Offline capture never posts accounting. On replay the server revalidates tenant, permission, device/session, idempotency, amount/currency and current state before accepting the collection.
5. Locally cached attachments/photographic evidence may be staged for later upload only when the governing attachment policy permits it; sensitive material must remain protected at rest and is not considered server-accepted until acknowledged.

## Offline write = DENY / ONLINE AUTHORITATIVE

The following are always online-authoritative unless a later owner decision explicitly supersedes this file:

- identity/login authority changes, user/role/permission/membership administration;
- device enrollment approval, assignment, transfer, revoke/recovery and trust/PoP administration;
- voucher/journal posting, settlement posting, accounting reversal, unpost, period close/reopen and account mapping;
- approval/finalization actions whose acceptance creates irreversible accounting/security/legal authority;
- destructive delete or history rewrite;
- master-data changes that alter security, accounting, numbering or tenant boundaries;
- any action lacking idempotency identity, version/precondition, server permission code or deterministic conflict owner.

## Mandatory queue envelope

Every allowed queued mutation must carry/bind:

- client operation ID and protocol version;
- entity/action identity and payload hash;
- expected/base version when the aggregate is mutable;
- user, company, branch and device/session provenance;
- client occurred-at timestamp plus server received-at timestamp;
- permission/action code;
- no client-authoritative tenant/role/permission/device-trust claim;
- bounded retention metadata and conflict ownership.

Replay must re-authorize against current server state. A revoked/expired user, membership, session or device causes fail-closed denial and freezes protected outbound submission. The client must never manufacture authority while offline.

## Conflict policy

- No silent merge for accounting, custody, quantity, approval or security state.
- Idempotent replay of the same accepted operation returns the existing outcome.
- Version conflict returns a governed conflict/reload outcome.
- Compensating/reversal operations are new auditable operations; they do not rewrite accepted history.

## Read-only Offline

Cached read access is permitted where the data classification allows it. Clients must display an as-of/last-sync state and must not present cached authorization or financial status as current server truth. Sensitive cached data requires platform-secure storage and retention controls.

## Per-action materialization rule

MISSION-03 is authorized to map every registered action in the canonical W2/action registers and Product code to this policy without another owner round-trip:

- if it matches an ALLOW class and all envelope requirements are satisfied: `OFFLINE_QUEUE = ALLOW`;
- otherwise: `OFFLINE_QUEUE = DENY`.

An action that is genuinely ambiguous after applying these rules is isolated and remains DENY until separately governed; ambiguity must not block unrelated actions.

The existing FLOW01 `OFFLINE_WRITE=0` remains valid until MISSION-03 explicitly reissues a specific action under this policy and verifies its queue/replay contract.

## Implementation boundary

This decision resolves owner authority only. DBP-006 and any persistence/schema work remain under DB-GOV. No Production or database mutation is authorized by this file.
