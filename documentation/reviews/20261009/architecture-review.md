# مراجعة المعمارية وربط مشاريع TransportERP — 2026-10-09

المراجع الفعلي: `/root/architecture`، مكلف من فريق المراجعة في هذه المحادثة. المهمة قراءة وتحليل فقط؛ لم يعدل المراجع أكواد المشروع أو قاعدة البيانات.

## تثبيت مصدر النتائج وحدودها

- المصدر الحالي: `جذر نسخة المصدر`.
- الفرع الأصلي: `feature/receipt-general-ledger-settings`.
- SHA: `9d02ac8d19e39971ad233108faecc8154a1d5745`، تحقق بواسطة `git rev-parse HEAD`؛ نسخة العمل نظيفة أثناء المراجعة.
- المصدر السابق `1026a9069756b956b17b55d853e5ebda535810ec` فُحص أولًا ثم استُبدل بعد اكتشاف المصدر الأحدث. أحكام غياب الدخول الحقيقي وغياب HTTP في النسخة القديمة **لا تنطبق على النسخة الحالية**.
- لا توجد AGENTS.md ضمن المستودع الحالي حسب `rg --files -g AGENTS.md`؛ لم أعثر كذلك على ملف حاكم في مسار العمل عند المسح الأول.
- كل مرجع `path:line` أدناه نسبي إلى المصدر الحالي ما لم يوصف صراحة بأنه تاريخي.
- هذا تحليل مصدر ثابت. تشغيل الخادم، تسجيل دخول حقيقي، استدعاء قاعدة PostgreSQL، تشغيل Windows، APK، أجهزة GPS، والنشر الخارجي غير متحقق منها. البيئة لا تحتوي dotnet وفق تحقق الفريق الرئيسي؛ وجود الاختبارات لا يثبت نجاحها على هذا SHA.
- «غير منفذ في النسخة» يعني عدم وجود التنفيذ ضمن الملفات المتتبعة التي فُحصت، وليس الادعاء بعدم وجوده في جهاز المستخدم أو مستودع آخر.

## الحكم المعماري

توجد طبقات خلفية حقيقية: Domain وApplication وContracts وInfrastructure وApi، ويوجد Desktop قابل للتشغيل من نقطة دخول WinForms. النسخة الحالية أضافت ربطًا حقيقيًا HTTPS للدخول، اختيار الشركة/الفرع/الفترة، سند القبض، وإعدادات الأستاذ العام. **الربط بين جميع المشاريع لم يكتمل**: مشاريع الجوال الثلاثة ما زالت ملفات مشروع فقط؛ بقية واجهات البوالص ترفع أحداث UI بلا عميل تشغيل موصول؛ إعدادات النظام العامة واجهة غير مرتبطة؛ GPS غير منفذ؛ المزامنة تحفظ طابورًا في الخادم دون منفذ للعمليات أو عميل offline.

لا يصح وصف المشروع بأنه مجرد شاشات، ولا يصح وصف التطبيقات الثلاثة بأنها تطبيقات جاهزة لأن أسماءها موجودة في Solution.

## 1. جرد المشاريع والمراجع الفعلية

يوجد 16 ملف csproj متتبعًا. Solution الرئيسي يسرد 14 مشروعًا في `TransportERP.slnx:5-18`. المشروعان الآخران أداتا فحص/استضافة خارج Solution. لم يعد ملف `TransportERP.Screens.slnx` موجودًا في المصدر الحالي.

