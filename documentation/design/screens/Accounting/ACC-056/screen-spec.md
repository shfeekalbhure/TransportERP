# ACC-056 — القيود العكسية وربط القيد الأصلي بالقيد العاكس — Canonical Screen Specification

**English:** Reversal Entries  
**Module:** Accounting  
**Profile / Variant:** `Transaction / Reversal`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-14`

## Authority
- Current57 closed execution register + current baseline V1.1 + current V1.3 field/tab/grid crosswalk.
- W2: `ACC056.View/Create/Edit/Cancel/Reverse`; six endpoints: List/Get/Create/Update/Cancel + server reversal of original JournalEntry.
- W1 root: `ReversalLink`; secondary `JournalEntry/ApprovalRequest`.
- Current W3 explicitly governs `Transaction / Reversal` and the 9-display-column details grid.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and track a reversal request/link from an eligible original posted journal to its server-generated reversing journal.

Tabs:
1. البيانات الرئيسية
2. القيد الأصلي والقيد العاكس
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

Capabilities exactly View/Create/Edit/Cancel/Reverse. No Post, Print, Export, direct approval-decision command, attachment mutation, Delete or offline final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared Transaction host:
`Toolbar → Header/MainData(Content) → Tabs/Workspace(Fill) → Original/Reversal Details(Fill) → Actions(Content/Fixed) → Audit`.
No local toolbar/grid/RTL/DPI/validation/audit implementation.

## FIELD_GRID — TEAM-D03 PASS
### Governing fields
| # | UI design key | Arabic label | Type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `reversalRequestNumber` | رقم طلب العكس | Display | Automatic | ReadOnly / server generated |
| 2 | `originalEntryRef` | القيد الأصلي | Lookup/Reference | Required | Editable in eligible request context; provider TBD-GATED |
| 3 | `originalEntryNumber` | رقم القيد الأصلي | Display | Automatic | ReadOnly / source-derived |
| 4 | `originalEntryDate` | تاريخ القيد الأصلي | Display | Automatic | ReadOnly / source-derived |
| 5 | `reversalReason` | سبب العكس | Multiline Text | Required | Editable before execution |
| 6 | `reversalDate` | تاريخ العكس | Date | Required | Editable before execution; open-period rule server-side |
| 7 | `reversalPolicy` | سياسة العكس | Enum | Required | Editable only from server-governed choices |
| 8 | `reversingEntryRef` | القيد العاكس | Display | Automatic | ReadOnly; generated after execution |
| 9 | `reversingEntryNumber` | رقم القيد العاكس | Display | Automatic | ReadOnly |
| 10 | `reversalState` | حالة العكس | Workflow state | Automatic | ReadOnly |

### Original/Reversal Details Grid
Current W3 supplies exactly **9 display columns**:
`# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

This grid is **read-only** in ACC-056: the governing field inventory issues no line-edit fields for this screen, and W2 reversal execution is server-authoritative against the original JournalEntry. The UI must not allow manually changing generated reversing journal lines through this screen.

All reversal eligibility, policy semantics, period validity, resulting entry values and linkage remain server/domain-authoritative.

## UX — TEAM-D04 PASS
- Create/Edit maintain only the reversal request metadata while eligible; original entry selection is restricted to authorized posted/non-reversed candidates by server lookup/revalidation.
- Reverse executes only through the W2 reversal route; no client-generated reversal lines or accounting formula.
- Cancel applies only to eligible pending request state.
- Original and reversing entries remain immutable projections in the details tab.
- Approvals tab creates no direct Approve/Reject/Return command because ACC-056 W2 does not issue one.
- Attachments tab creates no mutation command without W2 authority.
- Concurrency/idempotency/reversal errors use shared presenters; no offline final write/outbox/replay.

## VISUAL — TEAM-D05 PASS
Use shared Transaction RTL/DPI/tab/detail-grid/state/reference presentation only. Original/reversing distinction is semantic content, not custom local colors or architecture.

## TEAM-D06 — INDEPENDENT REVIEW PASS
`PASS — 0 open design findings` per `documentation/design/batches/BATCH-14_INDEPENDENT_REVIEW_2026-08-24.md`.

Confirmed: 10 fields, five tabs, read-only nine-column detail grid, exact `View/Create/Edit/Cancel/Reverse` surface, original immutability, and server-generated reversal semantics.

## Remaining technical gates
Exact lookup provider, field persistence/DTO mappings, reversal-policy identifiers, sort bindings and runtime/release evidence remain separate and nonblocking for design approval.
