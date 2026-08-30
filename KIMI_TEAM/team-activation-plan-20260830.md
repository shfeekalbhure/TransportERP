# KIMI Team Activation Plan — TransportERP

**Activation Date:** 2026-08-30  
**Branch:** `kimi/team-transport-20260829`  
**Authority Source:** `AGENTS.md`, `KIMI_TEAM/README.md`, `origin/governance/control-tower-20260828`  
**Coordinator:** KIMI-00  
**Status:** ACTIVATED — PREPARATION PHASE

---

## 1. Purpose

Activate a multidisciplinary Kimi engineering team to prepare, plan, and execute authorized work on TransportERP within the existing governance boundaries.

Hard boundaries (non-negotiable):
- Work only on `kimi/*` branches.
- Never push directly to `master`.
- Never merge a pull request.
- Never force-push or rewrite shared history.
- Never delete governance evidence, audit reports, or owner decisions.
- Preserve traceability for every material change.

---

## 2. Team Roster

| Role | Alias | Responsibility | Current Assignment |
|---|---|---|---|
| KIMI-00 | Coordinator / Task Router | Scope enforcement, delegation, handoff evidence collection | Overall activation and scope control |
| KIMI-01 | Repository Explorer / Evidence Collector | Read repo and governance; no implementation by default | Map current CONTROL TOWER state and DBP-002 evidence |
| KIMI-02 | Architecture and Planning | Produce implementation plans and architecture checks | Prepare authorized remediation planning |
| KIMI-03 | Implementation | Implement approved code changes | On standby until owner-authority clarified |
| KIMI-04 | Build / Tests / Migrations / CI | Run restore/build/tests/migrations/CI checks | Validate DBP-002 frozen checkpoint locally |
| KIMI-05 | Independent Reviewer | Review KIMI-03 output; cannot self-approve | Reserved for DBP-002 independent review if authorized |
| KIMI-06 | Governance and Handoff Evidence | Traceability, final task handoff | Maintain evidence register and delivery packages |

---

## 3. Authorized Scope (as of 2026-08-30)

Derived from `CONTROL_TOWER/04_CONTROL_TOWER_OPERATIONS/CONTROL_TOWER_LIVE_STATUS.md` and `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/CURRENT_DIRECTIVE.md`.

### 3.1 Permitted Preparation Work
1. **DBP-002 Independent Review Preparation**
   - Verify frozen checkpoint `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`.
   - Collect immutable evidence: CI runs `33222541097`, `33222541108`, `33222541109`.
   - Explicitly disposition legacy v2 run `33222541073 = FAIL`.
   - Prepare review package: report + evidence + manifest + SHA-256.

2. **P2-C01-D Remediation Assessment**
   - Review branch `origin/feature/p2-c01-d-arrival-transit-warehouse-20260822` at `05ea90b`.
   - Document required changes without modifying code unless explicitly authorized.

3. **Evidence Preservation and Traceability**
   - Maintain this activation plan, evidence register, and handoff records.
   - Cross-check external library claims against actual Git SHAs.

### 3.2 Prohibited Work
- Any DBP-004 product-source, test, entity, DbContext, migration, schema, seed, or configuration change.
- Any merge to `master` or governance branches.
- Any commit/push/merge on PR #69.
- Starting MISSION-04 or MISSION-05.
- Modifying the ten existing migrations.
- Treating the external ChatGPT library as governing authority.

---

## 4. Task Queue

| Order | Task | Owner | Prerequisite | Output | State |
|---:|---|---|---|---|---|
| 1 | Verify CONTROL TOWER current directive and frozen checkpoints | KIMI-01 | None | Evidence summary | READY |
| 2 | Validate DBP-002 frozen checkpoint builds and tests locally | KIMI-04 | Task 1 | Local build/test report | READY |
| 3 | Prepare DBP-002 independent review package | KIMI-05 + KIMI-06 | Task 2 | Review report + manifest + SHA-256 | WAITING OWNER AUTHORITY |
| 4 | Assess P2-C01-D remediation scope | KIMI-01 + KIMI-02 | None | Remediation assessment document | READY |
| 5 | Implement authorized P2-C01-D corrections | KIMI-03 + KIMI-04 | Owner authorization + KIMI-05 review | Commits on feature branch | NOT STARTED |
| 6 | Compile handoff evidence | KIMI-06 | Task completion | Delivery evidence package | NOT STARTED |

---

## 5. Evidence Register

| SHA / Reference | Description | Authority Class |
|---|---|---|
| `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | Authoritative product line | GOVERNING |
| `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce` | DBP-002 frozen review target | FROZEN CANDIDATE |
| `c3f2b7b4e8e32dd22920d08ce33870f51ece96f0` | DBP-004 unauthorized head | UNACCEPTED CANDIDATE — HOLD |
| `origin/governance/control-tower-20260828` | Current Control Tower state | GOVERNING |
| `KIMI_TEAM/reports/audit-report-20260830.md` | Branch audit report | REFERENCE |
| External `TransportERP_ChatGPT_Library.zip` | ChatGPT discussion archive | NOT GOVERNING — REFERENCE ONLY |

---

## 6. Current Blockers

| ID | Blocker | Impact | Next Action |
|---|---|---|---|
| B1 | DBP-002 independent acceptance missing | MISSION-03 cannot seal; DBP-004 cannot release | KIMI-05 review package preparation |
| B2 | DBP-004 unauthorized early execution | Current head `c3f2b7b4...` contaminated and CI-red | Preserve commits; no further product modification |
| B3 | PR #69 unmerged | No final remediation candidate merged | Hold until MISSION-03 sealed |
| B4 | P2-C01-D needs changes | Branch not ready for merge | Assessment then owner-authorized remediation |

---

## 7. Return / Handoff Rules

- Return to owner only when:
  1. A task is complete with full evidence, OR
  2. A new owner-reserved decision is required, OR
  3. A true external blocker is encountered after exhausting internally permitted work.

- Every completed delivery must report:
  1. Task objective.
  2. Branch name.
  3. Commit SHA(s).
  4. Files changed.
  5. Commands/tests executed and results.
  6. Known blockers or unresolved risks.
  7. Pull request number/link when a PR is opened.

---

## 8. Conclusion

The Kimi multidisciplinary team is activated for **preparation and authorized review work only**. No unauthorized implementation will begin. The immediate focus is verifying the DBP-002 frozen checkpoint and assessing P2-C01-D remediation scope, while strictly preserving DBP-004 HOLD status and all governance boundaries.
