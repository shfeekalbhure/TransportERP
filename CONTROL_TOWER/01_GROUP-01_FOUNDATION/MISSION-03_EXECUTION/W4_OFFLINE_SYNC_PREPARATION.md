# W4 — Offline/Sync Preparation and Entry Revalidation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `PREPARATION COMPLETE — PRODUCT ENTRY NOT SATISFIED`
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
- `DEP-011` has no accepted per-action Offline authority matrix.
- `DBP-006` has no rehearsal or execution authority.

The current signed repository authority for FLOW01 is
`ONLINE_ONLY / OFFLINE_WRITE=0 / Can Queue=NO`. The P1 Sync contract remains
`READY_FOR_OWNER_ACCEPTANCE`, not accepted. PR #69's five available actions are
evidence only and cannot widen authority.

## Prepared package split

| Package | Prepared boundary |
|---|---|
| `W4-P01` | default-closed intake/worker design; unknown version/action and DELETE deny with no side effect |
| `W4-P02` | typed request/result/error/version contracts and strict payload/hash/size validation; executable catalog empty |
| `W4-P03` | server-side action-authority, dispatcher and worker-reauthorization ports; missing authority fails closed |
| `W4-P04 / DBP-006` | split into protocol compatibility, inbox/claim/lease/outbox, device/PoP, and retention/legal-hold proposals |
| `W4-P05` | encrypted client outbox/restart/quarantine/credential-separation design; local schema separately governed |
| `W4-R01/R02` | runtime activation waits for DEP-011, W2/W3 and DBP-002/003/006 |

No Product implementation is started because the sealed W4 dependency entry is
not met. The planned fail-closed packages do not authorize an action or worker.

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
