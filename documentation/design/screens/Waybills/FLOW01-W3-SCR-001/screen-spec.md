# FLOW01-W3-SCR-001 — إدخال البوليصة — Canonical Screen Specification

## Mandatory pre-read / evidence readiness
- Queue row read: `YES`.
- Current governing kurrasa/current-design authority: read and reconciled.
- Current canonical W3 identity/transition authority: read.
- `SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001`: read; governing within FLOW01 for issued W2/W3 facts.
- W1 screen-level trace: read.
- W2 exact contract/security binding: read.
- Typed W3 ScreenDefinition: read.
- FLOW01 acceptance specification and atomic trace: read.
- Current repository waybill implementation: read as implementation/lineage evidence only.
- Current approved CoreUI/Profile authority: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `CoreUI_Controls_Catalog_V1.2`, `Shared_API_Error_Paging_Lookup_Contracts_TransportERP_V1.3`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, and CoreUI architecture tests: read.
- Legacy R2/V4 `SHP-005/006/007/008`: reconciled as non-governing lineage; no field/action/layout is imported merely because it exists there.
- Layout authority decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`: read and applied.

Readiness verdict for TEAM-D01 analysis: `COMPLETED`.
Readiness verdict for TEAM-D02 layout: `COMPLETED`.
Readiness verdict for TEAM-D03 field/grid stage: `HOLD_AUTHORITY` at stage exit because concrete GridColumnDefinitions required by the current approved ScreenDefinition contract are not fully issued for this screen.

## Identity
- ScreenCode: `FLOW01-W3-SCR-001`
- Current FLOW01 alias: `SHP-001`
- ArabicName: `إدخال البوليصة`
- English role: `Shipment Entry`
- Domain: `Waybills / FLOW01`
- GoverningKurrasaRefs: current FLOW01 W1/W2/W3 issuance and identity/trace artifacts listed below.
- ScreenProfile: `Transaction`
- Variant: `HeaderLines`
- Capabilities: Create Draft, Update Draft, Confirm, Get, Search — exactly as issued below.
- CurrentDesignState: `HOLD_AUTHORITY`
- BlockedStage: `FIELD_GRID`
- CompletedStages: `ANALYSIS`, `LAYOUT`
- OwnerTeam: `TEAM-D03`

## Authority chain
1. `CHG-20260818-FLOW01-W3-ID-002` — canonical FLOW01 identity map: `FLOW01-W3-SCR-001 / SHP-001 / إدخال البوليصة / Transaction / HeaderLines`.
2. `SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001` — current FLOW01 W2/W3 issuance; no DDL/code/runtime authority.
3. `FLOW01_W1_SCREEN_LEVEL_TRACE_2026-08-22.md` — logical boundary only; no DDL authority.
4. `FLOW01_W2_EXACT_CONTRACT_AND_SECURITY_BINDING_2026-08-22.md` — exact actions/routes/DTO/permissions/scope.
5. `FLOW01-W3-SCR-001_TYPED_SCREENDEFINITION.md` — typed screen definition.
6. `FLOW01_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — `TAE-F01-001`, issued but not run.
7. `FLOW01_ATOMIC_TRACEABILITY_AND_E2E_SPEC_2026-08-22.md` — F01-RQ-001 → SCR-001 → W1 → W2 → permission/scope → TAE-F01-001.
8. Current approved CoreUI/Profile authority: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `CoreUI_Controls_Catalog_V1.2`, `Shared_API_Error_Paging_Lookup_Contracts_TransportERP_V1.3`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, CoreUI architecture tests.
9. Project-owner layout resolution dated 2026-08-24: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.

## Legacy / repository reconciliation
The current repository implementation uses `SHP-005` as `رأس البوليصة` and presents `SHP-005/006/007/008` as tabs. The reviewed current kurrasa material classifies those R2 identities as `NON-GOVERNING / ID-CONFLICT` material. They remain implementation/lineage evidence only and do not define this canonical screen.

The canonical FLOW01 waybill-entry surface is `FLOW01-W3-SCR-001`, not repository-lineage `SHP-005`. Its issued tabs are `General | Items | Packages | Legs | Audit`.

## Purpose and actors
- Business purpose: create a Shipment as Draft, edit the Draft under optimistic concurrency, confirm Draft → Confirmed, and read/search shipments within server-authorized company/branch scope.
- Primary roles: no business role name is invented here; authority is represented only by issued permissions.
- Entry points: create, get, search through issued FLOW01 contracts.
- Preconditions: server-authorized scope and the action-specific permission/state rules below.

