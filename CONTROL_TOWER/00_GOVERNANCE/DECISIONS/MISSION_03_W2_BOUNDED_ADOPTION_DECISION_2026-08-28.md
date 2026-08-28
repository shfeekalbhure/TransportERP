# MISSION-03 W2 BOUNDED ADOPTION DECISION — 2026-08-28

- Decision time UTC: `2026-08-28T16:11:03Z`
- Decision time Asia/Aden: `2026-08-28T19:11:03+03:00`
- Supersedes for current operation: `MISSION_03_W2_REVALIDATION_DECISION_2026-08-28.md` and its status/directive chain through governance `137eeeeaefbdc18d1e94e8778b2e5535387a2a4b`
- Prior decision preserved: `YES — NOT REWRITTEN OR DELETED`
- Authoritative product: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- W1 accepted predecessor: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Adopted bounded execution baseline: `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`
- Decision: `CONTINUE — W2 VERIFIED CANDIDATE ADOPTED FOR BOUNDED EXECUTION`

## Why the prior hold decision is superseded

The prior decision correctly preserved the candidate, DB-GOV boundary, exact-head evidence, and the unresolved live database/Production surfaces. It treated live-role/cardinality and `User.BranchId = null` uncertainty as a global blocker to every code-only package. The current owner/Control Tower instruction explicitly requires those concerns to be separated: live DB uncertainty blocks DBP-002/003 material work, not an independently proven authority-neutral code control.

Source revalidation also establishes a narrower transitional invariant than the prior decision recorded:

1. a claimed Company/Branch never authorizes a Product or Sync action alone;
2. an active stored User must match the Company, and a non-null stored Branch must match the requested Branch;
3. an active stored Company and a Branch owned by that Company must exist;
4. the permission metadata `ScopeType` must be known;
5. an applicable persistent grant or explicit override must match that scope shape;
6. for a `BRANCH` permission, the persistent grant must carry the exact Company and Branch and the resolver verifies Branch→Company consistency;
7. explicit persistent deny wins;
8. malformed branch-without-company scope rows fail closed.

Therefore a null stored User Branch does not, by itself, operate as claim-authoritative access to every branch. It represents current company-level user affiliation, while the existing persistent RBAC scope must independently authorize the exact requested scope. This is a bounded transitional control over the existing schema, not the target membership model and not proof that live rows are clean.

The target explicit membership design, live null/mismatch counts, tenant-consistent physical keys/FKs/checks/indexes/RLS-equivalent controls, and safe backfill remain DBP-002 work. No material database action is released.

## Decisions

- `DEP-005 = CONTROL TOWER REVALIDATED` for current-source execution design and code-only server scope controls; live rows/roles/RLS remain DBP-002-only blockers.
- `DEP-006 = CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION`; `AUTH-001` remains a bounded owner decision for Production issuer/session authority.
- `DEP-007 = CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION`; registry/PoP/nonce/replay/session-device persistence remains DBP-003/006 work.
- W2-A1/A2/B1/B2A/C1/F1: `ADOPT — REBOUND TO SEALED PLAN`.
- W2-B2B/C2/D/E/F2: remain individually blocked.

The package-by-package evidence and rollback disposition is in `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/W2_CONTROL_TOWER_REVALIDATION_DECISION.md`.

## Non-authorizations

This decision does not authorize master merge, PR #69 adoption, rebase, cherry-pick, force-push, history rewrite, Production access/configuration, data repair, Entity/DbContext/Migration/schema/seed change, DBP-002/003/006 execution, complete W2 exit, MISSION-03 seal, or MISSION-04 start.

MISSION-03 continues from `9c5b7a1...` only into independently satisfied packages. MISSION-04 remains `WAIT`.
