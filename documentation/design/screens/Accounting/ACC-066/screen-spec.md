# ACC-066 — تسوية البنك — Canonical Screen Specification

**Profile / Variant:** `Transaction / Reconciliation`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**Batch:** `BATCH-16`

## Authority
Current baseline + current W2. Root: `BankReconciliation`; context: BankStatementLine/ReconciliationMatch/ReconciliationMatchItem/ReconciliationAdjustment/JournalEntry.

## ANALYSIS / LAYOUT — PASS
Tabs: بيانات التسوية | كشف البنك والحركات | المطابقة والاستثناءات | قيود التسوية | سجل العمليات.
Shared Transaction/Reconciliation host only.

## FIELD_GRID — PASS
Fields (7): الحساب البنكي | من تاريخ | إلى تاريخ | رصيد كشف البنك | رصيد الدفتر | فرق التسوية | عناصر غير مطابقة.

Current governed detail grid exact 9 display columns: `# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.
No matching/accounting formula is implemented in the client; balances, differences, matching state and adjustment results are server-authoritative.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Cancel/Match/Finalize/Reopen`. Match/Finalize/Reopen are explicit state/permission/version-bound server commands. No Post/Reverse/Print/Export/direct approval/attachment mutation/offline final write is invented. Shared CoreUI conflict/loading/error/audit only.

## TEAM-D06
Pending independent review.
