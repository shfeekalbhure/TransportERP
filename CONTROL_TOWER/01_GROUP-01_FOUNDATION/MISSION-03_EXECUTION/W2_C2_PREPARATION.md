# W2-C2 Non-Destructive Device/PoP Preparation

- Package: `W2-C2 / REM-220`
- State: `PREPARED — PERSISTENCE/RUNTIME ACTIVATION BLOCKED BY DBP-003/006`
- Baseline: `9c5b7a1...`; code-only session contract checkpoint: `cc67ad2...`

## Contract boundary

- Device identity is immutable registry ID plus company-scoped external installation ID; hardware strings are descriptive selectors only.
- Enrollment creates `PENDING`; an active tenant administrator with `devices.manage` approves/assigns. Self-report may request revoke/lost action but never self-approve or transfer.
- Assignment binds one active device to one current `(User, Company, Branch)` scope and never widens membership.
- Revoke/lost/replaced/transfer/key-recovery invalidates bound session families, freezes protected Offline submission, preserves queued payloads and appends immutable audit.
- No administrative override exists by default. A future override requires a separate permission, reason, step-up evidence, target, prior/next state and atomic audit.

## PoP request envelope

The future proof binds algorithm/key version/thumbprint, access-token hash, HTTP method, canonical HTTPS target, exact body hash, correlation ID, issued time and one-time server nonce/JTI. Missing, future, expired, reused or mismatched proof fails closed before business mutation. Private keys remain in platform secure hardware/storage where available and never enter API/database logs.

## API/service preparation

Future interfaces are separated into registry lookup, enrollment/assignment lifecycle, proof verification, nonce consumption, session-family revocation and audit append. Registration, assignment and nonce consumption require atomic store operations. Controllers must derive Company/User from current server authority, never from mutable body claims.

Client contract after revoke is `ClearAndSuspendOffline`: erase access/refresh credentials and PoP handles as policy requires, block protected navigation/submission, retain queue payloads quarantined for governed recovery/export, and require re-enrollment/re-authentication.

## Negative test preparation

- unregistered/pending/suspended/revoked/expired device;
- wrong user/company/branch assignment and cross-company device ID reuse;
- stale credential/key version; lost/replaced device; unauthorized transfer/recovery;
- missing/wrong signature, algorithm, key, token, method, target, body or correlation binding;
- expired/future proof, missing nonce, duplicate nonce/JTI and concurrent replay;
- unaudited override attempt;
- refresh and Offline submission after device/session revoke;
- Desktop/Mobile credential clearing and key-handle recovery behavior.

No Entity, DbContext, Migration, Schema, Seed, data or runtime endpoint was added. Registry, assignment, PoP key, nonce/replay and session-device persistence remain `BLOCKED — DBP-003/006 ENTRY GATE REQUIRED`.
