# MISSION-02 Evidence Index

| ID | Evidence / direct check | Exact source | Result / limitation |
|---|---|---|---|
| `M02-EV-001` | Control Tower start authority | `CURRENT_DIRECTIVE.md`; governance HEAD `f2fc5a73...` | START / planning only |
| `M02-EV-002` | sealed predecessor integrity | MASTER/GATE v2.0 `sha256sum -c` | `14/14 OK` |
| `M02-EV-003` | authoritative product object | `master@2ec6cccf...`, tree `516247dd...` | remote/local exact match |
| `M02-EV-004` | product equality | diff master→governance excluding `CONTROL_TOWER/` | no product delta |
| `M02-EV-005` | exact master inventory | `git ls-tree -r` | 378 files; 10 projects; 1 solution; 10 migrations; 22 C# tests; 1 test project; 7 workflows |
| `M02-EV-006` | Volume P0 | `ConcurrencySafeWaybillRepository.cs:76-87,119-137` | delete/reinsert mapper omits `Volume` |
| `M02-EV-007` | Sync owner/security | `SyncOperationService.cs` master lifecycle methods and `EnsureSameOwnerScope` | owner check limited to enqueue duplicate paths; active-user tenant membership incomplete |
| `M02-EV-008` | identity/RBAC | `Program.cs`; Waybill API modules | JWT claims/literal permissions are current request authority |
| `M02-EV-009` | accounting posting | `VoucherLifecycleService.cs:107-136`; `WaybillFinancePersistence.cs` | POSTED state and collection links do not prove/create balanced ledger effect |
| `M02-EV-010` | audit integrity | `AuditEventService.cs:138-153,160-199`; finance interceptor | canonical hash omits persisted fields; append-only DB parity unknown |
| `M02-EV-011` | clients/domains | solution/project/source inventory; Shipping API/store | Desktop Library, Mobile placeholders, Shipping through DEPART, Ticketing absent |
| `M02-EV-012` | QA/runtime access | local `dotnet --info` | command unavailable; no new runtime PASS |
| `M02-EV-013` | PR69 object/delta | `601f2d1c...`, tree `bfbcd140...`; git diff | open/draft/unmerged; 206 files; +53,011/-858 |
| `M02-EV-014` | PR69 P0 comparison | exact file diff for Waybill repository | identical; P0 not fixed |
| `M02-EV-015` | PR69 security candidate | Identity/Security/device files, migrations and tests | materially addresses; candidate only |
| `M02-EV-016` | PR69 Sync authority/gap | `SyncActionCatalog.cs:96-189`; candidate `SyncOperationService.cs:772-1010,1170-1181` | five available actions; legacy lifecycle owner gap remains |
| `M02-EV-017` | PR69 accounting | exact diff of `VoucherLifecycleService.cs` | unchanged; accounting post gap remains |
| `M02-EV-018` | PR69 client candidate | Desktop WinExe/E2E and Driver MAUI paths | material candidate; Admin/Customer not complete |
| `M02-EV-019` | accepted governing population | TEAM-D v1.1 crosswalk | 64/64 rows used and dispositioned |
| `M02-EV-020` | advisory priorities | TEAM-E v1.1 matrices | 39/39 P0/P1 and 8/8 P2/P3 represented |
| `M02-EV-021` | DB constraints | `DB-GOV-001`, C2 v1.1 DB constraints | all DB proposals gated |
| `M02-EV-022` | inaccessible evidence | no live DB/Production/IdP/latest Kurrasa/external workspace access | explicit unknowns; never PASS |
| `M02-EV-023` | safe governance convergence | preserved local ref `a79574f...`; remote parent `0d8be372...`; GitHub commit `e938881a...`, tree `780d36e4...` | complete MISSION-02 package delivered by Fast-Forward ref update with `force=false`; remote ref/tree/file list re-read |

Historical reports are evidence inputs. Exact current source and Git objects govern whenever they conflict.
