# TransportERP — عقد المزامنة والتشغيل Online/Offline

**الإصدار:** P1-SYNC-CONTRACT-2026-08  
**المعرف الحاكم:** `W1-P1-017` / `W2-P1-015` / `W3-P1-012`  
**الحالة:** `G3_POLICY_ACCEPTED — RUNTIME_CONFORMANCE_PENDING_G4`؛ لا يمثل اعتماد السياسة تفويضًا لتفعيل Offline أو تنفيذًا إضافيًا.
**Decision ID:** `DEC-G3-SYNC-20260825-01`

**Exact baseline:** `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-25؛ لا يدعي هذا السجل توقيع أشخاص أو مراجعين غير مثبتين.

**تثبيت حوكمي — 2026-08-25:** اعتمد القرار المفوض أعمدة وسياسات Offline/Sync فقط في عقود W2 المشار إليها، ولا يقبل بقية حقول W2 أو يغير حالة مراجعتها أو يفوض runtime أو مرحلة P2 لاحقة. يغطي القرار allowlist Payload Actions، وإعادة المحاولة وBackoff، وسياسة التعارض، والاحتفاظ، وحجم الدفعة وذريتها، وصلاحيات حل التعارض، وتسلسل الإعدادات. يبقى `sync.offline.enabled=false` حتى تثبت مطابقة التنفيذ واجتياز G4 ثم يصدر تفويض G5.

## 1. الغرض

يعرّف هذا العقد كيفية إنشاء عمليات محلية عند انقطاع الاتصال، وإرسالها لاحقًا، ومنع التكرار، والتحقق من نطاق الشركة والفرع، واكتشاف التعارض، وإعادة المحاولة، وحفظ التدقيق. صُمم `SyncOperation` جديدًا لأن مراجعة المصدر المرجعي لم تجد كيانًا مماثلًا في AlTayerERP؛ لذلك لا توجد إعادة استخدام تلقائية.

> المبدأ الحاكم: **المزامنة لا تعني قبول العملية**. العملية المحلية تحمل حالة مؤجلة، ويعيد الخادم قرارًا صريحًا: قبول، رفض، فشل قابل لإعادة المحاولة، أو تعارض يحتاج معالجة.

## 2. سجل العملية

| الحقل | النوع/القيد | الغرض |
|---|---|---|
| `Id` | UUID | معرف العملية على الخادم. |
| `DeviceId` | نص مطلوب | هوية الجهاز المسجل، لا يعتمد على اسم المستخدم وحده. |
| `UserId` | UUID مطلوب | صاحب العملية المحلي. |
| `CompanyId` | UUID مطلوب | نطاق الشركة الذي ستطبّق عليه العملية. |
| `BranchId` | UUID اختياري | الفرع، مع إعادة التحقق على الخادم. |
| `ActionCode` | نص مطلوب ومقيد بالـallowlist | الفعل التعاقدي المقصود؛ لا يستنتج من `OperationType` أو `EntityType`. |
| `ProtocolVersion` | نص مطلوب | نسخة بروتوكول ضمن `sync.protocol.allowed_versions`؛ القيمة الأولية `sync-v1`. |
| `OperationType` | CREATE/UPDATE/DELETE/COMMAND | نوع العملية. |
| `EntityType` | نص مطلوب | نوع الكيان المستهدف وفق ActionCode. |
| `EntityId` | UUID مشروط | مطلوب للكيان/aggregate الموجود؛ اختياري فقط في `CREATE` الذي يولد الخادم هويته. |
| `ClientOperationId` | نص مطلوب وفريد لكل جهاز | مفتاح idempotency الأساسي. |
| `RequestCorrelationId` | UUID مطلوب | ترابط طلب العميل والدفعة والتدقيق، ويحفظ مع النتيجة ولا يستبدل ClientOperationId. |
| `PayloadJson` و`PayloadHash` | JSON + SHA-256 أو مكافئ | الحمولة وسلامتها أثناء النقل. |
| `ClientOccurredAt` | وقت الجهاز | وقت الإنشاء المحلي، لا يُستخدم وحده للحسم المالي. |
| `ServerReceivedAt` | وقت الخادم | وقت الاستلام الحاكم للتدقيق. |
| `BaseVersion` | رقم إصدار مشروط | إلزامي لـoptimistic aggregate mutation مثل `UpdateWaybillDraft`؛ غير مطلوب لـCREATE أو append-only actions المحكومة بـClientOperationId وserver serialization/domain state. |
| `ResultEntityId` | UUID اختياري قبل التنفيذ، مطلوب عند النجاح الذي ينشئ/يعيد كيانًا | هوية الخادم الناتجة وخريطة local↔server. |
| `ResultVersion` | رقم الإصدار الناتج | الإصدار بعد القبول عند وجود aggregate versioned. |
| `Status` | حالات المزامنة | نتيجة المعالجة. |
| `RetryCount` و`NextRetryAt` | أعداد/وقت | التحكم في إعادة المحاولة. |
| `ErrorCode` | رمز اختياري | سبب الرفض أو الفشل. |
| `ConflictCaseId` | UUID اختياري | ربط العملية بملف تعارض. |
| `RowVersion` | إصدار تنافسي | منع الكتابة المتزامنة على سجل العملية. |

خريطة الهوية الحاكمة هي `(DeviceId, ClientOperationId, ActionCode) → ResultEntityId`. في CREATE ذي الهوية الخادمية يجوز أن يكون `EntityId` فارغًا، لكن يجب على نتيجة `SUCCEEDED` إعادة `ResultEntityId` وحفظ الخريطة؛ يعيد replay الخريطة نفسها. لا يجوز للعميل إنشاء سجل ثانٍ لتعويض نتيجة ضائعة.

## 3. الحالات والانتقالات

| الحالة | المعنى | الانتقال المسموح |
|---|---|---|
| `QUEUED` | عملية محفوظة محليًا أو على الخادم ولم ترسل بعد | `SENDING`, `REJECTED` بعد فحص محلي |
| `SENDING` | قيد الإرسال، ولا يجوز تشغيل نسخة ثانية من نفس المفتاح | `SUCCEEDED`, `FAILED`, `CONFLICT`, `REJECTED` |
| `SUCCEEDED` | قبل الخادم العملية وأعاد الإصدار الجديد | حالة نهائية مع سجل تدقيق |
| `FAILED` | فشل تقني أو مؤقت | `SENDING` بعد Backoff، أو `REJECTED` بعد استنفاد السياسة |
| `CONFLICT` | الإصدار الأساسي لم يعد مطابقًا أو قاعدة عمل متعارضة | `RESOLVED` أو `REJECTED` بعد قرار مخول |
| `REJECTED` | العملية غير صالحة أو غير مسموحة | حالة نهائية، مع سبب قابل للعرض |
| `RESOLVED` | عولج التعارض بقرار صريح | حالة نهائية مع رابط العملية/الإصدار البديل |

لا يجوز الانتقال إلى `SUCCEEDED` إذا فشل فحص الصلاحية أو النطاق أو Hash أو idempotency أو حالة الكيان. ولا يجوز اعتبار timeout سببًا لقبول العملية؛ يجب الاستعلام بمفتاح `DeviceId + ClientOperationId` قبل إعادة إنشاء العملية.

## 4. سياسة Idempotency وإعادة المحاولة

يولد العميل `ClientOperationId` مرة واحدة ولا يغيره عند إعادة الإرسال. يبحث الخادم أولًا عن الزوج الفريد `DeviceId + ClientOperationId`. إذا وجد عملية ناجحة يعيد النتيجة نفسها، وإذا وجد عملية قيد المعالجة يعيد حالة المعالجة، وإذا وجد عملية مرفوضة يعيد سبب الرفض، ولا ينشئ أثرًا ثانيًا.

تستخدم الأخطاء التقنية المؤقتة Backoff تصاعديًا مع حد أقصى وعدد محاولات يحدد في إعداد المنصة. لا تعاد محاولة أخطاء التحقق أو نقص الصلاحية أو Hash غير المطابق. وتظهر العملية في شاشة المزامنة بعدد المحاولات والسبب والوقت القادم، مع إمكانية إعادة المحاولة اليدوية لمن يملك الصلاحية.

## 5. سياسة التعارض

يُكتشف التعارض عندما يرسل الجهاز `BaseVersion` مختلفًا عن إصدار الخادم، أو عندما تتعارض العملية مع انتقال حالة أحدث، أو عندما يغير مستخدم آخر نفس السجل. لا يحل النظام التعارض بالكتابة فوق أحدث قيمة بصمت.

| نوع التعارض | القرار الافتراضي | سبب القرار |
|---|---|---|
| تعديل حقلين مستقلين في سجل غير مالي | `CONFLICT` في السياسة الأولية؛ الدمج المشروط مؤجل لتغيير حوكمي واختبارات مستقلة | يمنع دمجًا غير مثبت أو فقدانًا صامتًا |
| تعديل نفس الحقل | `CONFLICT` يحتاج قرارًا مخولًا | لا توجد أولوية عامة مفترضة |
| قيد أو سند مرحّل | رفض التعديل، واستخدام عكس/إصدار تغيير | الأثر المالي غير قابل للكتابة فوقه |
| حالة بوليصة/تذكرة/رحلة تغيرت بعد العمل المحلي | `CONFLICT` أو رفض حسب الحالة | منع التسليم/الترحيل المزدوج |
| عملية حذف مع وجود تبعيات | رفض مع سبب | الحذف ليس تجاوزًا لقواعد العلاقات |
| تعارض نطاق شركة/فرع | رفض أمني | لا يملك الجهاز نقل العملية إلى نطاق آخر |

يجب أن يحتوي `ConflictCase`، حتى إن لم يكن جزءًا من P1 W1، على العملية الأصلية، والسجل الحالي، و`BaseVersion` عندما ينطبق على Action، ونسخة الجهاز، والقرار، والمقرر، والسبب، ووقت الحسم، وأي عملية بديلة.

## 6. سياسة Online/Offline حسب فعل P1

| الفعل | Offline | السياسة |
|---|---|---|
| تسجيل الدخول | لا | يتطلب الخادم، مع عدم تخزين كلمة المرور. |
| إدارة المستخدمين والصلاحيات | لا | تغييرات الهوية والصلاحيات حساسة وتحتاج اتصالًا. |
| إعدادات المنصة والشركة والفرع | لا | تمنع اختلاف سياسات الجهاز عن الخادم. |
| دليل الحسابات والفترات والعملات والأبعاد | قراءة مخزنة اختياريًا؛ كتابة لا | يمكن عرض نسخة مؤرخة، ولا يعتمد تغيير مرجعي دون خادم. |
| إنشاء قيد يومي | نعم للمسودة فقط | تحفظ محليًا، ولا يعتمد أو يرحّل إلا Online. |
| ترحيل/عكس القيد | لا | أثر مالي نهائي يحتاج فترة وصلاحية وخادمًا. |
| سند قبض/صرف | مسودة فقط | يمنع إصدار سند نهائي أو تحصيل نهائي Offline. |
| قراءة التدقيق | لا | المصدر الحاكم هو سجل الخادم. |
| مزامنة العمليات | طابور محلي ثم Online | كل عملية لها نتيجة منفصلة ولا تقبل الكتلة ككل تلقائيًا. |

## 7. دفعة المزامنة API

يقبل `POST /api/v1/sync/operations:batch` قائمة محدودة الحجم من العمليات، مع `DeviceId` و`ProtocolVersion=sync-v1` و`RequestCorrelationId`. يرث كل SyncOperation هذه القيم ويحفظها. يعالج الخادم كل عملية بمعاملة مستقلة؛ والقرار هو **النتيجة لكل عملية** حتى لا تمنع عملية تالفة بقية العمليات السليمة.

يعيد الرد لكل عملية: `ClientOperationId`, `ServerOperationId`, `ActionCode`, `ResultEntityId`, `Status`, `ResultVersion`, `ErrorCode`, `ConflictCaseId`, `RequestCorrelationId`, و`ServerTime`. لا يعيد النظام Payload حساسًا في رسالة الخطأ، ويمنع تسريب وجود حساب أو مستند خارج النطاق.

## 8. التدقيق والأمن

تسجل كل مراحل الاستلام والقبول والرفض والتعارض وإعادة المحاولة والحسم في `AuditEvent` مع المستخدم والجهاز والشركة والفرع وCorrelation ID وHash. لا يثق الخادم في `CompanyId` أو `BranchId` المرسلين من الجهاز دون التحقق من جلسة المستخدم وتسجيل الجهاز.

تُشفّر البيانات الحساسة أثناء النقل والتخزين وفق سياسة البنية التحتية المعتمدة، ولا تخزن كلمات المرور أو أسرار التكامل في طابور Offline. فترات الاحتفاظ المعتمدة مثبتة في القسم 16 وتتحول إلى إعدادات رسمية وفق تسلسل القسم 17.

## 9. اختبارات قبول العقد

| المعرف | الاختبار | النتيجة المطلوبة |
|---|---|---|
| `T-SYNC-001` | إرسال عملية جديدة بمفتاح فريد | قبول واحد وإصدار خادم واحد. |
| `T-SYNC-002` | إعادة إرسال نفس العملية بعد timeout | نفس النتيجة دون أثر مكرر. |
| `T-SYNC-003` | إرسال Hash غير مطابق | رفض مع `HASH_MISMATCH` وتدقيق. |
| `T-SYNC-004` | إرسال عملية خارج Company/Branch | رفض أمني دون كشف بيانات. |
| `T-SYNC-005` | BaseVersion قديم لـoptimistic aggregate mutation مثل `UpdateWaybillDraft` | `CONFLICT` دون فقدان صامت؛ `auto_merge=false` في baseline. |
| `T-SYNC-006` | تعديل قيد مرحّل Offline | لا يسمح إلا بمسودة؛ الترحيل يرفض Offline. |
| `T-SYNC-007` | فشل تقني مؤقت | إعادة Backoff، عداد ومحاولة قادمة، دون تكرار. |
| `T-SYNC-008` | تعارض يحتاج قرارًا | يظهر في شاشة المزامنة مع المقرر والسبب والنتيجة. |
| `T-SYNC-009` | عمليتان متوازيتان لنفس السجل | واحدة تقبل والأخرى تتعارض أو ترفض بإصدار واضح. |
| `T-SYNC-010` | إعادة تشغيل الجهاز بعد انقطاع الكهرباء | يستعيد الطابور دون فقد أو تكرار. |

## 10. بوابة الاعتماد

اعتمدت قرارات G3 في 2026-08-25، فأصبحت حالة السياسة `G3_POLICY_ACCEPTED — RUNTIME_CONFORMANCE_PENDING_G4`. يظل التفعيل محظورًا بواسطة `sync.offline.enabled=false` حتى يثبت G4 مطابقة التطبيق والعميل وقاعدة البيانات والاختبارات لكل قرار أدناه، ثم يعتمد المالك G5. لا يجوز اعتبار اعتماد السياسة دليل تنفيذ أو تفويض Migration أو API أو عميل Offline أو تشغيل إنتاجي.

## 11. قائمة Offline الحصرية

المبدأ هو `deny by default`. لا يقبل الخادم أي كتابة Offline لمجرد أن `OperationType` أو `EntityType` صالحان تركيبيًا؛ يجب أن يطابق الطلب Action صريحًا في القائمة التالية، وأن يجتاز تسجيل الجهاز وصلاحية المستخدم ونطاق الشركة/الفرع وحالة الكيان والتحقق وHash وidempotency.

`SyncP1Operations` هو transport capability/endpoint لإرسال الدفعة، وليس Payload `ActionCode` ولا يدخل `sync.offline.allowed_actions`. لا يقبل transport إلا `ProtocolVersion=sync-v1` في القرار الأولي، ثم يفحص كل Payload Action مستقلًا.

Read-cache منفصل عن write queue ولا ينشئ `SyncOperation`. الاسمان الوحيدان المعتمدان هنا هما `SearchOperationalParties` و`ReadBasicWaybillCache`. لا تثبت بياناتهما صلاحية حالية أو حالة نهائية. أما قراءة Chart/FiscalPeriod/Currency/Dimension المذكورة تاريخيًا فتظل وصفًا اختياريًا بلا ActionCode صادر، ولا يضيف هذا القرار لها runtime أو cache authority.

### 11.1 Action mapping الحاكم

| ActionCode | Class | EntityId | BaseVersion | ResultEntityId عند النجاح | Runtime availability على baseline |
|---|---|---|---|---|---|
| `CreateJournalEntry` | Write queue / draft CREATE | اختياري فقط إذا ولّد الخادم ID | غير مطلوب | مطلوب؛ JournalEntryId | `OFFLINE_DISPATCH_UNAVAILABLE`؛ لا API/dispatcher إنتاجي مثبت |
| `CreateReceiptVoucher` | Write queue / draft CREATE | اختياري فقط إذا ولّد الخادم ID | غير مطلوب | مطلوب؛ ReceiptVoucherId | `OFFLINE_DISPATCH_UNAVAILABLE`؛ service موجود بلا sync dispatch مثبت |
| `CreatePaymentVoucher` | Write queue / draft CREATE | اختياري فقط إذا ولّد الخادم ID | غير مطلوب | مطلوب؛ PaymentVoucherId | `OFFLINE_DISPATCH_UNAVAILABLE`؛ service موجود بلا sync dispatch مثبت |
| `CreateWaybillDraft` | Write queue / draft CREATE | اختياري فقط إذا ولّد الخادم ID | غير مطلوب | مطلوب؛ WaybillId | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `UpdateWaybillDraft` | Write queue / optimistic aggregate UPDATE | مطلوب؛ WaybillId | **مطلوب** | مطلوب ويساوي WaybillId | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `CreateOperationalParty` | Write queue / CREATE | اختياري فقط إذا ولّد الخادم ID | غير مطلوب | مطلوب؛ OperationalPartyId | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `AddWaybillAttachment` | Metadata queue / append CREATE | مطلوب؛ owner WaybillId | غير مطلوب | مطلوب؛ Attachment metadata ID | `PHASE_RUNTIME_UNAVAILABLE`; metadata فقط، لا binary upload |
| `RecordCollection` | Append-only business command | مطلوب؛ WaybillId | غير مطلوب؛ يعتمد ClientOperationId + server state/serialization | مطلوب؛ CollectionTransactionId | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `LoadAllocatedQuantity` | Append-only quantity command | مطلوب؛ ManifestLineId | غير مطلوب؛ يعتمد ClientOperationId + serialized quantity/domain state | مطلوب؛ load/movement result ID | `ONLINE_RUNTIME_PRESENT; OFFLINE_DISPATCH_UNAVAILABLE` |
| `RecordArrival` | Append-only business command | مطلوب؛ TripId | غير مطلوب؛ يعتمد ClientOperationId + server serialization/domain state | مطلوب؛ ArrivalReceiptId | `PHASE_RUNTIME_UNAVAILABLE` |
| `RecordUnload` | Append-only quantity command | مطلوب؛ ArrivalReceiptId | غير مطلوب؛ يعتمد ClientOperationId + serialized quantity/domain state | مطلوب؛ unload/movement result ID | `PHASE_RUNTIME_UNAVAILABLE` |
| `DeliverQuantity` | Append-only quantity command | مطلوب؛ WaybillId | غير مطلوب؛ يعتمد ClientOperationId + serialized availability/domain state | مطلوب؛ DeliveryId | `PHASE_RUNTIME_UNAVAILABLE` |
| `RecordProofOfDelivery` | Metadata queue / append CREATE | مطلوب؛ DeliveryId | غير مطلوب | مطلوب؛ proof metadata ID | `PHASE_RUNTIME_UNAVAILABLE`; metadata فقط، لا binary upload |
| `CreateShipmentException` | Append-only business command | مطلوب؛ WaybillId | غير مطلوب؛ يعتمد ClientOperationId + domain state | مطلوب؛ ShipmentExceptionId | `PHASE_RUNTIME_UNAVAILABLE` |
| `SearchOperationalParties` | Read-cache; لا SyncOperation | لا ينطبق | لا ينطبق | لا ينطبق | Online source موجود؛ client cache runtime غير مثبت |
| `ReadBasicWaybillCache` | Read-cache; لا SyncOperation | مطلوب؛ WaybillId | لا ينطبق | لا ينطبق | `RUNTIME_UNAVAILABLE` على baseline |

إذا كان Action مسموحًا في policy لكن حالته التنفيذية ليست Offline-available على الإصدار الجاري، يرفض الخادم قبل enqueue بـ`ACTION_RUNTIME_UNAVAILABLE`. لا يحول إدراجه في الجدول عقد W2 كاملًا إلى مقبول ولا يفتح مرحلة P2 مقفلة. كل Action غير مذكور `ONLINE_REQUIRED`، ولا يسمح بأي generic `DELETE` Offline.

بالنسبة لـ`AddWaybillAttachment` و`RecordProofOfDelivery`، يحمل Payload metadata وcontent hash وlocal correlation فقط. لا يحمل binary ولا يفعّل رفعه؛ يبقى binary upload runtime مؤجلًا حتى يصدر عقد resumable upload/hash/size/storage/failure مستقل ويجتاز G4.

## 12. قرار إعادة المحاولة وBackoff

| Budget | Counter owner | الحد بعد المحاولة الأصلية | Default base | Default effective schedule | Max delay |
|---|---|---:|---:|---|---:|
| Client transport delivery | durable client queue؛ `ClientTransportRetryCount` | `5` | `5 seconds` | `5s, 10s, 20s, 40s, 80s` | `30 minutes` |
| Server business execution | server SyncOperation؛ `RetryCount`/`ServerExecutionRetryCount` | `5` | `5 seconds` | `5s, 10s, 20s, 40s, 80s` | `30 minutes` |

العدادان مستقلان. client transport counter يحسب محاولات إيصال HTTP/Batch ذات النتيجة المجهولة أو `INTERNAL_ERROR` أو `RATE_LIMITED`. server execution counter لا يزيد عند replay/duplicate enqueue؛ يزيد فقط عندما يحاول المعالج تنفيذ business action المقبول ويفشل بخطأ retryable. تمنع idempotency أي أثر مكرر عبر الميزانيتين، وتعيد duplicate client submission نفس `ServerOperationId/ResultEntityId/outcome`.

الجدول `5/10/20/40/80` هو **default الفعال** عندما يكون effective base=`5s`. إذا شدد Company/Branch/Device النطاق برفع base delay، يعاد حساب `min(maxDelay, effectiveBase × 2^(attempt-1))`؛ لذلك لا يدعى ثبات الجدول بعد override. لا تبدأ محاولة قبل `NextRetryAt`، والمحاولة اليدوية تستهلك budget الخادمي نفسه. بعد استنفاد client budget تتطلب العملية تدخلًا/اتصالًا صريحًا؛ وبعد استنفاد server budget تصبح `REJECTED` مع `RETRY_EXHAUSTED`.

أخطاء validation أو authentication أو permission أو scope أو device أو Hash أو idempotency mismatch أو invalid state أو business rule ليست قابلة لإعادة المحاولة؛ و`CONFLICT` يذهب إلى سياسة التعارض. إعادة المحاولة الخادمية المحفوظة تلقائيًا محصورة في `RATE_LIMITED` في baseline حتى يصدر تصنيف أخطاء أوسع ويختبر.

## 13. قرار Batch والذرية

- حجم الدفعة من `1` إلى `100` عملية؛ وخلاف ذلك `BATCH_SIZE_INVALID`.
- الذرية لكل عملية فقط. يسمح بالنجاح الجزئي، ولكل عنصر status/error مستقل، ولا تُرجع عملية ناجحة بسبب فشل عنصر آخر.
- لا توجد atomic group عابرة للعناصر، ولا يعد ترتيب عناصر الدفعة ضمان ترتيب business commits.
- عند ضياع استجابة الدفعة، تعاد العناصر غير المعلومة أو الدفعة نفسها باستخدام IDs وHashes الأصلية؛ لا تنشأ IDs بديلة.
- يجب أن تكون نسخة البروتوكول ضمن allowlist خادمية؛ القرار الأولي هو `sync.protocol.allowed_versions=["sync-v1"]`. يرفض إصدار آخر قبل enqueue بـ`PROTOCOL_VERSION_UNSUPPORTED`، وتحفظ `sync-v1` في كل SyncOperation ناتجة عن الدفعة.

## 14. سياسة التعارض النهائية

القيمة الأولية هي `sync.conflict.auto_merge=false`. لا توجد كتابة `last-write-wins` ولا `USE_DEVICE_OVERWRITE`.

- `BaseVersion` إلزامي فقط لفعل optimistic aggregate mutation، مثل `UpdateWaybillDraft` وأي UPDATE مماثل على سجل mutable. لا يلزم لأفعال append-only مثل Collection/Load/Arrival/Unload/Delivery/POD/Exception؛ تحكمها `ClientOperationId` وserver serialization وdomain state. يظل generic `DELETE` ممنوعًا Offline.
- stale draft أو stale non-final record يصبح `CONFLICT` مع Device/Server snapshots.
- تعارض نفس الحقل أو أي حقل مالي أو كمية أو حيازة أو هوية أو حالة يحتاج قرارًا يدويًا، ولا يدمج تلقائيًا.
- السجل approved/posted/finalized/settled/closed، أو الانتقال غير القانوني، أو delete ذي تبعيات، أو scope mismatch يصبح `REJECTED`. يعالج الأثر النهائي بأمر Online للتصحيح أو العكس، لا بالكتابة فوقه.
- `KEEP_SERVER_AND_REJECT_LOCAL`: تصبح العملية الأصلية `REJECTED`، ويصبح ConflictCase `RESOLVED` مع القرار والسبب والمقرر والتوقيت.
- `REAPPLY_AS_NEW`: ينشئ الخادم أولًا replacement مستقلًا بحالة `QUEUED` ويربطه بـ`ReplacedByOperationId`؛ بعدها تصبح العملية الأصلية `RESOLVED` بنتيجة `SUPERSEDED` ويصبح ConflictCase `RESOLVED`. تخضع العملية البديلة لكل التحققات والـbudgets من جديد.
- فشل replacement لاحقًا لا يعيد فتح العملية أو ConflictCase القديمين؛ يظهر failure/conflict جديدًا مستقلًا مرتبطًا بسلسلة الاستبدال.
- لا يفتح `MERGE_DISJOINT_DRAFT_FIELDS` إلا بتغيير حوكمي لاحق واختبارات مستقلة تثبت مقارنة الحقول والتدقيق.

## 15. صلاحيات حل التعارض

يشترط كل حل `sync.conflicts.resolve` مع صلاحية الفعل الأصلي، وجهازًا مسجلًا، ونطاق Company/Branch مطابقًا، وسببًا إلزاميًا. لا تكفي `sync.operations.execute` وحدها.

- نص/مسودة غير مالية: clerk المختص يعيد التطبيق كعملية جديدة بعد مراجعة حالة الخادم.
- كمية أو حيازة: Operations Supervisor أو الدور المعيّن للسياسة.
- Collection: Finance/Cashier authority.
- Delivery/POD: Delivery Supervisor.
- P1 accounting drafts: صاحب صلاحية create المعنية مع `sync.conflicts.resolve`؛ لا يملك الاعتماد أو الترحيل Offline.
- Numbering/approval/posting/reversal/finalization/settlement/financial close/reopen: لا حل Offline؛ يعاد الأمر Online.

يجب أن يكون Resolution code قيمة مقيدة، والسبب نصًا منفصلًا مدققًا. يجب أن تشير العملية البديلة إلى نفس النطاق والكيان وأن تحتفظ بالسلسلة الأصلية.

## 16. سياسة الاحتفاظ

| البيانات | القرار المعتمد |
|---|---|
| Local `SUCCEEDED` أو `RESOLVED` | حذف Payload بعد verified server acknowledgement وفترة سماح `24 hours` |
| Local `REJECTED` | الاحتفاظ بالـPayload `7 days` لعرض السبب/التصحيح ثم حذفه؛ تبقى metadata/hash/result |
| Local non-terminal | تشفيرها حتى الحالة النهائية؛ عند عمر `7 days` يمنع الجهاز عمليات Offline جديدة ويطلب sync/escalation؛ لا حذف صامت لتحصيل أو POD غير acknowledged |
| POD/identity binary | حذف محلي بعد verified upload وفترة سماح `24 hours`؛ لا يدخل read cache |
| Non-sensitive read cache | حد أقصى `24 hours`؛ لا يستخدم لإثبات permission أو final state |
| Server PayloadJson وConflict snapshots | حذف بعد `90 days` من terminal/resolution؛ لا تحذف open conflicts |

تبقى IDs وHashes والحالات والأوقات وAuditEvent وفق سياسة Audit/Legal Hold المنفصلة. يسبق Legal Hold الحذف الخادمي، لكنه لا يجيز إبقاء نسخة محلية غير ضرورية.

بعد انتهاء retention يصبح `PayloadJson=NULL` إن سمح المخطط، أو قيمة redacted ثابتة لا تحتوي أي بيانات أعمال إلى أن تكتمل migration nullable؛ ويبقى `PayloadHash` وmetadata. تعامل `DeviceSnapshot` و`ServerSnapshot` بالطريقة نفسها بعد 90 يومًا من resolution. يجب ألا تعيد API أو audit أو logs Payload المحذوف، وتثبت اختبارات الحدود nullability/redaction وعدم قابلية استرجاع المحتوى. لا يشمل retention binary attachment/POD لأن binary upload غير مفعل أصلًا في هذا القرار.

## 17. تسلسل الإعدادات وFail-closed

Global platform policy هو السقف. يجوز لـCompany ثم Branch التضييق فقط؛ وتتقاطع Device policy وcurrent role/permission أخيرًا ولا توسع النطاق.

- `effectiveAllowedActions = Global ∩ Company ∩ Branch ∩ Device ∩ Permissions`.
- `enabled` يحسب بـAND.
- client/server max retry counts، وbatch وcache وretention exposure تحسب بالأقل `MIN`.
- client/server `baseDelay` و`maxDelay` يحسبان بالأكبر `MAX` لمنع نطاق أدنى من زيادة معدل الحمل، ثم يعاد حساب exponential schedule من القيمة الفعالة.
- غياب Company/Branch override يعني fallback بالترتيب `Branch → Company → Global`.
- قيمة lower override غير الصالحة تعطل Offline لذلك النطاق؛ لا fallback صامت.
- كل تغيير إعداد Online-only وversioned ومدقق مع effective source.

| المفتاح | القيمة الابتدائية |
|---|---|
| `sync.offline.enabled` | `false` حتى G4/G5 |
| `sync.offline.allowed_actions` | القائمة الحصرية في القسم 11 |
| `sync.protocol.allowed_versions` | `["sync-v1"]` |
| `sync.retry.client_transport.max_count` | `5` |
| `sync.retry.client_transport.base_seconds` | `5` |
| `sync.retry.client_transport.max_delay_minutes` | `30` |
| `sync.retry.server_execution.max_count` | `5` |
| `sync.retry.server_execution.base_seconds` | `5` |
| `sync.retry.server_execution.max_delay_minutes` | `30` |
| `sync.batch.max_operations` | `100` |
| `sync.conflict.auto_merge` | `false` |
| `sync.retention.local_success_hours` | `24` |
| `sync.retention.local_rejected_days` | `7` |
| `sync.retention.server_payload_days` | `90` |
| `sync.cache.max_age_hours` | `24` |

## 18. شروط المطابقة في G4

لا يفعّل `sync.offline.enabled` قبل إثبات الآتي على exact SHA:

1. enforce `ActionCode` allowlist و`sync-v1` قبل enqueue، ورفض unknown action/entity وgeneric `DELETE`.
2. التحقق من سجل جهاز حقيقي وحالة الإلغاء، لا الثقة في claim منفردة.
3. فصل `sync.conflicts.resolve` عن `sync.operations.execute` وتطبيق مصفوفة domain roles.
4. تطبيق قواعد `EntityId` و`BaseVersion` لكل Action، وحفظ `ProtocolVersion`, `RequestCorrelationId`, `ResultEntityId` وخريطة ClientOperationId↔server entity.
5. اكتشاف BaseVersion conflict فقط للأفعال optimistic وتقييد Resolution codes وتسلسل KEEP_SERVER/REAPPLY كما هو معتمد.
6. API/worker فعليان للretry/conflict مع عدادي client/server منفصلين وsingle-claim concurrency واستعادة بعد restart.
7. cleanup/retention worker وطابور عميل durable ومشفر، مع redaction/nullability مثبتة.
8. اختبار allowlist مستقل table-driven **لكل Action** في 11.1، واختبار `ACTION_RUNTIME_UNAVAILABLE` لكل Action غير متاح في الإصدار، وعدم خلط read-cache بالwrite queue.
9. اختبارات `T-SYNC-001..010` كاملة، وbatch `0/1/100/101` والنجاح الجزئي، وجدولي retry الافتراضيين والاستنفاد وعدم زيادة server counter بالreplay، وصلاحيات التعارض، وحدود `24h/7d/90d` وتسلسل الإعدادات وinvalid fail-closed.
10. إثبات أن Attachment/POD queue metadata-only وأن binary يرفض قبل عقد resumable/hash، واختبار أن PayloadJson وconflict snapshots تصبح NULL/redacted بعد retention ولا تسرب عبر API/audit/logs.
