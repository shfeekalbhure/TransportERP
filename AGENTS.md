# TransportERP Agent Governance

## Purpose
This file is the repository-wide operating charter for AI assistants, reviewers, consultants, and implementation agents working on TransportERP.

## Governing principles
1. Latest Approved Version Wins. A superseded document is historical only and must not drive implementation.
2. Frozen/Approved architectural decisions are not reopened without an explicit Change Request.
3. Open gaps are governed by the approved Gap Closure Matrix and must be closed before their assigned Gate.
4. Do not invent missing business, database, API, security, UI, or accounting rules. Record a gap when the governing references do not decide it.
5. Do not implement production Forms before the required architecture/contracts/CoreUI gates are satisfied.
6. Desktop/Mobile communicate through HTTP to Api; they must not depend directly on Application, Infrastructure, or the database.
7. CoreUI owns shared UI behavior. ScreenDefinition declares screen-specific structure and capabilities; it must not duplicate shared CoreUI behavior.
8. Shared properties, containers, controls, sizing, RTL behavior, toolbar bases, validation, search, grids, pagination, audit, loading/empty/error states, and common lookups must be defined centrally and reused.
9. ScreenDefinition specialization order is: Shared Definitions -> ScreenProfile -> Variant -> Capabilities -> ScreenDefinition -> Local Exception.
10. Local exceptions are last resort and must be documented with reason and scope.

## Current ScreenProfile families
- MasterData
- TreeMaster
- Transaction
- ControlApproval
- ReportInquiry
- Settings

No seventh ScreenProfile may be introduced without evidence of a structural difference across layout, lifecycle, toolbar model, readonly model, and sizing behavior.

## Role separation
- Designers propose.
- Reviewers independently challenge.
- Domain specialists validate business rules.
- The General Supervisor resolves conflicts and issues the final integrated decision.
- The same role must not be the sole author and sole approver of a critical architectural decision.

## Change safety
- Never silently modify Frozen classifications, numbering, ownership, contracts, or database invariants.
- Never overwrite unrelated repository changes.
- Any material conflict between approved references must be escalated to the General Supervisor before implementation.

## Required evidence before completion
A work package is complete only when its required artifacts, traceability, tests/checks, and Gate criteria are satisfied and no unresolved Critical/High blocker remains for that Gate.

## Agent role files
Detailed role instructions live under `docs/agents/`.

## Governance workflow
Review and approval flow is defined in `docs/governance/REVIEW_WORKFLOW.md`.
