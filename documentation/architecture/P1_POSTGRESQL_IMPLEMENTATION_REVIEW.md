# مراجعة تنفيذ P1 على PostgreSQL

**الحالة:** PASS_WITH_SECURITY_WARNING

## النطاق

تغطي هذه المراجعة قرار PostgreSQL 18.6، وPhysical Schema المشتق من عقود P1، وEF Core Migration الأولية، وخدمة دورة حياة سندات القبض والصرف، واختبارات الوحدة والتكامل.

## الأدلة التنفيذية

| الفحص | النتيجة |
|---|---:|
| بناء TransportERP.Infrastructure | ناجح |
| اختبارات TransportERP.Tests | 29/29 ناجحة |
| اختبار PostgreSQL round-trip | ناجح |
| تطبيق Migration من قاعدة فارغة | ناجح |
| إعادة تطبيق Migration | لا تغييرات؛ القاعدة محدثة |
| جداول schema `transport_erp` | 21 جدولًا |
| قيود PostgreSQL المسجلة | 296 قيدًا |
| Migration history | صف واحد: `20260819032151_P1InitialPostgreSql` |
| `git diff --check` | ناجح |

## الوظائف المنفذة في طبقة P1 الإنتاجية

تمت إضافة كيانات P1 وDbContext وتهيئة PostgreSQL ومصنع وقت التصميم وخدمة `VoucherLifecycleService`. الخدمة تنفذ إنشاء سندي القبض والصرف، وidempotency بالمرجع الخارجي، وانتقالات `DRAFT -> APPROVED -> POSTED`، والإلغاء وفق القيود، ومنع إلغاء السند المرحّل. الاختبارات تستخدم EF Core InMemory للوحدة وتستخدم PostgreSQL فعليًا في اختبار تكامل اختياري.

## الملاحظة الأمنية

ظهر تحذير `NU1903` متكرر متعلق بالحزمة `System.Security.Cryptography.Xml` الإصدار 9.0.0 الموجودة ضمن اعتماديات المشروع. لم يمنع التحذير البناء أو الاختبارات، لكنه يجب أن يعالج في مسار أمني مستقل قبل النشر الإنتاجي. لا يجوز اعتبار هذا التحذير مغلقًا اعتمادًا على نجاح الاختبارات.

## الحدود المتبقية

هذه ليست نهاية تنفيذ P1 بالكامل. لم تُنفذ بعد طبقات API وواجهة المستخدم، وخدمة التدقيق append-only/hash-chain داخل PostgreSQL، وخدمة SyncOperation الإنتاجية، والقيود المحاسبية التشغيلية الكاملة، واختبارات الصلاحيات وRTL وOffline/Online على التطبيق. كما أن قاعدة الاختبار المحلية ليست قاعدة إنتاج.

**قرار المراجعة:** يمكن نقل التغييرات إلى PR للمراجعة، مع إبقاء حالة المشروع `P1_PRODUCTION_FOUNDATION_READY_FOR_REVIEW`، وليس `PRODUCTION_READY`.

## إضافة الحالة الحالية — 2026-08-25

النتائج أعلاه سجل تاريخي لمرحلة التنفيذ الأولية ولا تصف وحدها حالة شجرة العمل الحالية. نفذت حزمة `P0-CI` الحالية معالجة ساكنة للمخاطر الآتية: إزالة 13 مسار نجاح صامت من اختبارات PostgreSQL/HTTP عبر بوابة اتصال fail-closed مشتركة، وإضافة workflow عام لكل PR وpush إلى `master` يشغل PostgreSQL 18 وفحوص العقود والبناء وEF migrations وجميع الاختبارات، وتحويل workflows المرحلية من `contents: write` إلى `contents: read` مع منع توليد migrations أو commit/push من CI.

نجح المدققان `validate_p0_p1.py` و`validate_p2_c01_contracts.py` بنتيجة `ERROR_COUNT=0`. وثبت التنفيذ التشغيلي للحزمة على remote SHA `7626f0f8f8172ecd7286a6040436349b55c4de70` في [GitHub Actions run 32806769647](https://github.com/shfeekalbhure/TransportERP/actions/runs/32806769647): نجحت وظيفة `Desktop RTL contract surface`، ونجحت وظيفة `Core + PostgreSQL + HTTP` بما فيها مطابقة EF migrations وتطبيقها وتشغيل مجموعة الاختبارات fail-closed كاملة بنتيجة `124/124 PASS` و`0 failed` و`0 skipped`. لذلك أصبحت حالة هذه الحزمة `RUNTIME_VERIFIED_FOR_SHA`.

لا تعني هذه النتيجة `PRODUCTION_READY`: ما تزال حماية `master` وrequired checks بحاجة إلى تفعيل، ولا تعالج الحزمة Tenant/RBAC أو تسجيل الأجهزة، ولا تغلق عقد Sync أو بوابة G3.
