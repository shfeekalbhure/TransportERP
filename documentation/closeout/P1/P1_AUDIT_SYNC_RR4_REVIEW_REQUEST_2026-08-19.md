# طلب إعادة المراجعة الرابعة المستقلة — AuditEvent وSyncOperation

**المشروع:** TransportERP  
**المستودع:** `shfeekalbhure/TransportERP`  
**الفرع:** `feature/p1-audit-sync-production-20260819`  
**الالتزام المطلوب فحصه:** `87ab53c66eda8055966edaad2f8d9d368b670d5f`
**الالتزام السابق وقرار RR3:** `a018b7c70a66cce480553ce3e42713a03316987e` — `FAIL`  
**قاعدة الدمج:** لا PR ولا دمج قبل قرار مستقل `PASS` واعتماد المالك.

## التكليف

يُكلّف فريق مراجعة مستقل بإعادة فحص إغلاق CA-RR3-01 وCA-RR3-02 وCA-RR3-03 على الالتزام `87ab53c66eda8055966edaad2f8d9d368b670d5f` دون الاعتماد على الذاكرة أو على نتائج الالتزام السابق. يجب على الفريق مطابقة الملفات والأدلة من Git، وتشغيل الأوامر الموثقة في بيئة PostgreSQL 18.6، وإصدار قرار نهائي واحد فقط: `PASS` أو `FAIL`.

## نطاق الفحص الإلزامي

| البند | ما يجب التحقق منه |
|---|---|
| CA-RR3-01 | تشغيل `dotnet test TransportERP.Tests/TransportERP.Tests.csproj -v normal` بلا فلتر مع `TRANSPORTERP_TEST_CONNSTR`، والتحقق من 41/41 PASS و0 Failed و0 Skipped. فحص أن `PostgreSqlCollection` يمنع سباق Migrations، وأن SmokeTest وSeedScope قابلان لإعادة التشغيل على قاعدة الاختبار المشتركة. |
| CA-RR3-02 | مطابقة branch وrepository وcommit في وثائق RR4 والأدلة. حساب SHA-256 للسجلات ومقارنتها بالـmanifest، والتحقق من أن `a018b7c...` ظاهر فقط كسجل تاريخي لقرار RR3. |
| CA-RR3-03 | تنفيذ Clean ثم Build موحد لمشروع `TransportERP.Tests`، والتحقق من 0 Warning و0 Error، وفحص عدم وجود CS0618 في السجل. يجب مطابقة سجل البناء مع الالتزام نفسه، لا مع نسخة أخرى. |
| عدم التراجع | التأكد من استمرار اختبارات JWT Bearer وAuditEvent API وappend-only Trigger وHash-chain والعزل وConflictCase وRetry Backoff وعدم وجود حالة `RETRIED` مستقلة. |

## الأوامر المرجعية

```bash
cd /home/ubuntu/repo_review/TransportERP
export PATH=/home/ubuntu/.dotnet:$PATH
export DOTNET_ROOT=/home/ubuntu/.dotnet
export TRANSPORTERP_TEST_CONNSTR="Host=127.0.0.1;Port=15432;Database=poc14_pg_test;Username=poc14user;Password=poc14pass"

dotnet clean TransportERP.Tests/TransportERP.Tests.csproj -v quiet

dotnet build TransportERP.Tests/TransportERP.Tests.csproj -v normal 2>&1 | tee artifacts/rr4/BUILD_RR4_REVIEW.log

dotnet test TransportERP.Tests/TransportERP.Tests.csproj -v normal 2>&1 | tee artifacts/rr4/TEST_ALL_DEFAULT_RR4_REVIEW.log
```

## مخرجات الفريق المطلوبة

يجب إصدار تقرير مستقل يذكر الالتزام الكامل، branch، وقت التنفيذ، نسخة .NET، نسخة PostgreSQL، الأوامر الفعلية، النتائج الخام، SHA-256 للأدلة، والحالات التي تم تمريرها أو فشلها. لا يُسمح بإصدار `PASS` اعتمادًا على سجل قديم أو تشغيل تسلسلي غير مطابق للتشغيل الافتراضي المطلوب.

## قرار المراجعة

- القرار الوحيد المسموح: `PASS` أو `FAIL`.
- يصدر `PASS` فقط إذا أُغلقت البنود الثلاثة بالأدلة القابلة لإعادة التشغيل ولم تظهر فشلات أو تحذيرات غير مفسرة.
- يصدر `FAIL` عند أي فشل أو عدم تطابق traceability أو تعارض في نتائج البناء.
- يبقى فتح PR والدمج محظورين حتى صدور `PASS` مستقل موثق وبعد اعتماد المالك.

## تاريخ المراجعة

**الفريق المستقل:** ____________________  
**رئيس الفريق:** ____________________  
**تاريخ التكليف:** 2026-08-19 UTC+3  
**قرار الفريق بعد التنفيذ:** ____________________

## مراجع

[1]: https://github.com/shfeekalbhure/TransportERP/commit/87ab53c66eda8055966edaad2f8d9d368b670d5f "TransportERP — الالتزام المستهدف في RR4"
[2]: https://github.com/shfeekalbhure/TransportERP/commit/a018b7c70a66cce480553ce3e42713a03316987e "TransportERP — الالتزام المفحوص في RR3"
