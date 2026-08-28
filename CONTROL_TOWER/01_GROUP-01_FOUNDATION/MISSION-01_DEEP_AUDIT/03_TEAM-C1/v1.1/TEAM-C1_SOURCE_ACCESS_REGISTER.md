# TEAM-C1 Source Access Register — v1.1

**Mission / owner:** `MISSION-01_DEEP_AUDIT / TEAM-C1`

**Version / scope:** `1.1 / corrected current-architecture package; no target design`

**Analytical baseline:** `refs/heads/governance/control-tower-20260828` @ `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

**Reopen verification:** `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00` (Asia/Aden)

For inherited v1.0 accesses, the exact original access instant was not captured; that omission is stated rather than reconstructed. The correction source and governing files were re-read during v1.1.

| Source ID | Type | Name/path/ref | Access time UTC / Asia-Aden | Ref / full SHA / version | Access state | Reviewer/role | Scope read | Limitations / unavailable content |
|---|---|---|---|---|---|---|---|---|
| C1-SRC-001 | Local Git worktree | Repository root | Original date `2026-08-28`, exact time not recorded | `refs/heads/governance/control-tower-20260828` @ `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` | AVAILABLE | TEAM-C1 architecture reviewer | Current product tree and Git state used by v1.0 | Runtime not executed; v1.1 worktree has unrelated concurrent governance changes and is not used to rewrite the baseline |
| C1-SRC-002 | Remote Git ref | `origin/governance/control-tower-20260828` | Original date `2026-08-28`, exact time not recorded | `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` at original access | AVAILABLE | TEAM-C1 evidence role | Ref identity | Not refreshed during this limited reopen |
| C1-SRC-003 | Remote Git ref | `origin/master` | Original date `2026-08-28`, exact time not recorded | `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` at original access | AVAILABLE | TEAM-C1 architecture reviewer | Source comparison | Not a designation of authoritative current line; not refreshed in v1.1 |
| C1-SRC-004 | Git evidence | commit/tree/history/refs | Original date `2026-08-28`, exact time not recorded | Baseline and refs recorded in v1.0 | AVAILABLE | TEAM-C1 evidence role | Current/historical/unmerged classification | No destructive Git action performed |
| C1-SRC-005 | GitHub checks/status | Exact baseline SHA status | Original date `2026-08-28`, exact time not recorded | `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` | AVAILABLE | TEAM-C1 evidence role | Check/workflow presence | Historical access result only; not refreshed in v1.1 |
| C1-SRC-006 | GitHub PR metadata | Governance branch PR lookup | Original date `2026-08-28`, exact time not recorded | Baseline ref | AVAILABLE | TEAM-C1 evidence role | PR association | Historical access result only |
| C1-SRC-007 | GitHub/Git unmerged candidate | PR #69 / `origin/codex/p1-security-device-sync-offline-20260825` | Original date `2026-08-28`, exact time not recorded | `939f49fa9c2ae57fa532ad55f67461c5f3f256f3` at original access | PARTIALLY AVAILABLE | TEAM-C1 architecture reviewer | Targeted solution/project comparison | UNMERGED; not current; not refreshed in v1.1 |
| C1-SRC-008 | Toolchain | Local .NET SDK/CLI | Original date `2026-08-28`, exact time not recorded | N/A | ACCESS BLOCKED | TEAM-C1 verification role | Restore/build/test availability | `dotnet` unavailable in v1.0; not rerun in limited correction |
| C1-SRC-009 | Database | PostgreSQL runtime/database | Original date `2026-08-28`, exact time not recorded | N/A | ACCESS BLOCKED | TEAM-C1 data architecture role | Applied schema/live state | No instance or connection provided; `DB-GOV-001` prohibits modification |
| C1-SRC-010 | IDE | Visual Studio GUI | Original date `2026-08-28`, exact time not recorded | N/A | ACCESS BLOCKED | TEAM-C1 architecture reviewer | Rendered Solution UI | Raw `.slnx` used instead; no GUI-dependent claim |
| C1-SRC-011 | Deployment source | Production/deployment telemetry | Original date `2026-08-28`, exact time not recorded | N/A | ACCESS BLOCKED | TEAM-C1 release evidence role | Deployed/runtime state | Not provided; no Production access attempted |
| C1-SRC-012 | External workspace source | Codex sessions/workspaces outside accessible tree | Original date `2026-08-28`, exact time not recorded | UNKNOWN | ACCESS BLOCKED | TEAM-C1 preservation/evidence role | External workspace inventory | Exhaustive external inventory not possible |
| C1-SRC-013 | Governing files | README; `00_GOVERNANCE/**/*`; master command; TEAM-C1 order; v1.0 package | `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00` onward | Control HEAD `e2843caff509d34509146f9dfe2e748dea22df7e`; governing files as present | AVAILABLE | TEAM-C1 reopen reviewer/evidence role | Full instructions, v1.0 package, mandatory register fields | Concurrent files outside `03_TEAM-C1` were not authored or used to alter conclusions |
| C1-SRC-014 | Direct source code | `TransportERP.Infrastructure/Persistence/TransportErpDbContextFactory.cs:8-18` | `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00` | Baseline `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`; byte-identical at `e2843caff509d34509146f9dfe2e748dea22df7e` | AVAILABLE | TEAM-C1 data architecture + evidence roles | Entire 20-line file; exact environment-variable/fail-closed behavior | Static source only; EF operation was not executed |

No Source, Tests, Migrations, Database, or Production configuration was modified.
