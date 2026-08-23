# FLOW01-W3-SCR-001 — إدخال البوليصة — Canonical Screen Specification

**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**ScreenProfile:** `Transaction`  
**Variant:** `HeaderLines`  
**Canonical alias:** `SHP-001`  
**Domain:** `Waybills / FLOW01`  
**OwnerTeam:** `TEAM-D06`  
**Date:** `2026-08-24`

> This is the one canonical screen record. Stage evidence files are attachments/references, not competing final copies. Repository-lineage `SHP-005` is non-governing and is retained only for traceability.

## 1. Authority and evidence readiness
Read/reconciled before design claims:
- current live queue;
- current FLOW01 W1/W2/W3 authority and exact screen identity/trace;
- `FLOW01_W2_EXACT_CONTRACT_AND_SECURITY_BINDING_2026-08-22.md`;
- `FLOW01-W3-SCR-001_TYPED_SCREENDEFINITION.md`;
- `FLOW01_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` (`TAE-F01-001`, issued but not run);
- `FLOW01_ATOMIC_TRACEABILITY_AND_E2E_SPEC_2026-08-22.md`;
- `Transaction_Profile_Specification_V1.1`;
- `CoreUI_Properties_Specification_V1.4`;
- `CoreUI_Controls_Catalog_V1.2`;
- `Shared_API_Error_Paging_Lookup_Contracts_TransportERP_V1.3`;
- `CoreUI_Architecture_Tests_Specification_V1.2`;
- project-owner layout decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`;
- project-owner FIELD_GRID design authority: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`;
- stage evidence: `ux-stage-evidence.md`, `visual-stage-evidence.md`.

Completed stages:
- `TEAM-D01 / ANALYSIS = PASS`
- `TEAM-D02 / LAYOUT = PASS`
- `TEAM-D03 / FIELD_GRID = PASS`
- `TEAM-D04 / UX = PASS`
- `TEAM-D05 / VISUAL = PASS`
- `TEAM-D06 / INDEPENDENT_REVIEW = IN PROGRESS`

## 2. Identity and business boundary
- ScreenCode: `FLOW01-W3-SCR-001`
- Alias: `SHP-001`
- ArabicName: `إدخال البوليصة`
- English role: `Shipment Entry`
- Profile/Variant: `Transaction / HeaderLines`
- Business purpose: create a Shipment as Draft, edit Draft under optimistic concurrency, confirm Draft → Confirmed, and read/search within server-authorized company/branch scope.

Logical boundary:
- `Shipment`
- `ShipmentItem`
- `ShipmentPackage`
- `ShipmentLeg`

Lifecycle vocabulary visible to this screen:
`Draft / Confirmed / Active / Completed / Cancelled`.

This screen creates/edits Draft and confirms Draft → Confirmed only. No command is invented for Active, Completed, or Cancelled. Confirmed creates a commercial commitment/reference only; no posting, journal creation, revenue recognition, or commission is implied.

## 3. Permissions and capabilities
| Command | Capability | Permission | Enabled when | Contract result |
|---|---|---|---|---|
| إنشاء | `F01.Shipment.Create` | `f01.shipment.create` | new Draft | `POST /api/v1/shipments` → `ShipmentResource` |
| تعديل مسودة | `F01.Shipment.UpdateDraft` | `f01.shipment.edit` | Draft + `expectedVersion` | `PATCH /api/v1/shipments/{shipmentId}` |
| تأكيد | `F01.Shipment.Confirm` | `f01.shipment.confirm` | Draft only | Draft → Confirmed; reason/audit contract applies |
| عرض | `F01.Shipment.Get` | `f01.shipment.view` | authorized scope | read `ShipmentResource` |
| بحث | `F01.Shipment.Search` | `f01.shipment.view` | authorized scope | paged search |

UI permission state is a UX layer only; server permission and data scope remain authoritative.

## 4. Layout contract — TEAM-D02 PASS
Approved project-owner resolution:
- Header/MainData = `Content`;
- Tabs/Workspace = `Fill`;
- line grids = `Fill`;
- LocalException = `NONE`.

```text
RTL Transaction / HeaderLines
┌──────────────────────────────────────────────────────────────┐
│ Shared TransportToolbar / shell                              │
├──────────────────────────────────────────────────────────────┤
│ Header / MainData — Content                                  │
│ status | shipment id | customer | origin | destination      │
├──────────────────────────────────────────────────────────────┤
│ Tabs / Workspace — Fill                                      │
│ General | Items | Packages | Legs | Audit                    │
│ relevant grid owns Fill inside its tab                       │
├──────────────────────────────────────────────────────────────┤
│ Shared validation/status/action/audit presenters             │
└──────────────────────────────────────────────────────────────┘
```

No local size, padding, font, color, toolbar, grid, pagination, audit, RTL, or DPI override.

