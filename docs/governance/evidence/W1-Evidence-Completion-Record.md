# W1 Evidence Completion Record — TransportERP

## Evidence identity
- **Work package:** `WP-G2-W1-Evidence-Completion`
- **Author role:** DATA_MYSQL_ARCHITECT
- **Reviewed implementation snapshot:** `setup/initial-solution-structure` at
  `9b4a16f` (before the evidence-only commit that adds these artifacts).
- **Purpose:** establish fixed, independently readable evidence for W1 and review
  the snapshot for an undocumented W1 delta. This record does not modify W1 or
  close any G2 item.

## Immutable artifact manifest

The following files are byte-for-byte copies of the approved sources listed in
`W1-Approved-Baseline-Reference.md`. They are committed under this repository
path so a reviewer can reproduce every SHA-256 calculation without depending on
an external Library session.

| Approved artifact | Fixed repository path | Author SHA-256 | Approved SHA-256 | Result |
|---|---|---|---|---|
| Logical_Data_Model_TransportERP_V1.1.docx | `docs/governance/evidence/W1-approved-artifacts/Logical_Data_Model_TransportERP_V1.1.docx` | `c62987141a7014cafb358952511239397a088a34981f62d5004a3017e77e7921` | `c62987141a7014cafb358952511239397a088a34981f62d5004a3017e77e7921` | MATCH |
| DB_Constraint_Matrix_TransportERP_V1.2.xlsx | `docs/governance/evidence/W1-approved-artifacts/DB_Constraint_Matrix_TransportERP_V1.2.xlsx` | `01038c8e758a674e2e623a8624fad664a67fcb1d67701213bf00df2c2113c37b` | `01038c8e758a674e2e623a8624fad664a67fcb1d67701213bf00df2c2113c37b` | MATCH |
| Entity_Relationship_and_Ownership_Matrix_TransportERP_V1.xlsx | `docs/governance/evidence/W1-approved-artifacts/Entity_Relationship_and_Ownership_Matrix_TransportERP_V1.xlsx` | `f8e27a80c6f80ccdae1316ee61e0e67e96085068b3b1f0c162d768f3cc21baee` | `f8e27a80c6f80ccdae1316ee61e0e67e96085068b3b1f0c162d768f3cc21baee` | MATCH |
| Screen_to_Entity_Traceability_TransportERP_V1.1.xlsx | `docs/governance/evidence/W1-approved-artifacts/Screen_to_Entity_Traceability_TransportERP_V1.1.xlsx` | `ab8255550d5a77bd71b8d90636a97c2e40068a544f847ad2be0c89208e9fd755` | `ab8255550d5a77bd71b8d90636a97c2e40068a544f847ad2be0c89208e9fd755` | MATCH |
| OTS_W1_001_Physical_Precision_Closure_Report_V1.docx | `docs/governance/evidence/W1-approved-artifacts/OTS_W1_001_Physical_Precision_Closure_Report_V1.docx` | `0e17edf1d361a0707f8190a6ba6512f961525e90242aa6363ec392a5b48ee279` | `0e17edf1d361a0707f8190a6ba6512f961525e90242aa6363ec392a5b48ee279` | MATCH |
| OTS_W1_002_UUIDv7_Physical_PK_Closure_Report_V1.docx | `docs/governance/evidence/W1-approved-artifacts/OTS_W1_002_UUIDv7_Physical_PK_Closure_Report_V1.docx` | `9ff8604d12a43220109385fd6b6dc93d61ab50056bb2cc93f9a575f1533cad2b` | `9ff8604d12a43220109385fd6b6dc93d61ab50056bb2cc93f9a575f1533cad2b` | MATCH |

Reproduction command from repository root:

```bash
find docs/governance/evidence/W1-approved-artifacts -type f -print0 | sort -z | xargs -0 sha256sum
```

## DATA_MYSQL_ARCHITECT silent-change review

| Review area | Evidence examined | Finding |
|---|---|---|
| Logical Data Model | Approved DOCX above; current Domain/Application/Contracts source and project files | No data entities, aggregate definitions, or competing logical model was added in the reviewed snapshot. |
| Entities / relationships / ownership | Current source tree and project references | No entity persistence model, relationship configuration, repository, or ownership mapping exists that can diverge from the approved matrices. |
| Constraints | Source and project-file scan for MySQL/EF/DDL/migrations/constraint mapping | No MySQL/EF provider, DbContext, migration, SQL DDL, `HasKey`, `HasIndex`, or constraint mapping is present. |
| Precision | Source scan for `HasPrecision`, database decimal mapping, DDL | No executable physical precision mapping exists. No conflicting mapping was found. |
| UUIDv7 physical mapping | Source scan for UUIDv7/physical key mapping and DDL | No executable UUIDv7 physical mapping exists. No conflicting mapping was found. |
| Undocumented W1 change | `25f3b1d..9b4a16f` changed API W2 implementation and W3 runtime evidence only | No changed file in that range changes W1 model, relationship, ownership, constraint, precision, or UUID physical mapping. |

### Author judgement

**`PASS WITH NOTES`** — At snapshot `9b4a16f`, no evidence of a silent W1
implementation change exists. This is a negative finding based on the absence of
persistence/DDL/ORM implementation, not a declaration that future physical
mapping is approved.

## Mandatory boundary: logical model vs. physical mapping

- **Logical approved model:** the logical data model, constraint matrix,
  entity/relationship/ownership matrix, and screen traceability artifact are
  frozen evidence in this package; their hashes are verified above.
- **Physical mapping deferred:** W1 OTS precision and UUIDv7 closure reports are
  preserved as approved reference evidence. Their executable verification must
  occur with the first persistence/DDL/ORM work package, when it must compare
  migrations and mappings to the approved precision and UUIDv7 decisions.

## Independent-review ledger

| Required independent role | Required action | Status | Verdict |
|---|---|---|---|
| SOLUTION_ARCHITECT | Independently inspect the fixed artifact manifest and current branch for an undocumented W1 architectural/data-model delta. | PENDING | — |
| QA_TESTING_REVIEWER | From a clean checkout, independently run the reproduction command and compare all six values to this manifest; verify the reviewer result is traceable to the evidence commit. | PENDING | — |

## Current disposition

- No W1 document was changed.
- No W1 gap is closed by this record.
- `G2 = NOT READY` remains unchanged.
- `G2C-W1-BASELINE` remains pending until both independent review rows are
  completed and the operational Gap Closure Matrix is updated by its owner.
