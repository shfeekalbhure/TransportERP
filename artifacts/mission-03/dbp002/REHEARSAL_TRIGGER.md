# DBP-002 exact-head rehearsal trigger

Semantic reconciliation fixes are present through parent `8a453b6d46038141e2abe85b2c88e858ee944a3b`, including semantic column-key comparison and normalization of PostgreSQL-generated internal trigger identities.

This file changes no runtime, model, migration, test, or database behavior. Its sole purpose is to create an externally authored exact HEAD so GitHub push workflows execute after the one-shot GITHUB_TOKEN fix commit.
