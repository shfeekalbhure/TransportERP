# مراجعة الأكواد والخدمات الخلفية — TransportERP

المراجع الفعلي: `/root/code_audit`، كُلّف بمراجعة للقراءة فقط. المصدر النهائي: `9d02ac8d19e39971ad233108faecc8154a1d5745`، داخل `جذر نسخة المصدر`. تمت مقارنة التغييرات ذات الصلة بالمصدر السابق `1026a9069756b956b17b55d853e5ebda535810ec` لإسقاط العيوب التي أصلحت بالفعل. لم تُعدل ملفات المصدر، ولم يُشغّل بناء أو اختبار .NET؛ أخبر المنسق أن dotnet غير متاح. أدلة أدناه استنتاجات مباشرة من مسارات التنفيذ المقروءة، واختبارات الإثبات المذكورة **مطلوبة ولم تنفذ**.

## نطاق الفحص وما هو موجود

قراءة ملفات API الأربعة القديمة وموديولات تسجيل الدخول ونطاق القبض وإعدادات الأستاذ العام والقبض الجديدة؛ خدمات Application الثلاث للبوليصة والمالية والترحيل؛ Aggregate وقواعد المجال الثلاث؛ مستودعات EF، معاملات الحفظ، الترقيم، سجل التدقيق، المزامنة، سياسة الأستاذ العام، خطة قيد القبض، المرفقات والمراجع؛ خرائط EF وقيودها؛ ومراجعة مقاطع الاختبارات المرتبطة بالنتائج. جرى جرد migrations وأسماء الاختبارات؛ **لم تُراجع كل migration وكل اختبار سطرًا بسطر**، ولم تطبق migrations أو تثبت سلامة قاعدة منشورة.

التغطية المحددة لهذا المراجع: 35 ملف C# بقراءة كاملة أو مقاطع مركزة من أصل 57 ملف C# خارج migrations في API/Application/Infrastructure/Domain/Contracts؛ علاوة على فحص csproj الثلاثة ومقاطع اختبارات النتائج. هناك 28 ملف migration/snapshot و27 ملف C# اختبارات في الأقسام المذكورة؛ جردها لا يساوي مراجعة دلالية كاملة. ليست هذه شهادة مراجعة جميع ملفات المشروع أو جميع أحداث الشاشات.

قائمة الملفات التي قُرئت (كاملة أو مقاطع):

- API (8): `Program.cs`؛ `Waybills/WaybillApiModule.cs`؛ `Waybills/WaybillFinanceApiModule.cs`؛ `Waybills/ShippingExecutionApiModule.cs`؛ `Accounting/ReceiptApiModule.cs`؛ `Accounting/GeneralLedgerSettingsApi.cs`؛ `Authentication/DesktopLoginModule.cs`؛ `Authentication/DesktopReceiptScopeModule.cs`.
- Application (4): `P1Baseline/P1InMemoryBaseline.cs`؛ `Waybills/WaybillApplicationService.cs`؛ `Waybills/WaybillFinanceApplicationService.cs`؛ `Waybills/ShippingExecutionApplicationService.cs`.
- Domain (3): `Waybills/WaybillAggregate.cs`؛ `Waybills/WaybillFinancialRules.cs`؛ `Waybills/ShippingExecutionRules.cs`.
- Contracts (3): `Core/MoneyContracts.cs`؛ `Core/OperationContext.cs`؛ `Accounting/ReceiptContracts.cs`.
- Infrastructure/Persistence (17): `ConcurrencySafeWaybillRepository.cs`؛ `WaybillPersistenceServices.cs`؛ `WaybillFinancePersistence.cs`؛ `ShippingExecutionPersistence.cs`؛ `VoucherLifecycleService.cs`؛ `SyncOperationService.cs`؛ `AuditEventService.cs`؛ `TransportErpPersistenceExtensions.cs`؛ `ReceiptWorkspaceService.cs`؛ `ReceiptPostingPlan.cs`؛ `ReceiptReferences.cs`؛ `ReceiptWorkflow.cs`؛ `GeneralLedgerPolicyService.cs`؛ `ReceiptAttachments.cs`؛ `TransportErpDbContext.cs`؛ `TransportErpP2ModelCustomizer.cs`؛ `TransportErpP2ShippingModel.cs`. كذلك فُحصت أسطر indexes/refs المحددة في `TransportErpP2FinanceModel.cs` بالبحث؛ لا تحتسب كقراءة كاملة.

حالة التسليم بعد تصحيح المنسق: أصلح المنسق CODE-01 بإضافة نسخ Volume، وCODE-31 بقلب المدين والدائن في النموذج الذاكري، وأضاف اختبارات انحدار؛ راجع مراجع مستقل فرق المصدر ونجح git diff --check. تحققت أنا من السطرين المصححين في ملف التسليم. **اختبارات .NET والبناء لم ينفذا لغياب dotnet**؛ لا تُعد النتيجتان مجازتَين تشغيليًا. أوصافهما أدناه تثبت عيب SHA المصدر وآلية إصلاحه، وليستا عيبين متروكين في نسخة التسليم المصححة.

