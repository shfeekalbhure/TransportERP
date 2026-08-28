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
| `M03-BLK-W2-004` | Production external authority vs local session issuer | owner decision at governance `6b2d238...` selects local application authority | none for code-only lifecycle | `RESOLVED — AUTH-001 LOCAL APPLICATION AUTHORITY`; persistence remains DBP-003 |
| `M03-BLK-W2-005` | live tenant rows/applied history/roles/RLS and full DBP-002 impact/backfill/recovery | no authorized live DB evidence | W2-D DB mutation and W2 exit | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` |
| `M03-BLK-W2-006` | registry/session/device persistence migration, retention and recovery evidence | independent DB-GOV review `DBP-003_DB_GOV_REVIEW_DECISION.md` | durable B2B adapter, W2-C2/E and full PoP/revoke matrix | `DBP-003 HOLD AT REHEARSAL ENTRY — 003A REVISE; 003B/C DEFERRED` |
| `M03-BLK-W2-007` | emergency non-owner override authority | no approved permission/reason/audit contract | no required package; optional future override only | `FINAL DISPOSITION — DEFAULT DENY; NO OWNER DECISION REQUIRED FOR MISSION-03` |
| `M03-BLK-W2-008` | Control Tower issued a superseding W2 STOP/REPLAN directive at c274f9a after the worker's prior fetched base | current directive, exact diff/source, ADRs, run 33185419917, artifact digests and revalidation decision | none for adopted A1/A2/B1/B2A/C1/F1; historical deviation remains evidence | `RESOLVED BY CONTROL TOWER REVALIDATION — SIX BOUNDED PACKAGES ADOPTED` |
| `M03-BLK-EXT-001` | live DB/schema/applied history/backups unavailable | no authorized DB connection/evidence | DB impacts and release | safe-copy/read-only inventory and restore drill |
| `M03-BLK-EXT-002` | IdP/tenant/cardinality/accounting/offline/Kurrasa authority unavailable | M02 blockers retained | W2–W6 affected packages | provide approved ADRs/authority records |
| `M03-BLK-EXT-003` | signing/release/privacy/Production topology unavailable | M02 blockers retained | W5/W7 | approved non-secret topology and drills |
| `DBP003-BLK-001` | PostgreSQL refresh-family lock/single-successor/atomic-audit design required revision | revised proposal, named constraints and atomic code contract; test-only store remains non-durable | independent DBP-003A review/rehearsal gate | `INTERNAL DESIGN RESOLVED — INDEPENDENT DB-GOV + SAFE-COPY EVIDENCE REQUIRED` |
| `DBP003-BLK-002` | password algorithm/format/salt/legacy/rehash/failure/lockout reality unknown | source exposes only `PasswordHash varchar(500)` and test-only literals | login persistence activation | authorized sanitized inventory plus approved verifier/upgrade/lockout policy |
| `DBP003-BLK-003` | no bound safe-copy schema/data snapshot, backup digest, restore proof or reconciliation package | empty disposable CI only | DBP-003A rehearsal entry | prepare named non-Production safe copy and pre/post/restore evidence |
| `DBP003-BLK-004` | User Company/Branch are independent FKs and no User composite alternate key exists | exact `P1Entities.cs` / `TransportErpDbContext.cs` | tenant-consistent session/user and device assignment FKs | split dependency and obtain DBP-002 physical-key decision |
| `DBP003-BLK-005` | device/PoP/nonce store and retention not established | DBP-006, MDM/attestation and retention evidence absent | DBP-003B/C | keep deferred; settle DBP-006 lifecycle/storage/retention |
| `DBP003-BLK-006` | Production signing/encryption/pepper custody/rotation/recovery unproved | no approved operational evidence | Production activation only | provide secret-store ownership and rotation/recovery drill; non-blocking for proposal revision |
| `M03-BLK-W3-001` | accounting posting model was unresolved | ACC-001 selects operational Collection and later governed Settlement posting | no longer blocks execution design; exact mappings/config still block DBP-005 | `RESOLVED BY ACC-001`; configuration/safe-copy evidence remains external |
| `M03-BLK-W3-002` | sanitized legacy audit vectors and representative posting reconciliation unavailable | only repository tests/empty disposable DB are accessible | DBP-004/005 rehearsal and W3 exit | `ACCESS BLOCKED — AUTHORIZED EXTERNAL EVIDENCE REQUIRED` |
| `M03-BLK-W4-001` | per-action Offline authority was unaccepted | OFFLINE-001 supplies default-deny classification and bounded queue criteria | no longer blocks DEP-011 design; DBP-003/006 and W2/W3 atomicity still block runtime | `RESOLVED BY OFFLINE-001` |
| `M03-BLK-W5-001` | client release target scope was absent | CLIENT-001 fixes Windows Desktop + three Android package IDs; iOS deferred | target identity resolved; DEP-013, upstream runtime and Production signing still block W5 exit | `PARTIALLY RESOLVED BY CLIENT-001 — EXTERNAL SIGNING/ROUTE EVIDENCE REMAINS` |
| `M03-BLK-W6-001` | canonical post-departure Shipping/Ticketing/screen authority remains incomplete | current source ends at DEPART; exact Library and screens-workspace locators are reachable but explicitly analysis/design-only and not programming authority | REM-600/610/620 and DBP-007/008 | `AVAILABLE AS NON-GOVERNING ANALYSIS/LOCATORS — CANONICAL PROGRAMMING AUTHORITY REQUIRED` |
| `M03-BLK-W6-002` | DBP-009 Reporting has no distinct sealed REM owner | M02 DB register/test plan include Reporting but REM inventory has no matching package | Reporting only | `PLAN DEVIATION — CONTROL TOWER REVALIDATION REQUIRED`; no scope expansion |
| `M03-BLK-W7-001` | deploy/recovery/RPO/RTO/signing/license/privacy/KMS policies and evidence absent | repository has no publish/sign/backup-restore topology or governing legal/security policy | REM-710/720/730 and W7 exit | `ACCESS BLOCKED — AUTHORIZED EXTERNAL EVIDENCE REQUIRED` |
| `M03-BLK-W8-001` | no stable W7 baseline or complete external workspace ownership inventory | sealed ordering and W0 external inventory unknown | all W8 work | `BLOCKED — W8 NOT ENTERED; NO CLEANUP AUTHORIZED` |

No blocker is converted into a guessed implementation. W2 A1/A2/B1/B2A/C1/F1 and B2B code-only were independently revalidated and adopted at bounded head `cc67ad2...`. Durable B2B, C2, D/E and the persistence/device/client portions of F2 remain individually blocked. Every current DBP-003 next action is non-destructive; no `OWNER DECISION REQUIRED` is raised. No destructive, Production, merge, data-repair or irreversible step was attempted.

## DBP-003 bounded unknowns

| Unknown | Classification | Blocks |
|---|---|---|
| sanitized row counts/data shape, applied lineage on the safe copy, PostgreSQL roles/extensions/RLS and backup restore proof | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | DBP-003 rehearsal entry/activation |
| actual password-hash formats and safe upgrade policy | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | Production identity adapter/login activation |
| signing/password/device key custody and operator rotation/recovery evidence | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | Production issuance/PoP activation |
| nonce/replay/audit retention and legal hold | `DEFERRED — DBP-006 EVIDENCE/DECISION REQUIRED` | DBP-003C/006 retention DDL only |
| device MDM/attestation/platform key capability | `LIVE EXTERNAL EVIDENCE REQUIRED` | attestation strength and executable client release, not code-only lifecycle |

## End-to-end completion disposition

The repository/history/CI and reachable Library evidence paths are exhausted
for the listed external facts.
`MISSION03_COMPLETION_GATE_ASSESSMENT.md` defines the precise evidence and
bounded choices required. Independent packages remain isolated; no blocker is
used to justify a guess or unauthorized mutation.

## v1.0 final blocker reconciliation

Final execution `5d1352b...` and runs `33201720878`/`33201720896` exhaust the
available repository, CI and disposable recovery surfaces. Remaining items are
all external or independently governed: sanitized password/safe-copy/roles/RLS;
legacy accounting/audit/config mappings; executable client/secure-store/signing;
canonical W6 programming authority; Production recovery/privacy/KMS/supply
policy; and complete preservation inventory. No unresolved owner item is raised.
