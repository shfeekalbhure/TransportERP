# W5 — Desktop/Mobile Execution Preparation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `PREPARATION COMPLETE — PRODUCT ENTRY BLOCKED`
- Product modification: `NONE`

## Truthful client inventory

Desktop has no `Program.cs` and conditionally builds as a Library. Its forms are
not composed into an executable host, API client, session shell or permission
navigation. Mobile Admin, Customer and Driver each contain only a project file;
they have no MAUI program, App, Platforms or runtime resources and build as
`net10.0` Libraries. The current test project references none of the client
projects. Run `33191269475` proves only these Library build surfaces.

No current client contains HttpClient authentication, secure credential
storage, logout/revoke, offline queue or reconnect logic.

## Entry decision

`W5 PRODUCT ENTRY = BLOCKED — W2/W4 + DEP-013/014 NOT SATISFIED`

- durable session/device/tenant controls are incomplete;
- W4 Offline authority/runtime is incomplete;
- no accepted canonical screen/route/supersession registry exists;
- delivery platforms, application IDs, signing identities/custody and channels
  are not authorized.

## Prepared packages

1. Client inventory: entry point, screen, route, permission, API dependency and
   runtime/scaffold truth.
2. Session/credential matrix: `ClearAndSuspendOffline` means atomically clear
   access/refresh and key handles, close protected navigation, freeze outbound
   work and require reauthentication.
3. T-500 design: signed install/launch, TLS/config fail-closed, login/refresh/
   revoke, native secure storage, permission navigation, Offline suspension,
   reconnect, RTL/accessibility, upgrade/rollback and artifact hashes.
4. Packaging dossier: Windows and Android scope, application identity,
   certificate/keystore custody, distribution channel and cache compatibility.
5. PR #69 matrix: Desktop and Driver patterns are selective reimplementation
   candidates only; Admin/Customer complete-runtime claims are rejected.

After the gates close, Desktop host, shared session policy and each authorized
mobile application must be separate reversible increments. No plaintext
fallback is permitted when secure storage fails. Any client-local database or
migration requires its own DB-GOV path.

## External evidence required

- accepted canonical screen/supersession registry (`DEP-013`);
- approved client/delivery/platform scope and signing authority (`DEP-014`);
- signing certificate/keystore custody without exposing secrets;
- Windows/Android execution environments and signed install/launch evidence.
