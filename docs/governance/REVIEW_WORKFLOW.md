# TransportERP Review Workflow

## Objective
Provide a repeatable governance flow for work performed by multiple assistants, specialists, reviewers, and implementers without creating contradictory sources of truth.

## Workflow

### 1. Intake
The General Supervisor identifies the objective, governing references, current Gate, and known open gaps.

### 2. Decomposition
The objective is split into bounded Work Packages. Each package has one primary owner and one independent reviewer where the decision is material.

### 3. Specialist analysis/design
The assigned specialist produces only the artifacts within its scope. Missing governing decisions become gaps; they are not guessed.

### 4. Independent review
A different role reviews the output for:
- correctness
- completeness
- conflicts with approved references
- duplicated architecture
- security/accounting/data risks
- testability
- Gate readiness

### 5. Conflict and gap resolution
Conflicts are classified as:
- Documentation conflict
- Architecture conflict
- Domain/business-rule conflict
- Contract conflict
- Implementation defect
- Open Technical Specification

The General Supervisor resolves only when the governing references support one answer. Otherwise the item remains explicitly open with an owner and Gate.

### 6. Integration
Only approved specialist outputs are merged into the integrated report/specification. Superseded artifacts remain historical and must be labeled accordingly.

### 7. Validation
Validation is proportional to the artifact:
- Documentation: traceability and cross-reference checks
- CoreUI/ScreenDefinition: reference implementations and UI/architecture tests
- API: contract/integration tests
- Data: model/constraint/migration tests
- Accounting: invariant and reconciliation tests
- Security: permission/scope/audit tests
- Release: build, regression, migration, security, and performance checks

### 8. Gate decision
The General Supervisor issues one status:
- READY
- READY WITH NON-BLOCKING NOTES
- NOT READY — BLOCKED

No Gate may be marked READY while a required Critical/High or Gate-bound gap is still open.

## Source-of-truth rules
- Approved reference register determines which versions are current.
- Gap Closure Matrix is the single source for open/closed technical gaps and their Gate ownership.
- Screen Classification/Profile decisions must not be duplicated in local screen files.
- Shared CoreUI definitions own shared controls, containers, properties, RTL behavior, sizing, toolbar bases, grids, validation, pagination, audit, and common states.
- ScreenDefinition owns screen-specific fields, columns, tabs, variants, capabilities, permissions, validation bindings, and documented local exceptions.

## ScreenDefinition inheritance order
`CoreUI Shared Definitions -> ScreenProfile Template -> Variant -> Capabilities -> ScreenDefinition -> Local Exception`

A lower level may specialize but must not silently redefine a higher-level invariant.

## Parallel-work rule
Work packages may proceed in parallel only when their dependencies do not require an unresolved output from one another. If two packages touch the same governing concept, the supervisor must designate a single owner before work starts.

## Completion package
Every coordinated review cycle ends with one integrated package containing:
- executive summary
- decisions
- artifact register changes
- conflict log
- gap status
- validation evidence
- next Gate/readiness
- next prioritized work packages
