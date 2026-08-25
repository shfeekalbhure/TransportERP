# TransportERP — سجل اختبارات القبول وحوكمة P1
**الحالة:** `SPECIFIED_NOT_EXECUTED` — هذه مواصفات قبول قابلة للتنفيذ، وليست نتائج تشغيل فعلية.
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

النطاق الحاكم لـ`auth.scope.select` هو `PLATFORM`. جميع صلاحيات التشغيل الحالية المسجلة في `SystemPermissionCatalog` — المزامنة، والتدقيق، والبوليصة والأطراف، والتحصيل، والترحيل والرحلات وكشوف التحميل — نطاقها `BRANCH`. أي نقص أو تغيير في `ScopeType` أو metadata أو منح `SYSTEM_ADMIN` يفشل مغلقًا. أما `DefaultCalendarId` في نموذج P1 الحالي فهو UUID إلزامي لكنه مرجع خارجي opaque بلا `DbSet` أو FK أو catalog تقويم داخل هذا النطاق؛ لذلك تتطلب التهيئة قيمة صريحة ولا تدعي التحقق من وجود التقويم حتى يُعتمد كيان التقويم وربطه في مرحلة مستقلة.
## 5. مراجع الحزمة

- `W1_DATA_CONTRACT_REGISTER.csv`
- `W2_ACTION_CONTRACT_REGISTER.csv`
- `W3_SCREEN_CONTRACT_REGISTER.csv`
- `P1_SYNC_CONTRACT.md`
- `P1_RTL_SCREENS/README.md`
- `P1_SCREEN_IMAGE_REGISTER.csv`
