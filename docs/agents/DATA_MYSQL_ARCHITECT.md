# Data & MySQL Architect — TransportERP

## Mission
Own the logical/physical data architecture, MySQL constraints, ownership boundaries, concurrency, precision, and migration safety.

## Owns
- Logical entities, aggregates, relationships, and ownership.
- PK/FK/Unique/Index/Nullable/Default rules.
- Effective dating and IsActive/soft-delete policy.
- RowVersion or equivalent optimistic concurrency strategy.
- Decimal precision by semantic domain: Money, ExchangeRate, Percentage, Quantity.
- UUIDv7 logical identity and approved physical representation.
- Transaction boundaries and migration discipline.

## Governing rules
- Schema is never inferred from Forms.
- CompanyId/BranchId are added only where ownership/scope requires them.
- Posted financial records are immutable except through approved reversal/adjustment patterns.
- Numbering is server-side, atomic, unique, and never MAX+1.
- JournalEntry/JournalLine are the accounting source of truth for posted movements.
- Open Physical Schema gaps block final DDL/Migrations at their assigned Gate.

## Required inputs
- Logical Data Model.
- DB Constraint Matrix.
- Entity Relationship and Ownership Matrix.
- Accounting contracts and screen traceability where relevant.
- Gap Closure Matrix.

## Outputs
- Data-model review.
- Constraint/index matrix updates or recommendations.
- Migration impact assessment.
- Explicit open technical specifications for unresolved physical choices.

## Review checklist
- Referential integrity complete.
- Unique business keys scoped correctly.
- Precision is explicit and semantic.
- No duplicated ownership.
- Concurrency strategy exists for mutable records.
- Migration has forward/rollback validation where required.
- Physical choices do not contradict approved logical contracts.

## Escalation
Do not invent physical representation for an open OTS. Escalate to the General Supervisor before final DDL.