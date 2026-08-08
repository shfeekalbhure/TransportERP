# TransportERP Agent Team Roster

This file is the routing index for specialist agent roles. The General Supervisor owns assignment and final conflict resolution.

| Role | File | Primary scope | Must not self-approve |
|---|---|---|---|
| General Supervisor | `GENERAL_SUPERVISOR.md` | Coordination, conflicts, Gates, final integrated decision | Critical architecture/security/accounting/release changes |
| Solution Architect | `SOLUTION_ARCHITECT.md` | Solution boundaries, ownership, cross-module architecture | Own architecture changes |
| Screen & CoreUI Architect | `SCREEN_COREUI_ARCHITECT.md` | Profiles, CoreUI, ScreenDefinition, shared UI contracts | Frozen profile/core changes |
| Data & MySQL Architect | `DATA_MYSQL_ARCHITECT.md` | Logical/physical data model, constraints, migrations | Physical schema decisions with open OTS |
| Accounting Consultant | `ACCOUNTING_CONSULTANT.md` | Financial invariants, posting, reports, workflows | Accounting rules authored by same role |
| API & Security Reviewer | `API_SECURITY_REVIEWER.md` | API contracts, permissions, scope, idempotency, retry safety | Security waivers |
| UX/UI Reviewer | `UX_UI_REVIEWER.md` | RTL, usability, visual consistency, states | Changes to frozen CoreUI/Profile contracts |
| QA & Testing Reviewer | `QA_TESTING_REVIEWER.md` | Acceptance, regression, defects, Gate evidence | Waiver of Critical/High defects |
| Release & Integration Reviewer | `RELEASE_INTEGRATION_REVIEWER.md` | Build, migrations, deployment, rollback, release readiness | Waiver of blockers owned by other roles |

## Routing examples
- New cross-project dependency or module ownership question -> Solution Architect.
- New screen/profile/container behavior -> Screen & CoreUI Architect, then UX/UI review.
- PK/FK/precision/UUID/migration question -> Data & MySQL Architect.
- Posting/reversal/period/report correctness -> Accounting Consultant.
- Endpoint/permission/scope/retry/idempotency -> API & Security Reviewer.
- Acceptance/regression/Gate evidence -> QA & Testing Reviewer.
- Deployment/rollback/release candidate -> Release & Integration Reviewer.

## Critical work review pairs
Recommended minimum independent review:
- Architecture: Solution Architect -> General Supervisor.
- CoreUI/ScreenDefinition: Screen & CoreUI Architect -> UX/UI Reviewer + QA Reviewer -> General Supervisor.
- Data model: Data & MySQL Architect -> Solution Architect/Accounting Consultant as relevant -> General Supervisor.
- Accounting: Accounting Consultant -> Data/API/QA reviewers as relevant -> General Supervisor.
- Security/API: API & Security Reviewer -> QA Reviewer -> General Supervisor.
- Release: Release & Integration Reviewer -> QA Reviewer -> General Supervisor.

The roster defines responsibilities only. It does not imply that the execution environment has actually spawned multiple agents.