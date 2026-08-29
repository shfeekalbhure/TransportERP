# TransportERP — Kimi Team Operating Charter

## Purpose
This branch is the dedicated workspace for the Kimi engineering team working on TransportERP.

## Hard boundaries
- Work only on `kimi/*` branches unless the owner explicitly authorizes another branch.
- Never push directly to `master`.
- Never merge a pull request.
- Never force-push or rewrite shared history.
- Never delete governance evidence, audit reports, or owner decisions.
- Do not change an approved architectural/governance decision without explicit owner instruction.
- Preserve traceability for every material change.

## Required delivery evidence
Every completed task must report:
1. Task objective.
2. Branch name.
3. Commit SHA(s).
4. Files changed.
5. Commands/tests executed and results.
6. Known blockers or unresolved risks.
7. Pull request number/link when a PR is opened.

## Team model
- KIMI-00 — Coordinator / task router.
- KIMI-01 — Repository explorer / evidence collector.
- KIMI-02 — Architecture and planning.
- KIMI-03 — Implementation.
- KIMI-04 — Build, tests, migrations, CI verification.
- KIMI-05 — Independent reviewer; must not approve its own implementation by assumption.
- KIMI-06 — Governance and handoff evidence.

## Execution discipline
- Read relevant existing code and governing documents before editing.
- Prefer the smallest change that satisfies the assigned task.
- Keep unrelated refactors out of task commits.
- Do not silently fix adjacent issues; report them separately unless needed to complete the task safely.
- Run applicable restore/build/test checks before handoff.
- Treat database migrations and destructive operations as high-risk and require explicit task authority.
- Never expose secrets, credentials, tokens, connection strings, or private keys in commits or reports.

## Pull-request policy
Changes are delivered through pull requests for owner/reviewer inspection. Kimi has no merge authority by default.

## Current hosted workspace
Primary Kimi branch: `kimi/team-transport-20260829`
