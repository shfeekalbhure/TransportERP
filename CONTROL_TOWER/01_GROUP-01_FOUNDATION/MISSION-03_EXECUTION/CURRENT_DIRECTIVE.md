# CURRENT DIRECTIVE — MISSION-03

`WAIT — PENDING SEALED MISSION-02 REMEDIATION PLAN`

Current prerequisite:

- MISSION-02 must be `SEALED — READY FOR MISSION-03` with verified report, finding-to-remediation crosswalk, execution waves, dependencies, preservation requirements, DB-GOV plan, tests/acceptance, rollback/recovery, manifest, SHA-256, seal, and Control Tower handoff.

Automatic transition rule:

When Control Tower verifies the sealed MISSION-02 package, this file must be changed automatically to:

`START — EXECUTION UNDER SEALED MISSION-02 PLAN`

At START, execute only the exact planned/authorized scope. Reverify each item before change. `DB-GOV-001` remains binding. Preserve exact SHAs and evidence. Any destructive/Production/irreversible action or risk of losing preserved work requires the applicable HOLD/owner authority.

Do not claim `IN PROGRESS` until worker execution is evidenced. If START is authorized but no worker session is active, record `START AUTHORIZED — WAITING FOR WORKER SESSION` in Control Tower status.
