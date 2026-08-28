# TEAM-A — Local Asset Preservation Appendix

Snapshot time: `2026-08-28T00:45Z / 2026-08-28T03:45+03:00`. Comparison base: unmerged PR #69 snapshot `939f49fa9c2ae57fa532ad55f67461c5f3f256f3`. Comparison used read-only Git alternate-object access and `--cherry-pick --right-only --no-merges`; patch equivalence is not proof of semantic correctness.

## A-PRES-014 — local head `3bc7f431964b5d068ae2bab4205aa0c949fc0343`

Path: `/workspace/scratch/143c66febc8c/TransportERP`

Patch-unique commit manifest:

| Commit | Subject |
|---|---|
| `3bc7f431964b5d068ae2bab4205aa0c949fc0343` | fix(sync): preserve retention anchors during legal hold |
| `e1b93bd4f4ebdd5d3c59438d63c94ea7b81e924b` | fix(sync): exclude retryable failures from retention |
| `fbb3b0fc86ed9911efdcbd08982286617687cb9d` | fix(database): preserve legal hold lock ordering |
| `7c48d57a769b2ebfbaaa88f105c36f9522b0e749` | fix(database): prevent stale conflict parent legal holds |
| `6ae03cd4b0bec62d07c0fdcd97fc7a23caf5e930` | fix(finance): serialize actor scope validation |
| `f0880d6b3cee6289c9e5d1e8db679496e3d17286` | fix(sync): reject holds over redacted conflict evidence |
| `c590698efd8fbaf60c5530ff6c57ca3e6d2fa025` | fix(finance): enforce tenant-safe hierarchy references |
| `8e05e58eaaa85467321961e0671500aa426a95f4` | fix(sync): keep legal holds independently scoped |
| `94b60021d5c653838a5830ad4a734555bf7ba084` | feat(sync): add audited legal hold workflow |
| `9605f30e3141ac9bd059836eb72f832ad6ff9b0f` | fix(database): order finance hardening after legal-hold guard |
| `7c2230118d5d646a5cd8f06d61c1d4d0a8ffd833` | fix(finance): enforce tenant-safe accounting references |
| `2f4d8d1b0a850119e8a4a69ca88fb546a37ae84b` | test(android): align succeeded result evidence |
| `e69585476e9815c1d845a00dac720984d3038463` | Fix conflict reapply payload identity |

Required action: `KEEP UNTIL RECONCILED`; do not infer merge approval.

## A-PRES-015 — local head `7df4743ee3d13540ea82c4505e8e657e6abb6e65`

Path: `/workspace/scratch/263a0f4a787d/TransportERP`

Patch-unique commit manifest:

| Commit | Subject |
|---|---|
| `290ca5a090c260a830ab9679946227f41e0512c5` | test(sync): close Stage 4 proof evidence gaps |
| `1e622302c7ec21174ea1871a89d6ef456a9a243e` | docs(governance): record Stage 4 runtime CI pending |
| `840d15ba543c177b403c0f8596e4262d7d8fcf92` | feat(sync): complete Stage 4 proof runtime candidate |

Required action: `KEEP UNTIL RECONCILED`; do not infer merge approval.

## A-PRES-016 — dirty tracked image

- Repository: `/workspace/scratch/4c170dbb8858/TransportERP`
- Head: `06146e0f3ad6249e69d13239bbaf1c9d9ed472ea`
- Path: `documentation/closeout/P1/P1_RTL_SCREENS/W3-P1-003_RolesPermissions.png`
- Working-copy size: `1,687,552 bytes`
- Working-copy SHA-256: `e0631df6b985f6a2f68538d6ee52b4783376ca3f8e3887c9bd97e5b9279fda36`
- Status: tracked and modified at snapshot.

Required action: `PRESERVE`; owner must compare visual/source provenance before restore, replacement or deletion.
