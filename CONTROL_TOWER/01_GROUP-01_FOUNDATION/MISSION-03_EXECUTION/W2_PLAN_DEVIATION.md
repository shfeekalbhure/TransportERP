# W2 Plan Deviation and Revalidation Hold

- Detected: `2026-08-28T15:34Z`
- Previous governance base used by the worker: `b3c57873c609e6209dcebcb0de6751ce8963c39a`
- Superseding Control Tower head: `c274f9ab66a507e59eaf31cd850d88d9e1ff17d2`
- Superseding directive: `CONTINUE — PRESERVE VERIFIED W1; W2 HOLD — STOP/REPLAN REQUIRED`
- Preserved candidate execution head: `9c5b7a12e59d2c42e682717b8e90c491f8699b96`
- State: `REVALIDATED — BOUNDED CANDIDATE ADOPTED; HISTORICAL DEVIATION RETAINED`

## Timeline and scope

The worker began from the then-fetched governance checkpoint `b3c5787...` and the user's explicit instruction to resolve DEP-005/006/007 and execute each independently ready code-only package. It prepared ADR-W2-001/002/003, split W2, and advanced the isolated execution candidate through `a157c34...`, `d1c0a257...`, `d740740...` and `9c5b7a1...` without any DB/schema/data/Production action.

While preparing the Control Tower checkpoint, a fresh fetch showed that governance had independently advanced to `c274f9a...`. Its newer `CURRENT_DIRECTIVE.md` explicitly holds further W2 Product modification and requires Control Tower verification/rebinding. The worker had not observed that superseding ref before the fetch.

## Immediate containment

- No further Product change is permitted or attempted after detecting `c274f9a...`.
- All post-W1 candidate commits are preserved. No merge, delete, reset, force push or history rewrite is performed.
- PR #69 remains unmerged evidence only.
- No Entity, DbContext, Migration, Seed, schema, data or Production change occurred.
- Technical test evidence remains factual, but it does not grant execution authority or Control Tower acceptance.

## Candidate package for revalidation

| Item | Worker evidence | Governance disposition now required |
|---|---|---|
| DEP-005 | ADR-W2-001 plus current model/migration/PR69 comparison | Control Tower independently verify and rebind or reject |
| DEP-006 | ADR-W2-002 plus persistent-RBAC/API/Sync evidence | Control Tower independently verify; decide bounded AUTH-001 |
| DEP-007 | ADR-W2-003 plus lifecycle-owner negative tests | Control Tower independently verify; keep registry/PoP behind DB-GOV |
| W2-A1/A2/B1/B2A/C1/F1 | candidate commits through `9c5b7a1...`; exact run `33185419917` PASS | historical hold disposition; superseded by Control Tower `ADOPT — REBOUND TO SEALED PLAN` decision below |
| DBP-002/003 | no mutation performed | remain blocked under DB-GOV-001 |

## Required Control Tower decision

1. Verify ADR-W2-001/002/003 against the sealed plan and the superseding directive.
2. Rebind, revise or reject each candidate package individually.
3. State whether `9c5b7a1...` may become the execution baseline, must be selectively reimplemented, or must remain evidence only.
4. Preserve the failed run `33184771338` and successful exact-head run `33185419917` as evidence regardless of adoption.

## Control Tower closure of the deviation gate

Control Tower completed the required independent review in `W2_CONTROL_TOWER_REVALIDATION_DECISION.md`:

- DEP-005/006/007 were revalidated for their bounded code-only scopes;
- W2-A1/A2/B1/B2A/C1/F1 were each `ADOPT — REBOUND TO SEALED PLAN`;
- the W2-wide hold was lifted only for those packages;
- `9c5b7a1...` became the bounded execution baseline;
- W2-B2B/C2/D/E/F2 and DBP-002/003/006 remain separately blocked.

MISSION-03 remains open. This historical deviation record is not a MISSION-04 handoff.
