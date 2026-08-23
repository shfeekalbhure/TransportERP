# BATCH-01 — FLOW01 Post-Pilot Design Batch

**Status:** ACTIVE
**Released:** 2026-08-24
**Design-only:** yes
**Official kurrasa modification:** no
**Application code/API/DDL modification:** no

## Batch rationale
The pilot `FLOW01-W3-SCR-001` reached `DESIGN_APPROVED`. The next batch contains the remaining FLOW01 identities that were explicitly eligible template shells and now have current typed ScreenDefinitions in the 2026-08-22 FLOW01 specification subpackage.

## Screens
| ScreenCode | Alias | ArabicName | Profile | Variant | CurrentStage |
|---|---|---|---|---|---|
| FLOW01-W3-SCR-002 | SHP-002 | تخصيص الشحنة | Transaction | Allocation | FIELD_GRID |
| FLOW01-W3-SCR-003 | SHP-003 | تتبع البوليصة | ReportInquiry | Inquiry | FIELD_GRID |
| FLOW01-W3-SCR-006 | WHS-003 | الترانزيت وتسليم الحيازة | Transaction | HeaderLines | FIELD_GRID |
| FLOW01-W3-SCR-007 | TRIP-001 | رحلة الشحن | Transaction | HeaderLines | FIELD_GRID |
| FLOW01-W3-SCR-008 | TRIP-002 | مانيفست الرحلة | Transaction | HeaderLines | FIELD_GRID |

## Common authority
- `CHG-20260818-FLOW01-W3-ID-002` canonical FLOW01 identity map.
- `FLOW01_W1_SCREEN_LEVEL_TRACE_2026-08-22.md`.
- `FLOW01_W2_EXACT_CONTRACT_AND_SECURITY_BINDING_2026-08-22.md`.
- Typed ScreenDefinition for each screen under `FLOW01_TYPED_SCREENDEFINITIONS_2026-08-22`.
- `ScreenDefinition_Contract_V1`.
- `CoreUI_Containers_and_Layout_Specification_V1.1`.
- `Transaction_Profile_Specification_V1.1` or `ReportInquiry_Profile_Specification_V1` as applicable.

## Layout reconciliation applied
Typed FLOW01 definitions may retain historical `Fixed/Content` shorthand. Shared current CoreUI/Profile architecture governs unless an approved LocalException exists.

- Transaction: `Header/MainData(Content) -> Tabs/Workspace(Fill) -> Lines/Grid(Fill)`.
- ReportInquiry: `Filters(Content) -> Summary(Content) -> ResultsGrid(Fill) -> Pagination(Fixed)`.
- No LocalException is created by this batch.

## Stage decision
`TEAM-D01 ANALYSIS = PASS` and `TEAM-D02 LAYOUT = PASS` for all five screens based on current typed definitions and current CoreUI/Profile authority.

`TEAM-D03 FIELD_GRID` is next. Any missing per-column metadata or lookup/provider binding is handled under the no-guess protocol: use `TBD-GATED` when nonblocking and `HOLD_AUTHORITY` only when the current stage cannot be completed safely.
