# TransportERP — سجل اختبارات القبول وحوكمة P1
**دور الملف:** المرجع التشغيلي الموحد للحالة والبوابات والأدلة الخاصة بـ`PR #69`، مع إبقاء تفاصيل العقود في ملفاتها الأصلية وعدم نسخها هنا.

**آخر تحديث جوهري:** `2026-08-26 02:56 Asia/Riyadh`

**الحالة التشغيلية الحالية:** `PHASES_1_3_IMPLEMENTED_AND_CI_VERIFIED — PREPARING_INDEPENDENT_REVIEW`.

**حالة سجل الاختبارات التعاقدي ذي 203 حالة أدناه:** `SPECIFIED_NOT_EXECUTED` ما لم يربط صفه صراحةً بدليل تشغيل؛ لا يساوي نجاح مجموعة CI ذات 185 اختبارًا تنفيذ جميع حالات القبول التعاقدية البالغ عددها 203.

## 0. مركز قيادة PR #69 وحزمة إغلاق المراحل 1–3

### 0.1 المرجع والنطاق الثابت

اعتمد هذا الملف داخل المسار القائم `documentation/closeout/P1/` مرجعًا موحدًا بدل إنشاء `docs/command-center/` موازٍ. تبقى المواد الخام التاريخية في `evidence/`، ويبقى `P1_SYNC_CONTRACT.md` العقد الفني الحاكم، ولا تحل هذه الفهرسة محل أي تقرير مراجعة مستقل أو اعتماد مالك.

