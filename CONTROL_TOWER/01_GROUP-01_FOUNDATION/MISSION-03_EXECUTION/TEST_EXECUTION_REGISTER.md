# Test Execution Register

## Current worker probes — 2026-08-28T14:07Z

Environment: Linux x86_64, Git 2.51.1. No `.NET SDK`, Docker/Podman, `psql`, `pg_isready`, MSBuild, PowerShell or Android SDK was available.

| Test ID / probe | SHA | Command | DB mode | Exit/result | Classification |
|---|---|---|---|---|---|
| `T-000/info` | `2ec6cccf...` | `dotnet --info` | none | 127 | `BLOCKED` |
| `T-000/restore` | `2ec6cccf...` | `dotnet restore TransportERP.slnx --locked-mode` | none | 127 | `BLOCKED` |
| `T-000/build` | `2ec6cccf...` | `dotnet build TransportERP.slnx --no-restore -c Release` | none | 127 | `BLOCKED` |
| `T-000/discovery` | `2ec6cccf...` | `dotnet test ... --list-tests` | none | 127 | `BLOCKED` |
| `T-000/tests` | `2ec6cccf...` | `dotnet test ... --no-build -c Release` | none | 127 | `BLOCKED` |
| `T-000/EF` | `2ec6cccf...` | `dotnet ef migrations list ... --no-build` | none | 127 | `BLOCKED` |
| `T-000/API boot` | `2ec6cccf...` | `dotnet run --project TransportERP.Api ... --no-build` | none | 127 | `BLOCKED` |
| `T-000/Desktop` | `2ec6cccf...` | `dotnet build TransportERP.Desktop ...` | none | 127 | `BLOCKED` |
| `T-000/Mobile x3` | `2ec6cccf...` | project build probes | none | 127 | `BLOCKED` |

## Exact-SHA historical CI evidence

- Run: `https://github.com/shfeekalbhure/TransportERP/actions/runs/32867082533`
- Event/time: master push, `2026-08-25T15:38:47Z–15:40:24Z`
- Exact head/tree: `2ec6cccf...` / `516247dd...`
- SDK: .NET SDK `10.0.400`; Desktop runner Windows; core runner Ubuntu; PostgreSQL `18.6`.
- Core job: restore/build/contracts/pending-model/migrations/tests all reported success.
- Applied migrations: all 10 current implementations.
- Tests: `Passed 124; Failed 0; Skipped 0; Total 124`.
- Desktop: Library-mode Release build succeeded with one `CS8602` warning.
- Retained artifacts: none.
- Not covered: API boot, executable Desktop host, Mobile targets, current-worker reproducibility.

Historical exact-SHA success is retained as evidence but does not close T-000's missing boot/client/artifact requirements.

## Disposable W0 execution — run 33181045881

- Exact SHA/tree/parent: `a48b68023072122c3f71941b861d8b9eeca82d34` / `638a4f331e03150fcb9aebf61fbbb4af9f930401` / authoritative master `2ec6cccf...`.
- Environment: GitHub-hosted Ubuntu 24.04 and Windows; .NET SDK 10.0.400; PostgreSQL 18.6 container; synthetic credentials/data only.
- Linux artifact: `9689746319`, SHA-256 `fdc6933d16037b34decfc791ed0373a2125ac0c0ecc7c34f310f4a0fbe9b7527`.
- Desktop artifact: `9689710882`, SHA-256 `c09c6e2020c1d3079fc5ff198a8d2da019f199784f34066944cc7b6cfd52a5ef`.

| Test ID / probe | Command family | DB mode | Result |
|---|---|---|---|
| `T-000/sha` | checkout + `git rev-parse` exact binding | none | `PASS` |
| `T-000/runtime` | `dotnet --info`, Docker runtime, `pg_isready` | disposable PostgreSQL 18.6 | `PASS` |
| `T-000/restore` | tests, API, Mobile x3, Desktop restores | none | `PASS` |
| `T-000/build` | tests/API/Mobile x3 Release; Desktop Windows Release | none | `PASS` |
| `T-000/migrations` | list, pending-model check, database update | empty disposable DB | `PASS — 10/10 APPLIED` |
| `T-000/tests` | complete `dotnet test` with TRX | disposable DB | `PASS — 124/124; 0 failed/skipped` |
| `T-000/API boot` | built API process + protected HTTP probe | disposable DB | `PASS — listening; HTTP 401 expected` |
| `T-000/Desktop` | actual property probe + Windows build | none | `PASS — Library; entry point absent` |
| `T-000/Mobile x3` | actual property probes + builds | none | `PASS — Library; MAUI runtime not ready` |

