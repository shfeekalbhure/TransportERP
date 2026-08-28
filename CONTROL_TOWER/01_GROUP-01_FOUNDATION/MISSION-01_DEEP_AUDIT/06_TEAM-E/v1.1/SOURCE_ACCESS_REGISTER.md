# TEAM-E Source Access Register

- Status: `FINAL v1.1 — SEALED`
- Access window: `2026-08-28T02:13:57Z / 2026-08-28T05:13:57+03:00` through v1.1 closure `2026-08-28T02:59:34Z / 2026-08-28T05:59:34+03:00`.

| Source ID | Type | Path/ref/version | UTC / Asia-Aden access | Ref/full SHA | Access | Reviewer / scope / limit |
|---|---|---|---|---|---|---|
| `E-SRC-001` | Governance | `CONTROL_TOWER/README.md`, owner directive, supervision protocol, TEAM-E directive/order, full MISSION-01 command | 2026-08-28 02:14Z / 05:14+03 | governance worktree `e2843caf...`, later parent updates observed | AVAILABLE | coordinator; full governing instructions |
| `E-SRC-002` | Sealed package | `01_TEAM-A/` v1.0 | 2026-08-28 02:15Z / 05:15+03 | product snapshot `2ec6cccf...`; report SHA `e64c66f1...` | AVAILABLE | coordinator/governance reviewer; integrity + critical inputs |
| `E-SRC-003` | Sealed package | `02_TEAM-B/` v1.0 | 2026-08-28 02:15Z / 05:15+03 | product snapshot `2ec6cccf...`; report SHA `51b92496...` | AVAILABLE | coordinator/governance reviewer; integrity/P1/BLK-B-001 |
| `E-SRC-004` | Sealed package | `03_TEAM-C1/` v1.0 | 2026-08-28 02:15Z / 05:15+03 | product snapshot `2ec6cccf...`; report SHA `ef59d438...` | AVAILABLE | coordinator/DB/governance reviewers; architecture + false fallback fact |
| `E-SRC-005` | Sealed package | `04_TEAM-D/` v1.0 | 2026-08-28 02:15Z / 05:15+03 | anchor `8a36f88b...`; report SHA `a4fe28a7...` | AVAILABLE | all reviewers; Crosswalk/report/evidence/unknowns/seal chronology |
| `E-SRC-006` | Sealed package | `05_TEAM-C2/` v1.0 | 2026-08-28 02:15Z / 05:15+03 | input base `432cded2...`; report SHA `721ef8b5...` | AVAILABLE | all reviewers; target design/DB/preservation/seal chronology |
| `E-SRC-007` | Product source | local product tree identical to `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | 2026-08-28 02:16Z–02:30Z / 05:16–05:30+03 | full SHA `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | AVAILABLE | all reviewers; selected P0/P1/P2/P3 direct source rechecks; not authoritative current line |
| `E-SRC-008` | Git local | refs/worktrees/status and product-tree diff | 2026-08-28 02:16Z–02:25Z / 05:16–05:25+03 | governance HEADs moved by Control Tower; product tree unchanged | PARTIALLY AVAILABLE | coordinator/governance reviewer; no fetch/merge; authority unresolved |
| `E-SRC-009` | Database environment | live/applied PostgreSQL, roles/RLS/data/backups | not accessed | unknown | ACCESS BLOCKED | no credentials/live DB; DB-GOV-001; no runtime/data claim |
| `E-SRC-010` | External systems | IdP/session/device, Production, release, encryption/retention/recovery | not accessed | unknown | ACCESS BLOCKED | no external configuration or Production authority |
| `E-SRC-011` | Unmerged latest work | moving PR69/current external workspaces | limited local/remote-tracking observation only | not authoritative; exact current remote head not governing | PARTIALLY AVAILABLE | no evidence transferred to assessed snapshot |
| `E-SRC-012` | Corrected sealed package | `03_TEAM-C1/v1.1/` | 2026-08-28 02:50Z–02:54Z / 05:50–05:54+03 | report SHA `e8a867efc33cd02709e9ef5d897dbb456409c79138f00f43e4d93f65f95a926f` | AVAILABLE | coordinator; full correction, integrity, supersession, seal, handoff; 14/14 detached hashes verified |
| `E-SRC-013` | Corrected sealed package | `04_TEAM-D/v1.1/` | 2026-08-28 02:50Z–02:54Z / 05:50–05:54+03 | report SHA `0f04d8c5200cf7412f7b2ec20485f617c93886b8759409ec9606780f8bfaa73f` | AVAILABLE | coordinator; full 64-row reconciliation, integrity, supersession, seal, handoff; 14/14 detached hashes verified |
| `E-SRC-014` | Corrected sealed package | `05_TEAM-C2/v1.1/` | 2026-08-28 02:50Z–02:54Z / 05:50–05:54+03 | report SHA `0b312a4db66ab78417ae45cfd1a45a54f29b19fba683ac3314f8e5049c40febf` | AVAILABLE | coordinator; full proposal, 27-row crosswalk, integrity, supersession, seal, handoff; 16/16 detached hashes verified |
| `E-SRC-015` | Rejected sealed package | `06_TEAM-E/` v1.0 | 2026-08-28 02:58Z / 05:58+03 | main report `5d067dbf...`; matrix `5395ff24...`; detached list 15/15 passes | AVAILABLE | coordinator; immutable reissue baseline; central rejection was semantic consistency, not byte-integrity failure |

No live database, IdP, Production, external recovery environment, or authoritative-current-line authority record was accessed. Those limits remain explicit and are not converted into PASS claims.