| المشروع | الوظيفة المثبتة | ارتباطات المصدر | الحكم |
|---|---|---|---|
| TransportERP.Domain | قواعد وتجميع البوليصة والمالية والتنفيذ | `TransportERP/TransportERP.Domain.csproj:4`، ملفات `Waybills/` | كود مجال موجود، لا واجهة تشغيل |
| TransportERP.Contracts | DTO مشتركة للهوية، المحاسبة، البوالص، الأطراف، الجغرافيا، المرفقات | `TransportERP.Contracts/TransportERP.Contracts.csproj:4` | موجود؛ لا يساوي خدمة مشتركة منفذة |
| TransportERP.Application | خدمات البوالص والشحن والمالية وbaseline في الذاكرة | `TransportERP.Application/TransportERP.Application.csproj:10-11` يراجع Domain/Contracts | موجود |
| TransportERP.Infrastructure | EF Core + PostgreSQL + migrations + خدمات سند القبض/السياسات/المزامنة | `TransportERP.Infrastructure/TransportERP.Infrastructure.csproj:10-22` | موجود؛ اتصال قاعدة فعلية غير متحقق |
| TransportERP.Api | ASP.NET Core، JWT، login، receipt، ledger settings، waybill/finance/shipping، sync، audit | `TransportERP.Api/TransportERP.Api.csproj:15-17`، `Program.cs:18-30,73-83` | كود خدمة موجود |
| TransportERP.Desktop | WinForms مع login ثم FrmMain وUserControls | `Program.cs:8-9`، `TransportERP.Desktop.csproj:17,28-31` | تشغيل Windows لم يُختبر هنا |
| TransportERP.Mobile.Admin | تعريف مشروع مشروط بوجود ملفات MAUI | `TransportERP.Mobile.Admin.csproj:6-18` | ملف csproj فقط، يتحول حاليًا إلى Library net10.0 |
| TransportERP.Mobile.Customer | التعريف المشروط نفسه | `TransportERP.Mobile.Customer.csproj:6-18` | ملف csproj فقط |
| TransportERP.Mobile.Driver | التعريف المشروط نفسه | `TransportERP.Mobile.Driver.csproj:6-18` | ملف csproj فقط |
| TransportERP.EmptyForms | واجهات UserControl وأدوات سلوك محلية | `TransportERP.EmptyForms.csproj:9-14` | UI موجود؛ الاسم لا يعني خلو كل الواجهات من الكود |
| TransportERP.SharedVisuals | يجمع ملفات CoreUI/Templates المرتبطة من Desktop | `TransportERP.SharedVisuals.csproj:10-13` | مكتبة مرئية Windows، لا تستخدم للجوال |
| components/src/TransportERP.SharedUI | مكونات UI تدقيق/أوامر، مع استبعاد مسودة UserControl1/CoreUI | `TransportERP.SharedUI.csproj:11-18` | مكتبة WinForms |
| components/toolbar-preparation/DesignerDraft | يجمع المسودة مع ملفات موارد من Desktop | `DesignerDraft.csproj:15-24` | مكون مرئي مرتبط بمصادر أخرى، ليس API |
| TransportERP.Tests | اختبارات API/Application/Infrastructure/Contracts | `TransportERP.Tests.csproj:24-27` | ملفات اختبارات موجودة، نتائج هذا SHA غير متحققة هنا |
| CodeRepairChecks | أداة Windows تنفيذية تراجع Desktop/EmptyForms | `CodeRepairChecks/CodeRepairChecks.csproj:3-8` | خارج Solution الرئيسي؛ ليست تطبيق العميل/السائق |
| components/host/SharedComponents.Host | مضيف WinForms مرجعي اختياري للمكونات | `components/host/SharedComponents.Host.csproj:3-5,13-18,23-24` | خارج Solution الرئيسي؛ يحتاج مسار موارد |

اتجاه الخلفية المنفذ: Api يراجع Application + Infrastructure + Contracts؛ Application يراجع Domain + Contracts؛ Infrastructure يراجع الثلاثة. Desktop يراجع Contracts + مكتبات Windows فقط ولا يراجع Infrastructure، وهذا يمنع إدخال الاتصال المباشر بقاعدة PostgreSQL في UI. إعدادات ومراجع الحل ليست «ربط تشغيل» بمفردها.

## 2. مصفوفة الربط بين أجزاء النظام

