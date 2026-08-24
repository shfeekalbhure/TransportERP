# ACC-060 — كشف حساب مركز التكلفة — Canonical Screen Specification

**English:** Cost Center Statement  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-15`

## Authority
Current baseline V1.1 + V1.3 reconciliation + current W2 + CoreUI ReportInquiry. Read model: CostCenterStatement projection.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only statement of authorized cost-center movements and balance context.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact columns:
1. مركز التكلفة
2. التاريخ
3. رقم القيد
4. الحساب
5. البيان
6. مدين
7. دائن
8. الرصيد
9. الفرع

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. DrillDown preserves report context and rechecks target permission/scope. Export/Print are server generated from the same filters/context. No mutation or offline write.

Running balance/debit/credit are read-model/server authoritative; no client accounting formula. CoreUI owns RTL/DPI/grid/paging/loading/errors/audit.

## TEAM-D06 — PASS
`PASS — 0 open design findings`. Independent review confirmed Statement variant, 9 criteria, 9 result columns, exact four capabilities and server-authoritative balance.

## Final disposition
`DESIGN_APPROVED`. Runtime/acceptance/release evidence remains separate.
