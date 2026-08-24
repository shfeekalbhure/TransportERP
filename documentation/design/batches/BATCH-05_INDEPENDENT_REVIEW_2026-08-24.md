# BATCH-05 — Independent Design Review

Date: 2026-08-24
Reviewer: TEAM-D06
Verdict: PASS
Open design findings: 0
Runtime: NOT RUN

Reviewed:
- GEN-010 — الشركات
- GEN-011 — الفروع

## Review gates
1. Both screens remain separate `MasterData / Standard` identities under SRC-058.
2. Shared CoreUI layout, explicit grids, SingleRow and server paging are preserved.
3. GEN-010 owns required `BaseCurrencyId` and optional `CountryId`; no Currency-side `IsBaseCurrency` control is created.
4. GEN-010 has no Branch child tab/grid or hidden Branch-management side effect.
5. GEN-011 requires `CompanyId` on Create and does not permit replacement through Update.
6. GEN-011 optional `CountryId/GovernorateId/CityId` are existence/scope-validated references only; no unissued geographic-consistency rule is asserted.
7. Status is a read-only Active/Stopped projection; Disable requires reason + expectedVersion; stale versions use shared conflict handling.
8. No Print/Export/Delete/Enable/Activate/Move/Attachments/Offline/Queue capability.
9. UI authorization is advisory; server/default-deny and Company/Branch scope remain authoritative.

Acceptance specifications confirm all above boundaries. This is design approval only; runtime tests remain unexecuted.
