# سجل أدلة W1 — إصدار P1

## الغرض

يوثق هذا الملف الأدلة التي ظهرت في مستودع `AlTayerERP` ويمكن استخدامها كمرجع أولي لعقود W1 في إصدار `P1-PLATFORM-SETTINGS-ACCOUNTING-2026-08`. لا يُعد وجود الكيان أو جدول `DbSet` في المستودع المرجعي تنفيذًا داخل `TransportERP`، ولا يغلق عقد W1، ولا يجيز توليد DDL أو Migrations أو API تلقائيًا.

## مصادر القراءة

| المصدر | القيمة |
|---|---|
| مستودع الهدف | `shfeekalbhure/TransportERP` |
| خط أساس TransportERP | `301a5bf927840dac2547a2f8b48f037af8d1b3e8` بعد دمج PR #20 |
| المستودع المرجعي | `shfeekalbhure/AlTayerERP` |
| Commit المستودع المرجعي | يُثبت في سجل التسليم عند كل مراجعة مصدر |
| المصدر الحاكم للـDbSet | `AlTayerERP.Infrastructure/Data/AppDbContext.cs` |
| المصدر الحاكم للكيانات | `AlTayerERP.Core/Entities/*.cs` و`AlTayerERP.Core/Entities/Accounting/*.cs` |

## قواعد التعامل

تُصنف الأدلة إلى `EXACT_REFERENCE` و`PARTIAL_REFERENCE` و`COMPOSITE_REFERENCE` و`AMBIGUOUS_REFERENCE` و`SCOPE_UNCONFIRMED` و`NOT_FOUND`. لا تتحول أي فئة إلى `CLOSED` إلا بعد إنشاء عقد TransportERP مستقل يحدد الأعمدة والمفاتيح والعلاقات والنطاق والتزامن والتدقيق ودورة الحياة والترحيل والاختبارات، ثم موافقة مالك المنتج ومراجع مستقل.

## نتيجة أولية

يحتوي `AppDbContext` المرجعي على كيانات إدارية وهوية وإعدادات ومحاسبة، منها الشركات والفروع والمستخدمون والأدوار والصلاحيات والإعدادات والفترات المالية والعملات ودليل الحسابات ومراكز التكلفة والصناديق والبنوك والسندات والقيود والتدقيق. هذه القائمة تدعم قرار **إعادة استخدام محتملة** لبعض عقود P1، لكنها لا تثبت توافقها مع متطلبات TransportERP أو مع Offline/Online أو تعدد مشاريع النقل.

العنصر `SyncOperation` غير موجود في المصدر المرجعي المفحوص، ولذلك يبقى عقد المزامنة مفتوحًا ولا يجوز تمثيله بجدول افتراضي. كما أن ربط إعدادات المؤسسة والفرع يحتاج تثبيت نطاقه من متطلبات TransportERP وليس من اسم `SystemSetting` فقط.

## الحالة

`W1_STATUS = OPEN`

`REUSE_APPROVAL = PENDING_OWNER_APPROVAL`

`DDL_AUTHORIZATION = NO`

`NEXT_REQUIRED_EVIDENCE =` عقود TransportERP المعتمدة، DDL/Migration أو قرار صريح بعدم الحاجة، اختبارات W1، وقرار إعادة الاستخدام لكل صف في `W1_SOURCE_EVIDENCE_REGISTER.csv`.
