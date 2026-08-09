# G2 Gap Closure Verification Matrix — TransportERP

## Authority and scope
- **Record ID:** G2-Gap-Closure-Verification-Matrix
- **Governing work package:** `WP-G2-Evidence-Verification`
- **Operational owner:** GENERAL_SUPERVISOR
- **Independent matrix reviewer:** QA_TESTING_REVIEWER
- **Bound branch and evidence commit:** `setup/initial-solution-structure` @ `b2e7c3ff362f1f1910a0b7797d853bca97d44662`
- **Gate state:** `G2 = NOT READY` — unchanged.
- **Decision rule:** This is the operational closure-state register for the seven items in `WP-G2-Gap-Closure`. It records verification findings only; it neither changes approved W1/W2/W3 documents nor grants a Gate decision. GENERAL_SUPERVISOR alone may commission the final independent readiness review.

## Stable governing references
- [WP-G2-Gap-Closure](../work-packages/WP-G2-Gap-Closure.md)
- [WP-G2-Evidence-Verification](../work-packages/WP-G2-Evidence-Verification.md)
- [W1 approved baseline reference](W1-Approved-Baseline-Reference.md)
- [CI workflow](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/.github/workflows/build-validation.yml)
- [CI run 31286693059](https://github.com/shfeekalbhure/TransportERP/actions/runs/31286693059)

The W1 source register is `Current Approved References V1.17`; the approved W1 artifacts and six SHA-256 values are linked from the W1 baseline reference above. This matrix is the fixed, repository-reviewable G2 operational register; it does not replace the underlying approved W1 source set.

## Evidence-verification prerequisites

| Prerequisite | Status | Evidence / finding | Remaining action |
|---|---|---|---|
| CI execution | VERIFIED | Run `31286693059`, job `build`, completed successfully. Steps **Build** and **Test request policy and CoreUI architecture enforcement** both succeeded. The workflow invokes `dotnet build TransportERP.slnx --configuration Debug --no-restore` and `dotnet test TransportERP.Tests/TransportERP.Tests.csproj --configuration Debug --no-build`. | None for this prerequisite. Track the separate `NU1903` dependency warning as a risk. |
| Independent QA evidence review | PARTIALLY VERIFIED | QA_TESTING_REVIEWER independently reviewed current W2/W3 code, tests, and CI. It verified CI and W3 architecture tests, and identified missing API integration and Windows UX/runtime evidence. | Complete the listed independent evidence and re-review. |
| Independent W1 baseline review | VERIFIED AS REFERENCE | SOLUTION_ARCHITECT independently retrieved the six approved W1 sources and matched all recorded SHA-256 fingerprints. | DATA_MYSQL_ARCHITECT must still compare the reviewed branch against that baseline to establish no silent material data-model delta. |
| Gap register traceability | PENDING INDEPENDENT REVIEW | This immutable repository record now lists every G2 closure item and its current state. | QA_TESTING_REVIEWER reviews this record in a subsequent verification pass. |

## Closure-item register

| ID | Owner | Independent reviewer | Current status | Evidence reviewed | Impact / remaining action |
|---|---|---|---|---|---|
| G2C-W1-BASELINE | DATA_MYSQL_ARCHITECT | SOLUTION_ARCHITECT | PARTIALLY VERIFIED | Six Library-approved W1 sources were located and their SHA-256 fingerprints matched [W1 baseline reference](W1-Approved-Baseline-Reference.md). | No silent-change conclusion yet: DATA_MYSQL_ARCHITECT must perform and record the branch-to-baseline comparison. |
| G2C-W2-001 MaximumPageSize | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | NOT VERIFIED | [RequestLimitPolicy](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Api/Policies/RequestLimitPolicy.cs) defines default 100, hard cap 500, and rejects non-positive values; [unit tests](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Tests/ApiRequestPolicyTests.cs) ran in the successful CI. | QA found no proof of enforcement on an actual inbound API/controller path. Add an integration-level proof and re-review. |
| G2C-W2-002 Lookup result cap | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | NOT VERIFIED | The same policy defines lookup cap 100 and unit tests cover the limit. | No actual Lookup endpoint/provider path or documented overflow response was evidenced. Add integration/contract evidence and re-review. |
| G2C-W2-005 Retry/Backoff/Timeout | API_SECURITY_REVIEWER | QA_TESTING_REVIEWER | PARTIALLY VERIFIED | Numeric policy and named client wiring are present in [Program.cs](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Api/Program.cs); policy tests ran in CI. | QA needs executable handler/timeout evidence including no automatic retry for POST/PUT. |
| G2C-W3-001 Actual CoreUI | SCREEN_COREUI_ARCHITECT | QA_TESTING_REVIEWER | PARTIALLY VERIFIED | [CoreUiReferenceScreen](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Desktop/CoreUI/Architecture/CoreUiReferenceScreen.cs) hosts `TransportReferenceScreenShell`; CI builds it. | Windows runtime/visual reproducibility evidence remains absent. |
| G2C-W3-002 Six CoreUI reference screens | SCREEN_COREUI_ARCHITECT | UX_UI_REVIEWER | NOT VERIFIED | [ReferenceScreens](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Desktop/CoreUI/Architecture/ReferenceScreens.cs) and its catalog name all six profile families; architecture tests reject missing/duplicate profiles. | Requires independent UX runtime review on Windows for each screen, RTL/layout, and shared-CoreUI use. |
| G2C-W3-003 Architecture tests in CI | SCREEN_COREUI_ARCHITECT | QA_TESTING_REVIEWER | VERIFIED | [CoreUiArchitectureTests](https://github.com/shfeekalbhure/TransportERP/blob/b2e7c3ff362f1f1910a0b7797d853bca97d44662/TransportERP.Tests/CoreUiArchitectureTests.cs) executes in the passing CI test step and covers positive, missing-profile, and duplicate-profile conditions. | None for this item. |

## Independent-review records
- **QA_TESTING_REVIEWER:** `PARTIALLY VERIFIED`. CI succeeded; W2 inbound enforcement and retry integration evidence, and W3 Windows UX/runtime evidence, remain insufficient.
- **SOLUTION_ARCHITECT:** `VERIFIED AS REFERENCE`. The six approved W1 artifact hashes match; no data-model delta conclusion was made.
- **GENERAL_SUPERVISOR disposition:** no closure is declared by this record. G2 remains `NOT READY`.

## Prohibited inference
A passing CI run proves the commands and their included tests succeeded. It does not, by itself, prove runtime endpoint enforcement, runtime retry behavior, or Windows UX/RTL behavior. No item marked other than `VERIFIED` may be represented as closed.
