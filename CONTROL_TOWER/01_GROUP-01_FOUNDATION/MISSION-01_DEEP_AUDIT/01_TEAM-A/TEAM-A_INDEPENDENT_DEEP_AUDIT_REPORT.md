# TEAM-A — Independent Deep Audit Report

## 1. التقرير التنفيذي

**الحكم المستقل:** الحالة الحالية لـTransportERP هي **أساس خادمي جزئي ومحدد النطاق للبوالص وبعض المالية والشحن، وليست منظومة ERP مكتملة ولا إصدارًا جاهزًا للنشر**.

الخط الحاكم لهذه المراجعة هو:

`governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

الحقائق الحاكمة:

- الحل يضم **10 Projects بالضبط**، لا 13 ولا عددًا مستنتجًا من الوثائق.
- API وطبقة PostgreSQL يحتويان slice حقيقيًا جزئيًا للبوليصة والتحصيل والتنفيذ حتى `DEPART`، لكن التشغيل على SHA الحاكم لم يُنفذ.
- Desktop مكتبة WinForms منفصلة بلا entry point أو shell أو API client؛ Forms الموجودة prototypes/contracts وليست شاشات تشغيلية.
- Mobile Admin/Customer/Driver هي csproj placeholders فقط، وليست تطبيقات MAUI/Android قابلة للتشغيل.
- Ticketing غير منفذ في Source/DB/API/Desktop؛ الموجود عقود وتصميمات فقط.
- Accounting أساس كيانات ومigrations وخدمة lifecycle جزئية، وليس دورة محاسبية تشغيلية من القيد حتى التقارير.
- Offline/Sync أساس enqueue خادمي جزئي، لا worker/replay ولا local durable queue ولا device proof ولا end-to-end conflict resolution.
- exact-SHA Build/Test = **`UNKNOWN — REQUIRES VERIFICATION`**؛ الرأس الحاكم لا يملك GitHub checks، وبيئة TEAM-A المعزولة لا تحتوي .NET SDK.
- لا يوجد release artifact أو packaging/deployment/rollback/recovery chain مثبت.
- يوجد عيب P0 ثابت في مسار حفظ البوليصة يُسقط `Volume`، وفجوة P1 في ربط مستخدم المزامنة بالشركة/الفرع المدعى به؛ ثقة حقيقة الكود HIGH، أما قابلية الاستغلال الفعلية فتبقى رهينة IdP غير متحقق.
- توجد أعمال محلية غير منشورة ذات commits فريدة وملف dirty يجب حفظها قبل أي cleanup.

**قرار الجاهزية:**

- `READY FOR RELEASE: NO`
- `READY FOR PRODUCTION: NO`
- `CURRENT EXACT-SHA BUILD/TEST STATUS: UNKNOWN — REQUIRES VERIFICATION`
- `CURRENT IMPLEMENTATION CLASS: PARTIAL SERVER FOUNDATION / CONTRACTS / PROTOTYPES`

## 2. خط الأساس والمنهج والاستقلال

قُرئت كاملًا ملفات البداية الإلزامية: `CONTROL_TOWER/README.md`، جميع ملفات الحوكمة ذات العلاقة داخل `00_GOVERNANCE`، أمر المراجعة الرئيسي كاملًا، و`01_TEAM-A/START_ORDER.md`. ثم فُحص الواقع الفعلي: Git، GitHub، solution/projects، source، migrations، tests، workflows، Kurrasa، والنسخ المحلية المتاحة.

لم يُفتح أو يُقرأ أو يُستخدم أي تقرير/Findings/Evidence/Assessment/Recommendation تابع لـTEAM-B. ظهر اسم مسار البداية لفريق TEAM-B مرةً كاسم ملف ضمن جرد diff فقط، دون فتح محتواه؛ لا يوجد خرق استقلال ولا أثر على الحكم.

بدأ clone الرسمي نظيفًا. لم تنفذ TEAM-A أي تعديل على Source أو Tests أو Migrations أو DB، ولم تنفذ merge/rebase/cherry-pick/push. المخرجات الوحيدة المقصودة هي ملفات TEAM-A في هذا المجلد.

## 3. الواقع الفعلي للحل والمشاريع

| # | Project | الوظيفة الفعلية | التصنيف الحالي |
|---:|---|---|---|
| 1 | `TransportERP.Api` | composition root، JWT، و23 route mappings للمزامنة/التدقيق والبوليصة والمالية والشحن | PARTIAL RUNTIME SURFACE؛ التشغيل غير متحقق |
| 2 | `TransportERP.Application` | use cases/ports للبوليصة والمالية والشحن، ومعه baseline in-memory متوازٍ | PARTIAL + PROTOTYPE |
| 3 | `TransportERP.Contracts` | DTOs وعقود مشتركة، بعضها مستخدم وبعضها contract-only | CONTRACT/PARTIAL |
| 4 | `TransportERP.Desktop` | Forms للبوالص فقط؛ csproj يتحول إلى Library لغياب `Program.cs` | PROTOTYPE / DISCONNECTED UI |
| 5 | `TransportERP.Infrastructure` | EF Core/Npgsql، persistence، audit، sync، waybill/finance/shipping، migrations | PARTIAL PERSISTENCE RUNTIME |
| 6 | `TransportERP.Mobile.Admin` | csproj فقط؛ conditional net10 Library | NOT IMPLEMENTED / PLACEHOLDER |
| 7 | `TransportERP.Mobile.Customer` | csproj فقط؛ conditional net10 Library | NOT IMPLEMENTED / PLACEHOLDER |
| 8 | `TransportERP.Mobile.Driver` | csproj فقط؛ conditional net10 Library | NOT IMPLEMENTED / PLACEHOLDER |
| 9 | `TransportERP.Tests` | xUnit static/integration/contract corpus | TEST PROJECT؛ النتائج الحالية NOT RUN |
| 10 | `TransportERP/TransportERP.Domain.csproj` | Domain محدود بثلاثة ملفات لقواعد/aggregate البوليصة والشحن | PARTIAL DOMAIN |

Dependency direction المثبتة: `Application -> Domain + Contracts`; `Infrastructure -> Domain + Application + Contracts`; `Api -> Application + Contracts + Infrastructure`; `Desktop -> Contracts`; Tests -> server projects؛ Mobile بلا references. لا cycle في ProjectReferences. المعمارية الحالية أقرب إلى layered modular monolith جزئية، لكنها ليست ERP شاملًا.

لا توجد `.sln`/`.slnf`، ولا `global.json`، ولا central package management أو lock files. Solution مسطحة بلا logical solution folders.

## 4. الأنظمة والمجالات والشاشات

### 4.1 ما هو موجود

- Waybill foundation: draft/items/parties، validate، submit، approve، return-to-draft، cancel.
- Waybill finance: payment plan، collection، reversal، financial-link reference.
- Shipping execution: release، trip، allocate/unallocate، manifest، load/finalize/handover، start/departure.
- Server audit and sync enqueue foundations.
- PostgreSQL schema/migrations، idempotency and CAS patterns، وبعض append-only controls.

### 4.2 ما هو جزئي أو غير موجود

- Arrival/unload/warehouse/location/transfer/delivery/POD/COD/driver clearance: NOT IMPLEMENTED.
- Logistics returns/claims/customs: NOT IMPLEMENTED؛ approval `:return` ليس shipment return.
- Passenger ticketing/booking/payment/seat/boarding/settlement: NOT IMPLEMENTED في runtime.
- Accounting posting/GL/trial balance/reconciliation/reports: FOUNDATION/PROTOTYPE فقط.
- Mobile runtime/offline storage/security: NOT IMPLEMENTED.
- Release/package/deploy/upgrade/rollback/recovery: NOT EVIDENCED / NOT IMPLEMENTED end-to-end.

### 4.3 الشاشات

Desktop يحتوي 16 Form classes و19 screen definitions، كلها للبوالص. لا executable shell، login/navigation، DI، HTTP client، أو end-to-end save/load. كثير من Forms يرسل events أو يقبل Bind models فقط. لا شاشات Desktop فعلية للمحاسبة أو التذاكر أو الإدارة، ولا Mobile UI أصلًا.

يوجد conflict في screen identity: بعض `SHP-*` في المصدر توصف governed، بينما queue الحالية تسجل بعضها `NON_GOVERNING_LINEAGE` وتضع FLOW01 IDs كسلطة حالية. لا يجوز ربط evidence أو acceptance بهوية قديمة دون crosswalk حاكم.

### 4.4 التكرار والمكونات المشتركة

- تكرار claim/context/permission/error helpers بين Program وثلاثة API modules.
- تكرار RTL/grid/row helpers داخل Forms المختلفة.
- shared UI الفعلي محدود جدًا؛ لا common lookups أو resources أو validation shell.
- `P1InMemoryBaseline` نموذج prototype كبير موازٍ للـEF production model، مستخدم أساسًا في tests، ويجب ألا يقدم كruntime.

## 5. Database, Security, Isolation, Privacy

النموذج يملك نقاطًا إيجابية: PostgreSQL، `timestamptz`، precision constraints، idempotency indexes، CAS version predicate، serializable paths، audit/shipping append-only triggers.

لكن العزل ليس systemic: معظم العلاقات tenant-bearing تستخدم FK على surrogate ID فقط، ولا global tenant query filter أو RLS مثبت، بينما scoping موجود في عدد من service predicates. أي مسار جديد ينسى predicate يمكن أن ينشئ cross-tenant relation لا تمنعها DB.

الـAPI يثق في JWT literal claims، ولا يستخدم persistent RBAC tables كمصدر authorization عند الطلب. لا token issuance/refresh/revocation/device registry/PoP داخل runtime الحالي. جهاز المزامنة مثبت claim-only، وعضوية user للشركة/الفرع المدعى به غير مربوطة داخل `SyncOperationService`.

PII وsync/audit payloads وقيم settings يمكن أن تُخزن نصيًا على مستوى التطبيق. masking للهوية في response control إيجابي، لكنه لا يثبت encryption-at-rest أو backup protection. لا retention/legal hold/purge implementation مثبت.

## 6. Git, GitHub, workspaces والأعمال غير المدمجة

- Remote default: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Audit line: أربعة governance-only commits ahead من master، ولا source delta.
- 50 remote branches، 10 open PRs عند snapshot، no tags، no GitHub releases.
- governance exact SHA: no runs/checks/status contexts.
- master ruleset يمنع delete/non-fast-forward ويطلب thread resolution، لكنه يطلب 0 approvals وفحصين فقط؛ لا mobile/offline required gate.
- PR #69 تحرك أثناء المراجعة من `78b68bea...` إلى `939f49fa...`. الرأس القديم فشل Android native job؛ الرأس الجديد كان CI in-progress بلا final PASS عند snapshot. كل حكم مربوط بالـSHA والزمن.
- PRs #58/#63/#49 وغيرها غير مدمجة؛ نجاحاتها الجزئية لا تصبح current implementation.

تم العثور قراءةً فقط على نسختين محليتين برؤوس غير موجودة على GitHub وcommits patch-unique، ونسخة ثالثة dirty. هذه local-only evidence لا تدخل current architecture، لكنها **P0 preservation** حتى reconciliation.

## 7. Tests, CI/CD, Supply Chain, Release

الجرد الساكن: 101 `[Fact]` + 2 `[Theory]` مع 23 `[InlineData]` = 124 expected cases. منها 34 PostgreSQL/HTTP methods و90 expected non-DB/HTTP cases. لا `Skip=` صريح. هذا جرد، وليس نتيجة تشغيل.

- P1 acceptance register: 203/203 `SPECIFIED_NOT_EXECUTED`.
- P2: 35 + 7 = 42 `READY_FOR_REVIEW`، وليست PASS.
- HISTORICAL CI evidence: run `32862503000` succeeded at `522ccc61...`; zero code/csproj/sln/workflow deltas were found through the audit SHA, but exact-SHA policy prohibits inheriting PASS.
- general CI لا يبني `TransportERP.slnx` كاملًا؛ يبني Tests-referenced server projects، وDesktop كـLibrary منفصل، ولا Mobile.
- لا coverage collection/threshold/artifact retention، ولا packaging/deployment.
- positives: actions pinned to full SHAs و`permissions: contents: read`، direct package versions ثابتة.
- gaps: SDK/transitive/source locks، SBOM، dependency/license/vulnerability review، signing/provenance، immutable image digests، CODEOWNERS/SECURITY/LICENSE evidence.

## 8. Kurrasa reconciliation

قُرئت الكراسة الرسمية الرئيسية كاملة (version 72) ومسودة shipping كاملة (version 9) ومصادر ticketing المستهدفة.

- الكراسة الرئيسية تصنف نفسها candidate مع specification closure open، ولا تمنح general implementation authorization.
- design approved لا يعني runtime pass؛ وتسجل 0/593 runtime-verified في القياس الحاكم الموجود فيها.
- shipping lifecycle draft غني لكنه موسوم صراحة `NON-CANONICAL / NO AUTHORITY PROMOTION`؛ يستخدم لاكتشاف gaps، لا كسلطة API/DDL/permission.
- ticketing decisions/contracts تصميمية محكومة، لكن لا runtime source.
- `OFFLINE_WRITE=0` و`Can Queue=NO` guardrail حاكم؛ current generic sync enqueue لا يثبت أن business offline writes مأذونة.

## 9. Findings التفصيلية

### 9.0 Finding control crosswalk

هذا الجدول جزء حاكم من كل Finding أدناه ويكمل حقولها الإلزامية. الاسم المختصر `audit ref` في التفاصيل يعني حرفيًا:

`refs/heads/governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

