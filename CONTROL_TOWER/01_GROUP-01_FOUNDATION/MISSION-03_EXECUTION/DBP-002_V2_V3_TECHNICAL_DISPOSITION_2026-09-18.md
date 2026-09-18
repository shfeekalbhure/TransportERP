# DBP-002 v2 versus v3 TECHNICAL DISPOSITION

Date: 2026-09-18
Frozen SHA: `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`

## Decision for re-review input

`v2 = RETAINED FAILURE / HARNESS-SPECIFIC FALSE NEGATIVE`

`v3 = CORRECTED SUPERSEDING REHEARSAL PATH FOR TECHNICAL EVIDENCE`

This is a technical evidence disposition, not the independent DB-GOV acceptance decision.

## Why the v2 failure is harness-specific rather than candidate-significant

1. The v2 failure occurs in `Baseline catalog backup restore reconciliation` after the immutable original ten migrations are applied and restored.
2. At that point v2 has not executed generated candidate SQL, has not applied candidate Migration 11, has not entered RLS/tenant negative testing, has not run the full regression, and has not executed candidate backup/restore.
3. The v2 log shows the reconciliation diff is PostgreSQL textual deparse variance in CHECK definitions. Example:
   - source: `ARRAY['ACTIVE'::character varying, 'INACTIVE'::character varying]::text[]`
   - restored: `ARRAY['ACTIVE'::character varying::text, 'INACTIVE'::character varying::text]`
   These are alternate textual renderings of equivalent CHECK expression semantics.
4. v2 gates equality on raw `pg_get_constraintdef(...)` and similar textual catalog output.
5. The v3 harness deliberately introduces structural/semantic catalog capture:
   - constraint identity and column-key structure rather than raw deparse text;
   - structural index properties rather than raw index text;
   - policy structure plus dedicated normalized sensitive-policy assertions;
   - internal trigger identities normalized instead of comparing generated internal names;
   - raw textual captures are retained as evidence but excluded from equality gates.
6. On the same frozen SHA/tree/parent, v3 then completes:
   - baseline backup/restore structural reconciliation;
   - generated SQL application;
   - EF Migration 11 application;
   - generated-SQL versus EF structural reconciliation;
   - RLS, scope, fail-closed and negative checks;
   - 155/155 full regression;
   - candidate backup/restore structural reconciliation.
7. Independent exact-head W7 recovery also restores 11 migrations with `restore_result=PASS`.

Accordingly, the v2 red result remains historically true, but it does not demonstrate a candidate defect because its failure precedes candidate application and is explained by a raw textual-equivalence gate. v3 is the corrected technical rehearsal path and must not be represented as erasing or converting v2 to PASS.

## Required governance treatment

- Keep run 33222541073 and artifact 9705708441 in the evidence index.
- Use run 33222541097 as the corrected full-rehearsal technical evidence.
- Use W0 33222541108 and W7 33222541109 as corroborating exact-head evidence.
- Require independent DB-GOV re-review before changing DBP-002 acceptance state.
- Do not infer DBP-004 acceptance, MISSION-03 seal, Production authority, or MISSION-04 readiness.