| من → إلى | حالة الكود الحالي | الدليل | المتبقي |
|---|---|---|---|
| Desktop → API login | منفذ في المصدر | `FrmLogin.cs:58-69`، `Authentication/DesktopSession.cs:22-27` | تحقق التشغيل، توافق المنفذ، شهادة TLS وإعداد الهوية |
| Desktop → اختيار نطاق الشركة/الفرع/الفترة | منفذ | `DesktopLoginModule.cs:79-125`، `FrmMain.AuthenticatedSession.cs:21-42` | اختبار واقعي بمستخدم متعدد الشركات والفروع |
| Desktop → سند القبض | منفذ | `FrmMain.ReceiptWorkspace.cs:17-43,66-105,148-169` | تشغيل end-to-end ومراجعة النتائج المالية بمعرفة فريق المحاسبة |
| Desktop → إعدادات الأستاذ العام | منفذ | `FrmMain.LedgerSettings.cs:24-49,56-79` | تحقق سياسة الشركة وحقوق التعديل |
| شاشة القيد → إلزام الوصف من إعدادات الأستاذ | جزء محدد منفذ | `FrmMain.LedgerSettings.cs:13-20` | هذا الربط لا يثبت اكتمال حفظ/ترحيل القيد نفسه |
| Desktop → عمليات البوالص والشحن | UI/contracts موجودة، الربط التشغيلي غير موجود في مواضع الإنشاء الحالية | `Waybills/UcWaybillDraft.cs:20-23,40-50`، `Waybills/UcItemRelease.cs:22,60-64`؛ نقل HTTP المحصور في ملفات جلسة/قبض/أستاذ | تسجيل handlers، استخراج DTO، تحميل القوائم والحالة، قراءة/بحث من API |
| API → PostgreSQL | منفذ بالتسجيل | `Api/Program.cs:14-18`، `TransportErpPersistenceExtensions.cs:16-21,25-31` | تشغيل migrations على قاعدة اختبار منفصلة؛ لا دليل على قاعدة المستخدم |
| Mobile Admin/Customer/Driver → API | غير منفذ | محتوى المجلدات المتتبع ملف csproj واحد لكل مشروع؛ `*.csproj:6-18,45-48` لا ProjectReference ولا مصادر MAUI | تطبيقات MAUI وعميل API وتسجيل دخول وصلاحيات كل فئة |
| Driver → GPS → API → Desktop/Customer | غير منفذ | لا Geolocation/Latitude/Longitude/GPS implementation في ملفات cs/csproj الحالية؛ `MovementContracts.cs:7-19` أحداث حركة بلا إحداثيات | عقد تتبع ومصدر جهاز وتخزين وسياسة صلاحيات وشاشة عرض |
| Offline local store → sync → server operations | جزئي في الخادم فقط | `SyncOperationService.cs:68-137` enqueue، `Api/Program.cs:119-131` يستدعي enqueue | outbox محلي، device registration، executor، نتائج/قراءة، conflicts/retries متصلة |

## 3. الموجود فعلًا في خدمة API

هناك 41 تعريف MapGet/MapPost/MapPut في ملفات API الحالية: 23 تعريفًا سابقًا و18 للدخول/القبض/الأستاذ. لا يعني ذلك 41 شاشة أو 41 workflow كاملًا.

| المجموعة | العدد | المسارات الحالية | الدليل |
|---|---:|---|---|
| الدخول والنطاق | 4 | `/api/v1/auth/login`، `select-scope`، `receipt-scopes`، `receipt-scope` | `Authentication/DesktopLoginModule.cs:38-41` |
| سند القبض | 12 | bootstrap، configuration، list، by id، attachment upload/download، save، review، approve، post، cancel، reverse | `Accounting/ReceiptApiModule.cs:17-108` |
| إعدادات الأستاذ | 2 | GET/PUT `/api/v1/accounting/settings` | `Accounting/GeneralLedgerSettingsApi.cs:11-22` |
| تأسيس البوليصة | 9 | draft create/update، party search/create، validate/submit/approve/return/cancel | `Waybills/WaybillApiModule.cs:29-134` |
| مالية البوليصة | 3 | payment-plan update، collection create/reverse | `Waybills/WaybillFinanceApiModule.cs:24-65` |
| تنفيذ الشحن | 9 | release، trip create، allocation create/reverse، manifest create/load/finalize/handover، trip start | `Waybills/ShippingExecutionApiModule.cs:23-72` |
| المزامنة والتدقيق | 2 | batch enqueue، audit search | `Program.cs:85-151,153-190` |

لا توجد في الـAPI الحالي مجموعة CRUD تشغيلية لإعدادات النظام العامة/الشركات/الفروع/العملات/الجغرافيا أو قراءة البوالص والرحلات/حالتها أو GPS أو تطبيق العميل/السائق. DTO الجغرافيا `GeoContracts.cs:7-23` ليست Endpoint. وجود كيانات P1 وإجراءات في الذاكرة لا يثبت اتصالها بواجهة Desktop.

