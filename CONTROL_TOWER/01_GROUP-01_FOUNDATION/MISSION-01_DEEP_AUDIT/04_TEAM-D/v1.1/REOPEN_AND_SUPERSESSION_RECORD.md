# TEAM-D Reopen and Supersession Record

- Version: `v1.1`
- Reopen directive observed: `2026-08-28T02:20:49Z` / `2026-08-28T05:20:49+03:00` (filesystem timestamp of `04_TEAM-D/CURRENT_DIRECTIVE.md`)
- First v1.1 direct-source evidence snapshot recorded: `2026-08-28T02:25:19Z` / `2026-08-28T05:25:19+03:00`
- Corrected TEAM-C1 v1.1 final package received and reverified: `2026-08-28T02:32:18Z` / `2026-08-28T05:32:18+03:00`
- Original package: TEAM-D v1.0 files directly under `04_TEAM-D/`
- Replacement package: `04_TEAM-D/v1.1/`
- Authoritative current line: `UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

## Reopen causes and resolution

| Reopen evidence | v1.0 defect/new evidence | v1.1 resolution |
|---|---|---|
| `E-REOPEN-001` | C1 v1.0 falsely claimed a source-coded design-time connection fallback | consumes sealed C1 v1.1; adds `C1-CORR-001`; verifies fail-closed factory source |
| `E-REOPEN-002` | v1.0 evidence window ended after recorded closure | preserves v1.0 unchanged; records real v1.1 event times; closure occurs only after all v1.1 evidence and checks |
| `E-REOPEN-003` | v1.0 Crosswalk compressed A/B/C1 positions and omitted per-row Impact/Proposed Action | v1.1 provides all §34 fields for all 62 original rows plus two affected/derived rows |
| `E-REOPEN-004` | sync lifecycle ownership evidence was not included | rechecks four lifecycle methods and owner helper; expands A-OFF-002/TB-F-004 and adds `D-SEC-SYNC-001` |
| governance field audit | Source/Evidence/Files records lacked literal mandatory columns | v1.1 reissues each register with every required field explicitly represented |

## Version lineage

| Version | State | Preservation | Downstream use |
|---|---|---|---|
| v1.0 | `SEALED → REOPENED → SUPERSEDED BY v1.1 AFTER v1.1 ACCEPTANCE` | preserved byte-for-byte in parent directory | historical lineage only after v1.1 Control Tower acceptance |
| v1.1 | `CORRECTED REPLACEMENT — SEALED FOR CONTROL TOWER VERIFICATION` | new subdirectory, hashes, seal and handoff | governing TEAM-D input after central acceptance |

No v1.0 byte or predecessor package was modified. No product, test, migration, database, Production configuration, branch, or Git history was changed.
