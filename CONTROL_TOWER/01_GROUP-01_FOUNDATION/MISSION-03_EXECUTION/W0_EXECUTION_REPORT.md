# W0 Execution Report — Preservation and Exact-Baseline Evidence

## Disposition

`W0 EXIT CLOSED FOR ISOLATED NON-DESTRUCTIVE EXECUTION — NOT GLOBAL PASS`

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

Disposable run `33181045881` executed at `a48b68023072122c3f71941b861d8b9eeca82d34`, tree `638a4f331e03150fcb9aebf61fbbb4af9f930401`, direct parent authoritative master. Its only tree delta is the W0 evidence workflow. Ubuntu used .NET SDK 10.0.400 and a PostgreSQL 18.6 service; Windows covered Desktop.

- restore and Release builds: PASS;
- committed migration list/model-drift/apply to empty disposable DB: PASS, all 10 migrations;
- complete suite: 124 passed, zero failed/skipped;
- API process booted on `127.0.0.1:5080`; protected endpoint returned expected HTTP 401;
- Desktop actual configuration: `net10.0-windows`, Library, no entry point; build PASS with one existing nullable warning;
- Mobile Admin/Customer/Driver actual configuration: `net10.0`, Library, MAUI runtime not ready; all three restore/build probes PASS;
- retained artifacts: Linux `9689746319` (`fdc6933d...`), Desktop `9689710882` (`c09c6e20...`).

## Gate decision

T-000 and recovery requirements are met for an isolated non-destructive code-only execution path. External workspace/local-only/stash state outside the worker remains `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`; W0 is not labelled a global PASS and destructive/merge/cleanup operations remain prohibited. Because REM-100 neither merges nor deletes nor touches those assets, the bounded W0 exit is closed and W1 was allowed to proceed.
