# W1-CORE limited cross-cutting amendment — ORG-OD-003A

## Authority and scope

This amendment is authorized by **ORG-OD-003A** and is applied under
`W1-SETUP-ORG` only. It does not reopen, replace, or otherwise change the
historical W1-CORE closure.

The sole W1-CORE production-contract change is the addition of these approved
`TransportErrorCode` values:

- `Conflict`
- `StateTransitionInvalid`
- `ApprovalStateInvalid`
- `PeriodClosed`
- `SelfApprovalDenied`
- `DuplicateNumber`
- `NumberSequenceInactive`
- `NumberingStateInvalid`
- `IdempotencyConflict`

`W1CoreContractTests.TransportError_UsesOnlyTheApprovedStandardCodesAndCorrelation`
is amended to assert the complete 14-code catalog, validate every approved
value, reject `(TransportErrorCode)999`, and retain the mandatory
`CorrelationId` and `MessageKey` checks.

## Explicit exclusions

No change is authorized or made to `OperationContext`, `CapabilityState`,
`BusinessAuditEvent`, `IBusinessAuditWriter`, or the `TransportError` design.
No error code other than the nine listed above is introduced.

## Traceability

`ORG EXECUTION CONTRACT V1.1` → `ORG-OD-003A` → `TransportErrorCode` →
`W1-CORE Contract Test` → `W1-SETUP-ORG Error Mapping`.
