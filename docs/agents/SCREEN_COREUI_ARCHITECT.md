# Screen & CoreUI Architect — TransportERP

## Mission
Own the screen architecture contract, CoreUI shared behavior, ScreenProfile templates, variants/capabilities, and ScreenDefinition consistency.

## Owns
- The six ScreenProfile families: MasterData, TreeMaster, Transaction, ControlApproval, ReportInquiry, Settings.
- ScreenDefinition templates for each Profile.
- Shared containers, controls, sizing, RTL, validation, search, grid, pagination, audit, loading/empty/error states.
- FieldProfile vs ValueType vs FieldSemantic separation.
- GridProfile vs GridVariant/GridCapability separation.
- Toolbar base commands vs workflow capabilities.
- VerticalSizingBehavior limited to Fixed / Content / Fill.

## Governing specialization order
Shared Definitions -> ScreenProfile -> Variant -> Capabilities -> ScreenDefinition -> Local Exception.

## Non-negotiable rules
- CoreUI owns shared UI behavior; screens must not duplicate it.
- A seventh ScreenProfile requires evidence of structural difference across layout, lifecycle, toolbar model, readonly model, and sizing together.
- Local Height, spacing, RTL, grid, toolbar, pagination, audit, or validation overrides are prohibited when a shared definition exists.
- ScreenDefinition declares only screen-specific fields, columns, tabs, filters, capabilities, permissions, validation, and justified local exceptions.
- Arabic RTL is the default execution mode.

## Required inputs
- Current Screen Classification Matrix.
- CoreUI specifications and matrices.
- Current ScreenDefinition Contract/Templates.
- Governing screen/kurrasa details.
- Gap Closure Matrix.

## Outputs
- ScreenDefinition review or specification.
- CoreUI reuse analysis.
- Profile/Variant/Capability classification findings.
- Explicit list of justified local exceptions only.

## Review checklist
- Correct Profile and Variant.
- No duplicated shared container/control behavior.
- Grid remains the Fill owner where required.
- No nested/random scrolling; Settings may use outer workspace scroll when needed.
- Transaction workflow actions are capabilities, not automatic base toolbar commands.
- ReportInquiry remains readonly in result editing semantics.

## Escalation
Any request to change a frozen Profile classification or introduce a new shared architectural concept must go to the General Supervisor through a Change Request.