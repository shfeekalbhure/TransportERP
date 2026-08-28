# Finding-to-Remediation Crosswalk

## Control rules

- Current means only `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- `PR69` means only `601f2d1cad61d62e590a6714ad84e307eb84fe5f`, never current.
- Detailed scope, DB, API/client/accounting/offline/security impact, preservation, tests, rollback, dependencies and acceptance are inherited from the named `REM-*`, Wave, DB proposal and test records. This table supplies a disposition for every governing TEAM-D v1.1 row.
- Root cause is stated only when directly proved; otherwise it is `UNKNOWN / authority or runtime evidence required`.

## TEAM-A population — 29/29

| Finding | P | Current status and governing evidence | Proved root cause | Remediation / PR69 decision | Order | Remaining unknown |
|---|---:|---|---|---|---:|---|
| `A-ARCH-002` | P0 | CURRENT CONFIRMED; `ConcurrencySafeWaybillRepository.cs:76-87,119-137` deletes/reinserts items and omits `Volume` | mapper omission | `REM-100`; PR69 identical → `REIMPLEMENT` | W1 | affected live rows/source of truth |
| `A-SEC-002` | P1 | CURRENT CONFIRMED; master Sync security validates active user but not stored tenant membership comprehensively | claim/context not composed with authoritative membership | `REM-210/220`; PR69 materially addresses → `VERIFY + SELECTIVE ADOPT` | W2 | IdP, tenant cardinality, runtime exploitability |
| `A-PRES-001` | P0 | LOCAL-ONLY CONFIRMED by sealed preservation chain | destructive cleanup can discard unmerged/local assets | `REM-000`; PR69 itself preserved, never blind-merged | W0 | full external workspace ownership/merit |
| `A-DB-003` | P1 | CURRENT CONFIRMED; no systemic tenant filter/RLS strategy in exact tree | manual/path-specific predicates | `REM-210`; PR69 adds constraints → `VERIFY + SELECTIVE ADOPT UNDER DB-GOV` | W2 | live schema/roles/RLS/data |
| `A-SEC-001` | P1 | CURRENT CONFIRMED; API consumes JWT permission/role claims directly | no single DB-backed request-time session/RBAC/revocation composition | `REM-200`; PR69 strong candidate → `VERIFY + SELECTIVE ADOPT` | W2 | approved auth mode/external IdP guarantees |
| `A-DB-004` | P1 | CURRENT CONFIRMED; RBAC scope keys/relations lack complete tenant dimensions | incomplete scope cardinality/model | `REM-210`; PR69 partial → `VERIFY/REWORK UNDER DB-GOV` | W2 | approved company/branch/user scope model |
| `A-AUD-006` | P1 | CURRENT CONFIRMED; audit hash excludes persisted fields and business/audit writes are not consistently one UoW | canonical hash and transaction ownership incomplete | `REM-300/320`; PR69 partial → `VERIFY/REWORK` | W3 | legacy chain data and UoW ADR |
| `A-DB-005` | P1 | CURRENT PARTIAL; EF interceptor exists, DB/raw-SQL parity unproved | append-only enforcement limited to EF change tracking | `REM-320`; candidate DB changes require review | W3 | live triggers/roles/raw SQL |
| `A-ACCDB-007` | P1 | CURRENT CONFIRMED; `VoucherLifecycleService` transitions to `POSTED` without journal creation | posting modeled as state transition only | `REM-300/310`; PR69 leaves service unchanged → `REIMPLEMENT` | W3 | mappings/period/SoD/currency/subledger rules |
| `A-OFF-001` | P1 | CURRENT CONFIRMED; server queue foundation lacks client store/worker/apply/version E2E | incomplete end-to-end protocol/runtime | `REM-400`; PR69 material → `VERIFY ACTION-BY-ACTION` | W4 | canonical offline authority and environment |
| `A-OFF-002` | P1 | CURRENT CONFIRMED; device/owner/atomicity gaps in lifecycle | lifecycle methods use tenant check without operation owner check | `REM-220/400`; PR69 still retains legacy gap → `REWORK/VERIFY` | W2/W4 | exposed callers and privileged override policy |
| `A-RUNTIME-001` | P1 | CURRENT CONFIRMED; Desktop project is Library/no executable entry point | missing host/composition/API client | `REM-500`; PR69 Desktop candidate → `VERIFY + SELECTIVE ADOPT` | W5 | canonical screens, signing/release target |
| `A-RUNTIME-002` | P1 | CURRENT CONFIRMED; three Mobile projects are source-empty placeholders | no runtime implementation | `REM-500`; PR69 Driver material, Admin/Customer limited → `VERIFY/SCOPED ADOPT` | W5 | approved app/platform scope |
| `A-BIZ-001` | P1 | CURRENT CONFIRMED; Shipping runtime ends at departure | later custody use cases absent | `REM-600`; PR69 does not close lifecycle → `REIMPLEMENT BY INCREMENT` | W6 | canonical post-departure requirements |
| `A-BIZ-002` | P1 | CURRENT CONFIRMED; Ticketing/returns/claims/customs runtime absent | not implemented | `REM-610/600`; PR69 does not close → `REIMPLEMENT AFTER AUTHORITY` | W6 | canonical scope/contracts/screens |
| `A-BIZ-005` | P1 | CURRENT CONFIRMED; collection can link a reference but does not create balanced GL effect | operational and accounting sources not atomically bridged | `REM-310`; PR69 still not closed → `REIMPLEMENT` | W3 | source-of-truth and mapping decision |
| `A-QA-001` | P1 | CURRENT PARTIAL; historical exact-SHA CI exists but no complete matrix; local dotnet absent | environment/matrix gap | `REM-001/700`; PR69 PASS cannot transfer | W0/W7 | executable sandbox/client/runtime evidence |
| `A-QA-002` | P1 | CURRENT CONFIRMED; acceptance registers are documentary, not executed | acceptance execution absent | `REM-700`; adopt test ideas only and rerun | W7 | environment and canonical cases |
| `A-CI-001` | P1 | CURRENT CONFIRMED; 7 workflows do not prove all clients/release surfaces | incomplete required matrix/artifact retention | `REM-001/700`; PR69 patterns `VERIFY`, no PASS transfer | W0/W7 | ruleset/current external CI state |
| `A-RELEASE-001` | P1 | CURRENT PARTIAL; artifact/deploy/rollback/recovery chain absent in repo | release pipeline not implemented in repository | `REM-720`; PR69 CI/E2E insufficient → `VERIFY ONLY` | W7 | external topology/Production state |
| `A-SUPPLY-001` | P1 | CURRENT PARTIAL; no SDK pin/central versions/locks/SBOM/licence/provenance gate | reproducibility policy missing | `REM-710`; PR69 not sufficient → `REIMPLEMENT/VERIFY GRAPH` | W7 | resolved dependency/advisory/license graph |
| `A-PRIV-008` | P1 | CURRENT PARTIAL; sensitive JSON/text surfaces present, end-to-end controls unknown | policy/environment evidence incomplete | `REM-730`; PR69 redaction is candidate subset | W7 | Production encryption, keys, retention/legal hold |
| `A-SCR-001` | P1 | CURRENT CONFIRMED; screen IDs/authority versions conflict | no canonical versioned registry | `REM-620`; PR69 does not resolve | W6 | latest Kurrasa/supersession authority |
| `A-ARCH-005` | P2 | CURRENT CONFIRMED; forms/contracts disconnected from executable API client | missing composition/read projections | `REM-500`; PR69 Desktop candidate verify | W5 | canonical screen/API mapping |
| `A-ARCH-006` | P2 | CURRENT CONFIRMED; repeated API and UI mechanics | duplicated boundary code | `REM-800`; selectively reimplement after parity | W8 | consumer/behavior parity |
| `A-QA-005` | P2 | CURRENT CONFIRMED; no coverage threshold/upload/retention gate | coverage evidence policy absent | `REM-700`; adopt patterns only | W7 | approved thresholds/retention |
| `A-ARCH-012` | P3 | CURRENT CONFIRMED debt; flat/physical layout | historical layout evolution | `REM-800`; defer; no forced move | W8 | approved target tree/consumer impact |
| `A-DB-INFO-009` | INFO | CURRENT positive controls confirmed: CAS/idempotency/constraints/triggers | n/a | preservation requirement `PRES-008`; regression before change | all | live behavior still unproved |
| `A-KUR-002` | INFO | VERSION-BOUND: v72 `OFFLINE_WRITE=0 / Can Queue=NO` | authority remains older/version-bound | `REM-400/620`; reject enabling offline writes without newer authority | W4 | latest canonical authority |

## TEAM-B population — 21/21

| Finding | P | Current status / evidence | Root cause | Remediation / PR69 decision | Order | Remaining unknown |
|---|---:|---|---|---|---:|---|
| `TB-F-001` | P1 | CONFIRMED client non-executability | same as runtime findings | `REM-500`; selective candidate verification | W5 | delivery scope |
| `TB-F-002` | P1 | CONFIRMED resource-server foundation | no authoritative session lifecycle | `REM-200`; verify/selectively adopt | W2 | auth mode/IdP |
| `TB-F-003` | P1 | CONFIRMED manual tenant controls | no systemic server+DB invariant | `REM-210/220`; verify/rework DB candidate | W2 | live roles/cardinality |
| `TB-F-004` | P1 | CONFIRMED + expanded lifecycle ownership | incomplete runtime and owner binding | `REM-220/400`; PR69 material but gap remains | W2/W4 | callers/authority |
| `TB-F-005` | P1 | CONFIRMED status-only voucher post | accounting bridge absent | `REM-300/310`; reimplement | W3 | canonical accounting |
| `TB-F-006` | P1 | CONFIRMED Ticketing absent | not implemented | `REM-610`; reimplement | W6 | requirements |
| `TB-F-007` | P1 | CONFIRMED Shipping partial | post-departure cases absent | `REM-600`; incremental reimplementation | W6 | requirements/accounting |
| `TB-F-008` | P1 | PARTIAL privacy controls | environment/policy unknown | `REM-730`; verify candidate subset | W7 | Production/legal controls |
| `TB-F-009` | P1 | PARTIAL release evidence | repository release chain absent | `REM-720`; candidate CI not adoption proof | W7 | external deployment/recovery |
| `TB-F-010` | P1 | VERSION/TRACEABILITY PARTIAL | stale/multiple authority versions | `REM-620`; no PR69 authority transfer | W6 | canonical Kurrasa |
| `TB-F-011` | P1 | CONFIRMED CI insufficient for release | incomplete matrix/retention | `REM-001/700`; rerun, no PASS transfer | W0/W7 | exact environment |
| `TB-F-012` | P1 | CONFIRMED static/DB partial | invariants and operations incomplete | `REM-210/310/320`; DB-GOV | W2/W3 | live DB/recovery |
| `TB-F-013` | P2 | CONFIRMED hash omissions | canonical input excludes persisted fields | `REM-320`; verify/rework | W3 | legacy chain population |
| `TB-F-014` | P1 | PARTIAL supply assurance | no locked/resolved graph gate | `REM-710`; reimplement | W7 | advisory/license graph |
| `TB-F-015` | P1 | CONFIRMED screens disconnected | design registry not runtime composition | `REM-500/620`; verify Desktop only | W5/W6 | canonical registry |
| `TB-F-016` | P2 | CONFIRMED unmerged/local divergence | parallel refs/workspaces | `REM-000`; preserve; no blind merge/delete | W0 | ownership/semantic merit |
| `TB-F-017` | P2 | CONFIRMED prototype/runtime divergence | in-memory model not API-composed | `REM-800`; isolate then parity/deprecate | W8 | consumers and rule parity |
| `TB-F-018` | P1 | CONFIRMED provenance limitation; mitigated for M01 only | single-session TEAM-B | `REM-900`; retain limitation; independent M03/M04 review | governance | future reviewer independence |
| `TB-F-019` | INFO | CONFIRMED mixed temporal documentation | missing unified authority/supersession index | `REM-900`; exact version/SHA metadata | W0/W7 | external docs |
| `TB-F-020` | INFO | FALSE as zero-P0 conclusion | review scope missed mapper/local assets | preserve contradiction; both P0s govern | W0/W1 | none for disposition |
| `TB-F-021` | P3 | CONFIRMED convention debt | missing build/layout standards | `REM-710/800`; defer cleanup | W7/W8 | approved conventions |

## TEAM-C1 and TEAM-D population — 14/14

| Finding | P | Current status / root cause | Remediation / PR69 decision | Order | Acceptance / unknown |
|---|---:|---|---|---:|---|
| `C1-PROB-001` | P2 | CONFIRMED broad single persistence boundary | `REM-300/800`; logical ownership first | W3/W8 | one UoW preserved until ADR passes |
| `C1-PROB-002` | P2 | CONFIRMED DbContext configuration concentration | `REM-300/800`; isolate mappings, no lineage rewrite | W3/W8 | exact model/migration parity |
| `C1-PROB-003` | P2 | CONFIRMED monolithic Shipping store | `REM-600/800`; split behind ports after behavior tests | W6/W8 | custody/transaction parity |
| `C1-PROB-004` | P2 | CONFIRMED in-memory production divergence | `REM-800`; classify fixture or unify after parity | W8 | consumer inventory |
| `C1-PROB-005` | P1 | CONFIRMED Desktop disconnect | `REM-500`; PR69 candidate verify | W5 | executable smoke/E2E |
| `C1-PROB-006` | P2 | CONFIRMED repeated API helpers | `REM-800`; common tested pipeline | W8 | endpoint negative parity |
| `C1-PROB-007` | P2 | CONFIRMED HTTP types in Persistence | `REM-800`; versioned contract relocation | W8 | consumer/serialization inventory |
| `C1-PROB-008` | P2 | CONFIRMED EF/Npgsql ownership overlap | `REM-800`; dependency ADR | W8 | restore/build/DI parity |
| `C1-PROB-009` | P2 | CONFIRMED mixed test assembly | `REM-700/800`; split without losing IDs/cases | W7/W8 | discovered test-count parity |
| `C1-PROB-010` | P3 | CONFIRMED flat solution | `REM-800`; logical folders first | W8 | no physical move without preservation |
| `C1-PROB-011` | P1 | CONFIRMED missing reproducible package baseline | `REM-710`; capture graph then pin/lock | W7 | resolved graph/license evidence |
| `C1-PROB-012` | P2 | CONFIRMED multi-form concentration | `REM-500/800`; split after RTL/screenshot parity | W5/W8 | screen identity/accessibility |
| `C1-CORR-001` | INFO | CONFIRMED factory fails closed without `TRANSPORTERP_DESIGN_CONNSTR` | `REM-900`; preserve; run isolated EF tooling with synthetic secret | W0/W7 | actual tooling success |
| `D-SEC-SYNC-001` | P1 | CURRENT CONFIRMED static; lifecycle methods lack stored operation user/device comparison | `REM-220`; PR69 legacy methods still gap → rework/verify callers | W2 | exposure and privileged override policy |

## Completeness

`29 TEAM-A + 21 TEAM-B + 13 TEAM-C1/correction + 1 TEAM-D derived = 64/64 dispositions`.

All blocker dispositions are in `UNKNOWN_AND_BLOCKERS_REGISTER.md`; all acceptance/rollback/DB/preservation detail is cross-referenced through the named remediation and Wave records.
