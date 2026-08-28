# TEAM-D Formation and Assignment Register

| Role / reviewer | Assignment | Access | Independence / limit | State |
|---|---|---|---|---|
| TEAM-D coordinator `/root/team_d` | govern method, direct source rechecks, integrate Finding-by-Finding crosswalk, author and seal final package | read all inputs; write only `04_TEAM-D/` | sole final-package writer; no product modification | COMPLETE |
| bounded reviewer `/root/team_d/review_a` | independently read/check TEAM-A package and critical source evidence | read-only | did not edit; reported to coordinator | COMPLETE |
| bounded reviewer `/root/team_d/review_b` | independently read/check TEAM-B package, BLK-B-001, and P0 conflict | read-only | did not edit; reported to coordinator | COMPLETE |
| bounded reviewer `/root/team_d/review_c1` | independently read/check C1 package and architecture evidence | read-only | did not edit; reported to coordinator | COMPLETE |
| bounded reviewer `/root/team_d/governance_requirements` | extract exact TEAM-D package/seal rules | read-only | did not edit; reported to coordinator | COMPLETE |
| bounded reviewer `/root/team_d/current_lines` | independently classify candidate refs/SHAs and authority evidence | read-only | did not edit; no authority promotion | COMPLETE |

TEAM-D used multiple bounded reviewers for evidence coverage, while the coordinator alone reconciled and wrote the final package. This does not retroactively convert TEAM-B into a multi-reviewer team and does not erase `BLK-B-001`.
