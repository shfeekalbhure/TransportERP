# ACC-074 — أعمار الديون للعملاء — Canonical Screen Specification

**English:** Customer Aging  
**Profile / Variant:** `ReportInquiry / Aging`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-18`

## Authority
Current baseline + current W2 + CoreUI ReportInquiry. Read model: `CustomerAging` projection; context: Customer/OpenItem/PaymentAllocation. Current P0 implementation/runtime HOLD remains separate and is not cleared by design approval.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only customer aging analysis within authorized scope.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact 9 columns:
`العميل | الرصيد الإجمالي | غير مستحق | 0-30 | 31-60 | 61-90 | 91-120 | أكثر من 120 | العملة`.

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. DrillDown preserves parent context; Export/Print preserve exact filters/sort/scope and are server-generated. No mutation/offline write.

Total balance, due classification and all aging buckets are server/read-model authoritative. No client day-bucket, overdue or balance formula is introduced. Shared CoreUI only.

## TEAM-D06
Pending Batch-18 independent review.

## Remaining technical gates
Current P0 implementation/runtime HOLD, exact DTO/property/sort/provider bindings and runtime/acceptance/release evidence remain separate.
