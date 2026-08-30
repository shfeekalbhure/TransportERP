# OWNER-REVIEW-001 — Screen Workbook Owner Decisions and Design Gate

**Status:** `APPROVED — OWNER REVIEW COMPLETE`  
**Date:** `2026-08-30`  
**Issued by:** Project Owner  
**Recorded by:** TEAM-00  
**Scope:** TransportERP screen-workbook governance sequence after Waves A-D.  
**Branch:** `governance/control-tower-20260828-screens-workspace`

## 1. Governing review basis

TEAM-08 governing report:

`CONTROL_TOWER/02_GROUP-02_EXPANSION/مساحة_عمل_فرق_كراسة_الشاشات_2026-08-28/04_تقارير_الفرق/الفريق_08/تقرير_الفريق_08_المراجعة_والمصالحة.md`

- TEAM-08 commit: `e50afbdbc04bce18efae695be3dbe61ee985ad9b`
- TEAM-08 blob: `8e827930dd97be78a189dcde5b6dfcdfe29b14eb`
- TEAM-08 judgment: `PASS WITH OWNER DECISIONS REQUIRED — READY FOR OWNER REVIEW`
- TEAM-08 `OWNER QUESTIONS REVIEWED = 21`
- TEAM-08 `BLOCKERS = 0`
- TEAM-08 `MAJOR FINDINGS = 0`
- TEAM-08 `MINOR FINDINGS = 1` — required-field presentation conflict only.

Trace sources affected by these decisions, without modifying them:

- TEAM-03 report: `.../04_تقارير_الفرق/الفريق_03/تقرير_الحصر_والتصنيف_والغربلة.md`; commit `ad986d24db31444395706b3edcf6fd8a3b4d2be5`; blob `8eea872ad15ec03149c18561bdc2444512bb55f6`.
- TEAM-03-P population: `.../04_تقارير_الفرق/الفريق_03/سجل_مجتمع_الشاشات_المثبت_للانتقال_إلى_التفصيل.md`; commit `ad986d24db31444395706b3edcf6fd8a3b4d2be5`; blob `9fd4ace7a4f5387edda4c3e048f664ad6f20e958`.
- TEAM-06 report: `.../04_تقارير_الفرق/الفريق_06/تقرير_بطاقات_الشاشات_والحقول_والجداول.md`; commit `9be95da7af9eaa2dbb3ad824bbadf77cb8766e6b`; blob `8acd61e733be59c256aba71c857e672bbb4332c4`.

This record does **not** modify TEAM-01..TEAM-08 or TEAM-03-P. It resolves the owner-decision layer above those delivered artifacts.

## 2. Owner Decision Register — 21/21

### OWNER DECISION 01 — Channels

**Resolves:** TEAM-03 §7 item 1 / TEAM-08 Owner-Decision Register item 1.  
**Affected trace:** TEAM-03 / TEAM-06 / TEAM-07 / CLIENT-001.

Approved current release channels:
- Desktop.
- Android Admin.
- Android Customer.
- Android Driver.

Carrier / Vehicle Owner / Service Provider functions are delivered in phase one inside Android Driver according to Role/Permissions. No fifth client exists now. Future separation into an independent app/portal requires a later explicit owner decision.

### OWNER DECISION 02 — Carrier Network

**Resolves:** item 2.  
**Affected trace:** TEAM-03 / TEAM-04 / TEAM-06 / TEAM-07.

Phase one uses `CLOSED / INVITED CARRIER NETWORK` for approved/invited carriers only. Carriers may submit offers inside the system. A future `PUBLIC CARRIER MARKETPLACE` may be enabled only after Verification, Trust, Rating, Onboarding and Governance controls are established.

### OWNER DECISION 03 — Light / Medium / Heavy Transport Classification

**Resolves:** item 3.  
**Affected trace:** TEAM-03 / TEAM-04 / TEAM-05 / TEAM-06.

Classification is policy-driven using a combination of Weight, Volume/Dimensions, Axles, Vehicle Type, Cargo Nature/Hazard, and Permit Requirements. Numeric thresholds must not be hard-coded; they are managed through configurable Settings.