## REM-100 exact-head verification — run 33181376288

- Exact SHA/tree/parent: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a` / `561d5862916c76432aa845d20ca85809e4430fde` / `a48b680...`.
- Same disposable matrix and command families as W0.
- Linux artifact: `9689871882`, SHA-256 `a68e0948b91181d3403acbc55b519b8888c89fbd659f2f622dc4b0e846c346fa`.
- Desktop artifact: `9689839296`, SHA-256 `fa31b2f8e18fe6c32b3d1bcd4e6baa0272610ad5b9356c270bfacb4791e745fb`.

| Test ID | Evidence | Result |
|---|---|---|
| `T-100/round-trip` | `Create_update_persist_reload_preserves_explicit_item_volume` | `PASS` |
| `T-100/PostgreSQL allocation` | `Split_item_across_two_trips_preserves_total_weight_and_volume` | `PASS` |
| `T-100/domain allocation` | explicit Volume authoritative and split totals tests | `PASS` |
| `T-100/regression` | complete suite | `PASS — 125/125; 0 failed/skipped` |
| `T-100/migrations` | model drift + 10 existing migrations | `PASS` |
| `T-100/API/clients` | API boot/HTTP plus Desktop/Mobile probes | `PASS` |

## W2 code-only exact-head verification — run 33183870737

- Exact SHA/tree/parent: `04a875a2973c1ed0f3c05457707e1c7eec7b2823` / `a134646ce714dcebf976e3f4cc532d8c0055e4e6` / security commit `a157c34d6767deeb5544adf456a2a36946a599a9`.
- Security commit parent: W1 head `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- Environment: GitHub-hosted Ubuntu/Windows; .NET SDK 10.0.400; disposable PostgreSQL 18.6; synthetic test data only.
- Linux artifact: `9690897815`, SHA-256 `5226683ec74bda978fd98eddf93fb6776ae77a15db00371ea219799819339635`.
- Desktop artifact: `9690854262`, SHA-256 `8c61095c410f7a8554264a1d4412728cc1905782fd174834d68c14ffe57b871c`.

| Test ID | Exact-head evidence | Result |
|---|---|---|
| `T-210/Sync membership` | HTTP cross-company claim vs stored user; branch mismatch service negative | `PASS` |
| `T-200/persistent RBAC` | claim-only without DB grant denied; explicit DB deny after grant denied; valid persistent grant retained | `PASS` |
| `T-220/lifecycle owner` | different user/device denied for transition, retry, conflict create/resolve and foreign replacement | `PASS` |
| `T-220/pending retries` | query now predicates Company + User + Device + optional Branch | `PASS BY FULL REGRESSION; dedicated expansion remains in F2` |
| `T-W2/regression` | `dotnet test TransportERP.Tests/... --configuration Release --no-build` | `PASS — 128/128; 0 failed/skipped` |
| `T-W2/migrations` | EF list, pending-model check, database update | `PASS — existing 10/10; no pending model change` |
| `T-W2/API/clients` | API protected boundary HTTP 401; Desktop Windows and Mobile x3 builds/probes | `PASS` |

Warnings were non-blocking existing runner/nullable warnings; no W2 test or build failure was hidden.

## W2 A2/B2A first attempt — run 33184771338

- Exact SHA/tree/parent: `d1c0a2571bf3d240b9134e8614186acd70a6bd5d` / `59ac61e5e22a7bb8466575aa1f64530b2ef65581` / `04a875a...`.
- Core job `98894801318`: `FAIL` in `Build core runtime, tests, API, and Mobile probes` with CS0246 at `CurrentRequestSecurity.cs`; `OperationContext` was imported from the wrong namespace.
- Desktop job `98894801198`: `PASS`.
- Tests/migration/API: `SKIPPED/BLOCKED BY BUILD`; no PASS was claimed.
- DB mode: disposable PostgreSQL 18.6 container; the build failed before migration application or test Product mutations and the container was removed.
- Recovery: no history rewrite or force push. The one-line import correction was committed at `d740740...` and the full matrix was rerun. No partial DB/data state remained.

`EXECUTION FAILED — CORRECTED AND REVERIFIED`; the failed run remains retained evidence.

