# TEAM-D Domain Coverage Matrix

| Domain / area | TEAM-A | TEAM-B | TEAM-C1 | TEAM-D status | Governing evidence | Remaining gap / P0-P1 impact |
|---|---|---|---|---|---|---|
| Architecture / Solution | REVIEWED | REVIEWED | REVIEWED | RECONCILED | D-EV-004, D-EV-022/023 | target authority/design belongs to C2; line authority unknown |
| Database / Migrations | REVIEWED | REVIEWED | REVIEWED structural | PARTIAL | D-EV-009/011/012/025 | live/applied DB and recovery ACCESS BLOCKED; P0/P1 relevant |
| Security / Authentication | REVIEWED | REVIEWED | PARTIAL | PARTIAL | D-EV-007/008 | IdP/session/device dynamic controls ACCESS BLOCKED; P1 relevant |
| Multi-Tenant / RBAC | REVIEWED | REVIEWED | PARTIAL | RECONCILED STATIC / PARTIAL RUNTIME | D-EV-007/009 | live roles/RLS and adversarial matrix unknown; P1 relevant |
| Offline / Sync | REVIEWED | REVIEWED | REVIEWED structural | RECONCILED STATIC | D-EV-013 | end-to-end runtime absent on snapshot; latest unmerged candidate unknown; P1 |
| Desktop | REVIEWED | REVIEWED | REVIEWED | RECONCILED | D-EV-014/017/023 | executable Windows behavior not run; P1 |
| Mobile | REVIEWED | REVIEWED | REVIEWED | RECONCILED | D-EV-014 | snapshot has no implementation; unmerged latest candidate unknown; P1 |
| Shipping / Waybill | REVIEWED | REVIEWED | REVIEWED | RECONCILED STATIC | D-EV-006/015/022 | `Volume` P0 confirmed; runtime/data impact unknown; lifecycle P1 partial |
| Ticketing / Passenger | REVIEWED | REVIEWED | REVIEWED inventory | RECONCILED | D-EV-015 | absent on snapshot; external/local prototypes unknown; P1 |
| Accounting / Finance | REVIEWED | REVIEWED | REVIEWED structural | RECONCILED STATIC / PARTIAL DB | D-EV-012, D-EV-025 | posting/live DB/recovery unknown; P1 |
| Screens / UX / RTL | REVIEWED | REVIEWED | REVIEWED structural | RECONCILED | D-EV-014/017/023 | screen authority crosswalk/latest Library partial; P1 |
| Tests / Acceptance | REVIEWED | REVIEWED | REVIEWED inventory | PARTIAL | D-EV-016/017/024 | exact target tests and runtime acceptance not run; P1 |
| CI/CD | REVIEWED | REVIEWED | REVIEWED structural | RECONCILED SNAPSHOT / PARTIAL CURRENT | D-EV-016/018/024 | moving refs, artifacts, required controls and org controls incomplete; P1 |
| Supply Chain | REVIEWED | REVIEWED | REVIEWED structural | PARTIAL | D-EV-019 | transitive graph/advisories/licenses inaccessible; P1 |
| Privacy / Sensitive Data | REVIEWED | REVIEWED | PARTIAL | PARTIAL | D-EV-020 | encryption/retention/legal/production controls inaccessible; P1 |
| Release / Deployment / Recovery | REVIEWED | REVIEWED | PARTIAL | ACCESS BLOCKED | D-EV-018 | no repo chain; external environment unavailable; P1 |
| Kurrasa / Governance | REVIEWED version-bound | REVIEWED version-bound | NOT IN SCOPE | PARTIAL | D-EV-017 | latest authority/current-line decision unknown; gate relevant |
| Git / PRs / Workspaces / Preservation | REVIEWED | REVIEWED partial | PARTIAL | RECONCILED FOR LISTED ASSETS | D-EV-005/021 | external workspaces and final disposition unknown; P0 preservation |
| Reporting subsystem | REVIEWED inventory | REVIEWED inventory | REVIEWED inventory | RECONCILED | D-EV-015/022 | operational/reporting runtime not found; scope planning required |

No critical domain is left without a TEAM-D status. `PARTIAL` or `ACCESS BLOCKED` is carried into the gate and is not represented as completion.