| العنصر | القيمة المثبتة |
|---|---|
| الفرع | `codex/p1-security-device-sync-offline-20260825` |
| PR | [#69](https://github.com/shfeekalbhure/TransportERP/pull/69) — `OPEN`, `DRAFT`, `UNMERGED` |
| base | `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` |
| مرشح تنفيذ المراحل 1–3 | [`5ca9a86acef4053cf731fb896ca0c77b17a575ae`](https://github.com/shfeekalbhure/TransportERP/commit/5ca9a86acef4053cf731fb896ca0c77b17a575ae) |
| النظير المحلي | `d0860d70d808374bd5582d2e71c14afa5429f8cd` |
| tree المشترك | `9ce37459c22c6f4c47beee203af0fa9d0a167080`؛ تطابق remote/local حرفيًا |
| نطاق التغيير | `6` commits بعيدة، `48` ملفًا؛ يعاد إنتاج manifest بالأمر `git diff --name-status 2ec6ccc..5ca9a86` |
| الدمج | `NOT AUTHORIZED`; لم يتغير `master` ولم يصبح PR جاهزًا للدمج |

مرشح المراجعة المستقلة للمراحل 1–3 هو **commit التنفيذ البعيد `5ca9a86...` وtree المحدد أعلاه**. أي تحديث توثيقي لاحق على PR لا يغير هذا المرشح تلقائيًا؛ يجب بيان الفرق والتأكد أنه توثيقي فقط، أو تعيين مرشح تنفيذ جديد وإعادة الأدلة عليه.

### 0.2 فصل حالات السلطة

| النطاق | `IMPLEMENTED` | `VERIFIED BY CI` | `INDEPENDENTLY REVIEWED` | `OWNER APPROVED` |
|---|---|---|---|---|
| المرحلة 1 — الهوية والتفويض والتدقيق الأساسي | نعم، ضمن المرشح | نعم، ضمن CI الأخضر النهائي | لا؛ الحزمة قيد التجهيز للمراجع المستقل | لا يوجد اعتماد نهائي للـexact SHA |
| المرحلة 2 — العزل والتدقيق والمعاملات الذرية | نعم، ضمن المرشح | نعم، ضمن CI الأخضر النهائي | لا؛ الحزمة قيد التجهيز للمراجع المستقل | لا يوجد اعتماد نهائي للـexact SHA |
| المرحلة 3 — حدود ثقة الجهاز المسجل | نعم، ضمن المرشح | نعم، ضمن CI الأخضر النهائي | لا؛ الحزمة قيد التجهيز للمراجع المستقل | لا يوجد اعتماد نهائي للـexact SHA |
| المرحلة 4 — `TransportERP Sync-PoP v1` | لا؛ `CONTRACT_WIP` فقط | لا | مراجعة تصميم داخلية لا تساوي مراجعة مستقلة | تفويض صياغة قرارات فقط؛ لا G4/G5 ولا Offline |
| المرحلة 5 | `NOT AUTHORIZED` | لا | لا | لا |

لا تستبدل حالةٌ حالةً أخرى. نجاح CI لا يعني مراجعة مستقلة، والمراجعة المستقلة لا تعني اعتماد المالك، واعتماد القرار التعاقدي لا يعني وجود runtime.

### 0.3 سجل التنفيذ القابل للتتبع

| المرحلة | commit البعيد | النظير المحلي/الغرض | النتيجة |
|---|---|---|---|
| 1 وبداية 2 | `a08ee5860fbf263b82ad4073307ce9775a8a98be` | `ddebd9e` + `70735c0`: قرار G3، bootstrap، identity، authz، audit | نُفذ؛ ظهرت علة عقد 403 في CI |
| 2 | `da0e482c29589fa563d0e903ebc484ff4fa3907a` | `88b364b`: تحديث رؤوس stream المقفلة | نُفذ؛ بقي فشل عقد 403 |
| 2 | `cfb75c1378717e7cb8f38f382774d615715b3088` | `a2baa68`: ذرية batch وتنظيف السياق وعقد denial | إغلاق أخضر `162/162` |
| 3 | `3cd32ce2b04f0686abca4e1ce30456fef4a1dff4` | `7b1b2d4`: trusted registered device boundary | نُفذ؛ فشل اختباران لتصادم currency seed |
| 3/CI | `90cc841fca714c25aed8f4fc2fe33c5ea8303869` | `a361ebb`: allocator ذري موحد لاختبارات PostgreSQL | أزال التصادم؛ كشف خطأ استدعاء lock وأدى إلى 70 فشلًا |
| 3/CI | `5ca9a86acef4053cf731fb896ca0c77b17a575ae` | `d0860d7`: تنفيذ advisory lock كـnon-query | الإغلاق الأخضر النهائي `185/185` |

### 0.4 سجل CI — النجاح والفشل محفوظان

**الدليل الأخضر النهائي على نفس مرشح التنفيذ:** [CI run 32908493564](https://github.com/shfeekalbhure/TransportERP/actions/runs/32908493564)، انتهى `SUCCESS` في `2026-08-26 01:58:13 Asia/Riyadh` على `5ca9a86...`.

| البوابة | الدليل | النتيجة |
|---|---|---|
| Core + PostgreSQL + HTTP | [job 97997620157](https://github.com/shfeekalbhure/TransportERP/actions/runs/32908493564/job/97997620157) | validators P0/P1 وP2-C01، restore/build Release، EF pending-model clean، تطبيق migrations على PostgreSQL 18، ومجموعة fail-closed كاملة: `185 passed / 0 failed / 0 skipped` في 35s |
| Desktop RTL | [job 97997619963](https://github.com/shfeekalbhure/TransportERP/actions/runs/32908493564/job/97997619963) | `SUCCESS` |
| P2 foundation | [run 32908493566](https://github.com/shfeekalbhure/TransportERP/actions/runs/32908493566) | `SUCCESS`؛ job `97997619701` |
| P2 W0-3 contracts | [run 32908493452](https://github.com/shfeekalbhure/TransportERP/actions/runs/32908493452) | `SUCCESS`؛ job `97997619088` |
| A/B/C/W0-5 | path-filtered | `SKIPPED` بحسب المسارات، وليس PASS مزعومًا |

الأثر التاريخي غير مطموس:

- [`a08ee586` / run 32888563344](https://github.com/shfeekalbhure/TransportERP/actions/runs/32888563344): `159/160`؛ وفشل foundation وW0-3 لأن اختبار عقد HTTP حاول قراءة JSON من استجابة `403` فارغة.
- [`da0e482c` / run 32889572757](https://github.com/shfeekalbhure/TransportERP/actions/runs/32889572757): `160/161`؛ العلة نفسها. أغلقها `cfb75c13` بإرجاع عقد JSON ثابت `SCOPE_DENIED` مع correlation، ثم نجح [run 32890912940](https://github.com/shfeekalbhure/TransportERP/actions/runs/32890912940) بـ`162/162`.
- [`3cd32ce2` / run 32902855973](https://github.com/shfeekalbhure/TransportERP/actions/runs/32902855973): `183/185`؛ فشلان `23505 IX_currencies_Code` بسبب مساحة seed ذات 4096 احتمالًا. عولجت بمخصص sequence ذري في `90cc841f`.
- [`90cc841f` / run 32907492683](https://github.com/shfeekalbhure/TransportERP/actions/runs/32907492683): `115/185`؛ 70 فشلًا لأن `pg_advisory_xact_lock` يعيد void/null واستُدعي كـscalar. عولج في `5ca9a86` باستخدام non-query داخل المعاملة نفسها.

### 0.5 فهرس أدلة إغلاق المراحل 1–3

لا تُنسخ قوائم الملفات الكاملة هنا؛ الـcommit range هو manifest الحاكم. نقاط الدخول للمراجع:

- الهوية والمصادقة والصلاحيات: `TransportERP.Api/Identity/`, `TransportERP.Api/Security/`, و`TransportERP.Infrastructure/Persistence/IdentitySecurityServices.cs`.
- العزل والتدقيق والمعاملات: `CurrentSecurityContext.cs`, `PermissionAuthorization.cs`, `AuditEventService.cs`, `SyncOperationService.cs` وخدمات Waybill persistence المتغيرة في manifest.
- دورة الجهاز: `RegisteredDeviceService.cs`, `RegisteredDeviceApiModule.cs`, العقود في `TransportERP.Contracts/Identity/`، و`DeviceTrustResolver.cs`.
- المخطط والمهاجرات: `20260825220000_P1SecurityIdentity.cs`, `20260826010000_P1RegisteredDevices.cs`, `TransportErpDbContext.cs`, model snapshot, `P1Entities.cs`, و`SystemPermissionCatalog.cs`.
- الاختبارات الرئيسة: `P1SecurityIdentityTests.cs`, `P1SecurityHttpPostgreSqlTests.cs`, `BootstrapAdminPostgreSqlTests.cs`, `AuditEventPersistenceTests.cs`, `SyncOperationPersistenceTests.cs`, `ApiAuthenticationAndAuditTests.cs`, `Stage3RegisteredDevicePostgreSqlTests.cs`، وداعم `PostgreSqlTestCurrencyCodeAllocator.cs`.
- نطاق المراجعة الإلزامي: bootstrap/identity/authz، company/branch isolation، append-only audit، atomic rollback، device lifecycle/assignment/shared devices/rotation/revocation/expiry، session version binding، `LastSeenAt`، trusted `SyncOperation` provenance، legacy migration و`Up/Down/Up`، HTTP denial contracts، PostgreSQL concurrency، وإبقاء Offline معطلًا.

**Findings المغلقة:** stale audit head/context reuse (`da0e482c`)، batch rollback و403 contract (`cfb75c13`)، device lifecycle/session/provenance/migration/offline boundary (`3cd32ce2`)، currency seed collision (`90cc841f`)، واستدعاء advisory lock (`5ca9a86`).

**Findings/بوابات مفتوحة:** لا توجد علة Critical/High تنفيذية معروفة في مرشح المراحل 1–3 بعد CI النهائي؛ تبقى المراجعة المستقلة واعتماد المالك والدمج حالات غير منجزة. أعمال المرحلة الرابعة منفصلة أدناه، وOffline وG4/G5 مغلقة.

### 0.6 معيار تسليم المراجع المستقل

لا تسجل عبارة `READY FOR INDEPENDENT REVIEW` إلا بعد تحقق جميع ما يلي:

1. تثبيت مرشح التنفيذ وtree ونطاق الفرق، وعدم إدخال runtime للمرحلة الرابعة فيه.
2. نجاح jobي CI الإلزاميين على المرشح نفسه، مع validators وbuild وEF model وPostgreSQL migration وfull TRX.
3. وجود أدلة مهاجرتي P1 وStage 3، بما فيها التزامن و`Up/Down/Up` وlegacy/bound-unbound.
4. `git diff --check` نظيف، وعدم وجود conflict marker أو سر معروف في delta، وصفر Finding حرج/عالٍ مفتوح.
5. تحديد نطاق المراجعة والملفات والـCI والـfindings والاستثناءات، وأن يكون المراجع المستقل غير منفذ للـcommits المستهدفة.

**الحالة الآن:** `READY FOR INDEPENDENT REVIEW` للمراحل 1–3 على `5ca9a86...` وtree `9ce374...`؛ هذه العبارة تسليم حزمة فقط، وليست حكم `INDEPENDENTLY REVIEWED` ولا `OWNER APPROVED` ولا إذن دمج.

### 0.7 الحد الفاصل مع المرحلة الرابعة

`PHASE 3 CLOSED IMPLEMENTATION` على المرشح أعلاه. `PHASE 4 WORK IN PROGRESS` عقديًا فقط وفق القرار `DEC-P1-SYNC-POP-20260826-01` والقسم 19 من `P1_SYNC_CONTRACT.md`. لا يبدأ runtime قبل إغلاق تعارضات المراجعة الحاكمة، ولا يبدأ Phase 5، ولا يتغير `sync.offline.enabled=false`، ولا تُفتح G4/G5 بهذا السجل.

**حالة البوابة:** `STAGE4_CONTRACT_WIP — NOT READY FOR IMPLEMENTATION`. اتجاه السياسة معتمد، لكن التنفيذ محجوب حتى يثبت contract SHA يحتوي القرار نفسه، وتتطابق مفاتيح idempotency القديمة والجديدة مع نطاق الشركة، وتحسم دلالة `Down` عند وجود بيانات Stage 4، ويثبت عقد bind/rotate/recovery وترتيب linearization، ويجمد schema الدقيق للـnonce/replay/provenance والبصمة canonical مع golden vectors، ويفصل operation correlation عن attempt correlation، وتحدد حدود جسم الطلب والحمولة. هذه موانع تصميم حاكمة وليست عيوبًا مفتوحة في مرشح المراحل 1–3.
## 1. نطاق السجل
يغطي السجل كل عقود W1 السبعة عشر، وكل أفعال W2 الخمسة عشر، وكل شاشات W3 الاثنتي عشرة، إضافة إلى عشرة اختبارات حاكمة للمزامنة. كل اختبار مرتبط بمعرف عقد واضح، وله نتيجة مطلوبة ودليل تنفيذ يجب حفظه عند بدء الاختبار البرمجي.
| الفئة | عدد الاختبارات | الغرض |
|---|---:|---|
| `W1_SCHEMA` | 17 | مخطط W1 ومفتاحه |
| `W1_SCOPE` | 17 | نطاق W1 |
| `W1_CONCURRENCY` | 17 | تزامن W1 |
| `W1_AUDIT` | 17 | تدقيق W1 |
| `W1_LIFECYCLE` | 17 | دورة حياة W1 |
| `W2_HAPPY_PATH` | 15 | المسار السليم W2 |
| `W2_NEGATIVE` | 15 | الأخطاء W2 |
| `W2_IDEMPOTENCY` | 15 | Idempotency W2 |
| `W2_OFFLINE_AUDIT` | 15 | Offline وتدقيق W2 |
| `W3_LOAD_STATE` | 12 | حالات تحميل W3 |
| `W3_VALIDATION_PERMISSION` | 12 | تحقق وصلاحيات W3 |
| `W3_STATE_ACTIONS` | 12 | حالات وأفعال W3 |
| `W3_RTL_ACCESSIBILITY` | 12 | RTL وإتاحة W3 |
| `SYNC` | 10 | المزامنة |
| **الإجمالي** | **203** | تغطية P1 التعاقدية |

## 2. قاعدة حالة الاختبار

لا تتحول أي حالة إلى `PASS` أو `FAIL` إلا بعد تنفيذها على نسخة مرقمة من البرنامج، وحفظ بيانات الاختبار، والطلب والاستجابة، ولقطات الحالات، وسجل التدقيق، وتوقيع المراجع. قبل ذلك تبقى `SPECIFIED_NOT_EXECUTED` حتى لا تختلط الجاهزية التوثيقية بوجود تنفيذ.
## 3. بوابات الحوكمة

| البوابة | شرط الخروج | مالك القرار |
|---|---|---|
| G0 — نطاق P1 | قبول المالك لنطاق الإصدار والعقود | مالك المشروع |
| G1 — العقود | لا توجد حقول تعاقدية فارغة؛ W1/W2/W3 مترابطة | مالك المشروع + المراجع المستقل |
| G2 — التصميم | صورة كل شاشة مرتبطة بـW3، ولا صورة تمثل تنفيذًا | مالك المشروع + مراجعة UX |
| G3 — Offline/Online | اعتماد صلاحيات Offline، حد المحاولات، Backoff، التعارض والاحتفاظ | مالك المشروع + الأمن/المعمارية |
| G4 — الاختبارات | تنفيذ كل الاختبارات الحرجة وتوثيق الأدلة؛ لا عيوب حرجة مفتوحة | QA + مالك المشروع |
| G5 — التفويض | قبول G0–G4 وتحديد الإصدار والفرع وقيود التغيير | مالك المشروع |

### الحالة الحالية لبوابة G3 — 2026-08-25

**Decision ID:** `DEC-G3-SYNC-20260825-01`

**Exact baseline:** `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-25؛ لا يدعي هذا السجل توقيع أشخاص أو مراجعين غير مثبتين.

حالة G3 هي `G3_POLICY_ACCEPTED — RUNTIME_CONFORMANCE_PENDING_G4`. اعتمد القرار أعمدة وسياسات Offline/Sync فقط في W2، ولا يقبل بقية حقول W2 أو يغير status مراجعتها أو يفوض runtime. ثبت `P1_SYNC_CONTRACT.md` Payload Action allowlist، وهوية العملية والبروتوكول، وميزانيتي retries، وسياسة التعارض، وفترات الاحتفاظ، وحجم الدفعة وذريتها، وصلاحيات حل التعارض، وتسلسل الإعدادات. يبقى `sync.offline.enabled=false` حتى تثبت G4 المطابقة على exact SHA ثم يعتمد المالك G5.

| قرار G3 | القيمة المعتمدة |
|---|---|
| Offline Payload writes | P1 drafts: Journal/Receipt/Payment؛ P2-C01 Action mapping الحاكم؛ وما عداها Online؛ `SyncP1Operations` transport لا payload action |
| Read-cache | `SearchOperationalParties` و`ReadBasicWaybillCache` فقط؛ لا SyncOperation ولا write queue |
| Identity/protocol | `ActionCode`, `ProtocolVersion=sync-v1`, conditional `EntityId`, `ResultEntityId`, `RequestCorrelationId`; خريطة client operation↔server entity |
| Retry | ميزانيتان مستقلتان: client transport=`5` وserver execution=`5`; default `5/10/20/40/80s`; يعاد الحساب إذا شدد lower scope base؛ cap `30m` |
| Batch | `1..100`؛ ذرية ونتيجة لكل عملية؛ partial success مسموح |
| Conflict | `auto_merge=false`؛ BaseVersion فقط للoptimistic mutation؛ KEEP_SERVER يرفض الأصل، وREAPPLY ينشئ replacement QUEUED ثم يجعل الأصل RESOLVED/SUPERSEDED |
| Resolver | `sync.conflicts.resolve` + صلاحية الفعل + registered device + Company/Branch + reason |
| Retention | local success/resolved `24h`؛ rejected `7d`؛ server payload/snapshots `90d` بعد terminal/resolution |
| Protocol/hierarchy | `sync.protocol.allowed_versions=["sync-v1"]`; Global سقف؛ Company/Branch تضييق فقط؛ Device/Permission تقاطع أخير؛ invalid override fail-closed |
| Attachment/POD | metadata/hash queue فقط؛ binary runtime مؤجل حتى resumable/hash contract واختبارات G4 |
| Runtime gate | `sync.offline.enabled=false` حتى G4/G5 |

## 4. قرارات المالك التي لا يجوز افتراضها

لم تعد قرارات G3 المذكورة أعلاه مفتوحة للاجتهاد أو الافتراض؛ أي تغيير في allowlist أو retries أو conflict أو retention أو batch أو resolver أو hierarchy يمر بتغيير حوكمي مستقل واختبارات قبول. يبقى التفويض النهائي محجوبًا حتى G4/G5، كما يبقى أي قرار لإعادة استخدام كيان من AlTayerERP بدل تصميم TransportERP الجديد خاضعًا لاعتماد مستقل.

### شروط نقل G3 إلى G4

يجب أن تثبت G4 ActionCode/protocol allowlists قبل enqueue، وسجل جهاز حقيقي وإلغاءه، وفصل `sync.conflicts.resolve`، وقواعد EntityId/ResultEntityId/local↔server map، واكتشاف BaseVersion للأفعال optimistic فقط، وميزانيتي retry المنفصلتين وworkers/واجهاتها، وطابور عميل durable ومشفر، وretention cleanup/redaction، وتسلسل الإعدادات fail-closed. كما يلزم اختبار مستقل لكل Action واختبار رفض runtime-unavailable، وتنفيذ `T-SYNC-001..010` وحدود `0/1/100/101` والـreplay/counters والصلاحيات والاحتفاظ، ورفض binary Attachment/POD حتى عقده. لا يفتح G5 قبل نجاح ذلك على exact SHA وعدم وجود عيب حرج مفتوح.

### قرار تهيئة الهوية والصلاحيات الأولية — 2026-08-25

**Decision ID:** `DEC-P1-SEC-BOOTSTRAP-20260825-01`

اعتمدت تهيئة المسؤول الأول كأمر CLI صريح لمرة واحدة `--bootstrap-admin`، ولا يعمل في التشغيل العادي ولا يفتح endpoint. يقرأ سر المسؤول من مصدر السر الصريح `TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD` فقط، ويمنع تمريره ضمن argv أو `IConfiguration` العام، ولا يطبعه أو يضعه في marker أو AuditEvent. يتحقق الأمر من إعداداته قبل تطبيق Migrations، ثم يستخدم قفل PostgreSQL عامًا ومعاملة واحدة لإنشاء/التحقق من المراجع، والمستخدم المشفر، والدور، والمنح، والـmarker، والتدقيق. وجود marker أو أي مستخدم سابق يرفض التهيئة، والتشغيل العادي يتحقق من catalog فقط ولا يكتب إليه.

### قرار الجهاز الموثوق — Stage 3

يعتمد Stage 3 سجل تثبيت جهاز خاصًا بكل شركة، وتعيينًا صريحًا للمستخدم والفرع، ونسخة credential مرتبطة بالجلسة وبأصل عملية المزامنة. صلاحيات `devices.register` و`devices.read` و`devices.manage` نطاقها `COMPANY`؛ تظل `sync.operations.execute` بنطاق `BRANCH`. السر credential يولده العميل عشوائيًا لكل تثبيت (32 byte CSPRNG/Base64)، ولا يشحن داخل التطبيق، ويحفظه العميل في مخزن أسرار نظام التشغيل، بينما لا يخزن الخادم إلا SHA-256 ولا يعيد السر أو يسجله. لا ينقل السر إلا عبر TLS؛ هذه قاعدة نشر/بوابة موثوقة وليست ادعاءً بأن التطبيق الحالي يتحقق بنفسه من proxy/TLS termination.

هذا credential حامل per-install فقط، وليس مقاومًا لإعادة الإرسال ولا إثباتًا مربوطًا بكل طلب. لذلك Stage 3 يبقي Offline Sync محظورًا حظرًا صريحًا حتى لو ضُبط `sync.offline.enabled=true`. لا يجوز فتح المزامنة الإنتاجية قبل Stage 4 الذي يضيف freshness وnonce وإثبات حيازة/تقييد مرسل على مستوى الطلب (PoP أو آلية مماثلة لـ DPoP) واختبارات replay. كما يبقى مفتاح idempotency التاريخي `(DeviceId, ClientOperationId)` بلا تغيير في Stage 3؛ أي انتقال مستقبلًا إلى مفتاح request-bound يحتاج عقدًا ومهاجرة وpreflight مستقلة.

المرجع الأمني: [RFC 8252 §8.5](https://www.rfc-editor.org/rfc/rfc8252.html#section-8.5) يبيّن أن السر الثابت الموزع داخل تطبيق native لا يثبت هوية العميل؛ تصميم Stage 3 ليس app-wide بل عشوائي per-install، من دون تحويله إلى request proof. ويعرض [RFC 9449](https://www.rfc-editor.org/rfc/rfc9449.html) نمط DPoP لتقييد المرسل وتخفيف replay؛ يظل تطبيق آلية مماثلة مؤجلًا صراحةً إلى Stage 4.

### قرار إثبات طلب المزامنة — Stage 4

**Decision ID:** `DEC-P1-SYNC-POP-20260826-01`

**Decision basis:** exact commit `d0860d70d808374bd5582d2e71c14afa5429f8cd` مع مراجعة RFC 9449 وRFC 7638 وRFC 8725. هذا اعتماد سياسة وعقد واختبارات، وليس ادعاء تنفيذ أو PASS.

**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-26؛ لا يفوض الدمج أو تشغيل Offline أو نقل G4/G5 إلى PASS.

اعتمد المشروع `TransportERP Sync-PoP v1` لمسار `POST /api/v1/sync/operations:batch` وفق القسم 19 من `P1_SYNC_CONTRACT.md`. يبقى Access Token الحالي Bearer ولا يدعى أنه OAuth DPoP-bound؛ يضيف المسار proof غير متماثل `ES256/P-256` مربوطًا بالسجل الحي للجهاز ونسخة مفتاحه وبالـBearer token والطلب. يلزم public JWK/thumbprint وفق RFC 7638 و`ProofKeyVersion`، وclaims `htm`, `htu`, `iat`, `jti`, `ath`, `nonce`، وclaim المشروع `tbh` لSHA-256 لجسم HTTP الخام. لا ينتقل `DeviceCredential` داخل دفعة المزامنة بعد تفعيل profile، ولا يقبل External Authority claim بديلًا عن trusted local binding.

القيم التالية قرارات TransportERP وليست نصًا مفروضًا من RFC:

| القرار | القيمة الحاكمة |
|---|---|
| proof freshness | عمر سابق أقصى `120s` وانحراف مستقبلي أقصى `30s` بوقت خادم UTC |
| nonce | `32-byte CSPRNG`, Base64url بلا padding، hash-only في الخادم، صالح `5m` لنفس الجهاز ونسخة المفتاح |
| proof replay | `jti` واحد لكل جهاز/نسخة مفتاح؛ hash-only وunique ذري في PostgreSQL؛ retention `10m` |
| request body | `tbh=BASE64URL(SHA256(raw body octets))`; JSON وidentity content encoding فقط |
| target URI | HTTPS `sync.proof.public_origin` إلزامي + المسار الثابت؛ forwarded headers من KnownProxies/KnownNetworks فقط مع AllowedHosts |
| business idempotency | `(RegisteredDeviceId, ClientOperationId)`؛ `ActionCode` وكل الحقول التجارية الثابتة داخل full canonical fingerprint |
| legacy | لا backfill تخميني؛ partial legacy uniqueness؛ collision يرفض؛ migration additive مستقلة مع preflight وUp/Down/Up |
| runtime gate | `sync.offline.enabled=false`; production يعيد `OFFLINE_DISABLED` حتى G4/G5 |

اختيار `120s/30s` يحد proof المسروق إلى نافذة قصيرة مع سماح تشغيلي محدود لانحراف الساعة. مدة nonce `5m` تسمح بالتحدي وإعادة المحاولة والدفعات المتوازية من دون تحويل nonce إلى replay key؛ يبقى `jti` single-use. مدة replay `10m` تتجاوز عمر proof وعمر nonce وتمنع cleanup مبكرًا بين instances. أما `tbh` فقرار تشديد لأن RFC 9449 يغطي method وURI ولا يغطي جسم الطلب.

فصل القرار طبقتين لا يجوز دمجهما: `jti/nonce` يحميان محاولة HTTP الواحدة ويجب تغييرهما عند retry، بينما `ClientOperationId` يحمي الأثر التجاري ويبقى ثابتًا. proof جديد مع العملية نفسها يجب أن يعيد النتيجة نفسها؛ proof مكرر يرفض قبل enqueue. يُحسم بهذا القرار التعارض النصي السابق: `ActionCode` يدخل fingerprint ولا يدخل uniqueness، لأن `ClientOperationId` فريد لكل RegisteredDevice.

لا تنتقل حالة Stage 4 إلى `IMPLEMENTED` ولا G4 إلى PASS إلا بعد migration فعلية، والتحقق cryptographic، وshared replay store، وtrusted proxy deployment validation، واختبارات الحدود والتوازي والتدوير والتسريب والـlegacy على exact SHA. ولا يفتح G5 أو Offline بسبب هذا الاعتماد وحده.

النطاق الحاكم لـ`auth.scope.select` هو `PLATFORM`. جميع صلاحيات التشغيل الحالية المسجلة في `SystemPermissionCatalog` — المزامنة، والتدقيق، والبوليصة والأطراف، والتحصيل، والترحيل والرحلات وكشوف التحميل — نطاقها `BRANCH`. أي نقص أو تغيير في `ScopeType` أو metadata أو منح `SYSTEM_ADMIN` يفشل مغلقًا. أما `DefaultCalendarId` في نموذج P1 الحالي فهو UUID إلزامي لكنه مرجع خارجي opaque بلا `DbSet` أو FK أو catalog تقويم داخل هذا النطاق؛ لذلك تتطلب التهيئة قيمة صريحة ولا تدعي التحقق من وجود التقويم حتى يُعتمد كيان التقويم وربطه في مرحلة مستقلة.
## 5. مراجع الحزمة

- `W1_DATA_CONTRACT_REGISTER.csv`
- `W2_ACTION_CONTRACT_REGISTER.csv`
- `W3_SCREEN_CONTRACT_REGISTER.csv`
- `P1_SYNC_CONTRACT.md`
- `P1_RTL_SCREENS/README.md`
- `P1_SCREEN_IMAGE_REGISTER.csv`
