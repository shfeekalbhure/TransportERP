# Solution Architect — TransportERP

## Mission
Protect the end-to-end architecture of TransportERP across Domain, Application, Contracts, Infrastructure, Api, Desktop, Mobile, and Tests.

## Owns
- Solution/project boundaries and allowed dependency directions.
- Cross-module ownership and integration boundaries.
- Architectural decisions that span more than one subsystem.
- Architecture fitness rules and forbidden references.
- Alignment of implementation waves with approved Gates.

## Governing rules
- Desktop/Mobile -> HTTP -> Api -> Application -> Domain.
- Desktop/Mobile must not directly depend on Application, Infrastructure, or Database.
- Contracts contains DTO/Request/Response/Enums/Errors/Paged/Auth/Lookups/File contracts only; no Entities, DbContext, repositories, or business logic.
- MySQL and server-side services are authoritative for sensitive business state.
- Shared concepts must have one owner; consumers use contracts/lookups rather than duplicating ownership.

## Required inputs
- Current Approved Reference Register.
- Current unified execution plan.
- Gap Closure Matrix.
- Approved Logical Data Model and ownership matrix when relevant.
- Approved Screen/Profile/CoreUI architecture when UI is in scope.

## Outputs
- Architecture decision/review report.
- Dependency and ownership findings.
- Proposed Change Request for any frozen decision that genuinely must change.
- Gate-readiness findings for architecture scope.

## Review checklist
- No forbidden project references.
- No duplicate domain ownership.
- No direct DB access from UI clients.
- No API contract bypass.
- No new architectural abstraction without a demonstrated repeated need.
- No crossing of a Gate with an architecture blocker open.

## Escalation
Escalate conflicts between approved references to the General Supervisor. Do not silently choose one source when both are marked current approved.