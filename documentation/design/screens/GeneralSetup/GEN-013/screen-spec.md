# GEN-013 — الترقيم العام — Canonical Screen Specification

**English:** General Numbering  
**Profile / Variant:** `Settings / NumberingControlled`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-20`

## Authority
Current57 baseline + current W2 + `OWNER-WAVE1-20260823` + `ORG-OD-004` + controlled resolution R-008 + CoreUI Settings.

## ANALYSIS — TEAM-D01 PASS
Purpose: manage governed numbering policy and reservation lifecycle without client-side number generation.

Tabs exactly:
1. سياسات الترقيم
2. نطاقات الترقيم
3. الاستثناءات والاعتماد
4. سجل التخصيص

Executable surface exactly:
`View | Edit | Reserve | Commit | Cancel | Override`.

There is no Create/Disable/Delete/Print/Export or offline-final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared Settings/NumberingControlled host only. Policy/header regions are Content; reservation/history workspace uses shared Fill behavior where applicable. No local toolbar/grid/paging/RTL/DPI/error/audit architecture.

## FIELD_GRID — TEAM-D03 PASS
Governing fields (10):
1. `code` — الرمز — read-only sequence/document-type code projection; not a second business identity.
2. `arabicName` — الاسم العربي — read-only definition/catalog label.
3. `englishName` — الاسم الإنجليزي — read-only definition/catalog label.
4. `status` — الحالة — read-only/server sequence state; no independent enable/disable command.
5. `notes` — ملاحظات — optional policy annotation; editable only through issued Edit policy and audited where supported.
6. `scope` — النطاق — read-only/controlled projection of Company/Branch/FiscalYear scope tuple; exact binding server-owned.
7. `documentType` — نوع المستند — governed document-type identity/context.
8. `prefix` — بادئة — editable policy field through `Edit`; validation server-owned.
9. `lastNumber` — آخر رقم — read-only, derived from durable committed allocation; never computed by client and never equated silently to legacy `NextValue`.
10. `resetPolicy` — إعادة ضبط — governed policy field; ordinary policy editing is distinct from protected reset/override execution.

Current baseline issues no concrete screen-specific grid. Reservation/history presentation uses shared Settings/Details/List infrastructure only; no local business columns are invented.

## UX — TEAM-D04 PASS
- `Reserve` obtains an atomic server reservation; UI never calculates next number.
- `Commit` and `Cancel` operate on issued reservations and preserve history; cancelled numbers are not reused.
- `Override` is the only protected sequence override/reset action. It requires current permission, reason, ExpectedVersion and approval when policy requires.
- `Edit` changes only ordinary issued sequence policy metadata; it must not mutate `lastNumber` as an ordinary field.
- Scope and document type are revalidated server-side.
- Shared loading/error/conflict/audit behavior only; no offline queue/outbox/replay.

## VISUAL — TEAM-D05 PASS
Use shared Settings typography, RTL/DPI, semantic state and audit/history presentation. Protected values are visually read-only. No local color/state engine or numbering visualization that implies client authority.

## TEAM-D06 — INDEPENDENT REVIEW
Pending. Confirm exact 10 fields, four tabs, six W2 capabilities, no client `MAX+1`, read-only Last Number, protected Override separation and no migration claim.

## Remaining implementation/release gates
Legacy `NextValue → LastNumber` supersession/migration/backfill, exact physical/DTO bindings, scope FKs and final runtime/release evidence remain separate from design approval.
