# ACC-059 — ميزان مراجعة حسب العملة — Canonical Screen Specification

**English:** Trial Balance by Currency  
**Profile / Variant:** `ReportInquiry / Report`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-15`

## Authority
Current baseline V1.1 + V1.3 reconciliation + current W2 + CoreUI ReportInquiry. Read model: TrialBalanceByCurrency projection.

## ANALYSIS / LAYOUT — PASS
Purpose: report authorized balances by currency without mutating accounting data.

Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared layout only: `Toolbar → Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid, `AutoGenerateColumns=false`, server paging, read-only, exact columns:
1. العملة
2. رقم الحساب
3. اسم الحساب
4. افتتاحي
5. مدين
6. دائن
7. ختامي
8. المعادل بالعملة المحاسبية

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`, permission-bound to ACC059 equivalents and server rechecked. DrillDown preserves parent report context. Export/Print preserve exact filters/scope and are server generated. No local Create/Edit/Delete/Post/Reverse/Approval or offline write.

Opening/debit/credit/closing/accounting-equivalent values are server-authoritative. No client currency conversion, balance formula or rounding logic is introduced. Shared RTL/DPI/grid/paging/loading/error/audit only.

## TEAM-D06 — PASS
`PASS — 0 open design findings`. Independent review confirmed 9 criteria, 8 columns, Report variant, exact four capabilities, scope preservation and no client financial recomputation.

## Final disposition
`DESIGN_APPROVED`. Runtime/acceptance/release evidence remains separate.
