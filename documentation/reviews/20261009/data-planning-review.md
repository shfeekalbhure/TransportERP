# مراجعة البيانات والربط — TransportERP

المراجع الفعلي: وكيل `data_planning`، مراجعة مستقلة للقراءة فقط.

النسخة الحاكمة لهذه النتائج: `feature/receipt-general-ledger-settings`، SHA `9d02ac8d19e39971ad233108faecc8154a1d5745`، المسار المحلي `جذر نسخة المصدر`. جرى فحص أولي للنسخة `1026a9069756b956b17b55d853e5ebda535810ec` ثم إعادة فحص النتائج على النسخة الأحدث؛ لا تُستخدم أحكام غياب الربط المحاسبي من النسخة القديمة في الحكم الحالي. جميع مسارات الأدلة التالية نسبية لجذر النسخة الأحدث.

هذه مراجعة واقع الكود، وليست اعتمادًا لقاعدة البيانات أو اكتمال المشروع. لم يُنشأ مخطط أو قاموس جديد، ولم تُطبّق Migration، ولم يُعدل كود التطبيق. تثبيت مرجع Onyx واعتماده يسبق أي تصميم جديد، وفق توجيه المستخدم. نتيجة مراجعة المرجع التأسيسي المنقولة من الوكيل الرئيسي: `NOT READY — GAPS REMAIN`، 33 فجوة مفتوحة، وعدم اعتماد المالك؛ لم يراجع هذا الوكيل ملفات المرجع خارج المستودع، ولا يحول أعدادها إلى أعداد شاشات الكود.

## 1. ما هو منفذ فعليًا

| البند | الحكم المثبت | الدليل |
|---|---|---|
| مزود البيانات | PostgreSQL عبر Npgsql وEF Core 10؛ لا يظهر موفر MySQL في مشاريع الكود التي جرى البحث فيها | `TransportERP.Infrastructure/TransportERP.Infrastructure.csproj:9-16`؛ `TransportERP.Infrastructure/Persistence/TransportErpPersistenceExtensions.cs:9-21`؛ `TransportERP.Api/Program.cs:14-18` |
| تركيب النموذج | نموذج P1 الأساسي، ثم إضافة أساس البوالص، ثم التمويل والشحن | `TransportERP.Infrastructure/Persistence/TransportErpP2CombinedModelCustomizer.cs:10-19`؛ `TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs:13-17` |
| الإعدادات | ثلاث طبقات بيانات: منصة، شركة، فرع؛ فرع الإعدادات مربوط بالشركة والفرع بمفتاح مركب | `TransportERP.Infrastructure/Persistence/P1Entities.cs:139-161`؛ `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:186-217` |
| إعدادات الأستاذ العام | خدمة حفظ حقيقية في `CompanySettings` بمفتاح `accounting.general-ledger.workflow`، نسخة سياسة مستقلة لكل مستند جديد وتدقيق وتزامن | `TransportERP.Infrastructure/Persistence/GeneralLedgerPolicyService.cs:18-83` |
| API الأستاذ العام | GET وPUT على `/api/v1/accounting/settings`؛ تفويض حالي على مستوى الشركة | `TransportERP.Api/Accounting/GeneralLedgerSettingsApi.cs:9-22`؛ `TransportERP.Api/Accounting/ReceiptApiModule.cs:129-170` |
| سند القبض | حفظ تفاصيل، مراجعة، اعتماد، ترحيل قيد، إلغاء قبل الترحيل، عكس بعد الترحيل، مرفقات، حسابات وأبعاد ومراجع | `TransportERP.Api/Accounting/ReceiptApiModule.cs:84-108`؛ `TransportERP.Infrastructure/Persistence/ReceiptWorkspaceService.cs:49-201`؛ `TransportERP.Infrastructure/Persistence/ReceiptReferences.cs:9-32` |
| الربط من سطح المكتب | شاشة الأستاذ العام تتصل بالخدمة وتفتح إعدادات القبض؛ جلسة HTTP موثقة تقيد الشركة والفرع | `TransportERP.Desktop/FrmMain.LedgerSettings.cs:11-79`؛ `TransportERP.Desktop/Authentication/DesktopSession.cs:13-70` |
| البوالص والتنفيذ | كيانات وبنية حفظ للبوالص والأطراف والتفاصيل والدفع والتحصيل والرحلات والتخصيص والكشوف والتحميل والمغادرة | مواضع الجداول أدناه؛ `TransportERP.Api/Program.cs:28-30,81-83` |
| التدقيق والمزامنة | خدمات سجل تدقيق وطابور عمليات API، مع أجهزة وصلاحيات ونطاق | `TransportERP.Api/Program.cs:19-27,85-151`؛ `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:353-419` |

