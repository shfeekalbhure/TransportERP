# BATCH-19 — Deferred Authority Closures — Design Authority

**Screens:** `GEN-013`, `ACC-036`  
**Date:** 2026-08-24  
**State:** `INDEPENDENT_REVIEW`

## Why this batch is now eligible
Earlier design/runtime HOLDs are superseded by the owner-approved WAVE-1 closure of 2026-08-23:
- `GEN-013`: metadata/scope + `LastNumber` semantics over legacy `NextValue`; no-number-reuse preserved; existing W2 routes/permissions retained.
- `ACC-036`: separate `AccountGroup` + `AccountType` implementation with a discriminated DTO; legacy merged classification persistence remains excluded.

The exact-SHA WAVE-1 runtime gates are green, but the external WAVE-1 independent re-review remains a separate release gate and is **not** treated as design approval evidence.

## GEN-013 design authority
Canonical identity: `Settings / NumberingControlled`.

Governing UI fields exactly:
`الرمز | الاسم العربي | الاسم الإنجليزي | الحالة | ملاحظات | النطاق | نوع المستند | بادئة | آخر رقم | إعادة ضبط`.

Functional areas exactly:
`سياسات الترقيم | نطاقات الترقيم | الاستثناءات والاعتماد | سجل التخصيص`.

No concrete screen-specific business grid is issued.

Executable surface exactly:
`View | Edit | Reserve | Commit | Cancel | Override`.

Rules:
- numbering is server-side; no `MAX+1` and no client number generation;
- `Reserve → Commit | Cancel`; cancelled numbers are never reused;
- scope is Company/Branch/FiscalYear/DocumentType configuration, not a free-form label;
- `LastNumber` is protected, not ordinary metadata;
- protected override/reset requires permission, reason, expected version and approval binding where required;
- legacy ArabicName may be `NULL/unknown`; UI must not fabricate historical Arabic business text;
- Code technical derivation must not be presented as guessed business metadata.

## ACC-036 design authority
Canonical identity: `MasterData / Standard`.

Governing fields exactly:
`رمز المجموعة/النوع | الاسم العربي | التصنيف المالي | الطبيعة | يسمح بحسابات ترحيل | يظهر في القوائم المالية | ترتيب العرض | الحالة`.

Functional areas exactly:
`البيانات الرئيسية | الاستخدام والربط | التدقيق`.

No concrete screen-specific business grid is issued by the current screen baseline.

Executable surface exactly:
`View | Create | Edit | Disable`.

Rules:
- `AccountGroup` and `AccountType` remain separate owner-approved entities;
- the current discriminated DTO/context determines which entity the record belongs to;
- no merged `AccountClassification` persistence model may be recreated by the UI;
- financial classification, normal nature, posting allowance, financial-statement visibility and display order are server/domain validated; the UI does not derive accounting behavior from labels.

## Shared boundaries
CoreUI owns toolbar, layout, RTL/DPI, loading/error/validation/audit and shared presenters. No application code, official Kurrasa, W1/DDL, API, DTO, permission or offline-write authority is created by this design batch.

`TEAM-D01 ANALYSIS = PASS`  
`TEAM-D02 LAYOUT = PASS`  
`TEAM-D03 FIELD_GRID = PASS`  
`TEAM-D04 UX = PASS`  
`TEAM-D05 VISUAL = PASS`  
Next: `TEAM-D06 INDEPENDENT_REVIEW`.
