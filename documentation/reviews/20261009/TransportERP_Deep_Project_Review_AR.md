# تقرير مراجعة مشروع TransportERP وتوزيع التنفيذ

التاريخ 9 أكتوبر 2026. الفريق الفعلي TEAM-TRANSPORTERP-DEEP-AUDIT-20261009.

المشروع يحتوي نواة خلفية فعلية وربطًا للدخول وسند القبض وإعدادات الأستاذ العام، لكن الربط الشامل والشاشات المالية الأخرى وتطبيقات الجوال وGPS ورحلة البوليصة حتى التسليم لم تكتمل. هذا التقرير يجمع الدراسة المصدرية ونتائج التخصصات وسجل الجرد وتقسيم العمل وأمر التنفيذ التالي، مع إثبات ما أصلح وما بقي.

## النسخة التي بنيت عليها الدراسة

- المستودع https://github.com/shfeekalbhure/TransportERP
- الفرع المصدر feature/receipt-general-ledger-settings
- الالتزام المصدر 9d02ac8d19e39971ad233108faecc8154a1d5745
- تاريخ الالتزام 9 أكتوبر 2026 الساعة 04:34:22 بتوقيت عدن. تاريخ الالتزام لا يثبت وقت رفع Git push.
- فرع التسليم review/project-audit-latest-20261009
- التزام الإصلاحين f13ce501aad2ea6735c0a4bb92f4cd0184c4385c
- نسخة 4 أكتوبر codex/desktop-screens-20261004 عند1026a906 فُحصت للمقارنة. الفرعان يلتقيان عند2ec6cccf؛ لم يجر دمجهما أو تغيير master.

## حدود الدراسة والقبول

الجرد يتناول تعريفات الواجهات والمشاريع المسجلة في المصدر الحالي، والمراجعة الدلالية تتناول المسارات والملفات المحددة في تقارير التخصصات. سجل الأدوات لا يثبت صحة معنى كل حقل أو عمل كل زر. لا توجد نتيجة تشغيل Windows أو قبول Designer أو DPI أو تشغيل كل زر في هذه الجولة؛ dotnet غير مثبت في البيئة. لم يُنشأ مخطط جديد أو قاعدة أو Migration ولم يبدأ قاموس البيانات التنفيذي. الإصلاحان محدودان في الكود القائم ويحفظان التصميم.

مرجع Onyx الحالي قُرئ في بداية هذه المهمة من TransportERP_Foundation_Reference_ONYX_Phase1.xlsx نسخة الملف2 ومحتوى0.3 بتاريخ9 أكتوبر. ملخصه يقرر NOT READY و33 فجوة مفتوحة وعدم اعتماد الانتقال. قراءة الملخص والنطاق والمنهجية والمصادر لا تعني إعادة تدقيق33 فجوة أو مطابقة كل حقل في المرجع. التقرير المستندي السابق يحصي83 شاشة مرجعية جزئية؛ هذا العدد لا يمثل شاشات التنفيذ الحالية أو عدد الشاشات الناقصة في المشروع.

## المراجعين الذين عملوا فعلًا

| المراجع | المهمة |
|---|---|
| architecture | المعمارية ومراجع المشاريع والاتصال والمصادقة والجوال والمزامنة وGPS |
| ui_inventory | حصر الأصناف والشاشات والتبويبات والأدوات والشجرة وتجميع بنى التصميم |
| designer_audit | المصمم والحقول والأزرار والأحداث ومسارات المالية والشركات والشحن |
| code_audit | قواعد المجال والخدمات والحفظ والتزامن والترقيم والتدقيق والتحصيل |
| data_planning | خرائط EF والجداول الحالية والإعدادات وتطابق الحقول وخطة البيانات |
| independent_qa | إعادة تحقق مستقلة لنتائج محورية وتصحيح نطاق الأرقام ومراجعة فرق الإصلاحين |

المنسق جمع الأدلة ونفذ الإصلاحين وراجع الاختبارات النصية. أسماء فرق UI السبعة التالية حزم تنفيذ مقترحة؛ ليست سبعة فرق إضافية تعمل في الخلفية.

## عدد الشاشات وتقسيمها

يوجد16 ملف مشروع و14 مشروعًا في الحل الرئيسي. الجرد يرصد328 تعريف صنف UI:163 واجهة أعمال بها عناصر و9 واجهات أعمال فارغة و119 نافذة مستضيفة، والباقي أدوات وقوالب ودخول وحوارات.163 تعني تنفيذات محتوى متاحة بالمصدر، وتضم بدائل وقديمًا؛ لا تعني163 شاشة مكتملة أو أغراض أعمال فريدة. كل واجهات الأعمال163 لها ملف Designer مقترن.

الشجرة تسجل140 رمزًا فريدًا، ومصنع المحتوى138 حالة تقابل136 صنفًا مختلفًا:126 واجهة أعمال غير فارغة و9 فارغة وتغيير كلمة المرور. رمز الوصول لا يثبت الحفظ أو القبول التشغيلي. لا يُضاعف عدّ Form المستضيف مع UserControl للشاشة نفسها.

| حزمة التنفيذ | البنية المرصودة | العدد |
|---|---|---:|
| UI-01 | حقول وإعدادات دون جدول أو تبويبات | 19 |
| UI-02 | بيانات مرجعية مع جدول | 45 |
| UI-03 | حقول داخل تبويبات | 23 |
| UI-04 | تبويبات وجداول ومنها المستندات المالية | 55 |
| UI-05 | شجرة مع تفاصيل | 5 |
| UI-06 | أوامر شحن مع جدول | 10 |
| UI-07 | أوامر شحن مع حقول | 6 |

مجموعة UI-04 واسعة؛ يبدأ التنفيذ بتقسيمها إلى المستندات المالية ذات رأس وأسطر، وبيانات المؤسسة ذات تبويبات، ووثائق المخزون والمشتريات، بعد مقارنة ترتيب الحاويات والحقول. لا يُستبدل التصميم الحالي بقالب لتوحيد شكلها. أسماء جميع الأصناف وحزمها وأدلة ملفاتها محفوظة في قسم الجرد.

سجل control-inventory يحوي 11750 سجل أداة أو عمود معلن في Designer، مع خصائصه المرصودة وأدلة أسطره والأحداث المباشرة. هذا عدد سجلات جرد؛ ليس عدد حقول أعمال فريدة أو حقول مراجعة دلاليًا كاملة.

## ما أصلح وما اختبر

حُفظ Volume في مسار ConcurrencySafeWaybillRepository الذي كان يسقطه عند استبدال أسطر البوليصة. وصُحح ReverseJournal في نموذج P1 الذاكري ليقلب المدين والدائن دون تغيير الأصل. مسار عكس سند القبض الإنتاجي كان يقلبهما بالفعل؛ لم ينسب إليه هذا العيب.

عُدّل اختبار PostgreSQL القائم ليتحقق من Volume50 بعد الحفظ والاعتماد وقراءة جديدة. وعُدّل اختبار النموذج الذاكري ليتحقق من إلغاء صافي كل حساب والحفاظ على سطور الأصل. مراجع مستقل قرأ الفروق والعقود وأجاز محدودية التغيير مصدرًا فقط.

| الفحص | النتيجة وحدها |
|---|---|
| git diff --check | نجح |
| validate_p0_p1.py | ERROR_COUNT=0 |
| validate_p2_c01_contracts.py | PASS ERROR_COUNT=0 |
| بناء واختبارات .NET | لم تنفذ لغياب SDK |
| PostgreSQL وWindows والجوال وGPS | لم تنفذ اختبارات تشغيل |

نجاح فحوص سجلات العقود لا يثبت نجاح المالية أو الشاشات. بقية النتائج ليست مغلقة بهذا التسليم. التكرار بين تقارير التخصصات يشير إلى نتيجة مشتركة؛ لا تجمع أعدادها كعيوب مستقلة.

## العمل التالي بحسب الأولوية

1. تشغيل الإصلاحين وBuild على .NET10 وWindows وPostgreSQL اختبار منفصلة، وتسجيل SHA ونتيجة كل اختبار.
2. معالجة نطاق تفويض إعدادات القبض والتدقيق وإعادة الطلب والتسلسلات، وربط سند القبض بتحصيل البوليصة بعد حسم منع التكرار والعكس.
3. استكمال endpoints القراءة وadapters للشحن والصرف والقيد والتهيئة ضمن حزم التصميم الموجودة. فتحها من الشجرة واختبار الحفظ وإعادة القراءة والصلاحيات؛ لا تفعيل زر دون خدمة مثبتة.
4. حسم الإعدادات المركزية والمخصصة للتطبيق، وتسجيل الجهاز والمزامنة والتنفيذ دون اتصال، ثم تطبيقات الجوال الحقيقية وGPS.
5. إكمال المرجع التأسيسي واعتماده، ثم نطاق TransportERP، ثم مصفوفة تطابق الحقول وقاموس البيانات، ثم التصميم الجديد للقاعدة والشاشات. لا تبدأ هذه المراحل تلقائيًا من التقرير.

## أمر التنفيذ التالي لفريق Codex

تابع في المستودع نفسه على فرع مستقل من نسخة التسليم. اقرأ هذا التقرير وملاحقه وكود المصدر قبل كل إصلاح. حافظ على تصميم المستخدم والحاويات والتبويبات والحقول الحالية؛ صنف المطلوب إلى إصلاح كود أو ربط أو نقص عقد أو تعديل تصميم يحتاج مرجعًا. امنع التخمين وإغلاق النتائج اعتمادًا على أسماء الأزرار أو نجاح بناء مكتبة فقط.

كل حزمة UI تملك الأصناف المسندة إليها في سجل الشاشات، وتبدأ بالمراجعة لا بإعادة التصميم. فريق تكامل مستقل يملك الرحلات العابرة للحزم: الدخول والنطاق، حفظ القبض وترحيله وعكسه وربطه بالبوليصة، ثم الصرف والقيد، ثم البوليصة والترحيل والوصول والتسليم، ثم الجوال والمزامنة وGPS. لا يجيز فريق شاشة نجاحها دون التحقق من طرف API والحفظ وإعادة القراءة والأثر المالي.

نفذ العيوب المثبتة المحدودة بعد وضع اختبار يكشفها. افصل القرارات المحاسبية مثل تجميد إعدادات السند والفترة والأبعاد وترقيم الفروع عن أخطاء التنفيذ. لا تجر migrations أو تصميم قاعدة جديدة قبل اكتمال المرجع واعتماد الانتقال. لكل نتيجة سجل دليل الملف والسطر والمحفز والأثر والمعالجة ونتيجة الاختبار وSHA. المراجع المستقل يعيد اختبار المسار ويمنع SELF-PASS. ارفع التغييرات والتقرير في فرع التسليم مع طلب مراجعة؛ لا تدمج master تلقائيًا.

## ملاحق الجرد الكاملة

screen-inventory.csv يسجل328 صنفًا وتصنيفه وملف المصمم والعنوان المرصود والتبويبات والربط. screen-navigation-inventory.csv يربط الرموز بالمصنع ومحتوى الواجهة. control-inventory.csv يسجل الأدوات والأعمدة والخصائص والأحداث وأدلة الأسطر وحدود الاستخراج. الملفات الثلاثة مضغوطة معًا في Source_Inventories.zip داخل مسار التقرير نفسه. سجل الأدوات لا يتضمن ادعاء اكتمال الإنشاء الديناميكي أو تفويض الأوامر أو الفحص البصري.

الأقسام التالية تحتفظ بنصوص نتائج التخصصات وأدلتها، مع الملاحظات المستقلة والتصحيحات التي تلقاها المنسق.


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


# جرد واجهات TransportERP وتقسيم العمل حسب بنية التصميم

نسخة المصدر: `9d02ac8d19e39971ad233108faecc8154a1d5745`، الفرع `feature/receipt-general-ledger-settings`. فحص المصدر ساكن؛ لم يُشغّل WinForms ولم تُعتمد سلامة التصميم أو وظائف الحفظ.

## منهج العد

عُدّت تعريفات أصناف Form وUserControl بما فيها الوراثة غير المباشرة؛ جُمعت ملفات partial والمصمم داخل الدليل نفسه. عدد التعريفات الفيزيائية هو 328، وليس عدد الشاشات المكتملة. استُبعدت أغلفة Form والقوالب والأدوات المشتركة والنوافذ المساعدة من عدد واجهات الأعمال. اختلاف صنفين لشاشة ذات غرض واحد يظل تنفيذين مستقلين ولا يُحسم دمجهما آليًا. مصادر CoreUI والقوالب مستبعدة من تجميع Desktop المباشر لكنها مرتبطة ضمن SharedVisuals، فلا تُعد مفقودة.

| الفئة | عدد تعريفات الأصناف |
|---|---:|
| أصناف أساس | 4 |
| واجهات أعمال ذات عناصر معلنة؛ وجودها لا يعني اكتمالها | 163 |
| أغلفة Form تحوي واجهة أو تقبلها من المستدعي | 119 |
| نوافذ حوار واختيار نطاق وإعدادات مساعدة | 6 |
| واجهات أعمال بلا حقول؛ تحتوي تذييل تدقيق مشترك فقط | 9 |
| واجهات اختيار واستعلام مساعدة | 7 |
| أدوات مشتركة/أداة صف | 7 |
| الرئيسية ولوحة المعلومات والدخول وتغيير كلمة المرور | 5 |
| قوالب | 6 |
| نموذج تحقق | 1 |
| مضيف تحقق معزول | 1 |

واجهات الأعمال غير الفارغة: **163**. واجهات الأعمال الفارغة: **9**. مجموع أصناف محتوى الأعمال 172؛ يشمل تنفيذات قديمة أو بديلة غير مستهدفة بالمصنع النشط.

مصنع المحتوى النشط يعرّف 138 حالات رمز، تقابل 136 أصناف محتوى مميزة. تعدد رموز المستخدمين/الأدوار/الصلاحيات لا يساوي ثلاث شاشات مستقلة. الوصول إلى رمز لا يثبت أن نافذته قابلة للاستخدام.

## مجموعات التصميم المرصودة والتكليف المقترح

| الفريق المقترح | البنية المرصودة | واجهات أعمال |
|---|---|---:|
| UI-01-FIELDS | FIELDS_SETTINGS | 19 |
| UI-02-MASTER-GRID | MASTER_GRID | 45 |
| UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 6 |
| UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 10 |
| UI-03-TABBED-FIELDS | TABBED_FIELDS | 23 |
| UI-04-TABBED-GRID | TABBED_GRID | 55 |
| UI-05-TREE-DETAIL | TREE_DETAIL | 5 |

التقسيم مبني على وجود TreeView وTabPage وDataGridView فعليًا في التعريفات؛ مجموعة TABBED_GRID واسعة وتحتاج تقسيمًا فرعيًا بعد مقارنة ترتيب الحاويات وأعمدة السندات والمخزون. أسماء فرق الجدول تكليف مقترح للتنفيذ اللاحق وليست فرقًا جديدة شُغّلت في هذا الجرد. لكل فريق مراجعة الحاويات ثم الحقول ثم الأعمدة والأزرار ثم الأحداث ثم الوصول API؛ يجب إبقاء التصميم الحالي وإثبات أي نقص بمرجع مستقل.

## الواجهات الفارغة بأدلة