### OWNER DECISION 04 — Waybill Issuance / Field Issuance

**Resolves:** item 4.  
**Affected trace:** TEAM-01 / TEAM-03 / TEAM-04 / TEAM-06 / TEAM-07.

A waybill remains Draft/Request until required inspection is complete. Direct field issuance is allowed only for approved categories, approved permissions and approved exceptions with complete Audit. A field request never becomes an Issued Waybill automatically without verification.

### OWNER DECISION 05 — Price Source Priority

**Resolves:** item 5.  
**Affected trace:** TEAM-01 / TEAM-03 / TEAM-05 / TEAM-06.

Priority:
`Contract / Special Agreement → Party-specific Tariff → General Tariff → Approved Manual Quote`.

The system must preserve Price Source, Version, Effective Date, Changed By, Override Reason and Approval. Any override requires explicit authority.

### OWNER DECISION 06 — Quote Lifecycle

**Resolves:** item 6.  
**Affected trace:** TEAM-02 / TEAM-03 / TEAM-05 / TEAM-06.

Lifecycle:
`Draft → Submitted → Under Review → Accepted / Rejected / Expired / Repricing Requested → Revised → Resubmitted`.

Every modification or repricing creates a new Version. Previous quotes must not be overwritten.

### OWNER DECISION 07 — Commission Policy

**Resolves:** item 7.  
**Affected trace:** TEAM-01 / TEAM-02 / TEAM-03 / TEAM-05 / TEAM-06.

Commission is policy-driven and configurable by commission type. Each type defines Trigger (`Sale / Collection / Execution / Delivery / Settlement`), Calculation Base, Beneficiary, Rate or Fixed Amount, Effective Period, Approval Authority and Version History. No single trigger applies to all commissions.

### OWNER DECISION 08 — Driver Pay vs Carrier Cost

**Resolves:** item 8.  
**Affected trace:** TEAM-03 / TEAM-04 / TEAM-05 / TEAM-06.

Driver Pay and Carrier Cost are fully separate concepts even when the driver is the vehicle owner.

Driver Pay may be Salary, Percentage, Trip Amount, Task or Allowance based. Carrier Cost may be Trip, Shipment, Weight, Distance or Carrier Contract based. Each has Independent Trigger, Independent Approval and Independent Settlement.

### OWNER DECISION 09 — Damage / Loss / Shortage Liability

**Resolves:** item 9.  
**Affected trace:** TEAM-01 / TEAM-03 / TEAM-04 / TEAM-05 / TEAM-06.

Liability is not assigned automatically. It is determined only after `DOCUMENTED INVESTIGATION` using Custody Chain, Evidence and Operational Facts. Liability may be assigned to Driver, Carrier, Company, Insurance, Branch, Agent or Other Party, including percentage allocation when established by evidence.

### OWNER DECISION 10 — Claim Evidence

**Resolves:** item 10.  
**Affected trace:** TEAM-03 / TEAM-04 / TEAM-05 / TEAM-06 / TEAM-07.

Required evidence is determined by Claim Type, Service Type and Risk. Minimum applicable evidence includes Incident, Date/Time, Location, Waybill/Trip, Custody, Description, Affected Quantity/Weight, Photos, POD, Receipt/Delivery Evidence, Custody Chain and Actor Identity. No Financial Effect may occur until mandatory evidence for the case is complete.

### OWNER DECISION 11 — Refund / Compensation

**Resolves:** item 11.  
**Affected trace:** TEAM-03 / TEAM-04 / TEAM-05 / TEAM-06.

Refund/Compensation may be Full or Partial according to reason and policy. The original transaction is never silently deleted or rewritten. Record Amount, Fees/Deductions, Liable Party, Refund Method, Reason, Approval and Original Transaction Link. Refund/Reversal is a linked movement against the original transaction.

### OWNER DECISION 12 — Branch / Agent Unsettled Collections

**Resolves:** item 12.  
**Affected trace:** TEAM-03 / TEAM-05 / TEAM-06.

