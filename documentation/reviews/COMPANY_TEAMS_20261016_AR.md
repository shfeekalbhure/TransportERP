# خطة الفرق المتخصصة — شاشة الشركات (SCR-SET-004)

تاريخ الإنشاء: 2026-10-16 (Asia/Aden).
المستودع: `shfeekalbhure/TransportERP`.
فرع التكامل الوحيد: `design/setup-stage1-20261009`.
الأساس المثبت لتفرّع الفرق: `863bb6927a27e143b91b9fb6aac19b0cc4fdb869`.
المهمة الجامعة: [#77](https://github.com/shfeekalbhure/TransportERP/issues/77).

## قاعدة لا خلاف عليها
شاشة الشركات المعتمدة هي `UcCompanyManagement`، وتصميمها يُدار في `UcCompanyManagement.Designer.cs` عن طريق Visual Studio Designer فقط. الواجهة الداخلية هي UserControl داخل تبويبات `FrmMain`. يمكن إبقاء `FrmCompanyManagement` كحاوية توافق تستضيف نفس UserControl دون إعادة إنشاء حقول أو شاشة إضافية. لا تحذف `UcCompanyData` حتى إثبات انعدام استخداماته، ولا تنقل التصميم إلى `.cs`.

## فرق العمل ومسؤوليات الملفات
| المسار | التكليف | فرع الفريق | نطاق المسؤولية | حالة تنفيذ الإصلاحات |
|---|---|---|---|---|
| 01 — المصمم/WinForms | [#78](https://github.com/shfeekalbhure/TransportERP/issues/78) | `review/company-01-designer-20261016` | `UcCompanyManagement.cs` و`UcCompanyManagement.Designer.cs` و`.resx` والتذييل المرتبط | لم يُعتمد بعد |
| 02 — التنقل ومنع التكرار | [#79](https://github.com/shfeekalbhure/TransportERP/issues/79) | `review/company-02-navigation-20261016` | `FrmMain*` و`FrmCompanyManagement*` وفحص إحالات legacy | لم يُعتمد بعد |
| 03 — API والبيانات والصلاحيات | [#80](https://github.com/shfeekalbhure/TransportERP/issues/80) | `review/company-03-api-security-20261016` | API/Application/Infrastructure/Contracts واختبارات طبقات البيانات | لم يُعتمد بعد |
| 04 — اختبارات CI | [#81](https://github.com/shfeekalbhure/TransportERP/issues/81) | `review/company-04-ci-quality-20261016` | CI وProgram.cs وCodeRepairChecks والاختبارات | لم يُعتمد بعد |
| 05 — مراجعة مستقلة | [#82](https://github.com/shfeekalbhure/TransportERP/issues/82) | لا فرع تعديل — مراجعة قراءة فقط | PRs وDiffs والدليل على SHA نهائي | لم تُنفذ |

**تنبيه حوكمة:** هذه مهام تنسيق وفروع GitHub، وليست تعيينًا مؤكدًا لمطورين بشريين أو تشغيلًا تلقائيًا لوكلاء Codex. يبدأ التنفيذ الفعلي عندما يتولى مطور/وكيل مخوّل مهمة محددة ويقدم PR مع أدلة.

## عيوب/فجوات مثبتة قبل التقسيم
- `02.01.01` مرتبط أصلًا بـ`UcCompanyManagement` داخل Workspace و`FrmCompanyManagement` بوصفه نافذة مستقلة؛ لا تُنشأ شاشة ثالثة.
- `UcCompanyManagement.cs` يستدعي `OnyxPhaseOneProperties.Apply(this)` و`ScreenRootProperties.Attach(this)` فوق خصائص المصمم، ويغير عنوان `tabCompaniesSection` أثناء التشغيل. الفريق 01 يحدد أولًا كيفية إزالة تعارض هذه التعديلات دون تعطيل الوظيفة.
- `UcCompanyManagement.Designer.cs` يحتوي تبويبين `tabBasicData` و`tabStatus` دون مدخلات مباشرة؛ لا تُخترع حقول ولا يُحذف تبويب بلا مرجع معتمد.
- `UcCompanyData` تصميم legacy مختلف؛ فحص التكرار المرجعي مطلوب قبل أي حذف أو إعادة تسمية.
- GitHub Actions على `863bb6927a27e143b91b9fb6aac19b0cc4fdb869`: [run 38822289484](https://github.com/shfeekalbhure/TransportERP/actions/runs/38822289484) = **FAIL**: بناء Desktop فشل `CS0103 ApplicationConfiguration` عند Library output؛ الاختبارات `143 PASS / 3 FAIL / 146 TOTAL`. هذه مشاكل مانعة للإغلاق حتى إثبات إصلاحها.

## قواعد دمج حازمة
1. كل فريق يغيّر ملفات ملكيته فقط ويضع PR إلى `design/setup-stage1-20261009`؛ لا دفع مباشر على `master` ولا إعادة هيكلة واسعة أو حذف ملفات.
2. لا تعديل متوازٍ لنفس الملف. أي اكتشاف خارج نطاق الملكية يدوَّن على مهمة الفريق المختص بدلًا من إصلاح صامت.
3. لا يُدمج PR قبل إثبات Build/Tests ذات الصلة؛ تعارض الفرع أو Failed CI هو HOLD صريح، لا نجاح ضمني.
4. يُعاد تشغيل Windows Desktop CI والاختبارات المساندة على **رأس التكامل النهائي**، ثم تنفّذ المراجعة المستقلة #82 على نفس SHA بلا self-pass.
5. بعد النجاح والمراجعة فقط تُسحب نسخة الفرع المحدّث عبر Git Pull في Visual Studio.

## تسلسل مقترح للعمل ملفًا ملفًا
`UcCompanyManagement.cs` → `UcCompanyManagement.Designer.cs` مع تصميم Visual Studio → `UcCompanyManagement.resx` → `FrmCompanyManagement.cs/Designer.cs` → `FrmMain.PhaseOneNavigation.cs` → واجهات الخدمات/البيانات → اختبارات CI → مراجعة مستقلة.
