# TEAM-B Domain Coverage Matrix

| المجال | الواقع الحالي على baseline | الحالة | التحقق | أهم فجوة |
|---|---|---|---|---|
| Repository/Git | 551 commit على master، 50 branch، 0 tag | Runtime history / divergent work | VERIFIED | فروع متباعدة وأعمال غير مدمجة كثيرة |
| GitHub/PR | 10 PRs مفتوحة؛ #69 draft متغير وexact-head CI فاشل | Partial governance | VERIFIED REMOTE | لا مراجعات على #69؛ Android Release UI E2E/restart فشل |
| Solution/Projects | 10 projects | Foundation | VERIFIED STATIC | 4 مشاريع عميل ليست executables |
| API | 23 minimal endpoints | Partial Runtime | VERIFIED STATIC + prior CI | يغطي Waybill A/B/C وSync/Audit فقط |
| Domain/Application | Waybill/shipping rules + P1 in-memory baseline | Partial Runtime / Prototype | VERIFIED STATIC | لا نطاق ERP شامل |
| Database | PostgreSQL 18، 10 migrations، 22 DbSet معلنة مع نماذج P2 إضافية عبر Set<T>/model customizers | Foundation/Partial Runtime | VERIFIED STATIC + CI | Production state وrollback وRLS مجهولة |
| Security | JWT resource-server وpermission claims | Partial Runtime | VERIFIED STATIC + HTTP tests evidence | لا current-line login/session/device registry/PoP |
| Multi-tenant | فلاتر company/branch يدوية في الخدمات الرئيسية | Partial | VERIFIED STATIC | لا global tenant filter ولا DB RLS |
| Audit | append-only triggers + hash chain | Partial Runtime | VERIFIED STATIC + CI | hash لا يغطي كل الحقول؛ privacy retention مجهولة |
| Offline/Sync | queue intake، hash/idempotency/conflicts/retries | Foundation | VERIFIED STATIC + CI | لا client store/worker/dispatcher/crypto |
| Desktop | 16 concrete forms، 19 SHP IDs، OutputType=Library | Prototype/Contract | VERIFIED STATIC + CI library build | لا Program ولا API client/navigation |
| Mobile Admin | csproj فقط، 0 C# | Not Implemented | VERIFIED STATIC | لا MAUI runtime |
| Mobile Customer | csproj فقط، 0 C# | Not Implemented | VERIFIED STATIC | لا MAUI runtime |
| Mobile Driver | csproj فقط، 0 C# | Not Implemented | VERIFIED STATIC | لا MAUI runtime |
| Shipping | waybill + pricing/collection + release/trip/allocation/manifest/load/start | Partial Runtime | VERIFIED STATIC + CI | arrival/warehouse/delivery/POD/customs/claims/settlement ناقصة |
| Ticketing | لا code entities/services/endpoints/screens | Not Implemented | VERIFIED STATIC | Kurrasa design contracts فقط |
| Accounting | schema/entities/voucher lifecycle + in-memory prototype | Foundation | VERIFIED STATIC | POSTED لا ينشئ journal، لا GL/reporting/runtime API |
| Screens | 74 queue rows؛ 69 Design Approved و5 lineage؛ 70 specs | Contract/Design | VERIFIED STATIC | ليست runtime؛ current Desktop يغطي subset منفصل |
| Shared Components | Contracts core + ScreenProfile واحد | Foundation | VERIFIED STATIC | لا shell/design system/runtime reuse متكامل |
| Tests | 101 Fact + 2 Theory و23 InlineData | Test assets | VERIFIED STATIC; CI external | local NOT RUN؛ لا mobile/desktop E2E/release UAT |
| CI/CD | 7 workflows؛ master CI green على product SHA | CI partial | VERIFIED REMOTE | لا CD/artifact/signing/security scanning |
| Supply Chain | Actions pinned SHA؛ NuGet versions مباشرة | Partial | VERIFIED STATIC | لا lockfiles/SBOM/SCA/license/provenance |
| Kurrasa | official candidate v72، no general implementation authorization | Contract candidate | VERIFIED LIBRARY | repo ref stale وtraceability drift |
| Privacy | PII وpayload/audit snapshots مخزنة | Partial/Unverified | VERIFIED STATIC | لا evidence encryption/redaction/retention/DSAR |
| Release/Deployment | لا tags/releases/artifacts/deploy manifests | Not Implemented / Unknown external | VERIFIED REPO + REMOTE | لا reproducible release أو rollback |
| Governance/Evidence | Control Tower قائم؛ docs كثيرة | Foundation | VERIFIED | وثيقة لا تساوي تنفيذ؛ multi-reviewer ناقص |
