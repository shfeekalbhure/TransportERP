# SHP-005 — رأس البوليصة — Design Pilot

## Identity
- ScreenCode: `SHP-005`
- ArabicName: رأس البوليصة
- Domain: Waybills
- ScreenProfile: `Transaction`
- Variant: `TBD — must be reconciled against governing kurrasa before layout`
- CurrentDesignState: `ANALYSIS`
- OwnerTeam: `TEAM-D01`

## Governing references to reconcile
1. Official TransportERP kurrasa / current W3 design authority.
2. Frozen CoreUI ScreenProfile architecture.
3. CoreUI containers/layout specification.
4. CoreUI controls catalog.
5. Current repository implementation evidence: `TransportERP.Desktop/Waybills/WaybillFoundationForms.cs`.

## Current implementation evidence — not yet approved design
The current form shows:
- transaction screen shell;
- toolbar commands: جديد، حفظ مسودة، إرسال للاعتماد، إلغاء، إغلاق;
- summary values: رقم المسودة، رقم البوليصة، الحالة;
- tabs currently representing SHP-005/006/007/008;
- header fields currently including branch, date/time, origin, destination, currency, exchange rate, service type and priority.

These are recorded as implementation evidence only. TEAM-D01 must compare each item with the governing kurrasa and owner-approved waybill draft before declaring it part of the canonical design.

## Analysis questions
- Is SHP-005 one screen with SHP-006/007/008 as governed tabs, or are they independent screen identities presented together only by current implementation?
- What exact Variant is authoritative for SHP-005?
- Which commands are baseline toolbar commands vs. Capabilities?
- Which header fields belong to SHP-005 itself and which belong to other screen definitions/tabs?
- What is the approved fast sender/receiver lookup interaction?
- Which actions are online-authoritative vs. draft/local/cached?
- What layout regions own Content vs. Fill?

## Pilot exit condition
TEAM-D01 may move this row from `ANALYSIS` to `LAYOUT` only after the above questions have source-backed answers or are explicitly marked `HOLD_AUTHORITY`.

No code changes are authorized by this pilot specification.
