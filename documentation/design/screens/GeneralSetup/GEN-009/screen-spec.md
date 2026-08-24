# GEN-009 — أسعار الصرف — Canonical Screen Specification

**English:** Exchange Rates  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-06`

## Authority
- Owner issuance: `SRC-059 / OWNER-EXCHANGE-RATE-W1-W2-W3-ISSUANCE-001`.
- W2: ExchangeRate List/Get/Create/Update/Disable; `GEN009.View/Create/Edit/Disable`.
- W3: `EXCHANGE_RATE_TYPED_SCREENDEFINITION_2026-08-22.md`.
- Test input: `EXCHANGE_RATE_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain company-scoped effective-dated exchange-rate schedules between two distinct currencies without deciding how accounting documents consume those rates.

Fields:
- `CompanyId` — required Company lookup/context — Create; filter — no Branch scope.
- `FromCurrencyId` — required Currency lookup — Create only.
- `ToCurrencyId` — required Currency lookup — Create only; must differ from FromCurrencyId.
- `RateType` — required enum; current policy=`Standard` — Create only.
- `EffectiveFrom` — required UTC instant — Create only; immutable after create.
- `EffectiveTo` — optional UTC instant — Create/Edit; null=open-ended; when present > EffectiveFrom.
- `Rate` — required Decimal(20,10) — Create/Edit — positive.
- `MinimumRate` — optional Decimal(20,10) — Create/Edit — positive when present.
- `MaximumRate` — optional Decimal(20,10) — Create/Edit — positive when present.
- `Status` — read-only `Active|Stopped` contract projection.
- `Version` — hidden expectedVersion token for Update/Disable.

Capabilities: View/Create/Edit/Disable/server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content`, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit, no Tabs or LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns in order:
1. `FromCurrency` — من عملة — Reference — content.
2. `ToCurrency` — إلى عملة — Reference — content.
3. `RateType` — نوع السعر — Enum — content.
4. `EffectiveFrom` — ساري من — UTC Instant — content datetime.
5. `EffectiveTo` — ساري إلى — UTC Instant? — content datetime.
6. `Rate` — السعر — Decimal — content numeric.
7. `MinimumRate` — الحد الأدنى — Decimal? — content numeric.
8. `MaximumRate` — الحد الأعلى — Decimal? — content numeric.
9. `Status` — الحالة — Enum/read-only — content state.

Filters: `CompanyId`, `FromCurrencyId`, `ToCurrencyId`, `RateType`, `EffectiveAt`, `Status`, `SearchText`; server allow-listed sort only.

## UX — TEAM-D04 PASS
- Create establishes Company/currency pair/RateType/EffectiveFrom identity; these are read-only on existing records.
- Update surface exposes only Rate, MinimumRate, MaximumRate and EffectiveTo plus hidden expectedVersion.
- From and To currencies must differ; rate/bounds validation is presented from server/domain errors.
- minimum/maximum bounds are not recalculated as a client-side business formula; the client only provides issued values and displays validation.
- interval overlap/adjacency decision remains server/domain authoritative; no client-side schedule engine.
- ToCurrency is not forced to Company.BaseCurrencyId and no Branch/BaseCurrency selector is introduced.
- Disable requires nonblank reason + current version, removes prospective selection, and does not imply physical delete or historical rewrite.
- stale version uses shared concurrency Reload/Refresh; shared validation/loading/error/paging only.
- online authoritative writes only; no queue/outbox/retry path.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: RTL/DPI/typography/spacing, lookup/date/decimal/state presenters, grid/pagination/audit. No local colors, fixed metrics or custom effective-date widget architecture.

## Acceptance criteria
1. distinct From/To currencies and Standard RateType boundary preserved.
2. immutable Company/From/To/RateType/EffectiveFrom after creation.
3. Update only Rate/MinimumRate/MaximumRate/EffectiveTo with expectedVersion.
4. nine explicit grid columns, server paging, allow-listed sort.
5. overlapping same-key intervals rejected by server; adjacent non-overlap allowed.
6. ToCurrency is not forced to Company.BaseCurrencyId.
7. no Print/Export/Delete/Enable/Move/Branch/offline or independent rate-lookup capability.
8. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
W2/W3/acceptance cross-check confirmed immutable schedule identity fields, limited Update surface, server-authoritative overlap/bounds validation, BaseCurrency independence, controlled Disable and prohibited capabilities. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
