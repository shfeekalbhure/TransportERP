# W4 — Offline/Sync Preparation and Entry Revalidation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `DEP-011 RESOLVED BY OFFLINE-001 — CODE-ONLY CONTAINMENT READY; RUNTIME/DB ENTRY NOT SATISFIED`
- Product/DB mutation: `NONE`

## Current reality

The server can persist generic Sync operations and mutate their queue/conflict
lifecycle. W2 now binds existing mutations to stored Company/Branch/User/Device
and persistent RBAC. There is no typed business dispatcher, worker, client
outbox, registered-device/PoP authority or atomic business/audit/outbox path.
`ProtocolVersion` is required text but not an allowlisted negotiation, and
generic `CREATE/UPDATE/DELETE/COMMAND` types remain accepted.

## Entry decision

`W4 ENTRY GATE = NOT SATISFIED`

- W2 tenant/session/device persistence is incomplete.
- W3 transaction/accounting/audit atomicity is incomplete.
- `DEP-011` is resolved by OFFLINE-001, but W2/W3 durable authority and atomic
  completion remain incomplete.
- `DBP-006` has no rehearsal or execution authority.

OFFLINE-001 supersedes the unresolved classification question with default deny.
The effective 44-action P2 register classifies 11 queue candidates, one
read-cache action and 32 online-authoritative actions. Current Product handlers
exist for only five queue candidates: `CreateWaybillDraft`,
`UpdateWaybillDraft`, `CreateOperationalParty`, `RecordCollection`, and
`LoadAllocatedQuantity`. Attachment/arrival/unload/delivery/POD/exception remain
authorized in principle but unavailable until their Product contracts exist.
Accounting/security/permission/device administration/posting/settlement/period
operations remain online-authoritative.

## Prepared package split

| Package | Prepared boundary |
|---|---|
| `W4-P01` | default-closed intake/worker design; unknown version/action and DELETE deny with no side effect |
| `W4-P02` | typed request/result/error/version contracts and strict payload/hash/size validation; catalog has five AVAILABLE and six AUTHORIZED-BUT-UNAVAILABLE operations |
| `W4-P03` | server-side action-authority, dispatcher and worker-reauthorization ports; missing authority fails closed |
| `W4-P04 / DBP-006` | split into protocol compatibility, inbox/claim/lease/outbox, device/PoP, and retention/legal-hold proposals |
| `W4-P05` | encrypted client outbox/restart/quarantine/credential-separation design; local schema separately governed |
| `W4-R01/R02` | runtime activation waits for DEP-011, W2/W3 and DBP-002/003/006 |

Runtime activation remains prohibited. PR #69 accounting-draft queue entries
are rejected; catalog/dispatcher patterns may only be selectively
reimplemented and exact-head tested after the code-only package gate.

## DBP-006 boundary

Entity/model/migration work for ActionCode, protocol, fingerprint, claim/lease,
registered device, nonce/replay, inbox/outbox, retention or local SQLite remains
prohibited. A rehearsal requires exact lineage, named safe copy, backup/restore,
role/RLS inventory, pre/post reconciliation, failure injection, compatible
reader recovery and an independently approved per-action authority contract.

## Required authority evidence

An accepted matrix must bind each action code to payload/version, permission,
tenant/device/session scope, Offline availability, conflict owner, accounting
effect, retention and client surfaces. Until then the executable set is empty.

## Test and recovery design

Required negatives include disabled gate, version/action/DELETE denial without
DB/audit effects; A↔B/user/device mismatch; revoked authority between enqueue,
claim and commit; two-worker/lease/restart/lost-response races; duplicate key
with same/different hash; PoP/nonce replay; all-or-nothing business/audit/outbox;
client-store tamper/restart; and revoke-driven queue quarantine.

Recovery disables intake/worker, preserves and quarantines payload/provenance,
keeps a compatible reader/export path, and uses forward correction or verified
safe-copy restore. No queue/audit deletion or destructive downgrade is allowed.
