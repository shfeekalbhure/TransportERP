# ACC-069 — إقفال وردية الصندوق — Canonical Screen Specification

**Profile / Variant:** `ControlApproval / VarianceControl`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-16`

## Authority
Current baseline + current W2 + CoreUI ControlApproval. Root: `CashShift + CashShiftClosing`; context: Cashbox/JournalEntry/ApprovalRequest.

## ANALYSIS / LAYOUT — PASS
Tabs: بيانات الوردية | حركات الوردية | الجرد والفروقات | الاعتماد والإقفال | سجل العمليات.
Shared ControlApproval/VarianceControl host only. No concrete screen-specific grid is issued.

## FIELD_GRID — PASS
Fields (13): رقم الوردية | الصندوق | أمين الصندوق | وقت الفتح | الرصيد الافتتاحي | إجمالي المقبوضات | إجمالي المصروفات | الرصيد الدفتري | النقد المعدود | الفرق | سبب الفرق | وقت الإغلاق | حالة الوردية.

All opening/receipt/expense/book/counted/variance values are server/read-model authoritative except the explicitly captured counted cash/reason context. No local balance/variance formula is authoritative.

## UX / VISUAL — PASS
Actions exactly `View/Execute/Approve/Reject/Return/Reopen`. Execute/Approve/Reject/Return/Reopen are state/permission/version-bound server commands; SoD applies to approval. No Create/Edit/Delete/Print/Export/attachment mutation/offline final write is invented. Shared CoreUI only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-16B_TRANSACTIONS_CONTROLS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact W1/DTO/property/provider/action-code bindings and runtime/acceptance/release evidence remain separate `TBD-GATED` implementation items.