## 4. نتائج ونواقص ذات أولوية

### ARCH-001 — تطبيقات الجوال الثلاثة لم تبدأ كتنفيذ تشغيلي

الأولوية: عالية — نقص تنفيذ مؤكد في المصدر.

كل مجلد Mobile يحتوي ملف csproj فقط. شرط `MauiRuntimeReady` يتطلب MauiProgram/App/Platforms/icons/splash؛ لغياب هذه الملفات ينتقي `net10.0` و`Library` ولا يفعل UseMaui (`Mobile.Admin.csproj:6-18` ونفس المواضع في Customer/Driver). لا يوجد client ولا شاشة ولا entrypoint ولا مشاركة Contracts.

الحل: تخطيط سيناريوهات وصلاحيات وقاموس عقود كل تطبيق، ثم تنفيذ MAUI/clients. ليس إصلاح تصميم Desktop. لا تنقل مكتبات WinForms للجوال. شرط القبول: تشغيل APK وتسجيل دخول وقراءة/إرسال عمليات موثقة لكل فئة.

### ARCH-002 — عدد كبير من واجهات النظام لا يزال غير موصول بالخادم

الأولوية: عالية — نقص ربط مؤكد.

`UcWaybillDraft.cs:40-50` يستدعي SaveRequested/SubmitRequested دون خدمة. `UcItemRelease.cs:60-64` يصدر ReleaseRequested. لا يوجد subscriber تشغيلي لهذه الأحداث في ملفات المصدر الحالية حسب البحث، ولا HTTP waybill client. موضع الربط المركزي `FrmMain.ReceiptWorkspace.cs:13-45` يصل القبض والأستاذ فقط. `FoundationUiSession.cs:116-123` يعرض آلية BindCommands؛ البحث عن الاستدعاءات وجد تعريفها ولم يجد ربطًا عامًا للشاشات التأسيسية.

الحل: كود adapters/clients لكل مجموعة متشابهة، مع حصر binding الحالي وAPI gaps. الحفاظ على التصميم والتبويبات والحقول. لا تشغّل أزرار Save/Publish لمجرد إظهارها دون نجاح حفظ مؤكد من الخادم.

### ARCH-003 — إعدادات النظام العامة ليست مصدر إعدادات مشتركًا عاملًا

الأولوية: عالية — نقص تنفيذ مؤكد.

`Forms/SystemSettings/General/FrmGeneralSettings.cs` يحتوي الآن class `UcGeneralSettings`، رغم بقاء اسم الملف القديم. السطر 37 يصرح «واجهة تصميم؛ الحفظ والبيانات غير مرتبطة بعد» والسطور 39-43 تعطل Save/Reset/ModuleActivation/Validate/Publish/Revert/Audit/Refresh. توجد حقول النطاق واللغة/العملة والجلسة والمظهر وoffline/sync (`67-69`)، وتوجد GlobalSetting/CompanySetting/BranchSetting في `P1Entities.cs:139-162` وDbSets في `TransportErpDbContext.cs:16-18`، لكن لم توجد خدمة resolver عامة أو API لها.

الحل: مواصفة إعدادات مركزية أولًا: مفتاح، نوع، نطاق، قيمة افتراضية، توريث، صلاحية، نسخة، وقت نفاذ، audit، قدرة كل منصة. ثم الخدمة والعقود وربط الواجهات. لا تكرر نفس إعداد الشركة في كل تطبيق.

### ARCH-004 — عنوان الخادم الافتراضي لا يطابق إعداد تشغيل API المتتبع

الأولوية: عالية — تعارض إعداد مثبت، ليس فشلًا تشغيليًا مجربًا.

`FrmLogin.cs:14` يختار `https://localhost:7011/` ما لم يضبط `TRANSPORTERP_API_URL`. ملف `Api/Properties/launchSettings.json:9` يطلق `https://localhost:58516;http://localhost:58517`. لذلك عند التشغيل بالملف الحالي وبلا override سيشير Desktop إلى منفذ آخر.

الحل: توحيد profile وعنوان تطوير HTTPS وملف إعداد غير سري/متغير البيئة وتعليمات التشغيل. لا تغيّر شهادة النظام ولا تعطّل TLS؛ `DesktopSession.cs:15-20` يفرض HTTPS ويرفض redirects.

