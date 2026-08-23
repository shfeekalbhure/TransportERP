# FLOW01-W3-SCR-001 — إدخال البوليصة — Canonical Screen Specification

## Mandatory pre-read / evidence readiness
- Queue row read from `documentation/design/04_SCREEN_WORK_QUEUE.csv`: `YES`.
- Orchestrator protocol and workflow read: `YES`.
- Current governing FLOW01 kurrasa/current-design authority read and reconciled: `YES`.
- `SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001`: read; governing for issued W2/W3 facts only.
- W1 logical boundary, W2 exact contract/security binding, typed W3 ScreenDefinition, acceptance specification and atomic trace: read.
- Current repository waybill implementation: read as implementation/lineage evidence only.
- Current approved CoreUI/Profile authority read: `Transaction_Profile_Specification_V1.1`, `ScreenDefinition_Templates_V1.1`, `CoreUI_Controls_Catalog_V1.2`, `CoreUI_Properties_Specification_V1.4`, `Shared_API_Error_Paging_Lookup_Contracts_TransportERP_V1.3`.
- Legacy R2/V4 `SHP-005/006/007/008`: reconciled as non-governing lineage; nothing is imported merely because it exists there.
- Layout authority decision read and applied: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.
- FIELD_GRID owner authority decision read and applied: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

Stage readiness:
- TEAM-D01 / ANALYSIS: `COMPLETED`.
- TEAM-D02 / LAYOUT: `COMPLETED`.
- TEAM-D03 / FIELD_GRID: `COMPLETED`.
- TEAM-D04 / UX: `READY`.

## Identity
- ScreenCode: `FLOW01-W3-SCR-001`
- Current FLOW01 alias: `SHP-001`
- ArabicName: `إدخال البوليصة`
- English role: `Shipment Entry`
- Domain: `Waybills / FLOW01`
- ScreenProfile: `Transaction`
- Variant: `HeaderLines`
- Capabilities: `F01.Shipment.Create`, `F01.Shipment.UpdateDraft`, `F01.Shipment.Confirm`, `F01.Shipment.Get`, `F01.Shipment.Search`.
- CurrentDesignState: `UX`
- CompletedStages: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`
- OwnerTeam: `TEAM-D04`

## Authority chain
1. `CHG-20260818-FLOW01-W3-ID-002` — canonical identity: `FLOW01-W3-SCR-001 / SHP-001 / إدخال البوليصة / Transaction / HeaderLines`.
2. `FLOW01-REQ-CONTRACT-001` — approved business requirement; no direct code authority.
3. `W1-FLOW01-LOGICAL-DOMAIN-001` — logical boundary only; no DDL/physical field authority.
4. `OWNER-FLOW01-P1-BUSINESS-DECISIONS-001` — issued business decisions for FLOW01 boundaries.
5. `FLOW01_W2_EXACT_CONTRACT_AND_SECURITY_BINDING_2026-08-22.md` — exact actions/routes/DTO/permissions/scope.
6. `FLOW01-W3-SCR-001_TYPED_SCREENDEFINITION.md` — typed screen definition.
7. `FLOW01_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — `TAE-F01-001`, issued but not run.
8. `FLOW01_ATOMIC_TRACEABILITY_AND_E2E_SPEC_2026-08-22.md` — atomic trace.
9. Current approved CoreUI/Profile contracts listed in the pre-read section.
10. Project-owner layout decision dated 2026-08-24.
11. Project-owner FIELD_GRID design authority decision dated 2026-08-24.

## Legacy / repository reconciliation
The repository-lineage form uses `SHP-005` and presents `SHP-005/006/007/008` as tabs. Current FLOW01 authority treats that lineage as non-governing. It is not used to define this screen.

The governing surface is `FLOW01-W3-SCR-001`, with issued tabs:
`General | Items | Packages | Legs | Audit`.

## Purpose and actors
- Business purpose: create a Shipment as Draft, edit Draft under optimistic concurrency, confirm Draft → Confirmed, and read/search shipments inside server-authorized scope.
- Business role names are not invented; UI authority is represented by the issued permission codes only.
- Server scope is authoritative; client-supplied company/branch values are not authorization authority.

Issued permissions:
- `f01.shipment.create`
- `f01.shipment.edit`
- `f01.shipment.confirm`
- `f01.shipment.view`