وجود هذه الأكواد لا يثبت أن قاعدة تشغيل فعلية حوت الجداول أو أن النظام يعمل في جهاز المستخدم. لم يتصل المراجع بقاعدة فعلية، ولم يُنفّذ البناء أو الاختبارات في هذه المهمة.

## 2. الجداول الموجودة في مصدر النموذج، مقابل المسجلة توثيقيًا

عدد الجداول الفريدة في EF Snapshot: **41 جدولًا**. يوجد جدول تطبيقي إضافي منشأ عبر SQL في Migration باسم `shipping_command_outcomes`، فيصبح المصدر يعرّف **42 جدولًا تطبيقيًا**. جدول سجل EF `__EFMigrationsHistory` خارج هذا العدد. هذا تعداد التعريفات البرمجية، وليس تعداد قاعدة حية.

| مجموعة | جداول المصدر | عدد | الأدلة |
|---|---|---:|---|
| المؤسسة والهوية والإعدادات | currencies, companies, branches, users, roles, permissions, role_permissions, user_roles, user_permission_overrides, global_settings, company_settings, branch_settings | 12 | `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:77-217` |
| المحاسبة الأساسية | chart_of_accounts, fiscal_periods, financial_dimensions, journal_entries, journal_entry_lines, receipt_vouchers, payment_vouchers | 7 | الملف نفسه `:220-351` |
| التدقيق والمزامنة | audit_events, sync_operations, conflict_cases | 3 | الملف نفسه `:353-419` |
| أساس البوالص | operational_parties, waybills, waybill_parties, waybill_items, number_sequences, number_reservations | 6 | `TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs:19-161` |
| تمويل البوالص | payment_plan_lines, collection_transactions, waybill_financial_links | 3 | `TransportERP.Infrastructure/Persistence/TransportErpP2FinanceModel.cs:15-79` |
| تنفيذ الشحن | item_releases, trips, trip_stops, trip_allocations, manifests, manifest_lines, movement_events, waybill_holds | 8 | `TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs:17-188` |
| الإضافات الحديثة | receipt_attachments, accounting_workflow_snapshots | 2 | `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs:308-324` |
| نتائج أوامر الشحن SQL | shipping_command_outcomes | 1 | `TransportERP.Infrastructure/Persistence/Migrations/20260821170000_P2C01CTeam03PostgreSqlHardening.cs:14-36` |

التعداد الثاني المستقل من Snapshot: `TransportERP.Infrastructure/Persistence/Migrations/TransportErpDbContextModelSnapshot.cs:54,138,200,260,336,431,500,555,633,688,761,813,873,930,989,1085,1134,1204,1260,1353,1429,1491,1589,1668,1768,1836,1882,1991,2056,2094,2199,2269,2354,2401,2483,2524,2555,2665,2739,2815,2888`.

السجل `documentation/closeout/P1/TransportERP_سجل_الجداول_الموحد.csv` يصف جداول أقدم بحالة `LEGACY_REGISTERED` وقرار `RETAIN_AND_NORMALIZE`، مثل نقاط الخدمة والمحاور والمناطق وأنواع المركبات والأطراف والوكلاء وإثبات التسليم (`:4-8,16-18,27`). هذه ليست إثباتًا بوجود كيانات EF منفذة. كما أن الأعمدة القديمة تتضمن `bigint` و`tinyint`، بينما النموذج البرمجي الحالي يستخدم Guid وحالات نصية (`P1Entities.cs:20-25,38-61`؛ `TransportErpDbContext.cs:94-119`). يجب المحافظة على مصدر السجل القديم ووضع خريطة تطابق لاحقًا، لا تنفيذه تلقائيًا.