يوجد في النسخة الجديدة تسجيل دخول محلي قائم على PasswordHasher، اختيار شركة وفرع وفترة، صلاحيات القبض تُقرأ من قاعدة البيانات، سند قبض JSON محفوظ، ترحيل ينتج رأس قيد وسطورًا متوازنة في معاملة واحدة مع الترقيم والتدقيق، وعكس قبض يقلب المدين والدائن فعلًا. هذه أفعال منفذة في كود المصدر وليست مجرد أسماء أزرار. بالمقابل، `P1InMemoryService` و`VoucherLifecycleService` القديمان لا يظهر لهما استدعاء إنتاجي في API؛ استخدامهما الظاهر في الاختبارات. لذلك لا يجوز إسقاط عيوب النموذج القديم على ترحيل القبض الجديد.

## نتائج مثبتة في المسارات الإنتاجية

### CODE-01 — فقدان حجم صنف البوليصة عند الحفظ — عالٍ

الدليل: `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs:75-84,119-136`؛ `WaybillPersistenceServices.cs:210` يثبت أن النسخ الآخر ينسخ Volume؛ تسجيل المستودع الفعلي `TransportERP.Api/Waybills/WaybillApiModule.cs:16`.

كل SaveAsync يحذف أصناف البوليصة ويعيد إنشاءها، لكن ToItemEntity لا يعيّن `Volume`. إدخال Volume=50 وأبعاد حجمها 24 يؤدي إلى استجابة حفظ تحتوي 50 من الـ aggregate، ثم تحميل من قاعدة البيانات بقيمة null؛ كشف الحمولة يستعمل حاصل الأبعاد 24. يتكرر الفقد عند submit/return/approve أيضًا. أصغر إصلاح: إضافة `Volume = x.Volume`؛ الأفضل لاحقًا تحديث الأصناف الموجودة بدل حذفها جميعًا. الإثبات: حفظ draft بالحجم الصريح، إعادة القراءة في DbContext جديد، submit/approve، إنشاء manifest؛ تحقق بقاء 50 والتوزيع النسبي الصحيح.

### CODE-02 — رفض إعادة إرسال خطة الدفع الناجحة — عالٍ

الدليل: `TransportERP.Application/Waybills/WaybillFinanceApplicationService.cs:47-49` يفحص Version قبل `store.SetPaymentPlanAsync`؛ بينما `WaybillFinancePersistence.cs:35-40` يحوي replay الذي لا يصل إليه التطبيق.

نجاح الخطة يرفع الإصدار؛ إعادة الطلب نفسه مع ExpectedVersion القديم بعد ضياع الرد تعيد CONCURRENCY_CONFLICT بدل نتيجة العملية. أصغر إصلاح: نقل منطق replay والتحقق الكامل من البصمة إلى موضع واحد قبل رفض الإصدار، مع رفض نفس المفتاح بمحتوى مختلف. الإثبات: طلب متطابق مرتين عبر Application/API، ثم إعادة المفتاح بسطور أخرى.

### CODE-03 — مفاتيح عمليات البوليصة لا تتحقق من محتوى أو نوع الأمر — عالٍ

الدليل: `WaybillApplicationService.cs:79-81,311-320`؛ `WaybillPersistenceServices.cs:38-42` يقارن LastClientOperationId فقط.

إنشاء draft جديد بنفس المفتاح ولكن وجهة/عملة أخرى يعيد القديم دون conflict. بعد update ناجح، استعمال مفتاح update نفسه مع إصدار قديم في submit يجعل IsReplay يعيد true؛ API يعلن OK وبوليصة بقيت DRAFT، دون تنفيذ submit. أصغر إصلاح: outcome سجل مرتبط بالشركة/الفرع/الكيان/نوع الأمر وبصمة المحتوى، كما فعلت أوامر shipping النهائية. الإثبات: نفس المفتاح مع محتوى متغير، ونفس المفتاح لأمرين مختلفين، وإعادة أمر قديم بعد أمر أحدث.

### CODE-04 — سجل التدقيق خارج معاملة إنشاء/تعديل/إرسال البوليصة — عالٍ

الدليل: `WaybillApplicationService.cs:88-95,119-122,158-161,180-183,201-204,282-287`؛ `ConcurrencySafeWaybillRepository.cs:87-88` ينهي معاملته قبل audit؛ فقط approve مغلف في UnitOfWork.

فشل audit بعد نجاح الحفظ يترك التغيير التجاري محفوظًا مع رد فاشل وسجل ناقص. على retry يعاد replay مباشرة ولا يعاد التدقيق، فيبقى النقص. ينطبق على إنشاء الطرف كذلك. أصغر إصلاح: استخدام UnitOfWork لكلا الحفظ والتدقيق، وإضافة مراجعة ذرية أو outbox واضح. الإثبات: audit sink يرمي بعد save؛ قاعدة البيانات يجب أن تبقى كما كانت، ثم إعادة ناجحة تنتج سجلًا واحدًا.

