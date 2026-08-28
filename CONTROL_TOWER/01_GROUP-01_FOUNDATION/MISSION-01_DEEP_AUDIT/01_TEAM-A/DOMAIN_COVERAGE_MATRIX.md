# TEAM-A — Domain Coverage Matrix

Version: `A-COV-v1.0`. TEAM-B is `NOT REVIEWED` here by the independence rule; this is not a statement about TEAM-B's own work. TEAM-C1/TEAM-D states are pre-reconciliation placeholders only and do not start later missions.

| Domain / area | TEAM-A | TEAM-B | TEAM-C1 | TEAM-D reconciliation | Governing evidence | Principal files/components | Gaps / unavailable sources | Unresolved P0/P1? | Readiness effect |
|---|---|---|---|---|---|---|---|---|---|
| Repository/Git baseline | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-001/002/025/030 | local/remote Git, PRs, alternatives | Other machines/Codex sessions blocked | P0 preservation | BLOCKS READY until assets preserved |
| Solution/projects | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-003 | slnx + 10 csproj | Exact-SHA build blocked | P1 | BLOCKS READY claim, not inventory |
| Architecture | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-003–006/012 | project graph, API, application, Desktop | Runtime unavailable | P0 Volume + P1 | BLOCKS READY |
| API | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-004 | Program + 3 modules | Startup/runtime unavailable | P1 | BLOCKS READY |
| Database | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-007/008/010/011 | DbContext/models/migrations | Live/fresh/upgrade DB unavailable | P1 | BLOCKS READY |
| Migrations | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-008 | 10 Up, 9 Designers, snapshot | Not executed | P1 | BLOCKS READY |
| Security/Auth | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-004/007/009 | JWT/RBAC/sync | IdP/session/revocation blocked | P1 | BLOCKS READY |
| Multi-Tenant Isolation | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-007/009 | service predicates + FK model | Full negative matrix/RLS/grants unavailable | P1 | BLOCKS READY |
| Offline/Sync | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-009/018 | sync endpoint/service/Kurrasa | No worker/mobile/runtime/IdP | P1 | BLOCKS READY |
| Desktop | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-012/021/026 | csproj/forms/CI/tests | No executable/runtime | P1 | BLOCKS desktop-ready claim |
| Mobile Admin | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-013 | csproj only | No executable exists | P1 | BLOCKS mobile-ready claim |
| Mobile Customer | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-013 | csproj only | No executable exists | P1 | BLOCKS mobile-ready claim |
| Mobile Driver | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-013 | csproj only | No executable exists | P1 | BLOCKS mobile-ready claim |
| Waybill foundation | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-005/006 | aggregate/app/API/repository | Runtime/data impact unavailable | P0 | BLOCKS READY |
| Shipping | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-014/019 | shipping API/app/persistence/migration | Runtime; later lifecycle absent | P1 | BLOCKS end-to-end readiness |
| Ticketing | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-016/020 | source search + ticket docs | Unmerged/external work unknown | P1 | BLOCKS ERP completeness |
| Accounting/Finance | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-007/011/015 | entities/DbContext/voucher/finance | Posting/runtime/raw SQL/DB blocked | P1 | BLOCKS accounting-ready claim |
| Screens | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-012/017 | Desktop forms + queue/specs | Canonical crosswalk incomplete; no runtime | P1 | BLOCKS screen-ready claim |
| Shared Components | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-004/012 | API helpers/Desktop UI helpers | Target design authority not assessed | P2 | DOES NOT ALONE BLOCK READY |
| Tests/Acceptance | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-023/026/027 | Tests + registers + validators | .NET/runtime unavailable | P1 | BLOCKS READY |
| CI/CD | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-021/022/025 | workflows/rules/checks | Org controls partly blocked; CD absent | P1 | BLOCKS READY |
| Supply Chain | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-021/028 | csproj/workflows/config inventory | Current vuln/license scan unavailable | P1 | BLOCKS release-ready claim |
| Kurrasa/Governance | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-018/019/020/029 | main + shipping + targeted ticket docs | Full corpus not exhaustive | P1 authority uncertainty | UNKNOWN until owner crosswalk |
| Privacy/Sensitive Data | PARTIAL | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-007/009/010 | entities/sync/audit/API | Infrastructure/legal controls blocked | P1 | BLOCKS privacy-ready claim |
| Release/Deployment | BLOCKED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-022/028 | repo/GitHub inventory | External environment unavailable; chain absent | P1 | BLOCKS READY |
| Evidence/Governance | REVIEWED | NOT REVIEWED | NOT REVIEWED | N/A | A-EV-029 and TEAM-A registers | command + sealed package | Later-team reconciliation not started | No independent technical P0 | TEAM-A handoff only |

TEAM-A does not declare full runtime completion for any row marked `PARTIAL` or `BLOCKED`. Those states and their gate effects are carried into the final verdict.
