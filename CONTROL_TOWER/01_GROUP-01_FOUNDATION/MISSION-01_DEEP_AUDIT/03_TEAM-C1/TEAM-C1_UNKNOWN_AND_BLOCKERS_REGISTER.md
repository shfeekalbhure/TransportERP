# TEAM-C1 Unknowns and Blockers Register

| ID | Unknown/blocker | Evidence/status | Impact |
|---|---|---|---|
| C1-UNK-001 | Exact-SHA restore/build/test result | `dotnet` is unavailable locally; GitHub has zero check/workflow runs for the SHA | `NOT RUN`; compilation and tests for this exact SHA are unverified |
| C1-UNK-002 | Actual API boot behavior | No exact-SHA execution evidence and no current `appsettings*.json`/launch profile | `UNKNOWN — REQUIRES VERIFICATION` |
| C1-UNK-003 | Live PostgreSQL schema and migration application state | No database instance/connection supplied | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` |
| C1-UNK-004 | Transitive NuGet graph and resolved versions | No lock file; restore unavailable | Direct references are known; resolved transitive graph is `UNKNOWN — REQUIRES VERIFICATION` |
| C1-UNK-005 | Production/deployed runtime composition | No deployment manifest or telemetry supplied | “Current runtime” is limited to source-level reachability from `Program.cs` |
| C1-UNK-006 | External consumers of unused-looking contracts/events | Repository search finds none; external/reflection consumers cannot be ruled out | “Unused” means “no current in-repository reference beyond declaration/tests” |
| C1-UNK-007 | External Codex workspace/session inventory | No cross-workspace/session access source supplied | `ACCESS BLOCKED`; does not block authoritative Git-tree assessment |
| C1-UNK-008 | Visual Studio rendered solution UI | Visual Studio GUI unavailable | No blocker: raw `.slnx` proves a flat project list and no Solution Folder elements |
| C1-UNK-009 | Official requirement-source comparison outside repository | No external Kurrasa/other authoritative source was supplied to TEAM-C1 | Does not block current architecture inventory; requirement conformance remains outside this report |
| C1-UNK-010 | Whether prototype/foundation files are intentionally retained | Code/history contains no binding disposition record for each such surface | Classification is evidence-based; final disposition is `UNKNOWN — REQUIRES VERIFICATION` |

## Blocker disposition

No blocker prevents a source-level current-architecture assessment. C1-UNK-001 through C1-UNK-005 prevent claims that the authoritative SHA builds, tests, migrates, boots, or is deployed successfully.

