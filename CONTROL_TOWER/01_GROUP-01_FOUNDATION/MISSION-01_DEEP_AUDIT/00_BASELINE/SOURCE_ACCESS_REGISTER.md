# SOURCE ACCESS REGISTER — MISSION-01

| Source ID | Type | Name / Path / Ref | Access time UTC | Ref / SHA / Version | Access status | Reviewer | Scope read | Limits |
|---|---|---|---|---|---|---|---|---|
| SRC-001 | Local repository | `/workspace/scratch/2b4238adabfe/TransportERP` | `2026-08-28T00:50:33Z` | `governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1` before this package | AVAILABLE | Control Tower | Control files, Git identity, solution discovery | Governance workspace is not automatically the product authority |
| SRC-002 | Git remote | `origin` | `2026-08-28T00:50:33Z` | Direct remote heads recorded in baseline | AVAILABLE | Control Tower | Symbolic HEAD and selected heads | No fetch/pull used for audit conclusions |
| SRC-003 | GitHub repository metadata | `shfeekalbhure/TransportERP` | `2026-08-28T00:50:33Z` | Default branch `master` | AVAILABLE | Control Tower | Repository identity/default branch | Default branch is not automatically the audit authority |
| SRC-004 | GitHub pull requests | Open PR search | `2026-08-28T00:50:33Z` | PR heads recorded in baseline | AVAILABLE | Control Tower | Open PR metadata | Complete diffs, reviews, CI, and artifacts not yet inventoried |
| SRC-005 | Codex session | TEAM-A | `2026-08-28` | `SESSION IDENTIFIER NOT AVAILABLE TO CONTROL TOWER` | PARTIALLY AVAILABLE | Control Tower | Owner-confirmed existence only | Internal session state not visible; no start inferred |
| SRC-006 | Codex session | TEAM-B | `2026-08-28` | `SESSION IDENTIFIER NOT AVAILABLE TO CONTROL TOWER` | PARTIALLY AVAILABLE | Control Tower | Owner-confirmed existence only | Internal session state not visible; no start inferred |
| SRC-007 | Codex session | TEAM-C1 | `2026-08-28` | `SESSION IDENTIFIER NOT AVAILABLE TO CONTROL TOWER` | PARTIALLY AVAILABLE | Control Tower | Owner-confirmed existence only | Internal session state not visible; no start inferred |
| SRC-008 | Kurrasa / Library | Official TransportERP execution Kurrasa | `2026-08-28T00:50:33Z` | UNKNOWN | PARTIALLY AVAILABLE | Control Tower | Named source recorded only | Content access not verified during PRE-START; teams must verify without guessing |

This shared register contains access facts only. It must not be used to disclose TEAM-A or TEAM-B findings before both independent reports are sealed.
