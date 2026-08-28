# TEAM-D v1.1 Source and Candidate-Line Register

- Fresh remote observation: `2026-08-28T02:33:16Z` / `2026-08-28T05:33:16+03:00`
- Governing result: `AUTHORITATIVE CURRENT LINE: UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

| Ref/asset | Full SHA | Temporal classification | Evidence-bounded interpretation |
|---|---|---|---|
| symbolic remote HEAD / `master` | `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | CURRENT CANDIDATE | default remote snapshot only; not authority |
| sealed A/B/C1 audit anchor | `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` | AUDIT/GOVERNANCE ANCHOR | product tree equals master outside Control Tower; not product authority |
| remote governance at v1.1 recheck | `9b3db9c4350877df6bbf6c6603da83e3afee6545` | GOVERNANCE SNAPSHOT | moved during workflow; not product authority |
| PR69 branch and pull head at v1.1 recheck | `46a87a002b5b4b8bc456007716a0a75a6a3a7500` | UNMERGED | moving line; content not fetched/inspected; prior SHA evidence cannot transfer |
| WAVE-1 | `e3a2fe2ebefe478191446407153f099b36d9e2ca` | UNMERGED | separate candidate; not authority |
| W0 | `31ed28b2b4d314fa1c9665fc1e5b5e6f397f221a` | UNMERGED | separate candidate; not authority |
| P2-D | `05ea90b6eb2fb8edc8764d4bddacf2cc132051d8` | UNMERGED | sealed-audit candidate; not authority |
| local preservation head | `3bc7f431964b5d068ae2bab4205aa0c949fc0343` | LOCAL-ONLY | preserve; no merge conclusion |
| local preservation object | `7df4743ee3d13540ea82c4505e8e657e6abb6e65` | LOCAL-ONLY | preserve; no merge conclusion |
| dirty-artifact evidence head | `06146e0f3ad6249e69d13239bbaf1c9d9ed472ea` | LOCAL-ONLY | preserve/hash; no merge conclusion |

The remote PR69 and governance heads moved again between v1.0 and v1.1. This strengthens—not resolves—the rule that observation, commit time, and authority are distinct. No result is transferred to `46a87a...` without exact-SHA inspection.
