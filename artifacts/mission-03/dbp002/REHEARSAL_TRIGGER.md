# DBP-002 exact-head rehearsal trigger

Parent execution state is `a568c0faece7279b0cbf956935b31be01d21e48d` with tree `ff78959c69af182c531acca4d47bc1acfbbec54c`, including the PostgreSQL full-regression connection-variable wiring.

Semantic reconciliation fixes are present through `35e39fdb098160a0641743722a996b1fded5381a`, including semantic column-key comparison, normalization of PostgreSQL-generated internal trigger identities, and catalog-based sensitive-policy assertions.

This file changes no runtime, model, migration, test, database, or acceptance behavior. Its sole purpose is to create an externally authored exact HEAD so GitHub push workflows execute after the GITHUB_TOKEN-authored parent commit, which GitHub intentionally does not recursively trigger.
