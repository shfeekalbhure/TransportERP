# Change and Evidence Index

| Evidence ID | Evidence | Exact binding | Result |
|---|---|---|---|
| `M03-EV-001` | M02 v1.2 detached checksum verification | governance `f784dfb...` | `15/15 OK` |
| `M03-EV-002` | remote ref verification | `master`, governance, PR69 branch/pull ref | exact recorded SHAs observed before action |
| `M03-EV-003` | authoritative commit/tree | `2ec6cccf...` / `516247dd...` | exact match |
| `M03-EV-004` | execution branch/worktree | `codex/mission-03-execution-20260828` | created locally at exact master; zero product diff |
| `M03-EV-005` | full tracked tree | `EVIDENCE/W0/MASTER_TRACKED_TREE.txt` | 378 entries |
| `M03-EV-006` | remote branch inventory | `EVIDENCE/W0/REMOTE_HEADS.txt` | 50 branch heads; no remote tags observed |
| `M03-EV-007` | local refs/worktrees/stashes | `LOCAL_REFS.txt`, `WORKTREES.txt`, `STASH_LIST.txt` | two worktrees; current clone stash empty; external state unknown |
| `M03-EV-008` | migration lineage hashes | `EVIDENCE/W0/MIGRATION_SHA256.txt` | 10 implementations, 9 designers, 1 snapshot hashed |
| `M03-EV-009` | project/static test inventory | `PROJECT_SHA256.txt`, `STATIC_BASELINE_COUNTS.txt` | 10 projects; 22 C# test files; 103 static test attributes; not runtime discovery |
| `M03-EV-010` | PR69 exact comparison | `EVIDENCE/W0/PR69_EXACT_SUMMARY.txt` | tree `bfbcd140...`; 206 files; +53,011/-858; unmerged only |
| `M03-EV-011` | preservation bundle | `MISSION03_W0_PRESERVATION_20260828.bundle` | 29,758,224 bytes; SHA-256 `aebcb2399f61295eb002a92c8a8392917d146a06159a47517b3338d52aa4428b` |
| `M03-EV-012` | recovery rehearsal | recovered temporary clone | master/tree/PR69/tree/governance objects recovered; `git fsck` clean |
| `M03-EV-013` | local T-000 probes | Linux worker | every dotnet command exit `127`; PostgreSQL/container tooling absent |
| `M03-EV-014` | GitHub Actions run | run `32867082533`, exact SHA `2ec6cccf...` | success; two jobs success; 124 passed, 0 failed/skipped; 10 migrations applied |
| `M03-EV-015` | GitHub artifact query | run `32867082533` | zero retained artifacts |
| `M03-EV-016` | product change check | execution branch vs authoritative master | zero changed product files |
| `M03-EV-017` | W1 exact-source preflight | `W1_PREFLIGHT_REPORT.md`; exact master and PR69 file comparison | mapper omission confirmed; PR69 identical; implementation blocked by W0/DBP-001 |
| `M03-EV-018` | disposable T-000 workflow lineage | `a48b680...` / tree `638a4f33...` / parent exact master `2ec6cccf...` | evidence-only workflow is the sole baseline delta |
| `M03-EV-019` | W0 exact-head run and retained artifacts | run `33181045881`; artifacts `9689746319`, `9689710882` | both jobs PASS; artifact SHA-256 `fdc6933d...`, `c09c6e20...` |
| `M03-EV-020` | W0 runtime results | .NET SDK 10.0.400; PostgreSQL 18.6; Windows and Ubuntu runners | restore/build PASS; 10 migrations PASS; 124/124 tests; API listening + HTTP 401; Desktop and Mobile x3 probes PASS |
| `M03-EV-021` | REM-100 exact diff | before `a48b680...`; after `069a311...`; tree `561d5862...` | one mapper assignment plus one focused PostgreSQL test; no DB/migration change |
| `M03-EV-022` | REM-100 exact-head verification | run `33181376288`; Linux artifact `9689871882`, SHA-256 `a68e0948...` | 125/125 PASS; focused Volume and split allocation/shipping tests PASS; API/client/migration gates PASS |
| `M03-EV-023` | W2 dependency revalidation at W1 checkpoint | M02 dependencies/waves plus central DB register at `ebe74e0...` | recorded the then-current entry blockers; superseded for DEP-005/006/007 by M03-EV-024..028, while DBP-002/003 mutation gates remain blocked |
| `M03-EV-024` | W2 current-source/migration and PR69 comparative review | exact before `069a311...`; PR69 `601f2d1...` | Company root/Branch child/current singular user assignment; claim-driven auth and absent registry/PoP re-proved |
| `M03-EV-025` | ADR-W2-001/002/003 and package plan/matrix | M03 governance checkpoint | DEP-005 resolved for design; DEP-006/007 resolved for bounded implementation; AUTH-001 and DB gates isolated |
| `M03-EV-026` | W2 code-only exact diff | security commit `a157c34...`; parent `069a311...`; tree `2c02f7a...` | five source/test files; no Entity/DbContext/Migration/Seed/Schema/data change |
| `M03-EV-027` | W2 exact-head run | run `33183870737`; head/tree `04a875a...` / `a134646c...` | both jobs success; 128/128; ten migrations/no model drift; API/Desktop/Mobile pass |
| `M03-EV-028` | W2 retained artifacts | Linux `9690897815`; Desktop `9690854262` | SHA-256 `5226683e...`; `8c61095c...` |
| `M03-EV-029` | W2 API-wide authority-neutral diff | commit `d1c0a257...`; parent `04a875a...`; tree `59ac61e5...` | shared resolver plus three API modules and PostgreSQL/contract test support; no DB model change |
| `M03-EV-030` | W2 failed first A2/B2A run | run `33184771338`; head `d1c0a257...`; core job `98894801318` | `EXECUTION FAILED` at compile: missing OperationContext namespace; Desktop passed; no migration/test/API; disposable DB discarded |
| `M03-EV-031` | bounded corrective commit | `d740740...`; parent `d1c0a257...`; tree `071d4adf...` | one import corrected; no schema/data delta |
| `M03-EV-032` | W2 corrected-head run | run `33184994576`; head/tree `d740740...` / `071d4adf...` | both jobs success; 128/128; ten migrations/no model drift; API/Desktop/Mobile pass |
| `M03-EV-033` | corrected-head retained artifacts | Linux `9691350327`; Desktop `9691310607` | SHA-256 `dddbdbbf...`; `d66ed267...` |
| `M03-EV-034` | explicit API cross-company negative | commit `9c5b7a1...`; parent `d740740...`; tree `452b37f1...` | user A with company/branch B claims is denied; test-only delta, no DB model change |
| `M03-EV-035` | W2 final exact-head run | run `33185419917`; head/tree `9c5b7a1...` / `452b37f1...` | both jobs success; 128/128; ten migrations/no model drift; API/Desktop/Mobile pass |
| `M03-EV-036` | final retained artifacts | Linux `9691527827`; Desktop `9691490016` | SHA-256 `d2410979...`; `4010eeee...` |
| `M03-EV-037` | superseding governance fetch | worker base `b3c5787...`; newly observed Control Tower `c274f9a...` | newer CURRENT_DIRECTIVE imposes W2 STOP/REPLAN; Product work stopped and candidate lineage preserved |
| `M03-EV-038` | Control Tower exact diff/source revalidation | `069a311...9c5b7a1`; 15 paths; linear ancestry | 14 source/test paths plus one workflow line; no Entity/DbContext/Migration/Schema/Seed/data/Production config; `diff --check` PASS |
| `M03-EV-039` | Control Tower GitHub run/log/artifact revalidation | run `33185419917`; jobs `98897056951`/`98897057221`; artifacts `9691527827`/`9691490016` | exact head/tree, 128/128, PostgreSQL 18.6, 10 migrations/no drift, API 401, Desktop/Mobile pass and artifact digests independently confirmed |
| `M03-EV-040` | historical failed-run revalidation | run `33184771338`; head `d1c0a257...`; core job `98894801318` | failure and CS0246 independently confirmed; later steps did not run; recovery commit and passing head retained |
| `M03-EV-041` | package-by-package Control Tower decision | `W2_CONTROL_TOWER_REVALIDATION_DECISION.md` | DEP-005/006/007 revalidated; A1/A2/B1/B2A/C1/F1 each ADOPT/REBOUND; B2B/C2/D/E/F2 remain bounded blockers |
| `M03-EV-042` | AUTH-001 owner decision | governance `6b2d238...` | local application authority selected; code-only B2B released while DBP-003 remains gated |
| `M03-EV-043` | W2-B2B exact code-only diff | `9c5b7a1...cc67ad2`; tree `ea940e59...` | three new code/test files; no Entity/DbContext/Migration/Schema/Seed/data/Production config |
| `M03-EV-044` | W2-B2B exact-head run | run `33191269475`; jobs `98917044706`/`98917044568` | completed/success; 146/146; PostgreSQL 18.6; ten migrations/no drift; API 401; Desktop/Mobile x3 pass |
| `M03-EV-045` | W2-B2B retained artifacts | Linux `9693887564`; Desktop `9693865549` | SHA-256 `aefddb63...`; `88e0e11f...`; unexpired |
| `M03-EV-046` | DBP-003/C2/F2 preparation package | proposal and matrices at governance checkpoint | DBP-003 ready for review; no persistence execution; C2/F2 blockers separated |

The generated W0 evidence sidecar is `EVIDENCE/W0/W0_EVIDENCE_SHA256.txt`.
