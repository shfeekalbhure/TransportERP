# ACC-061 — تقرير القيود غير المرحلة والمسودات والقيود الملغاة — Canonical Screen Specification

**English:** Unposted / Draft / Cancelled Journal Report  
**Profile / Variant:** `ReportInquiry / Inquiry`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-15`

## Authority
Current baseline V1.1 + V1.3 reconciliation + current W2 + CoreUI ReportInquiry. Read model: JournalStatusInquiry projection.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only inquiry over journal lifecycle/status records within authorized scope.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact columns:
1. رقم القيد
2. التاريخ
3. الوصف
4. الحالة
5. إجمالي المدين
6. إجمالي الدائن
7. سبب الإلغاء
8. أنشئ بواسطة
9. آخر تعديل

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. ApplyFilters/Refresh are shared UI behavior, not new executable permissions. DrillDown preserves parent context; Export/Print use exact current filters and server data. No mutation/offline write.

Journal totals and lifecycle state are server/read-model authoritative; no client recomputation. Shared CoreUI presentation only.

## TEAM-D06
Pending independent review. Confirm Inquiry variant, 9 criteria, 9 result columns, exact four capabilities and no candidate-only actions promoted.
