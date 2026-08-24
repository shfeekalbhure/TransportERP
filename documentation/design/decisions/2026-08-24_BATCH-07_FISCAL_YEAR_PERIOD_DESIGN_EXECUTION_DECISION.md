# BATCH-07 — Fiscal Year and Period Design Execution Decision

Date: 2026-08-24
Status: OWNER APPROVED / DESIGN-ONLY CONTINUATION

Scope:
- GEN-012 — السنوات المالية / Fiscal Year
- ACC-041 — الفترات المحاسبية / Fiscal Period

Authority:
- `SRC-060 / OWNER-FISCAL-YEAR-PERIOD-W1-W2-W3-ISSUANCE-001`.
- Current `ControlApproval / Standard` CoreUI contracts.

Boundaries:
- Both screens are control/approval surfaces, not direct Create/Edit masters in this contract.
- W1 fields and Status are display-only. State change occurs only through server-authorized `ControlActionRequest`, shared approval decisions, or Reopen.
- The client does not invent action codes; server determines valid transitions.
- Approval history is append-only and SoD remains server-authoritative; self-decision is forbidden where policy applies.
- No direct Close/Open/Lock command outside the issued generic protected-action route.
- No Create/Edit/Delete/Print/Export/Disable/Enable/Move/Offline/Queue.
- `ACC-054 / PeriodAction` remains explicitly out of scope and unbound.
- No default page size or numeric cap is invented because none is issued in this contract.

No application code, DDL, API/DTO/permission contract, or official kurrasa is modified.
