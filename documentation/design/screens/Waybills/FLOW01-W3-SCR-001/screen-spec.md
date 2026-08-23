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
- Current approved CoreUI/Profile authority: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, and CoreUI architecture test contract: read.
- Legacy R2/V4 `SHP-005/006/007/008`: reconciled as non-governing lineage; no field/action/layout is imported merely because it exists there.
- Layout authority decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`: read and applied.

Readiness verdict for TEAM-D01 analysis: `COMPLETED`.
Readiness verdict for TEAM-D02 layout: `COMPLETED`.
Readiness verdict for TEAM-D03 field/grid stage: `READY_FOR_STAGE`, subject to the explicit `TBD-GATED` details recorded below.

## Identity
- ScreenCode: `FLOW01-W3-SCR-001`
- Current FLOW01 alias: `SHP-001`
- ArabicName: `إدخال البوليصة`
- English role: `Shipment Entry`
- Domain: `Waybills / FLOW01`
- GoverningKurrasaRefs: current FLOW01 W1/W2/W3 issuance and identity/trace artifacts listed in Authority chain.
- ScreenProfile: `Transaction`
- Variant: `HeaderLines`
- Capabilities: Create Draft, Update Draft, Confirm, Get, Search — exactly as issued below.
- CurrentDesignState: `FIELD_GRID`
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
8. Current approved CoreUI/Profile authority: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, CoreUI architecture tests.
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
- Fill owner: tabs/workspace occupies the remaining vertical space; line grids inside the relevant tabs use `Fill`.
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
│ where an issued line grid exists.                        │
└──────────────────────────────────────────────────────────┘
```

No local height, width, padding, color, font, toolbar, grid, pagination, audit, RTL, or DPI value is invented by this stage.

### Historical layout contradiction and resolution
`SRC-053` retained the issued structural text `Header(Fixed) → Tabs(Content) → LinesGrid(Fill)`. The approved shared Transaction/CoreUI authority instead governs `Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill)`.

On 2026-08-24 the project owner explicitly selected the current approved CoreUI/Transaction profile for this screen. Therefore:
- `Header = Content`;
- `Tabs/Workspace = Fill`;
- `Lines/Grid = Fill`;
- no `LocalException` is created;
- the earlier `Fixed/Content` labels remain historical evidence only and are not implemented as a screen-local deviation.

The former `HOLD_AUTHORITY` is resolved.

## Fields — authority-backed inventory
| FieldKey | Arabic Label | FieldProfile | ValueType | Required | Editable rule | Lookup / validation | Visibility rule | Authority/Evidence Ref |
|---|---|---|---|---|---|---|---|---|
| `shipmentId` | رقم البوليصة | KeyText | UUID | No | Read-only | server assigned | visible after identity exists | current FLOW01 typed definition |
| `customerRef` | العميل | Lookup | Reference | Yes | Draft only | permitted customer | normal | current FLOW01 typed definition |
| `originRef` | المصدر | Lookup | Reference | Yes | Draft only | scope-valid location | normal | current FLOW01 typed definition |
| `destinationRef` | الوجهة | Lookup | Reference | Yes | Draft only | scope-valid location | normal | current FLOW01 typed definition |
| `shipmentState` | الحالة | State | Enum | Yes | Read-only | W1/W2 lifecycle | normal | current FLOW01 typed definition |
| `itemLines` | البنود | LineGrid | Collection | Yes | Draft only | nonempty on confirm | Items tab | current FLOW01 typed definition |
| `packageLines` | الطرود | LineGrid | Collection | No | Draft only | package/item consistency | Packages tab | current FLOW01 typed definition |
| `legLines` | المراحل | LineGrid | Collection | No | Draft only | chronology validation | Legs tab | current FLOW01 typed definition |
| `expectedVersion` | إصدار التزامن | Hidden | Integer | Edit/confirm only | Read-only client token | required on update/confirm | hidden | W2 concurrency contract |

No additional field is promoted from the repository lineage implementation by this contract.

## Grids
### ItemsGrid — issued
Purpose: edit/view item lines for a Draft shipment within the issued transaction contract.

Authority-backed columns:
- item/package reference
- description
- quantity
- weight
- volume
- state

Other authority-backed behavior:
- explicit typed definition; no AutoGenerate-based design is promoted;
- server paging and typed sort are governing where issued by the contract;
- editable only while the shipment is Draft and only through authorized edit capability;
- confirm requires nonempty item lines.

Exact UI column keys/order/widths beyond the issued semantic set must be established by TEAM-D03 from exact authority; otherwise mark `TBD-GATED`.

### Packages grid — `TBD-GATED`
The detailed package-grid column inventory is not issued in the retrieved typed definition. TEAM-D03 must not infer it from legacy `SHP-007` or repository UI.

### Legs grid — `TBD-GATED`
The detailed legs-grid column inventory is not issued in the retrieved typed definition. TEAM-D03 must not infer it from legacy `SHP-008` or repository UI.

## Tabs / Sections
| Order | Tab/Section | Purpose | LayoutRole | Visibility Rule | Authority/Evidence Ref |
|---:|---|---|---|---|---|
| 1 | General | shipment header/general data | Workspace child under `Fill` | issued | FLOW01 typed definition + owner layout decision |
| 2 | Items | item lines | Workspace child; lines grid `Fill` | issued | FLOW01 typed definition + owner layout decision |
| 3 | Packages | package lines | Workspace child; grid details gated | issued | FLOW01 typed definition + owner layout decision |
| 4 | Legs | shipment legs | Workspace child; grid details gated | issued | FLOW01 typed definition + owner layout decision |
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