## W1 logical boundary
- `Shipment`
- `ShipmentItem`
- `ShipmentPackage`
- `ShipmentLeg`

Issued lifecycle vocabulary: `Draft / Confirmed / Active / Completed / Cancelled`.

This screen writes Draft and confirms Draft → Confirmed only. `Confirmed` creates a commercial commitment/reference only; it does not post a journal, recognize revenue, or create commission.

## Layout contract — TEAM-D02 PASS
- Shell: governed `Transaction / HeaderLines`.
- Header/MainData: `Content`.
- Tabs/Workspace: `Fill`.
- Relevant Lines/Grid inside workspace: `Fill`.
- Summary/Action/Audit: inherited shared CoreUI/Profile regions only.
- RTL/DPI/resize: inherited CoreUI behavior; no local exception.
- LocalException: `NONE`.

```text
Transaction / HeaderLines
┌──────────────────────────────────────────────────────────┐
│ Shared CoreUI command / shell regions                    │
├──────────────────────────────────────────────────────────┤
│ Header / MainData — Content                              │
│ status | customer | origin | destination                 │
├──────────────────────────────────────────────────────────┤
│ Tabs / Workspace — Fill                                  │
│ General | Items | Packages | Legs | Audit                │
│                                                          │
│ Relevant line grid owns Fill inside its tab              │
└──────────────────────────────────────────────────────────┘
```

No local pixel sizes, colors, fonts, toolbar/grid styling, pagination, audit, RTL or DPI rules are created.

## Fields — TEAM-D03 PASS
| FieldKey | Arabic Label | FieldProfile | ValueType | Required | Edit policy | Lookup / validation | Visibility |
|---|---|---|---|---|---|---|---|
| `shipmentId` | رقم البوليصة | KeyText | UUID | No | Read-only | server assigned | after identity exists |
| `customerRef` | العميل | Lookup | Reference | Yes | Draft only | permitted customer | normal |
| `originRef` | المصدر | Lookup | Reference | Yes | Draft only | scope-valid location | normal |
| `destinationRef` | الوجهة | Lookup | Reference | Yes | Draft only | scope-valid location | normal |
| `shipmentState` | الحالة | State | Enum | Yes | Read-only | W1/W2 lifecycle | normal |
| `itemLines` | البنود | LineGrid | Collection | Yes | Draft only | nonempty on confirm | Items tab |
| `packageLines` | الطرود | LineGrid | Collection | No | Draft only | package/item consistency | Packages tab |
| `legLines` | المراحل | LineGrid | Collection | No | Draft only | chronology validation | Legs tab |
| `expectedVersion` | إصدار التزامن | Hidden | Integer | update/confirm only | Read-only client token | required on update/confirm | hidden |

No field is promoted from legacy repository UI.

## FIELD_GRID design authority and boundaries
The Project Owner explicitly authorized TEAM-D03 to define screen-specific design metadata for `ItemsGrid`, `PackagesGrid`, and `LegsGrid`, bounded by current CoreUI and already issued FLOW01 business semantics.

This section is **design metadata**, not a new API/DTO/DDL/persistence contract. Exact unissued technical provider identifiers remain `TBD-GATED` and nonblocking.

Shared rules for all three grids:
- `GridProfile = TransactionLines`.
- `AutoGenerateColumns = false`.
- Grid workspace = `Fill` through CoreUI.
- CoreUI owns RTL, row/header styling, row height, editing infrastructure and focus rendering.
- `MultiSelect = false` / single-row selection because no issued bulk capability exists for this screen.
- Row/field editing is available only while the shipment is `Draft` and the issued edit capability/permission is available; outside Draft all line columns are read-only.
- No separate row-level permission or command is invented.
- Reference editors use `TransportLookup` only where the design semantic is Reference; exact domain provider/endpoint remains gated unless already issued.
- Width policy is semantic and CoreUI-owned: content-sized for compact reference/state/numeric columns and remaining-space/fill priority for descriptive text; no fixed pixel widths are specified.

### ItemsGrid — explicit design contract
Purpose: edit/view shipment item lines in Draft.

