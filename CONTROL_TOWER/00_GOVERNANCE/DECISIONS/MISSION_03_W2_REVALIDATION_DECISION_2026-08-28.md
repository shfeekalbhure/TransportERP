# MISSION-03 W2 CONTROL TOWER REVALIDATION DECISION — 2026-08-28

- Decision time UTC: `2026-08-28T16:09:51Z`
- Decision time Asia/Aden: `2026-08-28T19:09:51+03:00`
- Governing branch checked: `governance/control-tower-20260828@8c29dad5eb69ed392b30ef9aadd8fd83891551b3`
- Authoritative product line: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Accepted W1 checkpoint: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Preserved W2 candidate: `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`
- Decision: `W2 HOLD — RETAINED AFTER INDEPENDENT REVALIDATION`
- Candidate disposition: `PRESERVED TECHNICAL CANDIDATE — NOT ADOPTED AS EXECUTION BASELINE`
- MISSION-03: `IN PROGRESS — NOT SEALED`
- MISSION-04: `WAIT`

## Evidence independently reverified

Control Tower re-read the sealed MISSION-02 dependency and wave contracts, the MISSION-03 v0.5 checkpoint, ADR-W2-001/002/003, W2 dependency/plan-deviation records, DB-GOV execution register, test register, manifest, SHA-256 register, seal register and checkpoint handoff.

Repository reality was also rechecked directly:

- execution branch head is exactly `9c5b7a12e59d2c42e682717b8e90c491f8699b96`;
- `069a311b... -> 9c5b7a1...` is five commits ahead and changes 15 files: one evidence workflow plus API/Security/Sync source and tests;
- that compare contains no Entity, DbContext, Migration, Seed, schema, data or Production configuration file;
- exact-head GitHub Actions run `33185419917` completed successfully at `9c5b7a1...`;
- both jobs succeeded; retained artifacts are `9691527827` with digest `sha256:d24109795a2c4f9aff1d82465d7178f2f4eba410b8bd68f86edc504d1ae8357d` and `9691490016` with digest `sha256:4010eeee6c1e4eb504b27e9b14a5af94851528d6ee19c7c582c9f6806f243c1b`;
- the checkpoint test register records `128/128` passed, all ten existing migrations on disposable PostgreSQL 18.6 with no model drift, expected API HTTP 401 boundary, Desktop probe PASS and Mobile Admin/Customer/Driver probes PASS;
- the failed intermediate run `33184771338` remains retained as historical evidence and was not concealed.

These facts establish a technically successful isolated candidate. They do **not** establish that the sealed MISSION-02 W2 entry gates were satisfied before Product implementation or that the candidate can now be adopted retroactively.

## Revalidation against the sealed MISSION-02 contract

### DEP-005

The sealed `DEPENDENCY_AND_SEQUENCE_REGISTER.md` states that `DEP-005 — Tenant hierarchy/cardinality ADR` depends on `canonical authority, live-role evidence` and is a `W2 entry` gate.

Canonical product authority is now resolved, but live user/role/RLS evidence remains `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`. ADR-W2-001 is accepted as useful candidate design evidence, but it does not satisfy the complete sealed dependency as written.

Disposition:

`DEP-005 = NOT YET RELEASED AS A W2 EXECUTION GATE`

### DEP-006

The sealed register states that `DEP-006 — Identity/RBAC/session design` depends on `IdP mode/config and DEP-005`.

ADR-W2-002 isolates `AUTH-001`, but repository evidence still does not establish the Production issuer/session authority mode and DEP-005 is not released. The ADR therefore remains useful design evidence, not a complete execution-gate release.

Disposition:

`DEP-006 = NOT YET RELEASED AS A W2 EXECUTION GATE`

`AUTH-001` is carried as a bounded owner-authority item. It is not made an immediate global owner hold in this decision because the next permitted activity is prerequisite/evidence reconciliation, not issuer-specific Product or Production execution.

### DEP-007

The sealed register states that `DEP-007 — Device registry/PoP and lifecycle owner policy` depends on `DEP-005/006`.

ADR-W2-003 provides a coherent fail-closed candidate policy, but its upstream sealed dependencies are not released and registry/PoP persistence remains separately gated.

Disposition:

`DEP-007 = NOT YET RELEASED AS A W2 EXECUTION GATE`

### W2 wave entry and DB-GOV

The sealed `EXECUTION_WAVES.md` defines W2 entry as `W1 safety closed; IdP/tenant cardinality evidence; DBP-002/003 reviewed` and prohibits implementation from crossing an unmet dependency or stop condition.

The current MISSION-03 `DB_GOV_EXECUTION_REGISTER.md` still records both:

- `DBP-002 = BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED`;
- `DBP-003 = BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED`.

No database mutation occurred, which is correct, but that fact alone does not prove the complete W2 entry contract satisfied.

## Candidate-specific safety uncertainty

The preserved resolver code still depends on cardinality semantics that are not proven from live authoritative evidence. In particular, `CurrentRequestSecurityResolver` accepts a stored active user with `BranchId = null` for a claimed active branch in the same company through `!x.BranchId.HasValue || x.BranchId == branchId`. ADR-W2-001 itself states that explicit membership/company-wide scope must not be inferred as an ungoverned wildcard and that current live membership/role population is unknown.

Until the intended meaning and live population of null/company-wide user and role scopes are proven or the implementation is selectively redesigned under an authorized gate, Control Tower cannot certify the candidate as the authoritative security baseline.

## Package disposition

`W2-A1`, `W2-A2`, `W2-B1`, `W2-B2A`, `W2-C1`, and `W2-F1` remain:

`PRESERVED TECHNICAL CANDIDATE — NOT ADOPTED — NO FURTHER PRODUCT MODIFICATION`

This decision does not declare their code technically failed. It declares that the sealed execution prerequisites are still incomplete and that a material tenant/cardinality authority uncertainty remains.

`W2-B2B`, `W2-C2`, `W2-D`, `W2-E`, and `W2-F2` remain blocked by their recorded authority/DB-GOV/upstream conditions.

## Permitted next work

MISSION-03 may continue only with non-destructive prerequisite work that does not change Product:

1. obtain or register authoritative non-secret IdP mode/config evidence sufficient for DEP-006, or carry the exact owner-reserved choice to its proper gate;
2. obtain authorized read-only/live-role and tenant-cardinality evidence sufficient for DEP-005, including null/company-wide scope semantics and mismatch counts;
3. complete DBP-002/003 impact, preservation, forward-migration and recovery evidence to the level required by DB-GOV-001 before any DB execution request;
4. revise/rebind ADRs and package gates against those facts;
5. preserve all commits, failed/successful runs and artifacts without merge, reset, rewrite, deletion or force-push.

No Product Source, Tests, Migrations, Production configuration, database, PR merge or destructive Git action is authorized by this decision.

## Handoff consequence

MISSION-03 remains `OPEN — NOT SEALED`. The v0.5 checkpoint is received as a revalidation-hold checkpoint only. It is not a final MISSION-04 handoff. MISSION-04 remains `WAIT — PENDING SEALED MISSION-03 EXECUTION OUTPUTS` and must remain independent from the MISSION-03 execution worker.
