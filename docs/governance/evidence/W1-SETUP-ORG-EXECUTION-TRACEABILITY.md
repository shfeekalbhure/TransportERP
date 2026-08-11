# W1-SETUP-ORG execution traceability

| Screen | Aggregate / endpoint | Server permission | Audit action | Error mapping |
|---|---|---|---|---|
| GEN-008 Currency | `Currency`, `/api/v1/general/currencies` | `GEN008.View/Manage` | Create/Update/Disable | ValidationFailed, DuplicateNumber, ConcurrencyConflict |
| GEN-009 Exchange Rate | `ExchangeRate`, `/api/v1/general/exchange-rates` | `GEN009.View/Manage` | Create/Update | ValidationFailed, ScopeDenied, Conflict |
| GEN-010 Company | `Company`, `/api/v1/general/companies` | `GEN010.View/Manage` | Create/Update | ValidationFailed, DuplicateNumber |
| GEN-011 Branch | `Branch`, `/api/v1/general/branches` | `GEN011.View/Manage` | Create/Update | ValidationFailed, ScopeDenied, DuplicateNumber |
| GEN-012 Fiscal Year | `FiscalYear`, `/api/v1/general/fiscal-years` | `GEN012.View/Manage` | Create/Open/Close | StateTransitionInvalid, PeriodClosed, ConcurrencyConflict |
| GEN-013 General Numbering | `NumberSequence`/`NumberReservation`, `/api/v1/general/number-sequences` | `GEN013.View/Manage/Reserve` | Create/Update/ReserveNumber | DuplicateNumber, NumberSequenceInactive, NumberingStateInvalid, IdempotencyConflict |
| GEN-014 Languages | `Language`, `/api/v1/general/languages` | `GEN014.View/Manage` | Create/Update/Disable | ValidationFailed, DuplicateNumber |
| GEN-015 Operational Settings | `SettingDefinition`/`SettingOverride`, `/api/v1/general/operational-settings` | `GEN015.View/Manage` | Create/Update/Disable | ValidationFailed, ScopeDenied, ConcurrencyConflict |

Only the nine-code amendment and its W1-CORE test changed W1-CORE under `ORG-OD-003A`. No Offline Write, cache, sync, fiscal periods, GL posting, external provider, or client-side number generation was introduced.
