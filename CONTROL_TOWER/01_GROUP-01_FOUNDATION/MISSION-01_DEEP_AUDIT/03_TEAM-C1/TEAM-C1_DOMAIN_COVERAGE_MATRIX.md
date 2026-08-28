# TEAM-C1 Domain Coverage Matrix

This matrix records current architectural placement, not requirement completion or target design.

| Area | Source evidence reviewed | Current placement/state | Confidence |
|---|---|---|---|
| Repository/Git baseline | Branch, refs, history, worktree, current/master/unmerged diffs | Authoritative SHA fixed; source equals master plus governance-only commits | HIGH |
| Solution/projects | `.slnx`, all csproj | 10 projects, flat solution | HIGH |
| Domain | All Domain source | Waybill aggregate/financial/shipping rules only | HIGH |
| Application | All files structurally, active services in detail | Active Waybill services plus test-only P1 in-memory model | HIGH |
| Contracts/shared | All declarations and use search | Active Waybill/Core; several foundation-only surfaces | HIGH |
| API | Startup and all endpoint modules | Sole executable; 23 endpoints | HIGH |
| Infrastructure/data | DbContext/entities/models/services/migrations | One persistence project/DbContext for all current server data | HIGH |
| Database applied state | No live database source | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | NONE |
| Desktop | csproj and all source | 16 concrete forms/19 IDs; library; no composition/client wiring | HIGH |
| Mobile | all three project directories/csproj | Empty conditional MAUI placeholders; current libraries | HIGH |
| Offline/sync | sync API/service/entities and repo-wide search | Server enqueue/state persistence exists; no client offline/outbox/replay runtime | HIGH |
| Security/identity | API auth setup and P1 entities | JWT validation and persistence entities; no complete feature architecture asserted | MEDIUM |
| Multi-tenancy | operation context, API claims, entity/query scope patterns | Company/branch scope is cross-cutting in active server code | MEDIUM |
| Accounting/general setup | P1 entities, DbContext, voucher service, API/UI search | Data/service foundation; no complete API/UI module proved | HIGH |
| Reporting | production project/type/search | No dedicated reporting subsystem proved | HIGH |
| Ticketing | production project/type/search | No current production implementation found | HIGH |
| Lookups/common dialogs | Geo/Party contracts, routes, Desktop code | Operational-party search only; Geo contracts-only; no common dialog layer | HIGH |
| Tests | csproj, suites and test declarations | One broad xUnit project; 101 Facts + 2 Theories; exact SHA not run | HIGH structure / NONE execution |
| CI | workflow definitions and exact-SHA GitHub status | Definitions exist; zero exact-SHA runs/checks | HIGH |
| Historical/unmerged | Git refs/history and PR #69 targeted diff | 13-project offline/UI runtime architecture is unmerged, not current | HIGH |