### CODE-05 — بصمة تدقيق P2 تتغير بعد تخزين PostgreSQL — عالٍ

الدليل: `WaybillPersistenceServices.cs:416,421` يستخدم UtcNow كامل الدقة؛ `AuditEventService.cs:173` يضم تمثيل timestamp ذو سبع خانات في hash؛ خريطة OccurredAt timestamptz. المسار الجديد للقبض ينظّمها بالفعل في `AuditEventService.cs:64-65` والمسار القديم لا يفعل.

عندما لا تكون ticks قابلة للقسمة على 10، PostgreSQL يخزن microseconds فتختلف البصمة بعد قراءة جديدة. أصغر إصلاح: توحيد مولد أحداث P2 مع دقة microsecond، وترتيب متزايد داخل stream. الإثبات: إنشاء حدث P2 بوقت مضبوط ينتهي tick=1 ثم roundtrip والتحقق. لا تعني القراءة الحالية أن كل سجل سابق فاسد؛ ذلك يتطلب فحصًا على قاعدة فعلية.

### CODE-06 — التحقق الجزئي من سلسلة التدقيق يدمج streams مستقلة — متوسط

الدليل: `AuditEventService.cs:134-150`؛ وجود أي companyId/branchId/deviceId يجعل VerifyStream يعمل مرة واحدة على جميع الأحداث المطابقة.

التحقق لشركة وحدها بها فرعان أو لفرع به جهازان يجمع سلاسل مستقلة في سلسلة واحدة ويبلغ PREVIOUS_HASH_MISMATCH لسجل سليم. أصغر إصلاح: التجميع حسب Company/Branch/Device بعد تطبيق أي فلاتر، وليس عند غياب الفلاتر فقط. الإثبات: شركتان/فرعان/جهازان، ثم verify لكل مرشح جزئي وكامل.

### CODE-07 — بصمة التدقيق لا تغطي لقطات before/after أو نوع الكيان — متوسط

الدليل: `AuditEventService.cs:163-176` لا يشمل BeforeJson/AfterJson/EntityType/DeviceId/Ip رغم أن response يعرضها.

ComputeHash ينتج نفس البصمة إذا تغيرت لقطة بيانات السند أو نوع كيانه. حظر UPDATE في القاعدة يمنع مسارًا عاديًا للتعديل، لكنه لا يجعل التحقق الحالي إثباتًا لسلامة تلك الحقول. الإصلاح: canonical encoding مؤرشف بإصدار يشمل جميع حقول الدليل، مع سياسة انتقال تحفظ التحقق من السجلات السابقة. الإثبات: تعديل كل حقل في نسخة غير محفوظة والتأكد أن البصمة تختلف. لا ينصح بتغيير خوارزمية التاريخ القديم مباشرة.

### CODE-08 — يمكن اعتماد بوليصة باستخدام تسلسل سند قبض — عالٍ

الدليل: `WaybillApplicationService.cs:209-268` يمرر NumberSequenceId؛ `WaybillPersistenceServices.cs:303-305` يراجع company/branch/status ولا يراجع DocumentType.

تمرير تسلسل RECEIPT_VOUCHER للبوليصة يستهلكه ويولد رقم بوليصة ببادئة القبض. خدمة القبض الجديدة تتحقق من النوع في ReceiptWorkspaceService:141. أصغر إصلاح: تحقق صريح من document type في حجز البوليصة/خدمة متخصصة، قبل زيادة NextValue. الإثبات: sequence للقبض وموافق للبوليصة؛ الخاطئ مرفوض دون استهلاك رقم.

### CODE-09 — قراءة حجز الترقيم المعاد لا تقيد الفرع — عالٍ

الدليل: `WaybillPersistenceServices.cs:294-301` يبحث Company+IdempotencyKey فقط، في حين ScopedReservation:367-369 يراجع branch.

داخل الشركة، استخدام مفتاح حجز لفرع آخر يمكن أن يعيد DTO لحجز ذلك الفرع قبل فحص نطاق التسلسل. قد ينتهي التنفيذ لاحقًا برفض الربط، لكنه يكشف ويحمل حجزًا من نطاق غير مسموح بدل رفضه في القراءة. الإصلاح: التحقق من scope أيضًا عند replay ومن ارتباط الحجز بالكيان المقصود. الإثبات: فرعان، نفس key؛ عدم إعادة حجز الفرع الأول للثاني.

### CODE-10 — تخصيص شحنة بعد توليد الكشف يخلق تعارضًا يمنع الإقفال حتى التراجع — عالٍ

الدليل: `ShippingExecutionPersistence.cs:199-201` يسمح allocate لكل trip DRAFT دون فحص وجود manifest؛ GenerateManifest:374-377 يستبعد الأصناف المرتبطة بكشف؛ `TransportErpP2ShippingModel.cs:115-117` يفرض كشفًا واحدًا لكل trip؛ Finalize:586-589 يشترط تغطية جميع allocations. لا يوجد refresh manifest في IShippingExecutionStore/API.

