# WP-G2-Readiness-Review — TransportERP

## Identity
- Work Package ID: `WP-G2-Readiness-Review`
- Title: `WP-G2-Readiness-Review — TransportERP`
- Requested by: Project Owner
- General Supervisor: `GENERAL_SUPERVISOR`
- Target Gate: `G2 — Shared Foundation Ready`
- Priority: Critical / Gate Readiness

## Objective
Review the current approved W1, W2, and W3 outputs and determine the actual readiness of TransportERP for G2 without modifying any approved W1/W2/W3 artifact and without implementing code, SQL, migrations, API endpoints, or UI behavior.

The review is evidence-based only. Documentation statements are not sufficient when the gap requires executable or repository evidence.

## Scope
### In scope
- Verify W1 integrity and confirm no silent changes to the approved logical data model or related ownership/constraint decisions.
- Verify closure status and evidence for the three W2 G2-bound gaps:
  - `OTS-W2-001 — MaximumPageSize`
  - `OTS-W2-002 — Lookup maximum result count`
  - `OTS-W2-005 — Retry/Backoff/Timeout numeric policy`
- Verify closure status and evidence for the three W3 G2-bound implementation gaps:
  - `W3-IMP-001 — Actual CoreUI classes in repository`
  - `W3-IMP-002 — Six reference screens running on real CoreUI`
  - `W3-IMP-003 — Architecture tests active in Build/CI`
- Verify that evidence for every G2-bound gap is reviewable, attributable, and consistent with current approved references.
- Produce one integrated G2 readiness report.

### Out of scope
- Modifying W1, W2, or W3 approved documents.
- Changing the Logical Data Model, DB Constraint Matrix, API Contract Matrix, Permission Matrix, ScreenDefinition contracts, CoreUI specifications, or Gap Closure Matrix.
- Implementing or modifying code.
- Writing SQL, DDL, migrations, or database scripts.
- Creating or changing API endpoints.
- Creating or changing Forms, controls, ScreenDefinitions, or CoreUI behavior.
- Closing any gap merely by declaration without verifiable evidence.
- Reclassifying ScreenProfiles, Variants, Capabilities, ownership, numbering, or frozen architecture.

## Governing references
Use only CURRENT APPROVED references and directly relevant CURRENT SUPPORT evidence, including:
- `AGENTS.md`
- `docs/agents/GENERAL_SUPERVISOR.md`
- `docs/agents/DATA_MYSQL_ARCHITECT.md`
- `docs/agents/API_SECURITY_REVIEWER.md`
- `docs/agents/SCREEN_COREUI_ARCHITECT.md`
- `docs/agents/QA_TESTING_REVIEWER.md`
- `docs/agents/TEAM_ROSTER.md`
- `docs/governance/REVIEW_WORKFLOW.md`
- Current Approved Reference Register
- Current Gap Closure Matrix for W1/W2/W3
- Current approved W1 artifacts
- Current approved W2 V1.1 artifacts and review report
- Current approved W3 CoreUI / ScreenDefinition artifacts
- Repository/build/CI/test evidence relevant to G2

SUPERSEDED documents may be used only for historical traceability and must not drive the readiness decision.

## Assigned roles

### 1. DATA_MYSQL_ARCHITECT
**Responsibility:** W1 integrity review.

Must verify:
- W1 approved Logical Data Model remains internally consistent.
- Entity ownership and relationship decisions remain consistent with approved references.
- No silent or undocumented change has altered the approved data model, constraints, ownership, or physical-decision boundaries.
- Open physical OTS items are not falsely represented as closed.

Required output:
- W1 integrity status: `PASS / PASS WITH NOTES / FAIL`.
- List of any detected silent change or mismatch.
- Evidence references.
- Gate impact.

This role must not edit W1.

### 2. API_SECURITY_REVIEWER
**Responsibility:** W2 G2-bound gap verification.

Must verify each independently:

