# TransportERP — Design Orchestrator Protocol V1

## 1. Operating principle
TransportERP screen design uses one central orchestration path and one repository truth. Logical design teams are workflow roles, not independent authorities and not separate conversation histories.

The repository is the handoff medium. No owner/manual copy-paste between TEAM-D01..TEAM-D06 is required.

## 2. Orchestrator identity
`DESIGN-LEAD / ORCHESTRATOR` coordinates the workflow. It does not replace specialist ownership and does not invent missing facts.

Responsibilities:
- read the live queue;
- verify required inputs exist before assigning/advancing a stage;
- route the screen to the correct logical team;
- ensure each team reads current repository evidence and governing references;
- block work when authority or evidence is missing;
- record handoff/status in the queue;
- release controlled batches only after the pilot passes independent review.

## 3. Mandatory pre-read before every stage
Before a team changes a screen specification it must read, at minimum:
1. `documentation/design/04_SCREEN_WORK_QUEUE.csv` row for the screen;
2. the screen canonical `screen-spec.md`;
3. exact governing kurrasa/current-design references cited by the screen spec;
4. frozen/current CoreUI + ScreenProfile references relevant to the stage;
5. current repository implementation/evidence for the same screen/domain when it exists.

No stage may rely on conversation memory as authority.

## 4. No-guess rule
If an exact required fact is absent, contradictory, stale, or not authority-backed:
- do not infer it;
- set the specific item to `TBD-GATED` where safe, or move the screen to `HOLD_AUTHORITY` when the gap blocks the stage;
- record the blocking issue and required authority/evidence;
- do not create API/DTO/Permission/DDL/Offline-write authority from UI needs.

If a reviewed/current authority explicitly classifies a repository or historical screen ID as non-governing, the orchestrator must not keep designing under that ID. It records that identity as `NON_GOVERNING_LINEAGE`, excludes it from active routing, and continues only under an independently issued canonical identity when one exists. No fuzzy alias equivalence is permitted.

## 5. Single-conversation control plane
The owner may use one central conversation/session for direction and approvals. Specialist roles operate through repository artifacts and queue state. Separate team conversations, if ever used by humans, are advisory only and cannot become a competing source of truth.

## 6. Automatic handoff rule
A team finishes a stage by updating the canonical screen spec and queue evidence. The orchestrator advances the queue only when the stage exit criteria are satisfied.

Canonical progression:
`BACKLOG → ANALYSIS → LAYOUT → FIELD_GRID → UX → VISUAL → INDEPENDENT_REVIEW → DESIGN_APPROVED`

Revision and hold paths remain governed by `02_SCREEN_WORKFLOW_AND_TEAM_HANDOFF_V1.md`.

`NON_GOVERNING_LINEAGE` is a terminal evidence disposition, not a workflow stage. Rows in that disposition are skipped by specialist routing and retained only for traceability.

## 7. Stage ownership
- `ANALYSIS` → TEAM-D01
- `LAYOUT` → TEAM-D02
- `FIELD_GRID` → TEAM-D03
- `UX` → TEAM-D04
- `VISUAL` → TEAM-D05
- `INDEPENDENT_REVIEW` → TEAM-D06
- `DESIGN_APPROVED` closure → DESIGN-LEAD

The next team never needs a copied brief; it reads the same repository state.

## 8. Pilot policy — reconciled to current authority
The original repository pilot was opened under R2/repository ID `SHP-005`. Current 2026-08-23 reviewed kurrasa material classifies that R2 identity as non-governing/ID-conflict material. It is therefore retained only as `NON_GOVERNING_LINEAGE`.

The active design pilot is the independently issued canonical FLOW01 screen:
`FLOW01-W3-SCR-001 — إدخال البوليصة — Transaction / HeaderLines`.

The rebind does **not** assert that `SHP-005 == FLOW01-W3-SCR-001`; it replaces an ineligible pilot identity with a separately authoritative canonical identity. Repository implementation under SHP-005 remains reconciliation evidence only.

No broad batch release occurs until `FLOW01-W3-SCR-001` reaches independent-review PASS and the Design Lead confirms the workflow itself did not create duplication, authority leakage, or ambiguous ownership.

## 9. Authority boundary
This protocol governs design coordination only. It does not change the official kurrasa, application code, API contracts, database schema, permissions, or offline-write authority.
