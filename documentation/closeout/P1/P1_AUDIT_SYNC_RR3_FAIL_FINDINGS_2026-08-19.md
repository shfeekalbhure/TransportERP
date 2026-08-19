# خلاصة FAIL الثالث — AuditEvent وSyncOperation — 2026-08-19

## الالتزام المفحوص

- Repository: https://github.com/shfeekalbhure/TransportERP
- Branch declared: `feature/p1-audit-sync-production-20260819`
- Commit inspected: `a018b7c70a66cce480553ce3e42713a03316987e`
- Decision: `FAIL`

## ما ثبت نجاحه

ثبتت المراجعة الثالثة JWT Bearer، مساري batch وقراءة AuditEvent، idempotency، العزل، canonical Hash-chain على مستوى الخدمة، ConflictCase، Retry Backoff وتصنيف أخطاء Hash/Permission، append-only Trigger على PostgreSQL 18.6، ودورة Migration التنفيذية Up → Down → Up. نجح التشغيل التسلسلي 41/41، وفحص NuGet لم يظهر حزمًا ضعيفة حسب التقرير.

## الملاحظات الحرجة

### CA-RR3-01 — فشل التشغيل الافتراضي الكامل

التشغيل الافتراضي سجل 39/41، مع فشلين:

1. `PostgreSqlPersistenceSmokeTests.Migration_and_receipt_round_trip_work_on_postgresql`: `42703 column "ConflictCaseId" does not exist`.
2. `SyncOperationPersistenceTests.Enqueue_enforces_device_permission_and_company_branch_scope`: `42701 column "Outcome" of relation "audit_events" already exists`.

شرط الإغلاق: جعل suite آمنًا للتوازي أو عزل الاختبارات بقواعد مستقلة موثقًا، ثم تشغيل `dotnet test` الافتراضي حتى 0 فشل.

### CA-RR3-02 — عدم اتساق traceability

الطلب/سجل اعتماد المالك داخل الالتزام أشار إلى الالتزام السابق `05f0b64...` بدل الالتزام المفحوص `a018b7c...`. كما أشارت أدلة Build/NuGet إلى مسار repo مختلف عن نسخة المراجعة. يجب تحديث كل السجلات والأدلة إلى الالتزام الحالي أو إرفاق SHA-256 يثبت مطابقتها.

شرط الإغلاق: artifact manifest يذكر commit وbranch وrepository وworking tree وtoolchain وSHA-256 لكل سجل، وجميع وثائق signoff/review تشير إلى commit واحد.

### CA-RR3-03 — تعارض تحذيرات البناء

artifact أعلن `0 Warning(s)`, بينما إعادة البناء المباشر على الالتزام المفحوص أعلن `11 Warning(s)` من `CS0618` بسبب `HasCheckConstraint` المهجورة.

شرط الإغلاق: إعادة بناء الالتزام نفسه بأمر موحد، حفظ stdout/stderr و`git rev-parse HEAD` و`dotnet --info`، إزالة التحذيرات أو توثيق سببها ومعيار قبولها، وعدم إرفاق artifact من نسخة أخرى.

## ملاحظات غير حرجة يجب حسمها أو توثيقها

- إعداد Data Protection في Development سجل مفاتيح غير مشفرة at rest؛ يلزم توضيح أن ذلك development-only وسياسة الإنتاج.
- canonical Hash-chain مفروض في الخدمة والاختبارات، ولا يظهر قيد DB للتحقق من hash عند الإدخال المباشر؛ يجب ربط القرار بالعقد بدل التخمين.
- استعلام `__EFMigrationsHistory` استُخدم مع schema غير مطابق لمسار design-time؛ يجب توحيد runtime/design-time schema وإثباته.

## بوابة الإغلاق

لا PR ولا دمج قبل: 0 فشل في التشغيل الافتراضي، traceability موحد ومثبت بالـSHA، وحسم تعارض التحذيرات على نفس الالتزام. القرار السابق يظل FAIL حتى تصدر مراجعة مستقلة لاحقة PASS.
