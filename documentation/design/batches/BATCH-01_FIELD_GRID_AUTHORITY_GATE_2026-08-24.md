# BATCH-01 — FIELD_GRID Authority Gate

**Status:** HOLD_AUTHORITY
**Owner:** TEAM-D03 / DESIGN-LEAD
**Date:** 2026-08-24

## Affected screens
- FLOW01-W3-SCR-002 — تخصيص الشحنة
- FLOW01-W3-SCR-003 — تتبع البوليصة
- FLOW01-W3-SCR-006 — الترانزيت وتسليم الحيازة
- FLOW01-W3-SCR-007 — رحلة الشحن
- FLOW01-W3-SCR-008 — مانيفست الرحلة

## What is already governing and complete
- Canonical identities, Profiles and Variants.
- W1 boundaries.
- W2 exact Actions/Routes/DTO names/Permissions/State-or-scope for the in-scope FLOW01 screens.
- Typed screen-level fields and semantic grid column names.
- `AutoGenerateColumns=false` and issued paging behavior where stated.
- CoreUI/Profile layout composition.

## Exact gap
`ScreenDefinition_Contract_V1` requires concrete `GridColumnDefinition` metadata. The current FLOW01 typed definitions issue semantic column names but do not fully issue, for each column, all of:
- UI ValueType;
- Arabic display label;
- Required/ReadOnly/edit policy;
- order;
- CoreUI width policy;
- editor/lookup presentation;
- selection policy where not already fixed by profile/capability.

The W2 exact contract issues DTO families and semantic minimums but explicitly describes API payload semantics, not a complete UI GridColumnDefinition.

## Required owner decision
One batch-level decision may resolve all five screens:

Authorize TEAM-D03 to define **screen-specific UI design metadata only** for the issued grid semantics, bounded by current CoreUI/Profile contracts and current FLOW01 W1/W2 semantics.

This authority must NOT create or change:
- API route/action/provider identifiers;
- DTO payload fields beyond issued W2 contracts;
- Permission codes or scope;
- DDL/storage schema;
- business formulas/lifecycle rules;
- offline-write authority;
- official kurrasa content.

Any unissued technical binding remains `TBD-GATED` and may be nonblocking when it is not required to render the design contract.

Until this decision exists, FIELD_GRID remains `HOLD_AUTHORITY` for Batch-01; no column metadata is guessed from names alone.
