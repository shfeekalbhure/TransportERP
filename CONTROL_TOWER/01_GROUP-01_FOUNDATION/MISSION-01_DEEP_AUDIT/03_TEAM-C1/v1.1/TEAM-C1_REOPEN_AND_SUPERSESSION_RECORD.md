# TEAM-C1 Reopen and Supersession Record

**Record version:** `1.0`

**Reopen state:** `REOPENED → CORRECTED v1.1 READY FOR SEAL`

**Reopen assignment start:** `2026-08-28T02:18:32Z` / `2026-08-28T05:18:32+03:00` (Asia/Aden)

**Direct-source verification start:** `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00` (Asia/Aden)

**Original package:** TEAM-C1 v1.0 files directly under `03_TEAM-C1/`

**Replacement package:** `03_TEAM-C1/v1.1/`

## Reason

TEAM-E supplied new direct-source evidence that v1.0 made an incorrect claim in two places: it said `TransportErpDbContextFactory` had a source-coded local fallback connection string. Direct inspection of `TransportERP.Infrastructure/Persistence/TransportErpDbContextFactory.cs:8-18` at baseline full SHA `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` proves the opposite: lines 10-15 read `TRANSPORTERP_DESIGN_CONNSTR` and throw `InvalidOperationException` when it is missing/whitespace; lines 16-18 configure only the provided value.

The same source file is byte-identical at control-worktree HEAD `e2843caff509d34509146f9dfe2e748dea22df7e`, SHA-256 `d5c331d2180258fde574484de5f41a6ba78648743e0c7e3df68502620766c74c`.

The reopen also corrects formal register conformance: v1.0 Source Access, Evidence, Files Reviewed, Unknown/Blockers, Domain Coverage, and Manifest did not express every master-command field literally. v1.1 reissues those records with explicit fields and preserves unknown original timestamps rather than inventing them.

## Supersession chain

| Version | State | Preservation | Downstream use |
|---|---|---|---|
| v1.0 | `SEALED → REOPENED → SUPERSEDED BY v1.1` after v1.1 seal | Preserved byte-for-byte in the parent directory; not modified | Historical lineage only after v1.1 acceptance |
| v1.1 | `CORRECTED REPLACEMENT` | New files, new hashes, new seal/handoff | Governing TEAM-C1 package for renewed TEAM-D reconciliation after Control Tower acceptance |

## Correction boundary

- Affected factual ID: `C1-CORR-001`.
- New evidence: `C1-DATA-002`; source `C1-SRC-014`; file row `C1-FILE-017`; unknown boundary `C1-UNK-011`.
- Affected v1.0 documents: `TEAM-C1_CURRENT_ARCHITECTURE_ASSESSMENT.md` section 8; `TEAM-C1_ARCHITECTURE_INVENTORY.md` section 7.
- No other architecture conclusion, project count, dependency map, priority, or runtime claim was changed without evidence.
- No Source, Tests, Migrations, Database, or Production configuration was modified.
