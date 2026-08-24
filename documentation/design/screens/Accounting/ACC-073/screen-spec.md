# ACC-073 — كشف حساب المورد — Canonical Screen Specification

**English:** Vendor Statement  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-17`

## Authority
Current baseline + current W2 + CoreUI ReportInquiry. Read model: `SupplierStatement` projection; context: Supplier/OpenItem/JournalEntry/JournalLine/PaymentAllocation.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only supplier statement within authorized company/branch/supplier scope.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact 10 columns:
`التاريخ | رقم المستند | نوع المستند | المرجع | البيان | مدين | دائن | الرصيد | تاريخ الاستحقاق | العملة`.

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. DrillDown preserves report context and rechecks target scope. Export/Print preserve current filters/sort/scope and are server-generated. No mutation/offline write.

Debit/credit/running balance/due-date context is server/read-model authoritative; no client balance or open-item formula. Shared CoreUI only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-17_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact DTO/property/sort/provider bindings and runtime/acceptance/release evidence remain separate implementation items.
