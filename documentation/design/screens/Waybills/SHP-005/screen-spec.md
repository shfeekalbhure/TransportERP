# SHP-005 — رأس البوليصة — Design Pilot

## Mandatory pre-read / evidence readiness
- Queue row read: `YES`
- Governing kurrasa/current-design refs read: `PARTIAL — current main kurrasa reviewed; exact-match query for SHP-005 produced no direct hit in this review, so no canonical identity claim is made from that file`
- CoreUI/Profile refs read: `YES — frozen ScreenProfile/CoreUI architecture confirms Transaction is one of the six governed profile families`
- Current repository implementation/evidence read: `YES — TransportERP.Desktop/Waybills/WaybillFoundationForms.cs`
- Additional catalog/candidate evidence read: `YES — legacy/candidate V4 evidence identifies SHP-005 as "البوليصة — رأس المستند", Transaction, P0, requiring semantic hardening`
- Known contradictions/gaps: `Repository and candidate material use SHP-005, but exact current-main-kurrasa identity/Variant authority is not established by this pre-read.`
- Missing authority/evidence: `Explicit current governing identity + Variant/capability reconciliation for SHP-005 and its relationship to SHP-006/007/008.`
- Readiness verdict: `HOLD_AUTHORITY`

No missing fact below may be filled from conversation memory or assumption.

## Identity
- ScreenCode: `SHP-005` — repository/candidate identifier pending current governing reconciliation
- ArabicName: رأس البوليصة / candidate wording: البوليصة — رأس المستند
- Domain: Waybills
- ScreenProfile: `Transaction` — supported by repository definition and candidate material; final screen identity reconciliation remains required
- Variant: `TBD-GATED`
- Capabilities: `TBD-GATED`
- CurrentDesignState: `HOLD_AUTHORITY`
- OwnerTeam: `DESIGN-LEAD / ORCHESTRATOR`

## Governing/reference evidence reviewed
1. Current official TransportERP main kurrasa: reviewed for design governance; exact `SHP-005` match was not established in this pre-read, therefore it is not cited as direct identity authority.
2. Frozen CoreUI ScreenProfile architecture and verification report: six profiles only, including `Transaction`; shared UI stays in CoreUI.
3. CoreUI containers/layout specification.
4. CoreUI controls catalog.
5. Current repository implementation evidence: `TransportERP.Desktop/Waybills/WaybillFoundationForms.cs`.
6. Candidate/legacy evidence only, not promoted to current authority: V4 material and semantic-gap index record `SHP-005 — البوليصة — رأس المستند`, `Transaction`, P0, `REQUIRES_SEMANTIC_HARDENING`; related candidate identities include SHP-006/007/008.

## Current implementation evidence — not approved design
The current repository form shows:
- transaction screen shell;
- toolbar commands: جديد، حفظ مسودة، إرسال للاعتماد، إلغاء، إغلاق;
- summary values: رقم المسودة، رقم البوليصة، الحالة;
- tabs currently representing SHP-005/006/007/008;
- header fields currently including branch, date/time, origin, destination, currency, exchange rate, service type and priority.

These remain implementation evidence only. They are not promoted into canonical design until identity/authority reconciliation is explicit.

## Analysis questions blocked on authority
- Is SHP-005 a current governing screen identity, or only a repository/candidate identity awaiting reconciliation?
- Is SHP-005 one screen with SHP-006/007/008 as governed tabs, or are they independent screen identities presented together only by current implementation?
- What exact Variant is authoritative for SHP-005?
- Which commands are baseline toolbar commands vs. Capabilities?
- Which header fields belong to SHP-005 itself and which belong to other screen definitions/tabs?
- What is the approved fast sender/receiver lookup interaction in the governing screen identity that owns parties?
- Which actions are online-authoritative vs. draft/local/cached under the exact governing action contracts?
- What layout regions own Content vs. Fill after identity/Variant reconciliation?

## Handoff
- InputVersion: repository pilot evidence on master
- OutputVersion: this authority-precheck revision
- BlockingIssue: `Current governing SHP-005 identity/Variant relationship to SHP-006/007/008 is not source-backed strongly enough to proceed without inference.`
- NextTeam: `DESIGN-LEAD / ORCHESTRATOR`
- HandoffReady: `NO`

## Pilot exit condition
The screen may leave `HOLD_AUTHORITY` only after an explicit current governing reconciliation establishes the screen identity and enough Profile/Variant/relationship authority for TEAM-D01 to complete analysis without inference.

No code, API, DDL, permission, or offline-write changes are authorized by this pilot specification.
