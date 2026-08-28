# W1 Preflight Report — REM-100 Volume Integrity

## State

`PREPARED — IMPLEMENTATION BLOCKED BY W0 AND DBP-001 ENTRY GATES`

## Exact-source revalidation

- Baseline: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Actual runtime repository registered by API: `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs`.
- Update behavior at lines 76–87 deletes and reinserts all parties/items.
- `ToItemEntity` at lines 119–137 copies quantity, pieces, weight, length, width and height but does not assign `Volume`.
- The entity, contract, aggregate, application mapping, EF model, migration and model snapshot all contain `Volume`.
- PR69's exact copy of `ConcurrencySafeWaybillRepository.cs` is byte-identical to master for this file; it does not fix REM-100.

## Existing-test gap

The PostgreSQL update round trip in `P2C01AWaybillPostgreSqlIntegrationTests.cs` supplies no explicit `Volume` and contains no post-update/reload Volume assertion. Physical-measure tests verify allocation math on already persisted data, not preservation through the delete/reinsert update mapper.

## Authorized implementation unit when gates close

- REM: `REM-100`.
- Findings: `A-ARCH-002`, `M02-BLK-002`.
- Code candidate: add the missing `Volume = x.Volume` mapping only.
- Tests required by `T-100`: explicit non-null and null create → update → reload → allocation; concurrency/idempotency and field-parity regression.
- DB action: none for the mapper code. Affected-row assessment or repair remains a separately authorized DBP-001 action and must not be bundled with the code fix.
- Rollback: isolated code/test commit revert; no migration and no data mutation.

## Stop decision

No code or test file was changed. The W1 Entry Criteria explicitly require W0 executable baseline and reviewed DBP-001. Both are currently unmet, so starting the implementation would bypass the sealed dependency gate.
