# TEAM-E P0/P1 Advisory Matrix

- Status: `FINAL — SEALED`
- Population: every original predecessor row classified P0/P1 (`2 P0 + 36 P1 = 38 rows`) plus derived TEAM-D v1.1 P1 `D-SEC-SYNC-001`, for `39` reviewed P0/P1 rows.
- Method: preserve each original ID; evaluate the reconciled determination, direct evidence, cross-domain impact, and C2 treatment. `CONCUR` does not promote the assessed snapshot to the authoritative current line.

| Original ID | P | TEAM-D result | TEAM-E advisory disposition | Governing TEAM-E evidence / condition |
|---|---:|---|---|---|
| `A-ARCH-002` | P0 | CONFIRMED | CONCUR | `E-EV-006`; runtime/data population unknown; DB-GOV-001 impact + safe-copy regression required. |
| `A-PRES-001` | P0 | CONFIRMED | CONCUR | `E-EV-007`; preserve before any destructive cleanup; merge merit unknown. |
| `A-SEC-002` | P1 | CONFIRMED | CONCUR WITH EXPANDED SYNC-LIFECYCLE SCOPE | `E-EV-008`, `E-EV-010`; D reopen required for user/device ownership gap. |
| `A-DB-003` | P1 | CONFIRMED | CONCUR — STATIC | `E-EV-009`; live RLS/roles/data unknown. |
| `A-SEC-001` | P1 | CONFIRMED | CONCUR — STATIC | `E-EV-008`; external IdP/session/revocation unknown. |
| `A-DB-004` | P1 | CONFIRMED | CONCUR — STATIC | `E-EV-009`; tenant hierarchy/cardinality decision required. |
| `A-AUD-006` | P1 | CONFIRMED | CONCUR | `E-EV-011`; transaction failure evidence not run. |
| `A-DB-005` | P1 | PARTIALLY CONFIRMED | CONCUR WITH LIVE-DB LIMIT | `E-EV-013`; EF guard exists, raw-SQL/Production boundary unknown. |
| `A-ACCDB-007` | P1 | CONFIRMED | CONCUR | `E-EV-012`; POSTED state is not ledger posting. |
| `A-OFF-001` | P1 | CONFIRMED | CONCUR | `E-EV-010`; no client outbox/worker/pull/executor runtime. |
| `A-OFF-002` | P1 | CONFIRMED | CONCUR — REOPEN REQUIRED FOR NEW OWNER-GAP EVIDENCE | `E-EV-010`; generic enqueue/device-claim/atomicity gaps plus lifecycle owner check omission. |
| `A-RUNTIME-001` | P1 | CONFIRMED | CONCUR | `E-EV-014`; Desktop is Library/no entry point/composition. |
| `A-RUNTIME-002` | P1 | CONFIRMED | CONCUR | `E-EV-014`; Mobile projects are source-empty placeholders. |
| `A-BIZ-001` | P1 | CONFIRMED | CONCUR | `E-EV-015`; shipping remains partial through departure. |
| `A-BIZ-002` | P1 | CONFIRMED | CONCUR | `E-EV-015`; Ticketing/returns/claims/customs runtime absent on snapshot. |
| `A-BIZ-005` | P1 | CONFIRMED | CONCUR | `E-EV-012`; collection references do not create a GL effect. |
| `A-QA-001` | P1 | PARTIALLY CONFIRMED | CONCUR WITH SHA/EXECUTION LIMIT | `E-EV-016`; no TEAM-E runtime execution. |
| `A-QA-002` | P1 | CONFIRMED | CONCUR | `E-EV-016`; acceptance is documentary/contract, not executed runtime. |
| `A-CI-001` | P1 | CONFIRMED | CONCUR | `E-EV-016`; CI does not prove executable clients/release chain. |
| `A-RELEASE-001` | P1 | PARTIALLY CONFIRMED | CONCUR IN REPOSITORY / EXTERNAL STATE BLOCKED | `E-EV-017`; no complete artifact-to-recovery chain. |
| `A-SUPPLY-001` | P1 | PARTIALLY CONFIRMED | CONCUR WITH RESOLVED-GRAPH LIMIT | `E-EV-017`; locks/SBOM/SCA/license/provenance gates absent. |
| `A-PRIV-008` | P1 | PARTIALLY CONFIRMED | CONCUR WITH ENVIRONMENT/LEGAL LIMIT | `E-EV-018`; data surfaces confirmed, end-to-end controls unknown. |
| `A-SCR-001` | P1 | CONFIRMED | CONCUR | `E-EV-019`; canonical screen/version crosswalk remains required. |
| `TB-F-001` | P1 | CONFIRMED | CONCUR | `E-EV-014`; corroborates A runtime findings. |
| `TB-F-002` | P1 | CONFIRMED | CONCUR | `E-EV-008`; resource-server foundation only. |
| `TB-F-003` | P1 | CONFIRMED | CONCUR WITH EXPANDED SYNC-LIFECYCLE SCOPE | `E-EV-008..010`; application and DB defenses incomplete. |
| `TB-F-004` | P1 | CONFIRMED | CONCUR — REOPEN REQUIRED FOR NEW OWNER-GAP EVIDENCE | `E-EV-010`; mapped scope must include lifecycle ownership. |
| `TB-F-005` | P1 | CONFIRMED | CONCUR | `E-EV-012`; status transition only. |
| `TB-F-006` | P1 | CONFIRMED | CONCUR | `E-EV-015`; Ticketing absent. |
| `TB-F-007` | P1 | CONFIRMED | CONCUR | `E-EV-015`; shipping lifecycle incomplete. |
| `TB-F-008` | P1 | PARTIALLY CONFIRMED | CONCUR WITH ENVIRONMENT/LEGAL LIMIT | `E-EV-018`. |
| `TB-F-009` | P1 | PARTIALLY CONFIRMED | CONCUR IN REPOSITORY / EXTERNAL STATE BLOCKED | `E-EV-017`. |
| `TB-F-010` | P1 | PARTIALLY CONFIRMED | CONCUR — VERSION BOUND | `E-EV-019`; latest authority unknown. |
| `TB-F-011` | P1 | CONFIRMED | CONCUR — SHA BOUND | `E-EV-016`; no PASS transfer. |
| `TB-F-012` | P1 | CONFIRMED | CONCUR STATIC / LIVE DB PARTIAL | `E-EV-009`, `E-EV-012`, `E-EV-013`. |
| `TB-F-014` | P1 | PARTIALLY CONFIRMED | CONCUR WITH RESOLVED-GRAPH LIMIT | `E-EV-017`. |
| `TB-F-015` | P1 | CONFIRMED | CONCUR | `E-EV-014`, `E-EV-019`; designs are not executable screens. |
| `TB-F-018` | P1 | CONFIRMED | CONCUR — LIMITATION RETAINED | `E-EV-005`; TEAM-E mitigates coverage, not provenance. |
| `D-SEC-SYNC-001` | P1 | CONFIRMED | CONCUR — STATIC / CONDITIONAL EXPOSURE | `E-EV-010`, accepted D/C2 v1.1; require owner binding or explicit audited privileged override plus negative tests. |

## Portfolio conclusion

- No reviewed P0/P1 was downgraded or dismissed by opinion.
- `A-OFF-002/TB-F-004` and derived `D-SEC-SYNC-001` are correctly expanded in accepted D v1.1 and treated by C2 v1.1.
- All CURRENT language remains narrowed to the assessed snapshot until the authoritative product line and full SHA are recorded.
