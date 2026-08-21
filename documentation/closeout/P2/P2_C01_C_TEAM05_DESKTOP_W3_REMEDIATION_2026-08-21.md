# P2-C01-C — TEAM-05 Desktop RTL + W3 Remediation Evidence

**Scope:** TEAM-05 / Issue #45  
**Phase:** P2-C01-C only  
**Allowed screens:** SHP-015 / SHP-016 / SHP-019 / SHP-023 / SHP-024 / SHP-025 / SHP-027 / SHP-028 / SHP-029 / SHP-030  
**Status:** IMPLEMENTED — EXACT-HEAD VERIFICATION REQUIRED

## Remediation delivered

- Added typed W3 Desktop projection/input models in `TransportERP.Desktop/Waybills/ShippingExecutionW3Models.cs`.
- Completed W3 fields/actions in `ShippingExecutionForms.cs` for the ten governed C screens only.
- Preserved Arabic `RightToLeft = Yes` and `RightToLeftLayout = true` through the common `ShippingRtlForm`.
- Desktop remains contract-only; no EF/DbContext/Npgsql/SQL/Infrastructure reference was added.
- SHP-023/024/027 render priority, risk and capacity/resource state as readable text rather than relying on color.
- SHP-025 now emits the existing `CreateTripRequest` contract.
- SHP-015/016/027/028/029/030 emit only existing P2-C01-C command request contracts.
- SHP-028 includes quantity/weight/volume totals and an RTL print-preview representation.

## SHP-024 split-allocation rule

`LoadPlanningRow.FromManifestLine(...)` binds:

- `Qty` from `ManifestLineResponse.Quantity`;
- `AllocatedWeight` from `ManifestLineResponse.Weight`;
- `AllocatedVolume` from `ManifestLineResponse.Volume`.

The Desktop therefore displays the server-authoritative allocation measures and does not copy the original full WaybillItem weight/volume when one item is split across trips. The C backend rule/test remains responsible for deriving those manifest-line measures proportionally from the allocated quantity.

## CI hardening

The Windows `Shipping Desktop RTL` gate now checks:

- exact PR head checkout;
- Desktop build as `Library`;
- the ten governed screen IDs and rejection of any `SHP-031+` surface;
- explicit Arabic RTL and layout mirroring;
- no direct persistence tokens/project references;
- no untyped `IEnumerable<object>` W3 binding;
- required typed W3 models/actions, capacity/risk text, manifest totals and RTL printing;
- SHP-024 mapping to `line.Quantity`, `line.Weight`, and `line.Volume`;
- governed screen-profile mapping.

## Gate history

- `892396ca6d2a01dec8f6370715f8f14e10071965`: Desktop Library build succeeded and the strengthened static gate initially failed because the gate looked for a contract property name instead of the actual positional request binding.
- `f0ab520b13a24128e4fb1213a338deaf711711c1`: corrected gate; exact-head Desktop build and W3/RTL gate succeeded.
- A later TEAM-02 automation commit moved the PR head without touching Desktop/W3 files. Per exact-head governance, TEAM-05 PASS must be re-established on the new final head before it is reported.

No merge was performed. No Arrival/Transit/Warehouse/Delivery/Financial Close runtime was added.
