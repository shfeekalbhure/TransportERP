# CURRENT DIRECTIVE — MISSION-04

`WAIT — PENDING SEALED MISSION-03 EXECUTION OUTPUTS`

Current prerequisite:

- MISSION-03 must be sealed and handed off with exact implementation SHAs, execution logs/evidence, test results, DB-GOV compliance evidence, preservation and rollback/recovery records, unresolved items, manifest, SHA-256, seal, and Control Tower handoff.

Automatic transition rule:

When Control Tower verifies the sealed MISSION-03 package, this file must be changed automatically to:

`START — INDEPENDENT VERIFICATION`

MISSION-04 must independently reverify the executed scope and exact SHAs. It must not accept MISSION-03 claims without evidence and must classify each item PASS / FAIL / BLOCKED / UNKNOWN. Failed findings return to the execution loop through Control Tower; MISSION-04 does not silently fix them itself.

Independence is mandatory: the MISSION-03 execution worker must not self-certify MISSION-04.

Do not claim `IN PROGRESS` until an independent worker session is evidenced. If START is authorized but no worker session is active, record `START AUTHORIZED — WAITING FOR INDEPENDENT WORKER SESSION`.