## 3. الفجوات المؤكدة وحالات البيانات غير المربوطة

| المعرف | الأولوية | النتيجة الدقيقة | الأثر العملي | مصدر الإثبات | نوع المعالجة المقترح |
|---|---|---|---|---|---|
| DATA-01 | مرتفعة | واجهة الإعدادات العامة لم تُربط بالحفظ أو النشر أو الاسترجاع أو التدقيق؛ جميعها معطلة صراحة | لا يمكن ضبط السياسة العامة من هذه الشاشة رغم وجود جداول إعدادات | `TransportERP.Desktop/Forms/SystemSettings/General/FrmGeneralSettings.cs:32-48` | تنفيذ خدمات وعقود وربط، بعد اعتماد المرجع؛ لا إعادة تصميم |
| DATA-02 | مرتفعة | إدارة الشركات تصميم فقط؛ الحفظ والتعديل والحذف والبحث والطباعة والشعار معطلة | وجود الشركات في نموذج EF لا يعني أن شاشة الشركة تديرها | `TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.cs:7,32-48` | ربط الكود بالتصميم الحالي بعد تثبيت عقد الشركة |
| DATA-03 | مرتفعة | حقول الشركة المرئية تتجاوز كيان Company الحالي: المجموعة، النشاط، السجل التجاري، البلد/المحافظة/المدينة، الاتصال، البريد، الموقع والعنوان | لا يوجد تطابق مباشر يحفظ كل حقل ظاهر؛ قد تُفقد بيانات إذا رُبطت الشاشة بالكيان المختصر مباشرة | `TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.Designer.cs:1527-1566`؛ `TransportERP.Infrastructure/Persistence/P1Entities.cs:38-48` | دراسة تطابق حقول قبل القاموس، مع إثبات Onyx وقواعد TransportERP |
| DATA-04 | مرتفعة | شاشة أسعار الصرف غير مربوطة بالخادم أو السجلات؛ النموذج لا يعرّف جدول أسعار صرف زمني، رغم وجود FX Snapshot وعوامل سعر في المستندات | سعر مستند ثابت موجود، لكن إدارة السعر وحدود السريان ومصدره ليست خدمة مرجع موحدة | `TransportERP.Desktop/Forms/SystemSettings/General/UcExchangeRates.cs:7,22-44`؛ `TransportERP.Contracts/Core/MoneyContracts.cs:30-74`؛ قائمة Snapshot أعلاه | تثبيت عقد سعر الصرف ثم ربطه؛ تصميم البيانات لاحق |
| DATA-05 | مرتفعة | الجغرافيا لها DTO للدولة والمحافظة والمديرية والمدينة والمنطقة، لكن قائمة EF الحالية لا تحتوي جداول هذه المراجع | البلد والمعرفات الجغرافية في snapshots لا تضمن تحقق العلاقات مع مصدر جغرافي محفوظ | `TransportERP.Contracts/Geo/GeoContracts.cs:7-23`؛ `GeoAddressContracts.cs:7-33`؛ `P2WaybillEntities.cs:20-24,66-70`؛ قائمة Snapshot | مرجع جغرافي موحد بعد ضبط العقد؛ لا إنشاء جداول الآن |
| DATA-06 | مرتفعة | نموذج الرحلة يحمل VehicleId وDriverId وOriginId وDestinationId وتوقف LocationId دون مرجع EF للسائق/المركبة/الموقع في النموذج الحالي | ليس هناك تكامل مرجعي كامل مثبت بالقاعدة لهذه الكيانات؛ لا تُعرض الرحلة كحل مكتمل للأسطول | `TransportERP.Infrastructure/Persistence/P2ShippingEntities.cs:20-45`؛ `TransportErpP2ShippingModel.cs:39-71`؛ قائمة Snapshot | تثبيت مصدر الحقيقة لهذه المعرفات وربط الخدمات تدريجيًا |
| DATA-07 | مرتفعة | تنفيذ حركة الشحن الحالي يقيد event types إلى LOAD وDEPART فقط | لا يوجد في هذا النموذج إثبات اكتمال وصول/تسليم/إرجاع/تلف/مطالبات/GPS | `TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs:144-150`؛ سجل إثبات التسليم القديم `documentation/closeout/P1/TransportERP_سجل_الجداول_الموحد.csv:27` | مراحل تشغيلية مستقلة بعد اعتماد المرجع؛ لا توسعة عشوائية للأحداث |
| DATA-08 | مرتفعة | إعدادات سير الأستاذ العام يدعمها حاليًا RECEIPT_VOUCHER فقط؛ سند الصرف لا يمتلك DocumentJson أو روابط قيد مثل القبض | لا يجوز تعميم نجاح القبض على الصرف أو اليومية أو كل المستندات | `GeneralLedgerPolicyService.cs:37-48`؛ `P1Entities.cs:234-274` | توسيع حسب عقد كل مستند مع فصل دورته واختباره |
| DATA-09 | متوسطة | تغيير إعدادات القبض لا يستدعي AppendAuditEvent في مسار الحفظ؛ الأستاذ العام يستدعي التدقيق صراحة | لا يظهر سجل من غيّر سياسة وجهات/أنواع القبض والتقريب والترقيم عبر هذا endpoint | `TransportERP.Api/Accounting/ReceiptApiModule.cs:72-82`؛ المقارنة `GeneralLedgerPolicyService.cs:65-68` | إصلاح كود: تدقيق تغييرات القبض في معاملة الحفظ نفسها |
| DATA-10 | مرتفعة | تكوين القبض محفوظ على مستوى الشركة، ويتضمن NumberSequenceId واحدًا؛ endpoint يقبل تسلسل أي فرع في الشركة، بينما PostAsync يتطلب التسلسل العام أو تسلسل الفرع الحالي | إذا اختير تسلسل خاص بفرع A في إعداد الشركة، لن يستطيع فرع B الترحيل بهذه السياسة، مع أن إعداد الشركة واحد | `ReceiptWorkspaceService.cs:11-17,139-142`؛ `ReceiptApiModule.cs:62-63,72-80` | دراسة سياسة نطاق الترقيم: عام أو اختيار لكل فرع؛ ثم إصلاح التحقق والواجهة دون تخمين قرار المالك |
| DATA-11 | متوسطة | Currency.MinorUnit يسمح 0..6، بينما خطة ترحيل القبض ترفض أكثر من 4 | إعداد عملة قانوني وفق نموذج العملة قد يُمنع عند ترحيل القبض | `TransportErpDbContext.cs:80-83`؛ `ReceiptPostingPlan.cs:11-15` | حسم موحد لدقة المبالغ وإظهار الحد قبل التشغيل |
| DATA-12 | متوسطة | سند القبض يحفظ تفاصيله في DocumentJson بالإضافة إلى أعمدة رأس مختصرة، والسياسات في JSON؛ أسطر السند ليست جدول EF مستقلًا | استخراج تقارير تفصيلية، تصفية الأسطر وربطها بالحسابات يحتاج استخدام العقد/الخدمة؛ لا تُعامل JSON كحقول موثقة تلقائيًا | `P1Entities.cs:234-254`؛ `ReceiptWorkspaceService.cs:69-93,254-259`؛ `ReceiptContracts.cs:3-9` | توثيق الهيكل والإصدارات وطريقة التقرير؛ لا تحويل إلى جداول قبل القاموس |
| DATA-13 | مرتفعة | سند P1 القديم دون DocumentJson مستبعد من قائمة receipts الحديثة ويتعذر فتحه بخدمة التفاصيل | توجد مشكلة توافق عند وجود سندات محفوظة بالخدمة القديمة؛ لا يشمل الحكم قاعدة المستخدم الفارغة أو المملوءة دون فحصها | `ReceiptApiModule.cs:84-88`؛ `ReceiptWorkspaceService.cs:254-257`؛ `VoucherLifecycleService.cs:39-69` | مسار تهيئة/استكمال بيانات قديمة موثق إن ثبت وجودها |
| DATA-14 | متوسطة | القيد يدعم FinancialDimensionId واحدًا لكل سطر؛ اختيار بُعد الترحيل من costCenter/project/activity واحد فقط | الحقول الثلاثة يمكن التحقق منها وحفظها في تفاصيل السند، لكن لا تتحول كلها إلى أبعاد قيد مستقلة | `P1Entities.cs:221-231`؛ `ReceiptPostingPlan.cs:13-21`؛ `ReceiptReferences.cs:11-17` | قرار محاسبي موثق لاحقًا؛ لا الادعاء بتعدد أبعاد كامل |
| DATA-15 | مرتفعة | ثلاثة مشاريع Mobile الحالية لا تحتوي إلا csproj؛ تنتقل إلى net10.0 Library عند عدم وجود MAUI scaffold، ولا ProjectReference إلى Contracts | ظهور المشاريع في الحل لا يثبت وجود تطبيق العميل أو السائق أو الإدارة، ولا مشاركة إعدادات التشغيل معها | `TransportERP.Mobile.Admin/TransportERP.Mobile.Admin.csproj:4-18,45-50`، ونفس المواضع في Customer/Driver؛ فحص `rg --files` للمجلدات الثلاثة أعاد الملفات الثلاثة فقط | تطبيقات حقيقية لاحقًا، بعقود API مشتركة ومخزن محلي بعد اعتماد السيناريوهات |
| DATA-16 | متوسطة | المفاتيح المرجعية CompanyId وBranchId في أغلب كيانات P1 مرتبطة كلٌّ منها مستقلًا، خلاف إعدادات الفرع ذات FK المركب | القاعدة وحدها لا تمنع كل علاقات الشركة/الفرع المتقاطعة؛ تعتمد على تحقق الخدمة | `TransportErpDbContext.cs:137-138,216-217,285-288,346-348`؛ تحقق القبض `ReceiptWorkspaceService.cs:210-215,240-247` | مراجعة كل خدمة فعليًا واختبارات نطاق؛ لا تعميم ثغرة على كل endpoints |
| DATA-17 | متوسطة | CashBoxId في P1 معرف اختياري دون كيان CashBox مستقل، والقرار التوثيقي يؤجل عقده؛ إعدادات القبض تمثل الوجهات بقائمة JSON مع AccountId | «وجهة قبض» حقيقية في إعدادات القبض لا تثبت إدارة صندوق/بنك كاملة بالعهدة والرصيد والصلاحيات لكل صندوق | `P1Entities.cs:252,271`؛ `ReceiptContracts.cs:15-23`؛ `documentation/architecture/P1_PHYSICAL_SCHEMA_POSTGRESQL.md:44-46` | اكتمال العقد المستقل للصندوق/البنك وربطه دون إنشاء FK زائف |
| DATA-18 | مرتفعة | مسار تعديل إعدادات القبض يكتب إعدادًا مشتركًا للشركة لكن التفويض له لا يطلب companyWide، على خلاف إعدادات GL؛ Allowed يسمح هنا للمستخدم المقيد بفرع ولتعيين صلاحية فرعي | مستخدم مخول بإعداد القبض في فرعه يمكنه تغيير سياسة القبض المشتركة لبقية الفروع؛ الحدود المقصودة لصلاحية configure يجب حسمها | `ReceiptApiModule.cs:42-43,72-80,129-131,150-169`؛ المقارنة `GeneralLedgerSettingsApi.cs:16-17` | إصلاح تفويض شركة إذا كانت السياسة شركة، أو فصل نطاق السياسة بعد اعتماد القرار؛ اختبار مستخدم فرعي |

