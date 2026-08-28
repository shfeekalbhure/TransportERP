# CURRENT DIRECTIVE — MISSION-05

`WAIT — PENDING SEALED MISSION-04 INDEPENDENT VERIFICATION`

Current prerequisite:

- MISSION-04 must be sealed and handed off with independent verification results, exact verified SHAs, PASS/FAIL/BLOCKED/UNKNOWN dispositions, reopened findings if any, evidence index, manifest, SHA-256, seal, and Control Tower handoff.

Automatic transition rule:

When Control Tower verifies the sealed MISSION-04 package and the closure prerequisites are satisfied, this file must be changed automatically to:

`START — FINAL CLOSURE AND DELIVERY`

MISSION-05 must create the final crosswalk from finding → remediation plan → execution → independent verification → closure disposition, preserve all open UNKNOWN/BLOCKED items, produce the final delivery/closure package, and raise only the final owner approval/acceptance items that truly require owner authority.

Do not claim `IN PROGRESS` until worker execution is evidenced. If START is authorized but no closure worker session is active, record `START AUTHORIZED — WAITING FOR WORKER SESSION`.
