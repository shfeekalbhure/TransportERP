# ACC-057 — طلبات الاعتماد المحاسبية — Canonical Screen Specification

**English:** Accounting Approval Requests  
**Module:** Accounting  
**Profile / Variant:** `ControlApproval / ApprovalQueue`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-14`

## Authority
- Current57 closed execution register + current baseline V1.1 + current V1.3 field/tab crosswalk.
- W2: `ACC057.View/Execute/Approve/Reject/Return`; six endpoints: queue list/get, generic action request, approve/reject/return.
- W1/root: `ApprovalRequest`; secondary `ApprovalAction + target documents`; read model = approval queue/details.
- ControlApproval shared foundation: Queue Grid/DetailsHost is shared workspace; current ACC-057 screen contract has **no screen-specific grid-column inventory**.

## ANALYSIS — TEAM-D01 PASS
Purpose: present accounting approval requests within authorized scope, show linked target context and record server-authoritative decision actions with SoD.

Tabs:
1. الطلب
2. المستندات المرتبطة
3. قرار الاعتماد
4. سجل الاعتماد والتدقيق

Capabilities exactly View, Execute, Approve, Reject, Return. No Reopen, Create/Edit/Delete, Print/Export, attachment mutation or offline final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared ControlApproval ApprovalQueue foundation:
`Toolbar → Queue Grid/DetailsHost(Fill) → Decision Header/Reason(Content) → Tabs/History/Audit`.

The queue host is a shared CoreUI/foundation component. **No local ACC-057 business columns are invented**, because the current screen contract explicitly marks concrete grid columns N/A/not applicable. Any shared foundation columns remain governed by the ControlApproval component contract rather than this ScreenDefinition.

## FIELD_GRID — TEAM-D03 PASS
### Governing fields
| # | UI design key | Arabic label | Type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `requestNumber` | رقم الطلب | Display | Automatic | ReadOnly |
| 2 | `documentTypeRef` | نوع المستند | Reference display | Required context | ReadOnly / request-derived |
| 3 | `linkedDocumentRef` | المستند المرتبط | Reference display | Required context | ReadOnly / request-derived; authorized target scope |
| 4 | `requestedBy` | مقدم الطلب | Display | Automatic | ReadOnly |
| 5 | `requestedAt` | وقت الطلب | Display/DateTime | Automatic | ReadOnly |
| 6 | `documentAmount` | مبلغ المستند | Display | Automatic | ReadOnly / source-derived |
| 7 | `currency` | العملة | Display | Automatic | ReadOnly / source-derived |
| 8 | `requestReasonNote` | سبب/ملاحظة الطلب | Text display | Optional context | ReadOnly / request-derived on this queue screen |
| 9 | `approvalLevel` | مستوى الاعتماد | Display | Automatic | ReadOnly / policy-derived |
| 10 | `decision` | القرار | Enum | Conditional | Decision command surface only; server-governed choices |
| 11 | `decisionReason` | سبب القرار | Text | Conditional | Decision context; required for Reject/Return as issued |

Exact target-type display/provider ids, linked-document provider contracts and DTO property names remain `TBD-GATED` where not exposed.

### Queue grid
No screen-specific column list is created. Selection, loading, paging, details-host behavior and shared queue presentation belong to ControlApproval Shared Foundation. The server filters the queue by authorized target/company/branch scope.

## UX — TEAM-D04 PASS
- View/list is server-scope filtered; UI visibility is never authorization.
- `documentTypeRef`, `linkedDocumentRef` and request-note context describe the selected existing ApprovalRequest and are not edited/re-targeted from this queue.
- Execute uses the generic issued protected-action endpoint for the selected existing request and does not create/edit the request target or invent action codes.
- Approve uses expected ApprovalRequest version and target-state recheck; self-approval/SoD is server-enforced.
- Reject/Return append decision history and enforce valid approval state; reason rules remain server contract authority.
- Linked target preview/details remain read-only unless the target screen separately authorizes mutation.
- No local approval aging/priority/escalation feature is promoted from benchmarks.
- Shared conflict/loading/error/history/audit/focus behavior only; no offline queue/outbox/replay.

## VISUAL — TEAM-D05 PASS
Use shared ApprovalQueue RTL/DPI/queue/details/decision/history semantics only. Decision states and target context use CoreUI semantic presentation; no local queue styling or column invention.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition after pre-PASS correction: existing request target/context fields are read-only because ACC-057 has no Create/Edit authority. Must confirm 11 fields, four tabs, no screen-specific grid columns, exact View/Execute/Approve/Reject/Return surface, SoD/version/target-state behavior and no benchmark promotion.

## Remaining technical gates
Exact queue shared-column contract bindings, target display/provider bindings, DTO properties, unresolved physical mappings and runtime/release evidence remain separate.