أي وصف إضافي في التفاصيل هو qualifier ولا يغير قيمة الـENUM الرسمية في هذا الجدول.

| Finding ID | Domain / specialty | Implementation Status | Verification Status | Temporal | Priority | Confidence | Technical reviewer | Impacted-specialty reviewer | Evidence reviewer |
|---|---|---|---|---|---|---|---|---|---|
| A-ARCH-002 | Architecture / Waybill persistence | PARTIAL | VERIFIED | CURRENT | P0 | HIGH | `/root/team_a_architecture` | `/root/team_a_business_kurrasa` | `/root` |
| A-SEC-002 | Security / Tenant isolation / Sync | FOUNDATION ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_offline_mobile_privacy` | `/root` |
| A-PRES-001 | Git / Local-work preservation | NOT APPLICABLE | VERIFIED | LOCAL-ONLY | P0 | HIGH | `/root/team_a_git_github_ci` | `/root` | `/root/team_a_architecture` |
| A-DB-003 | Database / Multi-tenant isolation | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_architecture` | `/root` |
| A-SEC-001 | Security / RBAC | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_architecture` | `/root` |
| A-DB-004 | Database / RBAC schema | FOUNDATION ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_architecture` | `/root` |
| A-AUD-006 | Audit / Transaction integrity | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_offline_mobile_privacy` | `/root` |
| A-DB-005 | Database / Finance immutability | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_business_kurrasa` | `/root` |
| A-ACCDB-007 | Accounting / Database invariants | FOUNDATION ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_db_security` | `/root/team_a_business_kurrasa` | `/root` |
| A-OFF-001 | Offline/Sync / Execution and versioning | FOUNDATION ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_offline_mobile_privacy` | `/root/team_a_db_security` | `/root` |
| A-OFF-002 | Offline/Sync / Device and queue security | FOUNDATION ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_offline_mobile_privacy` | `/root/team_a_db_security` | `/root` |
| A-RUNTIME-001 | Desktop / Runtime | PROTOTYPE | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_architecture` | `/root/team_a_tests_runtime` | `/root` |
| A-RUNTIME-002 | Mobile / Runtime | NOT IMPLEMENTED | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_offline_mobile_privacy` | `/root/team_a_tests_runtime` | `/root` |
| A-BIZ-001 | Shipping / Operational lifecycle | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_business_kurrasa` | `/root/team_a_architecture` | `/root` |
| A-BIZ-002 | Ticketing and logistics exceptions | NOT IMPLEMENTED | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_business_kurrasa` | `/root/team_a_architecture` | `/root` |
| A-BIZ-005 | Waybill finance / Accounting bridge | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_business_kurrasa` | `/root/team_a_db_security` | `/root` |
| A-QA-001 | QA / Exact-SHA verification | PARTIAL | PARTIALLY VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_tests_runtime` | `/root/team_a_git_github_ci` | `/root` |
| A-QA-002 | QA / Acceptance | CONTRACT ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_tests_runtime` | `/root/team_a_business_kurrasa` | `/root` |
| A-CI-001 | CI / Repository governance | PARTIAL | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_git_github_ci` | `/root/team_a_tests_runtime` | `/root` |
| A-RELEASE-001 | Release / Deployment | NOT IMPLEMENTED | PARTIALLY VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_git_github_ci` | `/root/team_a_tests_runtime` | `/root` |
| A-SUPPLY-001 | Supply chain | PARTIAL | PARTIALLY VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_git_github_ci` | `/root/team_a_tests_runtime` | `/root` |
| A-PRIV-008 | Privacy / Sensitive data | PARTIAL | PARTIALLY VERIFIED | CURRENT | P1 | MEDIUM | `/root/team_a_offline_mobile_privacy` | `/root/team_a_db_security` | `/root` |
| A-SCR-001 | Screens / Governance identity | CONTRACT ONLY | VERIFIED | CURRENT | P1 | HIGH | `/root/team_a_business_kurrasa` | `/root/team_a_architecture` | `/root` |
| A-ARCH-005 | Desktop / API integration | PROTOTYPE | VERIFIED | CURRENT | P2 | HIGH | `/root/team_a_architecture` | `/root/team_a_business_kurrasa` | `/root` |
| A-ARCH-006 | Architecture / Duplication | PARTIAL | VERIFIED | CURRENT | P2 | HIGH | `/root/team_a_architecture` | `/root/team_a_business_kurrasa` | `/root` |
| A-QA-005 | QA / Coverage artifacts | NOT IMPLEMENTED | VERIFIED | CURRENT | P2 | HIGH | `/root/team_a_tests_runtime` | `/root/team_a_git_github_ci` | `/root` |
| A-ARCH-012 | Repository organization | NOT APPLICABLE | VERIFIED | CURRENT | P3 | HIGH | `/root/team_a_architecture` | `/root/team_a_git_github_ci` | `/root` |
| A-DB-INFO-009 | Database / Positive controls | PARTIAL | VERIFIED | CURRENT | INFO | HIGH | `/root/team_a_db_security` | `/root/team_a_business_kurrasa` | `/root` |
| A-KUR-002 | Kurrasa / Offline authority | NOT APPLICABLE | VERIFIED | CURRENT | INFO | HIGH | `/root/team_a_business_kurrasa` | `/root/team_a_offline_mobile_privacy` | `/root` |

### A-ARCH-002 — فقد `Volume` في مسار الحفظ المسجل

- **Observed Fact:** `IWaybillRepository` مسجل إلى `ConcurrencySafeWaybillRepository`. `SaveAsync` يحذف items ويعيد إدراجها، و`ToItemEntity` لا ينسخ `Volume` رغم وجوده في Domain وEntity.
- **Evidence:** A-EV-006؛ `WaybillApiModule.cs`; `ConcurrencySafeWaybillRepository.cs:76-87,119-137`; `WaybillAggregate.cs`; `P2WaybillEntities.cs`; tests cited in register.
- **Project/File/Symbol:** Infrastructure / `ConcurrencySafeWaybillRepository.SaveAsync`, `ToItemEntity`.
- **Branch/ref/SHA:** audit ref / full SHA above.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P0 / HIGH.
- **Qualifier:** direct static data-loss path; runtime reproduction was not run.
- **Impact:** silent loss of physical volume after update; capacity/allocation decisions may be corrupted.
- **Recommendation:** add mapping and PostgreSQL create-update-reload-shipping regression in a separately authorized remediation.
- **What remains unverified:** runtime reproduction and existing affected data.

### A-SEC-002 — عدم ربط مستخدم المزامنة بالـtenant المدعى به

- **Observed Fact:** sync security checks active User by ID/status but does not verify `User.CompanyId/BranchId` against claimed tenant; device registration is trusted from JWT booleans/IDs.
- **Evidence:** A-EV-009؛ `Program.cs:97-108`; `SyncOperationService.cs:346-368`.
- **Project/File/Symbol:** API + Infrastructure / sync endpoint and `EnsureSecurityAsync`.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** FOUNDATION ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** confidence covers the direct code fact; actual exploitability remains conditional on a valid mismatched signed token and unverified IdP claim issuance; attack runtime was not run.
- **Impact:** an issuer/token with mismatched active user and tenant claims can enqueue under another tenant; defense depends on unverified issuer behavior.
- **Recommendation:** enforce user-company-branch binding server-side; add negative integration matrix and device registry/key-bound proof.
- **What remains unverified:** external IdP guarantees and exploitability in deployed topology.

### A-PRES-001 — أعمال محلية غير منشورة معرضة للفقد

- **Observed Fact:** local heads `3bc7f431...` and `7df4743e...` were not found on GitHub and contain patch-unique commits versus PR69; another copy has a dirty tracked PNG.
- **Evidence:** A-EV-030 and `WORKSPACE_PRESERVATION_REGISTER.md`.
- **Project/File/Symbol:** alternative local repositories; dirty `W3-P1-003_RolesPermissions.png`.
- **Branch/ref/SHA:** LOCAL-ONLY snapshots listed in preservation register.
- **Implementation Status:** NOT APPLICABLE.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** LOCAL-ONLY / P0 / HIGH.
- **Qualifier:** Git metadata and patch comparison verified; semantic correctness is unverified.
- **Impact:** cleanup could irreversibly lose remediation/proof work.
- **Recommendation:** owner-controlled immutable snapshot/hash and semantic reconciliation before any deletion.
- **What remains unverified:** authorship/session mapping, quality and whether commits should merge.

### A-DB-003 — العزل العلاقي للـtenant غير شامل داخل DB

- **Observed Fact:** most tenant-bearing FKs use single IDs; no repository RLS/global tenant filter was found; service predicates provide only path-specific scoping.
- **Evidence:** A-EV-007؛ DbContext and P2 model files.
- **Project/File/Symbol:** Infrastructure persistence model.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** static model/path verification; adversarial DB tests were not run.
- **Impact:** missed predicate or direct SQL can create cross-company/branch relationships.
- **Recommendation:** tenant-consistent composite keys/FKs or equivalent invariant/RLS strategy plus bidirectional negative tests.
- **What remains unverified:** production roles/RLS and actual data.

### A-SEC-001 — persistent RBAC ليس مصدر authorization الحالي

- **Observed Fact:** API trusts literal JWT permission/role/company/branch claims; persistent RBAC tables are not resolved at request time.
- **Evidence:** A-EV-004, A-EV-007؛ Program/API modules/DbContext/auth tests.
- **Project/File/Symbol:** API authorization helpers; RBAC entities.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** DB revocations/overrides/status may not affect active requests without external issuer controls.
- **Recommendation:** one authoritative identity/RBAC pipeline with revocation/version semantics and integration tests.
- **What remains unverified:** actual IdP/session policy.

### A-DB-004 — مفاتيح نطاق RBAC غير كافية

- **Observed Fact:** keys for role permissions/user roles/overrides omit company/branch dimensions and lack scope-shape/tenant FKs.
- **Evidence:** A-EV-007؛ `TransportErpDbContext.cs:166-184`; `P1Entities.cs`.
- **Project/File/Symbol:** Infrastructure RBAC model.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** FOUNDATION ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** scoped assignments may collide or reference arbitrary/cross-tenant scope IDs.
- **Recommendation:** settle cardinality, then tenant-aware keys/FKs/checks/tests.
- **What remains unverified:** approved RBAC scope model.

### A-AUD-006 — audit integrity والذرية جزيتان

- **Observed Fact:** audit hash omits several material fields; some business/sync commits occur before a separate audit append, while DB trigger protects live-table update/delete only.
- **Evidence:** A-EV-010؛ `AuditEventService.cs`; sync/waybill persistence paths.
- **Project/File/Symbol:** Infrastructure audit/sync/waybill services.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** failure injection was not run.
- **Impact:** committed business state can lack audit on append failure; exported chain does not protect omitted fields.
- **Recommendation:** atomic transaction/outbox; versioned canonical hash covering material immutable fields while preserving old chain lineage.
- **What remains unverified:** live PostgreSQL behavior and external immutable logging.

### A-DB-005 — finance append-only application-only

- **Observed Fact:** accepted finance records are protected by EF interceptor, but no DB trigger equivalent was found; raw SQL can bypass it.
- **Evidence:** A-EV-011.
- **Project/File/Symbol:** `P2FinanceAppendOnlyInterceptor`; finance migrations.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** raw-SQL mutation test was not run.
- **Impact:** financial history can be rewritten outside tracked EF path.
- **Recommendation:** DB-level append-only control aligned with reversal semantics and least-privilege roles.
- **What remains unverified:** production grants and raw-SQL mutation outcome.

### A-ACCDB-007 — accounting persistence ليس posting runtime آمنًا

- **Observed Fact:** DB does not enforce `TotalDebit=TotalCredit`; tenant-consistent accounting FKs are incomplete; no persistent posting API/service; voucher `actorId` is unused and transitions do not create audit/journal.
- **Evidence:** A-EV-007, A-EV-015.
- **Project/File/Symbol:** DbContext journal mappings; `VoucherLifecycleService`.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** FOUNDATION ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** future/direct persistence can admit unbalanced or cross-tenant accounting state.
- **Recommendation:** transactional double-entry posting invariant, tenant/period/actor controls, immutable reversal/audit before exposure.
- **What remains unverified:** any external accounting implementation.

### A-OFF-001 — لا توجد مزامنة end-to-end

- **Observed Fact:** API enqueues and returns `QUEUED`; no production worker/dispatcher/executor/replay path was found. BaseVersion is accepted then discarded; ResultVersion is not assigned.
- **Evidence:** A-EV-009.
- **Project/File/Symbol:** sync endpoint, `SyncOperationService`, sync entities.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** FOUNDATION ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** end-to-end runtime was not run.
- **Impact:** queued operations cannot be proven to execute/recover/conflict-resolve; version safety is incomplete.
- **Recommendation:** governed allowlist/typed handlers, durable worker, version persistence, retry/conflict/status APIs and restart tests before enabling offline writes.
- **What remains unverified:** external workers outside repo.

### A-OFF-002 — device/queue/audit controls ناقصة

- **Observed Fact:** no device registry/revocation/PoP; operation/entity/payload are generic without governed allowlist/schema; payload size/clock bounds are weak; enqueue and audit are non-atomic; duplicate key is global by device/client operation.
- **Evidence:** A-EV-009.
- **Project/File/Symbol:** API sync records and `SyncOperationService`.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** FOUNDATION ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** replay/collision/authorization/audit risks increase before any real executor is attached.
- **Recommendation:** device key binding, tenant-aware idempotency, schema/size/version rules and atomic audit.
- **What remains unverified:** gateway/device controls outside repo.

### A-RUNTIME-001 — Desktop ليس runtime executable

- **Observed Fact:** no `Program.cs`; OutputType is Library; no shell/navigation/API client; Tests do not reference Desktop.
- **Evidence:** A-EV-012, A-EV-026.
- **Project/File/Symbol:** Desktop csproj and all Waybill Forms.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PROTOTYPE.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** runtime was not run.
- **Impact:** Forms cannot substantiate working screens.
- **Recommendation:** retain prototype classification until executable composition and UI/API smoke evidence exist.
- **What remains unverified:** possible external host absent from repo.

### A-RUNTIME-002 — Mobile projects placeholders

- **Observed Fact:** each Mobile directory contains csproj only; conditional settings yield net10 Library with no MAUI scaffold/source/tests/CI.
- **Evidence:** A-EV-013.
- **Project/File/Symbol:** Mobile Admin/Customer/Driver csproj.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** NOT IMPLEMENTED.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** no mobile app, local queue, secure storage, signing or device integration.
- **Recommendation:** do not classify executable; authorize separate scaffold/runtime/security/testing work.
- **What remains unverified:** future/unmerged mobile branches.

### A-BIZ-001 — shipping يتوقف عند DEPART

- **Observed Fact:** current commands end at start trip/departure; arrival/unload/warehouse/delivery/POD/COD/clearance are absent.
- **Evidence:** A-EV-014 and P2 scope documents.
- **Project/File/Symbol:** Shipping API/application/persistence/entities.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** server surface verified statically; runtime was not run.
- **Impact:** custody/shipment lifecycle cannot close end-to-end.
- **Recommendation:** authorize/design subsequent increments with invariant/audit/idempotency acceptance evidence.
- **What remains unverified:** actual runtime concurrency and integration.

### A-BIZ-002 — returns/claims/customs وTicketing غير منفذة

- **Observed Fact:** Hold entity is a blocker foundation without CRUD; no logistics return/claim/customs runtime. No ticket/passenger/booking/seat/boarding source, tables, endpoints or forms.
- **Evidence:** A-EV-014, A-EV-016, A-EV-020.
- **Project/File/Symbol:** repository-wide domain/API/DB/Desktop inventory.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** NOT IMPLEMENTED.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** ticket documents are contract/design evidence only; verification is bounded to the audited ref.
- **Impact:** major transport ERP domains are unavailable.
- **Recommendation:** preserve approved decisions but require canonical mappings/programming authority before implementation.
- **What remains unverified:** local-only/unmerged domain work not on current ref.

### A-BIZ-005 — collection لا تنشئ accounting posting

- **Observed Fact:** finance path may validate a pre-existing accounting reference and link it, but does not create/post balanced accounting documents; derived financial status lacks GET endpoint.
- **Evidence:** A-EV-015؛ Finance API/application/persistence.
- **Project/File/Symbol:** Waybill finance module/services.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** collections can remain operationally disconnected from ledger/settlement.
- **Recommendation:** idempotent posting/clearing contract or governed pre-posted-reference/reconciliation model.
- **What remains unverified:** downstream external jobs.

### A-QA-001 — لا Build/Test على exact SHA

- **Observed Fact:** audited SHA has no GitHub checks; isolated clone could not run .NET because SDK/runtime absent.
- **Evidence:** A-EV-022, A-EV-027.
- **Project/File/Symbol:** repository-wide verification state.
- **Branch/ref/SHA:** exact audit SHA.
- **Implementation Status:** PARTIAL.
- **Verification Status:** PARTIALLY VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** check absence is verified; build/test outcome is unverified.
- **Impact:** no current PASS claim is valid.
- **Recommendation:** approved disposable .NET10/PostgreSQL18 + Windows exact-SHA build/test/migrate evidence.
- **What remains unverified:** execution/discovery outcome for the 124 statically expected cases, migrations, API start and Desktop build.

### A-QA-002 — acceptance غير مغلق

- **Observed Fact:** P1 203/203 specified-not-executed; P2 42 ready-for-review; validators test contract structure, not production acceptance.
- **Evidence:** A-EV-026.
- **Project/File/Symbol:** P1/P2 acceptance registers and conformance tests.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** CONTRACT ONLY.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** release/phase closure cannot be inferred from registers/tests existing.
- **Recommendation:** execute numbered cases against exact runtime and retain request/response/audit/UI evidence.
- **What remains unverified:** all production acceptance outcomes.

### A-CI-001 — required CI coverage غير كافية

- **Observed Fact:** the audited governance-branch SHA had no checks/status contexts at query time. Tracked push triggers cover `master` or named feature branches, while `pull_request`/`workflow_dispatch` could run only when separately invoked/applicable. General CI does not build full solution/mobile; master rules require only core and Desktop-library checks with zero approvals.
- **Evidence:** A-EV-021, A-EV-022, A-EV-025.
- **Project/File/Symbol:** `.github/workflows`; GitHub ruleset.
- **Branch/ref/SHA:** CURRENT rules + audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** security/offline/mobile changes can lack governed required gates/independent approval.
- **Recommendation:** exact-SHA governed matrix and approvals covering every release-relevant surface.
- **What remains unverified:** organization-level controls.

### A-RELEASE-001 — لا release/deployment chain

- **Observed Fact:** zero tags/releases; no publish/package/deploy workflows, installers/mobile artifacts/signing/config/provisioning/rollback/restore evidence.
- **Evidence:** A-EV-028 plus GitHub tags/releases.
- **Project/File/Symbol:** repository/release configuration inventory.
- **Branch/ref/SHA:** audit ref/current repository.
- **Implementation Status:** NOT IMPLEMENTED.
- **Verification Status:** PARTIALLY VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** repository absence is verified; external deployed state is access blocked.
- **Impact:** no source-to-signed-artifact-to-deployment provenance or recovery basis.
- **Recommendation:** block release-ready claim; define version/tag, reproducible signed artifacts, provisioning, DB upgrade/rollback/restore and runbooks.
- **What remains unverified:** any external deployment.

### A-SUPPLY-001 — supply-chain reproducibility غير مكتملة

- **Observed Fact:** actions and direct versions are pinned positively, but SDK/transitives/sources/images are not fully locked; no vulnerability/license/SBOM/signing/provenance gates.
- **Evidence:** A-EV-021, A-EV-028.
- **Project/File/Symbol:** csproj/workflows/repository config.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** PARTIALLY VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Qualifier:** configuration gaps are verified; current vulnerability/license state is unverified.
- **Impact:** builds are less reproducible and dependency risk cannot be attested.
- **Recommendation:** exact-SHA restore/audit, SDK/package/source/image locks, dependency review, SBOM and provenance aligned to release plan.
- **What remains unverified:** actual vulnerabilities/licenses.

### A-PRIV-008 — sensitive data/retention controls غير مثبتة

- **Observed Fact:** identity/mobile/address, sync/conflict payloads, audit JSON and secret-marked setting values can be stored as text; audit endpoint returns broad before/after data; no retention/legal-hold/purge implementation found.
- **Evidence:** A-EV-007, A-EV-009, A-EV-010.
- **Project/File/Symbol:** P1/P2 entities, audit API/service, global settings.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** PARTIALLY VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P1 / MEDIUM.
- **Qualifier:** tracked application surfaces are verified; infrastructure encryption/retention is access blocked, hence MEDIUM confidence for end-to-end privacy impact.
- **Impact:** DB/export/backup exposure and over-retention risk.
- **Recommendation:** classification/minimization, redaction, key-managed encryption where required, access/export/retention/legal-hold controls and tests.
- **What remains unverified:** disk/backup encryption, populated data and governing law.

### A-SCR-001 — screen authority identity conflict

- **Observed Fact:** current source IDs conflict with current FLOW01/legacy queue authority for known SHP IDs.
- **Evidence:** A-EV-017.
- **Project/File/Symbol:** Desktop screen catalogs and design queue/specs.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** CONTRACT ONLY.
- **Verification Status:** VERIFIED.
- **Qualifier:** identity conflict is verified for the cited IDs only.
- **Temporal / Priority / Confidence:** CURRENT / P1 / HIGH.
- **Impact:** evidence, acceptance and future work can bind to wrong screen identity.
- **Recommendation:** authoritative crosswalk/owner decision before UI continuation.
- **What remains unverified:** SHP-015..030 final mapping.

### A-ARCH-005 — UI/API read/write integration gaps

- **Observed Fact:** Desktop forms emit events/bind models without request extraction/service wiring; shipping/finance lack required read projections/endpoints for several screens.
- **Evidence:** A-EV-004, A-EV-012.
- **Project/File/Symbol:** Desktop Forms and Finance/Shipping API modules.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PROTOTYPE.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P2 / HIGH.
- **Impact:** even an executable shell would not yield integrated screen flows.
- **Recommendation:** Screen→query/command→contract trace matrix and controller/client tests.
- **What remains unverified:** undocumented external client path.

### A-ARCH-006 — duplication/shared-component debt

- **Observed Fact:** repeated RTL/grid/form and API claim/context/error helpers; no common lookups/resources.
- **Evidence:** A-EV-004, A-EV-012.
- **Project/File/Symbol:** API modules and Desktop Waybill Forms.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P2 / HIGH.
- **Impact:** authorization/UI behavior drift and maintenance duplication.
- **Recommendation:** preserve behavior with tests, then evaluate shared filters/context/error mapper and reusable RTL UI components.
- **What remains unverified:** desired design-system authority.

### A-QA-005 — لا coverage/artifact retention gate

- **Observed Fact:** coverlet package exists but workflows do not collect/threshold/upload coverage or retain TRX; historical run shows no artifacts.
- **Evidence:** A-EV-021, A-EV-023, A-EV-026.
- **Project/File/Symbol:** Tests csproj/workflows.
- **Branch/ref/SHA:** audit ref + HISTORICAL run.
- **Implementation Status:** NOT IMPLEMENTED.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P2 / HIGH.
- **Impact:** no reproducible coverage metric or retained raw test evidence.
- **Recommendation:** Cobertura/thresholds plus SHA-bound immutable artifacts and retention policy.
- **What remains unverified:** actual coverage percentage.

### A-ARCH-012 — تنظيم مسار Domain

- **Observed Fact:** Domain csproj resides physically at `TransportERP/TransportERP.Domain.csproj` while other projects use named folders; solution is flat.
- **Evidence:** A-EV-003.
- **Project/File/Symbol:** solution/project layout.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** NOT APPLICABLE.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / P3 / HIGH.
- **Impact:** clarity only; no proven runtime defect.
- **Recommendation:** do not rename/move now; any C2 change requires history/reference preservation.
- **What remains unverified:** target repository-layout decision.

### A-DB-INFO-009 — controls worth preserving

- **Observed Fact:** CAS, idempotency, serializable transaction paths, precision/status constraints and audit/shipping DB append-only triggers exist.
- **Evidence:** A-EV-007 through A-EV-011.
- **Project/File/Symbol:** Infrastructure persistence/migrations/tests.
- **Branch/ref/SHA:** audit ref.
- **Implementation Status:** PARTIAL.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / INFO / HIGH.
- **Qualifier:** positive controls verified statically; runtime was not run.
- **Impact:** remediation that rewrites lineage could lose valuable safety properties.
- **Recommendation:** preserve and regression-test these controls.
- **What remains unverified:** execution on exact SHA.

### A-KUR-002 — offline write authority

- **Observed Fact:** Kurrasa states `OFFLINE_WRITE=0` and `Can Queue=NO`.
- **Evidence:** A-EV-018.
- **Project/File/Symbol:** official Kurrasa main, version 72.
- **Branch/ref/SHA:** current Library version; applies as governance evidence, not source SHA.
- **Implementation Status:** NOT APPLICABLE.
- **Verification Status:** VERIFIED.
- **Temporal / Priority / Confidence:** CURRENT / INFO / HIGH.
- **Qualifier:** current Library version 72; this is a governance guardrail and a contradiction would require a separate P1 finding.
- **Impact:** generic queue foundation must not be construed as permission for business offline writes.
- **Recommendation:** preserve server-authoritative financial/approval operations until separate authorization.
- **What remains unverified:** future authority changes.

## 10. Priority roll-up

### P0

1. Correct/prevent silent `Volume` loss, with data-impact assessment.
2. Preserve/hash/reconcile local-only heads and dirty evidence before cleanup.

### P1

Security/isolation/RBAC/device/audit/accounting invariants; end-to-end sync; Desktop/Mobile runtime absence; shipping/ticketing/accounting gaps; exact-SHA tests and acceptance; CI rules; release/deployment; supply chain; privacy; screen identity reconciliation.

### P2

UI/API integration/read-model gaps; duplication/shared components; coverage/artifact evidence; warning/maintenance debt.

### P3

Repository/solution organization only, with no current rename authorization.

### INFO / N/A

Preserve positive DB/idempotency/CAS/append-only controls; Kurrasa offline authority is a guardrail; clean Git state and historical CI are context, not runtime PASS.

## 11. ما يجب الحفاظ عليه

- audit SHA، master baseline، وكل SHA-bound CI evidence.
- PR69 old/new heads and open PR branches until reconciliation.
- local-only unique commits and dirty PNG.
- migration lineage/snapshot/manual migration.
- audit hash lineage and DB append-only triggers.
- CAS/idempotency/serializable transaction patterns and PostgreSQL tests.
- Kurrasa separation between discussion/design/programming authority.
- screen-lineage reconciliation evidence.

التفاصيل في `WORKSPACE_PRESERVATION_REGISTER.md`.

## 12. المجهولات والموانع

المجهولات الحاكمة تشمل exact-SHA build/test/migrate، live DB/data/roles/RLS، IdP/session/revocation، TLS، encryption/backups/restore، retention/legal hold، production deployment، other Codex sessions، other developer machines، full Kurrasa corpus، full tenant negative matrix، finance raw-SQL immutability. لا واحد منها عومل كPASS.

التفاصيل في `UNKNOWN_AND_BLOCKERS_REGISTER.md`.

## 13. تصريح الاستقلال والإغلاق

`INDEPENDENCE DECLARATION: THIS TEAM DID NOT READ OR RELY ON THE OTHER INDEPENDENT TEAM'S INITIAL REPORT BEFORE SEALING ITS OWN INITIAL REPORT.`

`TEAM-A ALSO DECLARES THAT IT DID NOT READ OR RELY ON TEAM-B REPORTS, FINDINGS, EVIDENCE INDEXES, ASSESSMENTS, OR RECOMMENDATIONS.`

لم يقع خرق استقلال. هذا التقرير مستقل ومبني على الأدلة التي تحقق منها TEAM-A. لا يبدأ التقرير TEAM-D أو أي مهمة لاحقة، ولا يمنح authorization لإصلاح أو merge أو release. مخرجات TEAM-A جاهزة للتسليم إلى Control Tower فقط بعد استكمال manifest والختم cryptographic.
