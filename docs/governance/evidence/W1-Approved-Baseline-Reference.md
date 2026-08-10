# W1 Approved Baseline Reference — TransportERP

## Purpose
This is a read-only evidence index for `G2C-W1-BASELINE`. It does not amend any approved W1 decision, model, constraint, ownership, or gap state.

## Current-approved source record
- **Register:** Current Approved References V1.21
- **Authoritative gap register:** Gap_Closure_Matrix_TransportERP_V1.5.xlsx
- **Scope:** logical data model, constraint architecture, ownership and traceability baseline required for the G2 silent-change comparison.

## Immutable source fingerprints

| Artifact | Current-approved location | SHA-256 |
|---|---|---|
| Logical_Data_Model_TransportERP_V1.1.docx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/Logical_Data_Model_TransportERP_V1.1.docx | `c62987141a7014cafb358952511239397a088a34981f62d5004a3017e77e7921` |
| DB_Constraint_Matrix_TransportERP_V1.2.xlsx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/DB_Constraint_Matrix_TransportERP_V1.2.xlsx | `01038c8e758a674e2e623a8624fad664a67fcb1d67701213bf00df2c2113c37b` |
| Entity_Relationship_and_Ownership_Matrix_TransportERP_V1.xlsx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/Entity_Relationship_and_Ownership_Matrix_TransportERP_V1.xlsx | `f8e27a80c6f80ccdae1316ee61e0e67e96085068b3b1f0c162d768f3cc21baee` |
| Screen_to_Entity_Traceability_TransportERP_V1.1.xlsx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/Screen_to_Entity_Traceability_TransportERP_V1.1.xlsx | `ab8255550d5a77bd71b8d90636a97c2e40068a544f847ad2be0c89208e9fd755` |
| OTS_W1_001_Physical_Precision_Closure_Report_V1.docx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/OTS_W1_001_Physical_Precision_Closure_Report_V1.docx | `0e17edf1d361a0707f8190a6ba6512f961525e90242aa6363ec392a5b48ee279` |
| OTS_W1_002_UUIDv7_Physical_PK_Closure_Report_V1.docx | Library /كراسة التنفيذ 8-8/النسخ المعتمدة الحديثة/W1 - Data Model/OTS_W1_002_UUIDv7_Physical_PK_Closure_Report_V1.docx | `9ff8604d12a43220109385fd6b6dc93d61ab50056bb2cc93f9a575f1533cad2b` |

## Recorded closure state
- `OTS-W1-001` Physical Precision: CLOSED in the authoritative matrix.
- `OTS-W1-002` UUIDv7 Physical PK: CLOSED in the authoritative matrix.

## Required validation before G2 closure
1. DATA_MYSQL_ARCHITECT verifies each source artifact against its listed SHA-256.
2. SOLUTION_ARCHITECT independently confirms that repository implementation/evidence has no undocumented delta from the baseline.
3. GENERAL_SUPERVISOR records the independent review result in the authoritative Gap Closure Matrix.
4. This index is evidence only; it does not close `G2C-W1-BASELINE` by itself.
