# W3 CoreUI Gap-Closure Evidence

## Scope and safeguards
- Work package: `WP-G2-Gap-Closure`
- Items: `G2C-W3-001`, `G2C-W3-002`, `G2C-W3-003`
- Primary owner: `SCREEN_COREUI_ARCHITECT`
- Independent reviewers: `QA_TESTING_REVIEWER` (W3-001/W3-003) and `UX_UI_REVIEWER` (W3-002)
- This evidence artifact does not change W1, W2, W3, or the G2 decision.

## G2C-W3-001 — actual shared CoreUI
The existing executable shell is `TransportERP.Desktop/CoreUI/Controls/TransportReferenceScreenShell.cs`.
The executable reference layer is:
- `TransportERP.Desktop/CoreUI/Architecture/CoreUiReferenceScreen.cs`
- `TransportERP.Desktop/CoreUI/Architecture/TransportScreenProfile.cs`

`CoreUiReferenceScreen` creates and hosts `TransportReferenceScreenShell`; it does not duplicate shared toolbar, search, grid, audit, RTL, sizing, or layout behavior. Its only per-reference declaration is the frozen profile identity and screen title.

## G2C-W3-002 — six profile references
| Profile | Concrete CoreUI reference |
|---|---|
| MasterData | `MasterDataReferenceScreen` |
| TreeMaster | `TreeMasterReferenceScreen` |
| Transaction | `TransactionReferenceScreen` |
| ControlApproval | `ControlApprovalReferenceScreen` |
| ReportInquiry | `ReportInquiryReferenceScreen` |
| Settings | `SettingsReferenceScreen` |

All references are declared in `TransportERP.Desktop/CoreUI/Architecture/ReferenceScreens.cs`, inherit `CoreUiReferenceScreen`, and thereby host the same RTL CoreUI shell. The catalog at `CoreUiReferenceScreenCatalog.cs` enforces one distinct concrete reference for every frozen profile.

## G2C-W3-003 — executable architecture checks
`TransportERP.Tests/CoreUiArchitectureTests.cs`:
1. validates the complete six-profile map;
2. proves a missing profile fails validation;
3. proves a duplicate reference fails validation.

The branch workflow `.github/workflows/build-validation.yml` must run:
`dotnet test TransportERP.Tests/TransportERP.Tests.csproj --configuration Debug --no-build`.

## Independent verification record
- QA reviewer must confirm the workflow run for the resulting commit is successful and inspect the three passing tests.
- UX reviewer must inspect each reference at runtime on Windows and confirm Arabic RTL, shared shell usage, and no duplicated common behavior.
- Until those independent checks are recorded in the Gap Closure Matrix, this evidence is not a gate decision and `G2 = NOT READY` remains unchanged.
