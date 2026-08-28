# Remediation Execution Crosswalk

| REM | Wave | Current execution state | Evidence / blocker |
|---|---:|---|---|
| `REM-000` | W0 | `CLOSED FOR ISOLATED EXECUTION / GLOBAL UNKNOWN` | repository-visible refs preserved and recovery-tested; external workspace ownership remains access-blocked but no external asset is touched |
| `REM-001` | W0 | `VERIFIED LOCALLY/DISPOSABLE` | exact-lineage run 33181045881 covers restore/build/PostgreSQL migrations/124 tests/API boot/Desktop/Mobile probes with retained artifacts |
| `REM-100` | W1 | `IMPLEMENTED — READY FOR INDEPENDENT VERIFICATION` | mapper fixed at 069a311; 125/125 pass including focused persist/reload Volume and shipping allocation tests; no DB/data change |
| `REM-200` | W2 | `PARTIAL — B1/B2A ADOPTED; B2B CODE-ONLY IMPLEMENTED/VERIFIED` | AUTH-001 local; cc67ad2 + run 33191269475; durable persistence/endpoint activation remains DBP-003 |
| `REM-210` | W2 | `PARTIAL — A1/A2 ADOPTED/IMPLEMENTED; DB DEFENSE BLOCKED` | server stored-scope controls independently revalidated; DBP-002 remains |
| `REM-220` | W2 | `PARTIAL — C1 ADOPTED/IMPLEMENTED; C2 BLOCKED` | existing Sync lifecycle owner controls independently revalidated; registry/PoP/override/DBP-003/006 remain blocked |
| `REM-300` | W3 | `PREPARED / PRODUCT BLOCKED` | DEP-008 single-UoW design prepared; Control Tower approval and W2 physical scope remain |
| `REM-310` | W3 | `PREPARED / OWNER+DB BLOCKED` | invariants fixed; accounting model/mappings/SoD/FX/period choice and DBP-005 absent |
| `REM-320` | W3 | `PREPARED / EXTERNAL+DB BLOCKED` | V1 preservation/V2 design prepared; legacy sample and DBP-004 absent |
| `REM-400` | W4 | `PREPARED / PRODUCT BLOCKED` | typed fail-closed design prepared; W2/W3, accepted action matrix and DBP-006 absent |
| `REM-500` | W5 | `PREPARED / PRODUCT BLOCKED` | client truth/test/packaging design complete; W2/W4, DEP-013/014 and executable environments absent |
| `REM-600` | W6 | `PREPARED / CANONICAL INPUT BLOCKED` | current/P2-D/PR69 revalidated; post-departure authority and DBP-007 absent |
| `REM-610` | W6 | `PREPARED / ACCESS BLOCKED` | Ticketing Product absent; external canonical artifacts/contracts and DBP-008 absent |
| `REM-620` | W6 | `PREPARED / ACCESS BLOCKED` | collision inventory ready; latest Kurrasa/screen supersession authority absent |
| `REM-700` | W7 | `PREPARED / ENTRY BLOCKED` | exact-head CI gap and package plan recorded; no stable W2-W6 candidate |
| `REM-710` | W7 | `PREPARED / POLICY BLOCKED` | graph/lock/SBOM/license sequence defined; policy authority absent |
| `REM-720` | W7 | `PREPARED / ACCESS BLOCKED` | recovery drill specified; deploy/RPO/RTO/signing topology absent |
| `REM-730` | W7 | `PREPARED / POLICY BLOCKED` | privacy defect/inventory path recorded; legal/retention/KMS authority absent |
| `REM-800` | W8 | `NOT ENTERED` | W7 parity gate and preservation inventory not reached; no cleanup approved |
| `REM-900` | W0/W7 | `PARTIAL — DISPOSABLE TOOLCHAIN VERIFIED` | provenance retained; .NET/EF restore, migration apply/model-drift check and exact-head tests passed in disposable CI, while later release/SBOM/deploy controls remain W7 work |

No finding is marked closed by this checkpoint.

## v1.0 superseding dispositions

- `REM-200/210/220`: all non-persistent code-only work available from current
  authority is implemented and exact-head tested; material exits remain
  DBP-002/003/006 and external-evidence blocked.
- `REM-300/310/320`: ACC-001 is consumed and direct posting fails closed;
  governed Settlement/audit persistence remains DBP-004/005/external blocked.
- `REM-400`: OFFLINE-001 is consumed and the complete action classification is
  prepared; runtime remains dependency/DBP-006 blocked.
- `REM-500`: approved Android identities are implemented; executable/security/
  signing proof remains external.
- `REM-600/610/620`: reachable inputs are explicitly non-governing; no Product
  implementation was guessed.
- `REM-700/720/900`: exact-head 153/153 baseline and disposable PG18 recovery
  pass at `5d1352b...`; Production policy/topology evidence remains external.
- `REM-800`: not entered. No finding is falsely closed and no mission seal is
  claimed.
