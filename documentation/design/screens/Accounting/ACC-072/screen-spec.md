# ACC-072 — كشف حساب العميل — Canonical Screen Specification

**English:** Customer Statement  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-17`

## Authority
Current baseline + current W2 + CoreUI ReportInquiry. Read model: `CustomerStatement` projection; context: Customer/OpenItem/JournalEntry/JournalLine/PaymentAllocation.

## ANALYSIS / LAYOUT — PASS
Purpose: read-only customer statement within authorized company/branch/customer scope.
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

## TEAM-D06
Pending independent review. Confirm Statement variant, 9 criteria, 10 columns, exact four capabilities and server-authoritative customer balance.
