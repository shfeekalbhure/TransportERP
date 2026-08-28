# TEAM-D Audit Output Manifest

- Owner: `TEAM-D — MISSION-01`
- Version: `v1.0`
- State: `SEALED — READY FOR CONTROL TOWER VERIFICATION`
- Closure: `2026-08-28T01:59:56Z` / `2026-08-28T04:59:56+03:00`
- Baseline: governance audit anchor `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`; assessed product tree `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Authoritative current line: `UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`
- Supersedes: none

| Output | Function | State | SHA-256 |
|---|---|---|---|
| `TEAM-D_EVIDENCE_RECONCILIATION_REPORT.md` | main reconciliation report | SEALED | `a4fe28a735635134ef9ccc5df06d351248df88bbe662f1ff363d1b118af90bae` |
| `TEAM-D_FINDING_CROSSWALK.md` | 62-record Finding-by-Finding crosswalk | SEALED | `b7e611f501ed02049633b78c69c91b52e404a08ce3c6a823fc2aa82a7bf2eacf` |
| `TEAM-D_SOURCE_AND_LINE_REGISTER.md` | candidate ref/SHA and authority reconciliation | SEALED | `664ad665c1da740d73b180b35563efb1c0f91bc24da128980eabbccb52294685` |
| `SOURCE_ACCESS_REGISTER.md` | source access and limits | SEALED | `e4a137b029745832f1ae04fdeec4aa76d78dd3dda8ea4ac5aa3607fef6ffa49a` |
| `EVIDENCE_INDEX.md` | direct and predecessor evidence index | SEALED | `699ba4d405cb0ae2c5c33a553c79e5067418939857cbdf095492232d9a1bece4` |
| `FILES_REVIEWED_REGISTER.md` | reviewed files/sources and coverage | SEALED | `cfa7190a609d6357a4c46f48b19ba293153ed0886a4f6107f1b7efc5f5b51a8d` |
| `UNKNOWN_AND_BLOCKERS_REGISTER.md` | unknowns and gate blockers | SEALED | `bfd0e180611b02210e5b745c6c640f6003dbe8427245d04ce49be341f9972aa8` |
| `DOMAIN_COVERAGE_MATRIX.md` | domain reconciliation coverage | SEALED | `ae08f807176baeaddbd28eda96f1f7f7b4a48e298429bd2b97d686beca542d0a` |
| `WORKSPACE_PRESERVATION_REGISTER.md` | protected audit/local/unmerged assets | SEALED | `28d580ae58c0c50a3b6f446b5cdaa3f7af12d8d7c245b93e62f7e2a2fb4c890d` |
| `TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md` | actual TEAM-D reviewer assignments | SEALED | `1b35dd777e9726254996451e6a42e57ed623887db1fef8c7b1a744958e0ec4f7` |
| `AUDIT_REPORT_SEAL_REGISTER.md` | package seal and closure assertions | SEALED | `4840fe0f7e7b4469f03c2ccc78aabd043f5ffdb1cce19192771d54e59bb0bdbf` |
| `CONTROL_TOWER_HANDOFF.md` | controlled handoff and acceptance checks | SEALED | `df93a72b16104ae278311f5ec13d451c19d0f7ec2a2eaffe723633728ec31884` |
| `AUDIT_OUTPUT_MANIFEST.md` | this output manifest | SEALED | recorded in detached `AUDIT_OUTPUT_SHA256.txt` |
| `AUDIT_OUTPUT_SHA256.txt` | detached integrity list for every sealed file except itself | SEALED | self-hash intentionally not embedded |

The detached checksum list is the reproducible integrity authority for this package. Any mismatch requires Control Tower to reject handoff or issue `RETURN FOR REWORK`; no silent edit is permitted.
