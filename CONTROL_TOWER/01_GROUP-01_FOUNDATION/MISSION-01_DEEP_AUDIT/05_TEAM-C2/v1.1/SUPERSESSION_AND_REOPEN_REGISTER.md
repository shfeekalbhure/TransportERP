# TEAM-C2 Supersession and Reopen Register

- Record version: `v1.1`
- Affected predecessor: `TEAM-C2 Target Architecture Proposal v1.0`
- Predecessor location: `05_TEAM-C2/`
- Corrected package location: `05_TEAM-C2/v1.1/`
- Reopen/reissue start UTC / Asia-Aden: `2026-08-28T02:18:36Z` / `2026-08-28T05:18:36+03:00`
- Reissue closure UTC / Asia-Aden: `2026-08-28T02:48:51Z` / `2026-08-28T05:48:51+03:00`
- Disposition: `Supersedes v1.0 for central acceptance due seal chronology defect`
- v1.0 state: `REJECTED FOR CENTRAL ACCEPTANCE — PRESERVED IMMUTABLY`
- v1.1 state: `REOPENED FOR CORRECTION → SEALED FOR CONTROL TOWER VERIFICATION`

## Defect

The v1.0 `EVIDENCE_INDEX.md` declared collection through `2026-08-28T02:19:00Z`, while its Source Access, Formation, Manifest, and Seal closed at `2026-08-28T02:12:51Z`. The declared end time postdated closure and did not match actual file chronology. Control Tower/TEAM-E rejected the v1.0 handoff.

## Correction

1. v1.0 bytes remain unchanged in the parent directory.
2. v1.1 is a complete self-contained reissue under `05_TEAM-C2/v1.1/`.
3. v1.1 records one truthful revalidation chronology from `2026-08-28T02:18:36Z` through `2026-08-28T02:48:51Z`.
4. The v1.1 Evidence Index distinguishes the revalidation window from predecessor evidence timestamps; it does not rewrite source collection times.
5. Technical conclusions, P0 constraints, unknowns, `BLK-B-001`, `AUTHORITATIVE CURRENT LINE = UNKNOWN`, and `DB-GOV-001` remain unchanged because no new technical evidence changed them.
6. All v1.1 outputs receive new detached SHA-256 values, manifest, seal, and handoff.
7. TEAM-C2 paused before seal when TEAM-E required TEAM-D reopening; it resumed only after Control Tower accepted TEAM-C1 v1.1 and TEAM-D v1.1.
8. v1.1 consumes `C1-CORR-001` and `D-SEC-SYNC-001`, expands its Sync ownership design and Source/Evidence/Crosswalk registers, and otherwise preserves prior conclusions.
9. TEAM-E remains `WAIT` until Control Tower accepts v1.1.

Any later correction requires another explicit version and supersession record; neither v1.0 nor v1.1 may be overwritten silently.
