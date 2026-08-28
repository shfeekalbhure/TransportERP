# TEAM-D Finding-by-Finding Crosswalk

This crosswalk preserves every formal TEAM-A Finding (`29`), every TEAM-B Finding (`21`), and every TEAM-C1 structural problem (`12`). Original temporal labels are retained as claims of the sealed reports; TEAM-D does not promote them to product authority. `D temporal = UNKNOWN / snapshot-present` means the fact was reverified on the assessed product tree represented by `master@2ec6cccf...`, while the authoritative product line remains unknown.

Allowed reconciliation values are used exactly: `CONFIRMED`, `PARTIALLY CONFIRMED`, `SUPERSEDED`, `FALSE`, `UNKNOWN — REQUIRES VERIFICATION`, `ACCESS BLOCKED — UNKNOWN`.

## TEAM-A findings

| Original ID | Original P/T | Counterparts | Direct recheck / governing evidence | TEAM-D determination | Implementation / verification | D temporal / confidence |
|---|---|---|---|---|---|---|
| A-ARCH-002 | P0 / CURRENT | TB-F-020 conflict | Registered repository deletes/reinserts items; `ToItemEntity` omits `Volume` although domain/entity/read path carry it | CONFIRMED | PARTIAL / VERIFIED STATIC; runtime and affected rows unknown | UNKNOWN / snapshot-present; HIGH |
| A-SEC-002 | P1 / CURRENT | TB-F-003 | `EnsureSecurityAsync` checks active User ID but not `User.CompanyId/BranchId`; later unmerged PR69 snapshots contain candidate binding changes | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC; exploitability and latest unmerged runtime unknown | UNKNOWN / snapshot-present; HIGH |
| A-PRES-001 (Finding) | P0 / LOCAL-ONLY | TB-F-016 | local objects/heads and dirty-artifact evidence remain observable; semantic merge merit not established | CONFIRMED | N/A / VERIFIED for existence and loss risk | LOCAL-ONLY; HIGH |
| A-DB-003 | P1 / CURRENT | TB-F-003, TB-F-012 | soft-delete filters exist; no systemic tenant query filter/RLS or complete tenant-consistent FK design found | CONFIRMED | PARTIAL / VERIFIED STATIC; live roles/data unknown | UNKNOWN / snapshot-present; HIGH |
| A-SEC-001 | P1 / CURRENT | TB-F-002 | API authorization consumes JWT claims; persistent RBAC is not request-time authority | CONFIRMED | PARTIAL / VERIFIED STATIC; IdP/session semantics unknown | UNKNOWN / snapshot-present; HIGH |
| A-DB-004 | P1 / CURRENT | TB-F-003, TB-F-012 | RBAC keys and relationships omit complete tenant/scope dimensions | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-AUD-006 | P1 / CURRENT | TB-F-013 | hash omits persisted fields and several business/audit writes are not one atomic transaction | CONFIRMED | PARTIAL / VERIFIED STATIC; failure injection unknown | UNKNOWN / snapshot-present; HIGH |
| A-DB-005 | P1 / CURRENT | TB-F-012 | EF finance append-only interceptor found; equivalent finance DB trigger not found in assessed tree | PARTIALLY CONFIRMED | PARTIAL / PARTIALLY VERIFIED; live DB/raw SQL unknown | UNKNOWN / snapshot-present; HIGH |
| A-ACCDB-007 | P1 / CURRENT | TB-F-005, TB-F-012 | voucher post changes status only; DB does not prove balanced posting/complete tenant invariants | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-OFF-001 | P1 / CURRENT | TB-F-004 | server enqueue/state exists; no production executor/client outbox/replay loop; version fields incomplete | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-OFF-002 | P1 / CURRENT | TB-F-004 | no registered-device persistence/PoP/allowlisted typed executor; generic payload and non-atomic audit remain | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-RUNTIME-001 | P1 / CURRENT | TB-F-001, TB-F-015, C1-PROB-005 | Desktop conditional output is Library; no entry point, host, API client or subscriber composition | CONFIRMED | PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-RUNTIME-002 | P1 / CURRENT | TB-F-001 | three Mobile projects contain csproj only and zero C# files | CONFIRMED | NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-BIZ-001 | P1 / CURRENT | TB-F-007 | shipping endpoints reach trip start/departure, not arrival/unload/delivery/POD/settlement closure | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-BIZ-002 | P1 / CURRENT | TB-F-006, TB-F-007 | ticketing absent; returns/claims/customs runtime not found in assessed tree | CONFIRMED | NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-BIZ-005 | P1 / CURRENT | TB-F-005 | collection can link a prior accounting reference but does not create/post balanced accounting documents | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-QA-001 | P1 / CURRENT | TB-F-011 scope distinction | no checks for governance audit SHA and local `dotnet` absent; product-identical `master@2ec6...` had separate CI evidence | PARTIALLY CONFIRMED | PARTIAL / PARTIALLY VERIFIED; no TEAM-D execution | UNKNOWN / SHA-specific; HIGH |
| A-QA-002 | P1 / CURRENT | TB-F-015 context | acceptance registers are specified/ready-for-review, not executed runtime acceptance | CONFIRMED | CONTRACT ONLY / VERIFIED DOCUMENTARY | UNKNOWN / snapshot-present; HIGH |
| A-CI-001 | P1 / CURRENT | TB-F-011 | exact governance SHA lacked checks; tracked CI did not prove full clients/release chain | CONFIRMED | PARTIAL / VERIFIED REMOTE+STATIC within snapshot | UNKNOWN / SHA-specific; HIGH |
| A-RELEASE-001 | P1 / CURRENT | TB-F-009 | no repository tags/releases/publish/package/signing/deploy/recovery chain found | PARTIALLY CONFIRMED | NOT IMPLEMENTED in repo / external state ACCESS BLOCKED | UNKNOWN; HIGH within repo |
| A-SUPPLY-001 | P1 / CURRENT | TB-F-014, C1-PROB-011 | no SDK pin, lockfile, SBOM, vulnerability/license/provenance gate | PARTIALLY CONFIRMED | PARTIAL / config verified; resolved graph/advisories unknown | UNKNOWN / snapshot-present; HIGH |
| A-PRIV-008 | P1 / CURRENT | TB-F-008 | sensitive text/JSON surfaces and broad audit data are present; end-to-end encryption/retention controls inaccessible | PARTIALLY CONFIRMED | PARTIAL / PARTIALLY VERIFIED | UNKNOWN / snapshot-present; MEDIUM |
| A-SCR-001 | P1 / CURRENT | TB-F-015 | cited source/design screen identities conflict; full unresolved identity range remains open | CONFIRMED | CONTRACT ONLY / VERIFIED for cited IDs | UNKNOWN / snapshot-present; HIGH |
| A-ARCH-005 | P2 / CURRENT | TB-F-015, C1-PROB-005 | forms expose models/events without executable/API-client/read-projection composition | CONFIRMED | PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-ARCH-006 | P2 / CURRENT | C1-PROB-006, C1-PROB-012 | repeated API boundary and RTL/form mechanics directly observed | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-QA-005 | P2 / CURRENT | TB-F-011 | coverlet reference exists but workflow threshold/upload/retention gate not found | CONFIRMED | NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-ARCH-012 | P3 / CURRENT | TB-F-021, C1-PROB-010 | Domain project physical placement differs from peers; flat solution confirmed; no move authorized | CONFIRMED | N/A / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| A-DB-INFO-009 | INFO / CURRENT | positive context for TB-F-012 | CAS/idempotency/serializable paths, constraints and audit/shipping triggers exist and must be preserved | CONFIRMED | PARTIAL / VERIFIED STATIC; runtime unknown | UNKNOWN / snapshot-present; HIGH |
| A-KUR-002 | INFO / CURRENT | TB-F-010, TB-F-004 | sealed Library evidence records `OFFLINE_WRITE=0` / `Can Queue=NO`; no new authority was supplied | CONFIRMED | N/A / VERIFIED VERSION-BOUND DOCUMENTARY | UNKNOWN authority; HIGH for v72 |

