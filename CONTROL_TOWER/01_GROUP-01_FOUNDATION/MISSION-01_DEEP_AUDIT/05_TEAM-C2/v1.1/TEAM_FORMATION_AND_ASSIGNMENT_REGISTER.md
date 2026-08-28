# TEAM-C2 Formation and Assignment Register

- Version: `v1.1`
- Start UTC / Asia-Aden: `2026-08-28T02:18:36Z` / `2026-08-28T05:18:36+03:00`
- Closure UTC / Asia-Aden: `2026-08-28T02:48:51Z` / `2026-08-28T05:48:51+03:00`
- Corrected predecessor intake/revalidation UTC / Asia-Aden: `2026-08-28T02:40:54Z` / `2026-08-28T05:40:54+03:00`
- Access: read all required sealed inputs and selected direct product evidence; write only `05_TEAM-C2/v1.1/` during this reissue.

| Role / reviewer | Assignment | Access / independence | State |
|---|---|---|---|
| TEAM-C2 coordinator `/root/team_c2` | read governing inputs, recheck selected source structure, choose/integrate proposal, author/seal package | sole final-package writer; no product/DB modification | COMPLETE |
| bounded reviewer `/root/team_c2/architecture_inputs` | extract reconciled architecture/P0/preservation inputs | read-only; no edits | COMPLETE |
| bounded reviewer `/root/team_c2/security_offline` | independently assess security/tenant/offline/runtime constraints | read-only; no edits | COMPLETE |
| bounded reviewer `/root/team_c2/db_accounting` | independently assess DB/migration/accounting constraints | read-only; no edits | COMPLETE |
| bounded reviewer `/root/team_c2/coverage_audit` | extract mandatory package/content/seal checklist | read-only; no edits | COMPLETE |

The coordinator alone made target-design choices and wrote the package. Reviewer use does not erase `BLK-B-001` or transform any predecessor's provenance.
