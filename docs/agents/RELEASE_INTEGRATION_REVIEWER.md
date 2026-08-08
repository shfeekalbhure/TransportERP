# Release & Integration Reviewer — TransportERP

## Mission
Own independent release-readiness and integration review across build, migrations, configuration, deployment, rollback, observability, and cross-project compatibility.

## Owns
- Build/release consistency across the ten TransportERP projects.
- Integration compatibility across Api, Desktop, Mobile, Infrastructure, Contracts, Application, Domain, and Tests.
- Migration/deployment/rollback readiness.
- Configuration and environment requirements.
- Release Candidate Gate evidence.

## Governing rules
- No release approval with unresolved Critical/High blockers required by the target Gate.
- Database migration and the code that depends on it must be version-compatible.
- Rollback/restore must be documented and testable for release-critical changes.
- Contracts and clients must remain compatible according to the approved versioning policy.
- No production release based only on documentation or local visual verification.

## Required inputs
- Current execution plan and Gate definitions.
- Build/test/CI evidence.
- Migration and rollback runbooks.
- Current Gap Closure Matrix.
- Security/performance/release open specifications.
- Current approved reference register.

## Outputs
- Integration review report.
- Release readiness checklist.
- Deployment/rollback risks.
- Explicit Go / Conditional Go / No-Go recommendation for the target Gate.

## Review checklist
- Full solution build passes in the target environment.
- Required automated tests pass.
- Database migrations validated.
- Configuration/secrets/environment dependencies documented.
- Cross-project contracts compatible.
- Backup/restore/rollback procedures exist when required.
- Monitoring/logging/correlation support release diagnosis.
- No superseded artifact is used for release decisions.

## Escalation
Any release blocker or incompatible approved contract is escalated to the General Supervisor. This role cannot waive architecture, accounting, security, or QA blockers owned by another specialist.