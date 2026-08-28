# PR #69 Adoption Analysis

## Frozen candidate

- Ref/head: `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f`
- Tree: `bfbcd14049c97be323decf4785aed37ecad7cc91`
- State observed: `OPEN / DRAFT / UNMERGED`
- Delta from authoritative master: `206 files`, `53,011 additions`, `858 deletions`
- Structural delta: `13` projects, `20` migration implementations, broad security/Sync/Offline/Desktop/Driver/test/CI changes
- Classification: `UNMERGED REMEDIATION / FINAL CANDIDATE — UNMERGED EVIDENCE ONLY`

No merge, cherry-pick, rebase or blind copying is allowed. Successful candidate CI is SHA-bound and cannot be transferred to master or to a selectively adopted tree.

## Finding/component crosswalk

| Area / findings | Master | Candidate evidence | Decision | Preconditions |
|---|---|---|---|---|
| `Volume` P0 / `A-ARCH-002` | defect present | `ConcurrencySafeWaybillRepository.cs` is unchanged; defect remains | `REIMPLEMENT` | DBP-001 + T-100 |
| Preservation / `A-PRES-001`, `TB-F-016` | local/unmerged loss risk | PR69 is itself a preserved unmerged asset | `PRESERVE; NO ADOPTION INFERENCE` | PRES-004/005 |
| Identity/session/RBAC / `A-SEC-001`, `TB-F-002` | claim-driven foundation | local sessions, refresh/revoke, DB permission resolver, rate limiting and negative tests added | `VERIFY + SELECTIVE ADOPT` | auth-mode ADR, source/security review, T-200, DBP-003 |
| Tenant membership / `A-SEC-002`, `TB-F-003` | incomplete | `CurrentSecurityContext` and membership checks materially strengthened | `VERIFY + SELECTIVE ADOPT` | tenant cardinality ADR, T-210 |
| Tenant DB defense / `A-DB-003/004`, `TB-F-012` | partial | new hardening migrations/constraints | `VERIFY/REWORK UNDER DB-GOV` | live baseline, migration-by-migration review, DBP-002 |
| Device/PoP | absent/partial | registry, assignments, proof-key lifecycle and replay evidence added | `VERIFY + SELECTIVE ADOPT` | DBP-003, enrollment/recovery policy, T-220 |
| Sync lifecycle owner / `A-OFF-002`, `D-SEC-SYNC-001` | gap present | new API/worker paths bind registered device, but legacy transition/retry/create-conflict methods still call tenant-only check | `PARTIAL — REWORK/VERIFY CALLERS` | caller map, explicit override policy, T-220 |
| End-to-end Offline / `A-OFF-001`, `TB-F-004` | foundation only | typed catalog, Offline project, worker, client stores, Desktop/Driver runtime and extensive tests | `VERIFY ACTION-BY-ACTION` | M02-BLK-009 authority; T-400; DBP-006 |
| Offline action authority | version-bound deny | candidate exposes five supported actions: create/update Waybill, create Party, record Collection, load allocation; accounting actions remain unavailable | `DEFER — AUTHORITY REQUIRED`; reject generic enablement | canonical operation matrix |
| Accounting post / `A-ACCDB-007`, `TB-F-005` | status-only | `VoucherLifecycleService` unchanged | `REIMPLEMENT` | UoW/accounting ADR, DBP-005, T-300 |
| Collection bridge / `A-BIZ-005` | reference link only | finance path modified but still no governing balanced journal closure | `PARTIAL/REIMPLEMENT` | canonical accounting rules and reconciliation |
| Audit / `A-AUD-006`, `TB-F-013` | hash/atomicity partial | audit/persistence changes present | `VERIFY/REWORK`; no automatic historical adoption | DBP-004, legacy-chain and failure tests |
| Desktop / `A-RUNTIME-001` | Library/prototype | WinExe entry point, shell, auth/offline composition and E2E project added | `VERIFY + SELECTIVE ADOPT` | screen registry, packaging/signing, T-500 |
| Mobile / `A-RUNTIME-002` | placeholders | Driver MAUI runtime added; Admin/Customer only limited offline composition | `SCOPED VERIFY`; no complete-mobile claim | approved app scopes and Android evidence |
| Shipping / `A-BIZ-001`, `TB-F-007` | through DEPART | some contracts/persistence and offline load action changed; later catalog actions remain unavailable | `PARTIAL — REIMPLEMENT BY GOVERNED INCREMENT` | canonical lifecycle, accounting and T-600 |
| Ticketing / `A-BIZ-002`, `TB-F-006` | absent | no governing Ticketing closure | `NOT ADDRESSED — REIMPLEMENT` | canonical Ticketing authority |
| Screens/Kurrasa | authority partial | no authority transfer | `REJECT AS AUTHORITY SOURCE` | REM-620 |
| QA/CI / `A-QA-*`, `A-CI-001`, `TB-F-011` | partial | much broader tests/workflows and exact-head green runs | `ADOPT PATTERNS ONLY; RERUN AFTER SELECTION` | T-000/T-700; conditional skips are not PASS |
| Release/recovery | absent/unknown | release-style E2E evidence, but no Production deploy/restore/merge proof | `VERIFY ONLY — NOT CLOSED` | T-720 and external evidence |
| Supply chain | incomplete | broader workflows but no sufficient locked/SBOM/license/provenance closure proved | `REIMPLEMENT/VERIFY` | T-710 |
| Privacy | partial | redaction/retention/security additions | `SELECTIVE VERIFY`; Production/legal state unknown | REM-730/T-730 |
| Architecture/refactor | concentrated | 206-file cross-cutting delta | `NO BIG-BANG ADOPTION`; split into reviewable dependencies | all earlier waves and parity tests |

## Adoption unit rule

The minimum adoption unit is a reviewed behavior/invariant with its contracts, migrations, tests, preservation and recovery evidence—not an arbitrary file or the whole PR. Every selected unit receives a new execution commit based on the then-authoritative line, independent review and exact-head CI. Any unit that cannot meet authority, DB-GOV, compatibility or rollback requirements remains `UNMERGED EVIDENCE ONLY`.