توليد كشف لتخصيص A، ثم تخصيص B في الرحلة نفسها مسموح. finalize للكشف الأول يرفض coverage، وتوليد كشف ثان يرفض unique TripId؛ لا يمكن unallocate A لأنه مرتبط بكشف. **يمكن التعافي حاليًا بإلغاء تخصيص B غير المرتبط بالكشف** وفق Unallocate:287-298 ثم إقفال كشف A؛ إذًا ليست رحلة محبوسة بلا أي مخرج، لكن B لا يمكن ضمه إلى الكشف الحالي ويجب التراجع عن إجراء سبق قبوله. أصغر إصلاح: منع allocate الجديد بعد وجود كشف مع رسالة واضحة؛ أو أمر refresh قبل التحميل يحافظ على IDs. الإثبات: A→generate→B→load→finalize ثم unallocate B→finalize، وتوثيق السلوك المقصود.

### CODE-11 — إضافة حجز/إيقاف بعد finalize لا تمنع مغادرة الرحلة — عالٍ

الدليل: EnsureNoActiveHold يستدعى release/allocate/load/finalize، لكنه لا يستدعى في `ShippingExecutionPersistence.cs:725-821` StartTrip ولا handover:652-705.

سجل WaybillHold ACTIVE أضيف بين finalize وstart يبقى قائمًا بينما start ينشئ DEPART. لا توجد واجهة إنشاء hold حالية، لذلك إعادة الإنتاج تستلزم إدخاله كبيانات مستودع أو خدمة مستقبلية؛ الخلل ثابت في حماية التنفيذ وليس ادعاء وجود شاشة hold. الإصلاح: إعادة فحص holds لجميع البوالص في معاملة start قبل التحول. الإثبات: finalize، ACTIVE hold، handover، start يرفض ولا ينشئ DEPART.

### CODE-12 — local JWT لا يسمح بالمزامنة الحالية — فجوة تكامل عالية

الدليل: `DesktopLoginModule.cs:152-161` يصدر claims sub/company/branch/fiscal_period/scope/permission فقط؛ `Program.cs:106-111` يطلب device_id وdevice_registered=true.

حتى المستخدم ذو sync.operations.execute الذي دخل من login الجديد يُرفض DEVICE_NOT_REGISTERED دائمًا. الإصلاح يتطلب تسجيل أجهزة مؤكد وتذكرة جلسة مرتبطة بالجهاز؛ لا ينبغي إزالة الشرط أو اختراع claim true. الإثبات: login/select-scope ثم sync batch بجهاز مسجل وغير مسجل.

### CODE-13 — المزامنة تخزن طابورًا دون منفذ إنتاجي — فجوة تكامل عالية

الدليل: API batch يستدعي EnqueueSyncOperationAsync فقط؛ بحث جميع مصادر API/Application/Infrastructure عن BackgroundService/IHostedService/Register worker أو استدعاء TransitionSyncOperationAsync لا يكشف executor إنتاجيًا. وظائف transition/retry/conflict موجودة كخدمات وغير مربوطة endpoints.

200 في batch يعني QUEUED؛ لا توجد هنا معالجة payload لإحداث تعديل فعلي على البوليصة/الحساب/القبض أو تنزيل تغييرات. الإصلاح: command dispatcher موثق + worker + dedup/permission/version checks + واجهة نتائج، بعد حسم العقود. الإثبات: offline إنشاء ثم اتصال يؤدي لكيان واحد محفوظ وإصدار نتيجة، لا مجرد صف queue.

### CODE-14 — replay المزامنة يتحقق hash الجسم فقط ويتجاهل هوية الأمر — عالٍ

الدليل: `SyncOperationService.cs:85-95,130-138` يقارن PayloadHash فقط.

نفس Device+ClientOperationId والجسم `{}` يمكن إرساله مع EntityId أو EntityType أو OperationType أو BaseVersion آخر؛ يعاد الصف القديم دون mismatch. الإصلاح: بصمة كاملة تشمل نوع الأمر والكيان والإصدار والجسم، أو مقارنتها صراحةً. الإثبات: تغيير EntityId فقط وإبقاء الجسم/hash؛ يجب رفضه.

### CODE-15 — صف sync فاشل يفسد العناصر اللاحقة في نفس batch — عالٍ

الدليل: `SyncOperationService.cs:122-139` يفصل entity فقط عند unique violation؛ ValidateCommand لا يحد طول ClientOperationId/EntityType؛ خريطة DbContext تحدهما إلى 120. `Program.cs:118-148` يلتقط أي exception ثم يواصل باستخدام DbContext نفسه دون فصل entity المرفوض.