تصنيف DATA-09 عيب كود محدد يمكن إصلاحه دون إعادة تصميم؛ DATA-10 وDATA-18 تعارض نطاق يحتاج قرارًا موثقًا ثم إصلاح الكود. باقي البنود خليط من نطاق لم يُنفذ وربط لم يكتمل وقرار عقدي مؤجل، ولا يصح اعتبار كل بند خطأ برمجيًا أو حذف حقول الشاشة لمعالجته.

## 4. ربط الإعدادات بين سطح المكتب والهواتف — التوصية التي يجب اعتمادها

**المقترح:** إعدادات العمل الأساسية تُدار من مصدر خادمي واحد وفق نطاق منصة/شركة/فرع، وتستهلكها كل واجهة حسب صلاحيتها. يتغير شكل عرض الإعدادات تبعًا للتطبيق؛ لا تُنشأ نسخة مستقلة متعارضة من إعدادات الشركة لكل تطبيق. السبب أن النموذج يملك بالفعل طبقات إعدادات مشتركة، وأن GL/Receipt المثبتين يستخدمان CompanySettings (`TransportErpDbContext.cs:186-217`؛ `GeneralLedgerPolicyService.cs:21,34-35`؛ `ReceiptWorkspaceService.cs:11-17`). هذا اقتراح معماري يحتاج اعتماد نطاقه، وليس وصفًا لتكامل Mobile الموجود.

