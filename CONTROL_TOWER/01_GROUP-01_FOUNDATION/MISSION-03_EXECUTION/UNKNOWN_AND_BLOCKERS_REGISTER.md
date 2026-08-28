# MISSION-03 Unknown and Blockers Register

| ID | Condition | Direct evidence | Blocks | Required resolution / status |
|---|---|---|---|---|
| `M03-BLK-W0-001` | local worker lacks .NET | local exit 127; disposable run 33181045881 uses SDK 10.0.400 | none for bounded execution | `RESOLVED BY DISPOSABLE ENVIRONMENT` |
| `M03-BLK-W0-002` | local worker lacks PostgreSQL/container tooling | disposable PostgreSQL 18.6 migration/test evidence retained | none for bounded execution | `RESOLVED BY DISPOSABLE ENVIRONMENT` |
| `M03-BLK-W0-003` | executable client runtime absent in repository | current probes prove Desktop/Mobile are Library-mode and scaffolds/entry points absent | W5 executable acceptance, not REM-100 | retain factual probe; implement only under W5 gates |
| `M03-BLK-W0-004` | historical artifacts absent | fresh run artifacts retained with digests | none | `RESOLVED` |
| `M03-BLK-W0-005` | external workspaces/local-only/stashes cannot be exhaustively inspected | worker-visible 50 heads, two worktrees and empty stash inventoried; external workspace APIs unavailable | destructive/merge/delete/cleanup and global REM-000 PASS | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`; non-blocking only for isolated additive code-only work |
| `M03-BLK-DB-001` | DBP-001 data state/repair authority absent | central register allows code-only fix; live affected rows unknown | affected-row assessment outside authorized disposable data; all data repair | `CODE FIX RESOLVED; DATA ACTION REMAINS BLOCKED` |
| `M03-BLK-W1-001` | W1 code gate | W0 bounded exit + central DBP-001 code-only authority + exact-head tests | none | `RESOLVED — REM-100 IMPLEMENTED` |
| `M03-BLK-W2-001` | tenant hierarchy/cardinality authority absent | `DEP-005`; DBP-002 says cardinality/live schema/roles unknown | REM-200/210/220 and W2 exit | Control Tower-approved tenant/cardinality ADR and safe live-role evidence |
| `M03-BLK-W2-002` | IdP/RBAC/session design absent | `DEP-006`; DBP-003 requires auth/device design and live baseline | REM-200/220 | approved IdP mode, session/revocation and permission pipeline design |
| `M03-BLK-W2-003` | device registry/PoP/override policy absent | `DEP-007`, M02-BLK-003/010 | REM-220 and later W4/W5 | approved device lifecycle, PoP, caller inventory and audited override policy |
| `M03-BLK-EXT-001` | live DB/schema/applied history/backups unavailable | no authorized DB connection/evidence | DB impacts and release | safe-copy/read-only inventory and restore drill |
| `M03-BLK-EXT-002` | IdP/tenant/cardinality/accounting/offline/Kurrasa authority unavailable | M02 blockers retained | W2–W6 affected packages | provide approved ADRs/authority records |
| `M03-BLK-EXT-003` | signing/release/privacy/Production topology unavailable | M02 blockers retained | W5/W7 | approved non-secret topology and drills |

No blocker is converted into a guessed implementation. No owner HOLD is activated because no destructive, Production, merge, data-repair or irreversible step was attempted. W2 is `BLOCKED`, not failed and not implemented.
