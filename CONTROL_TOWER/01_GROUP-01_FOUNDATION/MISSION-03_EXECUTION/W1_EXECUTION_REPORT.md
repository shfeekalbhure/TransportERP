# W1 Execution Report — REM-100 Volume

## Disposition

`IMPLEMENTED — READY FOR INDEPENDENT VERIFICATION`

## Package binding

- Remediation: `REM-100`.
- Finding: `A-ARCH-002` (`M02-BLK-002`).
- Authoritative Product baseline: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Exact execution SHA before: `a48b68023072122c3f71941b861d8b9eeca82d34`.
- Exact execution SHA after: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- After tree: `561d5862916c76432aa845d20ca85809e4430fde`.
- Branch: `codex/mission-03-execution-20260828`.
- PR #69: evidence only; no merge/cherry-pick/copy. Its mapper was reverified as byte-identical to the defective baseline.

## Revalidation and reason

Before modification, `ConcurrencySafeWaybillRepository.SaveAsync` deleted and reinserted Waybill item snapshots. Its private `ToItemEntity` copied Weight/Length/Width/Height and other fields but omitted `Volume`, while the entity, aggregate, contract, migration and reload mapper all carried the field. Consequently an explicit Volume survived the in-memory update response but was persisted as NULL.

## Change implemented

Smallest code fix: add `Volume = x.Volume` to `ToItemEntity`. One PostgreSQL integration test creates a draft, updates an item with explicit `42.125m` Volume, persists, reloads through a fresh repository/DbContext, and verifies both aggregate and entity values.

Files changed:

- `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs`.
- `TransportERP.Tests/P2C01AWaybillPostgreSqlIntegrationTests.cs`.

No entity, schema, migration, precision, relationship, index, constraint, seed, numbering or data change was made.

## Preservation and impact

- DB-GOV: DBP-001 code-only path used; data assessment/repair remains blocked.
- API/contracts: unchanged.
- Security/tenant/device: unchanged.
- Accounting/audit: unchanged; full regression passed.
- Offline/Sync: unchanged; full regression passed.
- Desktop/Mobile: source unchanged; exact-head build/probes passed.
- Shipping/allocation: existing explicit-volume and split-allocation tests passed, including PostgreSQL end-to-end split preservation.

## Verification

Run `33181376288` at exact after SHA completed successfully on disposable Ubuntu/Windows environments with PostgreSQL 18.6 and .NET SDK 10.0.400.

- Complete suite: `Passed 125; Failed 0; Skipped 0`.
- New test `Create_update_persist_reload_preserves_explicit_item_volume`: PASS.
- `Split_item_across_two_trips_preserves_total_weight_and_volume`: PASS.
- Existing explicit/split allocation measure tests: PASS.
- model-drift and all 10 committed migrations on empty disposable DB: PASS.
- API boot/HTTP boundary and client build/probes: PASS.
- Linux artifact `9689871882`, SHA-256 `a68e0948b91181d3403acbc55b519b8888c89fbd659f2f622dc4b0e846c346fa`.

## Rollback and residual risk

Rollback is a normal revert of `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`; no DB/data rollback is needed. Production/live affected rows were not accessed, counted or repaired, so historical rows that may already contain NULL Volume remain unknown and any data action remains blocked under DB-GOV-001. Final judgment belongs to MISSION-04.