Collected amounts remain `UNSETTLED COLLECTIONS` until Matching plus Actual Remittance/Settlement. They are not available cash-box balances before settlement. Supported states: Pending, Overdue, Escalated, Settled, with Due Period, Alerts, Escalation, Difference Register, Reasons and Approvals. Selected permissions may be suspended after approved limits are exceeded.

### OWNER DECISION 13 — Settlement Workflow

**Resolves:** item 13.  
**Affected trace:** TEAM-03 / TEAM-05 / TEAM-06 / TEAM-07 / ACC-001.

Workflow:
`Draft → Submitted → Under Review → Rejected / Returned for Rework → Approved → Posted → Reversed`.

Maker, Reviewer, Approver, Poster and Reverser are explicitly separated. Reason is mandatory for Reject, Return for Rework and Reverse. Previous Settlement/Posting records must not be deleted. `ACC-001` remains governing.

### OWNER DECISION 14 — Revenue Recognition

**Resolves:** item 14.  
**Affected trace:** TEAM-01 / TEAM-03 / TEAM-05 / TEAM-06 / ACC-001.

Revenue Recognition is policy-driven per Service Type. The recognized event may be Issuance, Execution Start, Trip Completion, Delivery, Service Completion or Other Approved Operational Event. Collection does not equal Revenue Recognition. Each rule is Versioned, Effective-Dated and Audited. No retroactive policy change applies to historical transactions without an approved corrective procedure.

### OWNER DECISION 15 — Dynamic Pricing

**Resolves:** item 15.  
**Affected trace:** TEAM-02 / TEAM-03 / TEAM-05 / TEAM-06.

Feature exists with `DEFAULT = OFF`. If enabled, configurable factors may include Season, Demand, Route, Remaining Capacity, Weight, Volume and Urgency, with Minimum/Maximum adjustment, Override limits, Approval, Version, Effective Date and Audit.

### OWNER DECISION 16 — MFA / Step-up

**Resolves:** item 16.  
**Affected trace:** TEAM-03 / TEAM-06 / TEAM-07.

Use `RISK-BASED MFA`. Admin/Sensitive Accounts require MFA by default. Sensitive actions including Posting, Reversal, Permission Changes, Recovery and Security-sensitive changes require `STEP-UP AUTHENTICATION`. Authentication factors are configurable; SMS or Passkey is not assumed as the sole factor. Unresolved high-risk state defaults to `DENY / REQUIRE STRONGER AUTH`.

### OWNER DECISION 17 — Session Policy

**Resolves:** item 17.  
**Affected trace:** TEAM-03 / TEAM-06 / TEAM-07.

Policy is configurable by Client/Role. Approved defaults:
- Access Token = 15 minutes.
- Refresh Token = 30 days with Rotation.
- Desktop/Admin Idle Timeout = 30 minutes.
- Android Admin Idle Timeout = 30 minutes.
- Android Driver Idle Timeout = 12 hours.
- Android Customer Idle Timeout = 24 hours.
- Absolute Re-auth: Admin/Desktop = 12 hours; Driver = 24 hours; Customer = 7 days.
- Concurrent Sessions: Admin = 2; Driver = 2; Customer = 3.
- Remember Trusted Device = 30 days where policy permits.

Password Change / Recovery / Revoke / Security Event invalidates applicable sessions. Step-up may be required even inside a valid session.

### OWNER DECISION 18 — Account / Device Recovery

**Resolves:** item 18.  
**Affected trace:** TEAM-03 / TEAM-06 / TEAM-07.

A lost device cannot prove Recovery. Recovery requires Documented Request plus Alternative Trusted Factor or Appropriate Identity Evidence. Sensitive accounts require Admin/Security Approval. After successful recovery: Revoke Old Sessions; Rotate/Revoke Refresh Tokens; Revoke Old Device Trust; Re-enroll MFA; Register New Device as New Device. High-risk recovery defaults to `DENY / MANUAL REVIEW`, with complete Audit.

### OWNER DECISION 19 — Device Trust Lifecycle

**Resolves:** item 19.  
**Affected trace:** TEAM-03 / TEAM-06 / TEAM-07.

