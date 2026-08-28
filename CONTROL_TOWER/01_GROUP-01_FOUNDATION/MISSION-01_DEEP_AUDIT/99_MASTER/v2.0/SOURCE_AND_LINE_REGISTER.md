# MASTER/GATE v2.0 Source and Line Register

| Source / line | Full SHA / version | Access | Classification | Decision |
|---|---|---|---|---|
| `refs/heads/master` | `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`; tree `516247dd...` | direct Git object + remote ref | `AUTHORITATIVE CURRENT` | governs current findings/counts |
| `governance/control-tower-20260828` | governance HEAD at revalidation | direct | `GOVERNANCE ONLY` | no product delta outside Control Tower |
| PR #69 branch | `601f2d1cad61d62e590a6714ad84e307eb84fe5f`; tree `bfbcd140...` | direct Git + GitHub metadata/workflows | `UNMERGED REMEDIATION / FINAL CANDIDATE` | compare only; no merge/current inference |
| MASTER/GATE v1.0 | sealed hashes in parent directory | direct | `HISTORICAL SEALED` | preserved, superseded for gate decision |
| A/B/C1v1.1/Dv1.1/C2v1.1/Ev1.1 | accepted sealed package versions | direct | `SEALED PREDECESSOR EVIDENCE` | inputs independently rechecked |
| live DB/Production/IdP | not supplied | inaccessible | `ACCESS BLOCKED — UNKNOWN` | later evidence gate |
| external workspaces/latest Kurrasa | partial predecessor evidence only | not exhaustive | `UNKNOWN / VERSION-BOUND` | preserve and reconcile in plan |

Observation time is not authority; the owner decision supplies authority. PR state and CI never change the CURRENT classification without a separate authorized transition.