- `UcOnyxSCREEN0092` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0092.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0092.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0104` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0104.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0104.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0105` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0105.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0105.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0106` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0106.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0106.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_01_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_01_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_01_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_02_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_02_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_02_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_03_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_03_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_03_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_04_11_06` — TransportERP.EmptyForms/Forms/FrmScreen_04_11_06.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_04_11_06.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_04_11_07` — TransportERP.EmptyForms/Forms/FrmScreen_04_11_07.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_04_11_07.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.

## قيود الجرد

عدد العناصر في screen-inventory.csv هو عدد الأسماء المرصودة في تعريفات المصدر، وقد يشمل متغيرات مساعدة داخل الدوال. control-inventory.csv أضيق: العناصر والأعمدة المعلنة في ملفات Designer، مع القيم الحرفية والأحداث المباشرة في المصمم والملفات المكملة. الإنشاء المتأخر والمصانع والأوامر المفوضة وAddRange وتغيير الخصائص في التشغيل تحتاج متابعة. عدم وجود Click مباشر لا يثبت أن الزر غير مربوط. لون الحقل مسجل كما هو دون افتراض أنه إلزامي. تعليق الملخص أو عنوان Label موسوم باعتباره مصدرًا احتياطيًا؛ لا يثبت عنوان النافذة وقت التشغيل.

## جميع واجهات الأعمال وتقسيمها

| الصنف | الفريق | بنية التصميم | تبويبات معلنة | عناصر مسماة | مصنع المحتوى النشط | دليل المصدر |
|---|---|---|---:|---:|---|---|
| UcPrintSettings | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 63 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/FrmPrintSettings.cs:5 |
| UcCityData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 36 | 02.02.07 | TransportERP.Desktop/Forms/SystemSettings/General/UcCityData.cs:6 |
| UcCountryData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 48 | 02.02.04 | TransportERP.Desktop/Forms/SystemSettings/General/UcCountryData.cs:7 |
| UcDefaultAccountingCharts | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 38 | INPUT:DEFAULT-CHARTS | TransportERP.Desktop/Forms/SystemSettings/General/UcDefaultAccountingCharts.cs:6 |
| UcGeneralVariables | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 27 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralVariables.cs:7 |
| UcGovernorateData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 36 | 02.02.05 | TransportERP.Desktop/Forms/SystemSettings/General/UcGovernorateData.cs:6 |
| UcInternationalRegions | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 31 | SETUP:INTERNATIONAL-REGIONS | TransportERP.Desktop/Forms/SystemSettings/General/UcInternationalRegions.cs:6 |
| UcOtherAccountGroups | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 30 | INPUT:OTHER-ACCOUNT-GROUPS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAccountGroups.cs:6 |
| UcOtherAccountLinks | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 34 | INPUT:OTHER-ACCOUNT-LINKS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAccountLinks.cs:6 |
| UcOtherAnalyticalAccounts | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 40 | INPUT:OTHER-ANALYTICAL-ACCOUNTS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAnalyticalAccounts.cs:6 |
| UcShipmentBooks | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 61 | SHIP:011 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentBooks.cs:11 |
| UcShipmentContentTypes | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentContentTypes.cs:11 |
| UcShipmentUnits | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 53 | SHIP:003 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentUnits.cs:11 |
| UcAccountLinking | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 77 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/ربط_الحسابات/UcAccountLinking.cs:11 |
| UcOnyxSCREEN0079 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 30 | ONYX:SCREEN-0079 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0079.cs:4 |
| UcOnyxSCREEN0084 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 41 | ONYX:SCREEN-0084 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0084.cs:4 |
| UcOnyxSCREEN0085 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 34 | ONYX:SCREEN-0085 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0085.cs:4 |
| UcAccountOpeningRequest | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 51 | ONYX:SCREEN-0086 | TransportERP.EmptyForms/Forms/UcAccountOpeningRequest.cs:6 |
| UcCustomerDebtReport | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 84 | CUS:DEBT-REPORT | TransportERP.EmptyForms/Forms/UcCustomerDebtReport.cs:5 |
| UcActivitySetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:ACTIVITIES | TransportERP.Desktop/Forms/SystemSettings/General/UcActivitySetup.cs:5 |
| UcAdditionalFieldSetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:ADDITIONAL-FIELDS | TransportERP.Desktop/Forms/SystemSettings/General/UcAdditionalFieldSetup.cs:6 |
| UcAdministrativeStructureTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:STRUCTURE-TYPES | TransportERP.Desktop/Forms/SystemSettings/General/UcAdministrativeStructureTypes.cs:5 |
| UcApprovalLevels | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:APPROVAL-LEVELS | TransportERP.Desktop/Forms/SystemSettings/General/UcApprovalLevels.cs:8 |
| UcAreaData | UI-02-MASTER-GRID | MASTER_GRID | 0 | 34 | 02.02.08 | TransportERP.Desktop/Forms/SystemSettings/General/UcAreaData.cs:6 |
| UcBranchGroups | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:BRANCH-GROUPS | TransportERP.Desktop/Forms/SystemSettings/General/UcBranchGroups.cs:6 |
| UcEmployeeGeneralCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 26 | SETUP:EMPLOYEE-CODES | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeGeneralCodes.cs:6 |
| UcEmployeeProfessionLinks | UI-02-MASTER-GRID | MASTER_GRID | 0 | 29 | INPUT:EMPLOYEE-PROFESSIONS | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeProfessionLinks.cs:6 |
| UcExchangeRates | UI-02-MASTER-GRID | MASTER_GRID | 0 | 80 | 02.04.03 | TransportERP.Desktop/Forms/SystemSettings/General/UcExchangeRates.cs:8 |
| UcGeneralCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 28 | SETUP:GENERAL-CODES | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralCodes.cs:7 |
| UcProjectExtraFieldCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:PROJECT-EXTRA-FIELDS | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectExtraFieldCodes.cs:6 |
| UcProjectSetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:PROJECTS | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectSetup.cs:5 |
| UcScreenBackgrounds | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:SCREEN-BACKGROUNDS | TransportERP.Desktop/Forms/SystemSettings/General/UcScreenBackgrounds.cs:6 |
| UcSystemPeriods | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | 04.11.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcSystemPeriods.cs:7 |
| UcTaxBracketCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:TAX-BRACKETS | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxBracketCodes.cs:6 |
| UcTaxTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | SETUP:TAX-TYPES | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxTypes.cs:7 |
| UcTextTranslation | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:TEXT-TRANSLATION | TransportERP.Desktop/Forms/SystemSettings/General/UcTextTranslation.cs:7 |
| UcShipmentCategories | UI-02-MASTER-GRID | MASTER_GRID | 0 | 59 | SHIP:002 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentCategories.cs:8 |
| dgvShipmentPriorities | UI-02-MASTER-GRID | MASTER_GRID | 0 | 48 | SHIP:004 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentPriorities.cs:11 |
| UcShipmentTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 45 | SHIP:001 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentTypes.cs:11 |
| UcOnyxSCREEN0080 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 48 | ONYX:SCREEN-0080 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0080.cs:5 |
| UcOnyxSCREEN0089 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 36 | ONYX:SCREEN-0089 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0089.cs:4 |
| UcOnyxSCREEN0090 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 27 | ONYX:SCREEN-0090 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0090.cs:4 |
| UcOnyxSCREEN0091 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 66 | ONYX:SCREEN-0091 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0091.cs:4 |
| UcOnyxSCREEN0102 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 52 | ONYX:SCREEN-0102 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0102.cs:6 |
| UcScreen_02_02_04 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 61 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_04.cs:5 |
| UcScreen_02_02_05 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_05.cs:5 |
| UcScreen_02_02_06 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | 02.02.06 | TransportERP.EmptyForms/Forms/FrmScreen_02_02_06.cs:5 |
| UcScreen_02_02_07 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_07.cs:5 |
| UcScreen_02_02_08 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_08.cs:5 |
| UcScreen_02_04_06 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 54 | 02.04.06 | TransportERP.EmptyForms/Forms/FrmScreen_02_04_06.cs:4 |
| UcScreen_02_05_21 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 53 | 02.05.21 | TransportERP.EmptyForms/Forms/FrmScreen_02_05_21.cs:5 |
| UcScreen_04_03_04 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 73 | 04.03.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_04.cs:4 |
| UcScreen_04_11_01 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 59 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_04_11_01.cs:4 |
| UcAccountCostCenterLinking | UI-02-MASTER-GRID | MASTER_GRID | 0 | 46 | ONYX:SCREEN-0093 | TransportERP.EmptyForms/Forms/UcAccountCostCenterLinking.cs:6 |
| UcAccountProjectLinking | UI-02-MASTER-GRID | MASTER_GRID | 0 | 46 | ONYX:SCREEN-0094 | TransportERP.EmptyForms/Forms/UcAccountProjectLinking.cs:6 |
| UcCustomerInstallmentSettlement | UI-02-MASTER-GRID | MASTER_GRID | 0 | 41 | CUS:INSTALLMENT-SETTLEMENT | TransportERP.EmptyForms/Forms/UcCustomerInstallmentSettlement.cs:6 |
| UcForeignPurchaseCosting | UI-02-MASTER-GRID | MASTER_GRID | 0 | 29 | PUR:FOREIGN-COSTING | TransportERP.EmptyForms/Forms/UcForeignPurchaseCosting.cs:6 |
| UcForeignPurchaseReceipt | UI-02-MASTER-GRID | MASTER_GRID | 0 | 32 | PUR:FOREIGN-RECEIPT | TransportERP.EmptyForms/Forms/UcForeignPurchaseReceipt.cs:6 |
| UcInventoryCustodyIssue | UI-02-MASTER-GRID | MASTER_GRID | 0 | 37 | INV:CUSTODY-ISSUE | TransportERP.EmptyForms/Forms/UcInventoryCustodyIssue.cs:4 |
| UcInventoryCustodyReceipt | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | INV:CUSTODY-RECEIPT | TransportERP.EmptyForms/Forms/UcInventoryCustodyReceipt.cs:4 |
| UcInventoryDamagedIssueOrder | UI-02-MASTER-GRID | MASTER_GRID | 0 | 26 | INV:DAMAGED-ISSUE | TransportERP.EmptyForms/Forms/UcInventoryDamagedIssueOrder.cs:4 |
| UcInventoryExternalRepairOrder | UI-02-MASTER-GRID | MASTER_GRID | 0 | 18 | INV:EXTERNAL-REPAIR | TransportERP.EmptyForms/Forms/UcInventoryExternalRepairOrder.cs:4 |
| UcInventoryQuantityReservation | UI-02-MASTER-GRID | MASTER_GRID | 0 | 12 | INV:QUANTITY-RESERVATION | TransportERP.EmptyForms/Forms/UcInventoryQuantityReservation.cs:4 |
| UcPurchaseRequest | UI-02-MASTER-GRID | MASTER_GRID | 0 | 45 | PUR:REQUEST | TransportERP.EmptyForms/Forms/UcPurchaseRequest.cs:6 |
| UcItemRelease | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 38 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcItemRelease.cs:6 |
| UcManifestHandover | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifestHandover.cs:6 |
| UcTripDeparture | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTripDeparture.cs:6 |
| UcWaybillApproval | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 26 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillApproval.cs:5 |
| UcWaybillFinancialStatus | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillFinancialStatus.cs:5 |
| UcWaybillPricing | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 28 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillPricing.cs:5 |
| UcLoadPlanning | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 28 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcLoadPlanning.cs:6 |
| UcManifest | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 38 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifest.cs:6 |
| UcManifestLoading | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifestLoading.cs:6 |
| UcReadyToLoadWaybills | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 24 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcReadyToLoadWaybills.cs:6 |
| UcRemainingShippingQuantity | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcRemainingShippingQuantity.cs:6 |
| UcTrip | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 39 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTrip.cs:6 |
| UcTripAllocation | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 30 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTripAllocation.cs:6 |
| UcWaybillCollections | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 22 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillCollections.cs:5 |
| UcWaybillDraft | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 4 | 53 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillDraft.cs:5 |
| UcWaybillPaymentPlan | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillPaymentPlan.cs:5 |
| UcActivityData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 45 | INPUT:ACTIVITY-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcActivityData.cs:7 |
| UcBranchData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 6 | 146 | 02.02.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcBranchData.cs:8 |
| UcCompanyData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 59 | 02.01.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyData.cs:4 |
| UcCompanyManagement | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 82 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.cs:8 |
| UcCostCenterData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 62 | 04.02.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcCostCenterData.cs:6 |
| UcGeneralVariablesSetup | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 137 | 02.04.07 | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralVariablesSetup.cs:4 |
| UcIntermediateAccounts | UI-03-TABBED-FIELDS | TABBED_FIELDS | 9 | 216 | INPUT:INTERMEDIATE-ACCOUNTS | TransportERP.Desktop/Forms/SystemSettings/General/UcIntermediateAccounts.cs:5 |
| UcProjectData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 89 | INPUT:PROJECT-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectData.cs:6 |
| UcBranchManagement | UI-03-TABBED-FIELDS | TABBED_FIELDS | 6 | 95 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/الفروع/UcBranchManagement.cs:6 |
| UcPackagingTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 58 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcPackagingTypes.cs:11 |
| UcShipmentPricingRules | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 70 | SHIP:009 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentPricingRules.cs:11 |
| dgvShipmentServiceFees | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 79 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentServiceFees.cs:11 |
| UcShipmentStatuses | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 63 | SHIP:010 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentStatuses.cs:11 |
| UcShipmentTransportMethods | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 48 | SHIP:007 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentTransportMethods.cs:11 |
| UcTransportGroups | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 44 | SHIP:006 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcTransportGroups.cs:11 |
| UcTransportTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 43 | SHIP:005 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcTransportTypes.cs:11 |
| UcDriverTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 57 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الشاشات المشتركة/UcDriverTypes.cs:11 |
| UcShipmentDispatch | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 177 | SHIP:013 | TransportERP.Desktop/البوالص_والشحن/العمليات/الشحن_والترحيل/UcShipmentDispatch.cs:7 |
| UcWarehouseReceiptOrder | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 52 | SHIP:014 | TransportERP.Desktop/البوالص_والشحن/العمليات/توريد_مخزني/UcWarehouseReceiptOrder.cs:11 |
| UcScreen_04_11_02 | UI-03-TABBED-FIELDS | TABBED_FIELDS | 5 | 59 | 04.11.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_02.cs:3 |
| UcScreen_04_11_05 | UI-03-TABBED-FIELDS | TABBED_FIELDS | 4 | 60 | 04.11.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_05.cs:3 |
| UcAccountGroupsAndTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/UcAccountGroupsAndTypes.cs:6 |
| UcInventoryStocktakeReport | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 86 | INV:STOCKTAKE-REPORT | TransportERP.EmptyForms/Forms/UcInventoryStocktakeReport.cs:5 |
| UcDocumentNumbering | UI-04-TABBED-GRID | TABBED_GRID | 4 | 99 | 02.04.01 | TransportERP.Desktop/Forms/SystemSettings/DocumentControl/UcDocumentNumbering.cs:9 |
| UcGeneralSettings | UI-04-TABBED-GRID | TABBED_GRID | 8 | 102 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/FrmGeneralSettings.cs:5 |
| UcCashFlowAccountLinks | UI-04-TABBED-GRID | TABBED_GRID | 2 | 29 | INPUT:CASHFLOW-ACCOUNT-LINKS | TransportERP.Desktop/Forms/SystemSettings/General/UcCashFlowAccountLinks.cs:6 |
| UcChartSetup | UI-04-TABBED-GRID | TABBED_GRID | 4 | 38 | 04.01.02 | TransportERP.Desktop/Forms/SystemSettings/General/UcChartSetup.cs:6 |
| UcCostCenterSetup | UI-04-TABBED-GRID | TABBED_GRID | 2 | 30 | SETUP:COST-CENTERS | TransportERP.Desktop/Forms/SystemSettings/General/UcCostCenterSetup.cs:6 |
| UcCurrencyManagement | UI-04-TABBED-GRID | TABBED_GRID | 3 | 65 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcCurrencyManagement.cs:7 |
| UcCurrencySetup | UI-04-TABBED-GRID | TABBED_GRID | 3 | 124 | 02.04.02 | TransportERP.Desktop/Forms/SystemSettings/General/UcCurrencySetup.cs:10 |
| UcElectronicInvoiceSequences | UI-04-TABBED-GRID | TABBED_GRID | 1 | 37 | SETUP:EINVOICE-SEQUENCES | TransportERP.Desktop/Forms/SystemSettings/General/UcElectronicInvoiceSequences.cs:6 |
| UcEmployeeData | UI-04-TABBED-GRID | TABBED_GRID | 26 | 314 | INPUT:EMPLOYEE-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeData.cs:4 |
| UcTaxCalculationMethods | UI-04-TABBED-GRID | TABBED_GRID | 2 | 47 | SETUP:TAX-METHODS | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxCalculationMethods.cs:7 |
| UcTextSetup | UI-04-TABBED-GRID | TABBED_GRID | 1 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcTextSetup.cs:6 |
| UcRoutes | UI-04-TABBED-GRID | TABBED_GRID | 2 | 63 | SHIP:008 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcRoutes.cs:11 |
| UcShipmentWaybill | UI-04-TABBED-GRID | TABBED_GRID | 13 | 250 | SHIP:012 | TransportERP.Desktop/البوالص_والشحن/العمليات/البوليصه/UcShipmentWaybill.cs:11 |
| UcShipmentTransfer | UI-04-TABBED-GRID | TABBED_GRID | 4 | 69 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/العمليات/ترحيل_الشحنات/UserControl1.cs:11 |
| UcOnyxSCREEN0081 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 42 | ONYX:SCREEN-0081 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0081.cs:4 |
| UcOnyxSCREEN0082 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 47 | ONYX:SCREEN-0082 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0082.cs:4 |
| UcOnyxSCREEN0083 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 55 | ONYX:SCREEN-0083 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0083.cs:4 |
| UcOnyxSCREEN0098 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 125 | ONYX:SCREEN-0098 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0098.cs:3 |
| UcOnyxSCREEN0100 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 124 | ONYX:SCREEN-0100 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0100.cs:3 |
| UcOnyxSCREEN0103 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 35 | ONYX:SCREEN-0103 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0103.cs:3 |
| UcOnyxSCREEN0110 | UI-04-TABBED-GRID | TABBED_GRID | 1 | 51 | ONYX:SCREEN-0110 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0110.cs:5 |
| UcOnyxSCREEN0111 | UI-04-TABBED-GRID | TABBED_GRID | 1 | 51 | ONYX:SCREEN-0111 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0111.cs:5 |
| UcScreen_04_03_01 | UI-04-TABBED-GRID | TABBED_GRID | 6 | 83 | 04.03.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_01.cs:5 |
| UcScreen_04_03_02 | UI-04-TABBED-GRID | TABBED_GRID | 4 | 97 | 04.03.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_02.cs:3 |
| UcScreen_04_03_03 | UI-04-TABBED-GRID | TABBED_GRID | 6 | 88 | 04.03.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_03.cs:4 |
| UcScreen_04_04_01 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 160 | 04.04.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_01.cs:3 |
| UcScreen_04_04_02 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 117 | 04.04.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_02.cs:4 |
| UcScreen_04_04_03 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_03.cs:3 |
| UcScreen_04_04_04 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_04.cs:3 |
| UcScreen_04_04_05 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_05.cs:3 |
| UcScreen_04_04_06 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.06 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_06.cs:3 |
| UcScreen_04_05_01 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 137 | 04.05.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_05_01.cs:3 |
| UcScreen_04_07_16 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 101 | 04.07.16 | TransportERP.EmptyForms/Forms/FrmScreen_04_07_16.cs:4 |
| UcScreen_04_08_01 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 87 | 04.08.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_01.cs:3 |
| UcScreen_04_08_02 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 87 | 04.08.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_02.cs:3 |
| UcScreen_04_08_05 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 100 | 04.08.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_05.cs:3 |
| UcScreen_04_09_06 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 80 | 04.09.06 | TransportERP.EmptyForms/Forms/FrmScreen_04_09_06.cs:3 |
| UcScreen_04_10_03 | UI-04-TABBED-GRID | TABBED_GRID | 3 | 58 | 04.10.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_03.cs:3 |
| UcScreen_04_10_04 | UI-04-TABBED-GRID | TABBED_GRID | 3 | 56 | 04.10.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_04.cs:3 |
| UcScreen_04_10_05 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 44 | 04.10.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_05.cs:3 |
| UcScreen_04_11_03 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 58 | 04.11.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_03.cs:3 |
| UcScreen_04_11_04 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 64 | 04.11.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_04.cs:3 |
| UcGeneralLedgerSettings | UI-04-TABBED-GRID | TABBED_GRID | 5 | 49 | ONYX:SCREEN-0078 | TransportERP.EmptyForms/Forms/UcGeneralLedgerSettings.cs:10 |
| UcInventoryIssueOrder | UI-04-TABBED-GRID | TABBED_GRID | 2 | 76 | INV:ISSUE-ORDER | TransportERP.EmptyForms/Forms/UcInventoryIssueOrder.cs:6 |
| UcInventoryManualStocktake | UI-04-TABBED-GRID | TABBED_GRID | 1 | 27 | INV:MANUAL-STOCKTAKE | TransportERP.EmptyForms/Forms/UcInventoryManualStocktake.cs:6 |
| UcInventoryMaterialRequest | UI-04-TABBED-GRID | TABBED_GRID | 2 | 57 | INV:MATERIAL-REQUEST | TransportERP.EmptyForms/Forms/UcInventoryMaterialRequest.cs:6 |
| UcInventoryReceiptAuthorization | UI-04-TABBED-GRID | TABBED_GRID | 1 | 37 | INV:RECEIPT-AUTHORIZATION | TransportERP.EmptyForms/Forms/UcInventoryReceiptAuthorization.cs:6 |
| UcInventoryReceiptOrder | UI-04-TABBED-GRID | TABBED_GRID | 2 | 68 | INV:RECEIPT-ORDER | TransportERP.EmptyForms/Forms/UcInventoryReceiptOrder.cs:6 |
| UcInventorySettlement | UI-04-TABBED-GRID | TABBED_GRID | 2 | 84 | INV:SETTLEMENT | TransportERP.EmptyForms/Forms/UcInventorySettlement.cs:6 |
| UcInventoryTransfer | UI-04-TABBED-GRID | TABBED_GRID | 2 | 70 | INV:TRANSFER | TransportERP.EmptyForms/Forms/UcInventoryTransfer.cs:6 |
| UcInventoryTransferReceipt | UI-04-TABBED-GRID | TABBED_GRID | 1 | 38 | INV:TRANSFER-RECEIPT | TransportERP.EmptyForms/Forms/UcInventoryTransferReceipt.cs:6 |
| UcPurchaseAdditionalDiscount | UI-04-TABBED-GRID | TABBED_GRID | 1 | 66 | SUP:ADDITIONAL-PURCHASE-DISCOUNT | TransportERP.EmptyForms/Forms/UcPurchaseAdditionalDiscount.cs:6 |
| UcPurchaseComparisonNomination | UI-04-TABBED-GRID | TABBED_GRID | 1 | 39 | PUR:COMPARISON-NOMINATION | TransportERP.EmptyForms/Forms/UcPurchaseComparisonNomination.cs:6 |
| UcPurchaseTechnicalComparison | UI-04-TABBED-GRID | TABBED_GRID | 1 | 32 | PUR:TECHNICAL-COMPARISON | TransportERP.EmptyForms/Forms/UcPurchaseTechnicalComparison.cs:6 |
| UcRepresentativeCommissions | UI-04-TABBED-GRID | TABBED_GRID | 2 | 48 | CUS:REPRESENTATIVE-COMMISSIONS | TransportERP.EmptyForms/Forms/UcRepresentativeCommissions.cs:5 |
| UcUsersPermissions | UI-05-TREE-DETAIL | TREE_DETAIL | 5 | 129 | 02.03.01;02.03.02;02.03.03 | TransportERP.Desktop/Forms/Setup/Security/FrmUsersPermissions.cs:7 |
| UcAccountingChartData | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 105 | 04.01.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcAccountingChartData.cs:5 |
| UcAdministrativeStructure | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 42 | INPUT:ADMINISTRATIVE-STRUCTURE | TransportERP.Desktop/Forms/SystemSettings/General/UcAdministrativeStructure.cs:6 |
| UcScreen_04_02_01 | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 67 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_04_02_01.cs:6 |
| UcChartOfAccounts | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 73 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/UcChartOfAccounts.cs:7 |

## محتوى المصنع النشط مقابل التنفيذات الأخرى

مصنع المحتوى النشط يستهدف 126 واجهة أعمال غير فارغة و9 واجهات أعمال فارغة وواجهة تغيير كلمة المرور؛ يعرّف 138 حالة رمز بسبب رموز المستخدمين والأدوار والصلاحيات التي تفتح الصنف نفسه. عدد الرموز أو تعريفات الأصناف لا يعبر عن اكتمال العمل.

| بنية محتوى المصنع النشط | عدد أصناف الأعمال غير الفارغة |
|---|---:|
| FIELDS_SETTINGS | 15 |
| MASTER_GRID | 40 |
| TABBED_FIELDS | 17 |
| TABBED_GRID | 51 |
| TREE_DETAIL | 3 |

يوجد 37 تنفيذ محتوى أعمال غير مستهدف مباشرة بالمصنع النشط. بعضه يُستخدم داخل واجهة أخرى أو يحتفظ به المضيف القديم، لذلك لا يجوز اعتباره ميتًا أو حذفه دون تتبع كل المراجع:

- `UcPrintSettings` — FrmPrintSettings.cs:5.
- `UcGeneralVariables` — UcGeneralVariables.cs:7.
- `UcShipmentContentTypes` — UcShipmentContentTypes.cs:11.
- `UcAccountLinking` — UcAccountLinking.cs:11.
- `UcScreen_02_02_04` — FrmScreen_02_02_04.cs:5.
- `UcScreen_02_02_05` — FrmScreen_02_02_05.cs:5.
- `UcScreen_02_02_07` — FrmScreen_02_02_07.cs:5.
- `UcScreen_02_02_08` — FrmScreen_02_02_08.cs:5.
- `UcScreen_04_11_01` — FrmScreen_04_11_01.cs:4.
- `UcItemRelease` — UcItemRelease.cs:6.
- `UcManifestHandover` — UcManifestHandover.cs:6.
- `UcTripDeparture` — UcTripDeparture.cs:6.
- `UcWaybillApproval` — UcWaybillApproval.cs:5.
- `UcWaybillFinancialStatus` — UcWaybillFinancialStatus.cs:5.
- `UcWaybillPricing` — UcWaybillPricing.cs:5.
- `UcLoadPlanning` — UcLoadPlanning.cs:6.
- `UcManifest` — UcManifest.cs:6.
- `UcManifestLoading` — UcManifestLoading.cs:6.
- `UcReadyToLoadWaybills` — UcReadyToLoadWaybills.cs:6.
- `UcRemainingShippingQuantity` — UcRemainingShippingQuantity.cs:6.
- `UcTrip` — UcTrip.cs:6.
- `UcTripAllocation` — UcTripAllocation.cs:6.
- `UcWaybillCollections` — UcWaybillCollections.cs:5.
- `UcWaybillDraft` — UcWaybillDraft.cs:5.
- `UcWaybillPaymentPlan` — UcWaybillPaymentPlan.cs:5.
- `UcCompanyManagement` — UcCompanyManagement.cs:8.
- `UcBranchManagement` — UcBranchManagement.cs:6.
- `UcPackagingTypes` — UcPackagingTypes.cs:11.
- `dgvShipmentServiceFees` — UcShipmentServiceFees.cs:11.
- `UcDriverTypes` — UcDriverTypes.cs:11.
- `UcAccountGroupsAndTypes` — UcAccountGroupsAndTypes.cs:6.
- `UcGeneralSettings` — FrmGeneralSettings.cs:5.
- `UcCurrencyManagement` — UcCurrencyManagement.cs:7.
- `UcTextSetup` — UcTextSetup.cs:6.
- `UcShipmentTransfer` — UserControl1.cs:11.
- `UcScreen_04_02_01` — FrmScreen_04_02_01.cs:6.
- `UcChartOfAccounts` — UcChartOfAccounts.cs:7.

## تفاصيل عناصر المصمم

تم توثيق 11,750 اسم عنصر/عمود/مكون في 214 ملف مصمم يحتوي عناصر، منها 10,979 اسمًا تابعًا لواجهات الأعمال 163. سجل control-inventory.csv يتضمن النص والمقاس والموقع والرسو واللون والظهور والتفعيل وقراءة فقط والخصائص الحرفية وأدلة ملفات المصدر؛ رُصدت أحداث مباشرة لـ519 عنصرًا. لا تساوي هذه الأرقام اختبارات وظائف أو عدد عناصر ظاهرة للمستخدم.

الواجهات التسع المصنفة فارغة تجاريًا تضم تذييل تدقيق فقط؛ لذا فحص Controls.Count>0 قد يعطي إشارة مضللة عن جاهزيتها. يجب قياس وجود حقول العمل ومسار الحفظ.


# مراجعة المصدر لشاشات Windows Forms — المراجع الفرعي للتصميم والأحداث

**نسخة الحكم:** `9d02ac8d19e39971ad233108faecc8154a1d5745`، فرع `feature/receipt-general-ledger-settings`.
**المسار المقروء:** `جذر نسخة المصدر`.
**طبيعة العمل:** قراءة المصدر والمصمم ومسار الاستدعاء فقط. لم أعدل الشاشات، ولم أشغّل Windows Forms أو المصمم في Windows. لا يوجد حكم قبول بصري أو تشغيل إنتاجي من هذه المراجعة.
**حدود النطاق:** سند القبض، سند الصرف، قيد اليومية، شاشات الشركات والفروع الجديدة والمحفوظة، البوليصة، الشحن والترحيل، صف البوليصة، التوريد المخزني، إنشاء الرحلة. جرد جميع الشاشات والحقول بالمستودع مسند إلى مراجع آخر؛ هذا الملحق يفصل إجراءات الشاشات المذكورة ومسارها.

## نتيجة المراجعة

المصدر الجديد يحوي شاشات قبض وصرف وقيد يومية فعلية من نوع UserControl مع مصمم وأزرار وتبويبات، خلاف النسخة القديمة بتاريخ 4 أكتوبر التي كانت تحوي قوالب مالية فارغة. سند القبض وحده يملك في مسار فتح الشاشة ربطًا واضحًا بخدمة تشغيلية للحفظ والعرض والتعديل والإلغاء والمراجعة والاعتماد والترحيل والعكس والمرفقات. سند الصرف وقيد اليومية يملكان عقد ربط محليًا، لكن لا يوجد في مصدر التطبيق استدعاء يربط أوامرهما بخدمة مالية؛ تحميل إعداد إلزام البيان في القيد لا يعد ربطًا لحفظ القيد.

الشجرة تستدعي `UcCompanyData` و`UcBranchData` الجديدتين بدل `UcCompanyManagement` و`UcBranchManagement` المحفوظتين. هذه نقطة قرار تصميم يجب توثيقها مع المالك؛ لا يجوز دمج حقول النموذجين عشوائيًا أو حذف النسخة المحفوظة. معظم الوظائف في شاشتي بيانات الشركات والفروع هي معاينة إدخال محلية، مع أزرار حفظ غير مرتبطة. فتح شاشة من الشجرة لا يثبت أنها تحفظ أو أن كل أدواتها تعمل.

## سجل النتائج المثبتة وخطة المعالجة

| المعرف | النتيجة المثبتة | الدليل | النوع والمعالجة المطلوبة |
|---|---|---|---|
| UI-01 | حفظ/تعديل/إلغاء/ترحيل/عكس سند الصرف غير موصول في التطبيق؛ الأزرار معطلة صراحة في المصمم وبواسطة جلسة الربط | `TransportERP.EmptyForms/Forms/FrmScreen_04_04_02.cs:30-52` يهيئ Binding فقط؛ `TransportERP.EmptyForms/Forms/BatchFiveUiSession.cs:167-197` يشترط handler وallowed؛ `TransportERP.Desktop/FrmMain.ReceiptWorkspace.cs:13-45` يربط القبض فقط | **ربط برمجي**: عقد صريح للسند والصلاحيات والحالات وVersion؛ لا يلزم استبدال التصميم |
| UI-02 | قيد اليومية له محررات واستيراد محلي، لكن أوامر الحفظ والترحيل والعكس غير موصولة؛ ربط الخادم يقتصر على إعداد إلزام البيان | `FrmScreen_04_05_01.cs:64-103`؛ `FrmMain.LedgerSettings.cs:13-23`؛ لا يوجد استدعاء `Binding.BindCommands` للقيد في المصدر | **ربط برمجي** مع إثبات التوازن ومنع تعديل المرحل والربط بالفرع/الفترة؛ اختبارات API ثم Windows |
| UI-03 | الشجرة الفعلية لا تفتح تصميم الشركات والفروع المحفوظ القديم | `FrmMain.PhaseOneNavigation.cs:494-497` يختار UcCompanyData/UcBranchData؛ `FrmMain.cs:578` يفتح CreateConnectedWorkspaceControl | **قرار مصدر واجهة**: مقارنة النموذجين واعتماد المسار الذي يحافظ على تصميم المالك؛ لا يعد بحد ذاته خطأ بناء |
| UI-04 | بيانات الشركات الجديدة معاينة محلية؛ الربط والفاتورة الإلكترونية غير مكتملين، ولا يوجد حفظ شركة | `UcCompanyData.Designer.cs:324,695,702` رسائل صريحة؛ `UcCompanyData.cs:10,15-20,40-48` جلسة دون bindDisabledActions وتمكين محلي للإدخال | **استكمال متطلبات وربط**؛ ملء حقول التبويب لا يتم بالتخمين لأن النص المصدر يصرح بأن التفاصيل غير متاحة |
| UI-05 | بيانات الفروع الجديدة تمكن المدخلات والعناوين وربط Lite محليًا؛ الحفظ والشعار والقوائم غير موصولة | `UcBranchData.cs:41-53,62-74,77-86` | **ربط برمجي** وشروط تبويبات؛ الحفاظ على ترتيب الحقول |
| UI-06 | البوليصة التي تفتح من الشجرة لا تنفذ إجراءات الأعمال؛ ملف code-behind يقتصر على التهيئة وأحداث فارغة | `FrmMain.PhaseOneNavigation.cs:631`؛ `البوالص_والشحن/العمليات/البوليصه/UcShipmentWaybill.cs:13-65` | **برمجة أعمال وربط**: الحفظ والترقيم والبحث والأطراف والأسطر والطباعة؛ أدوات المصمم ليست دليل اكتمال |
| UI-07 | شاشة الشحن والترحيل تعرض مصدر Bind supplied فقط؛ لا تستدعي API ولا يوجد تنفيذ حفظ/ترحيل/بحث/إغلاق/اختيار شاشة/إضافة شحنة في الجزأين السلوكيين | `UcShipmentDispatch.cs:10-117`؛ `UcShipmentDispatch.Binding.cs:8-47`؛ أسماء الأزرار في المصمم `btnSelectFromScreen`/`btnAddShipment` في الأسطر 2438/2451 | **ربط برمجي**؛ تمييز اختيار السطر عن تنفيذ إصدار الكمية، والمحطات ومقطع الرحلة |
| UI-08 | أدوات الترحيل الأساسية تظل قابلة للعرض من المصمم دون اشتراك Click عملي؛ ليست أزرار حفظ صورية برسالة نجاح، بل أدوات بلا مسار أعمال | لا يوجد `Click +=` لتلك الأدوات في ملفي UcShipmentDispatch؛ أوامر Import/Export/Help فقط معطلة صراحة بالمصمم:527,542,557 | **ضبط الإتاحة ثم الربط**: تعطيل الأدوات غير المنفذة مع سبب مفهوم حتى يكتمل ربطها؛ لا إعادة تصميم |
| UI-09 | شاشة التوريد المخزني مرتبطة بالشجرة SHIP:014 لكن عنوانها «ترحيل الشحنات»، وتضم حقول هوية/ملاحظات مرسل من قالب منقول، ولا منطق توريد | `FrmMain.PhaseOneNavigation.cs:633`؛ `UcWarehouseReceiptOrder.Designer.cs:293,318,346,361,376,509`؛ ملف `.cs` يهيئ فقط | **نقص عقد شاشة ومصمم + برمجة**؛ لا يكفي تغيير العنوان لإكمال التوريد، يلزم عقد الحقول/الأسطر/المستودع/الحالة أولًا |
| UI-10 | صف البوليصة يملك الآن تمددًا صحيحًا مشتقًا من ارتفاع الرأس والتفاصيل، بدل ارتفاع 225 القديم؛ ويوجد Binding يعرض أصناف البوليصة | `UcDispatchWaybillRow.cs:21-38`؛ `.Binding.cs:12-43` | **تحسن مثبت بالمصدر**؛ يحتاج Windows لضمان الحفاظ على التمدد بعد إخفاء تبويب/فتح آخر |
| UI-11 | صف البوليصة يصرح بتبويبين فقط: البيانات الأساسية والإضافية؛ لا يصرح بتبويبات المحتوى/الجمارك/التتبع داخل هذا التحكم | `UcDispatchWaybillRow.Designer.cs:429-431,450-452,539-542`؛ جميع Controls.Add الخاصة بالـTabControl | **نقص مقابل قائمة العمل السابقة للمستخدم يحتاج تثبيت عقد**؛ لا أفترض أنه يجب تكرار البيانات الموجودة في شاشة البوليصة الرئيسية |
| UI-12 | إنشاء الرحلة موجود كتحكم مستقل وعقد CreateTripRequest، لكنه يطلق حدثًا فقط؛ لم يوجد اشتراك تشغيل فعلي أو مسار له في CreatePhaseOneControl | `Waybills/UcTrip.cs:23,54-77`؛ `RootHosts/TripForm.cs:25` يعيد تصدير الحدث فقط؛ سجل الشجرة لا يذكر TripForm/UcTrip | **ربط الشجرة وAPI**؛ الاستدعاء الحالي لا يحفظ رحلة وحده |
| UI-13 | إدخال مراجع السائق والمركبة والمنشأ والوجهة في UcTrip يتطلب GUID كنص، لا قوائم اسم/كود مرجعية متصلة | `UcTrip.Designer.cs:134-164` TextBox؛ `UcTrip.cs:57-60` Guid.TryParse | **اختيار مراجع وربط**: توفير اختيار بالاسم والكود، إبقاء المعرف الحقيقي بعقد الربط؛ إجراء تجربة مستخدم Windows |
| UI-14 | جزء من محتوى تبويبات القبض وأدواته يبنى وقت التشغيل ويستبدل محتويات تبويب المرفقات، فلا تظهر كل الأدوات من ملف مصمم القبض وحده | `ReceiptTabs.cs:8-13,21-61`؛ `tp2.Controls.Clear()` في33، إنشاء unlink/template والربط | **مراجعة شرط designer-only مع المالك**؛ نقل العناصر إلى partial Designer إن كان الشرط لازال معتمدًا مع إبقاء جميع أحداثها وعقودها |
| UI-15 | لا يمكن اعتماد اكتمال القياسات الموحدة من وجود SharedScreenProperties؛ القياسات وتدرج DPI تختلف بين الشاشات | جدول القياسات أدناه؛ `FrmMain.cs:112-115` يستثني القيد من ConfigureWorkspace | **اختبار بصري** على Windows؛ لا أقرر وجود قص أو تداخل فعلي من الأرقام وحدها |

جميع مسارات ملفات الجدول المختصرة تقع تحت `TransportERP.Desktop` إلا ملفات FrmScreen/BatchFive/Receipt الواقعة تحت `TransportERP.EmptyForms/Forms`. راجع جدول الأزرار التالي للحصول على المسار الكامل للمصمم ورقم إنشاء/تسمية كل أداة.

## سند القبض — مسار التنفيذ الفعلي

من الشجرة: `04.04.01 → UcScreen_04_04_01 → CreateConnectedWorkspaceControl → ConfigureReceiptWorkspace` بعد تحميل bootstrap بجلسة تسجيل الدخول ونسخة نطاق للفرع. يحوي المصدر طلبات حقيقية إلى `api/v1/receipts/`، ولا يكتفي برسالة نجاح محلية. لا يعني ذلك أن قاعدة المستخدم الحالية مهيأة أو أن الخادم متاح؛ ذلك يحتاج تحقق البيئة.

`FrmMain.ReceiptWorkspace.cs:17-43` يحمّل bootstrap وفروع النطاق؛ 48-112 يربط الوظائف. Create/Edit يرسل PUT، والعرض يطلب القائمة ثم المستند، والترحيل/المراجعة/الاعتماد يرسل POST، والإلغاء/العكس يطلب سببًا والعكس تاريخًا. `ReceiptWorkspaceBinding.cs:45-80` يعيد حساب الإجراءات بحسب صلاحيات bootstrap وحالة السند. لذلك يُقيّم الزر من الحالة والصلاحية والربط، وليس من مجرد Enabled في المصمم.

العرض أحدث 200 سند حسب عنوان نافذة الاختيار (`FrmMain.ReceiptWorkspace.cs:71`). لا تظهر في هذا المسار أدوات تصفية تاريخ/رقم/طرف داخل النافذة، والجدول AutoGenerateColumns؛ يجب اختبار العناوين العربية وترتيبها على Windows. تحسين البحث هنا عمل واجهة محدود بعد اعتماد متطلبه، وليس سببًا لإعادة تصميم سند القبض.

### الحقول المرتبطة فعليًا وعقدها

`FrmScreen_04_04_01.cs:37-68` يربط paymentMethodCode، voucherTypeRef، salespersonRef، collectorRef، voucherNumber، voucherDate، partyRef، sourceCashBankRef، destinationCashBankRef، currencyRef، amount، exchangeRate، counterAccountRef، description، referenceNumber، referenceName، commission، receiptCostCenter، receiptProject، receiptActivity، linkedDocument، state. Required=true صريح للعملية/نوع السند/التاريخ/العملة/المبلغ/السعر/الحساب المقابل/البيان. رقم السند والحالة للعرض فقط. **لون أصفر وحده لا يحدد قاعدة Required**: `ApplyRequestedFieldColors:222-226` يلون كذلك الفرع ورقم السند المعروضين.

`ReceiptWorkspaceBinding.cs:119-160` يجمع الحقول في ReceiptDraft والأسطر في ReceiptLine. مركز التكلفة/المشروع/النشاط يستكملها السطر من الرأس عند فراغه. مجموع تفاصيل العملات يحسب عبر ReceiptTotalsCalculator ولا يستبدل سعر سطر مختلف العملة بسعر الرأس (`FrmScreen_04_04_01.cs:188-210`). الاشتراط النهائي للحفظ والترحيل يجب مراجعته في خدمة الخادم بواسطة مراجع الأكواد المالية.

### التبويبات حالة بحالة

| التبويب | الدليل | حالة الوظيفة |
|---|---|---|
| البيانات الرئيسية | Designer:2484؛ code:38-68 | حقول مربوطة بالعقد وبعضها للعرض فقط |
| بيانات إضافية | Designer:488؛ ReceiptWorkspaceBinding:119-140 | تحفظ المرجع والعمولة والأبعاد والروابط ضمن Additional، لا تُفترض أعمدة قاعدة مستقلة من أسماء التحكم |
| تفاصيل السند | Designer:515؛ ReceiptWorkspaceBinding:141-159 | Rows حقيقية؛ ينقص تحقق تشغيل كامل للأسطر والبحث عن المراجع |
| الحسابات | Designer:715؛ ReceiptTabs:174-192 | يعرض أسطر القيد المحفوظ بعد JournalId، وقبل ذلك تفاصيل السند مع تنبيه لا قيد مرحل |
| المرفقات والربط | Designer:817؛ ReceiptTabs:33-41,91-114 | تحميل وحفظ مرفق عبر API ثم تحقق SHA-256 عند التنزيل؛ ربط المستند باختيار من bootstrap |
| الاعتمادات | Designer:861؛ ReceiptWorkspaceBinding:84-112,228-229 | زر مرحلة Review/Approve مرتبط بالصلاحية والحالة؛ سجل الاعتماد يعرض Audit |
| سجل العمليات | Designer:920؛ ReceiptWorkspaceBinding:218-230 | أحداث السند من الخادم؛ لا يفترض أحداث غير محملة |
| البيانات الافتراضية | Designer:1357؛ ReceiptTabs:84-89,195-216 | حفظ إعدادات الشركة بصلاحية configure وبنسخة الإعداد؛ أثرها على المسودات الجديدة |
| استيراد من ملف | Designer:1439؛ code:75-76؛ BatchFiveUiSession:411-470 | CSV محلي إلى معاينة ثم إضافة للمسودة؛ لا يحفظ تلقائيًا |



### جميع أزرار سند القبض المصرح بها في المصمم

المصدر الكامل: `TransportERP.EmptyForms/Forms/FrmScreen_04_04_01.Designer.cs`. الحالة هنا مسلسلة في المصمم؛ عمود التنفيذ يذكر التحول وقت التشغيل.

| الأداة | التسمية | الحالة المسلسلة | سطر الدليل | الحدث والتنفيذ |
|---|---|---|---|---|
| `standardCommandDelete` | أيقونة | Enabled=false @2695؛ Visible=false @2696 | إنشاء:113؛ نص:2740 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandLast` | أيقونة | Enabled=false @2699؛ Visible=false @2700 | إنشاء:114؛ نص:2753 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandNext` | أيقونة | Enabled=false @2703؛ Visible=false @2704 | إنشاء:115؛ نص:2764 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandPrevious` | أيقونة | Enabled=false @2707؛ Visible=false @2708 | إنشاء:116؛ نص:2775 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandFirst` | أيقونة | Enabled=false @2711؛ Visible=false @2712 | إنشاء:117؛ نص:2786 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandRefresh` | تحديث | Enabled=false @2715؛ Visible=false @2716 | إنشاء:118؛ نص:2799 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandImport` | استيراد | Enabled=false @2719؛ Visible=false @2720 | إنشاء:119؛ نص:2809 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandExport` | تصدير | Enabled=false @2723؛ Visible=false @2724 | إنشاء:120؛ نص:2819 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandHelp` | مساعدة | Enabled=false @2727؛ Visible=false @2728 | إنشاء:121؛ نص:2829 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `btnReceiptApprove` | اعتماد سند القبض | Enabled=false @903؛ Visible=false @909 | إنشاء:179؛ نص:908 | يظهر بعد ConnectReceiptWorkspace؛ Review/Approve بحسب NextReceiptStage والحالة والصلاحية: ReceiptWorkspaceBinding:81-112. |
| `btnReceiptSettings` | إعدادات سند القبض | Enabled=false @1423؛ Visible=false @1429 | إنشاء:215؛ نص:1428 | يظهر بصلاحية configure؛ حوار ثم PUT configuration: ReceiptWorkspaceBinding:80-85؛ FrmMain.ReceiptWorkspace:54-65. |
| `documentImportChoose` | اختيار ملف ومعاينة | الحالة الافتراضية للأداة | إنشاء:221؛ نص:1495 | اختيار CSV ومعاينة محلية: code:75؛ BatchFiveUiSession:411-440. |
| `documentImportApply` | إضافة إلى المسودة | الحالة الافتراضية للأداة | إنشاء:222؛ نص:1504 | إضافة نتيجة المعاينة إلى المسودة: code:76؛ BatchFiveUiSession:442-470؛ لا حفظ تلقائي. |
| `btnClear` | إضافة | الحالة الافتراضية للأداة | إنشاء:280؛ نص:2333 | تفريغ/بدء مسودة جديدة محليًا: BatchFiveUiSession:140,381-389؛ يراعي pending changes وصلاحية إنشاء المسودة بعد الربط. |
| `btnView` | بحث / عرض | Enabled=false @2340 | إنشاء:281؛ نص:2346 | أمر View مربوط بـRunAsync ثم API list/get: code:61؛ BatchFive:135-139؛ FrmMain.ReceiptWorkspace:68-80. |
| `btnCreate` | حفظ | Enabled=false @2353 | إنشاء:282؛ نص:2359 | أمر Create إلى PUT receipts، مع Version؛ code:62 وFrmMain.ReceiptWorkspace:81. |
| `btnEdit` | حفظ التعديل | Enabled=false @2366 | إنشاء:283؛ نص:2372 | أمر Edit إلى PUT receipts؛ يتيحه adapter للمسودة المحفوظة مع الصلاحية: ReceiptWorkspaceBinding:55-64. |
| `btnCancel` | إلغاء المستند | Enabled=false @2379 | إنشاء:284؛ نص:2385 | أمر Cancel إلى POST مع السبب/Version؛ FrmMain.ReceiptWorkspace:86-98. |
| `btnPrint` | طباعة | Enabled=false @2391 | إنشاء:285؛ نص:2397 | تمكين وربط المعاينة في code:91-93؛ ReceiptPrintPreview:52-85؛ HTML/معاينة طباعة، لا حفظ/ترحيل. |
| `btnPost` | ترحيل | Enabled=false @2405 | إنشاء:286؛ نص:2411 | أمر Post مرتبط بـPOST id/post؛ الاستعداد من سياسة المراجعة/الاعتماد: ReceiptWorkspaceBinding:53-63. |
| `btnReverse` | عكس القيد | Enabled=false @2418 | إنشاء:287؛ نص:2424 | أمر Reverse إلى POST مع السبب وتاريخ العكس؛ FrmMain.ReceiptWorkspace:86-98. |
| `btnClose` | إغلاق | الحالة الافتراضية للأداة | إنشاء:288؛ نص:2436 | CloseRequested من code:77؛ FrmMain.cs:187 يربطه بإغلاق التبويب، مع حراسة التغييرات العامة. |
| `btnAddRow` | إضافة سطر | الحالة الافتراضية للأداة | إنشاء:289؛ نص:2445 | محلي AddRow من BatchFive:141 ثم القيم الافتراضية ReceiptTabs:116-123؛ لا حفظ منفصل. |
| `btnRemoveRow` | حذف سطر | الحالة الافتراضية للأداة | إنشاء:290؛ نص:2454 | محلي RemoveRow من BatchFive:142؛ لا حذف مستند مالي من الخادم. |


### أدوات القبض المنشأة في السلوك خارج المصمم

| الأداة | الفعل ودليل الربط | القيد |
|---|---|---|
| receiptUndo | ReceiptTabs:12,27-30,66-68 | تراجع محلي إلى baseline؛ ليس إلغاء مستند محفوظ |
| receiptUpload | ReceiptTabs:10,91-102 | إضافة مرفق لسند مسودة محفوظ دون تغييرات محلية وبصلاحية Edit؛ أقصى 5MB |
| receiptDownload | ReceiptTabs:11,104-114 | تنزيل مرفق محدد والتحقق من البصمة ثم ملف محلي |
| receiptSaveDefaults | ReceiptTabs:13,84-89 | PUT إعدادات القيم الافتراضية بصلاحية configure؛ ليس حفظ السند |
| unlink | ReceiptTabs:36-38 | يمسح رابط المستند من المسودة فقط |
| template | ReceiptTabs:48-61 | يحفظ رؤوس CSV لغير read-only؛ لا بيانات مالية |
| open في نافذة العرض | FrmMain.ReceiptWorkspace:71-80 | يختار سندًا من قائمة أحدث 200 ثم يطلب المستند الكامل |
| confirm في نافذة السبب | FrmMain.ReceiptWorkspace:87-97 | سبب غير فارغ للإلغاء/العكس؛ يرسل تاريخًا للعكس |
| print داخل المعاينة | ReceiptPrintPreview:62-64 | يستدعي WebBrowser.ShowPrintPreviewDialog بعد اكتمال DocumentCompleted |
| export داخل المعاينة | ReceiptPrintPreview:63,65-71 | حفظ نسخة HTML للعرض الحالي؛ لا إجراء مالي |

معاينة الطباعة تحمل علامة المسودة عندما توجد تغييرات أو لا يوجد Version، وهذا سلوك إيجابي مثبت بالمصدر `ReceiptPrintPreview:44-49`. لا يوجد في هذا الملف إرسال حدث «طباعة ناجحة» إلى خدمة السند؛ لذلك لا يجوز اعتماد عداد الطباعة باعتباره سجل طباعة تشغيلية مكتملًا دون مسار إضافي يثبت ذلك.

## سند الصرف — الحقول والتبويبات ومسار الربط

المتحكم المربوط بالشجرة هو `UcScreen_04_04_02`، وليس قالبًا فارغًا. حقوله المربوطة بـBatchFiveUiSession: voucherNumber، voucherDate، partyRef، sourceCashBankRef، destinationCashBankRef، currencyRef، amount، exchangeRate، counterAccountRef، description، state (`FrmScreen_04_04_02.cs:28-38`). التاريخ والعملة والمبلغ والسعر والحساب المقابل والبيان مطلوبة؛ الرقم والحالة للعرض. توجد حقول مرجعية إضافية read-only يصرح المصمم بأن خدمة السند غير مرتبطة بها؛ مثال referenceField10/referenceField11/referenceField12 في Designer:1248-1305.

| التبويب | الدليل في FrmScreen_04_04_02.Designer.cs | حقيقة التنفيذ |
|---|---|---|
| البيانات الرئيسية | 1779 | محررات وعقد محلي؛ لا تحميل سند من API |
| بيانات إضافية | 338 | مرجع بصري؛ بعض حقوله للعرض وغير مرتبطة |
| التفاصيل والحركات | 364 | جدول تحرير محلي |
| المرفقات والربط | 414 | عرض غير مرتبط؛ لا خدمات رفع/تنزيل مثل القبض |
| الاعتمادات | 454 | عرض سياق فقط؛ لا اعتماد API |
| سجل العمليات | 494 | سياق لم يُحمّل، لا سجل تشغيل فعلي |
| البيانات الافتراضية | 671 | لا خدمة حفظ افتراضيات للصرف في code-behind |
| الحسابات | 741 | جدول عرض؛ لا قيد مرحل محمل في مسار التطبيق |
| استيراد من ملف | 842 | CSV محلي للمعاينة ثم المسودة |

إكمال هذا المسار مطلوب من الكود والخدمات وعقد الصلاحيات، مع تدقيق المصمم على Windows. لا يجب نسخ إعدادات القبض إلى الصرف وتغيير الاسم؛ تختلف جهة المصدر/الوجهة والطلب والمستفيد والقيد.


### جميع أزرار سند الصرف المصرح بها في المصمم

المصدر الكامل: `TransportERP.EmptyForms/Forms/FrmScreen_04_04_02.Designer.cs`. الحالة هنا مسلسلة في المصمم؛ عمود التنفيذ يذكر التحول وقت التشغيل.

| الأداة | التسمية | الحالة المسلسلة | سطر الدليل | الحدث والتنفيذ |
|---|---|---|---|---|
| `standardCommandAdd` | أيقونة | Enabled=false @1979؛ Visible=false @1980 | إنشاء:157؛ نص:2030 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandDelete` | أيقونة | Enabled=false @1983؛ Visible=false @1984 | إنشاء:158؛ نص:2042 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandLast` | أيقونة | Enabled=false @1987؛ Visible=false @1988 | إنشاء:159؛ نص:2055 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandNext` | أيقونة | Enabled=false @1991؛ Visible=false @1992 | إنشاء:160؛ نص:2066 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandPrevious` | أيقونة | Enabled=false @1995؛ Visible=false @1996 | إنشاء:161؛ نص:2077 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandFirst` | أيقونة | Enabled=false @1999؛ Visible=false @2000 | إنشاء:162؛ نص:2088 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandSave` | أيقونة | Enabled=false @2003؛ Visible=false @2004 | إنشاء:163؛ نص:2099 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandRefresh` | تحديث | Enabled=false @2007؛ Visible=false @2008 | إنشاء:164؛ نص:2111 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandImport` | استيراد | Enabled=false @2011؛ Visible=false @2012 | إنشاء:165؛ نص:2121 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandExport` | تصدير | Enabled=false @2015؛ Visible=false @2016 | إنشاء:166؛ نص:2131 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandHelp` | مساعدة | Enabled=false @2019؛ Visible=false @2020 | إنشاء:167؛ نص:2141 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `documentImportChoose` | اختيار ملف ومعاينة | الحالة الافتراضية للأداة | إنشاء:223؛ نص:897 | ChooseCsv محلي code:48، لا API. |
| `documentImportApply` | إضافة إلى المسودة | الحالة الافتراضية للأداة | إنشاء:224؛ نص:906 | ImportPreview إلى مسودة فقط code:49. |
| `referencePrint` | طباعة | Enabled=false @1552 | إنشاء:274؛ نص:1558 | معطل بالمصمم، لا حدث معاينة أو طباعة مسجل في السلوك. |
| `btnView` | عرض / إعادة تحميل | Enabled=false @1634 | إنشاء:279؛ نص:1640 | عقد View فقط code:41؛ لا handler تشغيل في التطبيق، لذلك يظل غير متاح. |
| `btnCreate` | حفظ جديد | Enabled=false @1647 | إنشاء:280؛ نص:1653 | عقد Create فقط code:42؛ لا ربط API لصرف جديد. |
| `btnEdit` | حفظ التعديل | Enabled=false @1660 | إنشاء:281؛ نص:1666 | عقد Edit فقط code:43؛ لا ربط API للتعديل. |
| `btnCancel` | إلغاء المستند | Enabled=false @1673 | إنشاء:282؛ نص:1679 | عقد Cancel فقط code:44؛ لا ربط API للإلغاء. |
| `btnPost` | ترحيل | Enabled=false @1686 | إنشاء:283؛ نص:1692 | عقد Post فقط code:45؛ لا ربط API للترحيل. |
| `btnReverse` | عكس القيد | Enabled=false @1699 | إنشاء:284؛ نص:1705 | عقد Reverse فقط code:46؛ لا ربط API للعكس. |
| `btnClear` | تفريغ المسودة | الحالة الافتراضية للأداة | إنشاء:285؛ نص:1717 | مسودة محلية فقط BatchFiveUiSession:140,381-389؛ لا إنشاء محفوظ. |
| `btnClose` | إغلاق | الحالة الافتراضية للأداة | إنشاء:286؛ نص:1729 | CloseRequested من code:50؛ FrmMain.cs:186 يربطه بالتبويب. |
| `btnAddRow` | إضافة سطر | الحالة الافتراضية للأداة | إنشاء:287؛ نص:1740 | إضافة صف محلي BatchFiveUiSession:141؛ لا حفظ. |
| `btnRemoveRow` | حذف سطر | الحالة الافتراضية للأداة | إنشاء:288؛ نص:1751 | حذف صف محلي BatchFiveUiSession:142؛ لا حذف مستند. |


## قيد اليومية — الحقول والتبويبات ومسار الربط

مفاتيح الرأس الفعلية: documentNumber، accountingDate، externalReference، description، currencyRef، exchangeRate، state (`FrmScreen_04_05_01.cs:70-76`). البيان له RequiredWhen تابع لإعداد شركة محمل من API؛ null يعني غير متاح، والنص الفارغ ممنوع حتى تحديد الإعداد (`code:26-49`؛ BatchFiveUiSession:235-238). هذا يمنع استنتاج إلزام البيان من إعداد اعتماد آخر.

أسماء أعمدة العقد: rowNo، accountRef، costCenterRef، lineDescription، debit، credit، currencyRef، exchangeRate، accountingAmount. الحساب موسوم required. أعمدة analyticalAccount/accountName/foreignDebit/foreignCredit/approvalNumber/salesperson/collector/referenceNumber مستبعدة من إرسال Snapshot باعتبارها displayOnly، كما يصرح إنشاء Binding في code:85-86. يجب تحديد مصدر كل عمود للعرض قبل اعتبار القيد مكتملًا؛ وجود العنوان لا يعني وجود بياناته.

| التبويب | الدليل في FrmScreen_04_05_01.Designer.cs | حقيقة التنفيذ |
|---|---|---|
| البيانات الرئيسية | 619 | رأس تحريري؛ رقم مستند معروض مشترك بين field_documentNumber وtextBox2 |
| بيانات إضافية | 1323 | قالب حقول؛ لا خدمة تشغيل مثبتة |
| الحسابات | 1548 | جدول عرض غير مرتبط بأسطر قيد محفوظ في مسار الخادم |
| المرفقات والربط | 1650 | سياق عرض؛ لا تحميل/حفظ مرفقات في code-behind |
| الاعتمادات | 1694 | زر باسم اعتماد سند القبض مخفي؛ لا يمثل اعتماد قيد |
| سجل العمليات | 1753 | سياق؛ لا API سجل للقيد |
| البيانات الافتراضية | 1797 | لا حفظ إعدادات هذا التبويب مثبت في code-behind |
| استيراد من ملف | 1879 | XLSX أو CSV محلي؛ لا حفظ تلقائي |
| التفاصيل والحركات | 2534 | جدول المبالغ والمدين/الدائن؛ العقد غير موصول بالخادم |



### جميع أزرار قيد اليومية المصرح بها في المصمم

المصدر الكامل: `TransportERP.EmptyForms/Forms/FrmScreen_04_05_01.Designer.cs`. الحالة هنا مسلسلة في المصمم؛ عمود التنفيذ يذكر التحول وقت التشغيل.

| الأداة | التسمية | الحالة المسلسلة | سطر الدليل | الحدث والتنفيذ |
|---|---|---|---|---|
| `btnReceiptApprove` | اعتماد سند القبض | Enabled=false @1736؛ Visible=false @1742 | إنشاء:158؛ نص:1741 | عنصر موروث من قالب القبض، مخفي ومعطل؛ ليس تنفيذ اعتماد قيد. |
| `btnReceiptSettings` | إعدادات سند القبض | Enabled=false @1863؛ Visible=false @1869 | إنشاء:168؛ نص:1868 | عنصر موروث من قالب القبض، مخفي ومعطل؛ ليس إعدادًا متصلًا للقيد. |
| `documentImportChoose` | اختيار ملف ومعاينة | الحالة الافتراضية للأداة | إنشاء:174؛ نص:1935 | ChooseJournalImport من code:88؛ يدعم XLSX/CSV حسب BatchFiveUiSession.JournalImport.cs. |
| `documentImportApply` | إضافة إلى المسودة | الحالة الافتراضية للأداة | إنشاء:175؛ نص:1944 | ImportJournalPreview من code:89؛ تحميل محلي إلى مسودة. |
| `btnClear` | إضافة | الحالة الافتراضية للأداة | إنشاء:198؛ نص:2226 | مسودة محلية BatchFiveUiSession:140؛ الإضافة لا تحفظ. |
| `btnView` | بحث / عرض | Enabled=false @2234 | إنشاء:199؛ نص:2240 | عقد View code:79؛ لا handler مالي موصول. |
| `btnCreate` | حفظ | Enabled=false @2248 | إنشاء:200؛ نص:2254 | عقد Create code:80؛ لا API حفظ قيد. |
| `btnEdit` | حفظ التعديل | Enabled=false @2262 | إنشاء:201؛ نص:2268 | عقد Edit code:81؛ لا API تعديل قيد. |
| `btnCancel` | إلغاء المستند | Enabled=false @2276 | إنشاء:202؛ نص:2282 | عقد Cancel code:82؛ لا API إلغاء قيد. |
| `btnPrint` | طباعة | Enabled=false @2289 | إنشاء:203؛ نص:2295 | معطل بالمصمم دون handler طباعة للقيد. |
| `btnPost` | ترحيل | Enabled=false @2303 | إنشاء:204؛ نص:2309 | عقد Post code:83؛ لا API ترحيل. |
| `btnReverse` | عكس القيد | Enabled=false @2316 | إنشاء:205؛ نص:2322 | عقد Reverse code:84؛ لا API عكس. |
| `btnClose` | إغلاق | الحالة الافتراضية للأداة | إنشاء:206؛ نص:2335 | CloseRequested من code:90؛ FrmMain.cs:184 يربطه بالتبويب. |
| `btnAddRow` | إضافة سطر | الحالة الافتراضية للأداة | إنشاء:207؛ نص:2346 | إضافة صف محلي BatchFiveUiSession:141. |
| `btnRemoveRow` | حذف سطر | الحالة الافتراضية للأداة | إنشاء:208؛ نص:2357 | حذف صف محلي BatchFiveUiSession:142. |
| `standardCommandDelete` | أيقونة/بلا نص | Enabled=false @2380؛ Visible=false @2389 | إنشاء:210؛ نص:210 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandLast` | أيقونة/بلا نص | Enabled=false @2395؛ Visible=false @2404 | إنشاء:211؛ نص:211 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandNext` | أيقونة/بلا نص | Enabled=false @2410؛ Visible=false @2419 | إنشاء:212؛ نص:212 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandPrevious` | أيقونة/بلا نص | Enabled=false @2425؛ Visible=false @2434 | إنشاء:213؛ نص:213 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandFirst` | أيقونة/بلا نص | Enabled=false @2440؛ Visible=false @2449 | إنشاء:214؛ نص:214 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandRefresh` | تحديث | Enabled=false @2455؛ Visible=false @2464 | إنشاء:215؛ نص:2463 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandImport` | استيراد | Enabled=false @2470؛ Visible=false @2479 | إنشاء:216؛ نص:2478 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandExport` | تصدير | Enabled=false @2485؛ Visible=false @2494 | إنشاء:217؛ نص:2493 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |
| `standardCommandHelp` | مساعدة | Enabled=false @2500؛ Visible=false @2509 | إنشاء:218؛ نص:2508 | أمر معياري مخفي ومعطل صراحة؛ لا تنفيذ تشغيل مثبت لهذا الأمر في هذا المسار. |


