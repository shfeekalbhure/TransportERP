# WP-G2-W3-Windows-Runtime-Verification — TransportERP

## Identity

- **Work Package ID:** WP-G2-W3-Windows-Runtime-Verification
- **Target gate:** G2
- **Primary owner:** SCREEN_COREUI_ARCHITECT
- **Independent reviewer:** QA_TESTING_REVIEWER
- **Items:** W3-IMP-001 and W3-IMP-002 only

## Objective

Produce repeatable Windows runtime evidence for the existing six frozen-profile CoreUI reference screens. This package is verification-only: it must not alter the frozen ScreenProfile decisions, create a new business screen, or re-evaluate W3-IMP-003.

## Governing constraints

- `G2 = NOT READY` remains unchanged.
- W3-IMP-003 is already `CLOSED + VERIFIED` and is out of scope.
- A catalog, specification, or source-only mapping is not Windows runtime evidence.
- A runtime claim requires a Windows execution record tied to a commit, a launch route for each reference, and QA review.
- Any discovered implementation defect is recorded before a separate remediation package is proposed; it is not silently fixed here.

## Acceptance criteria

For MasterData, TreeMaster, Transaction, ControlApproval, ReportInquiry, and Settings, capture:

1. a reproducible Windows launch route and executed commit SHA;
2. the profile, concrete screen type, and reused CoreUI classes;
3. observed RTL, toolbar/field order, sizing, and applicable Grid/Tree/Settings workspace behavior;
4. observation of shared validation/loading/empty/error/pagination behavior where applicable;
5. an independent `VERIFIED`, `NOT VERIFIED`, or `INSUFFICIENT` QA verdict per screen.

## 2026-08-09 execution disposition

At `7b4a4b4868f4d1554046d91ec8121af9fcece088`, the assigned environment is Linux and has neither `dotnet` nor a Windows desktop runtime. No executable artifact, screenshot, video, Windows test log, or CI run for this commit is repository-accessible. The required runtime evidence could therefore not be produced.

Static inspection also found that the six `CoreUiReferenceScreen` types are present in the catalog but are not instantiated by `Program.cs`, `FrmLogin`, or `FrmDashboard`; consequently there is no user-reachable launch route to test. This is recorded as `W3-RT-002` in the companion evidence record.

**Package result:** `BLOCKED`. No W3 item is closed by this package.
