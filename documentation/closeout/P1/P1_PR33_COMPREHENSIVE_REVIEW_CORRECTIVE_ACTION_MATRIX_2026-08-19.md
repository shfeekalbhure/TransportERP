# مصفوفة الإجراءات التصحيحية — مراجعة PR #33

| المعرّف | الملاحظة | التصنيف | الإجراء الإلزامي | دليل الإغلاق المطلوب | الحالة |
|---|---|---|---|---|---|
| PR33-SEC-01 | fallback يحتوي على بيانات اعتماد اختبار ثابتة داخل `TransportErpDbContextFactory.cs` | حرج — أمن | إزالة بيانات الاعتماد الثابتة من الشيفرة أو نقلها إلى إعداد اختبار صريح ومعزول، مع منع استخدامها في بيئة الإنتاج | diff يثبت إزالة القيمة، ثم Clean/Build/Test وسجل فحص ساكن جديد بلا بيانات اعتماد ثابتة | مفتوح |
| PR33-TR-02 | نص PR يثبت PASS على `5c5aa2e` بينما head الحالي المفحوص هو `77d2a806...` | حرج — traceability | تحديث نص PR ووثائق الحزمة للفصل بين `PR head reviewed` و`source code baseline reviewed`، ثم تثبيت head نهائي وإعادة تشغيل الأدلة عليه | PR body وreport وmanifest تحمل head النهائي نفسه، مع SHA-256 جديد للسجلات | مفتوح |

## قاعدة الإغلاق

لا يُفتح طلب دمج ولا يُنفذ الدمج قبل إغلاق الملاحظتين وإصدار قرار مستقل واحد `PASS` على head النهائي. لا تُقبل صيغة `PASS_WITH_NOTES`.

## المراجع

- `P1_COMPREHENSIVE_INDEPENDENT_REVIEW_ASSIGNMENT_PR33_2026-08-19.md`
- `P1_PR33_COMPREHENSIVE_INDEPENDENT_REVIEW_REPORT_2026-08-19.md`
- `PR33_COMPREHENSIVE_PR_STATE.log`
- `PR33_BODY_AND_STATUS.log`
- `PR33_STATIC_SECURITY_SCOPE.log`
