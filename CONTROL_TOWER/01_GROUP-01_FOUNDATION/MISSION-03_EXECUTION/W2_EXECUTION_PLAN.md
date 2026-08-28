# W2 Execution Plan and Package Gates

- Exact start SHA: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Adopted W2 predecessor: `9c5b7a12e59d2c42e682717b8e90c491f8699b96`; current bounded code-only baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`.
- Preservation: current predicates, permission codes, IDs, audit, migration lineage and W1 behavior remain intact.
- PR #69: comparison only; no merge, bulk copy or cherry-pick.
- Stop: cross-tenant success, regression, unreviewed DB/schema/data mutation, secret/Production access or loss of preserved evidence.
- Current governance state: `CONTINUE — CODE-ONLY BASELINE ADOPTED; DBP-003 HOLD AT REHEARSAL ENTRY`.
- Owner decision: `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`.

| Package | REM/findings | Scope | Gate/result | Planned tests | Rollback |
|---|---|---|---|---|---|
| `W2-A1` | REM-210 / A-SEC-002 | Sync server binding of active user to stored Company/Branch | `ADOPT — REBOUND TO SEALED PLAN` | claimed company/branch vs stored user; inactive/mismatched scope | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-A2` | REM-210 / A-DB-003/004 | shared API TenantContext across all Product modules | `ADOPT — REBOUND TO SEALED PLAN; 9c5b7a1 / RUN 33185419917 PASS` | cross-company and foreign/missing stored user/company/branch denial; current behavior parity | ordered normal revert through d1c0a25 lineage |
| `W2-B1` | REM-200 / A-SEC-001 | Sync permission from persistent RBAC, not token boolean alone | `ADOPT — REBOUND TO SEALED PLAN` | claim-only deny, grant allow, explicit deny/revoke | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-B2A` | REM-200 | shared API persistent authorization pipeline | `ADOPT — REBOUND TO SEALED PLAN; 9c5b7a1 / RUN 33185419917 PASS` | claim-only denial, persistent branch grant, wrong-scope denial | ordered normal revert through d1c0a25 lineage |
| `W2-B2B` | REM-200 | local application issuer-specific login/refresh/revoke/logout/session lifecycle | `CODE-ONLY IMPLEMENTED AT cc67ad2; 146/146 PASS; DURABLE ADAPTER/ENDPOINT ACTIVATION BLOCKED BY DBP-003` | exact 18-test lifecycle matrix plus full regression | revert cc67ad2; local issuance remains unregistered until DBP-003 |
| `W2-C1` | REM-220 / D-SEC-SYNC-001 | owner bind all existing Sync lifecycle mutations | `ADOPT — REBOUND TO SEALED PLAN; DEFAULT NO OVERRIDE` | different user/device for transition/retry/conflict/resolve/replacement | normal revert `a157c34...`; stop if exposure would reopen |
| `W2-C2` | REM-220 | device registry/assignment/PoP runtime | `DBP-003B/C DEFERRED — DEPENDS ON DBP-002/006` | prepared device mismatch/revoke/replay/nonce/override matrix; no durable runtime claim | feature disable plus governed recovery |
| `W2-D` | REM-210 / DBP-002 | tenant keys/FKs/checks/indexes/RLS/equivalent | `BLOCKED — LIVE BASELINE + DB-GOV EXECUTION AUTHORITY` | migration/restore/direct-SQL and A↔B matrix | forward correction or safe-copy restore |
| `W2-E` | REM-200/220 / DBP-003 | memberships/sessions/device persistence | `AUTH-001 RESOLVED; DBP-003A REVISE BEFORE REHEARSAL; DBP-003B/C DEFERRED` | PostgreSQL atomic rotation/audit, hash baseline, safe-copy restore/reconciliation, device/replay gates | disable mode; forward correction/restore |
| `W2-F1` | REM-200/210/220 | code-only negative PostgreSQL tests for A1/A2/B1/B2A/C1 | `ADOPT — REBOUND TO SEALED PLAN; 128/128 AT 9c5b7a1` | focused cases included in complete exact-head regression | revert test commits only with corresponding control rollback |
| `W2-F2` | W2 exit | full API/client/offline/device/DB security matrix | `B2B CODE-ONLY PORTION PASS; DURABLE SESSION/DEVICE/DB/EXECUTABLE-CLIENT PORTION BLOCKED` | `W2_F2_TEST_MATRIX.md` | package-specific recovery |

## AUTH-001 implementation rules

- Local application authority is the Production target.
- Tokens do not carry authoritative tenant/RBAC/device grants; persistent server-side resolution remains authoritative.
- Short-lived access tokens and rotating one-time refresh families are required by the selected mode.
- Refresh reuse must revoke the family; logout/revoke must invalidate applicable sessions and protected clients must clear/suspend credentials.
- Signing secrets/keys are deployment secrets and must never be committed.
- Membership/session/refresh-family/device-session persistence requires DBP-003 and remains unimplemented until its gate passes.
- Device registry/PoP/nonce/replay persistence remains behind DBP-003/006.

The adopted packages are ready for later independent MISSION-04 verification but are not final verified. B2B non-destructive work may now proceed; its persistence portion and C2/D/E/F2 remain bounded. MISSION-04 remains waiting for the final MISSION-03 seal.