## الشركات والفروع — المقارنة التي يجب اعتمادها قبل تعديل التصميم

| الهوية | ما يفتحه التطبيق اليوم | ما يحفظه المستودع كذلك | الحالة |
|---|---|---|---|
| 02.01.01 | UcCompanyData | UcCompanyManagement وFrmCompanyManagement | اختيار الشجرة موثق بالمصدر؛ النموذجان لا يحفظان شركة تشغيلية |
| 02.02.01 | UcBranchData | UcBranchManagement وFrmBranchManagement | اختيار الشجرة موثق بالمصدر؛ النموذجان لا يحفظان فرعًا تشغيليًا |
| SETUP:BRANCH-GROUPS | UcBranchGroups | لا يصح اعتباره جدولًا مرتبطًا بمجرد وجوده | إضافة صفوف مؤقتة دون ترقيم تلقائي أو حفظ |

الشركات القديمة تحوي حقول Company ID/Code/اسم عربي/إنجليزي/مجموعة/نشاط/عملة/ضريبة/سجل/دولة/محافظة/مدينة/عنوان/هواتف/بريد/موقع/رمز بريدي/ملاحظات/نشط وشعار. تبويبا البيانات الإضافية والحالة بلا أدوات محتوى في المصدر؛ تبويب الشركات هو الذي يحوي النموذج. وجود cmbCompanyGroup داخل الأساسية لا يساوي تبويب مجموعة شركات كامل. المصدر القديم يحافظ على المصمم ولا يجوز حذفه بدعوى أنه غير موصول.

