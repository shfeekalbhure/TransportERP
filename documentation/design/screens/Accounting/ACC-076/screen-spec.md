# ACC-076 — تخصيص الدفعات وتسوية الأرصدة — Canonical Screen Specification

**English:** Payment Allocation & Settlement  
**Profile / Variant:** `Transaction / Allocation`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-18`

## Authority
Current V1.26 baseline + current W2 + `ODR-ACC076-001`. Root: `PaymentAllocation`; context: OpenItem/ReceiptVoucher/PaymentVoucher/Customer/Supplier.

## ANALYSIS / LAYOUT — PASS
Purpose: atomically allocate a payment/source balance to authorized open items and explicitly reverse/unallocate an applied allocation.

Tabs exactly:
1. بيانات التسوية
2. المستندات المفتوحة
3. التخصيص
4. الفروقات
5. سجل العمليات

Shared Transaction/Allocation host only.

## FIELD_GRID — PASS
Governing fields (11):
1. رقم التسوية — server-generated/display
2. نوع الطرف — Enum/Reference context
3. الطرف — Lookup/Reference; provider TBD-GATED
4. العملة — Lookup/Reference
5. تاريخ التسوية — Date
6. المستند المدفوع — Lookup/Reference; provider TBD-GATED
7. الدفعة/الرصيد المصدر — Lookup/Reference; provider TBD-GATED
8. المبلغ المخصص — Decimal/Money input in Allocate context
9. فرق التسوية — server-derived/read-only
10. حساب فرق التسوية — Lookup/Reference when required; provider TBD-GATED
11. الحالة — server workflow state/read-only

OpenItemsGrid: `AutoGenerateColumns=false`, server-paged/authorized, exact 7 columns:
`# | المستند المفتوح | نوع المستند | تاريخ الاستحقاق | الرصيد المفتوح | المبلغ المخصص | المتبقي`.

Open balance, allocation validity, remaining balance and difference are server-authoritative. No client allocation/balance formula is authoritative.

## UX / VISUAL — PASS
W2 actions exactly `View | Allocate | Unallocate`.
- `Allocate` is the issued atomic allocation POST; there is no generic Create/Edit/SaveDraft action.
- `Unallocate` uses the issued reverse endpoint and preserves allocation history; no delete/silent balance rewrite.
- Concurrency, open-item status, currency compatibility, available balance and idempotency are server-rechecked.
- No Post/Cancel/Print/Export/Approval/attachment mutation/offline final write is invented.
- Shared CoreUI selection/loading/error/conflict/audit behavior only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-18B_TRANSACTIONS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact W1/DTO/property/lookup/provider/sort bindings and runtime/acceptance/release evidence remain separate `TBD-GATED` implementation items.