Issued permission codes:
- `f01.shipment.create`
- `f01.shipment.edit`
- `f01.shipment.confirm`
- `f01.shipment.view`

Server scope is authoritative; the client does not assert company/branch scope.

## W1 logical boundary
- `Shipment`
- `ShipmentItem`
- `ShipmentPackage`
- `ShipmentLeg`

Issued lifecycle vocabulary visible to this contract: `Draft / Confirmed / Active / Completed / Cancelled`.

Issued write boundary for this screen: create/edit `Draft`, then `Confirm`. `Confirmed` creates a commercial commitment/reference only; it does not post a journal, recognize revenue, or create commission.

## Layout contract — TEAM-D02 completed
- Screen shell: governed `Transaction / HeaderLines` shell.
- Header/MainData role: `Content`.
- Workspace role: `Fill`.
- Fill owner: tabs/workspace occupies the remaining vertical space; line grids inside relevant tabs use `Fill`.
- Summary/Action/Audit regions: shared CoreUI/Profile behavior only; no local duplicate toolbar, pagination, audit chrome, or sizing system is introduced.
- RTL notes: inherit current CoreUI RTL behavior; no screen-local RTL exception.
- DPI/resize notes: inherit current CoreUI DPI/resize behavior; no screen-local pixel dimensions are authorized here.
- Local exception: `NONE`.
- Approval reference: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.

### Governed composition
```text
Transaction / HeaderLines shell
┌──────────────────────────────────────────────────────────┐
│ Shared CoreUI command / shell regions                    │
├──────────────────────────────────────────────────────────┤
│ Header / MainData — Content                              │
│ status | customer | origin | destination                 │
├──────────────────────────────────────────────────────────┤
│ Tabs / Workspace — Fill                                  │
│ General | Items | Packages | Legs | Audit                │
│                                                          │
│ Relevant Lines/Grid content owns Fill inside workspace   │
└──────────────────────────────────────────────────────────┘
```

No local height, width, padding, color, font, toolbar, grid, pagination, audit, RTL, or DPI value is invented.

### Historical layout contradiction and resolution
`SRC-053` retained historical structural text `Header(Fixed) → Tabs(Content) → LinesGrid(Fill)`. On 2026-08-24 the project owner explicitly selected the current approved shared Transaction/CoreUI composition:
- `Header/MainData = Content`;
- `Tabs/Workspace = Fill`;
- `Lines/Grid = Fill`;
- no `LocalException`.

The former layout `HOLD_AUTHORITY` is resolved and remains closed.

## Fields — TEAM-D03 verified authority-backed inventory
| FieldKey | Arabic Label | FieldProfile | ValueType | Required | Editable rule | Lookup / validation | Visibility rule | Authority/Evidence Ref |
|---|---|---|---|---|---|---|---|---|
| `shipmentId` | رقم البوليصة | KeyText | UUID | No | Read-only | server assigned | visible after identity exists | FLOW01 typed definition |
| `customerRef` | العميل | Lookup | Reference | Yes | Draft only | permitted customer | normal | FLOW01 typed definition |
| `originRef` | المصدر | Lookup | Reference | Yes | Draft only | scope-valid location | normal | FLOW01 typed definition |
| `destinationRef` | الوجهة | Lookup | Reference | Yes | Draft only | scope-valid location | normal | FLOW01 typed definition |
| `shipmentState` | الحالة | State | Enum | Yes | Read-only | W1/W2 lifecycle | normal | FLOW01 typed definition |
| `itemLines` | البنود | LineGrid | Collection | Yes | Draft only | nonempty on confirm | Items tab | FLOW01 typed definition |
| `packageLines` | الطرود | LineGrid | Collection | No | Draft only | package/item consistency | Packages tab | FLOW01 typed definition |
| `legLines` | المراحل | LineGrid | Collection | No | Draft only | chronology validation | Legs tab | FLOW01 typed definition |
| `expectedVersion` | إصدار التزامن | Hidden | Integer | Edit/confirm only | Read-only client token | required on update/confirm | hidden | W2 concurrency contract |

No additional field is promoted from repository-lineage implementation.

## Grids — TEAM-D03 authority review
### Current CoreUI/ScreenDefinition rule
The current approved `ScreenDefinition_Contract_V1` requires explicit `GridColumnDefinition` content for each concrete grid and treats omitted concrete grid columns as a definition-incomplete failure. The current Transaction profile assigns screen-specific column semantics/types/read-only/required/order/width-policy/lookup-editor metadata to `ScreenDefinition.GridColumns`; CoreUI owns shared grid rendering, RTL, fill, styling and common edit/selection states.

