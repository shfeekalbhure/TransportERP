# ACC-067 — كشف حركة الصندوق — Canonical Screen Specification

**English:** Cash Box Movement Statement  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-16`

## Authority
Current baseline V1.1 + V1.3 + current W2 + CoreUI ReportInquiry. Read model: `CashboxMovement` projection.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only statement of authorized cashbox movements and running balance context.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Server-paged read-only result columns exactly:
1. التاريخ
2. رقم الحركة
3. نوع الحركة
4. البيان
5. داخل
6. خارج
7. الرصيد
8. العملة
9. أمين الصندوق
10. الوردية

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. Context, sorting, scope, paging, export and print are server-authoritative. Running balance/in/out values are read-model facts; no client recomputation. No mutation/offline write. Shared CoreUI presentation only.

## TEAM-D06
Pending independent review.