أول operation بمفتاح 121 حرفًا يفشل DB save ويظل Added؛ operation صحيح يليه يحاول SaveChanges إعادة حفظ الصف الخاطئ فيفشل أيضًا. الإصلاح: تحقق طول/JSON/refs قبل tracked mutation، واستخدام سياق أو savepoint مستقِل لكل عنصر، فصل التغييرات الخاصة بالعنصر عند الفشل. الإثبات: batch [طول غير صالح، صحيح]؛ الثانية يجب أن QUEUED وتحفظ فعلًا.

### CODE-16 — عنصر null في batch يسبب 500 بدل نتيجة رفض — متوسط

الدليل: `Program.cs:118-146` يقرأ item.EntityId داخل try ثم يقرأ item.ClientOperationId داخل catch؛ عقد IReadOnlyList لا يمنع JSON `[null]` أثناء التشغيل.

NullReferenceException الأول يلتقطه catch العام، لكن الثاني داخل catch يهرب. الإصلاح: validate null element قبل try واستخدام نتيجة مرتبطة بفهرس العنصر عندما لا يوجد clientOperationId. الإثبات: operations=[null,valid]؛ HTTP200 بنتيجتي رفض/نجاح دون انهيار الطلب.

### CODE-17 — transition يسمح بتجاوز رفض retry وحده — متوسط

الدليل: `SyncOperationService.cs:162-166` وIsAllowedTransition FAILED→SENDING؛ RetryOperation:199-229 يمنع errors غير قابلة للإعادة ويحد MaxRetryCount، لكن Transition لا يطبق هذين الشرطين.

المستدعي المباشر يستطيع نقل FAILED/VALIDATION_ERROR إلى SENDING أو تجاوز retry limit دون RetryOperation. الخدمة غير معروضة حاليًا عبر API فلا يوصف هذا كمسار مستخدم منشور. الإصلاح: مركزية قواعد retry في state transition وتحديث العدد والتوقيت مرة واحدة. الإثبات: FAILED غير قابل، retry exhausted، ووقت backoff؛ جميعها لا تصل SENDING.

### CODE-18 — null حقول متداخلة تتحول إلى 500 في APIs — متوسط

الدليل: `WaybillApplicationService.cs:273,329` request.Address/input.Address؛ `WaybillFinanceApplicationService.cs:78` request.Amount؛ `ReceiptWorkspaceService.cs:209,59,234` Lines/Additional؛ API Execute الجديد لا يلتقط NullReferenceException، والقديم كذلك.

إرسال null لهذه الحقول صالح نحويًا في JSON؛ NRT ليست validation تشغيلية. الإصلاح: validator صريح متسلسل يتعامل مع null وبنية القوائم، قبل استعمالها، وإرجاع error field مناسب. الإثبات: request لكل null nested field، قائمة بها null، حدود الأطوال، دون 500 أو حفظ جزئي.

### CODE-19 — التحقق من نوع role يسمح بالأرقام غير المعرفة — متوسط

الدليل: `WaybillApplicationService.cs:327` Enum.TryParse يقبل "99"؛ WaybillPartyValue.EnsureValid لا يتحقق Enum.IsDefined؛ الخريطة CHECK SENDER/RECEIVER/PAYER ترفض لاحقًا.

Draft بparty role="99" يمر طبقة المجال ويصل خطأ constraint غير معالج في المستودع الآمن ExecuteSave، فينتج 500. الإصلاح: Enum.IsDefined أو قاموس قيم نصية مسموحة، مع رفض قبل الحفظ. الإثبات: "99" و"Sender" و"SENDER" وفق العقد.

### CODE-20 — قراءة party replay قد تعيد بيانات فرع آخر — عالٍ

الدليل: `WaybillApplicationService.cs:279-281` CreateParty replay؛ `WaybillPersistenceServices.cs:251-256` GetByClientOperation يبحث company+operation فقط، بينما search:237-239 يقيّد branch.

مستخدم فرع B يرسل مفتاح إنشاء مستخدم بفرع A فتعود بيانات الطرف بما فيها الهاتف والعنوان خارج نطاق نتائج بحث B. الإصلاح: company-wide key يمكن الاحتفاظ به، لكن تحقق branch/party visibility قبل الإرجاع وارفض conflict. الإثبات: فرعان في الشركة مع نفس operation key.

### CODE-21 — أخطاء إنشاء الطرف كلها تصنف كتحذير تكرار — متوسط

الدليل: `WaybillPersistenceServices.cs:281` catch DbUpdateException بالكامل → PARTY_DUPLICATE_WARNING.

عملة/شركة/فرع غير صالح، أطوال كبيرة، أو خطأ constraint يقال عنه duplicate؛ concurrent retry صحيح لا يعيد الكيان الموجود. الإصلاح: تصنيف SqlState/constraint محدد، إعادة قراءة replay في unique-operation فقط، وإعادة ترميز validation/infrastructure الحقيقي. الإثبات: replay متزامن، name زائد، FK غير صالح، ثم التحقق من error وentity count.

### CODE-22 — التحصيل يستخدم حقل محاسبي دون مطابقة المبلغ/العملة/الاتجاه — عالٍ

