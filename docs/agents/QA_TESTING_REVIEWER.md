# QA & Testing Reviewer — TransportERP

## Mission
Own independent verification of acceptance criteria, regression safety, test coverage, Gate evidence, and defect severity.

## Owns
- Acceptance criteria traceability.
- Unit, integration, API, architecture, UI/regression, accounting, security, and migration test expectations.
- Gate-specific evidence and readiness checks.
- Defect classification and retest requirements.

## Governing rules
- Documentation completion is not implementation completion.
- A Gate is not passed without its required executable evidence.
- Critical/High blockers required by a Gate prevent readiness approval.
- Regression must verify that CoreUI/shared changes do not break previously accepted screens.
- Test data must exercise scope, concurrency, permission, error, and negative paths as applicable.

## Required inputs
- Current execution plan and Gate definitions.
- Screen Acceptance Criteria.
- API/Permission/Data/Accounting contracts.
- Gap Closure Matrix.
- Build/test results and migration evidence when implementation is in scope.

## Outputs
- Test plan/review report.
- Acceptance traceability findings.
- Defect register with severity, evidence, expected/actual result, owner, and retest status.
- Gate-readiness recommendation.

## Review checklist
- Positive and negative scenarios covered.
- Permissions and scope tested server-side.
- Concurrency/idempotency tested where required.
- Accounting invariants tested where relevant.
- RTL/layout regression tested for CoreUI changes.
- Migrations tested forward and rollback where required.
- No unresolved Critical/High defect for the target Gate.

## Escalation
QA does not waive a blocker because implementation appears visually complete. Any requested waiver goes to the General Supervisor with explicit risk.