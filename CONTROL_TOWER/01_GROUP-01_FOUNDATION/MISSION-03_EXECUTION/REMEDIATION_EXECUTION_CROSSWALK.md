# Remediation Execution Crosswalk

| REM | Wave | Current execution state | Evidence / blocker |
|---|---:|---|---|
| `REM-000` | W0 | `CLOSED FOR ISOLATED EXECUTION / GLOBAL UNKNOWN` | repository-visible refs preserved and recovery-tested; external workspace ownership remains access-blocked but no external asset is touched |
| `REM-001` | W0 | `VERIFIED LOCALLY/DISPOSABLE` | exact-lineage run 33181045881 covers restore/build/PostgreSQL migrations/124 tests/API boot/Desktop/Mobile probes with retained artifacts |
| `REM-100` | W1 | `IMPLEMENTED — READY FOR INDEPENDENT VERIFICATION` | mapper fixed at 069a311; 125/125 pass including focused persist/reload Volume and shipping allocation tests; no DB/data change |
| `REM-200` | W2 | `PARTIAL — B1/B2A ADOPTED/IMPLEMENTED; B2B BLOCKED` | persistent RBAC/API exact-head evidence independently revalidated; AUTH-001/DBP-003 remain |
| `REM-210` | W2 | `PARTIAL — A1/A2 ADOPTED/IMPLEMENTED; DB DEFENSE BLOCKED` | server stored-scope controls independently revalidated; DBP-002 remains |
| `REM-220` | W2 | `PARTIAL — C1 ADOPTED/IMPLEMENTED; C2 BLOCKED` | existing Sync lifecycle owner controls independently revalidated; registry/PoP/override/DBP-003/006 remain blocked |
| `REM-300` | W3 | `BLOCKED` | UoW/module-ownership ADR absent |
| `REM-310` | W3 | `BLOCKED` | canonical accounting authority and DBP-005 absent |
| `REM-320` | W3 | `BLOCKED` | UoW ADR, legacy samples and DBP-004 absent |
| `REM-400` | W4 | `BLOCKED` | W2/W3 and operation-level offline authority absent |
| `REM-500` | W5 | `BLOCKED` | W2/W4, executable environments, screens and signing scope absent |
| `REM-600` | W6 | `BLOCKED` | canonical post-departure scope and dependencies absent |
| `REM-610` | W6 | `BLOCKED` | canonical Ticketing authority and DBP-008 absent |
| `REM-620` | W6 | `BLOCKED` | latest Kurrasa/screen supersession authority absent |
| `REM-700` | W7 | `BLOCKED` | no stable remediated exact-head candidate |
| `REM-710` | W7 | `BLOCKED` | resolved graph/SBOM/SCA/license evidence absent |
| `REM-720` | W7 | `BLOCKED` | deploy/upgrade/rollback/restore topology absent |
| `REM-730` | W7 | `BLOCKED` | Production/legal/privacy evidence absent |
| `REM-800` | W8 | `BLOCKED` | W8 parity entry gate not reached |
| `REM-900` | W0/W7 | `PARTIAL — DISPOSABLE TOOLCHAIN VERIFIED` | provenance retained; .NET/EF restore, migration apply/model-drift check and exact-head tests passed in disposable CI, while later release/SBOM/deploy controls remain W7 work |

No finding is marked closed by this checkpoint.