Therefore TEAM-D03 may not complete `FIELD_GRID` while concrete grid column definitions are absent.

### ItemsGrid — semantic inventory issued, full column definition not yet issued
Purpose: edit/view item lines for a Draft shipment within the issued transaction contract.

Issued semantic column set:
- item/package reference
- description
- quantity
- weight
- volume
- state

Issued behavior:
- explicit grid definition is required; legacy AutoGenerate behavior is not authority;
- server paging and typed sort are governing where issued;
- editable only while shipment is Draft and through authorized edit capability;
- confirm requires nonempty item lines.

Still required before FIELD_GRID PASS:
- exact column keys/labels mapped to each issued semantic;
- exact ValueType for each column;
- exact Required/ReadOnly/edit policy per column;
- exact order and width policy;
- exact lookup/editor binding where applicable;
- definitive selection policy where the typed definition says single/multi only "as capability requires".

These values are `TBD-GATED / BLOCKING` and must not be inferred.

### PackagesGrid — `TBD-GATED / BLOCKING`
The screen has an issued `packageLines` collection and Packages tab, but the current FLOW01 typed definition does not issue a concrete package-grid column inventory. Because the ScreenDefinition contract requires columns for a concrete grid, TEAM-D03 cannot invent package columns from legacy `SHP-007`, historical kurrasa candidates, DTO names, or repository UI.

Required authority: an issued Package grid column contract including column key/label/semantic/ValueType/Required/ReadOnly or edit policy/order/width policy/lookup-editor where applicable, plus selection policy.

### LegsGrid — `TBD-GATED / BLOCKING`
The screen has an issued `legLines` collection and Legs tab, but the current FLOW01 typed definition does not issue a concrete legs-grid column inventory. Because the ScreenDefinition contract requires columns for a concrete grid, TEAM-D03 cannot infer chronology/route columns from legacy `SHP-008`, DTO names, or repository UI.

Required authority: an issued Legs grid column contract including column key/label/semantic/ValueType/Required/ReadOnly or edit policy/order/width policy/lookup-editor where applicable, plus selection policy.

## Tabs / Sections
| Order | Tab/Section | Purpose | LayoutRole | Visibility Rule | Authority/Evidence Ref |
|---:|---|---|---|---|---|
| 1 | General | shipment header/general data | Workspace child under `Fill` | issued | FLOW01 typed definition + owner layout decision |
| 2 | Items | item lines | Workspace child; lines grid `Fill` | issued | FLOW01 typed definition + owner layout decision |
| 3 | Packages | package lines | Workspace child; concrete grid columns authority-gated | issued | FLOW01 typed definition + owner layout decision |
| 4 | Legs | shipment legs | Workspace child; concrete grid columns authority-gated | issued | FLOW01 typed definition + owner layout decision |
| 5 | Audit | audit/context display | Workspace child; shared audit behavior inherited | issued | FLOW01 typed definition + CoreUI |

## Commands
| Command | Capability | Permission Need | Enabled When | Confirmation | Result | Authority/Evidence Ref |
|---|---|---|---|---|---|---|
| إنشاء | `F01.Shipment.Create` | `f01.shipment.create` | new Draft | contract-governed | `POST /api/v1/shipments` → `ShipmentResource` | W2 exact binding |
| تعديل مسودة | `F01.Shipment.UpdateDraft` | `f01.shipment.edit` | Draft + `expectedVersion` | contract-governed | `PATCH /api/v1/shipments/{shipmentId}` | W2 exact binding |
| تأكيد | `F01.Shipment.Confirm` | `f01.shipment.confirm` | Draft only | reason/audit contract applies | Draft → Confirmed | W2 exact binding |
| عرض | `F01.Shipment.Get` | `f01.shipment.view` | authorized scope | none invented | read `ShipmentResource` | W2 exact binding |
| بحث | `F01.Shipment.Search` | `f01.shipment.view` | authorized scope | none invented | paged search | W2 exact binding |

No other toolbar command is promoted from the current repository form.

## Lookups — TEAM-D03 verified shared mechanics
The current approved shared W2/CoreUI lookup contract resolves common lookup mechanics:
- `TransportLookup` uses debounced server-side search for non-trivial sets;
- `MaximumLookupResults = 50` is the authoritative server cap;
- no full-table client loading is allowed;
- selected identity is `LookupItem.Id`;
- `Code`, `DisplayName`, `SecondaryName` are search/presentation values and never replace identity;
- permission, active/status filtering, and Company/Branch scope are enforced server-side;
- client context does not become scope authority.

