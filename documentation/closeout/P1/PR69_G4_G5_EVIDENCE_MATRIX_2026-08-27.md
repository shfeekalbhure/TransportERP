# PR #69 — G4/G5 exact-SHA evidence matrix

**Evidence date:** `2026-08-27`  
**Implementation SHA:** `b9788d5a6e4deca9505ae481fa92432ba3ddb6e3`  
**Implementation tree:** `25656a07ca26bd2d5d32281ab971b865eaf9e80f`  
**Branch:** `codex/p1-security-device-sync-offline-20260825`  
**PR:** [#69](https://github.com/shfeekalbhure/TransportERP/pull/69)  
**Production Offline:** `CLOSED` (`sync.offline.enabled=false`)  
**Merge:** `PROHIBITED`

This matrix supersedes earlier G4/Stage 5 status statements only for the implementation tree above. Earlier failing or cancelled runs remain historical evidence and are `STALE` for the implementation candidate; they are not erased.

## 1. Exact-head CI evidence

| Surface | Exact-head result | Preserved evidence |
|---|---|---|
| Core + PostgreSQL 18 + HTTP | `514 passed / 0 failed / 0 skipped` | [run 33024451748 / job 98362445066](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451748/job/98362445066); artifact `9627898268`; digest `sha256:c849db7f2f82b6b1d9d0cf618c89946ef801c14241d99890063e9f82f28eaa5f` |
| Encrypted Offline core | `56 passed / 0 failed / 0 skipped` | [job 98362445072](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451748/job/98362445072); artifact `9627848259`; digest `sha256:7c18befb23a1dd09e85a994311324f8bf09d902934fe6f907aa03e8b7a73c682` |
| Android native security runtime | `SUCCESS`; startup, seed, process restart, verify, key-loss cleanup | [job 98362444862](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451748/job/98362444862); artifact `9627937673`; digest `sha256:6ff235f8a2811af2aa3341170a1095311c0b1340f12908d7654a4fbeb9894bef` |
| Android/mobile builds and contracts | Admin + Customer build; Driver MAUI Android APK and release-manifest guard `SUCCESS` | [job 98362445081](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451748/job/98362445081); artifact `9627897891`; digest `sha256:4386140089d813ce9f377c2bdb13ad5bd69b1b8c70935f6724fa07ee5cabd521` |
| Desktop executable/runtime | WinExe build and closed-default startup smoke `SUCCESS` | [job 98362445205](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451748/job/98362445205); artifact `9627855464`; digest `sha256:d3dde8754b9c5041040891c98f7905ee08e0266a599b444702a873bff8ba1cae` |
| P2 foundation | `SUCCESS` | [run 33024451755](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451755) |
| P2 W0–3 contracts | `SUCCESS` | [run 33024451754](https://github.com/shfeekalbhure/TransportERP/actions/runs/33024451754) |

The exact-head manifests in every artifact bind `actual_sha`, `expected_sha`, and `tree_sha`. The Android evidence was produced on the pinned API-35 emulator, not on a physical device. No iOS runtime result is claimed.

## 2. T-SYNC-001..010 closure

| ID | Executed test(s) | Result and verifiable effect |
|---|---|---|
| `T-SYNC-001` | `Stage4SyncRuntimePostgreSqlTests.Nonce_claim_and_business_replay_are_atomic_and_idempotent`; `Stage5OfflineEndToEndPostgreSqlTests.Encrypted_outbox_reopens_then_nonce_batch_worker_and_status_replay_reach_succeeded` | `PASS`; one accepted server operation, one business result, stored result/version, scoped AuditEvent |
| `T-SYNC-002` | `Stage5OfflineEndToEndPostgreSqlTests.Lost_response_after_acceptance_replays_stable_business_identity_without_duplicate_effect`; `OfflineSyncTransportTests.Timeout_after_send_replays_stable_business_identity_with_new_attempt_and_proof` | `PASS`; new attempt proof with stable business identity; no second business effect |
| `T-SYNC-003` | `Stage4G4HttpNegativePostgreSqlTests.T_SYNC_003_signed_request_with_mismatched_payload_hash_is_rejected_audited_and_has_no_business_effect` | `PASS`; `HASH_MISMATCH`, denial audit, zero business effect |
| `T-SYNC-004` | `Stage4G4HttpNegativePostgreSqlTests.T_SYNC_004_cross_company_cross_branch_and_missing_party_references_are_indistinguishable_and_replay_safe` | `PASS`; uniform `SCOPE_DENIED`, no existence disclosure, request replay cannot bypass scope |
| `T-SYNC-005` | `Stage4SyncRuntimePostgreSqlTests.Stale_base_version_becomes_atomic_typed_conflict_then_keep_server_resolves_it` | `PASS`; typed `CONFLICT`, snapshot provenance, authorized resolution audit, no silent overwrite |
| `T-SYNC-006` | `Stage4G4HttpNegativePostgreSqlTests.T_SYNC_006_posting_and_unavailable_accounting_actions_are_rejected_before_enqueue_and_audited` | `PASS`; posting/finalization remains Online-only; draft-only contract enforced |
| `T-SYNC-007` | `Stage4SyncRuntimePostgreSqlTests.Actual_rate_limited_failures_alone_consume_budget_and_exhaustion_clears_claim`; `OfflineOperationStoreTests.Retry_budget_exhaustion_rejects_without_an_extra_retry` | `PASS`; independent server/client counters, governed backoff, exact exhaustion, no replay counter increment |
| `T-SYNC-008` | `Stage4SyncConflictResolutionPostgreSqlTests.Keep_server_atomically_rejects_original_resolves_conflict_and_writes_metadata_only_audit`; `OfflineSyncTransportTests.Conflict_reapply_survives_timeout_with_stable_replacement_identity_and_fresh_proof` | `PASS`; authorized decision, redacted review, idempotent replacement, metadata-only audit |
| `T-SYNC-009` | `Stage4G4ServerClosurePostgreSqlTests.Distinct_operations_for_same_entity_and_base_version_converge_to_success_and_conflict`; `Stage4SyncRuntimePostgreSqlTests.Execution_claim_race_allows_exactly_one_worker` | `PASS`; one successful aggregate mutation and one explicit conflict; one worker claim |
| `T-SYNC-010` | `OfflineOperationStoreTests.Expired_sending_lease_is_recovered_after_restart_with_new_attempt_identity`; `Stage5OfflineEndToEndPostgreSqlTests.Encrypted_outbox_reopens_then_nonce_batch_worker_and_status_replay_reach_succeeded`; Android native `seed → force-stop/restart → verify` phase | `PASS`; encrypted queue and stable identities survive restart; no loss or duplicate send |

For these rows, request/response, database state, and AuditEvent assertions are executable assertions in the named tests and are preserved in the Core/Offline TRX artifacts. The Android phase JSON additionally records restart and key-loss outcomes without exporting raw secrets.

## 3. Mandatory G4 security and runtime matrix

| Contract area | Executed evidence | Result |
|---|---|---|
| Batch `0/1/100/101`, protocol, partial success | `Stage4SyncActionCatalogTests.Batch_boundaries_have_the_governed_error_code`; `Stage4EffectiveSyncPolicyRuntimeTests.Runtime_batch_enforces_effective_batch_protocol_payload_and_action_limits`; `OfflineSyncTransportTests.Partial_batch_results_are_matched_by_both_stable_operation_identities` | `PASS` |
| Sequential/concurrent replay and every fingerprint field | `Stage4SyncOperationFingerprintV1Tests.FpV1_each_of_fourteen_fields_changes_the_hash_or_is_rejected_by_the_canonical_gate`; `Stage4G4ServerClosurePostgreSqlTests.Replay_mutation_of_each_variable_fingerprint_field_is_rejected_without_extra_effect`; concurrent replay tests | `PASS` |
| Tenant collision isolation | `Stage4G4ServerClosurePostgreSqlTests.Same_device_and_client_operation_text_are_isolated_between_companies`; `Stage4SyncRuntimePostgreSqlTests.Concurrent_business_replay_converges_and_same_client_key_is_isolated_by_tenant` | `PASS` |
| Device suspension/revocation/expiry/assignment and key rotation | `Stage4SyncRuntimePostgreSqlTests.Claim_rechecks_device_assignment_expiry_and_rotated_key_state`; `Stage4ProofKeyLifecycleRuntimePostgreSqlTests.Rotation_wins_device_lock_and_old_key_claim_leaves_no_partial_replay`; Offline transport fail-closed matrix | `PASS` |
| Dispatcher and business idempotency | table-driven `Stage4SyncBusinessDispatcherTests` plus `Stage4SyncBusinessExecutorIntegrationTests.Replayed_business_idempotency_key_returns_prior_result_without_second_effect` | `PASS`; bounded catalog, no generic reflection dispatch |
| Execution/retry worker and restart recovery | `Stage4SyncRuntimePostgreSqlTests.Execution_claim_race_allows_exactly_one_worker`, `Expired_sending_lease_is_recovered_after_restart_without_consuming_retry`, and completion-ambiguity/exhaustion tests | `PASS` |
| Conflict permissions/concurrency/idempotency | `Stage4SyncConflictApiTests`; `Stage4SyncConflictResolutionPostgreSqlTests`, including concurrent resolvers and concurrent reapply | `PASS` |
| 24h/7d/90d retention and redaction | `OfflineOperationStoreTests.Retention_redacts_only_acknowledged_terminal_payloads_at_exact_boundaries`; `Stage4SyncRetentionPostgreSqlTests`, including two-worker cleanup | `PASS`; metadata/hashes retained, raw payload/snapshots not returned |
| Attachment/POD and binary rejection | `Stage4G4HttpNegativePostgreSqlTests.Attachment_and_pod_contracts_remain_metadata_only_and_unapproved_binary_never_reaches_sync_storage` | `PASS`; binary runtime intentionally unavailable |
| PostgreSQL preflight, `Up → Down → Up`, fail-closed Down | `Stage4SyncIdempotencyMigrationPostgreSqlTests`; `Stage4G4MigrationClosurePostgreSqlTests`; conflict-permission migration tests | `PASS`; expected guard exceptions prove fail-closed downgrade behavior |
| Trusted proxies/origin/host spoofing | `Stage4SyncPopDeploymentProfileTests`; `Stage4SyncApiContractTests.Shared_authenticator_rejects_request_host_spoofing_before_nonce_or_claim_state` | `PASS` |
| Secret and redacted-data non-disclosure | `Stage4SyncLoggingRedactionTests`; `OfflineSyncTransportTests.Bearer_nonce_jti_and_proof_are_not_persisted`; Android artifact export guard | `PASS` |
| OperationalParty and client-supplied reference isolation | `Stage4G4HttpNegativePostgreSqlTests.T_SYNC_004...`; `P2C01AWaybillPostgreSqlIntegrationTests`; application scoped lookup plus PostgreSQL trigger/advisory-lock guards | `PASS`; cross-company, cross-branch, missing and replay cases fail uniformly |

## 4. Stage 5 client closure

| Capability | Evidence | Result |
|---|---|---|
| SQLCipher durable outbox and separate read cache | `OfflineOperationStoreTests`; Desktop DPAPI/SQLCipher contract; Android SQLCipher native runtime phase | `PASS` |
| Stable operation identities and fresh attempt identities | enqueue/duplicate/retry tests and end-to-end timeout replay | `PASS` |
| Nonce + request-bound PoP on every HTTP attempt | `OfflineSyncTransportTests.Nonce_challenge_is_followed_by_a_fresh_cryptographically_valid_proof_over_exact_body` and signed nonce refresh test | `PASS` |
| Independent retry budget and backoff | Offline store/transport exhaustion tests | `PASS` |
| Restart/power-loss recovery | Desktop encrypted store reopen E2E; Android process restart evidence | `PASS` |
| Operations/conflict UI | Desktop RTL operations screen and permission-bound actions; Android Driver scope/permission/activation-bound conflict surface | `PASS` for implemented Desktop and Android Driver surfaces |
| OS secure key storage | Desktop CurrentUser certificate/DPAPI handles; Android Keystore `PrivateKeyEntry.PrivateKey`, purpose separation and non-exportability | `PASS` |
| Offline production activation | governed activation requires exact implementation SHA and server runtime; checked-in production/default configuration remains false | `CLOSED BY DESIGN` |

## 5. Evidence interpretation and limitations

- The implementation tree is the immutable code candidate. A later documentation-only commit requires a new exact-head CI run but does not replace the implementation artifacts above.
- Earlier failed runs are retained and marked `STALE/SUPERSEDED`; assertions were not weakened to obtain green CI.
- The Android runtime result is emulator evidence. Physical-device and iOS runtime evidence are `NOT VERIFIED` and are not claimed as part of the approved Android-first scope.
- Binary attachment/POD upload is deliberately not implemented; only metadata/hash actions are allowed. Unsupported actions return the governed `ACTION_RUNTIME_UNAVAILABLE` decision.
- No production deployment, production migration, production secret use, auto-merge, or merge was performed.

## 6. Independent review

The independent read-only reviewer did not author the implementation. Its final classified findings and verdict are recorded in `PR69_FULL_EXECUTION_AND_COMPLETION_REPORT.md`. A GitHub-hosted human approval is not claimed.