| نوع الإعداد | المصدر المقترح | موضع الإدارة المقترح | ما يخص كل تطبيق |
|---|---|---|---|
| الشركة/الفرع/العملة الأساسية ودليل الحسابات | API ومراجع مركزية | سطح المكتب أو إدارة مخولة | تنزيل القيم المصرح بها، اختيار النطاق الحالي |
| سير المستند والتقريب والترقيم | سياسة شركة موثقة، واستثناء فرع إن اعتمد | واجهات إعدادات مخولة | تطبيق السياسة نفسها؛ حفظ snapshot للمستند |
| اللغة والمظهر والطباعة المحلية | تفضيل مستخدم/جهاز وفق قرار النطاق | التطبيق الذي يستعمل الجهاز | خيارات العرض والجهاز فقط |
| عنوان الخادم والمصادقة | ضبط اتصال لكل تثبيت، جلسة موثقة من API | الدخول/الاتصال | لا حفظ connection string PostgreSQL أو secret خادمي في الهاتف |
| العمل دون اتصال والمزامنة | عقد خادمي موحد، صلاحيات ونطاق | سياسة تشغيل مخولة | مخزن محلي وطابور عمليات خاص بالجهاز، مع إصدار السياسة |
| GPS | عقد أحداث/أذونات/تواتر وحماية بيانات مستقل | إعدادات تشغيل وتطبيق السائق | جمع الموقع بعد تعريف السيناريو؛ لا يعني وجود LAT/LON في سجل قديم تنفيذ GPS |

