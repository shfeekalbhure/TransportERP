# ACC-054 — إغلاق وفتح الفترات المحاسبية — Canonical Screen Specification

**English:** Open / Close Accounting Periods  
**Module:** Accounting  
**Profile / Variant:** `ControlApproval / PeriodAction`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-14`

## Authority
- Current57 closed execution register + current baseline + V1.3 field/tab crosswalk.
- W2: `ACC054.View/Execute/Approve/Reject/Return/Reopen`; seven endpoints with Approval Contract, SoD, expected versions and target-state recheck.
- W1/root: `ApprovalRequest + FiscalPeriod`; read model = PeriodAction decision model.
- Current screen contract explicitly states no concrete screen grid.

## ANALYSIS — TEAM-D01 PASS
Purpose: request and decide protected period actions without directly editing FiscalPeriod lifecycle state.

Tabs:
1. البيانات الرئيسية
2. الحالة/القرار
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

Executable surface exactly: View, Execute, Approve, Reject, Return, Reopen. No direct Create/Edit, Close/Open button invented outside the generic protected-action contract, Print/Export, attachment mutation or offline final write.

## LAYOUT — TEAM-D02 PASS
Shared ControlApproval foundation:
`Toolbar → Decision/MainData(Content) → Tabs/Workspace(Fill) → Approval/Decision History → Audit`.

There is **no concrete screen-specific grid** for ACC-054. No local grid is created to fill space.

## FIELD_GRID — TEAM-D03 PASS
| # | UI design key | Arabic label | Type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `fiscalYearRef` | السنة المالية | Lookup | Required | Editable only while initiating eligible protected action |
| 2 | `periodRef` | الفترة | Lookup | Required | Editable only while initiating eligible protected action |
| 3 | `currentState` | الحالة الحالية | State display | Automatic | ReadOnly |
| 4 | `requestedAction` | الإجراء المطلوب | Enum/Combo | Required | Eligible action request only; values server-contract governed |
| 5 | `reason` | السبب | Multiline Text | Required | Action-request context |
| 6 | `effectiveFrom` | تاريخ السريان | DateTime | Required | Action-request context |
| 7 | `requestedBy` | طلب بواسطة | Display | Automatic | ReadOnly |
| 8 | `approvedBy` | اعتماد بواسطة | Display | Automatic | ReadOnly |
| 9 | `unpostedCheckResult` | نتيجة فحص القيود غير المرحلة | Display | Automatic | ReadOnly / server-authoritative |

Exact lookup providers/action-code identifiers are not invented. `unpostedCheckResult` is a server result and may block close; the client does not duplicate that check.

## UX — TEAM-D04 PASS
- Execute posts a protected action through W2 with target expectedVersion; action/state validity remains server-authoritative.
- Approve uses ApprovalRequest expectedVersion + target state recheck and enforces SoD/self-approval prohibition server-side.
- Reject/Return use the shared decision contract and append decision history.
- Reopen is a separate issued command requiring current target version and reason; no silent state rewrite.
- `ACC-041` remains the FiscalPeriod lifecycle data screen; ACC-054 is the multi-request/control workbench and does not replace or merge it.
- Attachments tab creates no upload/delete command without W2 authority.
- Shared conflict/loading/error/audit/focus behavior only; no offline queue.

## VISUAL — TEAM-D05 PASS
Use shared ControlApproval RTL/DPI/decision-state/history/action presenters only. Current state, approval state and check result are semantic server values; no local state engine or custom styling.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition. Must confirm 9 fields, five tabs, no concrete grid, exact six capability permissions, SoD/version behavior and separation from ACC-041.

## Remaining technical gates
Exact provider/action-code/DTO bindings, unresolved physical field mapping, runtime acceptance and release evidence remain separate.
