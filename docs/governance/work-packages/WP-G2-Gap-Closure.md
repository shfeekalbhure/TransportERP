# WP-G2-Gap-Closure — TransportERP

## Identity
- **Work Package ID:** `WP-G2-Gap-Closure`
- **Title:** `WP-G2-Gap-Closure — TransportERP`
- **Requested by:** Project Owner
- **Package lead and final integration owner:** `GENERAL_SUPERVISOR`
- **Target Gate:** G2
- **Priority:** Critical
- **Precondition:** `G2 = NOT READY` remains unchanged under `G2-Readiness-Review-Report.md`.

## Objective
Provide auditable, reproducible evidence that closes only the G2 evidence gaps identified by `WP-G2-Readiness-Review`. This package does not itself change the G2 decision. G2 may be reconsidered only after all acceptance evidence below is completed, independently verified, recorded in the authoritative Gap Closure Matrix, and `WP-G2-Readiness-Review` is run again.

## Scope

### In scope
1. Restore or permanently link the approved W1 baseline, its review record, and its Gap Closure Matrix.
2. Produce approved and executable closure evidence for W2:
   - `OTS-W2-001` MaximumPageSize
   - `OTS-W2-002` Lookup maximum result count
   - `OTS-W2-005` Retry / Backoff / Timeout policy
3. Produce executable W3 closure evidence for:
   - `W3-IMP-001` actual CoreUI operation
   - `W3-IMP-002` six complete CoreUI reference screens
   - `W3-IMP-003` architecture tests running in Build/CI
4. Update only the authoritative Gap Closure Matrix and evidence/implementation artifacts required to record valid closure.

### Out of scope
- Editing an approved/frozen W1, W2, or W3 specification solely to make a finding disappear.
- Changing W1 logical entities, relationships, ownership, constraints, precision, numbering, or physical decisions without a separate approved Change Request.
- Reclassifying frozen ScreenProfiles, Variants, Capabilities, or CoreUI rules.
- Changing G2 status or overwriting the existing G2 readiness report.
- Closing a gap with a narrative, screenshot, TODO, or unverified claim.
- Any scope not necessary to create the stated closure evidence.

## Governing references
- `AGENTS.md`
- `docs/governance/REVIEW_WORKFLOW.md`
- `docs/governance/WORK_PACKAGE_TEMPLATE.md`
- `docs/governance/work-packages/WP-G2-Readiness-Review.md`
- `docs/governance/reviews/G2-Readiness-Review-Report.md`
- Current Approved Reference Register.
- Authoritative Current Gap Closure Matrix.
- Current approved W1, W2, and W3 artifacts.
- Current approved execution plan and G2 definition.

Latest Approved Version Wins. The Gap Closure Matrix is the single operational source of closure state.

## Work items, ownership, and acceptance evidence

| ID | Closure item | Primary owner | Independent reviewer | Acceptance evidence required |
|---|---|---|---|---|
| G2C-W1-BASELINE | W1 approved baseline, review record, and gap matrix are accessible from the repository or immutable reachable references. | DATA_MYSQL_ARCHITECT | SOLUTION_ARCHITECT | Current Approved Reference Register identifies W1 files/versions and commit or immutable URLs; approved Logical Data Model, DB Constraint Matrix, Entity/Ownership Matrix, physical OTS register, and W1 review record are accessible; comparison against the reviewed branch records hashes/deltas; no silent material change remains, or each delta has a governed CR. |
| G2C-W2-001 | `MaximumPageSize` is specified, implemented, and tested. | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | Approved numeric default and hard maximum; server-side enforcement path/commit; request/error or clamping behavior; negative/limit test; current Build/CI evidence. |
| G2C-W2-002 | Lookup result limit is specified, implemented, and tested. | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | Approved numeric lookup cap; server-side implementation path/commit; defined request/response/overflow behavior; negative/limit test; current Build/CI evidence. |
| G2C-W2-005 | Retry, Backoff, and Timeout policy is specified, implemented, and tested. | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | Approved numeric timeout, retry-attempt, backoff and jitter policy; explicit safe/idempotent-only automatic retry rule; implementation/config path; mutation negative test; current Build/CI evidence. |
| G2C-W3-001 | CoreUI operates as the shared implementation, not documentation-only. | SCREEN_COREUI_ARCHITECT | QA_TESTING_REVIEWER | Exact repository paths and current commit for CoreUI classes; proof that declared shared behavior is owned by CoreUI; reproducible build/run or architecture-test evidence; QA can reproduce the result. |
| G2C-W3-002 | Six named reference screens use actual CoreUI, one for each approved profile family. | SCREEN_COREUI_ARCHITECT | UX_UI_REVIEWER | Traceability table covering MasterData, TreeMaster, Transaction, ControlApproval, ReportInquiry, Settings; each named screen compiles/runs using shared CoreUI; no duplicated shared behavior; profile/variant mapping and evidence for each screen; RTL/layout regression evidence. |
| G2C-W3-003 | Architecture tests execute and enforce CoreUI/ScreenDefinition rules in Build/CI. | SCREEN_COREUI_ARCHITECT | QA_TESTING_REVIEWER | Test-project paths and rule coverage; CI workflow invokes the tests (for example `dotnet test`); a current-branch passing run/job/log; a demonstrated failing-rule case or equivalent proof that violation fails validation. |

## Execution and review rules
1. Each owner may change only implementation, evidence, or the authoritative Gap Closure Matrix necessary for that item's valid closure.
2. The assigned independent reviewer must not author the item it verifies.
3. QA verifies reproducibility, current-branch relevance, negative paths, and CI/build proof; QA cannot waive a missing Critical/High evidence item.
4. GENERAL_SUPERVISOR resolves only evidence-supported disagreements and records unresolved items as open.
5. The final closure record for each item must contain: owner, reviewer, status, exact evidence paths/commit/run, validation result, impact, and remaining action.
6. Any conflict with a frozen W1/W2/W3 decision stops the affected item and requires a Change Request; do not silently edit the approved document.

## Required outputs
- W1 baseline accessibility and comparison record.
- W2 policy, implementation, test, and CI evidence for all three W2 items.
- W3 implementation/traceability/test/CI evidence for all three W3 items.
- Authoritative Gap Closure Matrix updates with the closure evidence references.
- Independent review records for all seven items.
- A General Supervisor closure summary that lists each item as `CLOSED`, `OPEN`, `PARTIALLY CLOSED`, or `EVIDENCE INSUFFICIENT`.

## Acceptance criteria
1. All seven items have a primary-owner result and an independent-review result.
2. Every claimed closure has stable paths/URLs, current commit or immutable identifiers, and reproducible validation evidence.
3. All W2 policy values are numerical and server-side enforceable; no generic wording substitutes for a limit.
4. All six reference-screen profiles are explicitly named and evidenced.
5. Architecture tests run automatically in Build/CI and have current successful evidence.
6. The Current Approved Reference Register and authoritative Gap Closure Matrix are accessible and traceable.
7. No approved W1/W2/W3 document is silently changed.
8. The Gap Closure Matrix is updated before any closure is presented as final.
9. G2 remains `NOT READY` throughout this package. Only a new, independent execution of `WP-G2-Readiness-Review` may issue a new G2 decision.

## General Supervisor disposition
- **Current G2 state:** `G2 = NOT READY`
- **This package may:** create closure evidence and coordinate independent review.
- **This package may not:** change the G2 state.
- **Next mandatory step after every item is closed and independently verified:** run `WP-G2-Readiness-Review` again on the resulting branch state.
