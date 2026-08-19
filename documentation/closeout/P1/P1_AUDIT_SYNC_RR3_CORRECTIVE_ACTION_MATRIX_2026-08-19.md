# مصفوفة الإجراءات التصحيحية — إعادة المراجعة الثالثة

**المشروع:** TransportERP  
**نطاق المراجعة:** AuditEvent وSyncOperation  
**الالتزام المفحوص في RR3:** `a018b7c70a66cce480553ce3e42713a03316987e`

**الالتزام الإصلاحي المستهدف في RR4:** `fe9770bcfcffa84f05dca7a38027566cd210f5f1`
**قرار المراجعة الثانية:** `FAIL`  
**قاعدة الحوكمة:** لا PR ولا دمج قبل `PASS` مستقل موثق واعتماد المالك.

## الحالة التنفيذية

أُغلقت برمجياً البنود RR2-C-01 إلى RR2-C-05، وأُغلِق RR2-C-07 فنياً بعد إزالة تنبيهات NuGet من المشاريع المستهدفة. يبقى RR2-C-06 **معلقاً بانتظار اعتماد مالك صريح ومحدث** للحزمة الحالية؛ لا يجوز اعتبار هذا البند مغلقاً بالاستنتاج.

| المعرّف | الملاحظة الحرجة في RR2 | الإجراء المنفذ | شرط الإغلاق | الدليل القابل لإعادة التشغيل | الحالة |
|---|---|---|---|---|---|
| RR2-C-01 | غياب مواصفة canonical مستقلة وعدم تقييد تحقق السلسلة بالشركة/stream | تثبيت canonical في `AuditEventService` وفق الصيغة المعتمدة، تعريف stream من CompanyId/BranchId/DeviceId، والتحقق المنفصل لكل stream مع حماية scope | كشف تعديل Hash/PreviousHash، وعزل stream/company/branch، واختبار توازي على نفس stream | `AuditEventService.cs`، `AuditEventPersistenceTests.cs`، سجل PostgreSQL | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |
| RR2-C-02 | غياب API قراءة AuditEvent وpaging/scope/permission وتسجيل القراءة | إضافة `GET /api/v1/audit/events` مع paging، company/branch filters، permission `AUDIT_READ`، وعدم إعادة payload في سجل قراءة الطلب | رفض غير المصرح، قبول المصرح، عزل الشركة، استجابة paged، وتسجيل عملية القراءة | `Program.cs`، `AuditEventService.cs`، `ApiAuthenticationAndAuditTests.cs` | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |
| RR2-C-03 | عدم استبعاد أخطاء Hash والصلاحية والنطاق من Retry وغياب max attempts/clock حتمي | إضافة تصنيف أخطاء صريح، منع retry لـ`HASH_MISMATCH` و`IDEMPOTENCY_HASH_MISMATCH` و`SCOPE_DENIED` وأخطاء الصلاحيات، وإيقاف العملية عند `RETRY_EXHAUSTED` | اختبار retry للحالة المؤقتة، رفض retry للحالات غير القابلة، max attempts، وعدم وجود RETRIED | `SyncOperationService.cs`، `SyncOperationPersistenceTests.cs` | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |
| RR2-C-04 | `AddAuthentication()` بلا موفر فعلي | إضافة `JwtBearer` مع issuer/audience/signing key من configuration، claim mapping، وpolicy `Authenticated` | HTTP يرفض غياب/فساد/issuer غير صحيح ويقبل token صالحاً ويطبق scope/permission | `Program.cs`، `ApiAuthenticationAndAuditTests.cs` | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |
| RR2-C-05 | غياب HTTP integration والتوازي وSENDING/timeout وعدم تسريب payload | إضافة WebApplicationFactory، اختبارات HTTP للbatch والرفض والتكرار والعزل، واختبارات توازي AuditEvent وidempotency | اختبارات HTTP قابلة لإعادة التشغيل، concurrent duplicate، hash mismatch/conflict، وعدم تسريب payload | `ApiAuthenticationAndAuditTests.cs`، `AuditEventPersistenceTests.cs`، `SyncOperationPersistenceTests.cs` | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |
| RR2-C-06 | W1/W2/W3 بحالة READY_FOR_OWNER_ACCEPTANCE | إعداد طلب اعتماد مالك محدث يحدد نطاق الحزمة الحالية والقيود وعدم الدمج | توقيع/اعتماد مالك صريح محفوظ داخل Git قبل طلب PASS | `P1_AUDIT_SYNC_RR3_OWNER_SIGNOFF_REQUEST_2026-08-19.md` | PENDING_OWNER_SIGNOFF |
| RR2-C-07 | تحذيرات NU1903 عالية الخطورة | ترقية `Microsoft.OpenApi` إلى `2.7.5` وترقية/تثبيت `System.Security.Cryptography.Xml` إلى `10.0.10`، وإعادة restore/build/audit | `dotnet list package --vulnerable --include-transitive` بلا حزم ضعيفة للمشاريع المستهدفة، وبناء 0 تحذير/0 خطأ | `NUGET_VULNERABILITY_AUDIT_*_AFTER.txt`، سجلات restore/build، ملفات csproj | CLOSED_PENDING_INDEPENDENT_CONFIRMATION |

## نتائج الاختبار الحالية

| المجموعة | النتيجة |
|---|---:|
| الاختبارات الكاملة لمشروع `TransportERP.Tests` | 41/41 PASS |
| اختبارات AuditEvent وPostgreSQL | 4/4 PASS ضمن المجموعة |
| اختبارات SyncOperation وConflictCase | 4/4 PASS ضمن المجموعة |
| اختبارات HTTP/JWT/API | 4/4 PASS ضمن المجموعة |
| اختبار البناء المستهدف لـ`TransportERP.Tests` ومراجع API/Infrastructure | 0 تحذير، 0 خطأ |
| فحص NuGet للمشاريع API وInfrastructure | لا توجد حزم ضعيفة ظاهرة في المصادر الحالية |

## قيود لا تزال حاكمة

نجاح الاختبارات لا يساوي قرار مراجعة مستقلة. يجب أن يفحص الفريق الالتزام الجديد نفسه، وأن يثبت الأدلة من Git، وأن يصدر قراراً صريحاً. كما لا يجوز إغلاق RR2-C-06 قبل اعتماد المالك الفعلي؛ هذه المصفوفة لا تنشئ اعتماداً بالنيابة عنه.