## 5. Fields — TEAM-D03 PASS
| FieldKey | Arabic Label | Profile / Type | Required | Edit policy | Validation / semantics |
|---|---|---|---|---|---|
| `shipmentId` | رقم البوليصة | KeyText / UUID | No | Read-only | server assigned |
| `customerRef` | العميل | Lookup / Reference | Yes | Draft only | permitted customer |
| `originRef` | المصدر | Lookup / Reference | Yes | Draft only | scope-valid location |
| `destinationRef` | الوجهة | Lookup / Reference | Yes | Draft only | scope-valid location |
| `shipmentState` | الحالة | State / Enum | Yes | Read-only | governing lifecycle |
| `itemLines` | البنود | LineGrid / Collection | Yes | Draft only | nonempty on confirm |
| `packageLines` | الطرود | LineGrid / Collection | No | Draft only | package/item consistency |
| `legLines` | المراحل | LineGrid / Collection | No | Draft only | chronology validation |
| `expectedVersion` | إصدار التزامن | Hidden / Integer | update/confirm | read-only token | required on update/confirm |

No field is promoted from legacy `SHP-005/006/007/008` merely because it exists there.

## 6. Grids — TEAM-D03 PASS
Shared rules:
- `GridProfile = TransactionLines`;
- `AutoGenerateColumns = false`;
- `SelectionPolicy = SingleRow`;
- CoreUI owns RTL, header/row style, row height, editing/focus states;
- Draft-only editing; outside Draft all line columns are read-only;
- no row-level permission is invented;
- reference editors use `TransportLookup` where declared;
- widths use semantic CoreUI policies, not fixed local pixels.

### ItemsGrid
| # | Key | Arabic Header | Type | Required | Edit | Sort design | Width / editor |
|---:|---|---|---|---|---|---|---|
| 1 | `itemOrPackageRef` | مرجع الصنف/الطرد | Reference | Yes | Draft | Yes* | content / `TransportLookup` |
| 2 | `description` | الوصف | String | No | Draft | Yes* | primary Fill / text |
| 3 | `quantity` | الكمية | Decimal | Yes | Draft | Yes* | content numeric |
| 4 | `weight` | الوزن | Decimal | No | Draft | Yes* | content numeric |
| 5 | `volume` | الحجم | Decimal | No | Draft | Yes* | content numeric |
| 6 | `state` | الحالة | Enum | Yes | Read-only | Yes* | content state |

`*` Actual server sort-key mapping remains `TBD-GATED`; arbitrary database expressions are not authorized.

Items behavior:
- server paging = true;
- confirm requires at least one item line;
- typed sort only when an allow-listed server mapping exists.

### PackagesGrid
| # | Key | Arabic Header | Type | Required | Edit | Width / editor |
|---:|---|---|---|---|---|---|
| 1 | `packageRef` | مرجع الطرد | Reference | Yes | Draft | content / `TransportLookup` |
| 2 | `weight` | الوزن | Decimal | No | Draft | content numeric |
| 3 | `volume` | الحجم | Decimal | No | Draft | content numeric |
| 4 | `state` | الحالة | Enum | Yes | Read-only | content state |

- embedded Draft collection; no API paging contract is created by the design;
- package/item consistency remains authoritative domain/API validation;
- no package-item cardinality or hidden FK is invented.

### LegsGrid
| # | Key | Arabic Header | Type | Required | Edit | Width / editor |
|---:|---|---|---|---|---|---|
| 1 | `sequence` | الترتيب | Integer | Yes | Draft | content numeric |
| 2 | `stageOrStationRef` | المرحلة/المحطة | Reference | Yes | Draft | primary Fill / `TransportLookup` |

- embedded Draft chronology collection;
- chronology validity remains domain/API authority;
- no dates/times/from-to/route formulas are invented.

## 7. Tabs
| Order | Tab | Role |
|---:|---|---|
| 1 | General | shipment header/general data |
| 2 | Items | ItemsGrid Fill |
| 3 | Packages | PackagesGrid Fill |
| 4 | Legs | LegsGrid Fill |
| 5 | Audit | shared audit/context display |

Tabs are real functional sections; no decorative/empty tab is added.

## 8. Lookups
Shared contract for header lookups and declared grid references:
- server-side debounced search;
- selected identity = `LookupItem.Id`;
- maximum results = `50`;
- Code/DisplayName/SecondaryName are presentation/search values only;
- permission/status/company/branch filtering is server-authoritative;
- no full-table client loading.

Semantics:
- `customerRef`: permitted customer;
- `originRef`: scope-valid location;
- `destinationRef`: scope-valid location.

Nonblocking `TBD-GATED` technical bindings:
- exact customer/location/item/package/stage provider or endpoint identifiers;
- exact screen-specific lookup search keys beyond shared LookupItem values.

No API identifier is invented.

## 9. Search / paging
Issued `ShipmentSearchQuery` filters:
- shipment number;
- customer;
- origin;
- destination;
- state;
- date range.

Shared maximum page size = 200. Filtering/sorting are typed/allow-listed server-side. Client-provided company/branch values are not authorization authority.

## 10. Online / offline
FLOW01 write contracts are `ONLINE_AUTHORITATIVE`:
- `OFFLINE_WRITE = 0`;
- `Can Queue = NO`;
- no Outbox/ACK/Replay/sync write path is created by this screen design.

