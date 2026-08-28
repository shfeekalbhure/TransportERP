# W2 Control Tower Revalidation Decision

- Decision time: `2026-08-28T16:11:03Z` / `2026-08-28T19:11:03+03:00`
- Governing baseline: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Accepted W1 checkpoint: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Revalidated W2 execution baseline: `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`
- Exact tree: `452b37f1e2c68d9f3dae6e18f1cf1b67645105af`
- PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f — UNMERGED EVIDENCE ONLY`
- Decision: `CONTINUE — W2 VERIFIED CANDIDATE ADOPTED FOR BOUNDED EXECUTION`
- Central superseding decision: `CONTROL_TOWER/00_GOVERNANCE/DECISIONS/MISSION_03_W2_BOUNDED_ADOPTION_DECISION_2026-08-28.md`; the earlier retained-hold decision remains preserved as historical governance evidence.

This decision independently revalidates the preserved W2 candidate package by package. It does not approve a merge to master, a database/data change, Production access, complete W2 exit, MISSION-03 seal, or MISSION-04 start.

## Independent evidence basis

1. `069a311...` is an ancestor of `9c5b7a1...`. The exact linear commits are `a157c34...`, `04a875a...`, `d1c0a25...`, `d740740...`, and `9c5b7a1...`.
2. The exact diff contains 15 paths: 14 source/test paths and one workflow path. It adds 710 and removes 169 lines, and `git diff --check` passes.
3. No Entity file, `TransportErpDbContext`, EF model configuration, Migration, model snapshot, schema, seed, data-repair script, or Production configuration appears in the diff.
4. The source review confirms Company is the tenant root; each Branch has one required `CompanyId`; current User assignment is one required Company plus one optional Branch; request scope is reconciled against active stored User/Company/Branch and stored RBAC before Product actions.
5. Token tenant and permission values remain selectors/narrowing hints. They do not create stored membership or a persistent permission grant. Invalid or inconsistent selectors fail closed.
6. Existing Sync lifecycle mutations enforce Company/Branch plus the same User and Device owner. Registry, PoP, nonce, replay, session-device persistence, and emergency override are not claimed as implemented.
7. GitHub run `33185419917` reports exact head/tree `9c5b7a1...` / `452b37f1...`; core job `98897056951` and Desktop job `98897057221` succeeded. The job log records PostgreSQL 18.6, no pending model changes, all ten existing migrations applied, `128/128` tests, and API HTTP `401`.
8. Retained artifacts are Linux `9691527827` (`sha256:d24109795a2c4f9aff1d82465d7178f2f4eba410b8bd68f86edc504d1ae8357d`) and Desktop `9691490016` (`sha256:4010eeee6c1e4eb504b27e9b14a5af94851528d6ee19c7c582c9f6806f243c1b`).
9. Failed run `33184771338` remains recorded at `d1c0a257...`: core compilation failed with `CS0246` for `OperationContext`; Desktop succeeded; migrations/tests/API did not run. `d740740...` corrected only the import, and later exact-head evidence passed.

## Dependency decisions

### DEP-005

`DEP-005 = CONTROL TOWER REVALIDATED`

- Current Company/Branch hierarchy and singular User assignment are source- and migration-backed.
- A Branch is valid for a request only when active and owned by the selected active Company.
- The server reconciles the authenticated user selector with persistent User/Company/Branch and persistent RBAC at request time.
- Client claims cannot manufacture Company/Branch membership or widen permission.
- Multi-company/multi-branch membership, tenant-consistent physical keys/FKs/checks/indexes/RLS-equivalent defense, live inconsistent rows, applied Production history, and live DB roles remain only in `DBP-002`; they do not block the adopted code-only controls.

### DEP-006

`DEP-006 = CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION`

- Authentication, request-time authorization, persistent RBAC, and issuer/session persistence are separate concerns.
- The adopted resolver depends on a valid authenticated principal but does not choose an issuer, create sessions, rotate refresh tokens, or add session persistence.
- Persistent permission grants and explicit deny overrides are evaluated from the existing database model on each request; claims can narrow only.
- `AUTH-001 = OWNER DECISION REQUIRED — BOUNDED ITEM` for the Production token/session authority mode. It blocks only `W2-B2B` and any issuer-specific/local-session persistence path.

### DEP-007

`DEP-007 = CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION`

- Existing Sync lifecycle operations are owner-bound to the stored operation's User, Device, Company, and Branch and fail closed for non-owner mutation.
- Registry enrollment/assignment, device key/PoP, nonce/replay persistence, session-device binding, revoke propagation, and emergency override are not represented as complete.
- Those persistence/runtime surfaces remain behind `DBP-003/006`, external client-key/retention evidence, and the bounded override decision. They do not block the existing-operation owner checks.

## Package dispositions

| Package | Exact SHA before / after | Independent finding | Decision |
|---|---|---|---|
| `W2-A1` | `069a311...` → `a157c34...` | Sync validates active stored Company/Branch/User scope; mismatched membership fails closed; no DB/model delta | `ADOPT — REBOUND TO SEALED PLAN` |
| `W2-A2` | `04a875a...` → implementation `d740740...`; final test head `9c5b7a1...` | All three Product API modules use the shared stored-scope resolver; cross-company and wrong-branch negatives pass | `ADOPT — REBOUND TO SEALED PLAN` |
| `W2-B1` | `069a311...` → `a157c34...` | Sync requires persistent RBAC; claim-only and explicit persistent deny paths are rejected | `ADOPT — REBOUND TO SEALED PLAN` |
| `W2-B2A` | `04a875a...` → implementation `d740740...`; final test head `9c5b7a1...` | Product API requires both the narrowing token hint and an applicable persistent grant; no issuer/session persistence dependency | `ADOPT — REBOUND TO SEALED PLAN` |
| `W2-C1` | `069a311...` → `a157c34...` | transition, retry, conflict create/resolve, replacement, idempotent replay, and pending-retry ownership are constrained to same User/Device/tenant | `ADOPT — REBOUND TO SEALED PLAN` |
| `W2-F1` | `069a311...` → `9c5b7a1...` | Focused code-only negatives and full current regression pass at exact head; this is only the A1/A2/B1/B2A/C1 subset, not full T-200/T-210/T-220 exit | `ADOPT — REBOUND TO SEALED PLAN` |

No PR #69 code or CI state is adopted by this decision. The candidate was reviewed and tested on the execution lineage itself.

## Preservation and rollback

- Authoritative master, PR #69, W1, migration lineage, existing data, audit history, IDs, Product contracts, and client projects remain preserved.
- No merge, rebase, cherry-pick, force-push, history rewrite, data repair, migration, or Production action occurred.
- Rollback is an ordered normal revert only: `9c5b7a1...`, `d740740...`, `d1c0a257...`, optional workflow commit `04a875a...`, then `a157c34...`. `d1c0a257...` is not a deployable rollback target because its core build failed.
- Rollback must stop if it would reopen a deployed security exposure; no rollback is currently requested or authorized.

## Remaining bounded blockers

| Package | State | Exact reason |
|---|---|---|
| `W2-B2B` | `OWNER DECISION REQUIRED — BOUNDED ITEM` | `AUTH-001`; Production issuer/session authority mode and issuer-specific lifecycle not selected |
| `W2-C2` | `BLOCKED` | registry/assignment/PoP/revoke/replay persistence requires DBP-003/006 and external client-key/retention evidence |
| `W2-D` | `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` | DBP-002 live baseline, impact, migration, preservation, recovery, and direct DB negatives absent |
| `W2-E` | `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` | DBP-003 persistence design/authority and live baseline absent |
| `W2-F2` | `BLOCKED` | stale/revoked session, refresh/logout, revoked device, PoP/replay/nonce, override audit, offline-after-revoke, direct DB, and client security matrix depend on the blocked packages |

## Control Tower direction

The W2-wide hold is lifted only for the six adopted packages. `9c5b7a12e59d2c42e682717b8e90c491f8699b96` is the new MISSION-03 execution baseline. MISSION-03 may continue only into independently satisfied next packages under the sealed dependency order. DBP-002, DBP-003, and DBP-006 remain behind DB-GOV-001. MISSION-03 remains open and MISSION-04 remains waiting.
