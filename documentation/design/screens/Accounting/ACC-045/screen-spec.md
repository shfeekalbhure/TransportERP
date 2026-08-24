# ACC-045 — سند التحويل بين الصناديق والبنوك — Canonical Screen Specification

**English:** Cash/Bank Transfer Voucher  
**Module:** Accounting  
**Profile / Variant:** `Transaction / Transfer`  
**CurrentDesignState:** `FIELD_GRID_HOLD_AUTHORITY`  
**OwnerTeam:** `DESIGN-LEAD / TEAM-D03`  
**Batch:** `BATCH-12`

## Authority
- Current 57-screen baseline + Unified Design/Execution V1.3.
- W1 aggregate: `TransferVoucher`; related context Cashbox/BankAccount/Currency/JournalEntry.
- W2 exact commands: `ACC045.View/Create/Edit/Cancel/Post/Reverse`; List/Get/Create/Update/Cancel/Post/Reverse only.
- Specialist review FBR-042..043 remains blocking for exact field-level mapping of date/description.

## ANALYSIS — TEAM-D01 PASS
Current fields (11): رقم السند | التاريخ | الطرف | الصندوق/البنك المصدر | الصندوق/البنك الوجهة | العملة | المبلغ | سعر الصرف | الحساب المقابل | البيان | الحالة.

Tabs: البيانات الرئيسية | التفاصيل والحركات | المرفقات والربط بالمستندات | الاعتمادات | سجل العمليات.

The Transfer variant represents the source/destination transfer context. No screen-specific Print/Export, attachment mutation, or direct approval-decision action is issued. Posted records are immutable; Post/Cancel/Reverse are server-authoritative.

## LAYOUT — TEAM-D02 PASS
Shared Transaction layout with Transfer semantics: `Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`. No local layout/style duplication.

## FIELD_GRID — TEAM-D03 HOLD_AUTHORITY
Governing line grid: `# | الطرف/الجهة | الحساب المقابل | البيان | العملة | المبلغ | سعر الصرف | المبلغ المحاسبي`.

Exact persisted/read/API/audit/offline mapping for `التاريخ` and `البيان` remains BLOCKING. No mapping or Print/Attachment/Approval command is inferred.

See `documentation/design/batches/BATCH-12_ACCOUNTING_CORE_TRANSACTIONS_HOLD_2026-08-24.md`.
