# CONTROL TOWER AUTONOMOUS SUPERVISION PROTOCOL

## Purpose

This protocol keeps GROUP-01 moving through file-based governance without requiring the owner to relay reports or ordinary transition messages between sessions.

## Operating model

- `CONTROL_TOWER/` is the authoritative operational record.
- Teams read their current directive from `CONTROL_TOWER_TEAM_DIRECTIVES.md` before starting or resuming work.
- Teams write only inside their official mission/team directory.
- Control Tower verifies seals, manifests, hashes, handoffs, evidence sufficiency, and prerequisites before changing a directive.
- Ordinary analytical/review/planning disagreements are resolved by the next designated review layer, not by interrupting the owner.

## Owner-decision deferral rule

During MISSION-01 and MISSION-02, unresolved analytical findings, disagreement over priority, disagreement over P0/P1 classification, an unresolved authoritative-current-line question, or conflicting team conclusions do **not** by themselves stop the review chain. They are routed to the designated reconciliation/advisory stage and carried into the final decision backlog.

The owner is interrupted before final GROUP-01 delivery only when an **actual action** is about to occur that would require owner authority, including:

- destructive Production or database action;
- deletion or irreversible loss of data or valuable work;
- force-push or destructive Git history rewrite;
- merge/delete of preserved branches/worktrees/stashes where loss is possible;
- use of Production credentials or real Production data beyond approved read-only evidence gathering;
- any action explicitly reserved to owner authority by a governing decision.

A discovered P0 remains a governing blocker for release/execution as applicable, but it does not stop TEAM-D/TEAM-C2/TEAM-E from completing analysis, reconciliation, design proposal, and advisory work.

## Authoritative current line rule

If the authoritative product line is unresolved, TEAM-D must reconcile the candidate refs/SHAs and evidence, classify each line (`CURRENT CANDIDATE / UNMERGED / HISTORICAL / LOCAL-ONLY / UNKNOWN`), and recommend the governing candidate. TEAM-C2 and TEAM-E may continue using the reconciled scope with explicit classification. The MASTER/GATE may not claim a final CURRENT STATE or release readiness until the authority question is resolved or explicitly recorded as a final owner-decision item.

## Monitoring cadence

While a Control Tower session is actively running, it should re-check material team/mission files approximately every 10 minutes and immediately after a known handoff. It must not pretend to have monitored during periods when no active session was running.

The external scheduled monitor, when enabled, is a separate hourly condition watch. It supplements but does not replace active-session checks.

Checks update records only when there is a material state change, new blocker, new seal/handoff, reopening, or governing decision. No cosmetic timestamp churn is required.

## Automatic sequence

1. TEAM-A + TEAM-B + TEAM-C1
2. TEAM-D
3. TEAM-C2
4. TEAM-E
5. MASTER REPORT + RECONCILIATION GATE
6. MISSION-02 if gate permits planning
7. MISSION-03 according to the sealed remediation plan and execution authority
8. MISSION-04
9. MISSION-05

A team whose package is sealed is `SEALED — STOP`. A later correction requires `REOPEN`, a new version, a new hash, and a new seal.

## No-guessing rule

`REPORT SAYS SO != FACT`.

Receiving teams must re-check governing evidence within their scope. Unsupported claims remain `UNKNOWN — REQUIRES VERIFICATION`; inaccessible sources remain `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.

## Database governance

`DB-GOV-001` remains binding. This protocol does not authorize database modification. Database changes require the appropriate execution-phase authority, recorded impact analysis, preservation requirements, testing, and recovery path.
