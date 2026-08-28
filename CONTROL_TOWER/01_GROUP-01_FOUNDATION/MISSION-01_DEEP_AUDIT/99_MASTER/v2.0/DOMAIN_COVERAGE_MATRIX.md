# MASTER/GATE v2.0 Domain Coverage Matrix

| Domain | Current master state | PR69 candidate effect | Planning state |
|---|---|---|---|
| Architecture | 10-project partial modular layering; broad persistence boundary | expands to 13 projects and dedicated Offline/E2E | COVERED — plan boundary changes only after adoption review |
| Waybill/Shipping | partial; `Volume` P0; shipping through departure only | changes Waybill/Shipping paths but retains Volume mapper omission | COVERED — P0 first |
| Database/Migrations | 10 migrations + snapshot; live state unknown | 20 migrations; extensive candidate guards/tests | COVERED — DB-GOV-001 gate |
| Identity/RBAC/Tenant | claim/manual/partial DB defense | material candidate hardening | COVERED — independent negative plan required |
| Offline/Sync | server foundation; lifecycle owner gap | material typed runtime/device/client additions | COVERED — authority and adoption gates |
| Accounting/Audit | status-only/partial atomicity and hash scope | some persistence/finance/audit changes; canonical rules unresolved | COVERED — ADR/requirements gate |
| Desktop/Mobile | Desktop library; Mobile placeholders | executable Desktop and Android Driver candidate; Admin/Customer remain limited | COVERED — exact target/runtime matrix |
| Ticketing/Reporting | absent or unproved | no governing closure | COVERED AS GAP |
| Tests/CI | 1 test project, 22 C# tests, 7 workflows; partial exact-SHA CI | 3 test projects, 63 C# tests; exact-head workflows green | COVERED — PASS cannot transfer |
| Release/Recovery | repository chain absent; external state unknown | candidate CI/evidence; no Production deploy/merge | COVERED — release gate remains closed |
| Privacy/Supply chain | surfaces known; end-to-end controls/graph unknown | candidate redaction/security/CI additions | COVERED — external and policy evidence required |
| Kurrasa/Screens | version-bound authority | no authority transfer | COVERED — canonical crosswalk first |
| Git/Preservation | current SHA frozen; local/unmerged assets preserved | exact PR head frozen and classified | COVERED — no destructive action |

No critical domain lacks a state or planning action. `COVERED` here means sufficient for planning, not PASS for implementation or release.
