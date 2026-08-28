# TEAM-A — Audit Baseline Used

## Audit identity

- Audit subject: `TransportERP — shfeekalbhure/TransportERP + CONTROL_TOWER/MISSION-01_DEEP_AUDIT`
- Audit team: `TEAM-A — First Independent Review Team`
- Audit start: `2026-08-28T00:27:51Z` / `2026-08-28T03:27:51+03:00 Asia/Aden`
- Repository: `https://github.com/shfeekalbhure/TransportERP`
- Authoritative audit line supplied by the command owner: `refs/heads/governance/control-tower-20260828`
- Full audited SHA: `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`
- Remote default branch observed from GitHub and `refs/remotes/origin/HEAD`: `master`
- Remote default-branch SHA observed: `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Local repository root: `/workspace/scratch/07ca0e73fb59/TransportERP`
- Git directory: `/workspace/scratch/07ca0e73fb59/TransportERP/.git`

## Baseline determination

The branch and SHA named above are the authoritative line for this audit because the Control Tower task owner explicitly supplied that branch and the local clone resolved it to the full SHA above. The remote default branch is recorded separately and is not silently substituted for the audit line.

At baseline capture:

- the working tree was clean;
- one local worktree was visible: this audit clone;
- `git stash list` was empty;
- the audit line was four governance-only commits ahead of `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` and zero commits behind;
- the diff from that `master` SHA to the audit SHA added/changed only `CONTROL_TOWER/` governance artifacts and did not change application source, tests, project files, migrations, or workflows;
- no Git tags were returned by the remote tag inventory;
- GitHub returned no check runs or combined status contexts for the exact audited SHA.

Therefore source-code observations at the audit SHA are materially aligned with the recorded `master` SHA, while verification status remains exact-SHA specific: an older PASS is not promoted to a PASS for the audited SHA.

## Baseline inventory cross-references

- Solution/project inventory and tool/config absence: A-EV-003 and `FILES_REVIEWED_REGISTER.md`.
- Remote inventory: 50 heads, 10 open PRs at snapshot, no remote tags and no GitHub releases: A-EV-002/A-EV-025.
- Workflow inventory: seven tracked workflow files; triggers/gates in A-EV-021.
- Exact-SHA checks: A-EV-022.
- Issues/PRs were inventoried through GitHub; issue state fields not authoritative in the normalized bulk response were not promoted into Findings.
- Environment: Linux x86_64; Python `3.12.13`; `dotnet`, Docker, Podman, `psql`, and `pg_isready` absent in the isolated clone environment: A-EV-027.
- Source types/access/versions/times and unavailable portions: `SOURCE_ACCESS_REGISTER.md`.
- PR69 baseline movement: `AUDIT_BASELINE_DELTA_LOG.md` and A-EV-024.

## Baseline limitations

- Production access was prohibited and was not attempted.
- Other Codex sessions/workspaces were not enumerable through an authoritative local session registry: `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.
- The local environment did not provide `dotnet`; build/test/migration execution was therefore `NOT RUN — UNKNOWN — REQUIRES VERIFICATION`, not FAIL.
- Live database state, applied migration set, data integrity, roles, grants, RLS, backups, restore evidence, encryption, TLS termination, and deployment state were not accessible: `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.

## Independence boundary

No TEAM-B report, finding, evidence index, assessment, or recommendation was opened or used. TEAM-B paths were excluded from content searches. Merely observing the name of a TEAM-B directory or start-order path during repository inventory did not expose its contents and did not influence any finding.
