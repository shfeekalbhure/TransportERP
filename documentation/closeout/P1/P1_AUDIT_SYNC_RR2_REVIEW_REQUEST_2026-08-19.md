# طلب إعادة المراجعة المستقلة الثانية — AuditEvent وSyncOperation

انسخ النص التالي وأرسله إلى فريق المراجعة:

> نطلب إعادة المراجعة المستقلة لخدمتي **AuditEvent** و**SyncOperation** في مشروع **TransportERP**.
>
> تم تنفيذ الإجراءات التصحيحية بعد قرار FAIL السابق، ونُشرت الآن داخل GitHub على الفرع:
>
> `feature/p1-audit-sync-production-20260819`
>
> الالتزام المطلوب فحصه:
>
> `9cdf8c619e9945a9e9045bb7142cdc342591c7f3`
>
> المستودع:
>
> `https://github.com/shfeekalbhure/TransportERP`
>
> يرجى عدم فحص `master` القديم، وعدم الاعتماد على أي نتائج أو ملفات خارج الالتزام أعلاه.
>
> نطاق المراجعة الإلزامي:
>
> 1. التحقق من وجود PostgreSQL trigger يمنع UPDATE وDELETE على `transport_erp.audit_events`، وتنفيذ المحاولتين مباشرة.
> 2. التحقق من AuditEventService وصحة Hash-chain على سجل جديد، مع اختبار الفلاتر وعزل الشركة والفرع.
> 3. التحقق من كيان وجدول `ConflictCase` والعلاقة والفهارس ودورة الحسم.
> 4. التحقق من دورة SyncOperation دون استخدام RETRIED كحالة مستقلة.
> 5. التحقق من Exponential Backoff وRetryCount وNextRetryAt وحد الاستنفاد.
> 6. التحقق من idempotency باستخدام DeviceId وClientOperationId وPayloadHash، بما في ذلك إعادة الإرسال بنفس Hash وبHash مختلف.
> 7. التحقق من device/user/company/branch/permission وعزل المستأجر من خلال سياق الخادم.
> 8. التحقق من المسار `POST /api/v1/sync/operations:batch` ونتيجة كل عملية.
> 9. تشغيل الاختبارات الموجودة داخل الالتزام، ومراجعة السجل `documentation/closeout/P1/evidence/P1_AUDIT_SYNC_TEST_LOG_2026-08-19.txt` مع إعادة التشغيل الفعلي.
> 10. تنفيذ دورة Migration على قاعدة PostgreSQL 18.6: قاعدة فارغة ثم Up، ثم Down، ثم Up مرة أخرى، مع توثيق النتيجة.
>
> ملفات الحوكمة ومصفوفة الإجراءات موجودة داخل الالتزام نفسه:
>
> - `documentation/closeout/P1/P1_AUDIT_SYNC_CORRECTIVE_ACTION_MATRIX_RR2_2026-08-19.md`
> - `documentation/closeout/P1/P1_AUDIT_SYNC_RR2_CORRECTIVE_RESPONSE_2026-08-19.md`
> - `documentation/closeout/P1/P1_AUDIT_SYNC_INDEPENDENT_REVIEW_ASSIGNMENT_2026-08-19.md`
>
> المطلوب إصدار تقرير مستقل جديد يذكر الالتزام المفحوص، الأوامر المنفذة، النتائج، الأدلة، الحالات غير المغطاة، والقرار النهائي الصريح: `PASS` أو `FAIL`.
>
> لا يُفتح PR ولا يُسمح بالدمج قبل صدور قرار `PASS` موثق من فريق المراجعة المستقلة واعتماد المالك.