### ARCH-005 — اختيار Auth:Authority يلغي مسار الدخول المتاح في Desktop الحالي

الأولوية: متوسطة/عالية حسب بيئة التشغيل — عدم توافق وضعين مثبت.

الخادم يدعم JWT authority خارجيًا (`Program.cs:48-52`). لكن local login يعيد 503 عند وجود Authority (`DesktopLoginModule.cs:46-53,109`)؛ Desktop ينفذ login/password فقط (`DesktopSession.cs:22-27`). لا يوجد تدفق OAuth/OIDC بديل في ملفات العميل.

الحل: توثيق الوضع المدعوم حاليًا بأنه local issuer، أو تنفيذ تدفق الهوية الخارجية قبل تفعيل Authority. هذا لا يعني أن التحقق JWT الخارجي معيب؛ الفجوة في تدفق العميل.

### ARCH-006 — المزامنة الحالية لا تحقق offline end-to-end

الأولوية: عالية — نقص تنفيذ مؤكد.

المسار `/sync/operations:batch` يستدعي `EnqueueSyncOperationAsync` فقط (`Program.cs:126-131`)، والخدمة تحفظ Status=QUEUED (`SyncOperationService.cs:91-117`). عمليات transition/retry/conflict موجودة كطرق خدمة (`140-250,253+`) لكن لا worker ولا mapping لتطبيق PayloadJson على خدمة مستند ولا عميل محلي متصل. بحث AddHostedService/IHostedService/BackgroundService لم يجد تشغيل منفذ الطابور.

علاوة على ذلك، JWT الذي يصدره الدخول الحالي يحوي sub/company/branch/fiscal/permissions فقط (`DesktopLoginModule.cs:154-161`) ولا device_id/device_registered؛ sync endpoint يشترطهما (`Program.cs:104-109`). لذلك جلسة Desktop الحالية لن تجتاز شرط تسجيل الجهاز حتى لو منح sync.operations.execute.

الحل: device enrollment وعقد العميل/outbox ثم dispatch whitelist للعمليات، processing transaction/result version، acknowledgement/pull، conflicts/retry، اختبار فقد الشبكة وإعادة الإرسال دون تكرار. عدم اعتبار نجاح enqueue نجاحًا للعملية المالية. تحديد العمليات التي يسمح بتنفيذها دون اتصال قبل كتابة قاعدة محلية.

### ARCH-007 — GPS وتتبع السائق غير منفذين في المصدر الحالي

الأولوية: عالية في نطاق المستخدم، feature gap وليس bug في شاشة موجودة.

`Tracking/MovementContracts.cs:7-19` يصف حدثًا عملياتيًا ولا يتضمن إحداثيات. `Geo/GeoContracts.cs:7-23` وصف مناطق إدارية. لا توجد geolocation provider أو telemetry endpoint أو lat/long entity أو تطبيق Driver مصدر للموقع.

الحل تصميم عقد دقيق ثم تنفيذ المصدر الخلفي والجوال: هوية الشركة/الجهاز/المركبة/الرحلة، وقت الالتقاط UTC، الإحداثيات، accuracy/source، sequence/client operation، وقت الاستلام وحالة stale، صلاحيات المشاهدة، الفترة/الاحتفاظ. هذه حقول مقترحة للمراجعة وليست حقولًا موجودة أو قرار DB معتمدًا.

### ARCH-008 — إعادة التحقق من إلغاء الصلاحية غير موحدة بين مجموعات API

الأولوية: عالية — اختلاف أمني مثبت في مسار الكود.

API سند القبض يعيد قراءة المستخدم الفعال وصلاحياته الحالية من DB (`ReceiptApiModule.cs:149-170`). مجموعات البوالص/المالية/الشحن تستخدم permission claims فقط: `WaybillApiModule.cs:183-213`، `WaybillFinanceApiModule.cs:133-135`، `ShippingExecutionApiModule.cs:121-151`. JWT الصادر صالح 15 دقيقة (`DesktopLoginModule.cs:125`) ولا يوجد OnTokenValidated عام يعيد فحص المستخدم/إلغاء الصلاحية في `Program.cs:43-69`.

النتيجة المستنتجة مباشرة من المسار: سحب permission بعد إصدار token يؤثر فورًا على سند القبض، لكنه لا يغير claim في token نفسه، وقد يبقى مسار البوليصة يجيز الفعل حتى انتهاء token ما لم تمنعه قاعدة أخرى خاصة بالمستند. لم ينفذ المراجع تجربة استغلال.

الحل: خدمة Authorization مشتركة محددة السياسة أو security stamp/revocation/check current permissions، مع tests تعطيل المستخدم وسحب الدور أثناء جلسة. عدم تكرار قراءة/قواعد scope في كل Module دون عقد مشترك.

### ARCH-009 — سياسة الأستاذ المنفذة تغطي سند القبض فقط في workflow الحالي

الأولوية: عالية بالنسبة لشمول النظام — نقص تغطية مثبت.

إعدادات الأستاذ موجودة company-wide، مع optimistic concurrency/audit/snapshot للمستند الجديد (`GeneralLedgerPolicyService.cs:18-23,31-35,50-83`). قائمة SupportedDocumentTypes المعادة تساوي `["RECEIPT_VOUCHER"]` (`48`). ربط القيد يقرأ إلزام الوصف فقط (`FrmMain.LedgerSettings.cs:13-20`)، فلا يجوز تعميم وجود سياسة review/approve/post على سند الصرف والقيد وكافة المستندات.

الحل: توثيق قائمة supported بدقة، ثم مرحلة مستقلة لمستند الصرف والقيد مع حفظ نسخ سياسة تاريخية واختبار تأثير تغيير السياسة. لا تغيير تلقائي للمستندات القائمة.

### ARCH-010 — عقود القراءة المطلوبة لربط شاشات البوالص/الرحلات غير موجودة في API الحالي

الأولوية: عالية — نقص Endpoint مثبت بالجرد.

`WaybillApiModule.cs:29-134` أوامر وparty search فقط؛ `ShippingExecutionApiModule.cs:23-72` أوامر فقط. لا GET waybill/by id/list/item quantities/trip/manifest في الخرائط الحالية. UI يتطلب Bind من WaybillResponse/ItemQuantityStateResponse (`UcWaybillDraft.cs:25-34`، `UcItemRelease.cs:24-43`). لذا إضافة HttpClient للأوامر وحدها لا تكفي للفتح/البحث/التحديث أو متابعة السائق/العميل.

الحل: عقود query/read models ذات pagination/filter/version/scope/ownership، ثم clients. تحديد علاقة CustomerUser↔OperationalParty وDriverUser↔Driver قبل تعريض مسارات الموظف لتطبيق العميل/السائق. تحقق trip.DriverId في handover (`ShippingExecutionPersistence.cs:683-684`) ليس دليلًا على وجود ربط هوية السائق بالجوال.

### ARCH-011 — CI لا يثبت تشغيل WinExe أو تطبيقات الجوال أو نجاح الربط المرئي

الأولوية: متوسطة — فجوة تحقق مثبتة.

`ci.yml:60-76` يبني tests/backend ويجهز PostgreSQL ويشغّل suite؛ Desktop job يجبر `OutputType=Library` (`90-94`)، رغم وجود Program/Main. لا خطوة تشغيل واجهة ولا build APK ولا mobile workflows ولا نشر runtime. CodeRepairChecks خارج Solution (`TransportERP.slnx:5-18` مقابل csproj أداة الفحص).

الحل: بوابة Windows Release WinExe كاملة، فحوص فتح Designer/UserControl والأزرار الأساسية وDPI/RTL بعد الإصلاحات، بوابات MAUI عند وجود scaffold، وربط أداة الفحص ضمن المسار المطلوب. لا اعتبار build Library دليلًا على تشغيل التطبيق.

### ARCH-012 — تشغيل ونشر API يحتاجان وصف بيئة موحدًا

الأولوية: متوسطة — فجوة تسليم/تشغيل.

الخادم يوقف startup بدون connection/audience/issuer+key أو authority (`Program.cs:14-16,36-41`)، وهذا سلوك fail-closed صحيح. EF design-time يحتاج اسمًا مختلفًا `TRANSPORTERP_DESIGN_CONNSTR` (`TransportErpDbContextFactory.cs:10-18`). ملف launchSettings الحالي يحدد URLs وDevelopment فقط. لا appsettings template أو Docker/deploy/publish definition متتبع في الملفات المسحوبة. هذه ليست دعوة لوضع أسرار في Git.

