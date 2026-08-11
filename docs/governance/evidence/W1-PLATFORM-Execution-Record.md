# W1-PLATFORM Execution Record

## Identity and authority

| Field | Value |
|---|---|
| Work package | `W1-PLATFORM` |
| Authorized base SHA | `4bb32ee440f6b9164412bc469cfe22a4215ff110` |
| Implementation branch | `impl/w1-platform` |
| Start worktree status | clean |
| Owner authority | `AUTHORIZED WITH CONDITIONS` |

## Implemented shared contracts

- `TransportPresentationContext` and `TransportPresentationPolicy`: Arabic RTL and English LTR presentation direction, approved DPI design tokens, and dynamic visibility re-layout.
- `LookupPresentationContract` and `LookupPresentationItem`: source-neutral lookup presentation and selection only. It deliberately contains no API endpoint, database/entity, permission, cache, or offline policy.
- `InputValidationPresentation` and `InputValidationPresenter`: validation visual state and translation message key only; no business validation rule or hardcoded business label.
- `LookupComboBox.BindPresentationItems`: consumes caller-supplied presentation items under the shared contract without fetching or inferring a source.

No Desktop Form, business screen, API/DTO/contract, permission/authentication, DB/migration/entity/read model, offline/cache policy, accounting/business rule, ISO2, ISO3, or DialingCode is implemented or changed.

## Governing design to test traceability

The governing pack is an external frozen evidence artifact, not a file in this Git tree: `../outputs/WP-10-10-003/WP-10-10-003_FINAL_COMPLETION_PACK.zip`. Verified SHA-256: `3f65cf133e3cad9eb353cfedeab5476ae950e3b63d81692644a2fc47d1ef2e5c`. No section identifier is inferred beyond the file and row/table stated below.

| Governing design available in the verified external pack | Shared contract / component | Test |
|---|---|---|
| `WP-10-10-003 V1.3 Final Completion Pack` → `13_WAVE1_IMPLEMENTATION_WORK_PACKAGE_PLAN.md` → W1-PLATFORM: CoreUI, lookup, validation, localization presentation | `TransportPresentationContext`, `TransportPresentationPolicy` | `Presentation_context_supports_Arabic_Rtl_and_English_Ltr_at_each_approved_scale`; `Presentation_policy_applies_direction_recursively_and_preserves_lookup_direction_after_interaction`; `Presentation_context_rejects_an_unapproved_dpi_scale`; `Dynamic_visibility_relayouts_the_parent_without_hiding_other_required_content` |
| Same W1-PLATFORM row; `03_CENTRAL_LOOKUP_IMPLEMENTATION_CONTRACT.md` shared contract: `LookupId`, `LookupType`, `Context`, `SelectedId`, `AllowedFilters`, `SelectionMode`; no table/data owner/endpoint/permission/cache policy asserted | `LookupPresentationContract`, `LookupPresentationItem`, `LookupPresentationSelection`, `LookupComboBox.BindPresentationItems` | `Lookup_contract_describes_selection_without_a_data_source_or_authority`; `Lookup_contract_rejects_missing_presentation_identity`; `Lookup_combo_binds_caller_supplied_presentation_items_and_exposes_only_the_selection` |
| W1-PLATFORM row: validation/localization presentation; `10_WAVE1_TEST_DESIGN_AND_ACCEPTANCE_MATRIX.md`: RTL/LTR, DPI `100/125/150/200`, long text, validation/error, architecture/regression | `InputValidationPresentation`, `InputValidationPresenter` | `Validation_presentation_requires_a_field_and_a_validation_visual_state`; `Validation_presentation_preserves_the_active_presentation_direction` |
| `W3-CoreUI-Gap-Closure-Evidence.md`: shared CoreUI owns RTL, sizing and layout; individual reference screens do not duplicate common behavior | Central `TransportPresentationPolicy` in `CoreUI/Presentation` | `CoreUiArchitectureTests` regression suite and the W1-PLATFORM presentation tests |

## Required external Windows verification

This execution environment has no .NET SDK and is not Windows; it does not claim restore, build, test, runtime RTL/LTR, or DPI verification. Run the following on the final committed HEAD on Windows:

```powershell
dotnet restore TransportERP.slnx
dotnet build TransportERP.slnx --configuration Debug --no-restore
dotnet test TransportERP.Tests/TransportERP.Tests.csproj --configuration Debug --no-build
dotnet test TransportERP.Tests/TransportERP.Tests.csproj --configuration Debug --no-build --filter "FullyQualifiedName~W1PlatformContractTests"
dotnet test TransportERP.Tests/TransportERP.Tests.csproj --configuration Debug --no-build --filter "FullyQualifiedName~W1CoreContractTests|FullyQualifiedName~CoreUiArchitectureTests"
dotnet list TransportERP.slnx package --vulnerable --include-transitive
```

The UX reviewer must additionally exercise shared components at 100%, 125%, 150%, and 200% DPI in Arabic RTL and English LTR, with long text and dynamic visibility, recording no clipping, overlap, or hidden required content.

## Findings register

| Finding ID | Independent review | Status | Resolution / required next action |
|---|---|---|---|
| `QA-PLAT-001` | QA / Solution Architecture / UX | CLOSED | `LookupComboBox` no longer forces RTL during Enter/Leave; component-level RTL/LTR interaction test added in `46c16bd…`. |
| `QA-PLAT-002` | QA | CLOSED | Added direct tests for applying presentation direction, caller-supplied lookup selection, validation state, dynamic visibility, and long text presence in `46c16bd…`. |
| `QA-PLAT-004` | QA | CLOSED | Traceability now identifies the external frozen pack and its verified SHA-256. |
| `QA-PLAT-003` | QA | OPEN — external verification required | On the final pushed HEAD, run Windows Restore/Build/Full Tests/W1-PLATFORM/W1-CORE/W3, runtime RTL/LTR+DPI evidence, and dependency scan. This is not a code finding and cannot be closed in this non-Windows, no-.NET environment. |

Static independent conclusions for the current code: `SOLUTION_ARCHITECT = VERIFIED`, `UX_UI_REVIEWER = VERIFIED`, and `API_SECURITY_REVIEWER = VERIFIED`. QA confirms the code findings are closed while retaining `QA-PLAT-003` as the external evidence gate.

## Required final evidence

- Authorized base, merge-base, final HEAD, Base→Head commit list/diff, changed-files list, `git diff --check`, and clean-worktree proof.
- Restore, build, full-test, W1-PLATFORM, W1-CORE, W3 architecture, DPI/RTL-LTR, and vulnerability-scan evidence from Windows.
- Findings register and independent QA, solution architecture, UX/UI, and API/security reviews.
