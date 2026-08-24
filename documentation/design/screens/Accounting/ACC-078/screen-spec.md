# ACC-078 — إشعار دائن — Canonical Screen Specification

**English:** Credit Note  
**Profile / Variant:** `Transaction / Note`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-18`

## Authority
Current baseline + current W2. Root: `CreditNote`; context: Supplier/OpenItem/Account/CostCenter/Currency/JournalEntry/Attachment.

## ANALYSIS / LAYOUT — PASS
Tabs exactly:
1. البيانات الرئيسية
2. تفاصيل الإشعار
3. المستندات والتخصيص
4. الاعتماد والترحيل
5. سجل العمليات

Shared Transaction/Note host only.

## FIELD_GRID — PASS
Governing fields (15):
1. رقم الإشعار — server-generated/display
2. تاريخ الإشعار — Date / editable in eligible draft
3. المورد — Lookup/Reference
4. المستند الأصلي — Lookup/Reference / conditional
5. سبب الإشعار — Text / required business context
6. العملة — Lookup/Reference
7. المبلغ قبل الضريبة — Decimal/Money
8. الضريبة — Decimal/Money
9. الإجمالي — server/domain-authoritative monetary result
10. حساب الدائن — Lookup/Reference
11. الحساب المقابل — Lookup/Reference
12. مركز التكلفة/الأبعاد — Lookup/Reference / conditional
13. التخصيص على مستندات — allocation context; authoritative application server-side
14. الرصيد قبل/بعد — server-derived/read-only context
15. الحالة — server workflow state/read-only

Allocation grid: exact 7 columns:
`# | المستند المرتبط | تاريخ المستند | الرصيد المفتوح | المبلغ المخصص | المتبقي | العملة`.

Tax/base/total, open balance, allocation and balance-before/after are server/domain authoritative. No client accounting/tax/allocation formula is authoritative.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Cancel/Post/Reverse`.
- Create/Edit only in eligible draft/current-version context.
- Post is server-authoritative for period, numbering, approval, accounts, tax/amount, open-item/allocation and idempotency checks.
- Reverse preserves immutable posted original and creates/links server-authoritative reversal.
- Attachments tab creates no mutation command without W2 binding.
- No Print/Export/direct approval/offline final write is invented.
- Shared CoreUI only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-18B_TRANSACTIONS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact W1/DTO/property/lookup/provider/attachment/sort bindings and runtime/acceptance/release evidence remain separate `TBD-GATED` implementation items.