## Lookups
- `customerRef`: permitted customer.
- `originRef`: scope-valid location.
- `destinationRef`: scope-valid location.
- Exact server/cache behavior, search key, result cap, keyboard interaction and company/branch lookup UX beyond issued scope: `TBD-GATED` unless TEAM-D03 locates current authority.
- Client must not invent or submit company/branch scope authority; server/session scope is governing.

## Search / filters
Issued filters bind to `ShipmentSearchQuery`:
- shipment number
- customer
- origin
- destination
- state
- date range

No client-provided company/branch scope field is authorized.

## States and workflow
### Business states
`Draft / Confirmed / Active / Completed / Cancelled`.

### Issued transitions on this screen
- Create → new `Draft`.
- UpdateDraft → `Draft` only and requires `expectedVersion`.
- Confirm → only `Draft → Confirmed`, with reason/audit contract.
- `Active / Completed / Cancelled` are issued lifecycle vocabulary, but this screen does not invent transition commands for them.

### Conflict/reload behavior
- stale update/confirm → `version.conflict`;
- no silent overwrite;
- exact UI conflict interaction beyond governing error behavior is a later UX-stage responsibility and must remain authority-backed.

## Online / Offline classification
`SRC-053` classifies FLOW01 contracts as `ONLINE_ONLY`:
- `OFFLINE_WRITE = 0`
- `Can Queue = NO`
- no Outbox / ACK / Replay / sync contract is created by this screen design.

All issued state-changing actions are therefore `ONLINE_AUTHORITATIVE` for this design. No offline write path is exposed.

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
- Shared accessibility, RTL, DPI and focus infrastructure: inherited from current CoreUI where governed.
- Screen-specific default focus: `TBD-GATED` unless current authority is located.
- Screen-specific Enter behavior: `TBD-GATED` unless current authority is located.
- Screen-specific Escape behavior: `TBD-GATED` unless current authority is located.
- Exact tab order beyond semantic layout: TEAM-D04 responsibility after field/grid closure.
- No legacy shortcut is promoted from repository lineage without current authority.

## Acceptance criteria
1. The design identifies the screen only as `FLOW01-W3-SCR-001` with alias `SHP-001`, `Transaction / HeaderLines`.
2. Layout conforms to shared CoreUI: `Header/MainData=Content`, `Tabs/Workspace=Fill`, `Lines/Grid=Fill`; there is no local layout exception.
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
14. No field, command, permission, route, DTO, grid column, or layout behavior is imported from legacy `SHP-005/006/007/008` unless separately issued by current authority.
15. Missing Packages/Legs column details remain `TBD-GATED`; they are not guessed.

`TAE-F01-001` remains `NOT RUN`; these are design acceptance inputs, not runtime PASS.

## Known gated details for later stages
- Packages-grid detailed column inventory: `TBD-GATED`.
- Legs-grid detailed column inventory: `TBD-GATED`.
- Exact lookup search/caching/result-cap interaction: `TBD-GATED` unless current source is located.
- Any screen-specific keyboard shortcut beyond frozen CoreUI/profile behavior: `TBD-GATED` unless a current source is located.
- Any visual exception beyond CoreUI: none authorized.

## Evidence
- Wireframe/layout evidence: governed text wireframe in this canonical spec under `Layout contract`.
- Visual: not yet produced; TEAM-D05 stage.
- Review report: not yet produced; TEAM-D06 stage.
- Source references: Authority chain above.
- Repository evidence: current waybill implementation retained as lineage evidence only.
- Layout decision: `documentation/design/decisions/2026-08-24_FLOW01-W3-SCR-001_LAYOUT_AUTHORITY_DECISION.md`.

## Stage history
### TEAM-D01 — ANALYSIS
- Verdict: `PASS`
- CompletedStage: `ANALYSIS`
- Key result: identity, business boundary, fields, issued commands, states, online-only classification and acceptance inputs established from current authority.

### TEAM-D02 — LAYOUT
- Prior state: `HOLD_AUTHORITY` because `SRC-053` sizing labels contradicted current approved shared Transaction/CoreUI authority.
- Owner decision: 2026-08-24 selected shared CoreUI/Transaction composition; no LocalException.
- Layout verdict: `PASS`.
- CompletedStage: `LAYOUT`.
- Output: `Transaction/HeaderLines`, Header/MainData `Content`, Tabs/Workspace `Fill`, Lines/Grid `Fill`, shared CoreUI shell behavior inherited.

## Handoff
- InputVersion: `SRC-053 + current CoreUI/Profile authority + OWNER-DEC-2026-08-24`.
- OutputVersion: canonical `screen-spec.md` updated 2026-08-24.
- BlockingIssue: none for transition from LAYOUT to FIELD_GRID.
- NextTeam: `TEAM-D03`.
- HandoffReady: `YES`.

TEAM-D03 must preserve `TBD-GATED` for any package/leg grid detail or lookup behavior not supported by current authority.

## Independent review
- Reviewer: not yet assigned.
- Verdict: pending.
- Findings: pending.
- ClosedAt: pending.

No application code, official kurrasa, DDL, migration, permission, API contract, or offline-write authority is modified or created by this design artifact.
