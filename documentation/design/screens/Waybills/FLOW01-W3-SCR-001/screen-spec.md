# FLOW01-W3-SCR-001 — إدخال البوليصة — Canonical Design Pilot

## Mandatory pre-read / evidence readiness
- Live queue: read and reconciled.
- Current governing kurrasa/current-design authority: read.
- Current canonical W3 identity/transition authority: read.
- `SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001`: read; current governing within FLOW01.
- W1 screen-level trace: read.
- W2 exact contract/security binding: read.
- Typed W3 ScreenDefinition: read.
- FLOW01 acceptance specification and atomic trace: read.
- Current repository waybill implementation: read as implementation evidence only.
- Current approved CoreUI/Profile authority for TEAM-D02: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, and CoreUI architecture test contract: read and reconciled as far as current authority permits.
- Legacy R2/V4 `SHP-005/006/007/008`: reconciled as non-governing lineage for this pilot; no field/action/layout is imported from them merely because it exists there.

Readiness verdict for TEAM-D01 analysis: `READY / COMPLETED`.
Readiness verdict for TEAM-D02 layout: `HOLD_AUTHORITY` — current issued FLOW01 layout conflicts with current approved frozen Transaction/CoreUI vertical-sizing authority; no approved reconciliation or LocalException was found.

## Identity
- Canonical ScreenCode: `FLOW01-W3-SCR-001`
- Current FLOW01 alias: `SHP-001`
- ArabicName: `إدخال البوليصة`
- English role: `Shipment Entry`
- Domain: `Waybills / FLOW01`
- ScreenProfile: `Transaction`
- Variant: `HeaderLines`
- CurrentDesignState: `HOLD_AUTHORITY`
- CompletedStage: `ANALYSIS`
- BlockedStage: `LAYOUT`
- OwnerTeam: `DESIGN-LEAD / ORCHESTRATOR`
- ResumeTeamWhenResolved: `TEAM-D02`

