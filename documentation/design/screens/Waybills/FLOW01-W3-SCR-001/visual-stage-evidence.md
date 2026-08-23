# FLOW01-W3-SCR-001 — TEAM-D05 Visual Stage Evidence

Status: PASS
Stage: VISUAL
Owner: TEAM-D05
Date: 2026-08-24

## Authority used
- Canonical `screen-spec.md` and completed UX evidence.
- `CoreUI_Properties_Specification_V1.4`.
- `Transaction_Profile_Specification_V1.1`.
- `CoreUI_Controls_Catalog_V1.2`.
- `CoreUI_Architecture_Tests_Specification_V1.2`.
- Project-owner layout and FIELD_GRID decisions already recorded for this screen.

No raw visual value is invented by this screen. Shared values are references to CoreUI tokens only.

## Visual composition
```text
RTL Transaction / HeaderLines
┌──────────────────────────────────────────────────────────────┐
│ Shared TransportToolbar / shell                              │
├──────────────────────────────────────────────────────────────┤
│ Header / MainData — Content                                  │
│ [الحالة: ReadOnly] [رقم البوليصة: ReadOnly/server assigned] │
│ [العميل: Lookup Required] [المصدر: Lookup Required]          │
│ [الوجهة: Lookup Required]                                    │
├──────────────────────────────────────────────────────────────┤
│ Tabs / Workspace — Fill                                      │
│ عام | البنود | الطرود | المراحل | التدقيق                    │
│                                                              │
│ Active tab content owns remaining workspace                  │
│ Items/Packages/Legs use TransportDataGrid                     │
├──────────────────────────────────────────────────────────────┤
│ Shared status / validation / action / audit presenters       │
└──────────────────────────────────────────────────────────────┘
```

The diagram is composition evidence, not a pixel mockup.

## CoreUI token application
- `RightToLeft = Yes` and supported hosts use `RightToLeftLayout = True`.
- Shell-hosted screen uses the central shell start/docking policy; no local StartPosition or arbitrary MinimumSize.
- Screen padding, container gap, label-field gap, row gap and section gap come from `CoreUISpacing`.
- Standard field/control height comes from `CoreUISizing`; no local field-height override.
- Toolbar, tab header, pagination, audit and minimum-grid sizing remain central and DPI-scaled.
- MainData uses at most two columns under the current CoreUI rule; no third data-entry column is created.

## Typography
- Use the central System UI / Segoe UI baseline token with Arabic-capable fallback.
- Labels and inputs use the shared default font roles.
- Grid uses shared grid typography; headers use the centrally defined emphasis.
- Selected tabs and section/header emphasis come from CoreUI typography tokens.
- No per-screen font family, font size, bold rule, or text rendering override.

## Semantic visual states
Fields and controls use only the shared states:
- Normal.
- Required: required semantic background/marker; not treated as an error merely because it is required.
- ReadOnly: neutral read-only presentation and no edit affordance.
- Disabled: central disabled palette and interaction state.
- Error: semantic error border/message through `TransportValidationPresenter`.
- Focused: central visible focus cue without replacing the field's semantic state.

`shipmentId` and `shipmentState` render as ReadOnly. Draft-editable required references render with the shared Required state until populated/validated. Non-Draft lifecycle makes Draft-only controls ReadOnly/Disabled according to the shared control/profile behavior.

## Grid visual contract
All three line grids use `TransportDataGrid` + `GridProfile=TransactionLines`:
- central grid header/row styling;
- central row height;
- RTL rendering;
- visible focus/edit cues;
- `AutoGenerateColumns=false`;
- single-row selection;
- editable cells only when the screen/field contract allows Draft editing;
- compact numeric/reference/state columns use CoreUI content sizing;
- descriptive text receives fill priority where declared by FIELD_GRID;
- no fixed pixel column widths are introduced by the screen.

Items paging uses `TransportPagination` when server paging is active; the screen creates no local pagination controls.

## Tabs and workspace
- Tabs are rendered by the governed tab host with RTL/right-origin ordering.
- Tab padding, selected state, typography and header height come from CoreUI.
- Workspace remains `Fill`.
- No nested MainData scroll; the workspace/grid is the fill/scroll owner.
- No decorative/empty tab is added.

## Toolbar / commands
- Command rendering/order/icon-text/hover/disabled states are central.
- Capabilities and permission mappings control whether commands are visible/enabled.
- The screen does not locally restyle or reorder toolbar buttons.
- During loading, shared loading state visually disables conflicting commands according to the UX evidence.

## Validation, alerts, loading, empty and error states
- `TransportValidationPresenter`, `TransportAlerts`, `TransportLoadingState`, `TransportEmptyState`, and `TransportErrorState` are used as shared visual presenters.
- No local MessageBox validation path.
- CorrelationId appears only in technical/support details when applicable.
- Concurrency conflict uses the shared conflict/error presentation with Refresh/Reload affordance.

## Accessibility / DPI / RTL
- Logical-pixel tokens scale with per-monitor DPI policy.
- Arabic labels/controls must not clip at 100%, 125%, 150%, or 200% reference regression scales.
- Tab/focus order follows RTL visual flow.
- Focus cues remain visible.
- AccessibleName/AccessibleDescription are generated from ScreenDefinition/field metadata where supported.
- Numeric values preserve correct digit semantics while the overall screen remains RTL.

## Explicit visual non-inventions
TEAM-D05 does not create:
- raw colors;
- per-screen fonts;
- local control heights or margins;
- local toolbar/grid/tab/pagination/audit styling;
- fixed screen or grid sizes that conflict with Content/Fill behavior;
- custom error/loading/validation presenters;
- a LocalException.

## Visual acceptance checks
1. Shared styles are referenced through CoreUI only.
2. Header/MainData remains Content and workspace/grids remain Fill.
3. MainData does not exceed the central two-column limit.
4. Required, ReadOnly, Disabled, Error and Focused states remain visually distinct and centrally governed.
5. RTL order is preserved for fields, tabs and commands.
6. No Arabic clipping is introduced by local sizing.
7. Grid/pagination/audit/toolbar are shared controls, not screen-local clones.
8. No local visual exception is required.

## Verdict
`TEAM-D05 VISUAL = PASS`.

Nonblocking technical gates carried unchanged:
- exact lookup provider/endpoint identifiers;
- ItemsGrid exact server sort-key mapping;
- future screen-specific shortcuts only if separately issued.

Next stage: `TEAM-D06 — INDEPENDENT_REVIEW`.
