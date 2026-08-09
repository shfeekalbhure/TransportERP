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
| Gap register traceability | VERIFIED | QA_TESTING_REVIEWER independently verified this fixed repository record at commit `41796420fba0ef93179ccae2f5fa4bcd2bd46c7a`; it contains all seven G2 closure items and matches its evidence-review findings. | None for this prerequisite. |

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


## Reverification register — 2026-08-09

**Review snapshot:** `setup/initial-solution-structure` @ `6d395271e80ce4a436ad9b8622038b507aed4eca`. The CI evidence below is the already-completed [Run 31286693059](https://github.com/shfeekalbhure/TransportERP/actions/runs/31286693059) on `b2e7c3f`; it was inspected, not rerun.

| G2 item | Owner | Role-review status | QA verdict | Executable evidence | Remaining action | G2 blocking |
|---|---|---|---|---|---|---|
| W1 Integrity | DATA_MYSQL_ARCHITECT | PASS WITH NOTES | INSUFFICIENT | [W1 baseline reference](W1-Approved-Baseline-Reference.md); no persistence/DDL/ORM or competing precision/UUID mapping was found at `6d395271`. | Make all six approved binary sources independently retrievable for QA rehash; when persistence begins, verify migrations/DDL/ORM against Money `DECIMAL(22,6)`, Rate `DECIMAL(20,10)`, Percentage `DECIMAL(12,8)`, and UUIDv7 `BINARY(16)` / swap=0. | YES |
| OTS-W2-001 | API_SECURITY_REVIEWER | NOT VERIFIED | NOT VERIFIED | `TransportERP.Api/Policies/RequestLimitPolicy.cs::NormalizePageSize` (`550b78e`) clamps to 500, not governing 200; policy-only test `ApiRequestPolicyTests::NormalizePageSize_ClampsValuesAboveTheHardMaximum` (`59f2316`). | Independent remediation WP: bind the governing 200 policy to an inbound API endpoint and add contract/integration evidence. | YES |
| OTS-W2-002 | API_SECURITY_REVIEWER | NOT VERIFIED | NOT VERIFIED | `RequestLimitPolicy::LimitLookup<T>` (`550b78e`) caps at 100, not governing 50; unit test only. | Independent remediation WP: bind 50-result limit and defined overflow behavior to a real Lookup endpoint/provider, then test it. | YES |
| OTS-W2-005 | API_SECURITY_REVIEWER | PARTIALLY VERIFIED | NOT VERIFIED | `Program.cs` (`2b346ea`) wires `TransportERP.SafeReadClient`; `SafeReadRetryHandler::SendAsync` (`553c313`) handles safe verbs. Policy uses 15s and 250/500ms rather than total 30s, attempt 10s, and delay 2s; no Retry-After, Idempotency-Key contract, or client consumer test. | Independent remediation WP: implement and test the governing transport contract on a real IApiClient/consumer, including unsafe-operation behavior. | YES |
| W3-IMP-001 | SCREEN_COREUI_ARCHITECT | CLOSED + VERIFIED | INSUFFICIENT | `TransportERP.Desktop/CoreUI/Controls/TransportReferenceScreenShell.cs` is actual WinForms code; `Views/Setup/Geographic/UcCountries.Designer.cs` instantiates it and `FrmDashboard.cs::OpenWorkspaceView` opens the screen. | Produce independently reproducible Windows runtime evidence for shared-shell behavior; QA then rechecks it. | YES |
| W3-IMP-002 | SCREEN_COREUI_ARCHITECT | PARTIALLY VERIFIED | NOT VERIFIED | `CoreUI/Architecture/ReferenceScreens.cs` and `CoreUiReferenceScreenCatalog.cs` define/map the six profile families, all via `CoreUiReferenceScreen`. | Independent remediation WP: add an actual display route or executable runtime test and produce Windows UX/RTL evidence for all six named screens. | YES |
| W3-IMP-003 | SCREEN_COREUI_ARCHITECT | CLOSED + VERIFIED | VERIFIED | `TransportERP.Tests/CoreUiArchitectureTests.cs` has completeness/missing/duplicate checks; Tests project references Desktop; `.github/workflows/build-validation.yml` executes it. CI Run 31286693059: 10/10 tests passed. | None. | NO |

### Independent verification failures / evidence gaps

| ID | Owner | Severity | Required action |
|---|---|---|---|
| IV-W1-001 | DATA_MYSQL_ARCHITECT | High | Provide immutable, independently readable W1 source artifacts so QA can recalculate the six hashes; do not modify approved W1 content. |
| IV-W2-001 | API_SECURITY_REVIEWER | Critical | Reconcile the actual code with approved page/lookup values and prove endpoint enforcement in a separate remediation package. |
| IV-W2-002 | API_SECURITY_REVIEWER | Critical | Implement and prove the approved HTTP resilience contract, including Retry-After and Idempotency-Key behavior, in a separate remediation package. |
| IV-W3-001 | SCREEN_COREUI_ARCHITECT | High | Supply reproducible Windows runtime/UX evidence for the shared shell and six named references in a separate remediation package. |

**Gate disposition:** no closure is recorded here. `G2 = NOT READY` is unchanged, and no final readiness review has been run.

## Current-state addendum — W1 Integrity evidence completion (2026-08-09)

This addendum supersedes only the historical W1 Integrity entries above. The
earlier tables remain an accurate record of their respective review snapshots.

| Item | Current status | Verifiable evidence | Remaining action |
|---|---|---|---|
| G2C-W1-BASELINE / W1 Integrity | **VERIFIED** | [W1 Evidence Completion Record](W1-Evidence-Completion-Record.md) at `b2174be2c4806bb1fc46a794d7ee95a618ab1258`; six immutable artifacts are byte-for-byte matched to their approved SHA-256 fingerprints. SOLUTION_ARCHITECT and QA_TESTING_REVIEWER each reported a separate clean-checkout reproduction. | None for W1 integrity. Verify physical persistence mapping only when a future DDL/ORM/migration package is introduced. |

### Current W1 disposition

- `W1 Integrity = VERIFIED`.
- No approved W1 artifact, logical model, ownership decision, constraint, or
  OTS decision was changed by the evidence package.
- This closure does not change the W2 or W3 rows, does not run a final G2
  readiness review, and does not change `G2 = NOT READY`.