## 11. UX contract — TEAM-D04 PASS
Detailed evidence: `ux-stage-evidence.md`.

### Modes
- New Draft: issued editable fields/grids available under capability/permission.
- Existing Draft: update requires `expectedVersion`; confirm requires Draft + permission/capability + valid data.
- Non-Draft: Draft-only fields/grids become read-only; no extra lifecycle command is invented.

### Loading / submit
- use shared `TransportLoadingState`;
- disable conflicting state-changing commands during active request;
- prevent UI double-submit; server idempotency remains authoritative.

### Validation
- use `TransportValidationPresenter` for field/summary errors;
- map validation errors by field key/semantic path when available;
- no local MessageBox validation path;
- required visual state is not itself an error.

### Concurrency
On stale version:
- present shared conflict state;
- offer Refresh/Reload;
- never silently overwrite;
- no client merge algorithm is invented;
- returned current resource/version becomes authoritative after reload.

### Permission/error privacy
- permission/scope denial reveals no hidden data;
- unexpected error uses shared error state;
- CorrelationId appears only in technical/support details;
- retry only when contract says retry may be meaningful.

### Keyboard/focus
- shared CoreUI/Grid keyboard behavior only;
- no screen-specific Enter/Escape/global shortcut is invented;
- focus/tab order follows RTL visual flow and shared host behavior;
- exact runtime TabIndex is implementation-owned.

## 12. Visual contract — TEAM-D05 PASS
Detailed evidence: `visual-stage-evidence.md`.

- central CoreUI typography only; no per-screen font;
- central spacing/sizing tokens only;
- MainData maximum two columns;
- semantic states: Normal, Required, ReadOnly, Disabled, Error, Focused;
- Required uses required semantic presentation, not error color;
- `shipmentId` and `shipmentState` use ReadOnly state;
- grids use shared `TransportDataGrid` styles and semantic widths;
- tabs use central RTL/right-origin visual policy;
- toolbar uses central command styling/order;
- validation/loading/empty/error/audit/pagination are shared presenters/controls;
- no raw colors, local font/height/margin, or LocalException;
- logical-pixel tokens scale for DPI; Arabic text must not clip at reference DPI scales;
- focus cues remain visible and accessibility metadata comes from ScreenDefinition metadata where supported.

## 13. Validation and messages
Authority-backed conditions:
- permitted customer;
- scope-valid origin/destination;
- at least one item on confirm;
- package/item consistency;
- leg chronology validity;
- expectedVersion on update/confirm;
- default-deny permission/scope enforcement.

Shared error categories include validation, not-found, permission/scope denial, concurrency/state/idempotency conflict, and unexpected server error with CorrelationId. No screen-local error taxonomy is created.

## 14. Acceptance criteria
1. Identity is only `FLOW01-W3-SCR-001 / SHP-001`, not legacy `SHP-005`.
2. Profile/Variant is `Transaction / HeaderLines`.
3. Header/MainData=Content; Tabs/Workspace=Fill; line grids=Fill; no LocalException.
4. No local toolbar/grid/pagination/audit/validation/loading/error implementation is specified.
5. Create/edit/confirm/view/search remain bound to issued permissions/capabilities/state.
6. Draft edit requires `expectedVersion`; stale conflict never silently overwrites.
7. Confirm requires at least one item line.
8. All concrete grid columns are explicit and `AutoGenerateColumns=false`.
9. No unissued package cardinality, leg timing/route formula, API, DTO, permission, or storage field is invented.
10. Lookup is server-side, debounced, max 50, identity by Id, server scope authoritative.
11. UI prevents double-submit while server idempotency remains authority.
12. All write actions are online-authoritative; no offline queue/replay is exposed.
13. Visual system uses only CoreUI tokens and shared state presenters.
14. RTL/DPI/accessibility behavior is inherited and no Arabic clipping/local sizing hack is introduced.
15. Nonblocking TBD technical bindings are documented and do not masquerade as approved API contracts.

## 15. Nonblocking gated items
These do **not** block design approval and remain technical binding work for the appropriate authority/implementation stage:
- exact lookup provider/endpoint identifiers;
- exact ItemsGrid server sort-key mapping;
- any future screen-specific keyboard shortcut only if separately issued.

## 16. Evidence
- Layout decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`
- FIELD_GRID authority decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`
- UX evidence: `documentation/design/screens/Waybills/FLOW01-W3-SCR-001/ux-stage-evidence.md`
- Visual evidence: `documentation/design/screens/Waybills/FLOW01-W3-SCR-001/visual-stage-evidence.md`
- Independent review: pending `independent-review.md`

## 17. Handoff
- CurrentStage: `INDEPENDENT_REVIEW`
- OwnerTeam: `TEAM-D06`
- BlockingIssue: none at design level; only documented nonblocking technical gates.
- HandoffReady: `YES`

## 18. Independent review
- Reviewer: `TEAM-D06`
- Verdict: `PENDING`
- Findings: pending
- ClosedAt: pending

No application code, official kurrasa, DDL, migration, permission, API contract, or offline-write authority is modified or created by this design record.
