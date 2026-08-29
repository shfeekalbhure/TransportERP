# DBP-002 exact-head rehearsal trigger

Parent execution state is `f128d24dce7baf76a6ac8af4e62a331b80447311` with tree `7eb7970cdb2349aaefabfa7b8e2d4bdfa5e50501`.

The parent includes the PostgreSQL full-regression connection-variable wiring plus authoritative regression fixtures using `UserMembership`, `UserRoleGrant`, and `UserPermissionGrant Effect=DENY` where required. Semantic catalog reconciliation and fail-closed RLS/resolver fixes remain unchanged.

This file changes no runtime, model, migration, test, database, or acceptance behavior. Its sole purpose is to create an externally authored exact HEAD so GitHub push workflows execute after the GITHUB_TOKEN-authored fixture-fix commit.
