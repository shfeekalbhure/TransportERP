# FLOW01-W3-SCR-001 — FIELD_GRID Design Authority Decision

**Decision date:** 2026-08-24
**Authority:** Project Owner
**Scope:** Design documentation only for `FLOW01-W3-SCR-001 — إدخال البوليصة`.

## Decision
The Project Owner authorizes `TEAM-D03` to define the screen-specific FIELD_GRID contract required by the current `ScreenDefinition` rules, using the current approved CoreUI/Profile contracts and the already issued FLOW01 business semantics as hard boundaries.

This authority includes defining, as design metadata:
- concrete column keys and Arabic labels;
- semantic/value types appropriate to the issued business meaning;
- required/read-only/edit rules consistent with the issued Draft/edit lifecycle;
- display order and CoreUI-compatible width policy;
- selection policy;
- lookup/editor presentation binding where a lookup is already semantically required;
- concrete design contracts for `ItemsGrid`, `PackagesGrid`, and `LegsGrid`.

## Lookup disposition
For `customerRef`, `originRef`, and `destinationRef`, TEAM-D03 may complete the UI/design lookup contract using the already approved shared CoreUI/W2 lookup mechanics:
- server-side debounced search;
- selected identity by `LookupItem.Id`;
- maximum 50 results;
- server-authoritative permission/status/company/branch scope.

Exact domain API endpoint/action/DTO/search-provider identifiers that are not currently issued must remain `TBD-GATED` and are **nonblocking for FIELD_GRID design**. TEAM-D03 must not invent them.

## Hard boundaries
This decision does **not** authorize TEAM-D03 to create or modify:
- API routes or endpoints;
- DTO contracts;
- permissions or security scope;
- DDL, tables, migrations, or persistence contracts;
- offline-write authority;
- official kurrasa content;
- application code.

Legacy `SHP-005/006/007/008` repository UI may be consulted only as lineage/implementation evidence; it must not silently become governing authority.

## Effect on workflow
The prior `FIELD_GRID / HOLD_AUTHORITY` caused solely by missing screen-specific grid metadata and lookup-source detail is resolved by this owner decision.

`TEAM-D03` shall resume `FIELD_GRID`, produce the explicit design contract in the canonical `screen-spec.md`, keep unavailable technical bindings as `TBD-GATED`, and hand off to `TEAM-D04 / UX` only after the screen-specific grid/lookup design is internally complete and traceable.

No official kurrasa or application-code change is created by this decision.