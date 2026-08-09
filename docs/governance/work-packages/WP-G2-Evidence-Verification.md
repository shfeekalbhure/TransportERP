# WP-G2-Evidence-Verification — TransportERP

## Identity
- **Work Package ID:** WP-G2-Evidence-Verification
- **Target Gate:** G2
- **Package lead:** GENERAL_SUPERVISOR
- **Scope:** Verification only; candidate evidence is not an approved closure.

## Objective
Verify exactly four prerequisites before any final rerun of WP-G2-Readiness-Review:
1. A successful current-branch CI run with both `dotnet build` and `dotnet test`.
2. Independent QA_TESTING_REVIEWER verification of implementation, test, and CI evidence.
3. Independent SOLUTION_ARCHITECT verification of the W1 baseline and recorded SHA-256 fingerprints.
4. An accessible authoritative Gap Closure Matrix reference containing the status of every G2 closure item.

## Non-negotiable constraints
- Do not change `G2 = NOT READY`.
- Do not close a G2 item merely because evidence has been submitted.
- Do not edit approved W1, W2, or W3 documents.
- If CI fails, record only the failure evidence and open a separate remediation item; do not fix unrelated code.
- GENERAL_SUPERVISOR alone may issue the final Gate decision, and only after a new run of WP-G2-Readiness-Review.

## Assigned reviews

| Item | Owner | Independent reviewer | Acceptance evidence |
|---|---|---|---|
| CI execution | RELEASE_INTEGRATION_REVIEWER | QA_TESTING_REVIEWER | GitHub Actions run URL, commit SHA, workflow/job result, and visible successful steps for `dotnet build` and `dotnet test`. |
| QA evidence review | QA_TESTING_REVIEWER | GENERAL_SUPERVISOR | Reproducible verification of candidate W2/W3 evidence against the CI run and current branch; explicit verdict per evidence item. |
| W1 baseline review | DATA_MYSQL_ARCHITECT | SOLUTION_ARCHITECT | Exact approved source references, SHA-256 comparison record, and conclusion on material undocumented delta. |
| Gap register traceability | GENERAL_SUPERVISOR | QA_TESTING_REVIEWER | Accessible stable Gap Closure Matrix reference listing every G2 closure item and its current verification status. |

## Completion rule
This Work Package records `VERIFIED`, `NOT VERIFIED`, or `BLOCKED` for the four prerequisites. It does not close G2. Only when all four are VERIFIED may GENERAL_SUPERVISOR commission a final, independent rerun of `WP-G2-Readiness-Review`.
