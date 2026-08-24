# BATCH-06 — Exchange Rate Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY CONTINUATION

Scope: `GEN-009 — أسعار الصرف / Exchange Rates`.
Authority: `SRC-059 / OWNER-EXCHANGE-RATE-W1-W2-W3-ISSUANCE-001` plus current MasterData/CoreUI contracts.

Boundaries:
- `Profile=MasterData`, `Variant=Standard`.
- CompanyId, FromCurrencyId, ToCurrencyId, RateType and EffectiveFrom are Create-time identity/schedule fields and cannot be changed by Update.
- Update may change Rate, MinimumRate, MaximumRate and EffectiveTo only, with expectedVersion.
- FromCurrencyId must differ from ToCurrencyId.
- Rate and optional bounds are positive; MinimumRate <= Rate <= MaximumRate when bounds exist.
- Same Company/currency pair/RateType intervals may not overlap; adjacent non-overlapping intervals are allowed. Server/domain remains authoritative for interval validation.
- ToCurrencyId is not forced to Company.BaseCurrencyId.
- No Branch/BaseCurrency/IsActive client control, Print/Export/Delete/Enable/Activate/Move/Offline/Queue or independent rate-lookup API is created.

No application code, DDL, API/DTO/permission contract, or official kurrasa is modified.
