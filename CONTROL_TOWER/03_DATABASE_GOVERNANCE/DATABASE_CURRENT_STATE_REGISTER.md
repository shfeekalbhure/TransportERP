# DATABASE CURRENT STATE REGISTER

| ID | Area | Entity/Table | Current Evidence | Migration/Schema Ref | Status | Notes |
|---|---|---|---|---|---|---|
| `DB-CUR-001` | Repository EF baseline | EF model/migrations | authoritative product `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`; tree `516247dd320cfc0ef71607cd3d8e7946fe9375ab`; MISSION-03 inventory reports 10 migration implementations + 1 snapshot | exact source/migration inventory in MISSION-03 W0 evidence | `VERIFIED — REPOSITORY ONLY` | Does not prove live applied migration history or schema drift |
| `DB-CUR-002` | Live schema/applied history | Production/non-disposable DB | no current live DB access/evidence in MISSION-03 worker | unknown | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | Blocks DB/data mutation; does not block code-only remediation |
| `DB-CUR-003` | Roles/RLS/DB permissions | PostgreSQL roles/RLS/equivalent | no current live role/RLS inventory available | unknown | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | Required before tenant-defense DB execution |
| `DB-CUR-004` | Waybill Volume affected rows | persisted Waybill/Shipment data | mapper defect re-proved in source; no safe current affected-row count available | `A-ARCH-002 / REM-100 / DBP-001` | `UNKNOWN — READ-ONLY ASSESSMENT REQUIRED` | Do not infer affected rows; any repair requires separate approved DB-GOV action |
| `DB-CUR-005` | Historical disposable validation | PostgreSQL 18.6 historical CI | MISSION-03 reports exact-SHA historical CI with 124/124 tests and 10 migrations applied | historical CI only | `HISTORICAL EVIDENCE — NOT CURRENT RUNTIME PASS` | No retained artifacts; API boot/Mobile not covered; must rerun T-000 |

يوثق الواقع فقط. لا تخلط `Current` مع `Proposed`. أي Live/Production fact غير قابل للفحص يبقى `UNKNOWN` ولا يستنتج من source أو CI تاريخية.
