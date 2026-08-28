# W2-F2 Security Test Matrix

- State: `B2B CODE-ONLY PASS — PERSISTENCE/DEVICE/EXECUTABLE-CLIENT PORTION BLOCKED`
- Exact candidate: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- Disposable run: `33191269475`

| Required behavior | Exact test/evidence | Current disposition |
|---|---|---|
| valid login | `Valid_login_creates_session_and_issues_tokens` | `PASS — run 33191269475` |
| invalid credentials | `Invalid_credentials_fail_closed_and_clear_client_credentials` | `PASS — run 33191269475` |
| disabled user | `Disabled_user_is_denied` | `PASS — run 33191269475` |
| wrong tenant membership | `Wrong_tenant_membership_is_denied` | `PASS — run 33191269475` |
| issued token accepted | `Issued_access_token_is_accepted...` plus cryptographic JWT validation test | `PASS — run 33191269475` |
| expired token denied | `Expired_access_token_is_denied` | `PASS — run 33191269475` |
| revoked session denied | `Revoked_session_is_denied` | `PASS — run 33191269475` |
| refresh rotation | `Refresh_rotates_one_time_token_and_preserves_family` | `PASS — run 33191269475` |
| refresh reuse family revoke | `Reused_refresh_token_revokes_entire_family` | `PASS — run 33191269475` |
| expired refresh | `Expired_refresh_token_is_denied_and_family_revoked` | `PASS — run 33191269475` |
| logout then access/refresh denied | `Logout_denies_access_and_refresh` | `PASS — run 33191269475` |
| role/permission revoked after issue | existing `Enqueue_rejects_claim_only_permission_and_stored_membership_mismatch`; issuer omits authority claims | `PASS — full exact-head regression` |
| membership revoked after issue | `Revoked_membership_after_issue_revokes_family` | `PASS — run 33191269475` |
| stale security version | `Stale_security_version_is_denied` | `PASS — run 33191269475` |
| concurrent refresh race | `Concurrent_refresh_allows_at_most_one_rotation_and_revokes_family` | `PASS — run 33191269475` |
| cross-company session misuse | `Cross_company_session_misuse_is_denied` plus existing HTTP/store negatives | `PASS — run 33191269475` |
| device mismatch/revoked device | device mismatch code-only test; registry revoke requires C2 persistence | mismatch `PASS`; registry blocked |
| replay/nonce | refresh-token reuse covered separately; device nonce persistence | device nonce blocked by DBP-003/006 |
| unauthorized lifecycle/override audit | owner checks exact prior PASS; registry override audit | owner PASS; registry blocked |
| Offline submission after revoke | `Offline_submission_after_revoke_is_denied_and_suspended` | `PASS — run 33191269475` |
| claim cannot widen persistent RBAC | JWT has no permission/role claims; existing persistent-RBAC denial tests | `PASS — full exact-head regression` |
| Desktop/Mobile credential clearing | contract defined as `ClearAndSuspendOffline` | executable clients absent; W5/client evidence blocked |

PASS will be recorded only from the exact-head run. This matrix does not substitute the test-only in-memory store for DBP-003 durable PostgreSQL validation.