| Order | Key | Arabic Header | ValueType | Semantic | Required in editable row | Edit policy | Sortable design | Width policy | Editor |
|---:|---|---|---|---|---|---|---|---|---|
| 1 | `itemOrPackageRef` | مرجع الصنف/الطرد | Reference | issued item/package reference | Yes | Draft only | Yes* | CoreUI content-sized | `TransportLookup`; provider `TBD-GATED` |
| 2 | `description` | الوصف | String | issued description | No | Draft only | Yes* | CoreUI primary fill | text editor |
| 3 | `quantity` | الكمية | Decimal | issued quantity | Yes | Draft only | Yes* | CoreUI content-sized numeric | numeric editor |
| 4 | `weight` | الوزن | Decimal | issued mass/weight | No | Draft only | Yes* | CoreUI content-sized numeric | numeric editor; mass precision follows governing W1/A9 authority |
| 5 | `volume` | الحجم | Decimal | issued volume | No | Draft only | Yes* | CoreUI content-sized numeric | numeric editor; volume precision follows governing W1/A9 authority |
| 6 | `state` | الحالة | Enum | issued item state | Yes | Read-only | Yes* | CoreUI content-sized state | state display |

`*` Sort presentation is permitted by this design; actual server sort-key mapping remains `TBD-GATED`. No arbitrary database expression is authorized.

Grid behavior:
- `UsesServerPaging = true` because the issued typed ScreenDefinition explicitly requires server paging for `ItemsGrid`.
- Typed sort is enabled only after an allow-listed server mapping exists; the design does not invent sort keys.
- `SelectionPolicy = SingleRow`.
- Confirm requires at least one item line, as already issued.

### PackagesGrid — explicit design contract
Purpose: edit/view package lines owned by the Shipment aggregate without inventing a new package-to-item cardinality or persistence model.

| Order | Key | Arabic Header | ValueType | Semantic | Required in editable row | Edit policy | Sortable | Width policy | Editor |
|---:|---|---|---|---|---|---|---|---|---|
| 1 | `packageRef` | مرجع الطرد | Reference | shipment package reference | Yes | Draft only | No | CoreUI content-sized | `TransportLookup`; provider `TBD-GATED` |
| 2 | `weight` | الوزن | Decimal | package weight within issued measurement semantics | No | Draft only | No | CoreUI content-sized numeric | numeric editor; governing mass precision applies |
| 3 | `volume` | الحجم | Decimal | package volume within issued measurement semantics | No | Draft only | No | CoreUI content-sized numeric | numeric editor; governing volume precision applies |
| 4 | `state` | الحالة | Enum | issued package lifecycle/state | Yes | Read-only | No | CoreUI content-sized state | state display |

Grid behavior:
- `UsesServerPaging = false` as a screen-design choice for the embedded Draft package collection; this does not create or constrain an API route. If a future issued technical contract requires paging, that binding supersedes this design choice.
- `SelectionPolicy = SingleRow`.
- Package/item consistency remains a domain/API validation rule; the UI does not invent a cardinality, mapping formula or hidden relationship field.

### LegsGrid — explicit design contract
Purpose: edit/view the ordered operational stages/stations attached to the shipment while preserving the issued chronology requirement.

| Order | Key | Arabic Header | ValueType | Semantic | Required in editable row | Edit policy | Sortable | Width policy | Editor |
|---:|---|---|---|---|---|---|---|---|---|
| 1 | `sequence` | الترتيب | Integer | chronology order for the issued leg/stage collection | Yes | Draft only | No | CoreUI content-sized numeric | integer editor |
| 2 | `stageOrStationRef` | المرحلة/المحطة | Reference | issued operational stage/station reference | Yes | Draft only | No | CoreUI primary fill | `TransportLookup`; provider `TBD-GATED` |

Grid behavior:
- `UsesServerPaging = false` as a screen-design choice for the embedded Draft chronology collection; no API contract is created by this choice.
- `SelectionPolicy = SingleRow`.
- Chronology validity remains authoritative in domain/API validation. The UI does not invent dates, times, from/to pairs, route formulas or transition rules not already issued.

### Explicit non-inventions in FIELD_GRID
TEAM-D03 deliberately did **not** create:
- package-to-item cardinality or a hidden package-item FK;
- leg dates/times, from/to endpoints, route IDs or operational transition rules;
- API endpoints, DTO fields or provider identifiers;
- permissions, security scope or offline-write behavior;
- storage types, tables, columns, migrations or indexes.

