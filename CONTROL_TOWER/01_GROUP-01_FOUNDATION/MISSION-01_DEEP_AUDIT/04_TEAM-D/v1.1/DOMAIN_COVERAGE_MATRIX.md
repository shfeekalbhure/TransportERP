# TEAM-D v1.1 Domain Coverage Matrix

| Domain / area | TEAM-A coverage | TEAM-B coverage | TEAM-C1 v1.1 coverage | TEAM-D v1.1 direct coverage | Coverage state | Governing evidence | Unknown / blocker | P0/P1 consequence |
|---|---|---|---|---|---|---|---|---|
| Architecture / solution | REVIEWED | REVIEWED | FULL STRUCTURAL | solution/projects/integration rechecked | RECONCILED STATIC | D11-EV-022/026 | target line/design authority D11-BLK-001 | design/current-state gate blocked |
| Database / migrations | REVIEWED | REVIEWED | STRUCTURAL | models/migrations/guards rechecked | PARTIAL / ACCESS BLOCKED RUNTIME | D11-EV-009/011/024/027 | D11-BLK-003/009/013 | confirmed P0 plus P1/runtime uncertainty |
| Security / authentication | REVIEWED | REVIEWED | PARTIAL | API claims, context, permission and sync checks rechecked | PARTIAL / EXTERNAL BLOCKED | D11-EV-007/008/014 | D11-BLK-004/012 | P1; exploitability/environment partly unknown |
| Multi-tenant / RBAC | REVIEWED | REVIEWED | PARTIAL | tenant filters/context plus owner boundaries rechecked | RECONCILED STATIC / PARTIAL RUNTIME | D11-EV-007/009/014 | D11-BLK-003/004/012 | P1 and adversarial-runtime gap |
| Offline / sync | REVIEWED | REVIEWED | STRUCTURAL | complete service read; lifecycle ownership scope expanded | RECONCILED STATIC / RUNTIME ABSENT | D11-EV-013/014 | D11-BLK-012 | confirmed static P1 foundation defect |
| Desktop | REVIEWED | REVIEWED | REVIEWED | all project source/config inventoried | RECONCILED STRUCTURAL | D11-EV-015/022 | D11-BLK-002/006 | P1; no executable-runtime proof |
| Mobile | REVIEWED | REVIEWED | REVIEWED | three project directories/configs inventoried | RECONCILED STRUCTURAL | D11-EV-015 | D11-BLK-002/008 | P1; snapshot contains no implementation |
| Shipping / Waybill | REVIEWED | REVIEWED | INVENTORIED | repository/mapper/domain/API path rechecked | RECONCILED STATIC | D11-EV-006/016/022 | D11-BLK-003/009 | `A-ARCH-002` confirmed P0 |
| Ticketing / passenger | REVIEWED | REVIEWED | INVENTORIED | repository-wide bounded inventory rechecked | RECONCILED SNAPSHOT | D11-EV-016 | D11-BLK-001/008 | P1 scope absent on assessed snapshot |
| Accounting / finance | REVIEWED | REVIEWED | STRUCTURAL | lifecycle/persistence/models rechecked | RECONCILED STATIC / PARTIAL DB | D11-EV-011/012/024 | D11-BLK-003 | P1; posting/live enforcement unknown |
| Screens / UX / RTL | REVIEWED | REVIEWED | STRUCTURAL | desktop forms/design/acceptance references rechecked | PARTIAL AUTHORITY | D11-EV-018/022 | D11-BLK-006 | P1 requirement/screen mapping incomplete |
| Tests / acceptance | REVIEWED | REVIEWED | INVENTORIED | all test source/config and cited acceptance rows reviewed | FULL STATIC / NOT EXECUTED | D11-EV-017/018/023 | D11-BLK-002 | P1; no target execution/coverage PASS |
| CI/CD | REVIEWED | REVIEWED | STRUCTURAL | tracked workflows and bound predecessor CI reviewed | RECONCILED SNAPSHOT / PARTIAL CURRENT | D11-EV-017/019/023 | D11-BLK-001/002/008 | P1; moving refs and target run unknown |
| Supply chain | REVIEWED | REVIEWED | STRUCTURAL | all project/workflow configuration scanned | PARTIAL | D11-EV-020 | target restore/advisory/license graph unavailable | P1 |
| Privacy / sensitive data | REVIEWED | REVIEWED | PARTIAL | entity/payload/API surfaces rechecked | PARTIAL / EXTERNAL BLOCKED | D11-EV-021 | D11-BLK-005 | P1; external controls unknown |
| Release / deployment / recovery | REVIEWED | REVIEWED | PARTIAL | repository workflow/tag/deploy inventory reviewed | ACCESS BLOCKED EXTERNAL | D11-EV-019 | D11-BLK-002/003/005 | P1/final readiness blocked |
| Kurrasa / governance | VERSION-BOUND | VERSION-BOUND | NOT PRIMARY SCOPE | cited v72 and repository acceptance material reviewed | PARTIAL AUTHORITY | D11-EV-018 | D11-BLK-006 | final implementation scope blocked |
| Git / PRs / workspaces / preservation | REVIEWED | PARTIAL | PARTIAL | local and selected remote refs re-enumerated | RECONCILED FOR LISTED ASSETS | D11-EV-005 | D11-BLK-007/008/010 | `A-PRES-001` confirmed local-only P0 |
| Reporting subsystem | INVENTORIED | INVENTORIED | INVENTORIED | solution/API/application/source inventory reconciled | RECONCILED SNAPSHOT | D11-EV-016/022 | D11-BLK-001/002 | implementation/runtime scope remains gated |
| EF design-time factory | prior fallback claim affected | no independent claim | CORRECTED by v1.1 | source read at `CreateDbContext:8-18` | RECONCILED STATIC | D11-EV-027 | D11-BLK-013 | no source fallback; runtime value/tooling unknown |

`FULL`, `RECONCILED`, and `REVIEWED` are purpose-bound. They do not mean runtime PASS, Production readiness, or authority selection. No critical domain is silently omitted; partial and inaccessible coverage is carried into the gate.