الدليل: `WaybillFinancePersistence.cs:272-301` EnsureAccountingReferenceAsync يفحص وجود المستند ونطاقه وحالته فقط؛ RecordCollection:118-128 يربط المبلغ المرسل مباشرة.

سند قبض 10 يمكن ربطه بتحصيل 100 أو بعملة أخرى؛ نفس المستند يمكنه تغطية عدة بوالص دون توزيع يتأكد من إجمالي السند. PAYMENT_VOUCHER يقبل لتحصيل موجب أيضًا دون سياسة اتجاه. الإصلاح: allocations مالية typed تربط المستند والسطور وتتأكد من العملة والمبلغ القابل للتخصيص والاتجاه، في المعاملة ذاتها. الإثبات: مرجع 10 ومحاولة 100؛ عدة بوالص تتجاوز إجمالي المستند؛ اتجاه عكسي.

### CODE-23 — سند قبض معكوس يظل مرجعًا صالحًا لتحصيل جديد — عالٍ

الدليل: `ReceiptWorkspaceService.cs:198` يضيف ReversalJournalId دون تغيير Status؛ Document:258 يعرض REVERSED بالاشتقاق؛ `WaybillFinancePersistence.cs:282-284` يفحص Status APPROVED/POSTED فقط.

عكس السند بنجاح يحافظ على Status=POSTED في التخزين، لذلك جمع جديد مرتبط به مقبول رغم زوال أثر القبض بالقيد العكسي. أصغر إصلاح في فحص المرجع: ReversalJournalId==null؛ يلزم كذلك سياسة للعلاقات الموجودة عند عكس السند. الإثبات: post receipt → reverse → record collection referencing it؛ يجب رفضه. لا يغير الإصلاح حالة كل جدول دون migration متفق عليه.

### CODE-24 — سند القبض المرتبط بالبوليصة لا يحدث تحصيلها — فجوة محاسبية عالية

الدليل: `ReceiptWorkspaceService.cs:234-237` يفحص line.WaybillId ووجودها فقط؛ Post:147-160 ينشئ JournalEntry ويمرر السند POSTED؛ لا إنشاء CollectionTransaction أو FinancialLink أو تحديث Waybill.FinancialStatus. Reverse:188-200 لا ينشئ reversal collection كذلك.

اختيار بوليصة في سطر سند ثم ترحيله يعرض رابطًا ويُرحّل ماليًا، لكن البوليصة تبقى UNPAID حتى استدعاء API آخر مستقل، قد يكرر التحصيل إذا لم تكن الملكية واضحة. الإصلاح يحتاج عقد ملكية ومفاتيح سطور وتخصيصات موحدة، وليس زرًا إضافيًا فقط. الإثبات: بوليصة مستحقة 100 وسند لها 100؛ انتقال واحد PAID ثم reverse إلى UNPAID، مع replay ومنع duplication.

### CODE-25 — تحصيل جديد يقبل تاريخ فترة مقفلة — عالٍ

الدليل: `WaybillFinancePersistence.cs:102-148` RecordCollection لا يفحص fiscal period، في حين Reverse:187-193 يفحص الفترة الأصلية المقفلة.

يمكن إضافة تحصيل بتاريخ شهر مغلق ثم يصبح عكسه ممنوعًا فورًا. الإصلاح: تعريف تاريخ محاسبي منضبط بالمنطقة الزمنية، والتحقق من فترة مفتوحة عند الإدخال، أو فصل التحصيل التشغيلي عن اعتماد أثره المحاسبي مع حالات واضحة. الإثبات: closed period ثم record collection به؛ السياسة يجب أن تمنع أو تعزل الأثر صراحةً.

### CODE-26 — التاريخ المحاسبي للعكس يعتمد UTC بدل تاريخ العمل — متوسط

الدليل: `WaybillFinancePersistence.cs:187` original.CollectedAt.UtcDateTime.Date.

جمع 9 أكتوبر 00:30 +03 يصبح 8 أكتوبر في فحص الإقفال. الإصلاح: business date مستقل محفوظ أو timezone الشركة قبل استخراج Date؛ لا تستعمل timezone الخادم. الإثبات: حد منتصف الليل اليمني، وفتح يوم وإقفال اليوم المجاور.

### CODE-27 — replay التحصيل يتجاهل بيانات جوهرية — متوسط

الدليل: `WaybillFinancePersistence.cs:334-341` يقارن waybill/currency/amount/rate فقط؛ reversal replay:171-178 يقارن collectionId فقط.

نفس المفتاح مع PartyId أو payer/method/collector/time/accountingRef آخر يعاد كنجاح؛ reversal مع سبب/مرجع مختلف يعاد القديم. الإصلاح: كامل بصمة الطلب. الإثبات: تغيير كل حقل على حدة وإبقاء المفتاح.

### CODE-28 — uniqueness للتحصيل على مستوى الشركة لكن replay على مستوى الفرع — متوسط

الدليل: `TransportErpP2FinanceModel.cs:57` Company+ClientOperationId unique؛ `WaybillFinancePersistence.cs:266-268` يبحث Company+Branch+Key.