أزرار UcCompanyManagement وUcBranchManagement: الجديد، الحفظ، التعديل، الحذف، التحديث، الأول، السابق، التالي، الأخير، التراجع، البحث، الطباعة، الاعتماد، إضافة الشعار، حذف الشعار. كل زر منها موجود في قائمة ConfigureUnavailableActions، ويضبط Enabled=false مع سبب «لم يُنفذ ولم يربط بخدمة تشغيلية» (`UcCompanyManagement.cs:34-51` و`UcBranchManagement.cs:37-54`). الإغلاق يطلق CloseRequested؛ المقارنة القديمة كانت توحي بأنه متصل مرتين في الشركة، أما آخر مصدر فقد أزال الاشتراك الإضافي من المنشئ، فلا نكرر هذه الملاحظة كعيب حالي.

في UcCompanyData وظيفة الإضافة تمكّن الإدخال المحلي فقط. «رئيسي» يمكّن المجموعة الضريبية؛ تبويب الفاتورة الإلكترونية يظل معطلًا، وتبويب الربط يصرح بتعذر معرفة تفاصيله من النص المرجعي. في UcBranchData زر الإضافة يمكّن حقول التبويبات؛ خيار Lite يظهر حقول الاتصال وتبويب مستنداته؛ العناوين يعرض مجموعة اختيار، لا يحفظها في قاعدة.

المطلوب قرار واضح: هل يبقى تصميم المالك المحفوظ هو الأساسي مع إضافة قواعد المصدر، أم يعتمد النموذج الجديد؟ يُرفق بالمراجعة صور Windows للنموذجين وفرق الحقول؛ بعدها فقط تتوزع مهام المصمم. لا قرار اعتماد بصري في هذه المراجعة.

## البوالص والترحيل والمخزون والرحلة — تفصيل الإجراءات

| الشاشة / الإجراء | وجود المصدر | حالة التنفيذ المثبتة | العمل التالي |
|---|---|---|---|
| UcShipmentWaybill: جديد/حفظ/تعديل/حذف/تحديث/بحث/طباعة/اعتماد/تنقل/إغلاق | أزرار المصمم، code-behind لا يربطها | لا وظيفة أعمال ولا خدمة من هذا التحكم | تثبيت عقد البوليصة وربط الأوامر دون نقل الحاويات |
| UcShipmentWaybill: النقر على جدول الأصناف | Designer:3769؛ code:61-65 | handler فارغ | تحديد وظيفة النقر/اختيار الصنف |
| UcShipmentDispatch: جديد/حفظ/تعديل/حذف/تحديث/بحث/طباعة/اعتماد/تنقل/إغلاق | أدوات ظاهرة في المصمم، لا handlers للـClick في ملفات السلوك | لا مسار تشغيل لهذه الأوامر | سياسة الإتاحة ثم خدمات الترحيل |
| UcShipmentDispatch: اختيار من الشاشة | btnSelectFromScreen، Designer:2438 | لا نافذة اختيار موصولة | ربط بحث بوالص حسب الفرع/المحطة/الحالة وصلاحيات المستخدم |
| UcShipmentDispatch: إضافة شحنة | btnAddShipment، Designer:2451 | لا إضافة تشغيلية | ربط المصدر لا بيانات تجريبية |
| UcShipmentDispatch: مسح الباركود | txtBarcode والخيارات في Designer:2390-2464 | لا حدث scan/Enter تشغيل | عقد barcode ورفض المكرر/المصدر غير المسموح |
| UcShipmentDispatch: تحميل Bind | Binding.cs:8-47 | يبني صفًا لكل WaybillResponse supplied ثم يعرضه؛ لا query | استدعاء الخادم وتحميل بيانات حقيقية |
| UcDispatchWaybillRow: +/− | Designer:413؛ code:21-38 | تمدد/طي مع حساب ارتفاع الرأس والتفاصيل | اختبار Windows لتغيير حجم الحاوية وإخفاء التبويب |
| UcDispatchWaybillRow: اختيار سطر/كمية صادرة | checkbox/جدول/عمود qtyToDispatch | تحرير/تحديد محلي؛ لا تنفيذ إصدار كميات | تحقق عدم تجاوز المتاح ومنع التكرار وربط سند ترحيل |
| UcDispatchWaybillRow: زر اختيار صنف | DataGridViewButtonColumn btnSelectItem | لا CellContentClick handler في هذا التحكم | تحديد معنى الاختيار وربطه بنوافذ المصدر |
| UcDispatchWaybillRow: المحطة الحالية/التالية/التاريخ/المقطع/مسؤول التحسين/الباركود | Designer:586-746 | حقول إضافية موجودة، لا خدمات choices أو scan | ربط القوائم والعلاقة بمقطع الرحلة والصلاحيات |
| UcWarehouseReceiptOrder: الجديد وأوامر الشريط | Designer يحتويها؛ `.cs` تهيئة فقط | بلا مسار توريد مخزني | إعادة مراجعة عقد الشاشة ثم ربط الاستلام والجرد والحالة |
| UcTrip: إنشاء الرحلة | btnCreate.Click، Designer:251؛ code:54-77 | تحقق GUID/رقم ثم حدث CreateTripRequested دون مشترك موجود | ربط API واعادة TripResponse/Version والحالة |
| UcTrip: عرض الرحلة/مراجعها/توقفاتها | Bind/SetReferences/SetPlannedStops، code:25-52 | عرض supplied فقط | نوافذ lookup ومسار تحميل حقيقي |

لا أصف ما سبق بأنه «يعمل» لأنه يطلق حدثًا أو يقبل قيمًا محلية. الفصل ضروري بين التصميم، السلوك المحلي، والتشغيل المثبت بعد API.

## القياسات والخصائص — أدلة مصدر، وليست صور قبول

| المتحكم | Root Size المسلسل | DPI/Font baseline | RTL/تمرير | الدليل |
|---|---|---|---|---|
| قبض UcScreen_04_04_01 | 1440 × 940 | Dpi 96 × 96 | RTL Yes؛ tabs.RightToLeftLayout=true؛ AutoScroll=true | Designer:2468-2469,2617-2627 |
| صرف UcScreen_04_04_02 | 1100 × 720 | Dpi 96 × 96 | RTL Yes؛ tabs RTL layout؛ AutoScroll=true | Designer:1765-1766,1904-1918 |
| قيد UcScreen_04_05_01 | 1200 × 900 | Dpi 96 × 96 | RTL Yes؛ tabs RTL layout؛ AutoScroll=true | Designer:603-604,2552-2562 |
| شركات جديدة UcCompanyData | 1000 × 640 | Dpi 96 × 96 | RTL Yes؛ tab pages scroll؛ tabsFields MinimumHeight=520 | Designer:575-591 |
| فروع جديدة UcBranchData | 1020 × 736 | Dpi 120 × 120 | RTL Yes؛ AutoScroll؛ MinimumSize=600 × 440 | Designer:2275-2285 |
| الشحن والترحيل UcShipmentDispatch | 2564 × 850 | Dpi 120 × 120 | RTL Yes؛ حاويات فرعية scroll؛ صفوف بوالص ذات حد أدنى | Designer:3038-3048 |
| صف البوليصة UcDispatchWaybillRow | 1100 × 350 serialized، يرتفع/ينكمش حسب الحالة | راجع Apply embedded واصل مصمم الصف | MinimumSize=2330 × 0؛ أعمدة absolute مجموعها 2330 | Designer:105-126,762-763؛ code:30-35 |
| إنشاء الرحلة UcTrip | 920 × 680 | Dpi 120 × 120 | RTL Yes؛ مراجع نصية | Designer:259-265 |

