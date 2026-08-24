# Legacy Waybill Queue Lineage Reconciliation — 2026-08-24

**Scope:** `SHP-006`, `SHP-007`, `SHP-008`, `SHP-014`  
**Result:** `NON_GOVERNING_LINEAGE` for all four legacy queue rows.

## Governing evidence
1. Current pre-implementation screen review classifies each of the four IDs as `NON-GOVERNING / BLUE / ID-CONFLICT`:
   - `SHP-006 — أطراف البوليصة`
   - `SHP-007 — أصناف البوليصة`
   - `SHP-008 — الأبعاد/الوزن/الحجم/القيمة`
   - `SHP-014 — اعتماد البوليصة`
2. FLOW01 canonical identity decision states that old A9/R2 identifiers with semantic conflicts are retained as **Aliases/Lineage only** and must not be reused as canonical screen identities.
3. The current canonical FLOW01 design surface is already closed under `FLOW01-W3-SCR-*` identities; no owner decision promotes these four legacy IDs back into the canonical design scope.

## Disposition
| Legacy ID | Legacy label | Previous queue state | Final design-queue classification | Design action |
|---|---|---|---|---|
| SHP-006 | أطراف البوليصة | BACKLOG | NON_GOVERNING_LINEAGE | Do not design as standalone canonical screen |
| SHP-007 | أصناف البوليصة | BACKLOG | NON_GOVERNING_LINEAGE | Do not design as standalone canonical screen |
| SHP-008 | الأوزان والأبعاد والقيم | BACKLOG | NON_GOVERNING_LINEAGE | Do not design as standalone canonical screen |
| SHP-014 | اعتماد البوليصة | BACKLOG | NON_GOVERNING_LINEAGE | Do not design as standalone canonical screen |

## Boundary
This reconciliation changes **design queue classification only**. It does not delete historical material, change official Kurrasa content, create a replacement screen identity, modify application code, or alter W1/W2/W3/API/DTO/permissions.

If a future owner decision explicitly promotes any of these IDs or creates a new canonical identity for their semantics, that future authority must enter the normal design workflow from `ANALYSIS`.

**DESIGN-LEAD disposition:** queue backlog ambiguity closed; no outstanding design work is created from these four lineage rows.
