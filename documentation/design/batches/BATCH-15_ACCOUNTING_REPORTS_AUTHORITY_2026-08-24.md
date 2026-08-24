# BATCH-15 — Accounting Reports — Final Design Closure

**Screens:** `ACC-059`, `ACC-060`, `ACC-061`, `ACC-062`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`  
**Independent review:** `TEAM-D06 PASS / 0 open design findings`

## Governing contract
Current baseline V1.1 + V1.3 reconciliation + current W2 ReportInquiry contracts + CoreUI ReportInquiry foundation.

All four screens use the same governing 9 criteria:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Executable surface for every screen is exactly:
`View | DrillDown | Export | Print`.

No New/Create/Edit/Delete/Post/Reverse/Approval/offline-write action is authorized.

## Screens
- `ACC-059 — ميزان مراجعة حسب العملة` = `ReportInquiry / Report`; 8 result columns.
- `ACC-060 — كشف حساب مركز التكلفة` = `ReportInquiry / Statement`; 9 result columns.
- `ACC-061 — تقرير القيود غير المرحلة والمسودات والقيود الملغاة` = `ReportInquiry / Inquiry`; 9 result columns.
- `ACC-062 — أرصدة الحسابات حسب العملة` = `ReportInquiry / Report`; 7 result columns.

All financial balances, running balances, valuation rates, accounting equivalents, valuation differences, totals and status classifications are server/read-model authoritative. No client financial recomputation is authorized.

Layout is shared CoreUI only: `Toolbar → Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.

Technical DTO property names, server sort keys and provider ids remain implementation-level where not explicitly issued.

## Completed stages
`TEAM-D01 ANALYSIS = PASS`  
`TEAM-D02 LAYOUT = PASS`  
`TEAM-D03 FIELD_GRID = PASS`  
`TEAM-D04 UX = PASS`  
`TEAM-D05 VISUAL = PASS`  
`TEAM-D06 INDEPENDENT_REVIEW = PASS`

Independent review evidence: `documentation/design/batches/BATCH-15_INDEPENDENT_REVIEW_2026-08-24.md`.

No application code, official Kurrasa, W1/DDL, API, DTO or permission contract was modified by this closure.
