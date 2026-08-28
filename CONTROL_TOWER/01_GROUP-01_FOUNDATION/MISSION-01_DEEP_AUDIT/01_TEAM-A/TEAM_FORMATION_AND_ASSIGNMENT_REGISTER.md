# TEAM-A — Formation and Assignment Register

Version: `A-TEAM-v1.0`. All lanes operated read-only against `governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1`. Each reviewer was expressly instructed not to open or use TEAM-B content. Assignments were issued during the bounded audit window beginning `2026-08-28T00:27:51Z / 03:27:51+03:00`; exact per-lane assignment seconds were not captured and are not guessed.

| Lane / canonical agent ID | Assignment | Allowed sources | Access/independence constraints | Status/result | Independence declaration |
|---|---|---|---|---|---|
| TEAM-A lead `/root` | Mandatory reading, baseline, direct cross-checks, reconciliation, report/registers/seal | A-SRC-001–012 as applicable | No TEAM-B; no source/test/migration/DB mutation; no production | COMPLETE | Yes |
| Architecture `/root/team_a_architecture` | Solution/projects, dependencies, API, Desktop, screens, components, duplication | A-SRC-001 and TEAM-A outputs for final QA | Read-only; no TEAM-B; runtime not assigned | COMPLETE + final QA | Yes |
| Database/security `/root/team_a_db_security` | DB, migrations, security, RBAC, isolation, accounting integrity, sensitive data | A-SRC-001; repository-visible tests | Read-only; no TEAM-B; no DB execution | COMPLETE | Yes |
| Offline/mobile/privacy `/root/team_a_offline_mobile_privacy` | Offline/sync, device model, Mobile, privacy/retention/local data | A-SRC-001/A-SRC-004 as relevant | Read-only; no TEAM-B; no device/runtime | COMPLETE | Yes |
| Business/Kurrasa `/root/team_a_business_kurrasa` | Shipping, ticketing, accounting, screens, Kurrasa authority/gaps | A-SRC-001/A-SRC-004–007 | Read-only; no TEAM-B; docs are not implementation proof | COMPLETE | Yes |
| Git/GitHub/CI `/root/team_a_git_github_ci` | History/refs/PRs/checks/workspaces/local work/CI/supply/release | A-SRC-001–003/A-SRC-008/A-SRC-010 | Read-only; no TEAM-B; mutable evidence SHA-bound | COMPLETE | Yes |
| Tests/runtime `/root/team_a_tests_runtime` | Test inventory, acceptance, exact-SHA CI, isolated safe attempt, QA gaps | A-SRC-001/A-SRC-003/A-SRC-009 and TEAM-A outputs for QA | No TEAM-B; isolated clone; no source mutation; missing .NET runtime | COMPLETE + final QA | Yes |

The TEAM-A lead independently read every mandatory governance and command file before analysis and retained responsibility for final judgments. Sub-review findings were reconciled against direct paths and were not copied as unexamined conclusions. Finding-by-finding technical, impacted-specialty and evidence-review assignments are recorded in report section 9.0.
