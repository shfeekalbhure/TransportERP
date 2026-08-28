# CLIENT-001 — Delivery and Signing Scope

Decision date: 2026-08-28
Owner decision: `APPROVED — RESOLVED`
Owner approval record: `EXPLICIT OWNER APPROVAL — 2026-08-28 — "اعتمد"`

## Decision

`CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS IS DEFERRED`

The release targets for MISSION-03 are:

| Client | Release target | Platform | Canonical application identity | Initial distribution channel |
|---|---|---|---|---|
| Desktop | YES — primary staff client | Windows 10/11 x64 | `TransportERP.Desktop` | signed offline-capable/private enterprise installer |
| Mobile Admin | YES | Android | `com.altayer.transporterp.admin` | signed APK/AAB; private/managed/direct distribution |
| Mobile Customer | YES | Android | `com.altayer.transporterp.customer` | signed APK/AAB release package; public-store publication is not required for MISSION-03 exit |
| Mobile Driver | YES | Android | `com.altayer.transporterp.driver` | signed APK/AAB; private/managed/direct distribution |
| iOS/macOS mobile targets | NO for MISSION-03 | deferred | none | later owner/release decision |

## Executable truth requirement

Current Library/scaffold projects are not accepted as release evidence. W5 must produce real executable entry points and runtime proof for every YES target above.

A client passes only when the exact release candidate demonstrates, as applicable:

- application start and authenticated navigation;
- local-authority login/session lifecycle;
- secure credential storage;
- credential clearing and protected-navigation denial after revoke;
- Offline queue suspension/recovery behavior;
- reconnect/re-authentication behavior;
- tenant/permission fail-closed behavior;
- version/application identity binding;
- signed artifact generation and reproducible provenance.

A successful class-library build alone is not a client PASS.

## Signing authority and custody

Signing authority is owned by the TransportERP project/company release authority, not by an individual developer account.

- Windows code-signing certificate/private key, Android signing keys and any future store credentials remain outside source control.
- Private signing material must be held in approved protected custody such as HSM/KMS/secure certificate store/platform keystore; raw private keys must not be committed or printed in CI logs.
- CI may receive only an approved secret reference/short-lived signing capability.
- Development/debug keys are never Production release authority.
- Rotation, recovery, revocation and backup custody must be documented before W7 release closure.

## Release channels

MISSION-03 must prove signed release artifacts and installation/update/recovery in the approved private/direct channels above. App Store/Google Play publication, public listing, marketing metadata and iOS signing are outside the MISSION-03 closure requirement unless later explicitly added.

The products must remain usable in the intended offline-capable operating model after installation; distribution itself must not create an always-online runtime dependency.

## Application-ID stability

The identifiers above are owner-selected canonical release identities for this mission. They must not be changed by implementation teams without a superseding owner decision because identity changes affect upgrades, secure storage, signing, device enrollment and offline data continuity.

## Implementation boundary

This decision resolves client scope and signing authority. It does not provide signing keys/certificates or Production secrets. Actual certificates/keystores, release topology and recovery proof remain authorized external evidence for W7. No Product/DB/Production mutation is authorized by this decision file itself.
