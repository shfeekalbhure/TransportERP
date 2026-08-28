# TEAM-D Source and Candidate-Line Register

- Inspection UTC: `2026-08-28T01:55:21Z`
- Inspection Asia/Aden: `2026-08-28T04:55:21+03:00`
- Audit subject: `TransportERP — MISSION-01 evidence reconciliation`
- Governing determination: `AUTHORITATIVE CURRENT LINE: UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

TEAM-D did not promote a default branch, current worktree, PR head, or branch name to authority. Commit time, observation time, and governing authority are separate facts.

## Candidate and evidence lines

| Line / ref | SHA observed | Observation | TEAM-D classification | Authority determination |
|---|---|---|---|---|
| GitHub symbolic default `HEAD -> master` | `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | Direct `git ls-remote --symref` at TEAM-D inspection | `CURRENT CANDIDATE` / default remote snapshot | Not sufficient to prove product authority |
| `refs/heads/master` | `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | Direct remote read; source tree used by A/B/C1 through governance anchor | `CURRENT CANDIDATE` | Not selected as authoritative |
| sealed audit anchor `governance/control-tower-20260828` | `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` | A/B/C1 sealed baseline; product delta from master contained Control Tower files only | `AUDIT/GOVERNANCE ANCHOR` | Not product authority |
| remote governance branch at task start | `4a765cb4359cdd8cf522aed49bcaf2bce5f2796b` | Parent assignment snapshot | `GOVERNANCE HISTORY` | Not product authority |
| remote governance branch at TEAM-D recheck | `59b3812b390be7a11017aca521ec4054c81a151e` | Direct `ls-remote`; moved after task start | `GOVERNANCE CURRENT SNAPSHOT` | Not product authority; no product conclusion inferred |
| local TEAM-D worktree HEAD | `1e3f5ec0b83145ca923d1ccc366b5d77535f6e7c` | Local detached governance commit; product delta from sealed audit anchor is zero | `LOCAL GOVERNANCE SNAPSHOT` | Not product authority |
| PR #69 sealed-audit snapshot | `939f49fa9c2ae57fa532ad55f67461c5f3f256f3` | A/B/C1 evidence; OPEN/DRAFT/UNMERGED then | `UNMERGED` snapshot | Not authority |
| PR #69 direct remote recheck | `9c9cfdb753783772f4c6488f655f42dddd4f63f0` | Direct `ls-remote` for branch and `refs/pull/69/head`; moved after sealed audit | `UNMERGED` moving line; contents not fetched or inspected by TEAM-D | Not authority |
| WAVE-1 / PR #58 | `e3a2fe2ebefe478191446407153f099b36d9e2ca` | Sealed baseline/local ref evidence | `UNMERGED` | Not authority |
| W0 / PR #63 | `31ed28b2b4d314fa1c9665fc1e5b5e6f397f221a` | Direct remote recheck | `UNMERGED` | Not authority |
| P2-D / PR #49 | `05ea90b6eb2fb8edc8764d4bddacf2cc132051d8` | Sealed audit evidence | `UNMERGED` | Not authority |
| local preservation head | `3bc7f431964b5d068ae2bab4205aa0c949fc0343` | Git object/ref and sealed A preservation evidence | `LOCAL-ONLY — PRESERVE` | Not authority |
| local preservation object | `7df4743ee3d13540ea82c4505e8e657e6abb6e65` | Git object and sealed A preservation evidence | `LOCAL-ONLY — PRESERVE` | Not authority |
| dirty-worktree evidence head | `06146e0f3ad6249e69d13239bbaf1c9d9ed472ea` | Sealed A preservation evidence; associated PNG hash recorded there | `LOCAL-ONLY — PRESERVE` | Not authority |

## Temporal reconciliation

- The product source tree at sealed governance anchor `8a36f88b...` is byte-equivalent, outside `CONTROL_TOWER/`, to `master@2ec6cccf...`.
- A/B/C1 source findings are therefore valid for that *assessed snapshot*. Their original `CURRENT` labels are preserved in the crosswalk but TEAM-D reclassifies final authority as `UNKNOWN` until a governing product ref/SHA is proven.
- PR #69 is a moving unmerged line. Results attached to `939f49fa...` are not transferred to `9c9cfdb...`; the latter was observed remotely but not fetched or inspected.
- Local-only heads are preservation evidence, not merge recommendations and not product authority.

## Recommendation without authority promotion

Retain `master@2ec6cccf...` as the default-branch/current-candidate snapshot, preserve each unmerged/local line with its own SHA and temporal class, and require a repository/owner authority record naming one product ref and full SHA before MASTER/GATE makes a final `CURRENT STATE` or readiness determination.
