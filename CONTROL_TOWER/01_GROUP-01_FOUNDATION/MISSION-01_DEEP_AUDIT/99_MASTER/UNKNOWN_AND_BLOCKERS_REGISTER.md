# MASTER/GATE Unknown and Blockers Register

| ID | Unknown / blocker | Governing evidence | Impact | Required next evidence / authority | State |
|---|---|---|---|---|---|
| M-BLK-001 | authoritative product ref/full SHA | D11-BLK-001 / E-BLK-001 | blocks final CURRENT judgment and READY gate | owner/repository authority record; freeze SHA; revalidate | `OWNER DECISION REQUIRED` |
| M-BLK-002 | exact-target restore/build/test/migrate/boot | E-BLK-005 | blocks runtime/release readiness | disposable exact-SHA matrix with retained logs | ACCESS BLOCKED |
| M-BLK-003 | live schema/data/roles/RLS/backups/recovery | E-BLK-006 | blocks DB/security/release readiness | DB-GOV-001-authorized read-only evidence and safe-copy drills | ACCESS BLOCKED |
| M-BLK-004 | affected `Volume` rows and runtime reproduction | A-ARCH-002 / D11-BLK-009 | P0 confirmed; impact population and recovery unknown | non-mutating query + disposable regression/recovery | ACCESS BLOCKED |
| M-BLK-005 | IdP/session/revocation/device guarantees | E-BLK-007 | blocks security readiness | approved redacted configuration and negative tests | ACCESS BLOCKED |
| M-BLK-006 | Production privacy/deployment/recovery controls | E-BLK-008 | blocks Production/release readiness | non-secret config, provenance, runbooks and drills | ACCESS BLOCKED |
| M-BLK-007 | latest Kurrasa and screen/requirement authority | E-BLK-009 | blocks affected implementation scope | immutable canonical version and crosswalk | UNKNOWN |
| M-BLK-008 | local/unmerged asset ownership and disposition | A-PRES-001 / E-BLK-010 | P0 loss risk; blocks destructive cleanup | preserve/hash/bundle; semantic review; owner disposition | `OWNER DECISION REQUIRED BEFORE DESTRUCTIVE ACTION` |
| M-BLK-009 | latest PR #69 and external workspaces | E-BLK-011 | blocks adoption/completeness/destructive decisions | exact-SHA isolated review and global inventory | ACCESS BLOCKED |
| M-BLK-010 | canonical accounting mappings, period, SoD, reversal, subledger rules | E-BLK-012 | blocks executable accounting plan | authoritative requirements/ADR | UNKNOWN |
| M-BLK-011 | cross-module transaction/Unit-of-Work ownership | E-BLK-013 | blocks target implementation readiness | approved ADR preserving module boundaries and atomicity | UNKNOWN — REQUIRES DECISION |
| M-BLK-012 | authorized offline business operations | E-BLK-014 | blocks offline writes | explicit operation-level authority; current guardrail retained | HOLD |
| M-BLK-013 | Sync lifecycle callers/exposure/override policy | D-SEC-SYNC-001 / C2-BLK-017 | blocks safe exposure | caller inventory; owner binding/override policy; negative tests | UNKNOWN |
| M-BLK-014 | TEAM-B multi-reviewer separation | BLK-B-001 / E-BLK-015 | assurance provenance limitation | retained; mitigated by A/C1/D/C2/E chain | MITIGATED, NOT ERASED |
| M-BLK-015 | EF design-time environment and success | C1-CORR-001 / D11-BLK-013 | tooling readiness unknown | disposable exact-SHA EF command with controlled value | ACCESS BLOCKED |

No blocker permits guessing, product modification, database action, Production access, merge, or cleanup.