القبض يضبط مساحة الأدوات لتأخذ شريط التمرير عند الحاجة (`code:236-244`) والحد الأدنى ومساحة التمرير (`264-275`). يوجد تحسن عن تصميم fixed لا يأخذ DPI في الحسبان، لكن ذلك يحتاج إثبات Windows. صف الترحيل ذو 21 عمودًا بعرض 2330 لا يمكن أن يظهر كله دفعة واحدة داخل مساحة عمل 1000–1400؛ تم ضبط MinimumSize له، ويجب قبول التمرير الأفقي مع محاذاة الرأس والصف وإظهار الأعمدة المطلوبة، أو اعتماد عرض أقل بعد موافقة المالك. لا يصح ادعاء اختفاء الحقول فعليًا دون تشغيل.

مقاس الرسم في المصمم ليس مقاس النافذة النهائي؛ FrmMain يضع التحكم في TabPage، ويستعمل ConfigureWorkspace عمومًا ويستثني القيد Dock=Fill مباشرة (`FrmMain.cs:103-116`). تثبيت قاعدة القياسات يتم بمراجعة مساحة العمل الحقيقية بعد عرض الشجرة والشريط السفلي وليس بقياس root Designer وحده.

## خطة التحقق على Windows المطلوبة للإقفال

1. بناء solution على SDK المطلوب وفتح المصمم لكل شاشة مالية والشركتين/الفروع المحفوظة والجديدة دون أخطاء partial/resource. حفظ نسخة المصدر SHA قبل أي إصلاح.
2. التقاط صور لكل تبويب على 100% و125% و150% DPI، عرض نافذة المشروع وشجرتها، والتحقق من رأس/تفاصيل/إجماليات/بيانات تدقيق/شريط الحالة/أزرار نهاية الشريط. فحص 1440×900 و1536×864 في المساحة المتاحة فعليًا.
3. اختبار أزرار القبض المذكورة كلًا على حدة: المستخدم ذو View فقط، وCreate، وEdit، وReview، وApprove، وPost، وReverse، وconfigure، وقيمة سجل Version متعارضة. بعد كل إجراء أعد تحميل السند للتحقق من المخزن، لا تكتف برسالة UI.
4. اختبار مسودة قبض بعملة السند وبعملتين في الأسطر، أسعار مفقودة/صفر/سالبة، مبلغ فارغ، سطر فارغ، مجموع غير مطابق، شيك بلا رقم أو تاريخ، نوع يتطلب بوليصة، ربط مستند غير مخول، إضافة/حذف صف، استيراد غير صالح، عكس في فترة غير مفتوحة. أحكام المنع النهائية مصدرها الخادم.
5. اختبار فشل الشبكة عند bootstrap وعند حفظ سند، إغلاق الشاشة أثناء تحميل، محاولات مكررة، بقاء المسودة، إعادة الفتح، وعودة الحسابات/المرفقات/سجل التدقيق بعد reload.
6. اختبار تبديل الفرع في قبض جديد نظيف ثم بعد تعديل ثم بعد حفظ؛ تحقق رفض التبديل بحسب التغييرات/ExpectedVersion، وأن الخادم يعيد فحص النطاق ولا يعتمد اسم الفرع المعروض.
7. الصرف والقيد: يجب أن يظهر أنهما غير موصولين ولا يعطيان نجاح حفظ؛ بعد استكمال adapters اختبر حالات الصلاحية والحالة والعكس والتوازن والصفوف والأبعاد والتاريخ والعرض والاستيراد والطباعة.
8. شركات وفروع: افتح من الشجرة وقارن بالتصميم المحفوظ؛ جرّب الإضافة المحلية وLite والعناوين؛ لا تعتمد الحفظ قبل وجود خدمة حقيقية. توثيق أي نقص تبويب/حقل مقابل دليل معتمد بالمحتوى والصورة.
9. الترحيل: تحميل عدة بوالص supplied أو من API، صف بلا أصناف وصف متعدد الأصناف، تمدد ثم تبديل التبويب/الحجم، محاذاة الرأس والصف بالتمرير الأفقي، اختيار الكل وجزء الكمية، مسح باركود مكرر، عدم تجاوز الكمية، ربط المحطة/المقطع/الرحلة، استرجاع نسخة محفوظة.
10. المخزون/الرحلات: لا يقبل عنوان شاشة صحيح فقط كإقفال. تتبع المصدر ← الطلب ← الخدمة ← المخزن ← الاستجابة ← تحديث الواجهة، مع إثبات لكل زر.

## المقارنة مع المصدر القديم

نسخة `1026a9069756b956b17b55d853e5ebda535810ec` تستخدم قوالب مالية empty InitializeComponent وكانت تمنع فتحها. هذا **ليس عيبًا حاضرًا في مصدر الحكم الجديد**. النسخة الجديدة أضافت UserControls مالية، مساحة تبويبات، حماية تغييرات، خدمة قبض فعلية، إعدادات أستاذ عام، عرض بيانات تدقيق، وصف بوليصة بتمدد مشتق من أبعادها. ما بقي ناقصًا هو تشغيل الصرف والقيد وبقية الشاشات الموصوفة، واعتماد مصدر التصميم واختبار القياسات على Windows. لا تُنقل نتائج غياب الكود القديم إلى تقرير آخر نسخة.


# مراجعة الأكواد والخدمات الخلفية — TransportERP

المراجع الفعلي: `/root/code_audit`، كُلّف بمراجعة للقراءة فقط. المصدر النهائي: `9d02ac8d19e39971ad233108faecc8154a1d5745`، داخل `جذر نسخة المصدر`. تمت مقارنة التغييرات ذات الصلة بالمصدر السابق `1026a9069756b956b17b55d853e5ebda535810ec` لإسقاط العيوب التي أصلحت بالفعل. لم تُعدل ملفات المصدر، ولم يُشغّل بناء أو اختبار .NET؛ أخبر المنسق أن dotnet غير متاح. أدلة أدناه استنتاجات مباشرة من مسارات التنفيذ المقروءة، واختبارات الإثبات المذكورة **مطلوبة ولم تنفذ**.

## نطاق الفحص وما هو موجود

قراءة ملفات API الأربعة القديمة وموديولات تسجيل الدخول ونطاق القبض وإعدادات الأستاذ العام والقبض الجديدة؛ خدمات Application الثلاث للبوليصة والمالية والترحيل؛ Aggregate وقواعد المجال الثلاث؛ مستودعات EF، معاملات الحفظ، الترقيم، سجل التدقيق، المزامنة، سياسة الأستاذ العام، خطة قيد القبض، المرفقات والمراجع؛ خرائط EF وقيودها؛ ومراجعة مقاطع الاختبارات المرتبطة بالنتائج. جرى جرد migrations وأسماء الاختبارات؛ **لم تُراجع كل migration وكل اختبار سطرًا بسطر**، ولم تطبق migrations أو تثبت سلامة قاعدة منشورة.

التغطية المحددة لهذا المراجع: 35 ملف C# بقراءة كاملة أو مقاطع مركزة من أصل 57 ملف C# خارج migrations في API/Application/Infrastructure/Domain/Contracts؛ علاوة على فحص csproj الثلاثة ومقاطع اختبارات النتائج. هناك 28 ملف migration/snapshot و27 ملف C# اختبارات في الأقسام المذكورة؛ جردها لا يساوي مراجعة دلالية كاملة. ليست هذه شهادة مراجعة جميع ملفات المشروع أو جميع أحداث الشاشات.

قائمة الملفات التي قُرئت (كاملة أو مقاطع):

- API (8): `Program.cs`؛ `Waybills/WaybillApiModule.cs`؛ `Waybills/WaybillFinanceApiModule.cs`؛ `Waybills/ShippingExecutionApiModule.cs`؛ `Accounting/ReceiptApiModule.cs`؛ `Accounting/GeneralLedgerSettingsApi.cs`؛ `Authentication/DesktopLoginModule.cs`؛ `Authentication/DesktopReceiptScopeModule.cs`.
- Application (4): `P1Baseline/P1InMemoryBaseline.cs`؛ `Waybills/WaybillApplicationService.cs`؛ `Waybills/WaybillFinanceApplicationService.cs`؛ `Waybills/ShippingExecutionApplicationService.cs`.
- Domain (3): `Waybills/WaybillAggregate.cs`؛ `Waybills/WaybillFinancialRules.cs`؛ `Waybills/ShippingExecutionRules.cs`.
- Contracts (3): `Core/MoneyContracts.cs`؛ `Core/OperationContext.cs`؛ `Accounting/ReceiptContracts.cs`.
- Infrastructure/Persistence (17): `ConcurrencySafeWaybillRepository.cs`؛ `WaybillPersistenceServices.cs`؛ `WaybillFinancePersistence.cs`؛ `ShippingExecutionPersistence.cs`؛ `VoucherLifecycleService.cs`؛ `SyncOperationService.cs`؛ `AuditEventService.cs`؛ `TransportErpPersistenceExtensions.cs`؛ `ReceiptWorkspaceService.cs`؛ `ReceiptPostingPlan.cs`؛ `ReceiptReferences.cs`؛ `ReceiptWorkflow.cs`؛ `GeneralLedgerPolicyService.cs`؛ `ReceiptAttachments.cs`؛ `TransportErpDbContext.cs`؛ `TransportErpP2ModelCustomizer.cs`؛ `TransportErpP2ShippingModel.cs`. كذلك فُحصت أسطر indexes/refs المحددة في `TransportErpP2FinanceModel.cs` بالبحث؛ لا تحتسب كقراءة كاملة.

حالة التسليم بعد تصحيح المنسق: أصلح المنسق CODE-01 بإضافة نسخ Volume، وCODE-31 بقلب المدين والدائن في النموذج الذاكري، وأضاف اختبارات انحدار؛ راجع مراجع مستقل فرق المصدر ونجح git diff --check. تحققت أنا من السطرين المصححين في ملف التسليم. **اختبارات .NET والبناء لم ينفذا لغياب dotnet**؛ لا تُعد النتيجتان مجازتَين تشغيليًا. أوصافهما أدناه تثبت عيب SHA المصدر وآلية إصلاحه، وليستا عيبين متروكين في نسخة التسليم المصححة.

يوجد في النسخة الجديدة تسجيل دخول محلي قائم على PasswordHasher، اختيار شركة وفرع وفترة، صلاحيات القبض تُقرأ من قاعدة البيانات، سند قبض JSON محفوظ، ترحيل ينتج رأس قيد وسطورًا متوازنة في معاملة واحدة مع الترقيم والتدقيق، وعكس قبض يقلب المدين والدائن فعلًا. هذه أفعال منفذة في كود المصدر وليست مجرد أسماء أزرار. بالمقابل، `P1InMemoryService` و`VoucherLifecycleService` القديمان لا يظهر لهما استدعاء إنتاجي في API؛ استخدامهما الظاهر في الاختبارات. لذلك لا يجوز إسقاط عيوب النموذج القديم على ترحيل القبض الجديد.

## نتائج مثبتة في المسارات الإنتاجية

### CODE-01 — فقدان حجم صنف البوليصة عند الحفظ — عالٍ

الدليل: `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs:75-84,119-136`؛ `WaybillPersistenceServices.cs:210` يثبت أن النسخ الآخر ينسخ Volume؛ تسجيل المستودع الفعلي `TransportERP.Api/Waybills/WaybillApiModule.cs:16`.

كل SaveAsync يحذف أصناف البوليصة ويعيد إنشاءها، لكن ToItemEntity لا يعيّن `Volume`. إدخال Volume=50 وأبعاد حجمها 24 يؤدي إلى استجابة حفظ تحتوي 50 من الـ aggregate، ثم تحميل من قاعدة البيانات بقيمة null؛ كشف الحمولة يستعمل حاصل الأبعاد 24. يتكرر الفقد عند submit/return/approve أيضًا. أصغر إصلاح: إضافة `Volume = x.Volume`؛ الأفضل لاحقًا تحديث الأصناف الموجودة بدل حذفها جميعًا. الإثبات: حفظ draft بالحجم الصريح، إعادة القراءة في DbContext جديد، submit/approve، إنشاء manifest؛ تحقق بقاء 50 والتوزيع النسبي الصحيح.

### CODE-02 — رفض إعادة إرسال خطة الدفع الناجحة — عالٍ

الدليل: `TransportERP.Application/Waybills/WaybillFinanceApplicationService.cs:47-49` يفحص Version قبل `store.SetPaymentPlanAsync`؛ بينما `WaybillFinancePersistence.cs:35-40` يحوي replay الذي لا يصل إليه التطبيق.

نجاح الخطة يرفع الإصدار؛ إعادة الطلب نفسه مع ExpectedVersion القديم بعد ضياع الرد تعيد CONCURRENCY_CONFLICT بدل نتيجة العملية. أصغر إصلاح: نقل منطق replay والتحقق الكامل من البصمة إلى موضع واحد قبل رفض الإصدار، مع رفض نفس المفتاح بمحتوى مختلف. الإثبات: طلب متطابق مرتين عبر Application/API، ثم إعادة المفتاح بسطور أخرى.

### CODE-03 — مفاتيح عمليات البوليصة لا تتحقق من محتوى أو نوع الأمر — عالٍ

الدليل: `WaybillApplicationService.cs:79-81,311-320`؛ `WaybillPersistenceServices.cs:38-42` يقارن LastClientOperationId فقط.

إنشاء draft جديد بنفس المفتاح ولكن وجهة/عملة أخرى يعيد القديم دون conflict. بعد update ناجح، استعمال مفتاح update نفسه مع إصدار قديم في submit يجعل IsReplay يعيد true؛ API يعلن OK وبوليصة بقيت DRAFT، دون تنفيذ submit. أصغر إصلاح: outcome سجل مرتبط بالشركة/الفرع/الكيان/نوع الأمر وبصمة المحتوى، كما فعلت أوامر shipping النهائية. الإثبات: نفس المفتاح مع محتوى متغير، ونفس المفتاح لأمرين مختلفين، وإعادة أمر قديم بعد أمر أحدث.

### CODE-04 — سجل التدقيق خارج معاملة إنشاء/تعديل/إرسال البوليصة — عالٍ

الدليل: `WaybillApplicationService.cs:88-95,119-122,158-161,180-183,201-204,282-287`؛ `ConcurrencySafeWaybillRepository.cs:87-88` ينهي معاملته قبل audit؛ فقط approve مغلف في UnitOfWork.

فشل audit بعد نجاح الحفظ يترك التغيير التجاري محفوظًا مع رد فاشل وسجل ناقص. على retry يعاد replay مباشرة ولا يعاد التدقيق، فيبقى النقص. ينطبق على إنشاء الطرف كذلك. أصغر إصلاح: استخدام UnitOfWork لكلا الحفظ والتدقيق، وإضافة مراجعة ذرية أو outbox واضح. الإثبات: audit sink يرمي بعد save؛ قاعدة البيانات يجب أن تبقى كما كانت، ثم إعادة ناجحة تنتج سجلًا واحدًا.

### CODE-05 — بصمة تدقيق P2 تتغير بعد تخزين PostgreSQL — عالٍ

الدليل: `WaybillPersistenceServices.cs:416,421` يستخدم UtcNow كامل الدقة؛ `AuditEventService.cs:173` يضم تمثيل timestamp ذو سبع خانات في hash؛ خريطة OccurredAt timestamptz. المسار الجديد للقبض ينظّمها بالفعل في `AuditEventService.cs:64-65` والمسار القديم لا يفعل.

عندما لا تكون ticks قابلة للقسمة على 10، PostgreSQL يخزن microseconds فتختلف البصمة بعد قراءة جديدة. أصغر إصلاح: توحيد مولد أحداث P2 مع دقة microsecond، وترتيب متزايد داخل stream. الإثبات: إنشاء حدث P2 بوقت مضبوط ينتهي tick=1 ثم roundtrip والتحقق. لا تعني القراءة الحالية أن كل سجل سابق فاسد؛ ذلك يتطلب فحصًا على قاعدة فعلية.

### CODE-06 — التحقق الجزئي من سلسلة التدقيق يدمج streams مستقلة — متوسط

الدليل: `AuditEventService.cs:134-150`؛ وجود أي companyId/branchId/deviceId يجعل VerifyStream يعمل مرة واحدة على جميع الأحداث المطابقة.

التحقق لشركة وحدها بها فرعان أو لفرع به جهازان يجمع سلاسل مستقلة في سلسلة واحدة ويبلغ PREVIOUS_HASH_MISMATCH لسجل سليم. أصغر إصلاح: التجميع حسب Company/Branch/Device بعد تطبيق أي فلاتر، وليس عند غياب الفلاتر فقط. الإثبات: شركتان/فرعان/جهازان، ثم verify لكل مرشح جزئي وكامل.

### CODE-07 — بصمة التدقيق لا تغطي لقطات before/after أو نوع الكيان — متوسط

الدليل: `AuditEventService.cs:163-176` لا يشمل BeforeJson/AfterJson/EntityType/DeviceId/Ip رغم أن response يعرضها.

ComputeHash ينتج نفس البصمة إذا تغيرت لقطة بيانات السند أو نوع كيانه. حظر UPDATE في القاعدة يمنع مسارًا عاديًا للتعديل، لكنه لا يجعل التحقق الحالي إثباتًا لسلامة تلك الحقول. الإصلاح: canonical encoding مؤرشف بإصدار يشمل جميع حقول الدليل، مع سياسة انتقال تحفظ التحقق من السجلات السابقة. الإثبات: تعديل كل حقل في نسخة غير محفوظة والتأكد أن البصمة تختلف. لا ينصح بتغيير خوارزمية التاريخ القديم مباشرة.

### CODE-08 — يمكن اعتماد بوليصة باستخدام تسلسل سند قبض — عالٍ

الدليل: `WaybillApplicationService.cs:209-268` يمرر NumberSequenceId؛ `WaybillPersistenceServices.cs:303-305` يراجع company/branch/status ولا يراجع DocumentType.

تمرير تسلسل RECEIPT_VOUCHER للبوليصة يستهلكه ويولد رقم بوليصة ببادئة القبض. خدمة القبض الجديدة تتحقق من النوع في ReceiptWorkspaceService:141. أصغر إصلاح: تحقق صريح من document type في حجز البوليصة/خدمة متخصصة، قبل زيادة NextValue. الإثبات: sequence للقبض وموافق للبوليصة؛ الخاطئ مرفوض دون استهلاك رقم.

### CODE-09 — قراءة حجز الترقيم المعاد لا تقيد الفرع — عالٍ

الدليل: `WaybillPersistenceServices.cs:294-301` يبحث Company+IdempotencyKey فقط، في حين ScopedReservation:367-369 يراجع branch.

داخل الشركة، استخدام مفتاح حجز لفرع آخر يمكن أن يعيد DTO لحجز ذلك الفرع قبل فحص نطاق التسلسل. قد ينتهي التنفيذ لاحقًا برفض الربط، لكنه يكشف ويحمل حجزًا من نطاق غير مسموح بدل رفضه في القراءة. الإصلاح: التحقق من scope أيضًا عند replay ومن ارتباط الحجز بالكيان المقصود. الإثبات: فرعان، نفس key؛ عدم إعادة حجز الفرع الأول للثاني.

### CODE-10 — تخصيص شحنة بعد توليد الكشف يخلق تعارضًا يمنع الإقفال حتى التراجع — عالٍ

الدليل: `ShippingExecutionPersistence.cs:199-201` يسمح allocate لكل trip DRAFT دون فحص وجود manifest؛ GenerateManifest:374-377 يستبعد الأصناف المرتبطة بكشف؛ `TransportErpP2ShippingModel.cs:115-117` يفرض كشفًا واحدًا لكل trip؛ Finalize:586-589 يشترط تغطية جميع allocations. لا يوجد refresh manifest في IShippingExecutionStore/API.

توليد كشف لتخصيص A، ثم تخصيص B في الرحلة نفسها مسموح. finalize للكشف الأول يرفض coverage، وتوليد كشف ثان يرفض unique TripId؛ لا يمكن unallocate A لأنه مرتبط بكشف. **يمكن التعافي حاليًا بإلغاء تخصيص B غير المرتبط بالكشف** وفق Unallocate:287-298 ثم إقفال كشف A؛ إذًا ليست رحلة محبوسة بلا أي مخرج، لكن B لا يمكن ضمه إلى الكشف الحالي ويجب التراجع عن إجراء سبق قبوله. أصغر إصلاح: منع allocate الجديد بعد وجود كشف مع رسالة واضحة؛ أو أمر refresh قبل التحميل يحافظ على IDs. الإثبات: A→generate→B→load→finalize ثم unallocate B→finalize، وتوثيق السلوك المقصود.

### CODE-11 — إضافة حجز/إيقاف بعد finalize لا تمنع مغادرة الرحلة — عالٍ

الدليل: EnsureNoActiveHold يستدعى release/allocate/load/finalize، لكنه لا يستدعى في `ShippingExecutionPersistence.cs:725-821` StartTrip ولا handover:652-705.

سجل WaybillHold ACTIVE أضيف بين finalize وstart يبقى قائمًا بينما start ينشئ DEPART. لا توجد واجهة إنشاء hold حالية، لذلك إعادة الإنتاج تستلزم إدخاله كبيانات مستودع أو خدمة مستقبلية؛ الخلل ثابت في حماية التنفيذ وليس ادعاء وجود شاشة hold. الإصلاح: إعادة فحص holds لجميع البوالص في معاملة start قبل التحول. الإثبات: finalize، ACTIVE hold، handover، start يرفض ولا ينشئ DEPART.

