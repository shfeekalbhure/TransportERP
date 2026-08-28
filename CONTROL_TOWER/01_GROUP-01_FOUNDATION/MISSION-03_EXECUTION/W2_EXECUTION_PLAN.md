# W2 Execution Plan and Package Gates

- Exact start SHA: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Preservation: current predicates, permission codes, IDs, audit, migration lineage and W1 behavior remain intact.
- PR #69: comparison only; no merge, bulk copy or cherry-pick.
- Stop: cross-tenant success, regression, unreviewed DB/schema/data mutation, secret/Production access or loss of preserved evidence.
- Current governance state: `CONTINUE — W2 VERIFIED CANDIDATE ADOPTED FOR BOUNDED EXECUTION`; only A1/A2/B1/B2A/C1/F1 are adopted and W2 is not fully exited.

| Package | REM/findings | Scope | Gate/result | Planned tests | Rollback |
|---|---|---|---|---|---|
| `W2-A1` | REM-210 / A-SEC-002 | Sync server binding of active user to stored Company/Branch | `ADOPT — REBOUND TO SEALED PLAN` | claimed company/branch vs stored user; inactive/mismatched scope | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-A2` | REM-210 / A-DB-003/004 | shared API TenantContext across all Product modules | `ADOPT — REBOUND TO SEALED PLAN; 9c5b7a1 / RUN 33185419917 PASS` | cross-company and foreign/missing stored user/company/branch denial; current behavior parity | ordered normal revert through d1c0a25 lineage |
| `W2-B1` | REM-200 / A-SEC-001 | Sync permission from persistent RBAC, not token boolean alone | `ADOPT — REBOUND TO SEALED PLAN` | claim-only deny, grant allow, explicit deny/revoke | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-B2A` | REM-200 | shared API persistent authorization pipeline | `ADOPT — REBOUND TO SEALED PLAN; 9c5b7a1 / RUN 33185419917 PASS` | claim-only denial, persistent branch grant, wrong-scope denial | ordered normal revert through d1c0a25 lineage |
| `W2-B2B` | REM-200 | issuer-specific login/refresh/revoke/logout/session lifecycle | `OWNER DECISION REQUIRED — AUTH-001; DBP-003 IF LOCAL MODE` | stale/revoked identity, refresh rotation/reuse, logout, client/offline revoke | selected-mode disable/recovery |
| `W2-C1` | REM-220 / D-SEC-SYNC-001 | owner bind all existing Sync lifecycle mutations | `ADOPT — REBOUND TO SEALED PLAN; DEFAULT NO OVERRIDE` | different user/device for transition/retry/conflict/resolve/replacement | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-C2` | REM-220 | device registry/assignment/PoP runtime | `BLOCKED — DBP-003/006 + client/retention evidence` | device mismatch/revoke/replay/nonce/override audit | feature disable plus governed recovery |
| `W2-D` | REM-210 / DBP-002 | tenant keys/FKs/checks/indexes/RLS/equivalent | `BLOCKED — LIVE BASELINE + DB-GOV EXECUTION AUTHORITY` | migration/restore/direct-SQL and A↔B matrix | forward correction or safe-copy restore |
| `W2-E` | REM-200/220 / DBP-003 | memberships/sessions/device persistence | `BLOCKED — AUTH-001, LIVE BASELINE, DB-GOV EXECUTION AUTHORITY` | migrate/login/refresh/revoke/device/replay | disable mode; forward correction/restore |
| `W2-F1` | REM-200/210/220 | code-only negative PostgreSQL tests for A1/A2/B1/B2A/C1 | `ADOPT — REBOUND TO SEALED PLAN; 128/128 AT 9c5b7a1` | focused cases included in complete exact-head regression | revert test commits only with corresponding control rollback |
| `W2-F2` | W2 exit | full API/client/offline/device/DB security matrix | `BLOCKED BY B2B/C2/D/E` | T-200/T-210/T-220 complete matrix | package-specific recovery |

The six adopted packages are ready for later independent MISSION-04 verification but are not final verified. B2B/C2/D/E/F2 remain blocked; MISSION-04 remains waiting for the final MISSION-03 seal.