OpenAPI packages موجودة (`Api.csproj:10-11`) لكن AddOpenApi/MapOpenApi/Swagger غير مسجلة في Program، فلا يصح وصف Swagger بأنه متاح حاليًا بمجرد وجود PackageReference.

الحل: دليل تشغيل قابل للتكرار، template أسماء بلا أسرار، عنوان خادم موحد، migration/backup/recovery instructions، وإثبات نشر عند توفر بيئة اختبار. TLS termination وhealth/error logging سياسة نشر مطلوبة تحديدًا؛ وجود reverse proxy خارجي غير متحقق ولا يُفترض غيابه.

## 5. كيف تكون إعدادات Desktop والجوال مترابطة؟ — مقترح للمراجعة

المقترح مبني على الجداول الحالية ولا يعد تنفيذًا أو قرار قاعدة معتمدًا: مصدر business settings مركزي في API/DB؛ UI لكل منصة يناسب مستخدمها، وتفضيلات الجهاز محلية. لا ثلاث نسخ متعارضة من إعداد العملة/الترقيم/سياسة الاعتماد.

| نوع الإعداد | مكان الملكية المقترح | أمثلة | حالة المصدر الحالي |
|---|---|---|---|
| بيئة التشغيل | إعداد خادم/deployment؛ عنوان API لدى العميل | URL، Auth mode، connection، logs | env/runtime موجود؛ دليل موحد ناقص |
| إعدادات منصة عامة | API + GlobalSetting مع صلاحية منصة | سياسات متاحة لجميع الشركات ومفاتيح عامة غير سرية | entity موجود، resolver/API غير موجود |
| الشركة | API + CompanySetting | سياسة دفتر الأستاذ، إعدادات القبض، العملة الأساسية | الأستاذ/القبض فعليان؛ بقية الإعدادات غير متصلة |
| الفرع | API + BranchSetting حسب مفاتيح يسمح لها بالتخصيص | defaults تشغيلية خاصة بفرع إذا أقرت مواصفتها | entity موجود، توريث عام غير منفذ |
| المستخدم | مصدر مركزي للأدوار ونطاقات الدخول؛ تفضيلات شخصية حسب العقد | صلاحيات العرض والأفعال، النطاقات المتاحة | user/role tables + local login منفذة |
| الجهاز/واجهة المنصة | تخزين محلي غير حاكم ماليًا، ومصدر مركزي لسياسة الأجهزة | حجم عرض، لغة جهاز، نمط العرض، GPS sampling المسموح | لا local profile/offline implementation عام |

توريث مقترح يحتاج اعتماد لكل مفتاح: قيمة عامة ← override شركة ← override فرع، مع منع override للمفاتيح المالية company-only القائمة. GeneralLedgerPolicyService الحالي **متعمد company-wide** (`18`)؛ لا تضف branch override له تلقائيًا. الحسابات/الأرصدة/الترقيم/سند القبض والاعتماد يحسمها الخادم. العميل يتلقى capabilities وحالة اتصال/version وسبب منع الإجراء.

شاشات الإعداد المقترحة تُبنى فوق التصميم الحالي: تبويب النطاق والمفتاح/النوع/القيمة، تبويب النسخة والتوريث، تبويب القدرات المسموحة لكل تطبيق، تبويب الأجهزة والمزامنة، تبويب التدقيق. يجب فصل المقترح عن قائمة الحقول المعتمدة لدى فريق المرجع التأسيسي؛ لا تغيير الشاشة أو migrations قبل التوثيق والاعتماد المطلوبين.

## 6. مراحل الربط والتنفيذ المقترحة والاعتماديات

