# TransportERP — P2-C01 Offline / Sync Policy

**Release:** `P2-C01-WAYBILL-SHIPPING-2026-08`  
**Baseline dependency:** P1 SyncOperation / ConflictCase / ClientOperationId contracts on `master`  
**Status:** `READY_FOR_REVIEW`

## 1. Governing rule

No offline client is authoritative for official numbering, financial close, financial reopen, or any operation that creates an irreversible server-side accounting effect. Offline work creates local drafts or queued operations only. Final state exists only after server validation and acknowledgement.

## 2. Operation classes

| Class | Examples | Offline behavior | Server result |
|---|---|---|---|
| Draft-local | CreateWaybillDraft; UpdateWaybillDraft | Allowed | Sync creates/updates server draft; no official number |
| Capture-and-queue | Load; Arrival; Unload; POD; Collection by authorized field user; Exception | Allowed when role/device policy permits | Server revalidates scope; quantity; idempotency; state; then Accept/Reject/Conflict |
| Online-authoritative | Submit; Approve; official numbering; Hold/Release; Redirect; Trip settlement; Financial close/reopen | Not allowed as final offline operation | Must execute online |
| Read-cache | Party lookups; basic waybill read | Allowed using bounded cache | Cache is never proof of current authorization or final state |

## 3. Idempotency

Every queueable write must carry a stable `ClientOperationId`. Retrying the same logical operation must not create a second collection, movement event, delivery, release, allocation, or number reservation.

Server behavior:

1. locate prior operation by company + device/user policy + ClientOperationId;
2. if payload hash matches, return prior accepted/rejected outcome;
3. if identifier matches but payload differs, reject with `IDEMPOTENCY_CONFLICT`;
4. audit all retry outcomes.

## 4. Concurrency

Draft and mutable aggregate writes carry `BaseVersion` / ExpectedVersion. A stale operation becomes `CONFLICT` instead of silently overwriting server state. Quantity-ledger operations are additionally serialized at server transaction level.

## 5. Clock and event time

Clients send `ClientOccurredAt`; server records `ServerReceivedAt`. Operational ordering uses server acceptance plus domain OccurredAt policy. Client clocks never control official numbering or accounting period selection.

## 6. Field collections

A collection captured by driver/agent while offline is not financially final merely because the device shows it. It remains queued/pending until server acceptance. Once accepted it is immutable and becomes part of the collector accountability until settlement/remittance.

## 7. POD

Photo/signature/identity capture may occur offline where device policy permits. The client must preserve content hash and local correlation. Delivery reaches final server state only after proof upload/sync and server acknowledgement.

## 8. Movement events

Movement events are append-only. Offline retries must deduplicate by ClientOperationId. A correction is a new reversal/correction event and never an update/delete of an accepted movement.

## 9. Conflict authority

- Draft text/nonfinancial conflicts: designated clerk may reapply after reviewing server state.
- Quantity conflicts: operations supervisor or role configured by policy.
- Collection conflicts: finance/cashier authority.
- Delivery/POD conflicts: delivery supervisor.
- Numbering/approval/financial close: no offline conflict resolution; repeat online server command.

## 10. Device and scope controls

Every queued operation must carry registered DeviceId, UserId, CompanyId, BranchId, EntityId, operation type, payload hash, client time, and base version when applicable. Server re-evaluates current permission and scope on receipt; prior offline permission does not guarantee acceptance.

## 11. Retention

Successfully synchronized local sensitive payloads must follow device retention policy. Identity images and POD artifacts are removed from local storage after verified upload and configured grace period unless a legal/operational retention rule says otherwise.

## 12. Acceptance references

This policy is validated by UAT-P2C01-003, UAT-P2C01-021, UAT-P2C01-030, UAT-P2C01-031 and the W2 action Offline_Policy column.
