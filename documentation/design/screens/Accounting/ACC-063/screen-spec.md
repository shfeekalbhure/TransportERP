# ACC-063 — تحويل نقدي بين الصناديق — Canonical Screen Specification

**Profile / Variant:** `Transaction / Transfer`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-16`

## Authority
Current baseline + current W2. Root: `CashTransfer`; context: Cashbox/Currency/JournalEntry.

## ANALYSIS / LAYOUT — PASS
Tabs: البيانات الرئيسية | التفاصيل والحركات | المرفقات والربط بالمستندات | الاعتمادات | سجل العمليات.
Shared Transaction host only.

## FIELD_GRID — PASS
Fields (11): رقم السند | التاريخ | الطرف | الصندوق/البنك المصدر | الصندوق/البنك الوجهة | العملة | المبلغ | سعر الصرف | الحساب المقابل | البيان | الحالة.

Grid exact 8 columns: `# | الطرف/الجهة | الحساب المقابل | البيان | العملة | المبلغ | سعر الصرف | المبلغ المحاسبي`.
`AutoGenerateColumns=false`; draft-edit only where state/permission allows; accounting amount read-only/server-derived.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Cancel/Post/Reverse`. Source/destination eligibility, balance, currency/rate, posting and reversal remain server-authoritative. No Print/Export/direct approval/attachment mutation/offline final write. Shared CoreUI only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-16B_TRANSACTIONS_CONTROLS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact W1/DTO/property/lookup/sort bindings and runtime/acceptance/release evidence remain separate `TBD-GATED` implementation items.
