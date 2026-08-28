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

The generated W0 evidence sidecar is `EVIDENCE/W0/W0_EVIDENCE_SHA256.txt`.
