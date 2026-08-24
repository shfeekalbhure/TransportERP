# BATCH-04 — Currency and Language Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY CONTINUATION

Scope:
- GEN-008 — العملات / Currency
- GEN-014 — اللغات / Language

Authority:
- SRC-057 full W1/W2/W3 specification-design authority.
- Current MasterData / Standard CoreUI architecture.

Boundaries:
- GEN-008 does not expose or mutate IsBaseCurrency or Company.BaseCurrencyId.
- GEN-014 exposes only issued Code, CultureCode, Direction, Status and Version; no invented display-name fields.
- No Print/Export/Delete/Enable/Activate/Move/Offline/Queue capability.
- Status is response projection; Disable is controlled with reason + expectedVersion.

No application code, DDL, API/DTO/permission contract, or official kurrasa is modified.