1. **تثبيت النسخة**: SHA الحالي، تقارير المصدر وخرائط الشاشات/حقولها/أزرارها، قائمة known gaps، بدون نقل master أو استبدال التصميم.
2. **إزالة عائق التشغيل**: عنوان API/profile، template بيئة، DB test آمنة، تشغيل local login/القبض/الأستاذ، Windows executable checks.
3. **توحيد الأمن وعقد الإعدادات**: تحديد actor types والنطاق والملكية وإعادة فحص permissions، registry إعدادات وتوريث، فصل الهوية الخارجية عن المحلية.
4. **خدمات التهيئة**: CRUD/read للشركات/الفروع/العملات/الجغرافيا المطلوبة، clients وFoundation bindings بالمجموعات المتشابهة. لا استنتاج DB field من Control.Name.
5. **قراءة وربط البوالص والشحن**: query endpoints ثم DTO/forms handlers، حالات loading/empty/error/conflict، تحقق تعداد الحقول والعمليات ومراحل lifecycle؛ بعد ذلك arrival/delivery حسب المرجع المعتمد.
6. **الجوال بحسب الدور**: Admin يستهلك إعدادات التشغيل المركزية، Customer يرى ما يخصه، Driver يرى رحلاته/عهدته؛ shared Contracts/transport services فقط، UI MAUI لكل دور.
7. **Offline/GPS**: عقود أجهزة وoutbox/pull/executor أولًا، ثم driver telemetry وقراءة الحالة وعرضها، اختبارات إعادة الإرسال والساعات والتعارض والنطاق.
8. **تسليم**: gates backend/PostgreSQL/Windows/mobile، smoke حقيقية، trace IDs، دليل التشغيل/الاستعادة وقائمة ما بقي غير منفذ.

لا يبدأ فريق المعمارية قاعدة جديدة أو يعيد تعريف جداول موجودة تلقائيًا؛ توجد migrations فعلية في هذا المستودع. يجب أن يفصل الفريق الاستشاري بين الواقع المتتبع وقاموس البيانات المقترح، ويطابقهما بالمرجع التأسيسي المطلوب قبل أي تغيير تنفيذي.

## 7. تقسيم عمل مقترح للتنفيذ

| الفريق | النطاق | نوع الإصلاح | اعتماديات |
|---|---|---|---|
| تشغيل وهوية | API URL، env/runbook، local/external auth، permissions مشتركة | كود + config + اختبارات | مصدر مثبت، بيئة اختبار |
| إعدادات وتأسيس | الشركات/الفروع/العملات/الجغرافيا/الإعدادات/الترقيم | عقود ثم كود binding؛ الحفاظ على UI | مرجع وحقول معتمدة، authorization |
| مستندات مالية | القبض أولًا ثم الصرف والقيد وسياسة الأستاذ | خدمة workflow وربط/تحقق محاسبي | قوائم حسابات/سياسات ونطاقات |
| بوالص وتنفيذ | waybill/query/finance/trip/manifest handlers | API queries + clients + state UI | التهيئة والأمن والترقيم |
| MAUI Admin/Customer/Driver | scaffold وثلاث تجارب استخدام بصلاحيات واضحة | تصميم منصة ثم كود | عقود قراءة وهوية/ملكية |
| أجهزة وoffline وGPS | enrollment/outbox/executor/tracking | تصميم عقد ثم code | أنماط العمليات المسموحة، mobile driver |
| ضمان مستقل | تنفيذ Windows/DB/mobile وربط الحقول والأزرار بالأدلة | تحقق مستقل | نتائج الفرق؛ لا self-pass |

تقسيم الشاشة بحسب التشابه المرئي من اختصاص مراجع الشاشات المكلف؛ هذا التقسيم بحسب اعتماديات التشغيل ولا يستبدل جرده.

## 8. التغير عن المصدر التاريخي

| المجال | مصدر 1026a906 (قديم) | مصدر 9d02ac8 (الحالي) |
|---|---|---|
| الدخول | `FrmLogin.cs:73,396-401` فتح FrmMain مباشرة | `FrmLogin.cs:58-69` جلسة API حقيقية + scope |
| نقل HTTP في Desktop | غير موجود في المسح | DesktopSession + receipt/ledger requests |
| القبض | لا module خاص بworkspace | ReceiptApiModule + ربط UI + attachments/workflow |
| إعدادات الأستاذ | لا GeneralLedgerPolicyService | company policy + document snapshots + GET/PUT |
| الجوال | csproj-only | ما زال csproj-only |
| offline/GPS | غير مكتمل | ما زال غير مكتمل |

هذه المقارنة لا تدعي شمول كل الملفات الـ985 المتغيرة؛ تقتصر على المجالات المعمارية التي أعيد فحصها فعليًا. التقرير الحالي يتقدم على أي ملاحظة أولية صدرت من فحص المصدر القديم.