### CODE-12 — local JWT لا يسمح بالمزامنة الحالية — فجوة تكامل عالية

الدليل: `DesktopLoginModule.cs:152-161` يصدر claims sub/company/branch/fiscal_period/scope/permission فقط؛ `Program.cs:106-111` يطلب device_id وdevice_registered=true.

حتى المستخدم ذو sync.operations.execute الذي دخل من login الجديد يُرفض DEVICE_NOT_REGISTERED دائمًا. الإصلاح يتطلب تسجيل أجهزة مؤكد وتذكرة جلسة مرتبطة بالجهاز؛ لا ينبغي إزالة الشرط أو اختراع claim true. الإثبات: login/select-scope ثم sync batch بجهاز مسجل وغير مسجل.

### CODE-13 — المزامنة تخزن طابورًا دون منفذ إنتاجي — فجوة تكامل عالية

الدليل: API batch يستدعي EnqueueSyncOperationAsync فقط؛ بحث جميع مصادر API/Application/Infrastructure عن BackgroundService/IHostedService/Register worker أو استدعاء TransitionSyncOperationAsync لا يكشف executor إنتاجيًا. وظائف transition/retry/conflict موجودة كخدمات وغير مربوطة endpoints.

200 في batch يعني QUEUED؛ لا توجد هنا معالجة payload لإحداث تعديل فعلي على البوليصة/الحساب/القبض أو تنزيل تغييرات. الإصلاح: command dispatcher موثق + worker + dedup/permission/version checks + واجهة نتائج، بعد حسم العقود. الإثبات: offline إنشاء ثم اتصال يؤدي لكيان واحد محفوظ وإصدار نتيجة، لا مجرد صف queue.

### CODE-14 — replay المزامنة يتحقق hash الجسم فقط ويتجاهل هوية الأمر — عالٍ

الدليل: `SyncOperationService.cs:85-95,130-138` يقارن PayloadHash فقط.

نفس Device+ClientOperationId والجسم `{}` يمكن إرساله مع EntityId أو EntityType أو OperationType أو BaseVersion آخر؛ يعاد الصف القديم دون mismatch. الإصلاح: بصمة كاملة تشمل نوع الأمر والكيان والإصدار والجسم، أو مقارنتها صراحةً. الإثبات: تغيير EntityId فقط وإبقاء الجسم/hash؛ يجب رفضه.

### CODE-15 — صف sync فاشل يفسد العناصر اللاحقة في نفس batch — عالٍ

الدليل: `SyncOperationService.cs:122-139` يفصل entity فقط عند unique violation؛ ValidateCommand لا يحد طول ClientOperationId/EntityType؛ خريطة DbContext تحدهما إلى 120. `Program.cs:118-148` يلتقط أي exception ثم يواصل باستخدام DbContext نفسه دون فصل entity المرفوض.

أول operation بمفتاح 121 حرفًا يفشل DB save ويظل Added؛ operation صحيح يليه يحاول SaveChanges إعادة حفظ الصف الخاطئ فيفشل أيضًا. الإصلاح: تحقق طول/JSON/refs قبل tracked mutation، واستخدام سياق أو savepoint مستقِل لكل عنصر، فصل التغييرات الخاصة بالعنصر عند الفشل. الإثبات: batch [طول غير صالح، صحيح]؛ الثانية يجب أن QUEUED وتحفظ فعلًا.

### CODE-16 — عنصر null في batch يسبب 500 بدل نتيجة رفض — متوسط

الدليل: `Program.cs:118-146` يقرأ item.EntityId داخل try ثم يقرأ item.ClientOperationId داخل catch؛ عقد IReadOnlyList لا يمنع JSON `[null]` أثناء التشغيل.

NullReferenceException الأول يلتقطه catch العام، لكن الثاني داخل catch يهرب. الإصلاح: validate null element قبل try واستخدام نتيجة مرتبطة بفهرس العنصر عندما لا يوجد clientOperationId. الإثبات: operations=[null,valid]؛ HTTP200 بنتيجتي رفض/نجاح دون انهيار الطلب.

### CODE-17 — transition يسمح بتجاوز رفض retry وحده — متوسط

الدليل: `SyncOperationService.cs:162-166` وIsAllowedTransition FAILED→SENDING؛ RetryOperation:199-229 يمنع errors غير قابلة للإعادة ويحد MaxRetryCount، لكن Transition لا يطبق هذين الشرطين.

المستدعي المباشر يستطيع نقل FAILED/VALIDATION_ERROR إلى SENDING أو تجاوز retry limit دون RetryOperation. الخدمة غير معروضة حاليًا عبر API فلا يوصف هذا كمسار مستخدم منشور. الإصلاح: مركزية قواعد retry في state transition وتحديث العدد والتوقيت مرة واحدة. الإثبات: FAILED غير قابل، retry exhausted، ووقت backoff؛ جميعها لا تصل SENDING.

### CODE-18 — null حقول متداخلة تتحول إلى 500 في APIs — متوسط

الدليل: `WaybillApplicationService.cs:273,329` request.Address/input.Address؛ `WaybillFinanceApplicationService.cs:78` request.Amount؛ `ReceiptWorkspaceService.cs:209,59,234` Lines/Additional؛ API Execute الجديد لا يلتقط NullReferenceException، والقديم كذلك.

إرسال null لهذه الحقول صالح نحويًا في JSON؛ NRT ليست validation تشغيلية. الإصلاح: validator صريح متسلسل يتعامل مع null وبنية القوائم، قبل استعمالها، وإرجاع error field مناسب. الإثبات: request لكل null nested field، قائمة بها null، حدود الأطوال، دون 500 أو حفظ جزئي.

### CODE-19 — التحقق من نوع role يسمح بالأرقام غير المعرفة — متوسط

الدليل: `WaybillApplicationService.cs:327` Enum.TryParse يقبل "99"؛ WaybillPartyValue.EnsureValid لا يتحقق Enum.IsDefined؛ الخريطة CHECK SENDER/RECEIVER/PAYER ترفض لاحقًا.

Draft بparty role="99" يمر طبقة المجال ويصل خطأ constraint غير معالج في المستودع الآمن ExecuteSave، فينتج 500. الإصلاح: Enum.IsDefined أو قاموس قيم نصية مسموحة، مع رفض قبل الحفظ. الإثبات: "99" و"Sender" و"SENDER" وفق العقد.

### CODE-20 — قراءة party replay قد تعيد بيانات فرع آخر — عالٍ

الدليل: `WaybillApplicationService.cs:279-281` CreateParty replay؛ `WaybillPersistenceServices.cs:251-256` GetByClientOperation يبحث company+operation فقط، بينما search:237-239 يقيّد branch.

مستخدم فرع B يرسل مفتاح إنشاء مستخدم بفرع A فتعود بيانات الطرف بما فيها الهاتف والعنوان خارج نطاق نتائج بحث B. الإصلاح: company-wide key يمكن الاحتفاظ به، لكن تحقق branch/party visibility قبل الإرجاع وارفض conflict. الإثبات: فرعان في الشركة مع نفس operation key.

### CODE-21 — أخطاء إنشاء الطرف كلها تصنف كتحذير تكرار — متوسط

الدليل: `WaybillPersistenceServices.cs:281` catch DbUpdateException بالكامل → PARTY_DUPLICATE_WARNING.

عملة/شركة/فرع غير صالح، أطوال كبيرة، أو خطأ constraint يقال عنه duplicate؛ concurrent retry صحيح لا يعيد الكيان الموجود. الإصلاح: تصنيف SqlState/constraint محدد، إعادة قراءة replay في unique-operation فقط، وإعادة ترميز validation/infrastructure الحقيقي. الإثبات: replay متزامن، name زائد، FK غير صالح، ثم التحقق من error وentity count.

### CODE-22 — التحصيل يستخدم حقل محاسبي دون مطابقة المبلغ/العملة/الاتجاه — عالٍ

الدليل: `WaybillFinancePersistence.cs:272-301` EnsureAccountingReferenceAsync يفحص وجود المستند ونطاقه وحالته فقط؛ RecordCollection:118-128 يربط المبلغ المرسل مباشرة.

سند قبض 10 يمكن ربطه بتحصيل 100 أو بعملة أخرى؛ نفس المستند يمكنه تغطية عدة بوالص دون توزيع يتأكد من إجمالي السند. PAYMENT_VOUCHER يقبل لتحصيل موجب أيضًا دون سياسة اتجاه. الإصلاح: allocations مالية typed تربط المستند والسطور وتتأكد من العملة والمبلغ القابل للتخصيص والاتجاه، في المعاملة ذاتها. الإثبات: مرجع 10 ومحاولة 100؛ عدة بوالص تتجاوز إجمالي المستند؛ اتجاه عكسي.

### CODE-23 — سند قبض معكوس يظل مرجعًا صالحًا لتحصيل جديد — عالٍ

الدليل: `ReceiptWorkspaceService.cs:198` يضيف ReversalJournalId دون تغيير Status؛ Document:258 يعرض REVERSED بالاشتقاق؛ `WaybillFinancePersistence.cs:282-284` يفحص Status APPROVED/POSTED فقط.

عكس السند بنجاح يحافظ على Status=POSTED في التخزين، لذلك جمع جديد مرتبط به مقبول رغم زوال أثر القبض بالقيد العكسي. أصغر إصلاح في فحص المرجع: ReversalJournalId==null؛ يلزم كذلك سياسة للعلاقات الموجودة عند عكس السند. الإثبات: post receipt → reverse → record collection referencing it؛ يجب رفضه. لا يغير الإصلاح حالة كل جدول دون migration متفق عليه.

### CODE-24 — سند القبض المرتبط بالبوليصة لا يحدث تحصيلها — فجوة محاسبية عالية

الدليل: `ReceiptWorkspaceService.cs:234-237` يفحص line.WaybillId ووجودها فقط؛ Post:147-160 ينشئ JournalEntry ويمرر السند POSTED؛ لا إنشاء CollectionTransaction أو FinancialLink أو تحديث Waybill.FinancialStatus. Reverse:188-200 لا ينشئ reversal collection كذلك.

اختيار بوليصة في سطر سند ثم ترحيله يعرض رابطًا ويُرحّل ماليًا، لكن البوليصة تبقى UNPAID حتى استدعاء API آخر مستقل، قد يكرر التحصيل إذا لم تكن الملكية واضحة. الإصلاح يحتاج عقد ملكية ومفاتيح سطور وتخصيصات موحدة، وليس زرًا إضافيًا فقط. الإثبات: بوليصة مستحقة 100 وسند لها 100؛ انتقال واحد PAID ثم reverse إلى UNPAID، مع replay ومنع duplication.

### CODE-25 — تحصيل جديد يقبل تاريخ فترة مقفلة — عالٍ

الدليل: `WaybillFinancePersistence.cs:102-148` RecordCollection لا يفحص fiscal period، في حين Reverse:187-193 يفحص الفترة الأصلية المقفلة.

يمكن إضافة تحصيل بتاريخ شهر مغلق ثم يصبح عكسه ممنوعًا فورًا. الإصلاح: تعريف تاريخ محاسبي منضبط بالمنطقة الزمنية، والتحقق من فترة مفتوحة عند الإدخال، أو فصل التحصيل التشغيلي عن اعتماد أثره المحاسبي مع حالات واضحة. الإثبات: closed period ثم record collection به؛ السياسة يجب أن تمنع أو تعزل الأثر صراحةً.

### CODE-26 — التاريخ المحاسبي للعكس يعتمد UTC بدل تاريخ العمل — متوسط

الدليل: `WaybillFinancePersistence.cs:187` original.CollectedAt.UtcDateTime.Date.

جمع 9 أكتوبر 00:30 +03 يصبح 8 أكتوبر في فحص الإقفال. الإصلاح: business date مستقل محفوظ أو timezone الشركة قبل استخراج Date؛ لا تستعمل timezone الخادم. الإثبات: حد منتصف الليل اليمني، وفتح يوم وإقفال اليوم المجاور.

### CODE-27 — replay التحصيل يتجاهل بيانات جوهرية — متوسط

الدليل: `WaybillFinancePersistence.cs:334-341` يقارن waybill/currency/amount/rate فقط؛ reversal replay:171-178 يقارن collectionId فقط.

نفس المفتاح مع PartyId أو payer/method/collector/time/accountingRef آخر يعاد كنجاح؛ reversal مع سبب/مرجع مختلف يعاد القديم. الإصلاح: كامل بصمة الطلب. الإثبات: تغيير كل حقل على حدة وإبقاء المفتاح.

### CODE-28 — uniqueness للتحصيل على مستوى الشركة لكن replay على مستوى الفرع — متوسط

الدليل: `TransportErpP2FinanceModel.cs:57` Company+ClientOperationId unique؛ `WaybillFinancePersistence.cs:266-268` يبحث Company+Branch+Key.

نفس operationId مشروع في فرعين لا يعاد scoped replay، بل يتسبب DUPLICATE_OPERATION. لا توجد ضمانات عالمية توليد key في العقد. الإصلاح: تقرير مستوى uniqueness ثم توحيد index وlookup والعقد، دون حذف index حمايةً قبل الترحيل. الإثبات: فرعان ونفس المفتاح حسب السياسة المختارة.

### CODE-29 — تغير إعدادات القبض يعطل سندًا سبق مراجعته/اعتماده — متوسط، قرار سياسة مطلوب

الدليل: Review/Approve/Post تستدعي SettingsAsync الحالية؛ GeneralLedgerPolicyService يحفظ snapshot **سير العمل فقط**؛ ReceiptPostingPlan policy snapshot ينشأ أثناء post.

حذف Type/Collector/Destination من إعدادات الشركة بين الاعتماد والترحيل يمنع Validate، وتغيير الحساب المرتبط بالوجهة يغير حساب الترحيل لسند اعتمد سابقًا. لا نفترض أن ذلك مخالف لسياسة مستخدم معتمدة؛ هو السلوك الفعلي ويلزم حسم هل الاعتماد يجمد إعدادات الترحيل. الإصلاح وفق القرار: freeze snapshot عند save/approve أو إبطال الاعتماد صراحة مع audit. الإثبات: سند معتمد ثم تعديل وجهة حسابه أو حذفه.

### CODE-30 — جلسة الفترة المختارة لا تقيد تاريخ سند القبض — متوسط، قرار نطاق مطلوب

الدليل: JWT يحوي fiscal_period_id؛ ReceiptApiModule.Execute:133-140 ينشئ OperationContext دونه؛ Post:136-139 يختار أي OPEN period يحوي التاريخ.

اختيار فترة A في الدخول لا يمنع سندًا مؤرخًا في فترة B المفتوحة بالشركة. يلزم تحديد هل الاختيار مجرد سياق عرض أم نطاق إدخال؛ لا يعلن هذا عيبًا محاسبيًا محسومًا قبل القرار. الإصلاح بعد القرار: carry period context والتحقق أو إزالة الإيحاء من الواجهة. الإثبات: فترتان مفتوحتان، login A ثم receipt date B.

## نتائج النموذج القديم/الخدمات غير المربوطة إنتاجيًا

### CODE-31 — ReverseJournal في النموذج الذاكري لا يعكس المدين والدائن — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:389`: original with ينسخ Lines الأصلية كما هي. النتيجة قيد تعويضي بنفس اتجاه الأصل، لا عكسه. **خدمة القبض الجديدة تعكس السطور صحيحًا في ReceiptWorkspaceService:196-197.** إصلاح القديم: `Lines = original.Lines.Select(l => l with { Debit=l.Credit,Credit=l.Debit }).ToArray()`، وإسناد فترة عكس مفتوحة. الاختبار الحالي P1InMemoryBaselineBehaviorTests:60-62 يراجع ReversalOf/audit فقط ولا يفحص مبالغ السطور.

### CODE-32 — idempotency القيد في الذاكرة غير مقيد بالشركة — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:361-362`: بحث ClientOperationId عبر كل شركات Store. شركة B بنفس key تستلم قيد A بعد تحقق حسابات B دون تحقق owner. أصغر تصحيح Company/Branch+key وبصمة. الإثبات شركتان.

### CODE-33 — مدخلات القيد الذاكري تسمح بالمبالغ السالبة أو مدين ودائن في نفس السطر — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:358-360` مجموع متوازن وحسابات قابلة للترحيل فقط. القاعدة PostgreSQL `ck_journal_lines_amounts` تمنع السلوك، لذلك اختبارات النموذج لا تمثل القاعدة. الإصلاح validation نفس قواعد السطور. الإثبات قيد balanced negative، سطر يضع debit/credit معا، وقيد صفر.

### CODE-34 — فترات الذاكرة متداخلة مسموحة — متوسط

الدليل `P1InMemoryBaseline.cs:331-339` يفحص start>=end فقط ويسميه PERIOD_OVERLAP؛ لا مقارنة بالفترات الأخرى. قاعدة EF الحالية تمنع فقط التطابق التام عبر unique Company/Start/End ولا تمنع التداخل أيضًا؛ القبض الجديد يتحقق periods.Count==1 ويمنع post عند التداخل. الإصلاح قيد range exclusion/خدمة إدارة فترات حسب السياسة. الإثبات فترتان متقاطعتان غير متطابقتين.

### CODE-35 — VoucherLifecycleService القديم يرحّل بتغيير حالة فقط — فجوة إذا أعيد ربطه

الدليل `VoucherLifecycleService.cs:108-142`: PostReceipt/PostPayment → TransitionAsync فقط؛ actorId لا يدخل audit أو stamp، ولا إنشاء قيد. لا يوجد تسجيله داخل API. لا تستخدمه لتنفيذ سند الصرف قبل تكميل pipeline محاسبية؛ ترحيل القبض الفعلي الجديد مختلف وموجود.

## حدود تنفيذ وملاحظات تفصل الموجود عن المطلوب

1. endpoints عمليات البوليصة لا تحوي GET بوليصة واحدة/قائمة، وعمليات shipping لا تحوي GET رحلة/كشف/تفاصيل عامة. responses العمليات قد تكفي أثناء المسار المتصل، لكنها لا تكفي بعد إعادة تشغيل التطبيق لاكتشاف IDs واستئناف المسودة. GetFinancialStatus خدمة موجودة دون endpoint في WaybillFinanceApiModule.
2. `MovementEvent` يقيد EventType إلى LOAD/DEPART في TransportErpP2ShippingModel:149. ARRIVED/CLOSED أسماء حالات في constants وليست أوامر وصول/تسليم قابلة للتنفيذ في API. لا يوجد هنا إثبات اكتمال رحلة البوليصة حتى التسليم أو GPS.
3. CreateTrip يتحقق أن VehicleId/DriverId GUID غير فارغ فقط؛ خريطة Trip لا تحوي FK vehicle/driver. يمكن إنشاء رحلة بأرقام عشوائية. يجب حسم master بيانات وسائل النقل والسائقين ثم تطبيق نطاقها وحالتها؛ لا اختراع جدول جديد أثناء هذه المراجعة.
4. JWT صلاحيات P2 مأخوذة من claims حتى expiry؛ القبض الجديد يعيد قراءة DB. تعطيل المستخدم أو سحب صلاحية P2 لا يُفحص داخل TryContext. السياسة غير موحدة؛ يلزم توحيد revocation/session policy ولا وصف JWT الصحيح بأنه بلا مصادقة.
5. login يسمح قاعدة البيانات بتكرار username بين الشركات ثم يرفض جميع matches عندما count!=1. هذا fail-closed مكتوب صراحة، لكنه يمنع دخول مستخدمين صحيحين إذا تشابه اسمهم بالشركتين. مطلوب حسم login identity أو tenant hint قبل إنتاج multi-company.
6. DI يثبت اتصال PostgreSQL وJWT عند بدء API، فلا يبدأ دونها. لا توجد migration apply داخل Startup، وهذا لا يثبت أن القاعدة خالية أو migrations نُفذت؛ فحص القاعدة يتطلب اتصالًا فعليًا.
7. ملفات csproj تستهدف net10.0. الاختبارات PostgreSQL RequireConnection ترمي عند غياب إعداد البيئة ولا تتخطى صامتًا. لا يجوز تقديم عدد اختبارات «نجح» من مجرد أسماء الملفات.
8. يوجد تكرار TryContext/HasPermission/MapError في ثلاثة موديولات P2، ومساران مختلفان لتدقيق EF، ومستودعا Waybill بآليتي update مختلفتين؛ هذا التكرار أنتج اختلاف Volume ودقة timestamps فعلًا، لذلك توحيد الأجزاء المشتركة ذو سبب مثبت.

## الإصلاحات المرشحة الصغيرة قبل تعديل التصميم

الأكثر محدودية ويمكن مراجعتها دون تغيير شاشة: CODE-01 نسخ Volume؛ CODE-02 ترتيب replay مع بصمة؛ CODE-05 تنظيم تاريخ P2 audit؛ CODE-06 grouping لكل stream؛ CODE-08 document type للترقيم؛ CODE-10 منع تخصيص بعد الكشف؛ CODE-16 null عناصر batch؛ CODE-19 رفض enum غير معرف؛ CODE-23 رفض سند معكوس كمرجع تحصيل. بقية الربط بين القبض والبوليصة، تنفيذ المزامنة، توحيد GPS والسائق/المركبة والفترات تحتاج مخطط عقود وملكية معاملات قبل التنفيذ.

الإقفال المطلوب: تثبيت SHA، تنفيذ اختبارات المصدر والوحدة على .NET10، ثم PostgreSQL disposable database تطبق جميع migrations، ثم API tests مع tokens ونطاقين وشركتين، وإعادة القراءة بسياق جديد للتحقق من الدقة والتدقيق. نجاح compile وحده لا يثبت نقل الأموال أو replay أو الوصول والتسليم.

## عيوب قديمة استبعدت من التقرير الحالي

`SyncOperationService.BaseVersion` كان غير منسوخ في 1026a906 وأصبح منسوخًا في 9d02ac8d:110. كذلك canonical trim لمفاتيح replay أضيف :74-80، والتحقق من FAILED error قبل mutation أضيف :168-170. لا تعرض هذه الثلاثة كعيوب حالية. Audit القبض الجديد ينظّم timestamps ويسلسلها :64-65، وعكسه يقلب السطور صحيحًا؛ المشكلة الحالية المحددة هي P2 sink والنموذج الذاكري القديم.


# مراجعة البيانات والربط — TransportERP

