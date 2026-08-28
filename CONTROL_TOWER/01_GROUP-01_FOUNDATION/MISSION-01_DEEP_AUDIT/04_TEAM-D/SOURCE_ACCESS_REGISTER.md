# TEAM-D Source Access Register

| Source ID | Source | Access | Scope used | Limitation |
|---|---|---|---|---|
| D-SRC-001 | MISSION-01 governing command and Control Tower directives | AVAILABLE | Full governing requirements and TEAM-D authority | None for file content |
| D-SRC-002 | Sealed TEAM-A package | AVAILABLE; 13/13 manifest hashes verified | Report, evidence, files, unknowns, coverage, preservation, baseline, seal, handoff | External Library artifacts remain version-bound references |
| D-SRC-003 | Sealed TEAM-B package | AVAILABLE; 13/13 detached hashes verified | Report, evidence, files, unknowns, coverage, preservation, baseline, seal, handoff | `BLK-B-001` single-session assurance limitation |
| D-SRC-004 | Sealed TEAM-C1 package | AVAILABLE; 9/9 seal hashes verified | Architecture report/inventory/dependencies/evidence/files/unknowns/coverage/manifest/handoff | No build/runtime/live DB evidence |
| D-SRC-005 | Product source tree represented by `master@2ec6cccf...` | AVAILABLE read-only through identical local product tree | Direct source rechecks for critical and cross-team claims | This is a candidate snapshot, not proven authority |
| D-SRC-006 | Local Git refs, objects, log, worktrees and hashes | AVAILABLE read-only | Candidate-line and preservation reconciliation | External workspaces/developer machines not exhaustive |
| D-SRC-007 | Remote refs through `git ls-remote` | AVAILABLE read-only | Fresh symbolic HEAD and selected branch/PR heads | Remote file content at new PR69 head not fetched |
| D-SRC-008 | Local .NET SDK/CLI | ACCESS BLOCKED | Exact-SHA restore/build/test/runtime | `dotnet` executable absent |
| D-SRC-009 | PostgreSQL/live database | ACCESS BLOCKED / NOT PROVIDED | Applied schema, data, roles, RLS, backup/restore | DB-GOV-001 forbids inference or modification |
| D-SRC-010 | Production/deployment environment and external IdP | ACCESS BLOCKED / NOT PROVIDED | Runtime, session, encryption, retention, deployment, recovery | All remain unknown |
| D-SRC-011 | External Kurrasa/Library evidence | PARTIALLY AVAILABLE through sealed TEAM-A/B evidence | Authority/drift/offline guardrails | TEAM-D did not receive a fresh independent Library snapshot |

All reads were non-mutating. No Source, Tests, Migrations, Database, Production configuration, branch history, or predecessor output was changed.