توثيق هذا الجدول اقتراح، لا توجد في هذه المهمة ملفات MAUI منفذة تسمح باختبار تنزيل هذه الإعدادات. دليل الاتصال الموجود اليوم يخص DesktopSession HTTPS وBearer فقط (`DesktopSession.cs:13-27,65-70`).

## 5. ترتيب التنفيذ الاستشاري دون البدء بتصميم قاعدة البيانات

1. **إقفال المرجع التأسيسي Onyx:** مطابقة شاشة/تبويب/حقل/زر/قاعدة بمصدر محدد وصفحة أو صورة، مع فصل قواعد النقل المضافة عن قواعد Onyx. يظل قاموس القاعدة وتصميمها متوقفين حتى اعتماد المالك.
2. **تثبيت واقع النسخة الأحدث:** سجل SHA ومصدر كل مشروع وكل شاشة، وفصل «واجهة جاهزة شكليًا» عن «خدمة مربوطة» و«اختبار ناجح اليوم». مقارنة فرع October بفرع September وعدم استبدال تصميم المستخدم.
3. **حسم النطاق المشترك:** قرارات مكتوبة لمنصة/شركة/فرع/مستخدم/جهاز، وترقيم الفرع، صلاحيات تغيير الإعدادات، دعم القديم، العملات والدقة، اختيار الأبعاد، الإعدادات المسموح بها offline.
4. **مصفوفة تطابق قبل القاموس:** الحقل الظاهر → اسم control → معنى العمل → DTO موجود → entity/JSON موجود → endpoint موجود → تحقق الحفظ → التقرير الذي يستهلكه → المصدر. لا اختيار نوع SQL أو تصميم جدول جديد في هذه الخطوة. تُسجّل DATA-03/05/06/12/17 بوصفها قرارات غير مغلقة.
5. **إصلاحات كود محددة في فرع مستقل:** تدقيق حفظ إعدادات القبض؛ التحقق من نطاق تسلسل القبض بعد القرار؛ توحيد رسائل الحدود والدقة؛ توثيق مسار السند القديم. لا دمج تلقائي إلى الفرع الرئيسي، ولا حذف/إعادة تصميم شاشات.
6. **قاموس بيانات ثم تصميم لاحقًا:** يبدأ فقط بعد اعتماد المرجع ونطاق المشروع. يستند إلى الكود الفعلي والسجلات القديمة كمصدر تاريخي، ولا يحول كل صف `LEGACY_REGISTERED` إلى جدول مطلوب تلقائيًا.
7. **ربط شاشات desktop حسب عائلات التصميم:** الشركات/الفروع؛ المراجع والتهيئة؛ إعدادات السياسة؛ المستندات المالية؛ البوالص والحركة. تُستخدم نفس عقود API، لا EF أو DB مباشرة في واجهة الهاتف.
8. **تطبيقات الهاتف وGPS:** scaffold حقيقي لكل جمهور، علاقة واضحة مع Contracts، مصادقة ونطاق؛ بعدها تنزيل إعدادات ومراجع، عمليات online، ثم offline/conflicts، ثم GPS بأدلة اختبار. لا تُختبر خدمة غير موجودة بوصفها ناجحة.
9. **قبول مستقل على البيئة المناسبة:** بناء Windows/MAUI، اختبارات قواعد ومراجع وعزل شركات وفروع وتزامن وعكس وتدقيق، واختبار migration في قاعدة مؤقتة بعد السماح بالمرحلة. النتائج القديمة لا تحل محل نتيجة SHA الحالي.