المراجع الفعلي: وكيل `data_planning`، مراجعة مستقلة للقراءة فقط.

النسخة الحاكمة لهذه النتائج: `feature/receipt-general-ledger-settings`، SHA `9d02ac8d19e39971ad233108faecc8154a1d5745`، المسار المحلي `جذر نسخة المصدر`. جرى فحص أولي للنسخة `1026a9069756b956b17b55d853e5ebda535810ec` ثم إعادة فحص النتائج على النسخة الأحدث؛ لا تُستخدم أحكام غياب الربط المحاسبي من النسخة القديمة في الحكم الحالي. جميع مسارات الأدلة التالية نسبية لجذر النسخة الأحدث.

هذه مراجعة واقع الكود، وليست اعتمادًا لقاعدة البيانات أو اكتمال المشروع. لم يُنشأ مخطط أو قاموس جديد، ولم تُطبّق Migration، ولم يُعدل كود التطبيق. تثبيت مرجع Onyx واعتماده يسبق أي تصميم جديد، وفق توجيه المستخدم. نتيجة مراجعة المرجع التأسيسي المنقولة من الوكيل الرئيسي: `NOT READY — GAPS REMAIN`، 33 فجوة مفتوحة، وعدم اعتماد المالك؛ لم يراجع هذا الوكيل ملفات المرجع خارج المستودع، ولا يحول أعدادها إلى أعداد شاشات الكود.

## 1. ما هو منفذ فعليًا

| البند | الحكم المثبت | الدليل |
|---|---|---|
| مزود البيانات | PostgreSQL عبر Npgsql وEF Core 10؛ لا يظهر موفر MySQL في مشاريع الكود التي جرى البحث فيها | `TransportERP.Infrastructure/TransportERP.Infrastructure.csproj:9-16`؛ `TransportERP.Infrastructure/Persistence/TransportErpPersistenceExtensions.cs:9-21`؛ `TransportERP.Api/Program.cs:14-18` |
| تركيب النموذج | نموذج P1 الأساسي، ثم إضافة أساس البوالص، ثم التمويل والشحن | `TransportERP.Infrastructure/Persistence/TransportErpP2CombinedModelCustomizer.cs:10-19`؛ `TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs:13-17` |
| الإعدادات | ثلاث طبقات بيانات: منصة، شركة، فرع؛ فرع الإعدادات مربوط بالشركة والفرع بمفتاح مركب | `TransportERP.Infrastructure/Persistence/P1Entities.cs:139-161`؛ `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:186-217` |
| إعدادات الأستاذ العام | خدمة حفظ حقيقية في `CompanySettings` بمفتاح `accounting.general-ledger.workflow`، نسخة سياسة مستقلة لكل مستند جديد وتدقيق وتزامن | `TransportERP.Infrastructure/Persistence/GeneralLedgerPolicyService.cs:18-83` |
| API الأستاذ العام | GET وPUT على `/api/v1/accounting/settings`؛ تفويض حالي على مستوى الشركة | `TransportERP.Api/Accounting/GeneralLedgerSettingsApi.cs:9-22`؛ `TransportERP.Api/Accounting/ReceiptApiModule.cs:129-170` |
| سند القبض | حفظ تفاصيل، مراجعة، اعتماد، ترحيل قيد، إلغاء قبل الترحيل، عكس بعد الترحيل، مرفقات، حسابات وأبعاد ومراجع | `TransportERP.Api/Accounting/ReceiptApiModule.cs:84-108`؛ `TransportERP.Infrastructure/Persistence/ReceiptWorkspaceService.cs:49-201`؛ `TransportERP.Infrastructure/Persistence/ReceiptReferences.cs:9-32` |
| الربط من سطح المكتب | شاشة الأستاذ العام تتصل بالخدمة وتفتح إعدادات القبض؛ جلسة HTTP موثقة تقيد الشركة والفرع | `TransportERP.Desktop/FrmMain.LedgerSettings.cs:11-79`؛ `TransportERP.Desktop/Authentication/DesktopSession.cs:13-70` |
| البوالص والتنفيذ | كيانات وبنية حفظ للبوالص والأطراف والتفاصيل والدفع والتحصيل والرحلات والتخصيص والكشوف والتحميل والمغادرة | مواضع الجداول أدناه؛ `TransportERP.Api/Program.cs:28-30,81-83` |
| التدقيق والمزامنة | خدمات سجل تدقيق وطابور عمليات API، مع أجهزة وصلاحيات ونطاق | `TransportERP.Api/Program.cs:19-27,85-151`؛ `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:353-419` |

وجود هذه الأكواد لا يثبت أن قاعدة تشغيل فعلية حوت الجداول أو أن النظام يعمل في جهاز المستخدم. لم يتصل المراجع بقاعدة فعلية، ولم يُنفّذ البناء أو الاختبارات في هذه المهمة.

## 2. الجداول الموجودة في مصدر النموذج، مقابل المسجلة توثيقيًا

عدد الجداول الفريدة في EF Snapshot: **41 جدولًا**. يوجد جدول تطبيقي إضافي منشأ عبر SQL في Migration باسم `shipping_command_outcomes`، فيصبح المصدر يعرّف **42 جدولًا تطبيقيًا**. جدول سجل EF `__EFMigrationsHistory` خارج هذا العدد. هذا تعداد التعريفات البرمجية، وليس تعداد قاعدة حية.

| مجموعة | جداول المصدر | عدد | الأدلة |
|---|---|---:|---|
| المؤسسة والهوية والإعدادات | currencies, companies, branches, users, roles, permissions, role_permissions, user_roles, user_permission_overrides, global_settings, company_settings, branch_settings | 12 | `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:77-217` |
| المحاسبة الأساسية | chart_of_accounts, fiscal_periods, financial_dimensions, journal_entries, journal_entry_lines, receipt_vouchers, payment_vouchers | 7 | الملف نفسه `:220-351` |
| التدقيق والمزامنة | audit_events, sync_operations, conflict_cases | 3 | الملف نفسه `:353-419` |
| أساس البوالص | operational_parties, waybills, waybill_parties, waybill_items, number_sequences, number_reservations | 6 | `TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs:19-161` |
| تمويل البوالص | payment_plan_lines, collection_transactions, waybill_financial_links | 3 | `TransportERP.Infrastructure/Persistence/TransportErpP2FinanceModel.cs:15-79` |
| تنفيذ الشحن | item_releases, trips, trip_stops, trip_allocations, manifests, manifest_lines, movement_events, waybill_holds | 8 | `TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs:17-188` |
| الإضافات الحديثة | receipt_attachments, accounting_workflow_snapshots | 2 | `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:308-324` |
| نتائج أوامر الشحن SQL | shipping_command_outcomes | 1 | `TransportERP.Infrastructure/Persistence/Migrations/20260821170000_P2C01CTeam03PostgreSqlHardening.cs:14-36` |

التعداد الثاني المستقل من Snapshot: `TransportERP.Infrastructure/Persistence/Migrations/TransportErpDbContextModelSnapshot.cs:54,138,200,260,336,431,500,555,633,688,761,813,873,930,989,1085,1134,1204,1260,1353,1429,1491,1589,1668,1768,1836,1882,1991,2056,2094,2199,2269,2354,2401,2483,2524,2555,2665,2739,2815,2888`.

السجل `documentation/closeout/P1/TransportERP_سجل_الجداول_الموحد.csv` يصف جداول أقدم بحالة `LEGACY_REGISTERED` وقرار `RETAIN_AND_NORMALIZE`، مثل نقاط الخدمة والمحاور والمناطق وأنواع المركبات والأطراف والوكلاء وإثبات التسليم (`:4-8,16-18,27`). هذه ليست إثباتًا بوجود كيانات EF منفذة. كما أن الأعمدة القديمة تتضمن `bigint` و`tinyint`، بينما النموذج البرمجي الحالي يستخدم Guid وحالات نصية (`P1Entities.cs:20-25,38-61`؛ `TransportErpDbContext.cs:94-119`). يجب المحافظة على مصدر السجل القديم ووضع خريطة تطابق لاحقًا، لا تنفيذه تلقائيًا.

## 3. الفجوات المؤكدة وحالات البيانات غير المربوطة

| المعرف | الأولوية | النتيجة الدقيقة | الأثر العملي | مصدر الإثبات | نوع المعالجة المقترح |
|---|---|---|---|---|---|
| DATA-01 | مرتفعة | واجهة الإعدادات العامة لم تُربط بالحفظ أو النشر أو الاسترجاع أو التدقيق؛ جميعها معطلة صراحة | لا يمكن ضبط السياسة العامة من هذه الشاشة رغم وجود جداول إعدادات | `TransportERP.Desktop/Forms/SystemSettings/General/FrmGeneralSettings.cs:32-48` | تنفيذ خدمات وعقود وربط، بعد اعتماد المرجع؛ لا إعادة تصميم |
| DATA-02 | مرتفعة | إدارة الشركات تصميم فقط؛ الحفظ والتعديل والحذف والبحث والطباعة والشعار معطلة | وجود الشركات في نموذج EF لا يعني أن شاشة الشركة تديرها | `TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.cs:7,32-48` | ربط الكود بالتصميم الحالي بعد تثبيت عقد الشركة |
| DATA-03 | مرتفعة | حقول الشركة المرئية تتجاوز كيان Company الحالي: المجموعة، النشاط، السجل التجاري، البلد/المحافظة/المدينة، الاتصال، البريد، الموقع والعنوان | لا يوجد تطابق مباشر يحفظ كل حقل ظاهر؛ قد تُفقد بيانات إذا رُبطت الشاشة بالكيان المختصر مباشرة | `TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.Designer.cs:1527-1566`؛ `TransportERP.Infrastructure/Persistence/P1Entities.cs:38-48` | دراسة تطابق حقول قبل القاموس، مع إثبات Onyx وقواعد TransportERP |
| DATA-04 | مرتفعة | شاشة أسعار الصرف غير مربوطة بالخادم أو السجلات؛ النموذج لا يعرّف جدول أسعار صرف زمني، رغم وجود FX Snapshot وعوامل سعر في المستندات | سعر مستند ثابت موجود، لكن إدارة السعر وحدود السريان ومصدره ليست خدمة مرجع موحدة | `TransportERP.Desktop/Forms/SystemSettings/General/UcExchangeRates.cs:7,22-44`؛ `TransportERP.Contracts/Core/MoneyContracts.cs:30-74`؛ قائمة Snapshot أعلاه | تثبيت عقد سعر الصرف ثم ربطه؛ تصميم البيانات لاحق |
| DATA-05 | مرتفعة | الجغرافيا لها DTO للدولة والمحافظة والمديرية والمدينة والمنطقة، لكن قائمة EF الحالية لا تحتوي جداول هذه المراجع | البلد والمعرفات الجغرافية في snapshots لا تضمن تحقق العلاقات مع مصدر جغرافي محفوظ | `TransportERP.Contracts/Geo/GeoContracts.cs:7-23`؛ `GeoAddressContracts.cs:7-33`؛ `P2WaybillEntities.cs:20-24,66-70`؛ قائمة Snapshot | مرجع جغرافي موحد بعد ضبط العقد؛ لا إنشاء جداول الآن |
| DATA-06 | مرتفعة | نموذج الرحلة يحمل VehicleId وDriverId وOriginId وDestinationId وتوقف LocationId دون مرجع EF للسائق/المركبة/الموقع في النموذج الحالي | ليس هناك تكامل مرجعي كامل مثبت بالقاعدة لهذه الكيانات؛ لا تُعرض الرحلة كحل مكتمل للأسطول | `TransportERP.Infrastructure/Persistence/P2ShippingEntities.cs:20-45`؛ `TransportErpP2ShippingModel.cs:39-71`؛ قائمة Snapshot | تثبيت مصدر الحقيقة لهذه المعرفات وربط الخدمات تدريجيًا |
| DATA-07 | مرتفعة | تنفيذ حركة الشحن الحالي يقيد event types إلى LOAD وDEPART فقط | لا يوجد في هذا النموذج إثبات اكتمال وصول/تسليم/إرجاع/تلف/مطالبات/GPS | `TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs:144-150`؛ سجل إثبات التسليم القديم `documentation/closeout/P1/TransportERP_سجل_الجداول_الموحد.csv:27` | مراحل تشغيلية مستقلة بعد اعتماد المرجع؛ لا توسعة عشوائية للأحداث |
| DATA-08 | مرتفعة | إعدادات سير الأستاذ العام يدعمها حاليًا RECEIPT_VOUCHER فقط؛ سند الصرف لا يمتلك DocumentJson أو روابط قيد مثل القبض | لا يجوز تعميم نجاح القبض على الصرف أو اليومية أو كل المستندات | `GeneralLedgerPolicyService.cs:37-48`؛ `P1Entities.cs:234-274` | توسيع حسب عقد كل مستند مع فصل دورته واختباره |
| DATA-09 | متوسطة | تغيير إعدادات القبض لا يستدعي AppendAuditEvent في مسار الحفظ؛ الأستاذ العام يستدعي التدقيق صراحة | لا يظهر سجل من غيّر سياسة وجهات/أنواع القبض والتقريب والترقيم عبر هذا endpoint | `TransportERP.Api/Accounting/ReceiptApiModule.cs:72-82`؛ المقارنة `GeneralLedgerPolicyService.cs:65-68` | إصلاح كود: تدقيق تغييرات القبض في معاملة الحفظ نفسها |
| DATA-10 | مرتفعة | تكوين القبض محفوظ على مستوى الشركة، ويتضمن NumberSequenceId واحدًا؛ endpoint يقبل تسلسل أي فرع في الشركة، بينما PostAsync يتطلب التسلسل العام أو تسلسل الفرع الحالي | إذا اختير تسلسل خاص بفرع A في إعداد الشركة، لن يستطيع فرع B الترحيل بهذه السياسة، مع أن إعداد الشركة واحد | `ReceiptWorkspaceService.cs:11-17,139-142`؛ `ReceiptApiModule.cs:62-63,72-80` | دراسة سياسة نطاق الترقيم: عام أو اختيار لكل فرع؛ ثم إصلاح التحقق والواجهة دون تخمين قرار المالك |
| DATA-11 | متوسطة | Currency.MinorUnit يسمح 0..6، بينما خطة ترحيل القبض ترفض أكثر من 4 | إعداد عملة قانوني وفق نموذج العملة قد يُمنع عند ترحيل القبض | `TransportErpDbContext.cs:80-83`؛ `ReceiptPostingPlan.cs:11-15` | حسم موحد لدقة المبالغ وإظهار الحد قبل التشغيل |
| DATA-12 | متوسطة | سند القبض يحفظ تفاصيله في DocumentJson بالإضافة إلى أعمدة رأس مختصرة، والسياسات في JSON؛ أسطر السند ليست جدول EF مستقلًا | استخراج تقارير تفصيلية، تصفية الأسطر وربطها بالحسابات يحتاج استخدام العقد/الخدمة؛ لا تُعامل JSON كحقول موثقة تلقائيًا | `P1Entities.cs:234-254`؛ `ReceiptWorkspaceService.cs:69-93,254-259`؛ `ReceiptContracts.cs:3-9` | توثيق الهيكل والإصدارات وطريقة التقرير؛ لا تحويل إلى جداول قبل القاموس |
| DATA-13 | مرتفعة | سند P1 القديم دون DocumentJson مستبعد من قائمة receipts الحديثة ويتعذر فتحه بخدمة التفاصيل | توجد مشكلة توافق عند وجود سندات محفوظة بالخدمة القديمة؛ لا يشمل الحكم قاعدة المستخدم الفارغة أو المملوءة دون فحصها | `ReceiptApiModule.cs:84-88`؛ `ReceiptWorkspaceService.cs:254-257`؛ `VoucherLifecycleService.cs:39-69` | مسار تهيئة/استكمال بيانات قديمة موثق إن ثبت وجودها |
| DATA-14 | متوسطة | القيد يدعم FinancialDimensionId واحدًا لكل سطر؛ اختيار بُعد الترحيل من costCenter/project/activity واحد فقط | الحقول الثلاثة يمكن التحقق منها وحفظها في تفاصيل السند، لكن لا تتحول كلها إلى أبعاد قيد مستقلة | `P1Entities.cs:221-231`؛ `ReceiptPostingPlan.cs:13-21`؛ `ReceiptReferences.cs:11-17` | قرار محاسبي موثق لاحقًا؛ لا الادعاء بتعدد أبعاد كامل |
| DATA-15 | مرتفعة | ثلاثة مشاريع Mobile الحالية لا تحتوي إلا csproj؛ تنتقل إلى net10.0 Library عند عدم وجود MAUI scaffold، ولا ProjectReference إلى Contracts | ظهور المشاريع في الحل لا يثبت وجود تطبيق العميل أو السائق أو الإدارة، ولا مشاركة إعدادات التشغيل معها | `TransportERP.Mobile.Admin/TransportERP.Mobile.Admin.csproj:4-18,45-50`، ونفس المواضع في Customer/Driver؛ فحص `rg --files` للمجلدات الثلاثة أعاد الملفات الثلاثة فقط | تطبيقات حقيقية لاحقًا، بعقود API مشتركة ومخزن محلي بعد اعتماد السيناريوهات |
| DATA-16 | متوسطة | المفاتيح المرجعية CompanyId وBranchId في أغلب كيانات P1 مرتبطة كلٌّ منها مستقلًا، خلاف إعدادات الفرع ذات FK المركب | القاعدة وحدها لا تمنع كل علاقات الشركة/الفرع المتقاطعة؛ تعتمد على تحقق الخدمة | `TransportErpDbContext.cs:137-138,216-217,285-288,346-348`؛ تحقق القبض `ReceiptWorkspaceService.cs:210-215,240-247` | مراجعة كل خدمة فعليًا واختبارات نطاق؛ لا تعميم ثغرة على كل endpoints |
| DATA-17 | متوسطة | CashBoxId في P1 معرف اختياري دون كيان CashBox مستقل، والقرار التوثيقي يؤجل عقده؛ إعدادات القبض تمثل الوجهات بقائمة JSON مع AccountId | «وجهة قبض» حقيقية في إعدادات القبض لا تثبت إدارة صندوق/بنك كاملة بالعهدة والرصيد والصلاحيات لكل صندوق | `P1Entities.cs:252,271`؛ `ReceiptContracts.cs:15-23`؛ `documentation/architecture/P1_PHYSICAL_SCHEMA_POSTGRESQL.md:44-46` | اكتمال العقد المستقل للصندوق/البنك وربطه دون إنشاء FK زائف |
| DATA-18 | مرتفعة | مسار تعديل إعدادات القبض يكتب إعدادًا مشتركًا للشركة لكن التفويض له لا يطلب companyWide، على خلاف إعدادات GL؛ Allowed يسمح هنا للمستخدم المقيد بفرع ولتعيين صلاحية فرعي | مستخدم مخول بإعداد القبض في فرعه يمكنه تغيير سياسة القبض المشتركة لبقية الفروع؛ الحدود المقصودة لصلاحية configure يجب حسمها | `ReceiptApiModule.cs:42-43,72-80,129-131,150-169`؛ المقارنة `GeneralLedgerSettingsApi.cs:16-17` | إصلاح تفويض شركة إذا كانت السياسة شركة، أو فصل نطاق السياسة بعد اعتماد القرار؛ اختبار مستخدم فرعي |

تصنيف DATA-09 عيب كود محدد يمكن إصلاحه دون إعادة تصميم؛ DATA-10 وDATA-18 تعارض نطاق يحتاج قرارًا موثقًا ثم إصلاح الكود. باقي البنود خليط من نطاق لم يُنفذ وربط لم يكتمل وقرار عقدي مؤجل، ولا يصح اعتبار كل بند خطأ برمجيًا أو حذف حقول الشاشة لمعالجته.

## 4. ربط الإعدادات بين سطح المكتب والهواتف — التوصية التي يجب اعتمادها

**المقترح:** إعدادات العمل الأساسية تُدار من مصدر خادمي واحد وفق نطاق منصة/شركة/فرع، وتستهلكها كل واجهة حسب صلاحيتها. يتغير شكل عرض الإعدادات تبعًا للتطبيق؛ لا تُنشأ نسخة مستقلة متعارضة من إعدادات الشركة لكل تطبيق. السبب أن النموذج يملك بالفعل طبقات إعدادات مشتركة، وأن GL/Receipt المثبتين يستخدمان CompanySettings (`TransportErpDbContext.cs:186-217`؛ `GeneralLedgerPolicyService.cs:21,34-35`؛ `ReceiptWorkspaceService.cs:11-17`). هذا اقتراح معماري يحتاج اعتماد نطاقه، وليس وصفًا لتكامل Mobile الموجود.

| نوع الإعداد | المصدر المقترح | موضع الإدارة المقترح | ما يخص كل تطبيق |
|---|---|---|---|
| الشركة/الفرع/العملة الأساسية ودليل الحسابات | API ومراجع مركزية | سطح المكتب أو إدارة مخولة | تنزيل القيم المصرح بها، اختيار النطاق الحالي |
| سير المستند والتقريب والترقيم | سياسة شركة موثقة، واستثناء فرع إن اعتمد | واجهات إعدادات مخولة | تطبيق السياسة نفسها؛ حفظ snapshot للمستند |
| اللغة والمظهر والطباعة المحلية | تفضيل مستخدم/جهاز وفق قرار النطاق | التطبيق الذي يستعمل الجهاز | خيارات العرض والجهاز فقط |
| عنوان الخادم والمصادقة | ضبط اتصال لكل تثبيت، جلسة موثقة من API | الدخول/الاتصال | لا حفظ connection string PostgreSQL أو secret خادمي في الهاتف |
| العمل دون اتصال والمزامنة | عقد خادمي موحد، صلاحيات ونطاق | سياسة تشغيل مخولة | مخزن محلي وطابور عمليات خاص بالجهاز، مع إصدار السياسة |
| GPS | عقد أحداث/أذونات/تواتر وحماية بيانات مستقل | إعدادات تشغيل وتطبيق السائق | جمع الموقع بعد تعريف السيناريو؛ لا يعني وجود LAT/LON في سجل قديم تنفيذ GPS |

توثيق هذا الجدول اقتراح، لا توجد في هذه المهمة ملفات MAUI منفذة تسمح باختبار تنزيل هذه الإعدادات. دليل الاتصال الموجود اليوم يخص DesktopSession HTTPS وBearer فقط (`DesktopSession.cs:13-27,65-70`).