## Tabs / Sections
| Order | Tab | Purpose | LayoutRole | Visibility |
|---:|---|---|---|---|
| 1 | General | shipment header/general data | Workspace child under Fill | issued |
| 2 | Items | item lines | ItemsGrid owns Fill | issued |
| 3 | Packages | package lines | PackagesGrid owns Fill | issued |
| 4 | Legs | operational stages/legs | LegsGrid owns Fill | issued |
| 5 | Audit | audit/context display | shared audit behavior | issued |

## Commands
| Command | Capability | Permission | Enabled when | Result |
|---|---|---|---|---|
| إنشاء | `F01.Shipment.Create` | `f01.shipment.create` | new Draft | `POST /api/v1/shipments` → ShipmentResource |
| تعديل مسودة | `F01.Shipment.UpdateDraft` | `f01.shipment.edit` | Draft + `expectedVersion` | `PATCH /api/v1/shipments/{shipmentId}` |
| تأكيد | `F01.Shipment.Confirm` | `f01.shipment.confirm` | Draft only | Draft → Confirmed; reason/audit contract applies |
| عرض | `F01.Shipment.Get` | `f01.shipment.view` | authorized scope | read ShipmentResource |
| بحث | `F01.Shipment.Search` | `f01.shipment.view` | authorized scope | paged search |

No legacy toolbar command is promoted.

## Lookups — TEAM-D03 PASS
Shared UI contract for `customerRef`, `originRef`, `destinationRef`, and reference-type grid editors:
- server-side debounced search via shared `TransportLookup` behavior;
- selected identity = `LookupItem.Id`;
- maximum results = `50`;
- `Code`, `DisplayName`, `SecondaryName` are presentation/search values only;
- permission, active/status and company/branch filtering remain server authority;
- no full-table client loading.

Screen semantics:
- `customerRef`: permitted customer.
- `originRef`: scope-valid location.
- `destinationRef`: scope-valid location.
- grid reference fields: bounded reference selection only where declared above.

Nonblocking `TBD-GATED` technical bindings:
- exact customer lookup source/action/endpoint/provider identifier;
- exact location lookup source/action/endpoint/provider identifier;
- exact item/package/stage lookup provider identifiers;
- exact screen-specific search keys beyond shared LookupItem values.

No API identifier is invented to fill these gates.

## Search / filters
Issued `ShipmentSearchQuery` filters:
- shipment number
- customer
- origin
- destination
- state
- date range

Shared paging authority: page size maximum `200`; filtering and sorting remain typed and allow-listed server-side. Client-provided company/branch scope is never authority.

## States and workflow
Business vocabulary: `Draft / Confirmed / Active / Completed / Cancelled`.

Issued screen transitions:
- Create → Draft.
- UpdateDraft → Draft only, with `expectedVersion`.
- Confirm → Draft → Confirmed, reason/audit contract.
- No transition command is invented for Active/Completed/Cancelled.

Concurrency:
- stale update/confirm → governing concurrency conflict;
- no silent overwrite;
- exact reload/recovery interaction belongs to TEAM-D04 / UX.

## Online / Offline classification
FLOW01 is `ONLINE_ONLY`:
- `OFFLINE_WRITE = 0`
- `Can Queue = NO`
- all issued writes are `ONLINE_AUTHORITATIVE`.

No outbox/replay/sync design is created.

## Validation and messages
Authority-backed conditions:
- customer permitted;
- origin/destination scope-valid;
- item lines nonempty on confirm;
- package/item consistency;
- leg chronology validity;
- expectedVersion required for update/confirm;
- server-side default-deny permission and scope enforcement.

Shared error mapping is inherited from approved W2/CoreUI contracts. No local error taxonomy is created.

## Accessibility / keyboard — input to TEAM-D04
- CoreUI accessibility/RTL/DPI/common grid keyboard infrastructure is inherited.
- Default `MultiSelect=false` is fixed by FIELD_GRID because no bulk capability is issued.
- Screen-specific default focus, Enter/Escape behavior, tab order, confirmation/recovery interactions and shortcut policy are TEAM-D04 responsibilities.
- No legacy shortcut may be promoted without authority.