## Authority chain
1. `CHG-20260818-FLOW01-W3-ID-002` — canonical FLOW01 identity map: `FLOW01-W3-SCR-001 / SHP-001 / إدخال البوليصة / Transaction / HeaderLines`.
2. `SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001` — current FLOW01 W2/W3 issuance; no DDL/code/runtime authority.
3. `FLOW01_W1_SCREEN_LEVEL_TRACE_2026-08-22.md` — logical boundary only, no DDL.
4. `FLOW01_W2_EXACT_CONTRACT_AND_SECURITY_BINDING_2026-08-22.md` — exact actions/routes/DTO/permissions/scope.
5. `FLOW01-W3-SCR-001_TYPED_SCREENDEFINITION.md` — typed screen definition.
6. `FLOW01_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — `TAE-F01-001`, issued but not run.
7. `FLOW01_ATOMIC_TRACEABILITY_AND_E2E_SPEC_2026-08-22.md` — F01-RQ-001 → SCR-001 → W1 → W2 → permission/scope → TAE-F01-001.
8. Current approved CoreUI/Profile authority used by layout gate: `Transaction_Profile_Specification_V1.1`, `CoreUI_Containers_and_Layout_Specification_V1.1`, `ScreenProfile_Variant_Capability_Matrix_V1`, `ScreenDefinition_Contract_V1`, CoreUI architecture tests.

## Legacy / repository reconciliation
The current repository implementation uses `SHP-005` as "رأس البوليصة" and presents `SHP-005/006/007/008` as tabs. The current 2026-08-23 reviewed kurrasa material classifies those R2 identities as `NON-GOVERNING` / `ID-CONFLICT` material. They therefore remain implementation/lineage evidence only and do not define this canonical screen.

The canonical FLOW01 waybill-entry surface is `FLOW01-W3-SCR-001`, not `SHP-005`. Its issued tabs are `General | Items | Packages | Legs | Audit`.

## Business purpose and actors
Business purpose: create a Shipment as Draft, edit the Draft under optimistic concurrency, confirm Draft → Confirmed, and read/search shipments within server-authorized company/branch scope.

No business role name is invented here. Authority is expressed by the issued permission codes:
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

## Capabilities and commands
| Capability | Action ID | Method / Route | DTO | Permission | Enabled / state rule |
|---|---|---|---|---|---|
| إنشاء | `F01.Shipment.Create` | `POST /api/v1/shipments` | `CreateShipmentRequest → ShipmentResource` | `f01.shipment.create` | new Draft |
| تعديل مسودة | `F01.Shipment.UpdateDraft` | `PATCH /api/v1/shipments/{shipmentId}` | `UpdateShipmentDraftRequest → ShipmentResource` | `f01.shipment.edit` | Draft + `expectedVersion` |
| تأكيد | `F01.Shipment.Confirm` | `POST /api/v1/shipments/{shipmentId}:confirm` | `ConfirmShipmentRequest → ShipmentResource` | `f01.shipment.confirm` | Draft → Confirmed; reason/audit |
| عرض | `F01.Shipment.Get` | `GET /api/v1/shipments/{shipmentId}` | `ShipmentResource` | `f01.shipment.view` | authorized scope |
| بحث | `F01.Shipment.Search` | `GET /api/v1/shipments` | `ShipmentSearchQuery → PagedShipmentResult` | `f01.shipment.view` | authorized scope |

No other toolbar command is promoted from the current repository form by this analysis.

## Issued fields
| FieldKey | Arabic Label | FieldProfile | ValueType | Required | Editable rule | Lookup / validation |
|---|---|---|---|---|---|---|
| `shipmentId` | رقم البوليصة | KeyText | UUID | No | Read-only | server assigned |
| `customerRef` | العميل | Lookup | Reference | Yes | Draft only | permitted customer |
| `originRef` | المصدر | Lookup | Reference | Yes | Draft only | scope-valid location |
| `destinationRef` | الوجهة | Lookup | Reference | Yes | Draft only | scope-valid location |
| `shipmentState` | الحالة | State | Enum | Yes | Read-only | W1/W2 lifecycle |
| `itemLines` | البنود | LineGrid | Collection | Yes | Draft only | nonempty on confirm |
| `packageLines` | الطرود | LineGrid | Collection | No | Draft only | package/item consistency |
| `legLines` | المراحل | LineGrid | Collection | No | Draft only | chronology validation |
| `expectedVersion` | إصدار التزامن | Hidden | Integer | Edit/confirm only | Read-only client token | required on update/confirm |

## Issued tabs / regions
| Order | Tab / region | Authority-backed role |
|---:|---|---|
| 1 | General | shipment header/general data |
| 2 | Items | item lines |
| 3 | Packages | package lines |
| 4 | Legs | shipment legs |
| 5 | Audit | audit/context display |

`SRC-053` typed ScreenDefinition issues this structural text:
`Header(Fixed): shipment status, customer, origin, destination → Tabs(Content): General | Items | Packages | Legs | Audit → LinesGrid(Fill): items/packages/legs`.

That text is retained verbatim as issued evidence, but TEAM-D02 may not implement or normalize the `Fixed`/`Content` sizing labels while the authority contradiction below remains open.

## Grid contract known at analysis exit
Authoritative `ItemsGrid` columns:
- item/package reference
- description
- quantity
- weight
- volume
- state

Other issued behavior: `AutoGenerate=false` is implied by the typed explicit definition; server paging and typed sort; selection may be single/multi only as an issued capability requires.

Exact column sets for a separate Packages grid or Legs grid are **not issued in the retrieved typed definition**. They remain `TBD-GATED` for TEAM-D03 and must not be invented. They are not the cause of the current TEAM-D02 layout hold.

## Search / filters
Issued filters bind to `ShipmentSearchQuery`:
- shipment number
- customer
- origin
- destination
- state
- date range

No client-provided company/branch scope field is authorized; scope is server/session derived.

## State and concurrency analysis
- Create → new `Draft`.
- UpdateDraft → `Draft` only and requires `expectedVersion`.
- Confirm → only `Draft → Confirmed`, with reason/audit contract.
- `Active / Completed / Cancelled` are issued lifecycle vocabulary, but this screen analysis does not invent transition commands for them.
- Stale update/confirm → `version.conflict`.

## Online / offline classification
`SRC-053` classifies all FLOW01 contracts as `ONLINE_ONLY`:
- `OFFLINE_WRITE = 0`
- `Can Queue = NO`
- no Outbox / ACK / Replay / sync contract is created by this screen design.

## Validation / error contract
Authority-backed conditions include:
- customer must be permitted;
- origin/destination must be scope-valid locations;
- item lines must be nonempty on confirm;
- package/item consistency;
- leg chronology validation;
- `expectedVersion` on update/confirm;
- server-side default-deny permission and scope enforcement.

Common issued FLOW01 errors applicable when returned by the contract include `validation.failed`, `not_found`, `forbidden`, `scope.denied`, `version.conflict`, `idempotency.conflict`, and `invalid.transition`. No screen-local replacement error taxonomy is created.

## Acceptance criteria — TEAM-D01 output
1. The design identifies the screen only as `FLOW01-W3-SCR-001` with alias `SHP-001`, `Transaction / HeaderLines`.
2. Create is available only under `f01.shipment.create` and produces a Draft through `F01.Shipment.Create`.
3. Draft edit is available only under `f01.shipment.edit` and requires `expectedVersion`.
4. Confirm is available only under `f01.shipment.confirm`, only for Draft → Confirmed, and preserves reason/audit behavior.
5. Read/search requires `f01.shipment.view` and server-authorized scope.
6. Confirm with no item lines is represented as invalid and maps to the governing validation contract.
7. A stale version is represented as a concurrency conflict; no silent overwrite is designed.
8. Repeated state-changing requests preserve the governing idempotency contract.
9. Confirmed produces a CommercialCharge reference/commitment only; the design does not show or imply automatic journal posting, revenue recognition, or commission creation.
10. All write actions are online-only; the design exposes no offline queue/replay path.
11. The five issued tabs and their business roles remain intact; their vertical sizing remains blocked pending authority reconciliation.
12. No field, command, permission, route, DTO, grid column, or layout behavior is imported from legacy `SHP-005/006/007/008` unless separately issued by current authority.

`TAE-F01-001` remains `NOT RUN`; these are design acceptance inputs, not runtime PASS.

## Known gated details for later stages
- Packages-grid detailed column inventory: `TBD-GATED`.
- Legs-grid detailed column inventory: `TBD-GATED`.
- Any screen-specific keyboard shortcut beyond frozen CoreUI/profile behavior: `TBD-GATED` unless a current source is located.
- Any visual exception beyond CoreUI: none authorized.

## Historical handoff — ANALYSIS → LAYOUT
- CompletedBy: `TEAM-D01` under DESIGN-LEAD orchestration
- AnalysisVerdict: `PASS FOR LAYOUT`
- OriginalNextTeam: `TEAM-D02`
- OriginalNextState: `LAYOUT`
- AnalysisHandoffReady: `YES`

This handoff remains valid as an analysis-stage record. TEAM-D02 then performed the mandatory CoreUI/Profile reconciliation and discovered the blocking contradiction below.

## TEAM-D02 layout authority reconciliation — HOLD_AUTHORITY
- ReconciledOn: `2026-08-24`
- Stage: `LAYOUT`
- StageOwner: `TEAM-D02` under DESIGN-LEAD orchestration
- LayoutVerdict: `HOLD_AUTHORITY`
- NextTeamAfterResolution: `TEAM-D02`

### Exact contradiction
1. Current FLOW01 typed definition issued under `SRC-053` states `Header(Fixed)` and `Tabs(Content)` for `FLOW01-W3-SCR-001`.
2. Current approved `Transaction_Profile_Specification_V1.1` states `Header = Content`, `Lines workspace = Fill`, `Totals/Actions = Content/Fixed`; its reconciliation also fixes Transaction MainData/Header to `Content` and preserves Lines/Grid as `Fill`.
3. Current approved `CoreUI_Containers_and_Layout_Specification_V1.1` states the Transaction layering as `Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) + Totals(Content) + Actions(Content/Fixed)`.
4. Current approved ScreenProfile/Variant matrix likewise records Transaction header as `Content`, workspace as `Fill`, and `HeaderLines` as the approved Transaction variant.
5. `ScreenDefinition_Contract_V1` permits only `Fixed | Content | Fill` through governed LayoutRoles and requires a documented `LocalException`/approval for a screen deviation; it does not allow a screen definition silently to redefine frozen shared architecture.
6. No current approved reconciliation, Change Request, or `LocalException` authorizing `Header(Fixed)` / `Tabs(Content)` for this screen was found in the governing material reviewed for this stage.

### Non-guess decision
TEAM-D02 will not silently choose either source, reinterpret `Fixed` as `Content`, or normalize `Tabs(Content)` into a `Fill` workspace. That would invent precedence or a local exception. Therefore LAYOUT cannot be closed safely.

### What is already safe and retained
The following facts remain authority-backed and are not disputed by this hold:
- identity `FLOW01-W3-SCR-001 / SHP-001`;
- `Transaction / HeaderLines`;
- five business tabs `General | Items | Packages | Legs | Audit`;
- lines grid is the primary work area and uses `Fill`;
- shared CoreUI Toolbar/Grid/Pagination/Audit styling is not recreated locally;
- RTL/DPI behavior comes from CoreUI;
- W2 permissions/state predicates remain server-authoritative;
- repository `SHP-005` layout remains lineage/implementation evidence only.

### Required authority to unblock
An authority-backed reconciliation must explicitly establish the governing vertical sizing/composition for this screen, specifically:
- whether `Header` is `Content` or an approved `Fixed` exception;
- whether the tab workspace is `Fill` or an approved `Content` exception;
- if deviating from the frozen Transaction profile, the governing `LocalException` / CR / owner decision and scope.

Until that exists, state remains `HOLD_AUTHORITY`; no UX/UI/visual stage may consume a guessed layout.

No application code, official kurrasa, DDL, migration, permission, API contract, or offline-write authority is modified or created by this design artifact.
