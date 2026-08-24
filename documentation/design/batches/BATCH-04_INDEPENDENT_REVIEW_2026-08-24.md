# BATCH-04 — Independent Design Review

Date: 2026-08-24
Reviewer: TEAM-D06
Verdict: PASS
Open design findings: 0
Runtime: NOT RUN

Reviewed:
- GEN-008 — العملات
- GEN-014 — اللغات

Checks:
1. Both remain `MasterData / Standard` with shared CoreUI layout.
2. Explicit grids, SingleRow and server paging match issued W3/W2.
3. GEN-008 exposes Code/ArabicName/EnglishName/Symbol/DecimalPlaces/Status/Version only; no IsBaseCurrency or Company.BaseCurrencyId control/effect.
4. GEN-014 exposes Code/CultureCode/Direction/Status/Version only; no invented display-name fields.
5. Status is read-only projection; Disable requires reason + expectedVersion.
6. No Print/Export/Delete/Enable/Activate/Move/Offline/Queue capability.
7. Shared validation/error/concurrency and server default-deny preserved.
8. GEN-014 Direction value does not override CoreUI runtime direction locally.

Acceptance specifications confirm these boundaries. Design PASS does not claim API/CoreUI runtime PASS.