States: Pending, Trusted, Suspended, Revoked, Lost, Replaced. A new device is never automatically Trusted. Sensitive roles require appropriate verification/approval. Trust transfer follows `Revoke Old Device → Verify User → Approve/Register New Device`. Server is Authoritative Source. Unknown Device defaults to `UNTRUSTED / RESTRICTED`.

### OWNER DECISION 20 — Background GPS

**Resolves:** item 20.  
**Affected trace:** TEAM-01 / TEAM-03 / TEAM-04 / TEAM-06 / TEAM-07.

`DEFAULT = OFF` outside active trip/task. Background GPS is enabled only for roles/tasks that genuinely require it. Settings define Who, Start, Stop, Accuracy, Cadence, Retention and Background Permission. No hidden tracking. Refusal must have an Operational Alternative. GPS alone does not prove Custody Transfer or Delivery. `Data Minimization = Default`.

### OWNER DECISION 21 — POD / Identity / Signature / Photo / Biometric

**Resolves:** item 21.  
**Affected trace:** TEAM-01 / TEAM-02 / TEAM-03 / TEAM-04 / TEAM-06 / TEAM-07.

POD requirements are configurable by Service + Risk.

Default Normal Service:
`OTP OR Signature + Recipient Name + Delivery Time`.

Photo, Location and Identity Verification may be added when justified. High-value/Sensitive Service may require stronger evidence. Identity Photo is not mandatory unless an approved service/reason requires it. Biometric has `DEFAULT = OFF / DENY` and may be enabled only by an explicit decision proving Necessity, Legal Basis and Privacy Controls. Every evidence type must define Purpose, Access, Retention, Deletion and Legal Hold. `Data Minimization = Default`.

## 3. Global Configuration Principle

Approved principle:

`POLICY-DRIVEN CONFIGURATION — SETTINGS FIRST, WITH SAFE DEFAULTS`

Every configurable policy must carry:
- Default Value.
- Current Value.
- Scope: Company / Branch / Service / Role as applicable.
- Effective Date.
- Version.
- Changed By.
- Reason.
- Approval.
- Audit Trail.

No empty value may result in unknown behavior. Sensitive unresolved states use `SAFE DEFAULT / DENY`.

Governance invariants are not free configuration, including:
- `Collection ≠ Posting`.
- `Settlement ≠ Journal Posting`.
- `Posting/Reversal = ONLINE AUTHORITATIVE`.
- SoD boundaries, including ACC-001 constraints.

## 4. Minor Conflict Resolution — Required Field Marker

**TEAM-08 finding:** one MINOR R-006/R-007 presentation conflict.  
**Owner resolution:** `RESOLVED BY OWNER`.

Approved presentation/accessibility rule:
- `LIGHT GREEN INDICATION`.
- Required marker `*`.
- Display the word `مطلوب` where appropriate.
- Color must **not** be the sole mechanism for indicating a required field.

This is Presentation/Accessibility only and does not change identity, function, population, permissions or workflow.

## 5. Owner Review Closure Counters

- `OWNER QUESTIONS ORIGINAL = 21`.
- `OWNER QUESTIONS RESOLVED BY OWNER = 21`.
- `OWNER QUESTIONS REMAINING = 0`.
- `BLOCKERS = 0`.
- `MAJOR FINDINGS = 0`.
- `MINOR PRESENTATION CONFLICT = RESOLVED BY OWNER`.
- A11 archival recovery constraints remain unchanged; this owner review does not reclassify archival A11 as A11-LIVE.

## 6. Gate Decisions

`OWNER REVIEW = COMPLETE`

`OWNER REVIEW GATE = CLOSED — COMPLETED`

`DESIGN GATE = OPEN`

`VISUAL DESIGN = AUTHORIZED TO START`

`PROGRAMMING GATE = CLOSED`

`SOURCE / TESTS / DATABASE / MIGRATIONS = NO CHANGE`

This decision authorizes **visual design only**. It does not authorize implementation, Source changes, Tests, Database, Migrations, API, DDL, coding, merge or deployment.
