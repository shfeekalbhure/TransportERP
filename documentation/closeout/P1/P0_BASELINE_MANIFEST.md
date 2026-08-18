# TransportERP — P0 Baseline Manifest

**Baseline_ID:** `TRANSPORTERP-BASELINE-2026-08-19-FC607FC`  
**Release_ID:** `P1-PLATFORM-SETTINGS-ACCOUNTING-2026-08`  
**Repository:** `shfeekalbhure/TransportERP`  
**Branch at baseline:** `master`  
**Baseline Commit:** `fc607fc6e735f7b554f80dd9ad5d668bf50659c3`  
**Execution Branch:** `feature/p0-p1-closeout`  
**Default Branch:** `master`  
**Baseline Status:** `SEALED_FOR_REVIEW_ONLY`  
**Implementation Authorization:** `NO`

## 1. قاعدة المصدر

هذا الملف يثبت النسخة التي ستستخدم لبناء سجلات الإغلاق الأولية. لا يجوز اعتبار أي ملف غير متتبع في Commit المذكور مصدرًا حاكمًا إلا بعد إضافته صراحة إلى Manifest جديد ومراجعته.

## 2. الحالة المثبتة

يثبت Commit الأساس وجود حل TransportERP متعدد المشاريع في بنية تأسيسية. لا يثبت هذا Commit وجود Controllers أو كيانات نطاق أو Migrations أو اختبارات نطاق منفذة؛ لذلك تبدأ P1 كمرحلة تأسيس عقود وتوثيق واختبارات، وليس كإقرار بأن النظام جاهز.

## 3. قواعد التغيير

أي تعديل لاحق يجب أن يتم على فرع تنفيذ مستقل، ويرتبط بـ`Change_ID` وسبب التغيير ومراجعه. لا يُعاد استخدام كود من مستودع آخر دون سجل Mapping وقرار ملكية واختبارات قبول.

## 4. عناصر التحقق

| العنصر | القيمة/المسؤولية |
|---|---|
| Commit SHA | `fc607fc6e735f7b554f80dd9ad5d668bf50659c3` |
| عدد الملفات المتتبعة عند الأساس | 13 |
| نطاق الإصدار | المنصة الأساسية، الهوية، المؤسسات والفروع، الإعدادات، الحسابات |
| النطاق المؤجل | الشحن، التوصيل، النقل، المشاوير، السفر، الأسطول، الصيانة، الموارد البشرية، الجمارك والتكاملات غير الداخلة في P1 |
| مالك اعتماد النطاق | مالك المشروع |
| حالة G0 | `OPEN_PENDING_SHA256_AND_OWNER_SEAL` |
| حالة G1 | `OPEN_PENDING_SCOPE_ACCEPTANCE` |

## 5. قرار عدم التخمين

لا يجوز إنشاء جدول أو Route أو شاشة أو حقل أو Permission في P1 اعتمادًا على Benchmark خارجي فقط. يستخدم البحث الخارجي كمرجع مقارنة، بينما مصدر القرار هو متطلب المشروع أو قرار مالك موثق أو عقد W1/W2/W3 مع Evidence.