نفس operationId مشروع في فرعين لا يعاد scoped replay، بل يتسبب DUPLICATE_OPERATION. لا توجد ضمانات عالمية توليد key في العقد. الإصلاح: تقرير مستوى uniqueness ثم توحيد index وlookup والعقد، دون حذف index حمايةً قبل الترحيل. الإثبات: فرعان ونفس المفتاح حسب السياسة المختارة.

### CODE-29 — تغير إعدادات القبض يعطل سندًا سبق مراجعته/اعتماده — متوسط، قرار سياسة مطلوب

الدليل: Review/Approve/Post تستدعي SettingsAsync الحالية؛ GeneralLedgerPolicyService يحفظ snapshot **سير العمل فقط**؛ ReceiptPostingPlan policy snapshot ينشأ أثناء post.

حذف Type/Collector/Destination من إعدادات الشركة بين الاعتماد والترحيل يمنع Validate، وتغيير الحساب المرتبط بالوجهة يغير حساب الترحيل لسند اعتمد سابقًا. لا نفترض أن ذلك مخالف لسياسة مستخدم معتمدة؛ هو السلوك الفعلي ويلزم حسم هل الاعتماد يجمد إعدادات الترحيل. الإصلاح وفق القرار: freeze snapshot عند save/approve أو إبطال الاعتماد صراحة مع audit. الإثبات: سند معتمد ثم تعديل وجهة حسابه أو حذفه.

### CODE-30 — جلسة الفترة المختارة لا تقيد تاريخ سند القبض — متوسط، قرار نطاق مطلوب

الدليل: JWT يحوي fiscal_period_id؛ ReceiptApiModule.Execute:133-140 ينشئ OperationContext دونه؛ Post:136-139 يختار أي OPEN period يحوي التاريخ.

اختيار فترة A في الدخول لا يمنع سندًا مؤرخًا في فترة B المفتوحة بالشركة. يلزم تحديد هل الاختيار مجرد سياق عرض أم نطاق إدخال؛ لا يعلن هذا عيبًا محاسبيًا محسومًا قبل القرار. الإصلاح بعد القرار: carry period context والتحقق أو إزالة الإيحاء من الواجهة. الإثبات: فترتان مفتوحتان، login A ثم receipt date B.

## نتائج النموذج القديم/الخدمات غير المربوطة إنتاجيًا

### CODE-31 — ReverseJournal في النموذج الذاكري لا يعكس المدين والدائن — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:389`: original with ينسخ Lines الأصلية كما هي. النتيجة قيد تعويضي بنفس اتجاه الأصل، لا عكسه. **خدمة القبض الجديدة تعكس السطور صحيحًا في ReceiptWorkspaceService:196-197.** إصلاح القديم: `Lines = original.Lines.Select(l => l with { Debit=l.Credit,Credit=l.Debit }).ToArray()`، وإسناد فترة عكس مفتوحة. الاختبار الحالي P1InMemoryBaselineBehaviorTests:60-62 يراجع ReversalOf/audit فقط ولا يفحص مبالغ السطور.

### CODE-32 — idempotency القيد في الذاكرة غير مقيد بالشركة — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:361-362`: بحث ClientOperationId عبر كل شركات Store. شركة B بنفس key تستلم قيد A بعد تحقق حسابات B دون تحقق owner. أصغر تصحيح Company/Branch+key وبصمة. الإثبات شركتان.

### CODE-33 — مدخلات القيد الذاكري تسمح بالمبالغ السالبة أو مدين ودائن في نفس السطر — عالٍ إذا أعيد استخدامه

الدليل `P1InMemoryBaseline.cs:358-360` مجموع متوازن وحسابات قابلة للترحيل فقط. القاعدة PostgreSQL `ck_journal_lines_amounts` تمنع السلوك، لذلك اختبارات النموذج لا تمثل القاعدة. الإصلاح validation نفس قواعد السطور. الإثبات قيد balanced negative، سطر يضع debit/credit معا، وقيد صفر.

### CODE-34 — فترات الذاكرة متداخلة مسموحة — متوسط

الدليل `P1InMemoryBaseline.cs:331-339` يفحص start>=end فقط ويسميه PERIOD_OVERLAP؛ لا مقارنة بالفترات الأخرى. قاعدة EF الحالية تمنع فقط التطابق التام عبر unique Company/Start/End ولا تمنع التداخل أيضًا؛ القبض الجديد يتحقق periods.Count==1 ويمنع post عند التداخل. الإصلاح قيد range exclusion/خدمة إدارة فترات حسب السياسة. الإثبات فترتان متقاطعتان غير متطابقتين.

### CODE-35 — VoucherLifecycleService القديم يرحّل بتغيير حالة فقط — فجوة إذا أعيد ربطه

