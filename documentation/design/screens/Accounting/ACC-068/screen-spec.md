# ACC-068 — كشف حركة البنك — Canonical Screen Specification

**English:** Bank Movement Statement  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-16`

## Authority
Current baseline V1.1 + V1.3 + current W2 + CoreUI ReportInquiry. Read model: `BankMovement` projection.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only statement of authorized bank movements and reconciliation context.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Server-paged read-only result columns exactly:
1. التاريخ
2. رقم الحركة
3. نوع الحركة
4. المرجع البنكي
5. البيان
6. إيداع
7. سحب
8. الرصيد
9. العملة
10. حالة المطابقة

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. Context, sorting, scope, paging, export and print are server-authoritative. Deposit/withdrawal/running-balance and reconciliation-state values are read-model facts; no client recomputation. No mutation/offline write. Shared CoreUI presentation only.

## TEAM-D06
Pending independent review.