## TEAM-B findings

| Original ID | Original P/T | Counterparts | Direct recheck / governing evidence | TEAM-D determination | Implementation / verification | D temporal / confidence |
|---|---|---|---|---|---|---|
| TB-F-001 | P1 / CURRENT BASELINE | A-RUNTIME-001/002, C1-PROB-005 | Desktop Library/no entry point; Mobile csproj-only | CONFIRMED | PROTOTYPE + NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-002 | P1 / CURRENT BASELINE | A-SEC-001 | resource-server JWT validation exists; login/session/refresh/revoke runtime not found | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-003 | P1 / CURRENT BASELINE | A-SEC-002, A-DB-003/004 | manual tenant predicates and incomplete user/tenant/DB binding directly observed | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-004 | P1 / CURRENT BASELINE | A-OFF-001/002, A-KUR-002 | enqueue foundation without end-to-end offline product | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-005 | P1 / CURRENT BASELINE | A-ACCDB-007, A-BIZ-005 | `Post*` status transition has no journal/audit and ignores actor | CONFIRMED | FOUNDATION ONLY / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-006 | P1 / CURRENT BASELINE | A-BIZ-002 | ticket/booking/passenger/seat runtime absent | CONFIRMED | NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-007 | P1 / CURRENT BASELINE | A-BIZ-001/002 | shipping ends at trip start in assessed source | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-008 | P1 / CURRENT BASELINE | A-PRIV-008 | data surfaces confirmed; controls beyond repo/environment unknown | PARTIALLY CONFIRMED | PARTIAL / PARTIALLY VERIFIED | UNKNOWN / snapshot-present; MEDIUM |
| TB-F-009 | P1 / CURRENT BASELINE | A-RELEASE-001 | repository/remote release evidence absent; external deployment inaccessible | PARTIALLY CONFIRMED | NOT IMPLEMENTED in repo / external ACCESS BLOCKED | UNKNOWN; HIGH within repo |
| TB-F-010 | P1 / CURRENT DOC vs newer source | A-KUR-002, A-SCR-001 | v72 authority markers and older SHA versus newer source are version-bound; latest Library unknown | PARTIALLY CONFIRMED | CONTRACT/DRIFT / PARTIALLY VERIFIED | UNKNOWN authority; HIGH for cited versions |
| TB-F-011 | P1 / CURRENT PRODUCT EVIDENCE | A-QA-001, A-CI-001, A-QA-005 | product CI evidence is SHA-bound and does not prove executable clients/release | CONFIRMED | PARTIAL CI / VERIFIED REMOTE+STATIC | UNKNOWN / SHA-specific; HIGH |
| TB-F-012 | P1 / CURRENT BASELINE | A-DB-003/005, A-ACCDB-007 | DB invariants and operations incomplete; live DB/restore inaccessible | CONFIRMED | PARTIAL / PARTIALLY VERIFIED | UNKNOWN / snapshot-present; HIGH |
| TB-F-013 | P2 / CURRENT BASELINE | A-AUD-006 | `ComputeHash` omits `EntityType`, `DeviceId`, before/after JSON, IP | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-014 | P1 / CURRENT BASELINE | A-SUPPLY-001, C1-PROB-011 | supply-chain locks/gates absent | PARTIALLY CONFIRMED | FOUNDATION / config verified; advisory graph unknown | UNKNOWN / snapshot-present; HIGH |
| TB-F-015 | P1 / CURRENT BASELINE | A-SCR-001, A-RUNTIME-001, A-ARCH-005, C1-PROB-005 | design approvals are not runnable screens; client host absent | CONFIRMED | CONTRACT/PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-016 | P2 / CURRENT REMOTE SNAPSHOT | A-PRES-001 | divergent branch/worktree inventory and preservation risk confirmed; exact live remote inventory moves | CONFIRMED | GOVERNANCE DEBT / VERIFIED SNAPSHOT | UNMERGED+LOCAL-ONLY; HIGH |
| TB-F-017 | P2 / CURRENT/HISTORICAL FOUNDATION | C1-PROB-004, A-ARCH-006 | in-memory P1 prototype coexists with EF path and is test-used, not API-composed | CONFIRMED | PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| TB-F-018 | P1 / CURRENT AUDIT | BLK-B-001 | formation register proves one session/multiple lenses, not multi-reviewer separation | CONFIRMED | GOVERNANCE PROCESS PARTIAL / VERIFIED | CURRENT AUDIT; HIGH |
| TB-F-019 | INFO / MIXED | A-QA-002, A-KUR-002 | documentation corpus contains current/historical/superseded/design claims | CONFIRMED | EVIDENCE FOUNDATION / VERIFIED INVENTORY | MIXED CURRENT/HISTORICAL; HIGH |
| TB-F-020 | INFO / CURRENT AUDIT | conflicts A-ARCH-002 and A-PRES-001 | accessible source contains direct silent-`Volume` loss path satisfying governing P0 risk definition; TEAM-B did not deep-read that repository | FALSE | N/A / CONTRADICTED by direct evidence; process fact “TEAM-B found none” remains bounded | UNKNOWN / snapshot-present; HIGH |
| TB-F-021 | P3 / CURRENT BASELINE | A-ARCH-012, A-SUPPLY-001, C1-PROB-010/011 | build/layout conventions debt directly observed | CONFIRMED | TECHNICAL DEBT / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |

## TEAM-C1 structural problems

TEAM-C1 assigned no priority. TEAM-D preserves that and does not invent severity.

| Original ID | Original temporal | Counterparts | Direct recheck / governing evidence | TEAM-D determination | Implementation / verification | D temporal / confidence |
|---|---|---|---|---|---|---|
| C1-PROB-001 | assessed current architecture | A-DB-003, TB-F-012 | one broad Infrastructure persistence boundary across P1/P2 domains | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-002 | assessed current architecture | C1-PROB-001 | `TransportErpDbContext.cs` concentrates cross-domain model configuration | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-003 | assessed current architecture | A-BIZ-001, A-ARCH-006 | `EfShippingExecutionStore` combines persistence, workflow, transactions, idempotency, audit and mapping | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-004 | assessed current architecture | TB-F-017 | 664-line in-memory P1 model/service is parallel test/prototype semantics | CONFIRMED | PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-005 | assessed current architecture | A-RUNTIME-001, A-ARCH-005, TB-F-001/015 | Desktop structurally disconnected and non-executable | CONFIRMED | PROTOTYPE / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-006 | assessed current architecture | A-ARCH-006 | three Waybill API modules repeat context/claim/GUID/permission/error mechanics | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-007 | assessed current architecture | architecture placement | HTTP request/response/scope/helper types reside in Persistence while endpoint mapping sits in API | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-008 | assessed current architecture | package responsibility | API and Infrastructure directly own EF/Npgsql packages | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-009 | assessed current architecture | A-QA-001 | one test assembly spans unit/contract/API/EF/live-PostgreSQL styles | CONFIRMED | PARTIAL / VERIFIED STATIC; execution unknown | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-010 | assessed current architecture | A-ARCH-012, TB-F-021 | flat `.slnx` does not express physical/module grouping | CONFIRMED | N/A / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-011 | assessed current architecture | A-SUPPLY-001, TB-F-014/021 | no SDK pin, central package config, NuGet config or lockfile | CONFIRMED | NOT IMPLEMENTED / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |
| C1-PROB-012 | assessed current architecture | A-ARCH-006 | 772-line file contains base plus ten shipping forms/catalog responsibilities | CONFIRMED | PARTIAL / VERIFIED STATIC | UNKNOWN / snapshot-present; HIGH |

## Crosswalk completeness and conflict disposition

- Original records covered: `29 TEAM-A + 21 TEAM-B + 12 TEAM-C1 = 62`.
- `TB-F-020` is the only formal `FALSE` determination: it cannot stand as a governing zero-P0 conclusion because `A-ARCH-002` is directly confirmed from accessible source.
- P0 result: `A-ARCH-002` is a confirmed static P0 risk on the assessed source snapshot; existing affected rows/runtime reproduction remain unknown. `A-PRES-001` is a confirmed LOCAL-ONLY P0 preservation risk; preservation does not imply merge approval.
- TEAM-A's original product-authority implication is not accepted. Its source facts are snapshot-valid; final `CURRENT` authority remains unknown.
- `BLK-B-001` remains an assurance limitation. Independent A/C1 work, TEAM-D rechecks, and future TEAM-E review mitigate but do not rewrite TEAM-B's single-session provenance.