Screen-specific lookup semantics remain:
- `customerRef`: permitted customer.
- `originRef`: scope-valid location.
- `destinationRef`: scope-valid location.

Still `TBD-GATED` because no exact current FLOW01 source was located:
- domain-specific lookup source/action/endpoint for `customerRef`;
- domain-specific lookup source/action/endpoint for `originRef` and `destinationRef`;
- exact screen-specific search keys beyond the shared `Id/Code/DisplayName/SecondaryName` contract.

These lookup-source gaps are not used to invent an API. If the owner/technical authority intends them to be nonblocking at design time, that disposition must be explicit; otherwise they remain part of the TEAM-D03 authority gap.

## Search / filters
Issued filters bind to `ShipmentSearchQuery`:
- shipment number
- customer
- origin
- destination
- state
- date range

Read query transport supports cursor/page/pageSize, stable sort, and typed filters. Server page authority remains governing; no client-provided company/branch scope field is authorized.

## States and workflow
### Business states
`Draft / Confirmed / Active / Completed / Cancelled`.

### Issued transitions on this screen
- Create → new `Draft`.
- UpdateDraft → `Draft` only and requires `expectedVersion`.
- Confirm → only `Draft → Confirmed`, with reason/audit contract.
- `Active / Completed / Cancelled` are issued lifecycle vocabulary, but this screen does not invent transition commands for them.

### Conflict/reload authority
- stale update/confirm → `version.conflict`;
- no silent overwrite;
- exact screen interaction remains TEAM-D04 UX-stage responsibility after FIELD_GRID closure.

## Online / Offline classification
`SRC-053` classifies FLOW01 contracts as `ONLINE_ONLY`:
- `OFFLINE_WRITE = 0`
- `Can Queue = NO`
- no Outbox / ACK / Replay / sync contract is created by this design.

All issued state-changing actions are `ONLINE_AUTHORITATIVE`. No offline write path is exposed.

## Validation and messages
Authority-backed conditions include:
- customer must be permitted;
- origin/destination must be scope-valid locations;
- item lines must be nonempty on confirm;
- package/item consistency;
- leg chronology validation;
- `expectedVersion` on update/confirm;
- server-side default-deny permission and scope enforcement.

Common issued FLOW01 errors applicable when returned by the contract include:
- `validation.failed`
- `not_found`
- `forbidden`
- `scope.denied`
- `version.conflict`
- `idempotency.conflict`
- `invalid.transition`

No screen-local replacement error taxonomy is created.

## Accessibility / keyboard
- Shared accessibility, RTL, DPI and common grid keyboard infrastructure: inherited from current CoreUI where governed.
- Screen-specific default focus: `TBD-GATED` unless current authority is located.
- Screen-specific Enter behavior: `TBD-GATED` unless current authority is located.
- Screen-specific Escape behavior: `TBD-GATED` unless current authority is located.
- Exact tab order beyond semantic layout: TEAM-D04 responsibility only after FIELD_GRID closure.
- No legacy shortcut is promoted from repository lineage without current authority.

## Acceptance criteria
1. The design identifies the screen only as `FLOW01-W3-SCR-001` with alias `SHP-001`, `Transaction / HeaderLines`.
2. Layout conforms to shared CoreUI: `Header/MainData=Content`, `Tabs/Workspace=Fill`, `Lines/Grid=Fill`; no LocalException exists.
3. Shared Toolbar/Grid/Pagination/Audit, RTL and DPI behavior are inherited rather than recreated locally.
4. Create is available only under `f01.shipment.create` and produces a Draft through `F01.Shipment.Create`.
5. Draft edit is available only under `f01.shipment.edit` and requires `expectedVersion`.
6. Confirm is available only under `f01.shipment.confirm`, only for Draft → Confirmed, and preserves reason/audit behavior.
7. Read/search requires `f01.shipment.view` and server-authorized scope.
8. Confirm with no item lines is invalid and maps to the governing validation contract.
9. A stale version is represented as a concurrency conflict; no silent overwrite is designed.
10. Repeated state-changing requests preserve the governing idempotency contract.
11. Confirmed produces a CommercialCharge reference/commitment only; the design does not show or imply automatic journal posting, revenue recognition, or commission creation.
12. All write actions are online-only; the design exposes no offline queue/replay path.
13. The five issued tabs and their business roles remain intact.
14. No field, command, permission, route, DTO, grid column, lookup binding, or layout behavior is imported from legacy `SHP-005/006/007/008` unless separately issued by current authority.
15. `FIELD_GRID` cannot PASS until concrete screen-specific GridColumnDefinitions are authority-backed for ItemsGrid, PackagesGrid and LegsGrid.
16. Shared lookup mechanics use server-side bounded lookup with maximum 50 results and identity by `Id`; no competing local limit or full-table loading is introduced.