## Acceptance criteria
1. Identity remains `FLOW01-W3-SCR-001 / SHP-001`, `Transaction / HeaderLines`.
2. Layout remains Header/MainData=Content, Tabs/Workspace=Fill, Lines/Grid=Fill, LocalException=None.
3. Shared Toolbar/Grid/Pagination/Audit/RTL/DPI behavior is inherited, not recreated.
4. Create/Edit/Confirm/View/Search remain bound only to issued capabilities/permissions/states.
5. Confirm with zero item lines remains invalid.
6. Stale version produces concurrency conflict; no silent overwrite.
7. Confirmed does not imply accounting posting, revenue recognition or commission.
8. All writes remain online-only; no queue/replay path.
9. Five issued tabs remain intact.
10. All three concrete grids have explicit screen-specific columns and `AutoGenerateColumns=false`.
11. ItemsGrid uses server paging and typed allow-listed sorting only after technical mapping exists.
12. PackagesGrid does not invent package-item cardinality; package/item consistency stays domain/API authority.
13. LegsGrid represents only chronology order + stage/station reference; no unissued timing/route model is invented.
14. Grid editing is Draft-only and single-row; no bulk capability is implied.
15. Shared lookup behavior uses server-side bounded search, Id identity and cap 50; exact unissued provider IDs remain gated.
16. No legacy SHP-005/006/007/008 field/column/command becomes governing by lineage.

`TAE-F01-001` remains `NOT RUN`; this document records design inputs, not runtime PASS.

## Known gated details after FIELD_GRID
Nonblocking technical bindings:
1. Exact lookup provider/endpoint/action IDs for customer/location/item/package/stage references.
2. Exact ItemsGrid server sort-key allow-list mapping.
3. Any future technical paging contract for PackagesGrid/LegsGrid; none is created here.

TEAM-D04 UX-stage work still required:
4. default focus;
5. Enter/Escape behavior;
6. tab order and focus return after modal lookup/validation;
7. confirmation behavior for Confirm;
8. loading/double-submit behavior;
9. concurrency conflict reload/retry path;
10. validation summary/field focus behavior;
11. empty/loading/error presentation per shared CoreUI;
12. exact minimum-click workflow within issued capabilities.

These UX items are not pre-decided by TEAM-D03.

## Evidence
- Layout authority decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.
- FIELD_GRID authority decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.
- Canonical field/grid evidence: this document.
- Visual: not yet produced; TEAM-D05 not reached.
- Independent review: not yet produced; TEAM-D06 not reached.
- Repository implementation: lineage evidence only.

## Stage history
### TEAM-D01 — ANALYSIS
- Verdict: `PASS`.
- Result: canonical identity, business boundary, fields, issued commands/states, online-only classification and acceptance inputs established.

### TEAM-D02 — LAYOUT
- Verdict: `PASS` after owner resolution.
- Result: Transaction/HeaderLines; Header/MainData Content; Tabs/Workspace Fill; Lines/Grid Fill; no LocalException.

### TEAM-D03 — FIELD_GRID
- Prior state: `HOLD_AUTHORITY` because complete concrete GridColumnDefinitions had not been issued.
- Owner decision: `2026-08-24_FLOW01-W3-SCR-001_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md` resolved that design-authority gap and made unissued technical lookup/provider IDs nonblocking.
- Verdict: `PASS`.
- CompletedStage: `FIELD_GRID`.
- Result: explicit ItemsGrid, PackagesGrid and LegsGrid design contracts; single-row/Draft edit policy; shared lookup UI contract; exact unissued technical providers remain `TBD-GATED` without invention.

## Handoff
- CurrentStage: `UX`.
- OwnerTeam: `TEAM-D04`.
- NextTeam: `TEAM-D04`.
- HandoffReadyToUX: `YES`.
- Required input: this canonical specification + current CoreUI/Profile/W2 error/lookup contracts.
- TEAM-D04 must not alter FIELD_GRID business semantics or introduce new commands/permissions/API contracts.

## Independent review
- Reviewer: pending; stage not reached.
- Verdict: pending.
- Findings: pending.
- ClosedAt: pending.

No application code, official kurrasa, DDL, migration, permission, API contract or offline-write authority is modified or created by this design artifact.
