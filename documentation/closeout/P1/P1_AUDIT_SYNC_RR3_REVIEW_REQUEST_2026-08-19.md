# طلب إعادة المراجعة المستقلة الثالثة — AuditEvent وSyncOperation

نطلب من فريق المراجعة المستقلة تنفيذ إعادة المراجعة الثالثة الكاملة لخدمتي `AuditEvent` و`SyncOperation` في مشروع `TransportERP`، بناءً على الالتزام المنشور التالي فقط:

```text
Repository: https://github.com/shfeekalbhure/TransportERP
Branch: feature/p1-audit-sync-production-20260819
Commit: a018b7c70a66cce480553ce3e42713a03316987e
```

**الالتزام الإصلاحي المستهدف في RR4:** `fe9770bcfcffa84f05dca7a38027566cd210f5f1`

## المرجع الحاكم

يجب مقارنة كل بند من تقرير المراجعة الثانية ذي القرار `FAIL` مع مصفوفة الإجراءات التصحيحية RR3، وعدم الاكتفاء بقراءة التقرير أو سجلات خارج Git. يجب تثبيت الالتزام المفحوص قبل أي استنتاج.

## بنود التحقق الإلزامية

1. تشغيل `dotnet restore` و`dotnet build` و`dotnet test` للمشروع المستهدف `TransportERP.Tests` باستخدام .NET 10، مع تسجيل النتائج.
2. تشغيل الاختبارات على PostgreSQL 18.6 باستخدام متغير `TRANSPORTERP_TEST_CONNSTR`.
3. التحقق من JWT Bearer فعلياً: رفض الطلب غير الموثق، رفض التوكن غير الصحيح أو غير المطابق، وقبول claims صحيحة.
4. التحقق من `POST /api/v1/sync/operations:batch` مع claims الشركة والفرع والمستخدم والصلاحية والجهاز، واختبار idempotency والعزل.
5. التحقق من `GET /api/v1/audit/events`، paging، filter، permission `AUDIT_READ`، عزل الشركة/الفرع، وعدم تسريب payload في حدث قراءة التدقيق.
6. التحقق من canonical Hash-chain لكل stream مستقل بحسب CompanyId/BranchId/DeviceId، واختبار التوازي على نفس stream، وكشف التلاعب.
7. التحقق من append-only على مستوى PostgreSQL بمحاولة UPDATE وDELETE بحساب غير مالك جدول `transport_erp.audit_events`.
8. التحقق من `ConflictCase` كجدول وكيان مستقل مرتبط بـ`SyncOperation`.
9. التحقق من Retry Backoff والحالات المسموح تكرارها فقط، ومنع retry صراحةً لأخطاء `HASH_MISMATCH` و`IDEMPOTENCY_HASH_MISMATCH` و`SCOPE_DENIED` وأخطاء الصلاحيات، وعدم استخدام `RETRIED` كحالة مستقلة.
10. تنفيذ دورة Migration مستقلة `Up → Down → Up` وإثبات عدم وجود تغييرات معلقة.
11. تشغيل `dotnet list package --vulnerable --include-transitive` للمشروعين API وInfrastructure والتحقق من نتيجة RR2-C-07.
12. مراجعة سجلات الأدلة الموجودة في `artifacts/rr3/` وعدم اعتبار نجاح الاختبار وحده بديلاً عن التحقق من الكود والعقد.

## الأدلة الموجودة داخل الالتزام

- `artifacts/rr3/TEST_ALL_AFTER_RR3_SECURITY_UPGRADE.log`
- `artifacts/rr3/BUILD_TESTS_AFTER_SECURITY_UPGRADE.log`
- `artifacts/rr3/NUGET_VULNERABILITY_AUDIT_API_AFTER.txt`
- `artifacts/rr3/NUGET_VULNERABILITY_AUDIT_INFRA_AFTER.txt`
- `artifacts/rr3/SECURITY_ADVISORY_SOURCES.md`
- `documentation/closeout/P1/P1_AUDIT_SYNC_RR3_CORRECTIVE_ACTION_MATRIX_2026-08-19.md`
- `documentation/closeout/P1/P1_AUDIT_SYNC_RR3_OWNER_SIGNOFF_REQUEST_2026-08-19.md`

## القرار المطلوب

أصدروا تقريراً مستقلاً بقرار نهائي واحد فقط: `PASS` أو `FAIL`. يجب أن يتضمن التقرير الالتزام المفحوص، الأوامر، النتائج، الأدلة، الحالات غير المغطاة، وأي ملاحظة حرجة أو غير حرجة.

لا يُفتح PR ولا يُسمح بالدمج قبل صدور `PASS` مستقل موثق وبعد اعتماد المالك. إذا صدر `FAIL`، يجب تحديث مصفوفة الإجراءات وإعادة المراجعة قبل أي انتقال إلى الدمج.
