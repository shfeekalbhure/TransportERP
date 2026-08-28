# TEAM-E Evidence Index

- Status: `FINAL — SEALED`
- Collector: TEAM-E coordinator plus the four actual bounded reviewers in the formation register.
- Product evidence is bound to `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`; authoritative current line remains unknown.

| Evidence ID | Finding/design | Source | Type | Exact location/ref | Result and limit |
|---|---|---|---|---|---|
| `E-EV-001` | input integrity | E-SRC-002 | SHA-256 | TEAM-A manifest/sidecar | 13/13 plus sidecar pass; proves bytes only |
| `E-EV-002` | input integrity/BLK-B-001 | E-SRC-003 | SHA-256/governance | TEAM-B detached checks, formation/seal | 13/13 pass; single-session limitation retained |
| `E-EV-003` | C1 integrity/correction | E-SRC-004/007 | SHA-256/direct source | C1 assessment:204, inventory:122; `TransportErpDbContextFactory.cs:10-15` | 9/9 pass, but sealed fallback claim is false; limited C1 reopen required |
| `E-EV-004` | D integrity/completeness | E-SRC-005 | SHA-256/governance | D detached checks; Crosswalk; Evidence/Manifest/Seal | 13/13 pass; 62 IDs complete; chronology and schema deficiencies require reopen |
| `E-EV-005` | C2 integrity/completeness | E-SRC-006 | SHA-256/governance | C2 detached checks; Evidence/Manifest/Seal | 15/15 pass; 26 IDs complete; chronology inconsistent |
| `E-EV-006` | A-ARCH-002 | E-SRC-007 | Direct code | `WaybillApiModule.cs:14-21`; `ConcurrencySafeWaybillRepository.cs:76-87,119-137`; Domain/contract/entity/migration Volume fields | registered update path deletes/reinserts and omits Volume; runtime/data population unknown |
| `E-EV-007` | A-PRES-001 | E-SRC-005/008 | Git/preservation | D preservation/source-line registers; local worktree inventory | local/unmerged assets and loss risk supported; semantic merge merit unknown |
| `E-EV-008` | A-SEC-001/002, TB-F-002/003 | E-SRC-007 | Direct code | `Program.cs:30-69,92-108,193-195`; `SyncOperationService.cs:346-367`; P1 user/RBAC entities | claims drive request context; active user not bound to claimed company/branch; external controls unknown |
| `E-EV-009` | A-DB-003/004, TB-F-003/012 | E-SRC-007/009 | Code/model | `TransportErpDbContext.cs` tenant/RBAC keys, filters and relationships | soft-delete/manual scope controls exist; systemic tenant DB guarantee/live roles unknown |
| `E-EV-010` | A-OFF-001/002, TB-F-004 | E-SRC-007 | Direct code | `Program.cs:78-144`; `SyncOperationService.cs:68-180,182-395`; Mobile inventory | enqueue foundation only; lifecycle owner check omitted; no current route proved for lifecycle methods; exploitability conditional |
| `E-EV-011` | A-AUD-006, TB-F-013 | E-SRC-007 | Direct code | `AuditEventService.cs:138-153,160-199`; persisted AuditEvent fields | hash omits persisted fields; audit append is separate in several business paths |
| `E-EV-012` | A-ACCDB-007, A-BIZ-005, TB-F-005/012 | E-SRC-007 | Direct code/tests | `VoucherLifecycleService.cs:107-135`; finance persistence; voucher tests | actor ignored; post is status-only; no balanced journal/audit effect |
| `E-EV-013` | A-DB-005 | E-SRC-007/009 | Code/migrations | finance interceptor and hardening migrations | EF append-only positive control; equivalent live DB/raw-SQL boundary unproved |
| `E-EV-014` | runtime/client findings | E-SRC-007 | Project/source | Desktop csproj:4-10; Mobile csproj:4-18; source counts | Desktop Library; Mobile placeholders; no executable client evidence |
| `E-EV-015` | business lifecycle | E-SRC-007 | API/source inventory | shipping modules/endpoints; no Ticketing source | shipping partial; Ticketing absent on snapshot |
| `E-EV-016` | QA/CI/acceptance | E-SRC-005/007 | Workflow/tests/docs | `.github/workflows/ci.yml`; test/acceptance inventory | server/Library build evidence partial; no exact-target full client/release proof |
| `E-EV-017` | release/supply | E-SRC-007/010 | Repository/environment | workflow/project/package/release scans | repository chain and reproducibility gates incomplete; external state blocked |
| `E-EV-018` | privacy | E-SRC-005/007/010 | Source/environment | sensitive payload/audit surfaces; D unknowns | data surfaces confirmed; encryption/retention/legal/Production controls blocked |
| `E-EV-019` | screens/Kurrasa | E-SRC-002..007 | Documentary/source | sealed version-bound screen evidence; Desktop IDs/forms | identity/version conflict remains; latest authority unknown |
| `E-EV-020` | P2/P3 census | E-SRC-005/007 | Crosswalk/direct code | D Crosswalk; audit/API/Desktop/solution/package evidence | all 8 P2/P3 original rows reviewed; no determination changed |
| `E-EV-021` | C2 design suitability | E-SRC-006 | Architecture review | C2 proposal/tree/maps/crosswalk/DB constraints | broadly suitable proposed direction; transaction ownership unresolved |
| `E-EV-022` | seal chronology | E-SRC-005/006 | Governance evidence | D Evidence end 02:02 vs closure 01:59:56; C2 Evidence end 02:19 vs closure 02:12:51 | byte integrity does not cure impossible provenance chronology |
| `E-EV-023` | C1 governed correction | E-SRC-012 | SHA-256/direct source/supersession | C1 v1.1 detached list, `C1-CORR-001`, report/seal/handoff | 14/14 hashes pass; false v1.0 fallback claim corrected to fail-closed configuration; v1.0 provenance retained |
| `E-EV-024` | D governed reconciliation correction | E-SRC-013 | SHA-256/governance/direct evidence | D v1.1 detached list, 64-row Crosswalk, `D-SEC-SYNC-001`, report/seal/handoff | 14/14 hashes pass; valid chronology; required §34 fields present; Sync lifecycle owner gap reconciled |
| `E-EV-025` | C2 governed design reissue | E-SRC-014 | SHA-256/architecture/governance | C2 v1.1 detached list, 27-row crosswalk, `C2-TARGET-027`, `C2-BLK-017`, report/seal/handoff | 16/16 hashes pass; corrected chronology; owner-bound lifecycle design added; transaction-boundary ADR retained as implementation-planning blocker |

No runtime, live database, external IdP, Production, or recovery PASS is claimed.
