# TEAM-C2 Unknown and Blockers Register

- Version: `v1.0`
- Rule: unknowns outside design access do not block sealing this proposal; their gate/implementation effects remain explicit.

| Blocker ID | Unresolved question | Affected design / findings | Missing source / cause | Effect | Next verification / authority | Gate impact |
|---|---|---|---|---|---|---|
| C2-BLK-001 | Which ref/full SHA is authoritative current product line? | all current/transition assumptions; D-BLK-001 | no authority record | design is snapshot-bound; MASTER cannot assert current/readiness | owner/repository authority record; revalidate target deltas | BLOCKS READY, not C2 seal |
| C2-BLK-002 | Does exact target restore/build/test/migrate/boot? | runtime/test/release; D-BLK-002 | no authorized exact-target environment | no PASS/FAIL/runtime claim | disposable exact-SHA matrix | BLOCKS READY/release |
| C2-BLK-003 | What is live PostgreSQL schema/data/roles/RLS/drift/recovery? | DB/tenant/accounting; D-BLK-003 | database access blocked | physical DB decisions and safety unknown | authorized read-only evidence + disposable restore/upgrade | BLOCKS READY/DB execution |
| C2-BLK-004 | Which rows were affected by `Volume` omission? | C2-TARGET-013; D-BLK-009 | no runtime/data access | data-repair scope/source of truth unknown | DB-GOV-001 impact query on safe copy | BLOCKS safe remediation/release |
| C2-BLK-005 | What does external IdP/session/device system guarantee? | C2-TARGET-015; D-BLK-004 | config/runtime inaccessible | final auth/device integration undecided | approved config and negative tests | BLOCKS security readiness |
| C2-BLK-006 | What Production encryption/retention/legal-hold/backup/recovery/deployment controls exist? | privacy/release; D-BLK-005 | Production/environment inaccessible | target controls cannot be gap-closed | authorized evidence without secrets/mutation | BLOCKS READY |
| C2-BLK-007 | What is latest canonical Kurrasa and screen-ID crosswalk? | modules/UI/offline policy; D-BLK-006 | only version-bound sealed evidence | exact Ticketing/UI/offline contracts cannot be fixed | controlled authority/crosswalk | BLOCKS affected implementation scope |
| C2-BLK-008 | Who owns and what is the disposition of local/unmerged assets? | preservation/tree migration; D-BLK-007 | semantic/ownership review incomplete | no cleanup/merge/tree move | hash/preserve/semantic review; owner disposition | BLOCKS destructive cleanup |
| C2-BLK-009 | What exists/passed on latest PR69 head? | security/offline/mobile; D-BLK-008 | moving unmerged line not inspected | no target code adoption | isolated exact-SHA review if selected | DOES NOT BLOCK C2 seal |
| C2-BLK-010 | Are external Codex/developer workspaces/binaries absent? | preservation/runtime; D-BLK-010 | cross-machine inventory unavailable | project extraction/cleanup completeness unknown | controlled global workspace inventory | UNKNOWN / blocks destructive action |
| C2-BLK-011 | Is TEAM-B assurance limitation adequately mitigated? | BLK-B-001 / TB-F-018 | single-session provenance | closure SoD not satisfied by B alone | TEAM-E multidisciplinary review; retain provenance | BLOCKS MISSION-01 assurance if unmitigated |
| C2-BLK-012 | What is the approved tenant hierarchy/cardinality and RLS/equivalent decision? | C2-TARGET-015/016 | requirements/live roles unknown | exact keys/FKs/policies cannot be designed as executable | security/data ADR after authority | BLOCKS tenant DB implementation |
| C2-BLK-013 | What are canonical accounting mappings, period/SoD/reversal/subledger rules? | C2-TARGET-019/020 | approved accounting requirements incomplete in evidence | exact posting schema/use cases cannot be implemented | canonical accounting decision/crosswalk | BLOCKS accounting implementation |
| C2-BLK-014 | Which offline business operations are authorized? | C2-TARGET-017 | current evidence says `OFFLINE_WRITE=0 / Can Queue=NO` | generic business offline writes prohibited | newer explicit authority per operation | BLOCKS offline write enablement |
| C2-BLK-015 | Are module assemblies or logical folders the safe first boundary? | C2-TARGET-001/002 | exact dependency/build/transaction analysis not executed | project count/extraction wave remains candidate | ADR + architecture/build tests at authoritative SHA | DOES NOT BLOCK logical proposal |
| C2-BLK-016 | What is the approved release topology and mobile delivery scope? | hosts/mobile/release | no deployment/artifact/platform authority | packaging/project targets remain proposed | delivery/operations decision and evidence | BLOCKS release implementation |

No entry authorizes TEAM-C2 to choose an answer by inference.
