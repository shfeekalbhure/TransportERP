# MISSION-03 Unknown and Blockers Register

| ID | Condition | Direct evidence | Blocks | Required resolution |
|---|---|---|---|---|
| `M03-BLK-W0-001` | `.NET SDK` absent | `dotnet` command exit 127; filesystem search found no executable | T-000 current restore/build/test/EF/boot/client probes; W0 exit | approved disposable .NET 10 environment |
| `M03-BLK-W0-002` | PostgreSQL/container tooling absent | no Docker/Podman/psql/pg_isready | current fresh migrate/upgrade/DB tests | disposable PostgreSQL 18.6 environment |
| `M03-BLK-W0-003` | API boot and executable client evidence absent | historical CI has no API boot/Mobile and Desktop is Library mode | W0 exit, W5 | Linux/Windows/Android execution matrix |
| `M03-BLK-W0-004` | historical exact-SHA CI artifacts not retained | run 32867082533 artifact list empty | immutable artifact part of T-000 | rerun exact bound matrix with artifact retention |
| `M03-BLK-W0-005` | external workspaces/local-only/stashes cannot be exhaustively inspected | this worker sees only two current worktrees and an empty current stash | full REM-000/PRES-005 closure; any cleanup/delete/merge | Control Tower/Codex global inventory and owner disposition |
| `M03-BLK-DB-001` | central DB-GOV registers have no reviewed entries | both central tables empty | every DBP-001..009 execution | DB authority must populate/review/authorize applicable entries |
| `M03-BLK-W1-001` | W1 entry requires closed W0 and reviewed DBP-001 | sealed wave contract | REM-100 code/data action | close W0 and DBP-001; data repair remains separate owner gate |
| `M03-BLK-EXT-001` | live DB/schema/applied history/backups unavailable | no authorized DB connection/evidence | DB impacts and release | safe-copy/read-only inventory and restore drill |
| `M03-BLK-EXT-002` | IdP/tenant/cardinality/accounting/offline/Kurrasa authority unavailable | M02 blockers retained | W2–W6 affected packages | provide approved ADRs/authority records |
| `M03-BLK-EXT-003` | signing/release/privacy/Production topology unavailable | M02 blockers retained | W5/W7 | approved non-secret topology and drills |

No blocker is converted into a guessed implementation. No owner HOLD is activated because no destructive, Production, merge, data-repair or irreversible step was attempted.
