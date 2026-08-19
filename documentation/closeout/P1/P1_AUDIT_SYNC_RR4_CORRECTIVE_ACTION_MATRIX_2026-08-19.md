# مصفوفة الإجراءات التصحيحية — إعادة المراجعة الرابعة

**المشروع:** TransportERP  
**النطاق:** AuditEvent وSyncOperation وConflictCase وتهيئة PostgreSQL واختبارات P1  
**الفرع:** `feature/p1-audit-sync-production-20260819`  
**الالتزام الإصلاحي المستهدف في RR4:** [`5c5aa2e76c058f1350896bf98125c710a71596a7`](https://github.com/shfeekalbhure/TransportERP/commit/5c5aa2e76c058f1350896bf98125c710a71596a7)
**الالتزام السابق الذي صدر عليه FAIL الثالث:** `a018b7c70a66cce480553ce3e42713a03316987e`  
**قاعدة الحوكمة:** لا PR ولا دمج قبل `PASS` مستقل موثق واعتماد المالك.

## غرض المصفوفة

تغلق هذه المصفوفة إجراءات CA-RR3-01 وCA-RR3-02 وCA-RR3-03 على مستوى التنفيذ والأدلة، لكنها لا تصدر قرار المراجعة المستقلة ولا تستبدل اعتماد المالك. يجب على فريق RR4 إعادة تشغيل الأدلة على الالتزام المستهدف نفسه وإصدار قرار واحد صريح.

| المعرّف | ملاحظة FAIL الثالث | الإجراء المنفذ في الالتزام المستهدف | معيار الإغلاق | الدليل القابل لإعادة التشغيل | الحالة التنفيذية |
|---|---|---|---|---|---|
| CA-RR3-01 | التشغيل الافتراضي السابق فشل 39/41 بسبب سباق تطبيق Migrations، واختبار Smoke كان غير آمن لإعادة التشغيل بسبب رمز عملة ثابت. | إنشاء مجموعة xUnit باسم `PostgreSql` مع `DisableParallelization = true` وإسناد اختبارات PostgreSQL وHTTP التي تستدعي `MigrateAsync` إليها. تحديث SmokeTest لاستخدام `TRANSPORTERP_TEST_CONNSTR` ثم توليد رمز عملة ثلاثي فريد. جعل SeedScope في SyncOperation مقاومًا لتصادم رموز العملات. | تشغيل `dotnet test` بلا فلتر على قاعدة PostgreSQL 18.6 مشتركة بنتيجة 41/41 PASS و0 فشل. | `TransportERP.Tests/PostgreSqlCollection.cs`، `PostgreSqlPersistenceSmokeTests.cs`، `SyncOperationPersistenceTests.cs`، `artifacts/rr4/TEST_ALL_DEFAULT_RR4_RERUN.log` | IMPLEMENTED — PENDING_INDEPENDENT_CONFIRMATION |
| CA-RR3-02 | وثائق وأدلة RR3 لم تكن موحدة على الالتزام المفحوص، مع خلط بين الالتزام التاريخي ومسار الإصلاح. | تثبيت الالتزام الإصلاحي الكامل في وثائق RR4، وإضافة manifest موحد يذكر المستودع والفرع والالتزام وشجرة العمل والأداة وSHA-256 لكل سجل. الإشارات إلى `a018b7c...` تبقى موسومة كسجل تاريخي لقرار FAIL الثالث ولا تمثل هدف RR4. | كل وثائق RR4 وطلب المراجعة وطلب اعتماد المالك تشير إلى `5c5aa2e...` نفسه، مع قابلية مطابقة الأدلة عبر SHA-256. | `P1_AUDIT_SYNC_RR4_EVIDENCE_MANIFEST_2026-08-19.txt` وجميع ملفات RR4 في `documentation/closeout/P1/` | IMPLEMENTED — PENDING_INDEPENDENT_CONFIRMATION |
| CA-RR3-03 | إعادة البناء السابقة سجلت 11 تحذير CS0618 من `HasCheckConstraint`، بينما artifact آخر أعلن 0 تحذيرات. | نقل جميع قيود EF Core المتأثرة إلى صيغة `ToTable(t => t.HasCheckConstraint(...))` الحديثة، مع الحفاظ على أسماء القيود وتعابير SQL، ثم تنفيذ Clean/Build موحد. | البناء النظيف لمشروع الاختبارات ومراجعه يعلن 0 Warning و0 Error على الالتزام المستهدف. | `TransportErpDbContext.cs`، `artifacts/rr4/BUILD_AFTER_FIX_RR4.log`، `artifacts/rr4/CLEAN_AFTER_FIX_RR4.log` | IMPLEMENTED — PENDING_INDEPENDENT_CONFIRMATION |

## نتائج التنفيذ المعلنة قبل RR4

| الفحص | النتيجة المسجلة |
|---|---:|
| `dotnet test TransportERP.Tests/TransportERP.Tests.csproj -v normal` بلا فلتر | 41/41 PASS، 0 Failed، 0 Skipped |
| `dotnet build TransportERP.Tests/TransportERP.Tests.csproj -v normal` بعد Clean | 0 Warning، 0 Error |
| PostgreSQL | PostgreSQL 18.6 عبر `127.0.0.1:15432`, قاعدة `poc14_pg_test` |
| الالتزام المفحوص | `5c5aa2e76c058f1350896bf98125c710a71596a7` |

## حدود القرار

لا تعتبر عبارة `IMPLEMENTED` قرار `PASS`. يظل فتح PR أو الدمج محظورًا حتى يعتمد المالك الحزمة ويعيد فريق مستقل تشغيل الأدلة على الالتزام نفسه ويصدر `PASS` موثقًا. لا يغير هذا الملف قرار RR3 التاريخي `FAIL`.

## مراجع

[1]: https://github.com/shfeekalbhure/TransportERP/tree/5c5aa2e76c058f1350896bf98125c710a71596a7 "TransportERP — الالتزام الإصلاحي المستهدف في RR4"
[2]: https://github.com/shfeekalbhure/TransportERP/commit/a018b7c70a66cce480553ce3e42713a03316987e "TransportERP — الالتزام المفحوص في RR3"
