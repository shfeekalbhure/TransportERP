# DBP-002 exact-head rehearsal trigger

Semantic reconciliation fixes are present through `35e39fdb098160a0641743722a996b1fded5381a`, including semantic column-key comparison, normalization of PostgreSQL-generated internal trigger identities, and catalog-based sensitive-policy assertions.

This file changes no runtime, model, migration, test, or database behavior. Its sole purpose is to create an externally authored exact HEAD so GitHub push workflows execute after the one-shot GITHUB_TOKEN fix commit.
