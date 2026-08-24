# ACC-062 — أرصدة الحسابات حسب العملة — Canonical Screen Specification

**English:** Account Balances by Currency  
**Profile / Variant:** `ReportInquiry / Report`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-15`

## Authority
Current baseline V1.1 + V1.3 reconciliation + current W2 + CoreUI ReportInquiry. Read model: AccountBalanceByCurrency projection.

## ANALYSIS / LAYOUT — PASS
Purpose: report authorized account balances by source currency and accounting equivalent.
Regions: `معايير التقرير | النتائج | الملخص والتفاصيل`.
Shared ReportInquiry layout only.

## FIELD_GRID — PASS
Nine criteria exactly:
`الشركة | الفرع | من تاريخ | إلى تاريخ | السنة/الفترة | العملة | الحساب/النطاق | مركز التكلفة | الحالة/نوع القيد`.

Results grid is server-paged/read-only with exact columns:
1. رقم الحساب
2. اسم الحساب
3. العملة
4. الرصيد الأصلي
5. سعر التقييم
6. المعادل المحاسبي
7. فرق التقييم

## UX / VISUAL — PASS
Capabilities exactly `View | DrillDown | Export | Print`. DrillDown/export/print preserve current filters, sort and authorized scope. No mutation/offline write.

Original balance, valuation rate, accounting equivalent and valuation difference are server/read-model authoritative. No client valuation, FX formula or early rounding is introduced. Shared CoreUI presentation only.

## TEAM-D06
Pending independent review. Confirm Report variant, 9 criteria, 7 columns, exact four capabilities and server-authoritative valuation semantics.
