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
