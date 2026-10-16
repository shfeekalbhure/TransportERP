# Dependency remediation — 2026-10-16

Base: `05e3bd802f649168cce4522679116b1e739efede`.

Confirmed P1: `C:\Users\shfee\.codex\worktrees\31c6\TransportERP\TransportERP.Infrastructure\TransportERP.Infrastructure.csproj:12` explicitly referenced `System.Security.Cryptography.Xml` 10.0.10, resolving `System.Security.Cryptography.Pkcs` 10.0.10 transitively. Whole-solution restore emitted NU1902/NU1903 warnings for five advisories. Cause: pinned package predates security patches.

Fix: raise Xml to 10.0.13. Microsoft advisories identify 10.0.13 as patched for [Pkcs](https://github.com/advisories/GHSA-846f-f764-76mx) and [Xml](https://github.com/advisories/GHSA-r955-8q5j-3h83). No suppression of warnings or test changes.

Executed verification:

- `.NET SDK 10.0.400`: solution restore after update exited 0 without NU1902/NU1903; local log `artifacts/company-restore.log`.
- `dotnet list TransportERP.slnx package --vulnerable --include-transitive --no-restore` exited 0; all 14 projects reported no vulnerable packages against configured sources. Local log `artifacts/company-package-audit.log`.
- This package scan does not certify the installed runtime, Visual Studio, production deployments, or future advisory state. Final build/test results belong to TEAM06.

Baseline full Release build before final patch verification: 0 errors, 649 warnings. These include existing generated-code nullable/unused-field warnings and xUnit analyzer warnings. They were not disabled; the final QA log is authoritative for the changed tree.
