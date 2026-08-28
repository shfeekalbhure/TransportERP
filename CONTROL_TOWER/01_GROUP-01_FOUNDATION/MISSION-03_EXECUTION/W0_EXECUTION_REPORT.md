# W0 Execution Report — Preservation and Exact-Baseline Evidence

## Disposition

`W0 EXECUTED — MATERIAL EVIDENCE PRODUCED — EXIT BLOCKED`

W0 started from the sealed MISSION-02 v1.2 package. All 15 detached package hashes passed. The authoritative remote master and PR69 refs remained exactly bound to their recorded SHAs and trees.

## Exact baseline

- Product: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Tree: `516247dd320cfc0ef71607cd3d8e7946fe9375ab`.
- Execution branch: `codex/mission-03-execution-20260828` at the same commit/tree.
- Product diff at checkpoint: zero files.
- Governance: `f784dfb273b8244dc2f215e6de283b70639b1037`, clean before W0 outputs.
- PR69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f`, tree `bfbcd14049c97be323decf4785aed37ecad7cc91`, 206 files, +53,011/-858, unmerged evidence only.

## Preservation result

The repository exposed 50 remote branch heads and no remote tags. Current local inventory showed the governance and execution worktrees and no stash entries. A complete-history bundle with 54 refs was created, hash-verified and recovery-tested. External Codex workspaces and local-only assets outside this worker remain unknown, so REM-000 is not closed globally.

## Source/test/migration baseline

- 378 tracked files.
- 10 projects and one `.slnx` solution.
- 10 migration implementations, nine designer files and one model snapshot.
- 22 C# test files; 103 static `[Fact]`/`[Theory]` attributes. This is not runtime discovery.
- seven workflow files.

## Runtime evidence

The current worker cannot execute .NET or PostgreSQL; all requested `dotnet` probes exited 127. Exact-SHA GitHub run `32867082533` historically passed core/PostgreSQL and Desktop Library-mode jobs: 124 tests passed with zero failed/skipped, and all 10 migrations applied to PostgreSQL 18.6. No artifacts remain; API boot and Mobile were not covered.

## Gate decision

W0 exit requires complete preservation inventory and retained exact-SHA restore/build/test/migrate/boot evidence. Those conditions are not fully met. W1 therefore remains blocked, and no Product/DB change was made.