#### OTS-W2-001 — MaximumPageSize
Evidence must show an approved, unambiguous maximum page-size policy/default suitable for production shared paging behavior.

#### OTS-W2-002 — Lookup maximum result count
Evidence must show an approved, bounded server-side lookup result cap/default suitable for the shared LookupProvider behavior.

#### OTS-W2-005 — Retry/Backoff/Timeout numeric policy
Evidence must show approved numeric timeout/retry/backoff defaults and safe-retry rules, including that automatic retry is restricted to safe/idempotent operations according to policy.

For each gap report:
- Owner
- Status: `CLOSED / OPEN / PARTIALLY CLOSED / EVIDENCE INSUFFICIENT`
- Evidence
- Impact
- Remaining action
- G2 blocking: `YES / NO`

This role must not modify W2 contracts or matrices.

### 3. SCREEN_COREUI_ARCHITECT
**Responsibility:** W3 G2-bound implementation verification.

Must verify each independently:

#### W3-IMP-001 — Actual CoreUI classes in repository
Evidence must demonstrate real implementation in the repository, not documentation-only completion.

#### W3-IMP-002 — Six reference screens running on real CoreUI
Evidence must demonstrate six reference implementations, one for each approved ScreenProfile family where required by the G2 contract, running on actual shared CoreUI without duplicated shared behavior.

Approved profile families:
- MasterData
- TreeMaster
- Transaction
- ControlApproval
- ReportInquiry
- Settings

#### W3-IMP-003 — Architecture tests active in Build/CI
Evidence must demonstrate architecture tests are implemented and actually participate in build/CI validation, rather than existing only as specifications.

For each gap report:
- Owner
- Status: `CLOSED / OPEN / PARTIALLY CLOSED / EVIDENCE INSUFFICIENT`
- Evidence
- Impact
- Remaining action
- G2 blocking: `YES / NO`

This role must not implement CoreUI or edit W3 specifications.

### 4. QA_TESTING_REVIEWER
**Responsibility:** Independent evidence verification.

Must review evidence supplied by the other roles and independently confirm:
- Evidence is reproducible or reviewable.
- Repository paths, commits, build output, test output, CI runs, or approved decision records actually support the claimed status.
- Documentation completion is not being substituted for implementation evidence.
- Each of the six G2-bound gaps has a clear verification trail.
- Any test/build/CI evidence is current enough to apply to the reviewed branch/state.

Required output:
- Evidence verdict per gap: `VERIFIED / NOT VERIFIED / INSUFFICIENT`.
- Missing evidence list.
- Defects or inconsistencies with severity.
- Independent G2 readiness recommendation.

QA must not waive an open or unverified G2-bound blocker.

### 5. GENERAL_SUPERVISOR
**Responsibility:** Integration and final Gate disposition.

Must:
- Collect all specialist results.
- Resolve only evidence-supported disagreements.
- Keep unresolved contradictions explicitly open.
- Produce one integrated report.
- Not claim that separate runtime agents executed unless the environment actually invoked them; otherwise identify results as structured role reviews.

## Dependencies
- Current Approved Reference Register is accessible.
- Current Gap Closure Matrix is accessible.
- Current approved W1/W2/W3 artifacts are accessible.
- Repository state for `setup/initial-solution-structure` is accessible.
- Build/CI/test evidence, if claimed, is accessible for verification.

## Required outputs

### A. Specialist review results
One bounded review result from each assigned specialist role.

### B. Unified G2 readiness report
The General Supervisor must produce a table with at least these columns:

| Gap / Review Item | Owner | Status | Evidence | Impact | Remaining Action | G2 Blocking |
|---|---|---|---|---|---|---|
| W1 Integrity | DATA_MYSQL_ARCHITECT |  |  |  |  |  |
| OTS-W2-001 MaximumPageSize | API_SECURITY_REVIEWER |  |  |  |  |  |
| OTS-W2-002 Lookup Result Cap | API_SECURITY_REVIEWER |  |  |  |  |  |
| OTS-W2-005 Retry/Backoff/Timeout | API_SECURITY_REVIEWER |  |  |  |  |  |
| W3-IMP-001 Actual CoreUI | SCREEN_COREUI_ARCHITECT |  |  |  |  |  |
| W3-IMP-002 Six Reference Screens | SCREEN_COREUI_ARCHITECT |  |  |  |  |  |
| W3-IMP-003 Architecture Tests in Build/CI | SCREEN_COREUI_ARCHITECT |  |  |  |  |  |

The report must also include QA's verification verdict for every one of the six G2-bound gaps.

### C. Final Gate decision
Exactly one of:
- `G2 = READY`
- `G2 = NOT READY`

No intermediate wording may override the binary Gate result.

## Acceptance criteria
1. W1 has been reviewed for integrity and silent-change risk without modification.
2. All three W2 G2-bound gaps have explicit status and evidence.
3. All three W3 G2-bound implementation gaps have explicit status and evidence.
4. QA independently verifies the evidence for all six G2-bound gaps.
5. Every finding states owner, status, evidence, impact, remaining action, and blocking effect.
6. No approved W1/W2/W3 artifact is modified by this Work Package.
7. No code, SQL, migration, endpoint, Form, ScreenDefinition, or CoreUI implementation is performed.
8. `G2 = READY` is allowed only when all six G2-bound gaps are actually CLOSED and their evidence is VERIFIED by QA, and W1 integrity review has no G2-blocking defect.
9. If any one of the six G2-bound gaps is OPEN, PARTIALLY CLOSED, EVIDENCE INSUFFICIENT, or NOT VERIFIED, the mandatory final result is `G2 = NOT READY`.
10. Any detected silent change to W1 that materially affects G2 also forces `G2 = NOT READY` until resolved through governance.

## Evidence rules
Acceptable evidence may include, as appropriate:
- Current approved decision/specification with explicit numeric/default policy.
- Repository file/path and commit SHA.
- Build output.
- Test output.
- CI workflow run/job evidence.
- Architecture-test results.
- Reference-screen implementation paths and runtime/build verification.

Unacceptable as sole evidence:
- A conversation statement.
- A TODO marked done without implementation proof.
- A superseded document.
- An unverified screenshot with no repository/build/test traceability.
- A claim that code exists without a repository path/commit or equivalent reviewable artifact.

## Review sequence
1. General Supervisor confirms governing references and current branch/state.
2. DATA_MYSQL_ARCHITECT performs W1 integrity review.
3. API_SECURITY_REVIEWER verifies the three W2 gaps.
4. SCREEN_COREUI_ARCHITECT verifies the three W3 gaps.
5. QA_TESTING_REVIEWER independently verifies all supplied evidence.
6. General Supervisor reconciles findings and produces the unified report.
7. General Supervisor issues the binary G2 disposition.

Steps 2–4 may run in parallel if the execution environment supports actual parallel agents; otherwise they may be performed sequentially as independent role reviews. Step 5 must remain independent from the original specialist conclusions.

## General Supervisor disposition rule
The General Supervisor must apply this rule mechanically:

```text
IF
  W1 integrity has no G2-blocking defect
  AND OTS-W2-001 = CLOSED + QA VERIFIED
  AND OTS-W2-002 = CLOSED + QA VERIFIED
  AND OTS-W2-005 = CLOSED + QA VERIFIED
  AND W3-IMP-001 = CLOSED + QA VERIFIED
  AND W3-IMP-002 = CLOSED + QA VERIFIED
  AND W3-IMP-003 = CLOSED + QA VERIFIED
THEN
  G2 = READY
ELSE
  G2 = NOT READY
```

## Change control
This Work Package authorizes review and reporting only. Any remediation discovered by the review requires a separate Work Package or approved change path with the correct owner and Gate.
