# ACC-064 — إيداع نقدي في البنك — Canonical Screen Specification

**Profile / Variant:** `Transaction / Transfer`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**Batch:** `BATCH-16`

## Authority
Current V1.26 baseline + current W2 + W3-CORR-ACC064-065-001. Root: `BankDeposit`; context: Cashbox/BankAccount/Currency/JournalEntry/Attachment.

## ANALYSIS / LAYOUT — PASS
Tabs: البيانات الرئيسية | التفاصيل والحركات | المرفقات والربط بالمستندات | الاعتمادات | سجل العمليات. Shared Transaction host only.

## FIELD_GRID — PASS
Fields (11): رقم السند | التاريخ | الطرف | الصندوق/البنك المصدر | الصندوق/البنك الوجهة | العملة | المبلغ | سعر الصرف | الحساب المقابل | البيان | الحالة.

Grid exact 8 columns: `# | الطرف/الجهة | الحساب المقابل | البيان | العملة | المبلغ | سعر الصرف | المبلغ المحاسبي`.
Draft edit only; accounting amount read-only/server-derived. Attachment tab does not create mutation commands without W2 binding.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Cancel/Post/Reverse`. Cashbox/bank eligibility, amount/rate, posting and reversal are server-authoritative. No Print/Export/direct approval/offline final write. Shared CoreUI only.

## TEAM-D06
Pending independent review.
