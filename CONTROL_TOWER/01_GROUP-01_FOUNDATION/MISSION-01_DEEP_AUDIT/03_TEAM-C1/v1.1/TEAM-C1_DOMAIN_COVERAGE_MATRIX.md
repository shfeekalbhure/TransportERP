# TEAM-C1 Domain Coverage Matrix — v1.1

**Baseline ref/full SHA:** `refs/heads/governance/control-tower-20260828` @ `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

**Version / scope:** `1.1 / TEAM-C1 current-architecture coverage only; TEAM-A/B/D columns are N/A in this team-local register`

| Domain / area | TEAM-C1 status | Governing evidence IDs | Important files/components | Gaps/unavailable sources | Unresolved P0/P1? | Gate effect | Current placement/state |
|---|---|---|---|---|---|---|---|
| Repository/Git baseline | REVIEWED | C1-BASE-001/002, C1-HIST-001 | refs/history/worktree | Current-line authority is governed centrally, not designated by this local matrix | UNKNOWN outside C1 | Does not block C1 handoff | Baseline snapshot recorded |
| Solution/projects | REVIEWED | C1-SOL-001, C1-PROJ-001 | `.slnx`, 10 csproj | IDE GUI unavailable | No | Does not block C1 | 10 projects; flat solution |
| Domain | REVIEWED | C1-DOM-001 | `TransportERP/Waybills/*` | Runtime not executed | No C1 P0/P1 | Does not block C1 | Waybill rules only |
| Application | REVIEWED | C1-APP-001/002 | Waybill services; P1 in-memory | Runtime not executed | No C1 P0/P1 | Does not block C1 | Active Waybill plus prototype |
| Contracts/shared | REVIEWED | C1-CON-001, C1-SHARED-001 | Contracts/Core/Waybills/Geo/etc. | External consumers unknown | No C1 P0/P1 | Does not block C1 | Active plus foundation surfaces |
| API | REVIEWED | C1-RUN-001/002, C1-ARCH-004/005 | Program + Waybill modules | Boot unavailable | No C1 P0/P1 | Blocks runtime-ready claim only | Sole static startup; 23 routes |
| Infrastructure/data | REVIEWED | C1-DATA-001/002, C1-INF-001 | DbContext/factory/entities/services | Applied DB unavailable | UNKNOWN | May affect later gate | One broad persistence boundary; factory fails closed without env var |
| Database applied state | BLOCKED | C1-MIG-001, C1-UNK-003 | Migrations/snapshot only | Live/disposable DB evidence absent | UNKNOWN | Later gate effect UNKNOWN | `ACCESS BLOCKED — UNKNOWN` |
| Security | PARTIAL | C1-RUN-001, C1-DATA-001 | JWT setup/P1 entities | End-to-end/runtime tests absent | UNKNOWN | Later gate effect UNKNOWN | Static foundation/composition only |
| Multi-tenant isolation | PARTIAL | C1-CON-001, C1-RUN-001 | Operation context/claims/query patterns | Bidirectional runtime tests absent | UNKNOWN | Later gate effect UNKNOWN | Cross-cutting static patterns |
| Offline/sync | REVIEWED | C1-INF-001, C1-RUN-001 | sync API/service/entities | Client outbox/replay runtime absent | UNKNOWN outside C1 | Does not block architecture handoff | Server enqueue/state only |
| Desktop | REVIEWED | C1-UI-001/002 | Desktop csproj/forms/catalog | Not launched | No C1 P0/P1 | Blocks executable-runtime claim only | Library; 16 forms/19 IDs; disconnected |
| Mobile | REVIEWED | C1-MOB-001 | Three mobile csproj | No source/runtime | No C1 P0/P1 | Does not block inventory | Empty placeholder libraries |
| Shipping/Waybills | REVIEWED | C1-DOM-001, C1-APP-001, C1-RUN-002, C1-ARCH-002 | five server layers + Desktop | Runtime not executed | UNKNOWN outside C1 | Later gate effect UNKNOWN | Broad static composition |
| Ticketing | REVIEWED | C1-PROJ-001, repository search | production tree | External requirement source absent | No current implementation found | Does not block C1 | Not present in current source |
| Accounting/general setup | REVIEWED | C1-DATA-001, C1-INF-001 | P1 entities/DbContext/voucher | Complete API/UI/runtime absent | UNKNOWN outside C1 | Later gate effect UNKNOWN | Data/service foundation |
| Reporting | REVIEWED | C1-PROJ-001, repository search | production tree | External reports may exist | No current subsystem found | Does not block C1 | No dedicated subsystem proved |
| Tests | REVIEWED structure / BLOCKED execution | C1-TEST-001, C1-CI-001 | test project/files | CLI/CI execution absent | UNKNOWN | Blocks exact-SHA verification claim | One broad test project |
| CI/CD and supply chain | PARTIAL | C1-CI-001, C1-PKG-001 | workflows/csproj | No exact-SHA run or resolved graph | UNKNOWN | Later gate effect UNKNOWN | Definitions/direct packages only |
| Release/deployment | BLOCKED | C1-UNK-005 | no deployment source | Manifest/telemetry/provisioning absent | UNKNOWN | Blocks release-ready claim | `ACCESS BLOCKED — UNKNOWN` |
| Privacy/sensitive data | PARTIAL | C1-DATA-001, C1-RUN-001 | entities/API static surfaces | Runtime/log/export/retention evidence incomplete | UNKNOWN | Later gate effect UNKNOWN | Architecture-only observation |
| Governance/Kurrasa | PARTIAL | C1-SRC-013, C1-UNK-009 | Control Tower files | External Kurrasa not supplied | UNKNOWN | Later gate effect UNKNOWN | Governing C1 scope read; requirement comparison excluded |
| Preservation/local work | PARTIAL | C1-HIST-001, C1-UNMERGED-001, C1-UNK-007 | Git/PR/worktree evidence | External Codex sessions unavailable | UNKNOWN | Later preservation gate effect UNKNOWN | Known unmerged PR separated; exhaustive inventory blocked |

No field in this matrix converts a partial/blocked domain into a complete runtime claim.
