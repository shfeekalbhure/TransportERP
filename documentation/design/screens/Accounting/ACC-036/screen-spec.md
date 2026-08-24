# ACC-036 — مجموعات وأنواع الحسابات — Canonical Screen Specification

**English:** Account Groups and Types  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-20`

## Authority
Current57 baseline + current W2 + `OWNER-WAVE1-20260823` + CoreUI MasterData.
Owner Wave-1 requires separate `AccountGroup` and `AccountType` implementation with a discriminated DTO; legacy merged classification persistence is non-governing.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain the governed account-group/account-type reference catalogs used by the chart of accounts without merging their persistence semantics.

Tabs exactly:
1. البيانات الرئيسية
2. الاستخدام والربط
3. التدقيق

Executable surface exactly:
`View | Create | Edit | Disable`.

No Delete/Enable/Move/Print/Export/Post/Approval/offline-final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared MasterData/Standard host only: toolbar/list-detail/main data/audit behavior comes from CoreUI. Current baseline has no concrete screen-specific grid-column inventory, so no local grid columns are invented.

## FIELD_GRID — TEAM-D03 PASS
Governing screen fields (8):
1. `classificationCode` — رمز المجموعة/النوع — Text — required — editable in eligible Create/Edit context.
2. `arabicName` — الاسم العربي — Text — required — editable in eligible context.
3. `financialClassification` — التصنيف المالي — governed Enum/catalog — required — server-validated.
4. `normalBalanceNature` — الطبيعة — governed debit/credit semantic — required — server-validated.
5. `allowsPostingAccounts` — يسمح بحسابات ترحيل — Boolean/capability metadata — editable only under current record-kind rules.
6. `financialStatementVisibility` — يظهر في القوائم المالية — Boolean/policy presentation — server-validated.
7. `displayOrder` — ترتيب العرض — Integer/order presentation — server-validated.
8. `status` — الحالة — read/server state; Disable is the issued state-change command.

### Group / Type discriminator boundary
- The screen represents two separate governed record kinds: `AccountGroup` and `AccountType`.
- The UI must preserve the server/W2 discriminator and must not persist both into one legacy merged classification entity.
- Exact discriminator property name, DTO property names and physical columns remain implementation-owned/TBD-GATED where not issued.
- No local rule infers record kind from financial classification or any other field.

Current baseline marks concrete screen grid columns N/A. Shared MasterData list/search may present foundation columns only where governed centrally; this ScreenDefinition creates no local business grid contract.

## UX — TEAM-D04 PASS
- Create/Edit use the current discriminated W2 contract and authorized company/branch scope.
- Disable is explicit, permission/state-bound, preserves history, and is not physical delete.
- Existing chart-of-accounts records remain server-authoritative consumers of group/type references.
- No client accounting classification formula or account-postability inference is authoritative.
- Shared lookup/loading/error/conflict/audit behavior only.

## VISUAL — TEAM-D05 PASS
Use shared MasterData RTL/DPI/field/status/audit presentation. Group/type distinction must be visible from server-provided discriminator/context without inventing a local persistence model.

## TEAM-D06 — INDEPENDENT REVIEW
Pending. Confirm 8 fields, three tabs, no local grid columns, exact View/Create/Edit/Disable surface, separate AccountGroup/AccountType semantics and no legacy merged persistence promotion.

## Remaining implementation/release gates
Exact physical field mapping, discriminator/DTO property names, migrations and final runtime/release evidence remain separate from design approval.