الدليل `VoucherLifecycleService.cs:108-142`: PostReceipt/PostPayment → TransitionAsync فقط؛ actorId لا يدخل audit أو stamp، ولا إنشاء قيد. لا يوجد تسجيله داخل API. لا تستخدمه لتنفيذ سند الصرف قبل تكميل pipeline محاسبية؛ ترحيل القبض الفعلي الجديد مختلف وموجود.

## حدود تنفيذ وملاحظات تفصل الموجود عن المطلوب

1. endpoints عمليات البوليصة لا تحوي GET بوليصة واحدة/قائمة، وعمليات shipping لا تحوي GET رحلة/كشف/تفاصيل عامة. responses العمليات قد تكفي أثناء المسار المتصل، لكنها لا تكفي بعد إعادة تشغيل التطبيق لاكتشاف IDs واستئناف المسودة. GetFinancialStatus خدمة موجودة دون endpoint في WaybillFinanceApiModule.
2. `MovementEvent` يقيد EventType إلى LOAD/DEPART في TransportErpP2ShippingModel:149. ARRIVED/CLOSED أسماء حالات في constants وليست أوامر وصول/تسليم قابلة للتنفيذ في API. لا يوجد هنا إثبات اكتمال رحلة البوليصة حتى التسليم أو GPS.
3. CreateTrip يتحقق أن VehicleId/DriverId GUID غير فارغ فقط؛ خريطة Trip لا تحوي FK vehicle/driver. يمكن إنشاء رحلة بأرقام عشوائية. يجب حسم master بيانات وسائل النقل والسائقين ثم تطبيق نطاقها وحالتها؛ لا اختراع جدول جديد أثناء هذه المراجعة.
4. JWT صلاحيات P2 مأخوذة من claims حتى expiry؛ القبض الجديد يعيد قراءة DB. تعطيل المستخدم أو سحب صلاحية P2 لا يُفحص داخل TryContext. السياسة غير موحدة؛ يلزم توحيد revocation/session policy ولا وصف JWT الصحيح بأنه بلا مصادقة.
5. login يسمح قاعدة البيانات بتكرار username بين الشركات ثم يرفض جميع matches عندما count!=1. هذا fail-closed مكتوب صراحة، لكنه يمنع دخول مستخدمين صحيحين إذا تشابه اسمهم بالشركتين. مطلوب حسم login identity أو tenant hint قبل إنتاج multi-company.
6. DI يثبت اتصال PostgreSQL وJWT عند بدء API، فلا يبدأ دونها. لا توجد migration apply داخل Startup، وهذا لا يثبت أن القاعدة خالية أو migrations نُفذت؛ فحص القاعدة يتطلب اتصالًا فعليًا.
7. ملفات csproj تستهدف net10.0. الاختبارات PostgreSQL RequireConnection ترمي عند غياب إعداد البيئة ولا تتخطى صامتًا. لا يجوز تقديم عدد اختبارات «نجح» من مجرد أسماء الملفات.
8. يوجد تكرار TryContext/HasPermission/MapError في ثلاثة موديولات P2، ومساران مختلفان لتدقيق EF، ومستودعا Waybill بآليتي update مختلفتين؛ هذا التكرار أنتج اختلاف Volume ودقة timestamps فعلًا، لذلك توحيد الأجزاء المشتركة ذو سبب مثبت.

## الإصلاحات المرشحة الصغيرة قبل تعديل التصميم

الأكثر محدودية ويمكن مراجعتها دون تغيير شاشة: CODE-01 نسخ Volume؛ CODE-02 ترتيب replay مع بصمة؛ CODE-05 تنظيم تاريخ P2 audit؛ CODE-06 grouping لكل stream؛ CODE-08 document type للترقيم؛ CODE-10 منع تخصيص بعد الكشف؛ CODE-16 null عناصر batch؛ CODE-19 رفض enum غير معرف؛ CODE-23 رفض سند معكوس كمرجع تحصيل. بقية الربط بين القبض والبوليصة، تنفيذ المزامنة، توحيد GPS والسائق/المركبة والفترات تحتاج مخطط عقود وملكية معاملات قبل التنفيذ.

الإقفال المطلوب: تثبيت SHA، تنفيذ اختبارات المصدر والوحدة على .NET10، ثم PostgreSQL disposable database تطبق جميع migrations، ثم API tests مع tokens ونطاقين وشركتين، وإعادة القراءة بسياق جديد للتحقق من الدقة والتدقيق. نجاح compile وحده لا يثبت نقل الأموال أو replay أو الوصول والتسليم.

## عيوب قديمة استبعدت من التقرير الحالي

`SyncOperationService.BaseVersion` كان غير منسوخ في 1026a906 وأصبح منسوخًا في 9d02ac8d:110. كذلك canonical trim لمفاتيح replay أضيف :74-80، والتحقق من FAILED error قبل mutation أضيف :168-170. لا تعرض هذه الثلاثة كعيوب حالية. Audit القبض الجديد ينظّم timestamps ويسلسلها :64-65، وعكسه يقلب السطور صحيحًا؛ المشكلة الحالية المحددة هي P2 sink والنموذج الذاكري القديم.
