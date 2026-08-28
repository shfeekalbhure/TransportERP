# TEAM-C1 Workspace Preservation Register — v1.1

**Version / scope:** `1.1 / assets directly relevant to TEAM-C1 correction and known architecture comparison`

| Preservation ID | Asset type | Name/path/ref | Branch/ref/full SHA | State | Value/work description | Evidence/findings | Proposed preservation | Reason | Dependencies | Loss/wrong-merge risk | Authority before delete/merge/exclude |
|---|---|---|---|---|---|---|---|---|---|---|---|
| C1-PRES-001 | OTHER / SEALED AUDIT ARTIFACT | `03_TEAM-C1/` v1.0 files outside `v1.1/` | Baseline `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`; individual hashes in v1.0 seal | SUPERSEDED after v1.1 acceptance | Original sealed architecture package and error lineage | C1-CORR-001; v1.0 seal | PRESERVE | Required immutable audit trail | v1.1 supersession record and downstream reconciliation | Silent replacement would destroy evidence lineage | Control Tower reopening/version authority |
| C1-PRES-002 | BRANCH / UNMERGED | `origin/codex/p1-security-device-sync-offline-20260825` / PR #69 | `939f49fa9c2ae57fa532ad55f67461c5f3f256f3` at v1.0 access | UNMERGED | Candidate 13-project Offline/Desktop-E2E architecture | C1-UNMERGED-001 | KEEP UNTIL RECONCILED | Potentially valuable non-current work | TEAM-D/MASTER authority classification | Wrong merge or deletion could lose work or misstate current architecture | Owner/approved Git governance decision |
| C1-PRES-003 | CODEX WORKSPACE | External project sessions/workspaces | UNKNOWN | UNKNOWN | Potential local-only work not visible to TEAM-C1 | C1-UNK-007 | UNKNOWN | Access blocked | Workspace inventory | Undiscovered valuable work could be lost | Workspace owner / Control Tower |
| C1-PRES-004 | OTHER / CORRECTED AUDIT ARTIFACT | `03_TEAM-C1/v1.1/` | Control creation HEAD `e2843caff509d34509146f9dfe2e748dea22df7e`; detached hashes in v1.1 | CURRENT corrected package after acceptance | Correct factory behavior and conformant registers | C1-DATA-002 | PRESERVE | Governing replacement evidence | Control Tower/TEAM-D reopen chain | Modification after seal would break integrity | Explicit REOPEN and new version/seal |

Presence here does not authorize merge, deletion, cleanup, or implementation.