## 5. ترتيب التنفيذ الاستشاري دون البدء بتصميم قاعدة البيانات

1. **إقفال المرجع التأسيسي Onyx:** مطابقة شاشة/تبويب/حقل/زر/قاعدة بمصدر محدد وصفحة أو صورة، مع فصل قواعد النقل المضافة عن قواعد Onyx. يظل قاموس القاعدة وتصميمها متوقفين حتى اعتماد المالك.
2. **تثبيت واقع النسخة الأحدث:** سجل SHA ومصدر كل مشروع وكل شاشة، وفصل «واجهة جاهزة شكليًا» عن «خدمة مربوطة» و«اختبار ناجح اليوم». مقارنة فرع October بفرع September وعدم استبدال تصميم المستخدم.
3. **حسم النطاق المشترك:** قرارات مكتوبة لمنصة/شركة/فرع/مستخدم/جهاز، وترقيم الفرع، صلاحيات تغيير الإعدادات، دعم القديم، العملات والدقة، اختيار الأبعاد، الإعدادات المسموح بها offline.
4. **مصفوفة تطابق قبل القاموس:** الحقل الظاهر → اسم control → معنى العمل → DTO موجود → entity/JSON موجود → endpoint موجود → تحقق الحفظ → التقرير الذي يستهلكه → المصدر. لا اختيار نوع SQL أو تصميم جدول جديد في هذه الخطوة. تُسجّل DATA-03/05/06/12/17 بوصفها قرارات غير مغلقة.
5. **إصلاحات كود محددة في فرع مستقل:** تدقيق حفظ إعدادات القبض؛ التحقق من نطاق تسلسل القبض بعد القرار؛ توحيد رسائل الحدود والدقة؛ توثيق مسار السند القديم. لا دمج تلقائي إلى الفرع الرئيسي، ولا حذف/إعادة تصميم شاشات.
6. **قاموس بيانات ثم تصميم لاحقًا:** يبدأ فقط بعد اعتماد المرجع ونطاق المشروع. يستند إلى الكود الفعلي والسجلات القديمة كمصدر تاريخي، ولا يحول كل صف `LEGACY_REGISTERED` إلى جدول مطلوب تلقائيًا.
7. **ربط شاشات desktop حسب عائلات التصميم:** الشركات/الفروع؛ المراجع والتهيئة؛ إعدادات السياسة؛ المستندات المالية؛ البوالص والحركة. تُستخدم نفس عقود API، لا EF أو DB مباشرة في واجهة الهاتف.
8. **تطبيقات الهاتف وGPS:** scaffold حقيقي لكل جمهور، علاقة واضحة مع Contracts، مصادقة ونطاق؛ بعدها تنزيل إعدادات ومراجع، عمليات online، ثم offline/conflicts، ثم GPS بأدلة اختبار. لا تُختبر خدمة غير موجودة بوصفها ناجحة.
9. **قبول مستقل على البيئة المناسبة:** بناء Windows/MAUI، اختبارات قواعد ومراجع وعزل شركات وفروع وتزامن وعكس وتدقيق، واختبار migration في قاعدة مؤقتة بعد السماح بالمرحلة. النتائج القديمة لا تحل محل نتيجة SHA الحالي.

## 6. حدود الاختبار وأدلة الاختبارات الموجودة

لا توجد نتيجة اختبار جديدة من هذا المراجع. الوكيل الرئيسي أفاد بعدم توفر dotnet في البيئة. قاعدة البيانات لم تُفحص ولم تُشغّل.

في المصدر اختبارات مفيدة: العملات المختلطة وفارق التقريب وعدم إنشاء قيد عند فشل الترحيل (`TransportERP.Tests/ReceiptWorkspaceTests.cs:20-37`)، النطاق الخاطئ والنسخة القديمة والبوليصة المفقودة (`:185-196`)، والصلاحيات الحالية وسياسة الترحيل التلقائي (`TransportERP.Tests/LedgerWorkflowTests.cs:17-39`). لكن ReceiptWorkspaceTests يستخدم InMemory إلا عند ضبط `RECEIPT_ISOLATED_POSTGRES=54439`، حين ينشئ قاعدة اختبار ويطبّق Migrate (`ReceiptWorkspaceTests.cs:199-210`). لذلك نجاح اختبارات InMemory وحدها لا يثبت قيود PostgreSQL أو transactional behavior أو Migration.

اختبارات PostgreSQL العامة تتطلب صراحة متغير اختبار ولا تتخطى الغياب بصمت (`TransportERP.Tests/PostgreSqlTestEnvironment.cs:8-18`). smoke test يستدعي `db.Database.MigrateAsync()` (`TransportERP.Tests/PostgreSqlPersistenceSmokeTests.cs:13-16`). لم تُشغّل هذه الاختبارات احترامًا لتوقف إنشاء/تعديل القاعدة في هذه المرحلة.

**الحكم:** توجد نواة PostgreSQL حقيقية وسبق الفرع الحديث بتنفيذ ربط سند القبض وسياسة الأستاذ العام. المتبقي المثبت هو اكتمال مصادر المراجع والتهيئة والكيانات التشغيلية والربط الشامل والتطبيقات المحمولة، مع عيوب محددة في تدقيق إعدادات القبض ونطاق الترقيم. لا توجد أدلة تسمح بوصف المشروع كله مكتملًا أو قابلًا للتشغيل النهائي.


# تقرير التحقق المستقل — TransportERP

التاريخ: 2026-10-09. المصدر الذي فُحص: `9d02ac8d19e39971ad233108faecc8154a1d5745` في `TransportERP-latest`، مع مراجعة الفروق للإصلاحين المثبتين لاحقًا بالالتزام `45f2fa81783efba0db8ba5b5209a145efcb2ea1b`. لم يغير هذا المراجع كود المشروع.

## حكم المراجعة وحدودها

المشروع يحتوي تنفيذًا فعليًا لأجزاء من تسجيل الدخول وسند القبض والبوليصة والشحن، لكنه ليس رحلة تشغيل مكتملة بين جميع الشاشات والتطبيقات. حكم هذه المراجعة: **لا يُقبل إقفال المشروع تشغيليًا** من قراءة الملفات أو عدد عناصر الواجهة وحدهما. ثبتت ثغرات ربط وإعادة إرسال بالمسارات الآتية.

المراجعة هنا قراءة مستقلة للمصدر ومسارات محددة وإعادة عدّ، وليست قبولًا بصريًا أو اختبار كل زر في Windows أو مقارنة مكتملة لكل حقل مع Onyx. لا يتوفر `dotnet` في هذه البيئة؛ لذلك لا نجاح بناء ولا نجاح اختبارات معلن. بقي إنشاء قاعدة البيانات والقاموس التنفيذي خارج هذه المهمة. المرجع التأسيسي يجب اعتماده قبل هذا الإنشاء؛ وعدد فجواته يتطلب دليل النسخة الحالية وليس ذاكرة أو رواية سابقة.

التصنيفات: CONFIRMED = مثبت بالمصدر؛ SCOPE_CORRECTION = تصحيح نطاق الادعاء؛ UNVERIFIED = لم يثبت تنفيذيًا في هذه الجولة.

## النتائج المثبتة

| الرمز | التصنيف | النتيجة والدليل | الأثر وشروط الإقفال |
|---|---|---|---|
| QA-01 | CONFIRMED | قبل الإصلاح، `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs`، `ToItemEntity` بدءًا من 119، ينقل الوزن والأبعاد ولا ينقل Volume. عقد الإدخال والتطبيق ينقلانه: `WaybillContracts.cs:29` و`WaybillApplicationService.cs:328,342`. | الحجم المرسل قد يُفقد عند الحفظ في المسار الآمن للتزامن. أصلحه الفرق المحلي بإضافة `Volume = x.Volume`. يلزم تشغيل اختبار PostgreSQL للتحقق من الحفظ والقراءة بعد الاعتماد. |
| QA-02 | CONFIRMED | قبل الإصلاح، `TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs:382`، `ReverseJournal` ينسخ أصل القيد بسطوره نفسها دون تبادل المدين والدائن. | أصلحه الفرق المحلي بتكوين سطور جديدة وتبادل المبلغين. يلزم تشغيل اختبار إلغاء صافي كل حساب وعدم تغيير سطور الأصل. هذا خلل في نموذج P1 في الذاكرة؛ ليس حكمًا على كل قيد عكسي في الإنتاج. |
| QA-03 | CONFIRMED | `WaybillFinanceApplicationService.cs:48` يفحص ExpectedVersion قبل استدعاء التخزين. `WaybillFinancePersistence.cs:34` يحاول معالجة replay بفحص LastClientOperationId، لكن الطلب المعاد بإصداره القديم يُرفض في التطبيق أولًا. | إعادة الطلب نفسه بعد نجاح الاستجابة المفقودة قد تُرجع CONCURRENCY_CONFLICT بدل نجاح العملية الأولى. يلزم سجل عملية مستقل مع بصمة payload وتحقق نطاق؛ إعادة الطلب المطابق قبل رفض الإصدار، ورفض إعادة المفتاح مع جسم مختلف. |
| QA-04 | CONFIRMED | `ReceiptWorkspaceService.cs:116`، `PostAsync` يكوّن journal ويخزن PostingJournalId. `Validate` قرب234 يفحص WaybillId في سطر السند، لكنه لا يسجل CollectionTransaction. `WaybillFinancePersistence.cs:303`، Ledger، يحسب من Collections فقط. | ترحيل سند قبض مرتبط ببوليصة لا يثبت بذاته تحصيلًا في رصيدها المالي. مطلوب ربط محاسبي وتشغيلي في معاملة واحدة أو نمط موثق يمنع التكرار، مع عكس مترابط. لا يُضاف تحصيل آخر تلقائيًا قبل حسم سياسة منع الازدواج. |
| QA-05 | CONFIRMED | `DesktopLoginModule.cs`، IssueSession، يصدر sub/company/branch/fiscal_period/scope/permission، دون device_id أو device_registered. `TransportERP.Api/Program.cs:104` وما بعدها يطلبهما على sync/operations:batch. | جلسة الدخول المحلية الحالية لا تكفي لاستدعاء المزامنة؛ ينتج403 DEVICE_NOT_REGISTERED. يتطلب مسار تسجيل جهاز والتحقق منه وإصدار claims موثوقة، ثم اختبار جهاز مسجل/غير مسجل، جهاز مستخدم آخر، شركة أخرى، وإعادة الإرسال. |
| QA-06 | CONFIRMED | أحداث GenerateManifestRequested/LoadRequested/FinalizeRequested/AcceptRequested في شاشات الشحن تتحول لطلبات أو تُمرر عبر RootHosts، دون اشتراك عميل Desktop ينفذها عبر HTTP في المشروع الحالي. لا تظهر شاشات كشف/تحميل/تسليم manifest في PhaseOneScreens/CreatePhaseOneControl. | وجود الحدث وHost لا يحقق رحلة تشغيل. مطلوب ربط الأوامر والحالة والإصدار والرسائل وفتح الشاشات في الشجرة. يوجد API للأوامر، لكن وحدة ShippingExecutionApiModule تعرض POST ولا تعرض قراءة trip/manifest لاستعادة العمل بعد إغلاق التطبيق. |
| QA-07 | CONFIRMED | أُعيد عد PhaseOneScreens مستقلًا:140 كودًا فريدًا. CreatePhaseOneControl:138 حالة، وLogin/Main لهما معالجة خاصة. CreatePhaseOneScreen:126 حالة ومنها01.02 يعيد null. CSV المصدر يحصي328 صنف UI، منها163 business_surface و119 form_host. | لا يصح القول328 شاشة مستقلة مكتملة أو140 شاشة متصلة بقاعدة البيانات. العدّ يفصل الكود الملاحي، سطح العمل، المضيف، القالب والمكوّن، ثم يُراجع التنفيذ والاتصال لكل وظيفة. |
| QA-08 | CONFIRMED | بعد توليد كشف A، `ShippingExecutionPersistence.cs:199` يسمح تخصيص B طالما Trip=DRAFT؛ توليد كشف جديد يستبعد تخصيصات A المرتبطة بالسطور، و`TransportErpP2ShippingModel.cs:115` يفرض كشفًا واحدًا للرحلة؛ Finalize يقارن تغطية كل active allocations في586. | لا يمكن ضم B إلى الكشف الحالي بالمسارات الموجودة؛ طلب التخصيص يُقبل ثم يعرقل الإقفال. يُمنع التخصيص بعد الكشف أو يقدم refresh معتمد قبل التحميل. **تصحيح:** ليست رحلة محبوسة نهائيًا؛ UnallocateB يظل مسموحًا إذا لم يرتبط بسطر كشف/LOAD، ثم يمكن إكمال A. |
| QA-09 | CONFIRMED | `ReceiptApiModule.cs:42` يُفوّض configuration بصلاحية accounting.receipts.configure. Execute129 يفعل companyWide فقط لرمز إعداد الأستاذ العام. Allowed150 يسمح هنا مستخدمًا وصلاحية مقيدين بالفرع الحالي، بينما الحفظ72 يستعمل CompanySettings دون BranchId. | مستخدم فرعي لديه configure يستطيع تعديل سياسة مشتركة للشركة. إذا بقيت السياسة شركة تُطلب صلاحية companyWide؛ إذا اعتمدت سياسة فرع يُعدل نطاق التخزين والخدمة والواجهة وفق العقد. شرط الإقفال اختبار مستخدم فرعي وشركة كاملة، مع عدم تغيير سياسات الفروع الأخرى. |

## تصحيحات النطاق الواجبة

1. **SCOPE_CORRECTION:** العكس الإنتاجي لسند القبض في `ReceiptWorkspaceService.cs:178` يبدل بالفعل `Debit = l.Credit, Credit = l.Debit` عند إنشاء القيد العكسي. لا يُعمم QA-02 على هذا المسار، ولا يقال إنه أُصلح الآن وهو كان صحيحًا أصلًا.
2. **SCOPE_CORRECTION:** المزامنة ليست غائبة تمامًا؛ `SyncOperationService` يحتوي canonicalization وبصمة payload وملكية النطاق وقيود إعادة الإرسال. الفجوة المثبتة هي إصدار جلسة محلية ملائمة وتنفيذ رحلة عميل متكاملة؛ API batch يطابور العمليات، ولا يكفي لإثبات تطبيق كل عملية على الكيان التجاري.
3. **SCOPE_CORRECTION:** CSV يحصي أصناف المصدر، وبعضها مستبعد صراحة من Desktop compile: `CoreUI/*.cs;Forms/Templates/**/*.cs` في csproj، لكنه يُربط ضمن SharedVisuals. لا يُعد غائبًا أو يُحسب مرتين باعتباره شاشتين؛ يجب إظهار موضع التجميع الفعلي عند عد ما يُبنى.
4. **SCOPE_CORRECTION:** `direct_navigation=True` في جرد الأصناف لا يعني عدد مفردات شجرة منفردة، لأن Host وUserControl قد يحملان هوية واحدة. يُستخدم140 لعدد مفردات الشجرة، وتُذكر باقي الأرقام كتعداد أصناف.
5. **SCOPE_CORRECTION:** named_controls وتعداد أنواع الأدوات والتبويبات دليل جرد بنيوي، لا إثبات أن جميع حقول المستخدم المطلوبة أو Onyx أو جميع الأزرار صحيحة وظيفيًا.
6. **SCOPE_CORRECTION:** أحدث CSV أعيد فحصه بعد تصحيح FrmTextSetup كمضيف عام؛ المجموع328 =163 سطح أعمال غير فارغ +119 مضيفًا + باقي الفئات. أعداد164/118 كانت وسيطة وسُحبت. مجموع عائلات الأعمال55+45+23+19+10+6+5=163.
7. **SCOPE_CORRECTION:** الأصل المثبت هو SHA أعلاه؛ ادعاءات آخر رفع يوم4 أكتوبر لا تُستخدم لتحديد النسخة الحالية ما دام المصدر المراجع أحدث ومختلفًا. وقت commit ليس برهانًا على وقت push؛ لا يُسمى أحدهما الآخر.

## مراجعة الإصلاحين المحليين بصورة مستقلة

قُرئ git diff للإصلاحين وأُعيد فحص العقود والأصناف والاختبارات. `WaybillItemEntity.Volume` موجود بنوع decimal?؛ التغيير يحافظ على القيمة ولا يغير عقدًا أو مخطط قاعدة. `P1JournalLine` record غير قابل للتحوير؛ `with` مع مصفوفة جديدة يحافظ على بيانات الأصل والبُعد.

عدّل الفريق اختبار PostgreSQL القائم بإدخال Volume50 والتحقق من العنصر المخزن ومن الاستجابة بعد الاعتماد. وعدّل اختبار عكس القيد القائم للتحقق أن صافي الأصل والعكس صفر لكل حساب وأن مبلغ سطر الأصل بقي100/0. سيناريو الاختبار الحالي يحوي حسابين مختلفين، لذا Assert.Single الملحق يتوافق معه.

القرار: **CONFIRMED — التغييران ملائمان بالمراجعة المصدرية ومحدودان؛ UNVERIFIED — البناء وتشغيل الاختبارات**. مر `git diff --check` دون أخطاء whitespace. لا يُستبدل هذا بعبارة tests passed.

## ما لا يجوز اعتماده بعد

- **UNVERIFIED:** قبول Windows/Designer، القياسات الفعلية، DPI، RTL، ظهور الحقول وعدم القص، ودورة ضغط كل زر.
- **UNVERIFIED:** نجاح ربط جميع المشاريع، أو تنفيذ GPS مباشر، أو اكتمال تطبيق عميل/سائق/إدارة. يجب أن يحمل كل ادعاء رحلة قابلة للتشغيل مع مصدر واستجابة واختبار صلاحيات.
- **UNVERIFIED:** مطابقة جميع حقول Onyx وتبويباته ومراجع الصور؛ مجرد العثور على ملفات وصور أو33 فجوة موصوفة في تقرير آخر لا يشكل تحققًا مستقلًا منها.
- **UNVERIFIED:** نجاح قاعدة PostgreSQL و migrations والاختبارات التكاملية في بيئة النشر الحالية.

## ترتيب القبول التالي

أولًا تثبيت المصدر والأرقام وتجنب مضاعفة المضيف والسطح، ثم تشغيل إصلاحي الحجم والعكس في بيئة Windows/PostgreSQL، ثم معالجة replay وخط سند القبض↔تحصيل البوليصة، ثم تسجيل الجهاز وربط شاشات الشحن وواجهات قراءة الحالة. بعد ذلك تُختبر الرحلات بين Desktop وMobile والـAPI وGPS، ثم تُقارن الحقول والتبويبات بالمراجع المعتمدة. تقسيم الفرق حسب تشابه التصميم مفيد للتصميم؛ يجب أن تُسند ملكية التكامل لفرق الرحلات لكيلا يقفل كل فريق شاشته دون الطرف المقابل.

## فحص تقارير التخصصات عند وصولها

قُرئ تقرير المعمارية وتقرير البيانات وتقرير الأكواد وجرد الشاشات. أعيد تحقق النتائج المحورية في هذا التقرير من مصدرها، ولم تُعتمد بقية النتائج تلقائيًا. ثبت تحديد DATA-18 بنطاق التفويض أعلاه، وأُرسل تصحيح CODE-10 إلى مراجع الأكواد والمنسق. المعمارية والبيانات يقران أن33 فجوة المرجع نقل من المنسق وليس فحصًا مستقلاً من الوكيلين؛ بقي هذا التمييز محفوظًا.

إثبات اكتمال التطبيقات المحمولة يُفصل عن وجود csproj: المجلدات الحالية Admin/Customer/Driver تضم ملفات المشروع فقط، وتعمل كـLibrary fallback عند غياب scaffold MAUI. لا يجوز اعتبار طبقات API المشتركة دليلًا على وجود عميل هاتف منفذ.

## مراجعة النسخة المجمعة النهائية والمصمم

قُرئت مقدمة `documentation/reviews/20261009/TransportERP_Deep_Project_Review_AR.md` وحدودها وأعدادها، ثم تقرير المصمم الجديد. **CONFIRMED:** أُعيد عد CSV النهائي:328 تعريفًا؛163 سطح أعمال غير فارغ كلها لها Designer،119 مضيفًا، ومصنع138 رمزًا يقابل136 صنفًا فريدًا =126 business_surface +9 empty_scaffold +UcChangePassword. أُعيد عد control-inventory:11750 صفًا، ولا يعني ذلك11750 حقل أعمال مختلفًا أو اختبارًا ناجحًا. مجموع حزم التصميم163.

**CONFIRMED:** مصنع الشاشات `FrmMain.PhaseOneNavigation.cs:513-515` يفتح UserControls القبض/الصرف/القيد. `FrmMain.ReceiptWorkspace.cs` يتصل بـAPI القبض فعليًا؛ بحث BindCommands لا يجد تشغيلًا للصرف والقيد، بينما `FrmMain.LedgerSettings.cs:13-23` يربط للقيد إعداد إلزام البيان فقط. يفصل ملحق المصمم هذه الحالات بصورة صحيحة؛ وجود ملف Designer أو حدث محلي لا يمثل حفظ مستند. `FrmMain.cs:112-115` يضبط Dock=Fill للقيد ويستعمل ConfigureWorkspace للآخرين؛ لا نتيجة قص أو تداخل أو قبول بصري دون Windows.

**SCOPE_CORRECTION:** الواجهات التسع فارغة من محتوى الأعمال، وليست صفر أدوات: مصمم العميل `FrmScreen_03_01_01.Designer.cs:13-21` يضيف standardAuditMetadata فقط؛ والمصممون الثمانية الآخرون لهم التذييل نفسه. تعليق no controls قديم لا يصف التنفيذ الحالي. شرط `FrmMain.cs:96` الذي يرفض Controls.Count==0 لا يكتشف هذا النوع من نقص محتوى الأعمال؛ يمكن فتح مساحة بلا حقول عمل لأنها تحتوي تذييلًا. يجب استعمال هذه العبارة الدقيقة في الملخص والملاحق.

نتيجة مراجعة التقرير المجمّع: الأعداد والتفريق بين المصدر والتشغيل والإصلاحين والبناء غير المنفذ واضحة، وتصحيح CODE-10 موجود. لا تُعتمد النتائج الأخرى كاختبارات تشغيل بمجرد إدراجها في تقرير موحد.
