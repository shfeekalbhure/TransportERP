# MASTER/GATE v2.0 Evidence Index

| ID | Evidence | Exact ref / source | Result | Limitation |
|---|---|---|---|---|
| `M2-EV-001` | Owner authoritative-line decision | governance decision file | `master@2ec6cccf...` approved | no implementation authority |
| `M2-EV-002` | Git commit/tree verification | `2ec6cccf...` / tree `516247dd...` | object exists; remote master equals exact SHA | remote state may later move; this review is frozen |
| `M2-EV-003` | Product-delta comparison | authoritative SHA vs governance HEAD excluding `CONTROL_TOWER/` | no product delta | governance files are not product truth |
| `M2-EV-004` | Exact tree inventory | `git ls-tree -r` on authoritative SHA | 378 files; 10 projects; 1 solution; 10 migrations; 22 test C#; 7 workflows | counts are tracked-tree counts, not runtime coverage |
| `M2-EV-005` | P0 direct source | `ConcurrencySafeWaybillRepository.cs:76-87,119-137` plus registration and Volume surfaces | `A-ARCH-002` reconfirmed CURRENT | runtime affected-row population unknown |
| `M2-EV-006` | TEAM-D v1.1 Crosswalk | sealed 64-row package | complete revalidation population | predecessor report alone not used as fact |
| `M2-EV-007` | TEAM-E v1.1 P0/P1 review | sealed advisory package | 39/39 P0/P1 and 8/8 P2/P3 population represented | advisory, not implementation proof |
| `M2-EV-008` | Predecessor integrity | checksum reruns for B, C1v1.1, Dv1.1, C2v1.1, Ev1.1, Master v1.0; A manifest/sidecar previously centrally verified | accepted bytes unchanged | A uses manifest + sidecar rather than one detached list |
| `M2-EV-009` | Master exact-SHA CI evidence | TEAM-B evidence `GitHub Actions run 32867082533` | Core+PostgreSQL+HTTP and Desktop contract succeeded; zero artifact | not full release/client matrix |
| `M2-EV-010` | Local tool check | current Control Tower runtime | `dotnet` absent | no local restore/build/test execution |
| `M2-EV-011` | PR #69 metadata | GitHub PR #69 exact head `601f2d1c...` | open, draft, unmerged; 214 commits; 206 files | PR descriptions are claims unless corroborated |
| `M2-EV-012` | PR #69 Git comparison | exact Git objects, trees and diff | 53,011 additions / 858 deletions; 13 projects; 20 migrations; 63 test C# | candidate only |
| `M2-EV-013` | PR #69 exact-head workflows | GitHub runs `33146859943`, `33146860001`, `33146859981` | CI/foundation/contract success; conditional workflows skipped | does not transfer PASS to master |
| `M2-EV-014` | PR #69 P0 mapper comparison | same `ConcurrencySafeWaybillRepository.ToItemEntity` at both SHAs | Volume omission remains | candidate does not close `A-ARCH-002` |
| `M2-EV-015` | Direct P1 surfaces | exact authoritative source and sealed direct-evidence locations | all P1 groups remain evidence-bounded | live DB/Production/IdP/Kurrasa access unavailable |
| `M2-EV-016` | Gate command | MISSION-01 command §§34–43 | all mandatory conditions evaluated | READY means planning only |
| `M2-EV-017` | MISSION-02 order | `MISSION-02_REMEDIATION_PLANNING/START_ORDER.md` | planning must reverify each finding; no Source/DB changes | plan remains open, not sealed |

Any claim outside these bounds is `UNKNOWN — REQUIRES VERIFICATION`.
