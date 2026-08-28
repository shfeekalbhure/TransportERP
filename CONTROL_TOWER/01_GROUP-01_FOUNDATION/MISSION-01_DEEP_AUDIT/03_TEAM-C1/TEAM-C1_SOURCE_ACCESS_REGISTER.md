# TEAM-C1 Source Access Register

**Mission:** MISSION-01_DEEP_AUDIT

**Team:** TEAM-C1 — Current Architecture Assessment

**Assessment date:** 2026-08-28 UTC

**Authoritative ref:** `refs/heads/governance/control-tower-20260828`

**Authoritative SHA:** `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

| ID | Source | Access/result | Architectural use |
|---|---|---|---|
| C1-SRC-001 | Local Git worktree at the authoritative ref | AVAILABLE; clean before TEAM-C1 outputs | Primary source for all current-state conclusions |
| C1-SRC-002 | `origin/governance/control-tower-20260828` | AVAILABLE; same SHA as local HEAD | Ref identity verification |
| C1-SRC-003 | `origin/master` | AVAILABLE at `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | Current-source comparison; the four control-branch-only commits are governance files |
| C1-SRC-004 | Git commit/tree/history and remote refs | AVAILABLE, read-only | Current versus historical/unmerged separation |
| C1-SRC-005 | GitHub checks/status for exact SHA | AVAILABLE; zero check runs, zero workflow runs, combined status `pending` with no contexts | Establishes absence of exact-SHA CI evidence |
| C1-SRC-006 | GitHub PR metadata | AVAILABLE; no PR for the authoritative branch | Branch/PR status |
| C1-SRC-007 | PR #69 / `origin/codex/p1-security-device-sync-offline-20260825` | AVAILABLE, read-only; OPEN, DRAFT, UNMERGED; remote head `939f49fa9c2ae57fa532ad55f67461c5f3f256f3` | Unmerged architecture comparison only |
| C1-SRC-008 | Local .NET SDK/CLI | ACCESS BLOCKED: `dotnet` executable not installed | Prevented local restore/build/test/runtime checks |
| C1-SRC-009 | PostgreSQL runtime/database instance | NOT PROVIDED | Applied schema and live behavior remain unknown |
| C1-SRC-010 | Visual Studio GUI | NOT AVAILABLE | `.slnx` was parsed directly; no GUI-dependent conclusion was used |
| C1-SRC-011 | External deployment/production telemetry | NOT PROVIDED | Deployment state is outside evidence; “runtime” in this report means code-reachable startup composition only |
| C1-SRC-012 | External Codex workspaces/sessions | ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION | Does not alter the authoritative Git tree; prevents exhaustive external-workspace inventory |

## Git-state facts

- Local branch tracks `origin/governance/control-tower-20260828` with no divergence at assessment start.
- There is one local worktree and no local stash or local tag evidence relevant to the assessed tree.
- `origin/master..HEAD` contains four commits, all limited to `CONTROL_TOWER/` governance content.
- The product source tree at the authoritative SHA is identical to `origin/master` at `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- No merge, rebase, cherry-pick, push, force-push, source edit, project edit, solution edit, migration edit, or database edit was performed.