## W2 A2/B2A corrected exact-head verification — run 33184994576

- Exact SHA/tree/parent: `d74074045491ed2259c4ed3f411f84b0bd82356a` / `071d4adf9a5c96be65ad7fe1681db1906d8ccf94` / `d1c0a257...`.
- Environment: GitHub-hosted Ubuntu 24.04 and Windows; .NET SDK 10.0.400; disposable PostgreSQL 18.6; synthetic test data only.
- Core job `98895572695`: `PASS`; Desktop job `98895572296`: `PASS`.
- Linux artifact: `9691350327`, SHA-256 `dddbdbbf301816fc55f411d3f6a62ff68810c4bff2abdb1b1b110f2e984a8a47`.
- Desktop artifact: `9691310607`, SHA-256 `d66ed26704c459e8b3f688ff0dd582e57dd944b10dbb80cb95ad068e0744db62`.

| Test ID | Exact-head evidence | Result |
|---|---|---|
| `T-210/API stored scope` | all three Product API modules use one resolver; foreign active-user/branch claims are denied before Product service execution | `PASS` |
| `T-200/API persistent RBAC` | claim-only user without persistent branch grant denied; valid token hint plus grant allowed | `PASS` |
| `T-W2/regression` | complete Release test suite against PostgreSQL | `PASS — 128/128; 0 failed/skipped` |
| `T-W2/migrations` | EF list, pending-model check, database update | `PASS — existing 10/10; no pending model change` |
| `T-W2/API boot` | built API protected-boundary probe | `PASS — HTTP 401 expected` |
| `T-W2/clients` | Desktop Windows plus Mobile Admin/Customer/Driver builds/probes | `PASS` |

The run emitted existing analyzer/runner warnings (xUnit2031, Desktop nullable and Node action deprecation); none was a failed gate. No Entity, DbContext, Migration, Seed, Schema or data change was present.

## W2 final cross-company exact-head verification — run 33185419917

- Exact SHA/tree/parent: `9c5b7a12e59d2c42e682717b8e90c491f8699b96` / `452b37f1e2c68d9f3dae6e18f1cf1b67645105af` / `d740740...`.
- Delta: one explicit PostgreSQL API assertion that user A carrying company/branch B claims receives HTTP 403; no Product or DB model change.
- Core job `98897056951`: `PASS`; Desktop job `98897057221`: `PASS`.
- Linux artifact: `9691527827`, SHA-256 `d24109795a2c4f9aff1d82465d7178f2f4eba410b8bd68f86edc504d1ae8357d`.
- Desktop artifact: `9691490016`, SHA-256 `4010eeee6c1e4eb504b27e9b14a5af94851528d6ee19c7c582c9f6806f243c1b`.
- Complete suite: `PASS — 128/128; 0 failed/skipped`.
- EF: all ten existing migrations applied to disposable PostgreSQL 18.6; `No changes have been made to the model since the last migration`.
- API: protected boot probe returned expected HTTP 401. Desktop and Mobile Admin/Customer/Driver builds/probes passed.

## Control Tower independent W2 revalidation

- Revalidation time: `2026-08-28T16:11:03Z`.
- GitHub run API independently returned run `33185419917`, exact head `9c5b7a12e59d2c42e682717b8e90c491f8699b96`, branch `codex/mission-03-execution-20260828`, `completed/success`.
- Core job `98897056951` and Desktop job `98897057221` independently returned `completed/success`; all named restore/build/migration/test/API/client steps succeeded.
- Decoded core log independently records tree `452b37f1...`, PostgreSQL 18.6, no model changes since the last migration, ten committed migrations applied, `Passed: 128, Failed: 0, Skipped: 0`, and `http_code=401`.
- Artifact API independently returned Linux `9691527827` and Desktop `9691490016` with the recorded SHA-256 digests and `expired=false`.
- Failed run `33184771338` independently returned `completed/failure` at `d1c0a257...`; decoded job `98894801318` records `CS0246` for `OperationContext` during build. No later migration/test/API step ran.
- Disposition: exact-head evidence supports adoption of bounded W2-A1/A2/B1/B2A/C1/F1 only. It does not cover W2-B2B/C2/D/E/F2 or complete T-200/T-210/T-220.

This is the current exact-head W2 evidence. Earlier successful and failed runs remain historical evidence and are not substituted for it.
