# WP-G2-W1-Evidence-Completion — TransportERP

## Identity
- **Work Package ID:** WP-G2-W1-Evidence-Completion
- **Title:** Immutable approved-W1 evidence and silent-change verification
- **Requested by:** GENERAL_SUPERVISOR
- **General Supervisor:** GENERAL_SUPERVISOR
- **Target Gate:** G2
- **Priority:** High

## Objective
Make the six approved W1 binary references independently readable from a fixed
repository path, record their SHA-256 values, and establish whether the reviewed
branch contains any undocumented W1 data-model change. This package is evidence
work only; it must not amend W1.

## Scope

### In scope
- Fixed repository copies of the six approved W1 source artifacts.
- SHA-256 manifest and an author-side branch review for logical model, entities,
  relationships, ownership, constraints, precision, and UUIDv7 physical mapping.
- Independent SOLUTION_ARCHITECT review followed by an independent QA rehash.

### Out of scope
- Editing any W1 approved artifact, the approved data model, constraints,
  ownership, or OTS decisions.
- Creating DDL, persistence, ORM mappings, migrations, or a physical schema.
- Any G2 Gate decision or final readiness review.

## Governing references
- `AGENTS.md`
- `docs/agents/DATA_MYSQL_ARCHITECT.md`
- `docs/agents/SOLUTION_ARCHITECT.md`
- `docs/agents/QA_TESTING_REVIEWER.md`
- `docs/governance/REVIEW_WORKFLOW.md`
- `docs/governance/evidence/W1-Approved-Baseline-Reference.md`
- Current Approved References V1.17 and its W1 artifacts.

## Assigned roles
- **Primary specialist/author:** DATA_MYSQL_ARCHITECT
- **Independent reviewer 1:** SOLUTION_ARCHITECT
- **Independent reviewer 2:** QA_TESTING_REVIEWER
- **Final approver:** GENERAL_SUPERVISOR

## Dependencies
- Required prior Work Packages: `WP-G2-Evidence-Verification`.
- Required closed gaps: none; this package supplies evidence for `G2C-W1-BASELINE`.
- External dependencies: the six approved W1 artifact bytes identified by the
  Current Approved References register.

## Required outputs
- `docs/governance/evidence/W1-approved-artifacts/` containing the six source
  files unchanged.
- `docs/governance/evidence/W1-Evidence-Completion-Record.md` with hashes,
  comparison scope, reviewer records, and boundary between logical and physical
  verification.

## Acceptance criteria
1. Every artifact’s repo-path SHA-256 equals the approved fingerprint.
2. SOLUTION_ARCHITECT independently reviews the branch for an undocumented W1
   data-model delta.
3. QA_TESTING_REVIEWER independently recalculates all six hashes from the fixed
   repository paths and records an explicit verdict.
4. The record distinguishes the approved logical baseline from physical mapping,
   which remains deferred until persistence begins.

## Constraints
- Do not modify Frozen/Approved W1 decisions without a Change Request.
- Do not infer a physical schema from forms or create one as evidence.
- Do not close `G2C-W1-BASELINE` until both independent reviews are recorded.
- `G2 = NOT READY` remains unchanged.

## Review record

### Author result
- **Status:** `PASS WITH NOTES` pending independent reviews.
- **Findings:** All six copied bytes match their approved SHA-256 fingerprints.
  No EF/MySQL provider, DbContext, migration, DDL, ORM mapping, or competing
  precision/UUID mapping is present in the reviewed implementation snapshot.
- **Files changed/created:** evidence artifacts, completion record, and this work
  package only.
- **Open gaps:** physical mapping must be verified against W1 when persistence is
  introduced; this is not evidence of an existing silent change.

### Independent review
- **Reviewer:** SOLUTION_ARCHITECT — pending assignment/completion.
- **QA rehash:** QA_TESTING_REVIEWER — pending after solution review.

## General Supervisor disposition
- **Final status:** Pending.
- **Readiness for target Gate:** Pending independent reviews; no Gate decision.
- **Next Work Package:** resume `WP-G2-Evidence-Verification` after both reviews.
