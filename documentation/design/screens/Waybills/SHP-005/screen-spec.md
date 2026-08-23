# SHP-005 — رأس البوليصة — Non-Governing Lineage Record

## Disposition
- Repository / R2 identifier: `SHP-005`
- Arabic wording: `رأس البوليصة` / R2 wording `البوليصة — رأس المستند`
- Design disposition: `NON_GOVERNING_LINEAGE`
- Active workflow eligibility: `NO`
- Successor canonical FLOW01 pilot: `FLOW01-W3-SCR-001 — إدخال البوليصة`
- Canonical package: `documentation/design/screens/Waybills/FLOW01-W3-SCR-001/screen-spec.md`

## Authority reconciliation
The 2026-08-23 corrected/reviewed kurrasa material explicitly classifies the R2 SHP screen identities, including `SHP-005`, as `NON-GOVERNING` / `ID-CONFLICT` material and warns that the governing decision comes from section classification and the latest owner decision, not mere presence in the consolidated file.

The current FLOW01 identity authority instead issues:
- Canonical identity: `FLOW01-W3-SCR-001`
- Alias: `SHP-001`
- Role: `إدخال البوليصة / Shipment Entry`
- Profile / Variant: `Transaction / HeaderLines`

`SRC-053 / OWNER-FLOW01-W2-W3-TECHNICAL-ISSUANCE-001` and `FLOW01-W3-SCR-001_TYPED_SCREENDEFINITION.md` then issue the current W1/W2/W3 design contract for that canonical screen.

Therefore no silent equivalence is asserted between R2 `SHP-005` and canonical `FLOW01-W3-SCR-001`. `SHP-005` is retained only so repository implementation evidence and historical lineage remain traceable.

## Repository implementation evidence — retained, not promoted
`TransportERP.Desktop/Waybills/WaybillFoundationForms.cs` currently represents:
- `SHP-005` رأس البوليصة;
- `SHP-006` أطراف البوليصة;
- `SHP-007` أصناف البوليصة;
- `SHP-008` الأوزان والأبعاد والقيم;

as governed transaction tabs in the implementation candidate, with toolbar/header fields defined locally.

That implementation structure is **not** current design authority for the canonical FLOW01 waybill-entry screen. It is evidence to be reconciled later; it must not supply missing fields, commands, variants, permissions, API contracts, or tabs to the canonical design.

## Historical blocker closure
The previous `HOLD_AUTHORITY` question is no longer treated as an unknown mapping. Current evidence resolves the disposition by showing that `SHP-005` itself is non-governing for the current design workflow. The design pilot therefore continues under the separately issued canonical identity `FLOW01-W3-SCR-001`.

No code or official kurrasa is changed by this lineage record.