## 6. حدود الاختبار وأدلة الاختبارات الموجودة

لا توجد نتيجة اختبار جديدة من هذا المراجع. الوكيل الرئيسي أفاد بعدم توفر dotnet في البيئة. قاعدة البيانات لم تُفحص ولم تُشغّل.

في المصدر اختبارات مفيدة: العملات المختلطة وفارق التقريب وعدم إنشاء قيد عند فشل الترحيل (`TransportERP.Tests/ReceiptWorkspaceTests.cs:20-37`)، النطاق الخاطئ والنسخة القديمة والبوليصة المفقودة (`:185-196`)، والصلاحيات الحالية وسياسة الترحيل التلقائي (`TransportERP.Tests/LedgerWorkflowTests.cs:17-39`). لكن ReceiptWorkspaceTests يستخدم InMemory إلا عند ضبط `RECEIPT_ISOLATED_POSTGRES=54439`، حين ينشئ قاعدة اختبار ويطبّق Migrate (`ReceiptWorkspaceTests.cs:199-210`). لذلك نجاح اختبارات InMemory وحدها لا يثبت قيود PostgreSQL أو transactional behavior أو Migration.

اختبارات PostgreSQL العامة تتطلب صراحة متغير اختبار ولا تتخطى الغياب بصمت (`TransportERP.Tests/PostgreSqlTestEnvironment.cs:8-18`). smoke test يستدعي `db.Database.MigrateAsync()` (`TransportERP.Tests/PostgreSqlPersistenceSmokeTests.cs:13-16`). لم تُشغّل هذه الاختبارات احترامًا لتوقف إنشاء/تعديل القاعدة في هذه المرحلة.

**الحكم:** توجد نواة PostgreSQL حقيقية وسبق الفرع الحديث بتنفيذ ربط سند القبض وسياسة الأستاذ العام. المتبقي المثبت هو اكتمال مصادر المراجع والتهيئة والكيانات التشغيلية والربط الشامل والتطبيقات المحمولة، مع عيوب محددة في تدقيق إعدادات القبض ونطاق الترقيم. لا توجد أدلة تسمح بوصف المشروع كله مكتملًا أو قابلًا للتشغيل النهائي.
