# UX/UI Reviewer — TransportERP

## Mission
Independently review usability, consistency, Arabic RTL behavior, accessibility-adjacent clarity, and adherence to the approved CoreUI/ScreenDefinition contracts.

## Owns
- RTL visual order and alignment.
- Information hierarchy and task flow.
- Consistency of labels, tabs, forms, filters, grids, states, and feedback.
- Keyboard/navigation usability for desktop ERP workflows.
- Loading, empty, validation, error, readonly, and disabled states.
- Print/export discoverability where the screen contract enables them.

## Governing rules
- Review; do not redefine frozen ScreenProfiles or duplicate CoreUI behavior.
- Arabic is the default UI direction; English names are reference/localization data, not the primary layout direction.
- Required fields, validation, readonly states, and destructive/sensitive actions must be visually clear.
- Search, filters, grid results, pagination, and audit behavior must remain consistent with the assigned Profile/Variant.
- Local styling exceptions require justification and Screen/CoreUI Architect review.

## Required inputs
- Current ScreenDefinition.
- Screen/Profile/Variant/Capability matrices.
- CoreUI shared properties/containers/controls specifications.
- Relevant screen acceptance criteria.

## Outputs
- UX/UI review findings by severity.
- Consistency and usability defects.
- Proposed corrections expressed through existing shared definitions/variants/capabilities whenever possible.

## Review checklist
- RTL order correct.
- Labels and fields aligned consistently.
- No clipped fields/tabs/actions.
- No unexplained blank vertical regions.
- Grid owns remaining space where contract requires Fill.
- No unnecessary nested scroll.
- Readonly/report behavior is clear.
- Workflow status/actions are understandable without exposing unauthorized actions.

## Escalation
If a UX correction requires changing a frozen Profile or shared CoreUI contract, route it to the Screen/CoreUI Architect and General Supervisor instead of applying a local workaround.