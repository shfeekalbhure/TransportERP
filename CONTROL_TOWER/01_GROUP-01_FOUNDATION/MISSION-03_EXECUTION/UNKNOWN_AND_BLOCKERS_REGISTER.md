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
| `M03-BLK-W2-001` | tenant hierarchy/cardinality | ADR-W2-001 from exact source/migrations/sealed requirements | code-only REM-210 controls | `RESOLVED FOR EXECUTION DESIGN`; live rows/roles/RLS remain isolated under DBP-002 |
| `M03-BLK-W2-002` | common identity/RBAC/session pipeline | ADR-W2-002; W2-B1/B2A exact tests | authority-neutral implementation | `RESOLVED AND IMPLEMENTED FOR CURRENT PRODUCT API/SYNC`; issuer-specific persistence isolated as AUTH-001/DBP-003 |
| `M03-BLK-W2-003` | device owner/lifecycle/PoP policy | ADR-W2-003; W2-C1 exact tests | owner binding | `RESOLVED FOR IMPLEMENTATION`; registry/PoP persistence remains DBP-003/006 blocked |
| `M03-BLK-W2-004` | Production external authority vs local session issuer | repository and sealed evidence do not select mode | local session endpoints/schema or Production IdP integration only | `OWNER DECISION REQUIRED — BOUNDED ITEM AUTH-001` |
| `M03-BLK-W2-005` | live tenant rows/applied history/roles/RLS and full DBP-002 impact/backfill/recovery | no authorized live DB evidence | W2-D DB mutation and W2 exit | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` |
| `M03-BLK-W2-006` | registry/session/device persistence migration, retention and recovery evidence | DBP-003/006 not execution-authorized | W2-C2/E and full PoP/revoke matrix | `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` |
| `M03-BLK-W2-007` | emergency non-owner override authority | no owner/security-approved permission/reason/audit contract | override only | default deny implemented; `OWNER DECISION REQUIRED — BOUNDED ITEM` before any override |
| `M03-BLK-W2-008` | Control Tower issued a superseding W2 STOP/REPLAN directive at c274f9a after the worker's prior fetched base | current directive and external governance delta; preserved candidate head 9c5b7a1 | every further W2 Product modification and adoption of post-W1 candidates | `PLAN DEVIATION — REVALIDATION REQUIRED`; preserve commits; Control Tower must verify/rebind/revise/reject |
| `M03-BLK-EXT-001` | live DB/schema/applied history/backups unavailable | no authorized DB connection/evidence | DB impacts and release | safe-copy/read-only inventory and restore drill |
| `M03-BLK-EXT-002` | IdP/tenant/cardinality/accounting/offline/Kurrasa authority unavailable | M02 blockers retained | W2–W6 affected packages | provide approved ADRs/authority records |
| `M03-BLK-EXT-003` | signing/release/privacy/Production topology unavailable | M02 blockers retained | W5/W7 | approved non-secret topology and drills |

No blocker is converted into a guessed implementation. W2 candidate A1/A2/B1/B2A/C1/F1 is technically exact-head verified but now held as `PRESERVED CANDIDATE — NOT ADOPTED/NOT AUTHORIZED PENDING CONTROL TOWER REVALIDATION`. No further Product action follows detection of the superseding hold. No destructive, Production, merge, data-repair or irreversible step was attempted.
