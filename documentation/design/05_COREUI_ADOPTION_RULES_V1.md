# TransportERP — CoreUI Adoption Rules V1

## 1. Central ownership
The following are shared CoreUI concerns and must not be redesigned per screen:
- RTL/LTR behavior
- DPI scaling
- typography and colors
- spacing/padding/control/button heights
- toolbar rendering/order
- grid styling and common grid behavior
- pagination
- audit presentation
- validation presentation
- loading / empty / error states
- standard lookup/search behavior

## 2. ScreenProfile authority
Every desktop screen uses one of exactly six profiles:
`MasterData`, `TreeMaster`, `Transaction`, `ControlApproval`, `ReportInquiry`, `Settings`.

Profile defines structural family. Variant defines structural variation. Capability defines optional action/feature. ScreenDefinition owns screen-specific fields, grids, tabs, filters, lookup sources, permissions and sizing map.

## 3. Layout ownership
- `TransportScreenHost` owns the client area.
- headers/filters/totals/actions are Content/Fixed, not competing Fill owners.
- the primary Grid/Tree/Details/Workspace owns Fill.
- traditional main data uses one or two columns only.
- nested AutoScroll chains are prohibited.
- no fixed local pixel heights may defeat logical-DPI sizing.

## 4. Shared controls
Use shared equivalents for Toolbar, DataGrid, Search, Filters, Pagination, Audit, Validation, Lookup, Alerts, Totals, Loading, Empty, Error and Print/Export command surfaces.

## 5. Prohibited local duplication
A screen design must not request:
- screen-local toolbar styling/order;
- screen-local pagination implementation;
- screen-local audit footer;
- cloned DataGrid styling when shared grid applies;
- duplicated MessageBox validation path when shared validation exists;
- local business calculations inside shared visual controls.

## 6. Existing code reconciliation
Current forms under `TransportERP.Desktop/Waybills` are implementation evidence, not automatic design authority. During pilot analysis, compare them with the governed CoreUI/Profile rules and the kurrasa. Any mismatch becomes a review finding; design documentation must not silently normalize the mismatch.

## 7. Offline boundary
UI may describe cached reads, local drafts or queued capture only where the governing offline policy permits it. Official numbering, approval, irreversible financial effects and final server authority remain online-authoritative unless a later governed decision explicitly changes that.