`TAE-F01-001` remains `NOT RUN`; these are design acceptance inputs, not runtime PASS.

## Known gated details
### Blocking FIELD_GRID authority gaps
1. Complete `GridColumnDefinition` contract for `ItemsGrid` — exact per-column definition metadata beyond the issued semantic set.
2. Complete `GridColumnDefinition` contract for `PackagesGrid`.
3. Complete `GridColumnDefinition` contract for `LegsGrid`.

### Gated lookup-source details
4. Exact domain lookup source/action/search-key binding for `customerRef`.
5. Exact domain lookup source/action/search-key binding for `originRef` and `destinationRef`.

### Later-stage gated UX details
6. Any screen-specific default focus, Enter, Escape, tab-order exception or shortcut beyond frozen CoreUI/profile behavior.
7. Any visual exception beyond CoreUI: none authorized.

## Exact authority required to resume TEAM-D03
The owner/technical authority must issue or approve a screen-specific FIELD_GRID contract for `FLOW01-W3-SCR-001` that supplies, without inference:
- for each concrete grid (`ItemsGrid`, `PackagesGrid`, `LegsGrid`): column key, label, semantic, ValueType, Required/ReadOnly or edit policy, order, width policy, lookup/editor binding where applicable, and selection policy;
- exact domain lookup source/action/search-key bindings for `customerRef`, `originRef`, `destinationRef`, **or** an explicit decision that those lookup-source bindings may remain `TBD-GATED` and nonblocking for FIELD_GRID while only the shared W2/CoreUI lookup mechanics govern.

No values are assumed in this document.

## Evidence
- Wireframe/layout evidence: governed text wireframe under `Layout contract`.
- Field/grid evidence: TEAM-D03 authority review in this canonical spec.
- Visual: not produced; TEAM-D05 stage not reached.
- Independent review report: not produced; TEAM-D06 stage not reached.
- Source references: Authority chain above.
- Repository evidence: current legacy waybill implementation retained as lineage evidence only.
- Layout decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.

## Stage history
### TEAM-D01 — ANALYSIS
- Verdict: `PASS`
- CompletedStage: `ANALYSIS`
- Key result: identity, business boundary, fields, issued commands, states, online-only classification and acceptance inputs established from current authority.

### TEAM-D02 — LAYOUT
- Prior state: `HOLD_AUTHORITY` because `SRC-053` sizing labels contradicted current approved shared Transaction/CoreUI authority.
- Owner decision: 2026-08-24 selected shared CoreUI/Transaction composition; no LocalException.
- Verdict: `PASS`.
- CompletedStage: `LAYOUT`.
- Output: `Transaction/HeaderLines`, Header/MainData `Content`, Tabs/Workspace `Fill`, Lines/Grid `Fill`, shared CoreUI shell behavior inherited.

### TEAM-D03 — FIELD_GRID
- Verdict: `HOLD_AUTHORITY`.
- CompletedStage: `NO`.
- Verified: issued field inventory; ItemsGrid semantic column set; tab/grid ownership; Draft edit boundary; server paging/typed sort where issued; shared lookup mechanics and `MaximumLookupResults=50`.
- Blocking finding: current approved ScreenDefinition contract requires explicit columns for each concrete grid, but the current FLOW01 issuance does not supply complete concrete GridColumnDefinitions for ItemsGrid/PackagesGrid/LegsGrid.
- Additional gated finding: exact FLOW01 domain lookup source/search-key bindings for customer/location are not issued in the retrieved authority.
- No legacy repository definition was promoted to fill either gap.

## Handoff
- CurrentStage: `HOLD_AUTHORITY` at `FIELD_GRID`.
- OwnerTeam: `TEAM-D03`.
- NextTeam: `TEAM-D03` after authority is issued.
- HandoffReadyToUX: `NO`.
- Resume condition: exact authority listed under `Exact authority required to resume TEAM-D03`.

## Independent review
- Reviewer: not yet assigned.
- Verdict: pending; stage not reached.
- Findings: pending.
- ClosedAt: pending.

No application code, official kurrasa, DDL, migration, permission, API contract, or offline-write authority is modified or created by this design artifact.