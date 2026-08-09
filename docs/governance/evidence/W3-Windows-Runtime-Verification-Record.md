# W3 Windows Runtime Verification Record

## Scope and immutable decision

- Work package: `WP-G2-W3-Windows-Runtime-Verification`
- Reviewed snapshot: `setup/initial-solution-structure` at `7b4a4b4868f4d1554046d91ec8121af9fcece088`
- Owner: `SCREEN_COREUI_ARCHITECT`
- Required independent reviewer: `QA_TESTING_REVIEWER`
- `G2 = NOT READY` is unchanged.
- `W3-IMP-003` is excluded: it remains `CLOSED + VERIFIED` under the retained CI evidence.

## Execution-environment result

| Check | Result | Reproducible evidence |
|---|---|---|
| Host OS | BLOCKED | `uname -a` reports Linux `6.18.35`; the Desktop project targets `net10.0-windows` and uses Windows Forms. |
| .NET SDK | BLOCKED | `dotnet --info` returns `dotnet: command not found`. |
| Windows runtime artifact | NOT AVAILABLE | Repository contains no `bin/`, `.exe`, runtime screenshot, recording, or Windows execution log. |
| CI evidence for reviewed snapshot | NOT AVAILABLE | Retained successful run `31286693059` is for `b2e7c3f`, not the reviewed W3/W2 snapshot. It proves build/test policy execution only, not a manual visual/runtime verification of all six reference screens. |

This record deliberately makes **no claim** that a reference screen was opened, rendered, passed DPI testing, or exhibited the required shared behaviors.

## Static traceability — not accepted as runtime proof

| Profile | Concrete type | Source path | Shared CoreUI route declared in code | Runtime/QA verdict |
|---|---|---|---|---|
| MasterData | `MasterDataReferenceScreen` | `TransportERP.Desktop/CoreUI/Architecture/ReferenceScreens.cs` | `CoreUiReferenceScreen` → `TransportReferenceScreenShell` | INSUFFICIENT |
| TreeMaster | `TreeMasterReferenceScreen` | same | same | INSUFFICIENT |
| Transaction | `TransactionReferenceScreen` | same | same | INSUFFICIENT |
| ControlApproval | `ControlApprovalReferenceScreen` | same | same | INSUFFICIENT |
| ReportInquiry | `ReportInquiryReferenceScreen` | same | same | INSUFFICIENT |
| Settings | `SettingsReferenceScreen` | same | same | INSUFFICIENT |

The static mapping is enforced by `TransportERP.Desktop/CoreUI/Architecture/CoreUiReferenceScreenCatalog.cs` and architecture tests in `TransportERP.Tests/CoreUiArchitectureTests.cs`, but those tests do not instantiate a WinForms application, display the six types, inspect layout, or exercise UI behavior.

## Findings requiring separate remediation or evidence collection

| ID | Severity | Owner | Finding | Evidence | Required action |
|---|---|---|---|---|---|
| W3-RT-001 | High | SCREEN_COREUI_ARCHITECT | No Windows runtime environment/artifact exists to perform the required visual and interaction verification. | Desktop target in `TransportERP.Desktop/TransportERP.Desktop.csproj`; no `dotnet` on Linux host. | Run a dedicated Windows verification job or manual signed artifact test on the exact commit; preserve launch logs and screenshots/video. |
| W3-RT-002 | High | SCREEN_COREUI_ARCHITECT | The six reference types have no application launch route. `Program.cs` opens `FrmLogin`; grep finds the reference types only in the catalog/tests/definitions, not in `FrmLogin` or `FrmDashboard`. | `TransportERP.Desktop/Program.cs`, `TransportERP.Desktop/Forms/FrmDashboard.cs`, `CoreUiReferenceScreenCatalog.cs`. | Create a separately approved remediation package that supplies a non-production verification host/route for the six existing references, then test it on Windows. |
| W3-RT-003 | Medium | SOLUTION_ARCHITECT | Two distinct `TransportScreenProfile` enums coexist: the frozen reference enum uses `TreeMaster`/`ControlApproval`/`ReportInquiry`; `CoreUI/Profiles/TransportScreenProfiles.cs` uses `TabbedMaster`/`ReadOnlyLog`/`Tree` and lacks those identities. This prevents a verified assertion that each reference applies the intended profile-specific policy. | `CoreUI/Architecture/TransportScreenProfile.cs`; `CoreUI/Profiles/TransportScreenProfiles.cs`. | Architecture decision/Change Request required before reconciliation; do not change a frozen profile in verification work. |

## W3 item result

| Item | Implementation observation | Evidence-verification status | QA-ready disposition |
|---|---|---|---|
| W3-IMP-001 — actual CoreUI use | `CoreUiReferenceScreen` constructs `TransportReferenceScreenShell`, which centrally owns shell controls, RTL and sizing code. Existing production `UcCountries` also uses this shell. | PARTIALLY VERIFIED (static only) | INSUFFICIENT until Windows runtime evidence and QA review exist. |
| W3-IMP-002 — six runtime references | Six concrete types are mapped one-to-one in the catalog, but none is launched by the application. | NOT VERIFIED | NOT VERIFIED; blocked by W3-RT-001 and W3-RT-002. |

## QA review handoff

The QA_TESTING_REVIEWER must reproduce the Windows run at the committed remediation/evidence snapshot. QA may mark a screen `VERIFIED` only after reviewing the runtime artifact, launch route, visual RTL/order/sizing proof, and relevant interactive behavior. Until then, this record's values remain `INSUFFICIENT` and `NOT VERIFIED`.
