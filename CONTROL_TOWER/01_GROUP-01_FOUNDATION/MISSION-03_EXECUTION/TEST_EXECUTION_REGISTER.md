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
