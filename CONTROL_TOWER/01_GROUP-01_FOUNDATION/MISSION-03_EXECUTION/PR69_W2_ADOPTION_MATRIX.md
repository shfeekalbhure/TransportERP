# PR #69 W2 Adoption Matrix

PR #69 head `601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED EVIDENCE ONLY`.

| Component/control | Finding | Classification | Reason / required action |
|---|---|---|---|
| `CurrentSecurityContext` | A-SEC-001/002 | `REIMPLEMENT + VERIFY` | strong active lookups, but company-null user plus `auth.scope.select` is not explicit membership |
| `TenantScopeResolver` | A-SEC-002 | `REIMPLEMENT` | useful branch/company check; target forbids null-as-wildcard |
| `EffectivePermissionResolver` | A-SEC-001/A-DB-004 | `PR69 CANDIDATE — SELECTIVE + REVERIFIED` | DB request-time authority is correct direction; current keys/scope data need fail-closed tests |
| authorization policy/provider/result handler | A-SEC-001 | `PR69 CANDIDATE — SELECTIVE + REVERIFIED` | centralizes 401/403 behavior; must run on execution SHA and preserve contracts |
| local `IdentitySessionService` | TB-F-002 | `OWNER-BOUNDED CANDIDATE` | depends on AUTH-001 and DBP-003; cannot define Production mode itself |
| refresh rotation/reuse/family revoke | A-SEC-001 | `VERIFY CANDIDATE` | sound intent; needs exact concurrency/audit/recovery review after mode choice |
| external-authority mode | A-SEC-001 | `VERIFY EXTERNAL` | code option does not prove IdP session/revocation/device semantics |
| `RegisteredDeviceService` and assignments | A-SEC-002/REM-220 | `VERIFY + SELECTIVE ADOPT AFTER DB-GOV` | candidate lifecycle; policy, migration and client enrollment gates remain |
| proof-key lifecycle and `SyncPopProofValidator` | REM-220 | `VERIFY + SELECTIVE ADOPT AFTER DB-GOV` | strong PoP candidate; deployment topology, nonce/replay persistence and client key evidence required |
| identity/device migrations | DBP-003 | `VERIFY/REWORK — BLOCKED` | unapproved schema/data/seed/trigger changes; must be decomposed and rehearsed |
| Stage5 tenant hardening migration | DBP-002/006 | `VERIFY/REWORK — BLOCKED` | combines tenant, Sync, retention/legal-hold concerns; not adopted as one bulk migration |
| modified Sync intake | REM-220 | `VERIFY ACTION-BY-ACTION` | strong registry/PoP path but depends on offline authority and DBP-006 |
| legacy Sync lifecycle methods | D-SEC-SYNC-001 | `REJECT AS SOLUTION / REIMPLEMENT` | PR analysis confirms tenant-only owner gap remains in affected methods |
| PR tests/CI status | all | `EVIDENCE ONLY` | tests are candidates to port selectively; PR CI never transfers to execution SHA |

No PR file has been copied, merged or cherry-picked by this decision package.
