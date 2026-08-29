# OWNER-A11-RECOVERY-EXCEPTION-001

**Status:** `VALID`

**Issued by:** Project Owner  
**Recorded by:** TEAM-00  
**Scope:** Screen-workbook governance sequence only: `TEAM-03 → TEAM-06 → TEAM-08`.

## MISSING_INPUT
Exact current A11-LIVE workbook / successor content snapshot matching the formerly accessible live identity and SHA `0f6036b62901983f7ba72bbaf74559df513061e68bb530bacac22f711e9663d6`.

## EXCEPTION_REASON
The exact live File Library identity is no longer accessible and no non-archival byte-identical successor can currently be recovered. Repeated recovery and provenance reviews failed. However, an archival A11 workbook remains readable and preserves the 593-screen population, 213 logical concepts, W1 executive closure markers, and W3 593/593 gate-bound markers needed for screen identity/crosswalk work.

## IMPACT_SCOPE
TEAM-03 Wave-B screen population, A11 ID disposition, classification, deduplication and crosswalk only.

## ALLOWED_JUDGMENT
1. TEAM-03 may use `file_00000000b9a481f4858f08bad3d7581b` only as:
   `ARCHIVAL A11 RECOVERY EVIDENCE — OWNER EXCEPTION`.
2. It MUST NOT be renamed or represented as `A11-LIVE`.
3. It may be used for:
   - enumerating the 593 A11 Screen IDs;
   - reading names/modules/purposes needed for classification;
   - disposition of every A11 ID;
   - duplicate/similarity Crosswalk;
   - verifying whether a Wave-A requirement already has an A11 ID;
   - completing NO-A11-MATCH checks against the available 593-ID population;
   - confirming preserved W1/W3 markers that physically exist in that workbook.
4. Newer authoritative inputs override archival semantics whenever applicable, including:
   - `R-011 / ACC-001`;
   - `R-012 / OFFLINE-001`;
   - `R-013 / CLIENT-001`;
   - Final Handoffs TEAM-01/02/04/05/07;
   - later explicit owner/governance decisions.
5. For every use of this workbook TEAM-03 must record:
   - `REFERENCE BASIS = OWNER-A11-RECOVERY-EXCEPTION-001`
   - `ARCHIVAL RECOVERY EVIDENCE — NOT A11-LIVE`
6. TEAM-03 resumes from its existing blocked outputs and MUST NOT redo completed Wave-B work from zero unless necessary for reconciliation.

## PROHIBITED_JUDGMENTS
- Do not declare `file_00000000b9a481f4858f08bad3d7581b` to be A11-LIVE.
- Do not claim byte parity with `0f6036b62901983f7ba72bbaf74559df513061e68bb530bacac22f711e9663d6`.
- Do not infer that archival state is the latest technical state.
- Do not use archival technical-readiness values to override later W1/W2/W3 decisions.
- Do not create parallel A11 IDs.
- Do not promote Local Candidates to canonical IDs.
- Do not resolve `NEEDS OWNER DECISION`.
- Do not infer ScreenDefinition, fields, actions, permissions, DDL, API, Offline authority or programming readiness from the archival workbook.
- Do not reopen or modify Wave-A final reports.

## DESIGN_GATE_EFFECT
This exception permits TEAM-03 to complete 03 and 03-P and permits TEAM-00 to evaluate them for `DELIVERED` status.

If both TEAM-03 and TEAM-03-P pass TEAM-00 DELIVERY CHECK under this exception, Wave-C / TEAM-06 MAY be opened by a separate governance action.

This exception DOES NOT by itself open the visual-design gate.

Visual design remains dependent on completion of TEAM-06, TEAM-08 reconciliation, and owner review.

## EXPIRY/BOUNDARY
This exception applies only to the screen-workbook governance sequence `TEAM-03 → TEAM-06 → TEAM-08`.

It expires for A11 reference purposes if an independently verified current A11-LIVE/successor workbook is recovered.

If such a workbook is recovered before TEAM-08 closes, TEAM-08 must perform a delta reconciliation between the recovered current A11 and the Population produced under this exception.

## REQUIRED TEAM-03 DECLARATIONS
TEAM-03 final outputs must explicitly state:

- `A11-LIVE EXACT CURRENT SNAPSHOT AVAILABLE = NO`
- `ARCHIVAL RECOVERY EXCEPTION USED = YES`
- `ARCHIVAL FILE_ID = file_00000000b9a481f4858f08bad3d7581b`
- `ARCHIVAL FILE TREATED AS A11-LIVE = NO`
- `SILENT SUBSTITUTION = NO`

TEAM-03 must complete:
- full 593-row disposition;
- NO-A11-MATCH verification;
- final classification/dedup/crosswalk;
- corrected counters;
- TEAM-03 report;
- TEAM-03-P population.

## GATE STATE
- `OWNER-A11-RECOVERY-EXCEPTION-001 = VALID`
- `TEAM-03 = AUTHORIZED TO RESUME FROM EXISTING BLOCKED OUTPUTS`
- `TEAM-06 = REMAINS CLOSED UNTIL 03 + 03-P DELIVERED`
- `TEAM-08 = REMAINS CLOSED`
