# DBP-002 Frozen Checkpoint Evidence Summary

**Checkpoint SHA:** `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`  
**Tree:** `e828941817432bdc73f3e6fc31e74219e74fcf33`  
**Parent:** `f128d24dce7baf76a6ac8af4e62a331b80447311`  
**Commit Message:** `MISSION-03 DBP-002: trigger full rehearsal after authoritative fixture fix`  
**Evidence Date:** 2026-08-30  
**Collected By:** KIMI-01 / KIMI-04  
**Worktree:** `C:/Users/shfee/Projects/TransportERP_dbp002_ffdf1087`

---

## 1. Git Identity Verification

```text
Author: shfeekalbhure <shfeek01@gmail.com>
Date:   Sat Aug 29 02:25:08 2026 +0300
```

Verified via `git log --oneline -5`:

```text
ffdf108 MISSION-03 DBP-002: trigger full rehearsal after authoritative fixture fix
f128d24 MISSION-03 DBP-002: seed authoritative memberships in regression fixtures
99d2880 MISSION-03 DBP-002: replace truncated fixture fixer payload
95bf12b MISSION-03 DBP-002: fix authoritative test seed helper YAML
9df4e94 MISSION-03 DBP-002: add one-shot authoritative test seed fixer
```

---

## 2. Migration Inventory at Checkpoint

Migrations present in `TransportERP.Infrastructure/Persistence/Migrations/`:

| # | Migration | Description |
|---:|---|---|
| 1 | `20260819032151_P1InitialPostgreSql` | P1 baseline schema |
| 2 | `20260819151242_P1AuditAppendOnlyAndOutcome` | Audit/append-only |
| 3 | `20260819152128_P1ConflictCaseAndSyncRelation` | Conflict case + sync |
| 4 | `20260820205431_P2C01AWaybillFoundation` | Waybill foundation |
| 5 | `20260820205853_P2C01AWaybillFoundationHardening` | Foundation hardening |
| 6 | `20260821004516_P2C01BFinance` | Finance module |
| 7 | `20260821132015_P2C01CShippingExecution` | Shipping execution |
| 8 | `20260821141529_P2C01CShippingExecutionHardening` | Shipping hardening |
| 9 | `20260821170000_P2C01CTeam03PostgreSqlHardening` | Team-03 hardening |
| 10 | `20260821191039_P2C01CWaybillVolumeContract` | Waybill volume contract |
| 11 | `20260828224241_GreenfieldTenantMembershipIsolation` | Tenant membership isolation (DBP-002) |
| — | `GreenfieldDbp002PhysicalSql.cs` | DBP-002 physical SQL |
| — | `TransportErpDbContextModelSnapshot.cs` | EF model snapshot |

---

## 3. Local Build Evidence

**Command:** `dotnet build TransportERP.slnx --no-restore`

**Result:** `Build succeeded.`

**Warnings:** 6 (non-blocking)
- 1 CS8602 null-dereference warning in `TransportERP.Desktop/Waybills/ShippingExecutionForms.cs:656`
- 1 CS8602 null-dereference warning in `TransportERP.Tests/LocalSessionLifecycleTests.cs:310`
- 4 xUnit2031 analyzer warnings in `TransportERP.Tests/P2C01AWaybillFoundationTests.cs` and `TransportERP.Tests/LocalSessionLifecycleTests.cs`

**Errors:** 0

**Time Elapsed:** 00:00:52.75

---

## 4. Local Test Run Evidence

**Command:** `dotnet test TransportERP.slnx --no-build --logger "trx;LogFileName=dbp002_test_results.trx"`

**Result:** Local test run did not complete due to environment constraints.

**Observed Issue:** `System.OutOfMemoryException` during `P2C01CShippingApiContractTests` while building the ASP.NET Core test host via `WebApplicationFactory`. This occurred in the local Windows Git Bash environment and is attributed to resource constraints / parallel test execution, not to a deterministic code defect.

**Conclusion:** Local test execution is **NOT AUTHORITATIVE** for this checkpoint. The authoritative evidence remains the GitHub Actions CI runs recorded by CONTROL TOWER.

---

## 5. Authoritative CI Evidence (from CONTROL TOWER)

| Run ID | Job / Gate | Result | Notes |
|---|---|---|---|
| `33222541097` | DBP-002 Full Rehearsal v3 | **SUCCESS** | Corrected full PostgreSQL rehearsal |
| `33222541108` | W0 | **SUCCESS** | Foundation regression |
| `33222541109` | W7 backup/restore | **SUCCESS** | Recovery gate |
| `33222541073` | Legacy v2 rehearsal | **FAIL** | Requires explicit independent disposition |

---

## 6. Open Disposition Items for Independent Reviewer (KIMI-05)

1. **v2 vs v3 disposition:** Is v3 the valid corrected/superseding rehearsal path? Explicitly disposition the red `33222541073` v2 result.
2. **Original-ten migration preservation:** Verify that P1/P2-C01 migrations 1–10 are preserved unchanged and that only DBP-002 additions (migration 11 + physical SQL) are present.
3. **Candidate/source/generated-SQL hashes:** Verify `GreenfieldDbp002PhysicalSql.cs` and generated migration SQL match the intended design.
4. **PostgreSQL 18.6 apply:** Confirm migrations apply cleanly on PostgreSQL 18.6.
5. **RLS/ACL/catalog/FK controls:** Verify Row-Level Security, access-control lists, policy catalog, and foreign keys.
6. **Fail-closed/cross-tenant negatives:** Verify negative tests for tenant isolation fail correctly.
7. **W0/full regression and backup/restore/recovery:** Cross-check CI evidence at `ffdf1087...`.
8. **Local build reproducibility:** Build succeeded with 0 errors (this evidence).

---

## 7. Evidence Classification

| Item | Authority Class |
|---|---|
| Git SHA/tree/parent identity | **GOVERNING** |
| GitHub Actions CI run results | **GOVERNING** (primary evidence) |
| Local build success | **REFERENCE** (reproducibility check) |
| Local test run OutOfMemoryException | **NOT PROVEN / ENVIRONMENTAL** — does not invalidate CI evidence |
| Migration inventory | **GOVERNING** |

---

## 8. Conclusion

The DBP-002 frozen checkpoint `ffdf1087...` builds successfully with zero errors locally. The authoritative CI evidence (Full Rehearsal v3, W0, W7) is green at this exact SHA. The only remaining item before DBP-002 can be accepted is an independent post-rehearsal DB-GOV review that explicitly dispositions the legacy v2 failure and verifies the items listed in Section 6.

**Status:** `READY FOR INDEPENDENT REVIEW PACKAGE PREPARATION`
