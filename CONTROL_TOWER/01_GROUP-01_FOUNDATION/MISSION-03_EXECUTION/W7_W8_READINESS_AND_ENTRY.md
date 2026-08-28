# W7/W8 — Readiness and Entry Revalidation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `W7 PREPARATION ONLY; W8 ENTRY CLOSED`
- Product modification/cleanup: `NONE`

## W7 current evidence

- Main CI covers core/PostgreSQL/tests and a Desktop Library build, not Mobile,
  Offline E2E, API boot, coverage or signed executable clients.
- The M03 disposable workflow provides stronger exact-SHA migration/test/API/
  client-build evidence, but no coverage threshold, internal artifact manifest
  or executable client proof.
- There is no SDK pin, central version file, lock file, NuGet source policy,
  current SBOM, license inventory or release provenance.
- There is no publish/package/install/sign, `pg_dump`/`pg_restore`, representative
  upgrade, rollback pair, RPO/RTO or recovery drill.
- Audit currently records Waybill party Name/Mobile in `AfterJson` and has no
  central privacy minimization/retention/legal-hold policy.
- EF design tooling fails closed without its synthetic connection and has
  disposable migration/model-drift evidence. A focused fail-closed test was
  added after this revalidation; its exact passing SHA/run is required before
  REM-900 can advance beyond candidate status.

## Prepared W7 sequence

1. EF/provenance fail-closed test and authority/supersession index (Product
   candidate authored; validation tracked separately).
2. Current exact dependency graph, SDK/source/assets/vulnerability/deprecation
   evidence before any lock change.
3. SDK pin/lock/SBOM/license reproducibility increment after graph review.
4. Exact-head workflow with coverage raw report, artifact hashes/retention and
   truthful client-mode probes.
5. Privacy inventory and approved minimization/redaction controls.
6. Disposable publish, fixture, backup/restore-to-new-instance, reconciliation,
   upgrade and forward-recovery drill.
7. Final W7 matrix only after W2–W6 provide immutable candidates.

W7 final entry is not met; these are prepared increments, not PASS claims.

## W8 decision

`W8 ENTRY = CLOSED — MUST REMAIN LAST`

No cleanup target is approved. Proposed structure candidates—solution folders,
logical DbContext mapping split, large store/test/form decomposition, DTO
boundary cleanup and in-memory-fixture classification—remain proposals only.
There is no authorized delete, rename, move, branch cleanup, migration rewrite
or physical database split. External workspace ownership remains unknown, so
removal work is additionally prohibited.

## External evidence required

- Production deploy/recovery topology, RPO/RTO and off-site backup metadata;
- release/mobile delivery scope and signing authority/custody;
- approved NuGet sources, license/advisory exception and provenance trust policy;
- privacy/legal classification, retention/legal hold and DSAR/delete/anonymize
  rules;
- KMS/encryption key custody and recovery evidence;
- stable completed W2–W7 baseline and complete external workspace inventory.
