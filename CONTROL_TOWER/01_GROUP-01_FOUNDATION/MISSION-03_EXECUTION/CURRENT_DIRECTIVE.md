# CURRENT DIRECTIVE — MISSION-03

`START — EXECUTION UNDER SEALED MISSION-02 PLAN`

Start basis:

- MISSION-02 package: `MISSION-02-v1.2`
- MISSION-02 state: `SEALED — DELIVERED TO CONTROL TOWER — READY FOR MISSION-03`
- Remote governance delivery: `governance/control-tower-20260828@85fb92b664a70fab497b60962bf34753a66f7dce`
- Governing product baseline: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- PR #69: `UNMERGED REMEDIATION / FINAL CANDIDATE — EVIDENCE ONLY`

Read `MISSION_CHARTER.md`, `START_ORDER.md`, the complete sealed MISSION-02 package, and all referenced prerequisite/acceptance registers before changing product files.

Execution order begins with `W0 — Preservation and Exact-Baseline Evidence`. No product modification is permitted before each work package's own preservation, authority, baseline, dependency, test, rollback/recovery and DB-GOV entry gates are satisfied.

Execute only the exact scope authorized by the sealed plan. Reverify facts on the then-current exact SHA before action. `DB-GOV-001` remains binding for every Database/Schema/Entity/Migration/field/relationship change.

Any destructive, Production, irreversible, high-risk Git action, or risk of losing preserved work remains subject to the applicable HOLD/owner authority. Do not merge PR #69 by implication from this directive.

Do not claim `IN PROGRESS` until worker execution is evidenced by mission-local outputs or implementation evidence. If no worker session is active, governance state is `START AUTHORIZED — WAITING FOR WORKER SESSION`.
