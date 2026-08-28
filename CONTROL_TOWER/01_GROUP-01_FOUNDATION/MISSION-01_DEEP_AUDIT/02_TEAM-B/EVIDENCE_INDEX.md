# TEAM-B Evidence Index

جميع الأدلة مستخرجة باستقلال من TEAM-B. لا يوجد أي اعتماد على TEAM-A.

| Evidence ID | المصدر المباشر | ref/SHA/version | الملاحظة المثبتة | التحقق |
|---|---|---|---|---|
| E-B-001 | git rev-parse/status/log/diff | 8a36f88b56a43cd5b47277b645ba2030ed3da4f1 | الفرع الحاكم clean؛ فرق المنتج عن master@2ec6cccf... = صفر | VERIFIED LOCAL |
| E-B-002 | TransportERP.slnx و10 csproj | governing SHA | 10 projects؛ Desktop/Mobile تتحول إلى Library عند غياب scaffold | VERIFIED STATIC |
| E-B-003 | جميع ملفات C# غير المولدة | governing SHA؛ 20,988 LOC تشمل tests | 3 Domain، 4 API، 4 Application، 14 Contracts، 5 Desktop، 39 Infrastructure، 0/0/0 Mobile، 22 Tests | VERIFIED ENUMERATION/STATIC |
| E-B-004 | Program.cs وAPI modules | governing SHA | 23 endpoint؛ JWT/claims؛ Waybill/Sync/Audit فقط | VERIFIED STATIC |
| E-B-005 | TransportErpDbContext وموديلات P1/P2 | governing SHA | 22 DbSet معلنة، models إضافية، 4 soft-delete filters فقط، لا tenant global filters/RLS | VERIFIED STATIC |
| E-B-006 | Migrations directory | governing SHA | 10 migrations فعلية + 9 designers + snapshot؛ PostgreSQL | VERIFIED STATIC |
| E-B-007 | VoucherLifecycleService.cs وtests | governing SHA | Post يغير status فقط؛ actorId غير مستخدم؛ لا journal posting | VERIFIED STATIC |
| E-B-008 | SyncOperationService.cs وProgram.cs | governing SHA | queue/hash/idempotency/retry/conflict؛ device_registered من claim؛ لا client runtime/dispatcher | VERIFIED STATIC |
| E-B-009 | AuditEventService.cs/migrations | governing SHA | append-only + hash chain؛ hash يغطي subset من حقول الحدث | VERIFIED STATIC |
| E-B-010 | Desktop source/csproj | governing SHA | 16 concrete forms، 19 SHP IDs، لا Program/Application.Run/HttpClient | VERIFIED STATIC |
| E-B-011 | Mobile directories/csproj | governing SHA | صفر ملفات C# في المشاريع الثلاثة؛ build condition ينتج Libraries | VERIFIED STATIC |
| E-B-012 | Test project/source | governing SHA | 101 Fact، 2 Theory، 23 InlineData؛ fail-closed PostgreSQL environment | VERIFIED STATIC |
| E-B-013 | .github/workflows | governing SHA | 7 workflows، checks للبناء/اختبار/migration؛ لا CD/security scan/artifact upload | VERIFIED STATIC |
| E-B-014 | GitHub Actions run 32867082533 | master product SHA 2ec6cccf... | وظيفتا Core+PostgreSQL+HTTP وDesktop contract نجحتا؛ 0 artifact | VERIFIED REMOTE |
| E-B-015 | GitHub PR #69 metadata/reviews/runs | head 939f49fa...؛ run 33129851527 | draft، 198 commit، 203 file، +51286/-858، reviews=0؛ CI FAILURE في Android Release UI E2E/restart، مع 4 jobs ناجحة | VERIFIED REMOTE / DELTA |
| E-B-016 | Git mirror refs | snapshot 2026-08-28 | 50 branches، 0 tags، تباعدات تصل مئات commits | VERIFIED LOCAL MIRROR |
| E-B-017 | git worktree/stash | audit clone | worktree واحد، stash صفر داخل clone فقط | VERIFIED LOCAL / LIMITED SCOPE |
| E-B-018 | documentation/design/04_SCREEN_WORK_QUEUE.csv | governing SHA | 74 rows: 69 DESIGN_APPROVED و5 NON_GOVERNING_LINEAGE؛ 70 screen specs | VERIFIED STATIC |
| E-B-019 | Desktop screen IDs | governing SHA | 19 SHP IDs في source ولا identity/accounting/general setup runtime screens | VERIFIED STATIC |
| E-B-020 | Kurrasa official main file | Library file_00000000a88081f4a753c0b9f06d9fa4، version 72، 783 lines | OFFICIAL BASELINE CANDIDATE؛ NO GENERAL IMPLEMENTATION AUTHORIZATION؛ repo ref 0dd6c9... | VERIFIED PRIMARY LIBRARY |
| E-B-021 | Kurrasa ticket sections/decision file | main v72 + ticket decisions v2 | DEC-TRV-001..006 وTRV contracts تصميم/تفصيل؛ لا تمنح runtime | VERIFIED PRIMARY LIBRARY |
| E-B-022 | Repository supply/deploy scan | governing SHA | لا lockfile/global.json/SBOM/SCA/license scan/deploy/installer/tag/release artifact | VERIFIED REPO; external UNKNOWN |
| E-B-023 | PII field scan | entities/contracts/migrations | email/phone/mobile/identity/address، payload/snapshots/audit JSON نصية | VERIFIED STATIC |
| E-B-024 | toolchain probe | audit environment | dotnet: command not found | VERIFIED ENVIRONMENT |
| E-B-025 | GitHub repository metadata | current query | default master، public؛ branch rules غير معروضة | VERIFIED REMOTE / PARTIAL |

## Direct evidence rules

- وجود مستند أو test أو migration صُنّف كدليل asset فقط.
- نتيجة CI مربوطة بالـexact SHA المرصود، ولا تُنقل إلى PR متغير.
- لا توجد استنتاجات عن Production أو جلسات Codex غير المتاحة.
- الأرقام قابلة لإعادة الحساب بالأوامر المسجلة في التقرير.
