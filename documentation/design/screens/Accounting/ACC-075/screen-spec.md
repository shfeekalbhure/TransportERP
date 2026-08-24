# ACC-075 — أعمار الالتزامات للموردين — Canonical Screen Specification

**English:** Vendor Aging  
**Profile / Variant:** `ReportInquiry / Aging`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-18`

## Authority
Current baseline + current W2 + CoreUI ReportInquiry. Read model: `SupplierAging` projection; context: Supplier/OpenItem/PaymentAllocation.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only supplier aging analysis within authorized scope.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact 9 columns:
`المورد | الرصيد الإجمالي | غير مستحق | 0-30 | 31-60 | 61-90 | 91-120 | أكثر من 120 | العملة`.

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. DrillDown preserves report context; Export/Print preserve exact filters/sort/scope and are server generated. No mutation/offline write.

Total liability, due classification and aging buckets are server/read-model authoritative. No client aging/balance formula.

The current runtime `OPEN_ITEM_SOURCE_RECONCILIATION` hold is explicitly separate: this design spec does not claim implementation/runtime/release readiness.

## TEAM-D06
Pending independent review. Confirm Aging variant, 9 criteria, 9 result columns, exact four capabilities and runtime/design separation.
