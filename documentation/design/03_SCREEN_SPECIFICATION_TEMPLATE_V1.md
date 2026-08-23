# TransportERP — Canonical Screen Specification Template V1

> One canonical `screen-spec.md` per active screen. Do not fork competing final copies.

## Identity
- ScreenCode:
- ArabicName:
- Domain:
- GoverningKurrasaRefs:
- ScreenProfile: `MasterData | TreeMaster | Transaction | ControlApproval | ReportInquiry | Settings`
- Variant:
- Capabilities:
- CurrentDesignState:

## Purpose and actors
- Business purpose:
- Primary roles:
- Entry points:
- Preconditions:

## Layout contract
- Screen shell:
- Header/MainData role:
- Workspace role:
- Fill owner:
- Summary/Action/Audit regions:
- RTL notes:
- DPI/resize notes:
- Local exception, if any, with approval reference:

## Fields
| FieldKey | Arabic Label | Semantic | ValueType | Required | Editable Rule | Lookup | Validation | Visibility Rule |
|---|---|---|---|---|---|---|---|---|

## Grids
For each grid define: purpose, row identity, columns, editable columns, selection mode, paging, filters, sorting, empty/loading/error behavior.

## Tabs / Sections
| Order | Tab/Section | Purpose | LayoutRole | Visibility Rule |
|---:|---|---|---|---|

## Commands
| Command | Capability | Permission Need | Enabled When | Confirmation | Result |
|---|---|---|---|---|---|

## Lookups
Define server/cache behavior, search key, result cap, selected identity and company/branch scope.

## States and workflow
- Business states:
- UI modes:
- allowed transitions:
- conflict/reload behavior:

## Online / Offline classification
For each action: `READ_CACHE_ONLY`, `DRAFT_LOCAL`, `CAPTURE_AND_QUEUE`, or `ONLINE_AUTHORITATIVE` — only when supported by governing authority. Otherwise `TBD-GATED`.

## Validation and messages
- field errors:
- summary errors:
- permission/scope errors:
- concurrency conflict:
- loading/empty/error:

## Accessibility / keyboard
- default focus:
- Enter behavior:
- Escape behavior:
- tab order:
- screen-reader/contrast notes where applicable:

## Acceptance criteria
Numbered, testable criteria only.

## Evidence
- Wireframe:
- Visual:
- Review report:
- Source references:

## Independent review
- Reviewer:
- Verdict: `PASS | NEEDS_REVISION | HOLD`
- Findings:
- ClosedAt:
