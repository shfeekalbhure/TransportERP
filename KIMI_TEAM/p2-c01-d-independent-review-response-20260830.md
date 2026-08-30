# P2-C01-D Independent Review Response

**Review Source:** Owner/Independent reviewer deep comparison on GitHub at exact SHA `0922d54452f04f27ffc6b5f604cbc17fbf1f0840`  
**Response Date:** 2026-08-30  
**Branch:** `kimi/p2-c01-d-remediation-20260830`  
**Responder:** KIMI Team  
**Previous Verdict:** `READY_FOR_REVIEW` (retracted)  
**Revised Verdict:** `CHANGES_REQUIRED`

---

## 1. Findings Accepted

The independent review identified four material blockers that remain open. KIMI accepts all verified findings and retracts the premature `READY_FOR_REVIEW` verdict.

### Blocker B1: C→D Phase Boundary Regression (`P2D-RR-005`)

**Status:** OPEN — verified in code.

**Evidence:**
- Test: `TransportERP.Tests/P2C01CShippingApiContractTests.cs:143`
  - `C_does_not_expose_next_phase_runtime_endpoints`
  - Expects `404 Not Found` for `/api/v1/arrivals/{id}:finalize` and `/api/v1/trips/{id}:close`
- Actual: `403 Forbidden` because P2-C01-D registers these endpoints.
- This is a phase-boundary violation: D runtime is exposed while C contract tests assert it should not exist.

### Blocker B2: `CloseTrip` Does Not Check ShipmentException (`P2D-RR-006`)

**Status:** OPEN — verified in code.

**Evidence:**
- File: `TransportERP.Infrastructure/Persistence/ArrivalExecutionPersistence.cs:455`
  - `ArrivalExecutionRules.EnsureTripClose(trip.Status, departed, accounted, custodyOpen, exceptionBlocked: false);`
- The `exceptionBlocked` parameter is hardcoded to `false`.
- No query is executed to determine whether any `ShipmentException` is open for the trip.
- The only test for `EXCEPTION_BLOCKED` passes `true` manually to the rule, not through the persistence path.

### Blocker B3: Missing D-Specific PostgreSQL Persistence Tests (`P2D-RR-003`)

**Status:** OPEN — verified in code.

**Evidence:**
- `P2C01DArrivalExecutionTests.cs` uses `NoopArrivalStore` for rules/application tests.
- `P2C01DArrivalApiContractTests.cs` uses `RecordingArrivalStore` for HTTP contract tests.
- No file `P2C01DArrivalPostgreSqlIntegrationTests.cs` exists.
- The workflow step `Run P2-C01-D PostgreSQL and HTTP gates` runs `--filter "FullyQualifiedName~P2C01D"`, but neither D test file actually uses PostgreSQL.

### Blocker B4: CI Did Not Reach EF/Migration/PostgreSQL Gates (`P2D-RR-008`)

**Status:** OPEN — verified from GitHub run.

**Evidence:**
- GitHub run stopped at `Run non-database regression` with 1 product failure + 13 environmental failures.
- Steps skipped:
  - `Require P2-C01-D migration to be committed`
  - `Verify EF model matches committed migration`
  - `Apply P1 A B C and D migrations to PostgreSQL 18`
  - `Run P2-C01-D PostgreSQL and HTTP gates`
- Therefore, remote independent evidence for migration application and D PostgreSQL behavior does not yet exist.

---

## 2. Additional Finding: CI Test Category Mismatch

**Status:** OPEN — verified in code.

**Evidence:**
- Workflow step: `Run non-database regression`
  - Filter: `--filter "Category!=P2PostgreSQL"`
- Some PostgreSQL tests use `[Trait("Category", "P2PostgreSQL")]` (correctly excluded).
- Other PostgreSQL tests use `[Trait("Category", "PostgreSQL")]` (not excluded):
  - `AuditEventPersistenceTests.cs`
  - `PostgreSqlPersistenceSmokeTests.cs`
  - `SyncOperationPersistenceTests.cs`
  - `ApiAuthenticationAndAuditTests.cs`
- Result: 13 tests fail in the "non-database" step due to missing `TRANSPORTERP_TEST_CONNSTR`.
- This is a CI filter bug, not a product logic failure, but it prevents the CI from reaching the real D gates.

---

