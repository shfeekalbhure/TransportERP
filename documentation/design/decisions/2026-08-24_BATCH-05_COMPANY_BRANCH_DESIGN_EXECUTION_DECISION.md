# BATCH-05 — Company and Branch Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY CONTINUATION

## Scope
- GEN-010 — الشركات / Company
- GEN-011 — الفروع / Branch

## Governing authority
- `SRC-058 / OWNER-COMPANY-BRANCH-W1-W2-W3-ISSUANCE-001`.
- Current `MasterData / Standard` CoreUI contracts.

## Design boundaries
- `GEN-010` owns the Company `BaseCurrencyId` reference; this does not create or restore any Currency-side `IsBaseCurrency` control.
- Company has no child Branch tab/grid under the current contract.
- `GEN-011.CompanyId` is required on Create and immutable afterward.
- Branch `CountryId`, `GovernorateId`, `CityId` are optional issued references. Current authority requires existence/scope validation only and does not issue a geographic-consistency rule among them; design must not invent one.
- Status remains response projection; Disable uses reason + expectedVersion.
- No Print/Export/Delete/Enable/Activate/Move/Attachments/Offline/Queue capability.

No application code, DDL, API/DTO/permission contract, or official kurrasa is modified.
