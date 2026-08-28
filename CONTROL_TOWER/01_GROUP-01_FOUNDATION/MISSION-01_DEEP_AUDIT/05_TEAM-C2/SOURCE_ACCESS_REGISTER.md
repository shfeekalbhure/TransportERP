# TEAM-C2 Source Access Register

- Version: `v1.0`
- Access window: `2026-08-28T02:05:10Z–2026-08-28T02:12:51Z`
- Constraint: read-only outside `05_TEAM-C2/`; no remote fetch, product mutation, build, test, migration, or database access.

| Source ID | Type | Source / ref | Access state | Reviewer / scope | Limit |
|---|---|---|---|---|---|
| C2-SRC-001 | Governance | `CONTROL_TOWER/README.md` | AVAILABLE | coordinator; full | workspace rules only |
| C2-SRC-002 | Governance | Owner directive, autonomous protocol, TEAM-C2 directive, DB-GOV-001 | AVAILABLE | coordinator; full | file-based authority only |
| C2-SRC-003 | Governing command | full MISSION-01 command | AVAILABLE | coordinator + requirements reviewer; full | defines report/design contract |
| C2-SRC-004 | Team order | `05_TEAM-C2/CURRENT_DIRECTIVE.md`, `START_ORDER.md` | AVAILABLE | coordinator; full | START confirmed; proposal only |
| C2-SRC-005 | Sealed reconciliation | complete `04_TEAM-D/` package | AVAILABLE / HASH VERIFIED BY CONTROL TOWER | coordinator + bounded reviewers; full | authoritative line remains unknown |
| C2-SRC-006 | Sealed independent audit | complete `01_TEAM-A/` package | AVAILABLE / SEALED | coordinator/reviewers; architecture, P0/P1, preservation | original temporal authority narrowed by D |
| C2-SRC-007 | Sealed independent audit | complete `02_TEAM-B/` package | AVAILABLE / SEALED | coordinator/reviewers; architecture, DB, runtime, release | `BLK-B-001` retained |
| C2-SRC-008 | Sealed current architecture | complete `03_TEAM-C1/` package | AVAILABLE / SEALED | coordinator/reviewers; full architecture inventory | structural snapshot, not target design |
| C2-SRC-009 | Direct product tree | product files at local snapshot byte-equivalent outside `CONTROL_TOWER/` to `master@2ec6cccf...` | AVAILABLE | coordinator; `.slnx`, csproj references, selected source symbols | proves only inspected snapshot; no build/runtime |
| C2-SRC-010 | Database environment | live/applied PostgreSQL, data, roles/RLS, backups/recovery | ACCESS BLOCKED | DB reviewer; none | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` |
| C2-SRC-011 | Runtime environments | exact-target .NET/Windows/Mobile/API/Production | ACCESS BLOCKED / NOT RUN | runtime/security reviewers; documentary only | no PASS/FAIL or deploy claim |
| C2-SRC-012 | External authority | latest Kurrasa/screen crosswalk/owner current-line record | PARTIALLY AVAILABLE | sealed version-bound evidence only | current authority unresolved |
| C2-SRC-013 | Moving/unmerged source | latest PR #69 and other unmerged/local assets | PARTIALLY AVAILABLE | TEAM-D registers only | latest content/CI/semantic merit not adopted |

No inaccessible source was replaced by inference.