## 3. Findings Previously Closed (Retained)

| Finding | Status | Evidence |
|---|---|---|
| Branch exists on origin | CLOSED ✅ | `git ls-remote origin kimi/p2-c01-d-remediation-20260830` |
| Branch based on current master | CLOSED ✅ | `ahead_by=17`, `behind_by=0`, merge-base `2ec6ccc…` |
| 16 original D commits preserved | CLOSED ✅ | Cherry-picked cleanly |
| D migration committed | CLOSED ✅ | `20260830011249_P2C01DArrivalTransitWarehouse` |
| Workflow `contents: write` removed | CLOSED ✅ | `permissions: contents: read` |
| Auto-push migration removed | CLOSED ✅ | No `git push` in workflow |
| Remediation branch in triggers | CLOSED ✅ | Branch name added to `push` and job conditions |
| Desktop D surface | CLOSED ✅ | `Arrival Desktop RTL` job passed |

---

## 4. Required Fixes Before PR-Ready

### Fix F1: Resolve C→D Phase Boundary Conflict

**Options:**
1. **Defer D endpoints behind a feature flag** so they do not register when C-only contract surface is tested.
2. **Update C contract test** to expect `403 Forbidden` instead of `404 Not Found` for D endpoints, with explicit documentation that this is an intentional phase overlap.
3. **Gate D endpoint registration** on an owner-authorized scope decision that explicitly allows C and D runtime to coexist during this transition.

**Recommendation:** Option 2 is the smallest change, but it weakens the C contract. Option 1 is safer but adds complexity. Option 3 requires owner decision. **Default recommendation: Option 1 with an environment/scope flag.**

### Fix F2: Make `CloseTrip` Check for Open Exceptions

**Required:**
- Add query in `ArrivalExecutionPersistence.CloseTripAsync` to detect any open `ShipmentExceptionEntity` for the trip.
- Pass the result as `exceptionBlocked` to `EnsureTripClose`.
- Add persistence test that creates an open exception and asserts `EXCEPTION_BLOCKED`.

### Fix F3: Add D PostgreSQL Integration Tests

**Required:**
- Create `TransportERP.Tests/P2C01DArrivalPostgreSqlIntegrationTests.cs`.
- Cover:
  - Record arrival → receipt created → movement events persisted.
  - Record unload → quantities updated.
  - Reallocate transit → holding created.
  - Finalize arrival → status transition.
  - Close trip with open exception → `EXCEPTION_BLOCKED`.
  - Idempotency / retry with same `ClientOperationId`.
  - Cross-tenant negative cases.

### Fix F4: Fix CI Test Category Filter

**Required:**
- Change workflow filter from `Category!=P2PostgreSQL` to `Category!=PostgreSQL&Category!=P2PostgreSQL`.
- Or rename all `[Trait("Category", "PostgreSQL")]` to `[Trait("Category", "P2PostgreSQL")]`.

**Recommendation:** Update the filter to exclude both categories; do not modify test files unless necessary.

### Fix F5: Re-run Full CI After Fixes

**Required:**
- Push fixes.
- Ensure exact-head CI reaches and passes:
  - `Require P2-C01-D migration to be committed`
  - `Verify EF model matches committed migration`
  - `Apply ... migrations to PostgreSQL 18`
  - `Run P2-C01-D PostgreSQL and HTTP gates`
  - `Arrival Desktop RTL`

---

## 5. PR #49 Handling

**Status:** OPEN.

**Finding:** PR #49 remains `OPEN / DRAFT / UNMERGED` and explicitly states that D is not closed until migration + PostgreSQL/HTTP/RTL + independent exact-head PASS.

**Required Action:** Update the PR body or close PR #49 with a superseded note, and open a new PR from `kimi/p2-c01-d-remediation-20260830` after all blockers are closed.

---

## 6. Revised Conclusion

The branch `kimi/p2-c01-d-remediation-20260830` is **published and structurally improved**, but it is **not PR-ready**. It requires:

1. Phase-boundary resolution.
2. `CloseTrip` exception checking.
3. D PostgreSQL integration tests.
4. CI filter fix.
5. Full exact-head CI green.
6. PR #49 superseded / reconciled.

**Next Step:** Obtain explicit owner authorization to implement Fixes F1–F4 on the same branch, then re-push and re-run CI.
