# FLOW01-W3-SCR-001 — TEAM-D04 UX Stage Evidence

Status: PASS
Stage: UX
Owner: TEAM-D04
Date: 2026-08-24

## Authority used
- Current canonical `screen-spec.md` for `FLOW01-W3-SCR-001`.
- `Transaction_Profile_Specification_V1.1`.
- `CoreUI_Controls_Catalog_V1.2`.
- `CoreUI_Properties_Specification_V1.4`.
- `Shared_API_Error_Paging_Lookup_Contracts_TransportERP_V1.3`.
- Current FLOW01 W2/W3 action, permission, lifecycle and concurrency bindings already cited by the canonical screen specification.

This evidence creates UX behavior only. It creates no API, DTO, permission, DDL, persistence, offline-write, or official-kurrasa authority.

## UX state model
### New / Draft create
- Editable header fields follow issued field edit rules.
- `shipmentId` remains read-only/server-assigned.
- Grid line editing is permitted only where the current screen contract allows Draft editing.
- Create command availability requires the issued capability/permission state.

### Existing Draft
- Draft-editable fields and grids remain editable only when both lifecycle and issued permission/capability allow it.
- Update requires the current `expectedVersion` token.
- Confirm is available only for Draft and only with the issued confirm permission/capability.
- Confirm remains blocked by governing validation such as an empty Items collection.

### Confirmed / other non-Draft states
- Screen data becomes read-only for fields and line collections governed as Draft-only.
- No additional lifecycle command is invented for Active, Completed, or Cancelled.
- Read/search remain governed by `f01.shipment.view` and server-authorized scope.

## Loading and command behavior
- Use shared `TransportLoadingState`; do not create screen-local loading UI.
- While a state-changing request is in progress, conflicting state-changing commands are disabled.
- Prevent duplicate submit from the UI while the request is active; server idempotency remains the authoritative protection.
- Search, paging, and lookup loading indicators use shared CoreUI presenters/controls.

## Validation behavior
- Use `TransportValidationPresenter` for field and summary errors.
- W2 validation errors map by field key/semantic path when available.
- No duplicate MessageBox validation path is introduced.
- Required is a required-state presentation, not an error until validation fails.
- Confirm validation keeps the user on the current screen and exposes the mapped field/summary errors without silent data mutation.

## Concurrency behavior
For stale `expectedVersion` / concurrency conflict:
1. present the shared concurrency-conflict state;
2. explain that the record changed on the server using the localized safe message from the API/error mapping;
3. provide Refresh/Reload through the shared conflict/error affordance;
4. do not silently overwrite;
5. do not locally invent a merge algorithm;
6. after reload, use the returned current resource/version as the new authoritative display state.

## Permission / scope / not-found / unexpected-error behavior
- Permission or scope denial must not reveal hidden data.
- Not-found is displayed only within the caller's authorized scope; no existence inference is added.
- Unexpected server failure uses shared `TransportErrorState` and exposes `CorrelationId` only through technical-details/support affordance, not mixed into the business message.
- Retry is offered only when the error contract marks retry as meaningful; retry never bypasses idempotency/state rules.

## Lookup interaction
For `customerRef`, `originRef`, `destinationRef`, and reference editors declared by FIELD_GRID:
- use shared `TransportLookup`;
- server-side debounced search only;
- maximum rendered results: 50;
- selected identity: `LookupItem.Id`;
- display/search values may use Code/DisplayName/SecondaryName;
- authorization, active/status filtering, company and branch scope remain server-authoritative;
- refine the search to reach additional matches; never download a full table.

Exact provider/endpoint identifiers remain `TBD-GATED` and nonblocking; none are invented here.

## Grid interaction
- Grid selection remains `SingleRow` as established by FIELD_GRID.
- Shared `TransportDataGrid` owns keyboard/focus/edit-state rendering.
- Items server paging remains shared CoreUI/W2 behavior.
- Sort affordance is active only where the server allow-listed mapping exists; unresolved Items sort-key mapping remains `TBD-GATED` and nonblocking.
- Packages/Legs paging behavior is not made authoritative by UX; any unissued paging binding remains technical `TBD-GATED` and does not invent an API contract.
- Packages and Legs retain their current collection semantics; no hidden route/cardinality behavior is added.

## Tabs and navigation
- Tabs remain `General | Items | Packages | Legs | Audit` because they are issued by the governing screen definition.
- RTL order and tab rendering are inherited from CoreUI.
- No additional decorative tab is introduced.
- No nested scroll is introduced into MainData; workspace/grid remains the scroll/fill owner according to the approved Transaction layout.

## Keyboard and focus
- Shared CoreUI/Grid keyboard behavior is inherited.
- No screen-specific Enter shortcut, Escape shortcut, global shortcut, default-focus target, or override is introduced because no current authority issues one.
- Tab/focus sequence follows the shared RTL visual-order rule.
- Exact runtime TabIndex/focus-target details remain implementation-owned unless separately governed.

## Success feedback
- Successful create/update/confirm uses shared success/status feedback.
- After create, the server-returned identity/resource is authoritative.
- After update/confirm, the returned resource/state/version replaces stale client values.
- Confirm success visibly reflects `Confirmed`; it does not imply posting, journal creation, revenue recognition, or commission.

## UX acceptance checks
1. Draft-only editability is preserved.
2. Permission + lifecycle + capability jointly gate sensitive commands.
3. Loading disables conflicting submissions and avoids double-submit.
4. Validation is rendered through the shared presenter.
5. Concurrency conflict offers Refresh/Reload and never silently overwrites.
6. Permission/scope denial leaks no hidden data.
7. CorrelationId stays in support/technical details.
8. Lookup is server-side, debounced, capped at 50, and binds by Id.
9. No local MessageBox validation, local grid behavior clone, or screen-specific loading/error presenter is created.
10. No unissued shortcut, default-focus target, endpoint, permission, lifecycle transition, paging contract, offline path, or business rule is invented.

## Verdict
`TEAM-D04 UX = PASS` after independent-review cleanup of unsupported focus wording.

Nonblocking gates carried forward:
- exact lookup provider/endpoint identifiers;
- ItemsGrid exact server sort-key mapping;
- any future screen-specific keyboard/focus rule only if separately issued.

Next stage: `TEAM-D05 — VISUAL`.
