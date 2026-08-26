# TransportERP — عقد المزامنة والتشغيل Online/Offline

**الإصدار:** P1-SYNC-CONTRACT-2026-08  
**المعرف الحاكم:** `W1-P1-017` / `W2-P1-015` / `W3-P1-012`  
**الحالة:** `G3_POLICY_ACCEPTED — STAGE4_SYNC_POP_POLICY_ACCEPTED — RUNTIME_CONFORMANCE_PENDING_G4`؛ لا يمثل اعتماد السياسة تفويضًا لتفعيل Offline أو ادعاء اكتمال التنفيذ.
**Decision ID:** `DEC-G3-SYNC-20260825-01`

**G3 exact baseline:** `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

**Stage 4 implementation baseline reviewed:** remote implementation `5ca9a86acef4053cf731fb896ca0c77b17a575ae` / tree `9ce37459c22c6f4c47beee203af0fa9d0a167080`.
**Stage 4 initial contract record:** remote docs commit `9d9ba3fe158eab6307ea0860e942d6cd56c273d2`.

**Stage 4 normative amended contract:** remote commit `63057d0f8b47ebf10c935b7349aeeb67128ff412` / tree `db5cbe92db5c467c48699146829ab72e21865517`، وقد نجحت CI #16 عليه. هذه الإضافة توثق البصمة فقط ولا تغير أحكام الأقسام 19.1–19.12 المثبتة في ذلك commit.
**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-25؛ لا يدعي هذا السجل توقيع أشخاص أو مراجعين غير مثبتين.

**تثبيت حوكمي — 2026-08-25:** اعتمد القرار المفوض أعمدة وسياسات Offline/Sync فقط في عقود W2 المشار إليها، ولا يقبل بقية حقول W2 أو يغير حالة مراجعتها أو يفوض runtime أو مرحلة P2 لاحقة. يغطي القرار allowlist Payload Actions، وإعادة المحاولة وBackoff، وسياسة التعارض، والاحتفاظ، وحجم الدفعة وذريتها، وصلاحيات حل التعارض، وتسلسل الإعدادات. يبقى `sync.offline.enabled=false` حتى تثبت مطابقة التنفيذ واجتياز G4 ثم يصدر تفويض G5.

**قيد Stage 3 للأجهزة:** credential التثبيت العشوائي per-install ليس request-bound ولا replay-resistant؛ لا يعد وحده تفويضًا للمزامنة. يحفظ العميل السر في OS secure storage ولا يخزن الخادم إلا SHA-256، ولا ينقل السر إلا عبر TLS بوصفها قاعدة نشر/بوابة موثوقة لا ادعاء enforcement داخل التطبيق الحالي. يعيد Runtime في Stage 3 `OFFLINE_DISABLED` دائمًا، حتى لو كانت قيمة الإعداد `true`. لا يفتح المسار قبل Stage 4 الذي يثبت freshness/nonce وrequest-level proof-of-possession أو sender constraint مكافئًا واختبارات replay. لا يغير Stage 3 مفتاح idempotency التاريخي `(DeviceId, ClientOperationId)`؛ المرشح المستقبلي المرتبط بالطلب يحتاج قرار عقد ومهاجرة مستقلة ولا يستنتج هنا.

**قرار Stage 4 الحاكم — 2026-08-26:** اعتمد المشروع `TransportERP Sync-PoP v1` وفق القسم 19، بوصفه profile خاصًا بمسار دفعة المزامنة ومشتقًا من قواعد DPoP في RFC 9449. القرار لا يدعي أن Access Token الحالي أصبح OAuth DPoP-bound token؛ يبقى `Authorization: Bearer` في هذه المرحلة، ويضاف إثبات حيازة غير متماثل ومربوط بالجهاز والطلب والـBearer token. لا يُرسل `DeviceCredential` داخل دفعة المزامنة بعد تفعيل هذا profile. ويبقى `sync.offline.enabled=false` حتى اكتمال تنفيذ الخادم والعميل واختبارات G4 ثم تفويض G5.

المصادر الأولية لهذا الحد: [RFC 8252 §8.5](https://www.rfc-editor.org/rfc/rfc8252.html#section-8.5) عن عدم صلاحية static secret الموزع داخل native app كإثبات هوية (بينما سرنا per-install وليس app-wide)، و[RFC 9449](https://www.rfc-editor.org/rfc/rfc9449.html) عن sender-constrained DPoP وتخفيف replay. لا تدعي هذه المرحلة تطبيق DPoP.

## 1. الغرض

يعرّف هذا العقد كيفية إنشاء عمليات محلية عند انقطاع الاتصال، وإرسالها لاحقًا، ومنع التكرار، والتحقق من نطاق الشركة والفرع، واكتشاف التعارض، وإعادة المحاولة، وحفظ التدقيق. صُمم `SyncOperation` جديدًا لأن مراجعة المصدر المرجعي لم تجد كيانًا مماثلًا في AlTayerERP؛ لذلك لا توجد إعادة استخدام تلقائية.

> المبدأ الحاكم: **المزامنة لا تعني قبول العملية**. العملية المحلية تحمل حالة مؤجلة، ويعيد الخادم قرارًا صريحًا: قبول، رفض، فشل قابل لإعادة المحاولة، أو تعارض يحتاج معالجة.

## 2. سجل العملية

| الحقل | النوع/القيد | الغرض |
|---|---|---|
| `Id` | UUID | معرف العملية على الخادم. |
| `DeviceId` | نص مطلوب | هوية الجهاز المسجل، لا يعتمد على اسم المستخدم وحده. |
| `RegisteredDeviceId` | UUID مطلوب للصفوف الجديدة في Stage 4 | الهوية الخادمية الثابتة للجهاز؛ تدخل في مفتاح idempotency ولا تستنتج من claim منفردة. |
| `UserId` | UUID مطلوب | صاحب العملية المحلي. |
| `CompanyId` | UUID مطلوب | نطاق الشركة الذي ستطبّق عليه العملية. |
| `BranchId` | UUID اختياري | الفرع، مع إعادة التحقق على الخادم. |
| `ActionCode` | نص مطلوب ومقيد بالـallowlist | الفعل التعاقدي المقصود؛ لا يستنتج من `OperationType` أو `EntityType`. |
| `ProtocolVersion` | نص مطلوب | نسخة بروتوكول ضمن `sync.protocol.allowed_versions`؛ القيمة الأولية `sync-v1`. |
| `OperationType` | CREATE/UPDATE/DELETE/COMMAND | نوع العملية. |
| `EntityType` | نص مطلوب | نوع الكيان المستهدف وفق ActionCode. |
| `EntityId` | UUID مشروط | مطلوب للكيان/aggregate الموجود؛ اختياري فقط في `CREATE` الذي يولد الخادم هويته. |
| `ClientOperationId` | نص مطلوب وفريد لكل جهاز | مفتاح idempotency الأساسي. |
| `RequestFingerprintVersion` | `fp-v1` للصفوف الجديدة في Stage 4 | يثبت encoder ولا يسمح بتغيير صامت لخوارزمية البصمة. |
| `RequestFingerprintHash` | SHA-256 مطلوب للصفوف الجديدة في Stage 4 | بصمة canonical للحقول التجارية الثابتة المحددة في 19.9؛ لا تتضمن proof أو nonce أو token. |
| `OperationCorrelationId` | UUID مطلوب لكل item في Stage 4 | ثابت للعملية ويخزن مع النتيجة ويدخل `fp-v1`. |
| `AttemptCorrelationId` | UUID مطلوب top-level في Stage 4 | جديد لكل HTTP attempt؛ يسجل في proof/audit ولا يخزن كهوية للعملية. |
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

خريطة الهوية الحاكمة للصفوف الجديدة هي `(CompanyId, RegisteredDeviceId, ClientOperationId) → ActionCode + ResultEntityId + outcome`. يدخل `ActionCode` و`OperationCorrelationId` في البصمة الثابتة ولا يوسعان مفتاح uniqueness؛ لذلك فإن إعادة استخدام `ClientOperationId` نفسه مع Action أو OperationCorrelation مختلف هي `IDEMPOTENCY_MISMATCH` وليست عملية جديدة. في CREATE ذي الهوية الخادمية يجوز أن يكون `EntityId` فارغًا، لكن يجب على نتيجة `SUCCEEDED` إعادة `ResultEntityId` وحفظ الخريطة؛ يعيد business replay الخريطة والنتيجة نفسيهما. لا يجوز للعميل إنشاء سجل ثانٍ لتعويض نتيجة ضائعة.

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

لا يجوز الانتقال إلى `SUCCEEDED` إذا فشل فحص الصلاحية أو النطاق أو Hash أو idempotency أو حالة الكيان. ولا يجوز اعتبار timeout سببًا لقبول العملية؛ ينشئ العميل proof جديدًا ويحافظ على `ClientOperationId` و`OperationCorrelationId` نفسيهما، ثم يستعلم الخادم بمفتاح `CompanyId + RegisteredDeviceId + ClientOperationId` قبل أي أثر تجاري جديد.

## 4. سياسة Idempotency وإعادة المحاولة

يولد العميل `ClientOperationId` و`OperationCorrelationId` مرة واحدة ولا يغيرهما عند إعادة الإرسال. للصفوف الجديدة في Stage 4 يبحث الخادم أولًا عن الثلاثي `CompanyId + RegisteredDeviceId + ClientOperationId`. إذا وجد fingerprint مطابقًا يعيد النتيجة/الحالة نفسها، وإذا اختلف أي حقل داخل fingerprint يرفض بـ`IDEMPOTENCY_MISMATCH` دون كشف القيمة المختلفة ودون إنشاء أثر ثانٍ. أما `(CompanyId, DeviceId, ClientOperationId)` فهو مفتاح legacy وفق 19.10.

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

يقبل `POST /api/v1/sync/operations:batch` قائمة محدودة الحجم من العمليات، مع `DeviceId` و`ProtocolVersion=sync-v1`، ويأخذ AttemptCorrelationId من header `X-Correlation-Id` المطابق لـproof claim `cid`، ويحمل كل item `OperationCorrelationId`. لا يرث item محاولة HTTP كهوية تجارية. يعالج الخادم كل عملية بمعاملة مستقلة؛ والقرار هو **النتيجة لكل عملية** حتى لا تمنع عملية تالفة بقية العمليات السليمة.

يعيد الرد top-level `AttemptCorrelationId` الحالي، ولكل عملية: `ClientOperationId`, `OperationCorrelationId` الأصلي, `ServerOperationId`, `ActionCode`, `ResultEntityId`, `Status`, `ResultVersion`, `ErrorCode`, `ConflictCaseId`, و`ServerTime`. لا يعيد النظام Payload حساسًا في رسالة الخطأ، ويمنع تسريب وجود حساب أو مستند خارج النطاق.

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
4. تطبيق قواعد `EntityId` و`BaseVersion` لكل Action، وحفظ `ProtocolVersion`, `OperationCorrelationId`, `ResultEntityId` وخريطة ClientOperationId↔server entity، مع `AttemptCorrelationId` مستقل لكل HTTP attempt.
5. اكتشاف BaseVersion conflict فقط للأفعال optimistic وتقييد Resolution codes وتسلسل KEEP_SERVER/REAPPLY كما هو معتمد.
6. API/worker فعليان للretry/conflict مع عدادي client/server منفصلين وsingle-claim concurrency واستعادة بعد restart.
7. cleanup/retention worker وطابور عميل durable ومشفر، مع redaction/nullability مثبتة.
8. اختبار allowlist مستقل table-driven **لكل Action** في 11.1، واختبار `ACTION_RUNTIME_UNAVAILABLE` لكل Action غير متاح في الإصدار، وعدم خلط read-cache بالwrite queue.
9. اختبارات `T-SYNC-001..010` كاملة، وbatch `0/1/100/101` والنجاح الجزئي، وجدولي retry الافتراضيين والاستنفاد وعدم زيادة server counter بالreplay، وصلاحيات التعارض، وحدود `24h/7d/90d` وتسلسل الإعدادات وinvalid fail-closed.
10. إثبات أن Attachment/POD queue metadata-only وأن binary يرفض قبل عقد resumable/hash، واختبار أن PayloadJson وconflict snapshots تصبح NULL/redacted بعد retention ولا تسرب عبر API/audit/logs.
11. إثبات مطابقة `TransportERP Sync-PoP v1` في القسم 19: lifecycle المفتاح العام، وnonce/freshness، و`jti` single-use على PostgreSQL مشترك، وربط `ath` و`tbh`، وحدود trusted proxy، وفصل HTTP proof replay عن business idempotency.

## 19. قرار Stage 4 الحاكم — TransportERP Sync-PoP v1

**Decision IDs:** `DEC-P1-SYNC-POP-20260826-01`, المعدل والمفسر بالقرار `DEC-P1-SYNC-POP-AMEND-20260826-02` أدناه. عند التعارض تكون صياغة القرار المعدل هي الحاكمة.

**Decision basis:** مراجعة عقد P1 وتنفيذ Stage 3 وسجل الجهاز على exact commit `d0860d70d808374bd5582d2e71c14afa5429f8cd`، مع RFC 9449 وRFC 7638 وRFC 8725 بوصفها مصادر معيارية. القيم الرقمية والـbody-hash والـproxy profile أدناه **قرارات خاصة بالمشروع** وليست أرقامًا مفروضة من RFC.

**Authority:** قرار مفوض من المالك داخل المحادثة بتاريخ 2026-08-26 لصياغة قرارات Stage 4 واعتمادها، من دون تفويض الدمج أو تشغيل Offline أو الادعاء باجتياز G4/G5.

### 19.1 النطاق وعدم الادعاء

- يطبق profile أولًا على `POST /api/v1/sync/operations:batch` فقط، وعلى الجلسات المحلية المرتبطة بـ`RegisteredDeviceId` وCompany/Branch assignment فعالين.
- لا يثق في `device_id` أو public JWK أو Company/Branch قادمة من الطلب منفردة؛ يجب مطابقتها بالسجل الحي والجلسة والتعيين والصلاحية `sync.operations.execute`.
- يبقى Access Token في Stage 4 من نوع Bearer. لا يوصف بأنه `DPoP-bound` ولا يستخدم `Authorization: DPoP` ما لم يصدر عقد هوية مستقل يربط token عند الإصدار بـ`cnf.jkt` ويغطي refresh.
- إثبات PoP لا يستبدل المصادقة أو التفويض أو قواعد النطاق والحالة. وهو لا يفتح Offline؛ production يعيد `OFFLINE_DISABLED` ما دام gate مغلقًا، ولا يصدر nonce ولا يستهلك `jti` في هذا المسار المغلق. تختبر مكونات Stage 4 عبر policy override معزول في الاختبار فقط.
- External Authority يبقى fail-closed للمزامنة لأن Stage 3 لا ينشئ منه trusted local registered-device binding؛ لا يعتمد claim جهاز خارجي لإكمال PoP.

### 19.2 مفتاح الجهاز

- يولد العميل لكل تثبيت key pair مستقلًا من نوع `EC P-256` ويوقع بـ`ES256`. يحفظ private key في OS secure/non-exportable storage حيث تدعمه المنصة، ولا يرسله أو يسجله أو يدخله في backup عام.
- لا يقبل الخادم `none` أو MAC/symmetric algorithms أو RSA أو curve أخرى في `sync-pop-v1`. هذه algorithm allowlist قرار profile أولي، وأي توسعة تحتاج قرارًا واختبارات algorithm-confusion مستقلة.
- يحفظ الخادم public JWK فقط بالشكل canonical ذي الأعضاء المطلوبة `crv`, `kty`, `x`, `y`، ويحفظ `ProofKeyThumbprint=BASE64URL(SHA256(RFC7638-canonical-public-JWK))` و`ProofKeyVersion` يبدأ من `1`.
- يرفض JWK يحوي private members أو `jku`/`x5u` أو key غير P-256 أو نقاطًا غير صالحة. لا يجلب الخادم key material من URL.
- public JWK مطلوب لأهلية المزامنة، لا لمجرد وجود سجل جهاز. الجهاز legacy بلا مفتاح يبقى صالحًا للوظائف Online المسموحة لكنه يرفض للمزامنة بـ`DEVICE_PROOF_KEY_REQUIRED`.
- يصبح المفتاح authoritative عند إتمام تغيير مفتاح ناجح. جميع المسارات Online-only وبصلاحية `devices.manage` ذات نطاق الشركة، ولا تقبل External Authority أو claim واردًا بدل الربط المحلي.
- تنشأ challenge hash-only قصيرة العمر عبر `POST /api/v1/devices/{id}/proof-key-challenges` وترتبط بـ`ChangeType`, `ChangeRequestId`, `ExpectedProofKeyVersion`, وthumbprint المفتاح الجديد. لا يقبل JWK جديد بلا إثبات امتلاك challenge بالمفتاح الجديد.
- `POST /api/v1/devices/{id}:bind-proof-key`: مسموح لـ`PENDING|ACTIVE` عندما `ProofKeyVersion IS NULL`، ويتطلب new-key proof.
- `POST /api/v1/devices/{id}:rotate-proof-key`: مسموح لـ`ACTIVE` مع `ExpectedProofKeyVersion`، ويتطلب current-key proof وnew-key proof.
- `POST /api/v1/devices/{id}:recover-proof-key`: مسموح لـ`ACTIVE|SUSPENDED|EXPIRED` لا `REVOKED`، ويتطلب expected version وسببًا وnew-key proof، ويلغي جلسات الجهاز ولا يعيد تنشيطه تلقائيًا. لا يوجد استبدال صامت للمفتاح المفقود.
- يسجل تغيير المفتاح append-only بمفتاح `(RegisteredDeviceId, ChangeRequestId)`. replay مطابق يعيد النتيجة، وتغيير payload تحت المفتاح نفسه يعطي `PROOF_KEY_CHANGE_MISMATCH`. لا يعاد public JWK في response؛ يعاد thumbprint/version فقط.
- `CredentialVersion` الحالي للجلسة مستقل عن `ProofKeyVersion`. التدوير/الاستعادة يزيدان ProofKeyVersion ذريًا، ويبطلان nonces للنسخة السابقة، ولا يحذفان proof replay history، ولا يسجلان JWK أو challenge أو proof الخام.

#### 19.2.1 صيغة إثبات تغيير المفتاح

- يعيد endpoint التحدي قيمة `Challenge` من `32 random bytes` Base64url بلا padding مرة واحدة، و`ChallengeId`, `ChangeRequestId`, `ExpiresAt`. يخزن الخادم `SHA256(raw challenge bytes)` فقط؛ الصلاحية `5m`؛ ويستهلك التحدي مرة واحدة ذريًا داخل معاملة تغيير المفتاح.
- طلب bind/rotate/recover هو JSON بحد `16 KiB` ويحمل `ChallengeId`, `ChangeRequestId`, `ExpectedProofKeyVersion`, `ChangeType`, `NewPublicJwk`, و`Reason` عند recovery. يحسب proof `tbh` على raw body كما في 19.8.
- يرسل `Device-Key-Proof-New` دائمًا، و`Device-Key-Proof-Current` في ROTATE فقط. كلاهما compact JWS بحد `4096 bytes`، protected header دقيق: `typ=transporterp-key-change+jwt`, `alg=ES256`, وpublic `jwk`؛ new يطابق `NewPublicJwk`، وcurrent يطابق thumbprint/نسخة السجل الحية. ترفض private members وURLs وduplicate/unknown critical headers.
- payload الإلزامي لكل proof: `cid=ChallengeId`, `rid=ChangeRequestId`, `did=RegisteredDeviceId`, `ct=BIND|ROTATE|RECOVER`, `iat`, `jti`, `chl=BASE64URL(raw challenge bytes)`, `nkt=NewProofKeyThumbprint`, `htm=POST`, `htu` canonical للendpoint المحدد، `ath=BASE64URL(SHA256(ASCII(raw bearer token)))`, و`tbh`. current/new payloads يجب أن تتطابق حرفيًا عدا المفتاح والتوقيع و`jti`.
- نافذة `iat` هي `-120s/+30s`. `jti` مختلف لكل proof ولا يعاد، لكن one-time challenge/ChangeRequestId هما replay barrier الحاكم لهذا المسار. يقارن الخادم hash لـ`chl` constant-time بالسجل، ويتحقق من signature وbody/token/URI قبل استهلاك challenge.
- BIND وRECOVER يثبتان المفتاح الجديد. ROTATE يثبت المفتاح الحالي والجديد؛ غياب أيهما يرفض. recovery بلا current proof مسار high-risk: يتطلب session محلية، `devices.manage`, same-company row lock، reason، وتدقيقًا وإلغاء جميع جلسات الجهاز، ولا يعيد حالة الجهاز إلى ACTIVE.
- معاملة تغيير المفتاح ذات ترتيب ثابت: قفل RegisteredDevice وإعادة فحص company/device والحالة والنسخة **القديمة** → user advisory → قفل challenge والتحقق من proof → insert change ledger بينما current device ما زال بالنسخة القديمة → ضبط ConsumedAt → تحديث device CAS إلى ResultProofKeyVersion → invalidate nonces للنسخة القديمة → session revoke عند recovery → AuditStreamHead أخيرًا → commit. نقطة linearization هي هذا commit الواحد. لا يجوز تحديث device قبل insert ledger؛ وفشل Audit أو أي constraint يعيد المعاملة كاملة.

### 19.3 بنية proof الإلزامية

يرسل العميل header واحدًا فقط باسم `DPoP`. القيمة compact signed JWT لا يتجاوز طولها `4096 bytes`. يرفض تعدد header أو طيه إلى قائمة. JOSE header والclaims الإلزامية هي:

| العنصر | قرار `sync-pop-v1` |
|---|---|
| `typ` | القيمة الدقيقة `dpop+jwt`. |
| `alg` | القيمة الدقيقة `ES256`. |
| `jwk` | public P-256 JWK فقط؛ thumbprint مطابق لسجل الجهاز ونسخته الحية. |
| `jti` | UUIDv4 أو Base64url لناتج CSPRNG لا يقل عن 96 bit؛ طول النص `16..128`؛ single-use وفق 19.6. |
| `htm` | القيمة الدقيقة `POST`. |
| `htu` | URI canonical المحدد في 19.7، بلا query أو fragment. |
| `iat` | NumericDate بالثواني ويجتاز نافذة 19.5. |
| `ath` | `BASE64URL(SHA256(ASCII(raw bearer access token)))`. |
| `nonce` | nonce خادمي حي مطابق للجهاز ونسخة المفتاح وفق 19.4. |
| `tbh` | claim خاص بهذا المشروع: `BASE64URL(SHA256(raw HTTP request-body octets))` وفق 19.8. |
| `cid` | UUID النصي الدقيق من header الإلزامي `X-Correlation-Id`؛ هو AttemptCorrelationId لهذه المحاولة. |

أي `crit` غير معروف أو duplicate JOSE/claim name أو private JWK member أو claim مطلوب من نوع غير صحيح يجعل proof غير صالح. لا تحفظ JWT الخام ولا public JWK القادم من كل طلب في logs/audit.

### 19.4 سياسة nonce الخادمي

- يولد الخادم nonce من `32 random bytes CSPRNG` ويعرضه Base64url بلا padding. لا يخزن الخام؛ يخزن SHA-256 مع `RegisteredDeviceId`, `ProofKeyVersion`, `IssuedAt`, `ExpiresAt`.
- **قرار المشروع:** صلاحية nonce هي `5 minutes` من وقت الخادم. يقبل أي nonce غير منتهٍ أصدره الخادم لنفس الجهاز ونسخة المفتاح؛ إصدار nonce أحدث لا يبطل السابق قبل انتهاء مدته، حتى لا تكسر الدفعات المتوازية.
- يجوز استخدام nonce نفسه في أكثر من proof خلال صلاحيته، لكن يجب أن يكون لكل proof `jti` جديدًا؛ nonce وحده ليس replay key.
- لا يقبل المسار أي proof بلا nonce. لطلب ذي Bearer context صحيح وجهاز مربوط لكنه بلا nonce حي، يرد الخادم `401` مع `WWW-Authenticate: DPoP error="use_dpop_nonce"` و`DPoP-Nonce` و`Cache-Control: no-store`.
- يجوز أن يرسل الرد الناجح `DPoP-Nonce` جديدًا للاستعمال اللاحق. لا يصدر nonce لمستخدم غير مصادق أو جلسة غير مرتبطة أو جهاز خارج النطاق، ولا يظهر nonce في JSON أو audit أو logs.
- التدوير أو التعليق أو الإلغاء أو انتهاء الجهاز/التعيين يبطل nonces المرتبطة فورًا. يمنع nonce downgrade: بعد إصدار nonce لا يقبل proof بلا claim مطابق.

### 19.5 freshness والانحراف الزمني

- **قرار المشروع:** يقبل proof عندما `serverNow - 120 seconds <= iat <= serverNow + 30 seconds` وعندما يكون nonce حيًا. `120s` تحد exposure بعد التسريب، و`30s` يسمح بانحراف محدود دون فتح pre-generation طويل؛ nonce الخادمي ذو `5m` يعالج retry والتوازي لكنه لا يوسع عمر proof.
- لا يستخدم `ClientOccurredAt` بدل `iat`، ولا يثق بساعة الجهاز لاتخاذ قرار مالي. كل المقارنات الأمنية تستخدم وقت خادم UTC.
- حدود `120/30/300 seconds` إعدادات Platform-only ثابتة لهذا الإصدار، validated at startup، ولا تخففها Company/Branch/Device overrides. القيمة الغائبة أو غير المطابقة تعطل Sync-PoP fail-closed ولا تعود إلى default صامت.

### 19.6 منع proof replay

- بعد نجاح signature/key/claims/nonce/freshness/token/body checks وقبل enqueue، يحجز الخادم proof ذريًا في PostgreSQL مشترك بين جميع instances.
- يثبت claim في معاملة قصيرة مستقلة قبل معاملات عناصر الدفعة، ولا يُعاد أو يحذف عند فشل parsing اللاحق أو عنصر تجاري أو ضياع الاستجابة؛ الاسترداد يكون دائمًا بـproof جديد وbusiness IDs نفسها.
- يخزن فقط `JtiHash=SHA256(UTF8(jti))` مع `CompanyId`, `RegisteredDeviceId`, `DeviceId`, `DeviceAssignmentId`, `UserId`, `BranchId`, `ProofKeyVersion`, `ProofKeyThumbprint`, `HtuHash`, `HttpMethod`, `NonceRecordId`, `IssuedAt`, `FirstSeenAt`, `ExpiresAt`, و`AttemptCorrelationId`. لا يخزن raw jti أو raw proof أو token.
- **قرار المشروع:** uniqueness أقوى من حد target URI في RFC: `(RegisteredDeviceId, ProofKeyVersion, JtiHash)` فريد خلال مدة الاحتفاظ؛ لا يجوز إعادة jti على URI آخر أو دفعة أخرى.
- **قرار المشروع:** يحتفظ replay record مدة `10 minutes` من `FirstSeenAt`. هذه المدة تتجاوز نافذة proof (`120s + 30s`) وعمر nonce (`5m`) وتوفر هامش cleanup/cluster دون قبول replay متأخر. cleanup لا يحذف سجلًا قبل `ExpiresAt`.
- duplicate sequential أو concurrent يرفض `401 invalid_dpop_proof` ولا يصل إلى enqueue. unique constraint هو الحاكم، لا check-then-insert في الذاكرة.
- إذا استُهلك proof ثم ضاع الرد أو فشل النقل، ينشئ العميل proof و`jti` جديدين ويعيد **نفس** `ClientOperationId` والحمولة؛ business idempotency يعيد النتيجة ولا ينشئ أثرًا ثانيًا.
- نقطة linearization لقبول proof هي commit لمعاملة قصيرة تبدأ بقفل صف `RegisteredDevice FOR UPDATE`، ثم تعيد فحص الشركة وحالة الجهاز والتعيين والـthumbprint والنسخة، ثم تقفل nonce وتدرج replay record. إذا سبق commit أي lifecycle change — تدوير، تعليق، إلغاء، انتهاء، أو إزالة assignment — فشل proof قبل claim. وإذا سبق commit قبول proof جاز **لنفس HTTP batch الجاري فقط** الإكمال بaccepted immutable proof context حتى لو وقع lifecycle change بعده؛ لا يشطر الحدث دفعة قبلت أمنيًا، لكنه يمنع كل claim لاحق. لا يتحول AcceptedProofReplayId إلى capability قابلة لإعادة الاستخدام خارج request scope.
- ترتيب الأقفال في كل مسارات الجهاز هو: `RegisteredDevice` → user-scope advisory lock الحاكم الحالي → challenge/assignment/nonce بترتيب ID → replay/key-change insert → sessions عند recovery → `AuditStreamHead` أخيرًا. يأخذ user scope update trigger advisory lock نفسه، وبذلك يبقى منع drift متبادلًا ولا يفتح TOCTOU. enqueue بعد accepted claim لا يعيد قفل الجهاز؛ يستخدم `AcceptedProofReplayId` داخليًا ويأخذ user advisory ثم business-idempotency ثم operation ثم audit. لا يضاف advisory lock آخر لمسار واحد.
- قبل تنفيذ runtime Stage 4 يجب إعادة ترتيب المسارات الحالية التي تخالف ذلك: refresh/LastSeen يصبح `RegisteredDevice → AuthSession(s ordered by Id) → AuditStreamHead`، وself-revoke يأخذ Session ثم يحرره قبل Audit أو يدخل عبر ترتيب موحد لا يصنع دورة Session↔Audit. يثبت concurrency test غياب deadlock بين refresh/revoke/rotate/assignment/proof claim.

### 19.7 `htu` وحدود reverse proxy

- `sync.proof.public_origin` إعداد نشر إلزامي عند تشغيل Stage 4، absolute HTTPS origin بلا path/query/fragment. expected `htu` هو هذا origin زائد المسار الثابت `/api/v1/sync/operations:batch` بعد RFC 3986 syntax/scheme normalization. لا يقبل `http` أو query على `sync-v1`.
- لا يبنى expected `htu` من Host أو `X-Forwarded-*` غير موثوق. يضبط ASP.NET Core Forwarded Headers قبل auth باستخدام `KnownProxies`/`KnownNetworks` صريحة و`ForwardLimit` و`AllowedHosts`; لا يسمح مسح قوائم trust لقبول جميع المصادر.
- يجب أن يرى التطبيق scheme خارجيًا `https` بعد trusted proxy processing. فشل topology أو public origin أو host/scheme comparison يعطل المسار fail-closed؛ لا يستبدل `htu` بعنوان backend الداخلي.
- canonical expected htu هو ASCII exact: `https://` + host محول إلى IDNA A-label lowercase + `:port` فقط إذا لم يكن `443` + المسار الحرفي `/api/v1/sync/operations:batch`. لا trailing slash إضافي، ولا query/fragment، ولا percent-decoding أو case-fold للمسار. يجب أن يساوي claim هذه السلسلة byte-for-byte، و`HtuHash` المخزن هو raw 32 bytes من `SHA256(ASCII(exact canonical expected htu))`.

### 19.8 سلامة جسم دفعة المزامنة

- RFC 9449 القياسي يغطي method وURI ولا يضمن body integrity؛ لذلك يعتمد المشروع claim إضافيًا `tbh` لطلب الكتابة.
- `sync-v1` يقبل `Content-Type: application/json` و`Content-Encoding` غائبًا أو `identity` فقط. يحسب العميل والخادم SHA-256 على **نفس raw body octets كما أرسلت قبل JSON deserialization**؛ ثم Base64url بلا padding.
- يفشل اختلاف `tbh` قبل parsing/enqueue بـ`invalid_dpop_proof`. لا يستخدم re-serialization أو ترتيب JSON properties لحساب hash، ولا تسجل body أو hash input في رسالة الخطأ.
- يبقى `PayloadHash` لكل عنصر تحققًا مستقلًا بعد parsing؛ `tbh` يحمي envelope والmetadata والترتيب والعمليات مجتمعة، ولا يحل مكان PayloadHash.
- الحد الأقصى لجسم HTTP الخام هو `2,097,152 bytes` قبل parsing. `Content-Length` الأكبر يرفض `413 REQUEST_BODY_TOO_LARGE` فورًا؛ وإذا غاب أو كان chunked يوقف bounded reader عند byte `2,097,153`. لا buffering غير محدود، ولا يستهلك nonce/jti قبل تجاوزات request-level.
- `PayloadJson` لكل عنصر، بعد JSON unescape إلى UTF-8، يجب أن يكون JSON صالحًا بحد `16,384 bytes` وعمق أقصى `32`. الحد نفسه مقبول، والـ`+1` يعطي item-level `PAYLOAD_TOO_LARGE` وفق partial-success؛ retry يستخدم proof جديدًا. يحسب `PayloadHash` على bytes المحتوى المفكوك، بينما `tbh` على envelope الخام.
- حدود app وreverse proxy يجب أن تتطابق أو يكون effective minimum موثقًا؛ إعداد غير متوافق يمنع startup عند فتح Stage 4. `Content-Encoding` غير غائب/`identity` يعطي `415 CONTENT_ENCODING_UNSUPPORTED`.

### 19.9 business idempotency والبصمة الكاملة

- للصفوف الجديدة المفتاح الفريد هو `(CompanyId, RegisteredDeviceId, ClientOperationId)`. لا يدخل `jti`, `nonce`, `iat`, `ath`, `tbh`, Bearer token، `AttemptCorrelationId`, retry counters أو أوقات الخادم في المفتاح أو fingerprint.
- يرسل `AttemptCorrelationId` UUID غير صفري جديدًا لكل HTTP attempt في header الإلزامي `X-Correlation-Id`، ويجب أن يساوي claim `cid` في proof. لذلك يتاح قبل JSON parsing ويمكن حفظه مع replay حتى عند malformed envelope بعد claim. لا يخزن في `SyncOperation` ولا يدخل fingerprint؛ يسجل في Audit ويرجع top-level. يرسل كل item `OperationCorrelationId` UUID غير صفري ثابتًا لكل `ClientOperationId`؛ يخزن في العملية ويدخل fingerprint ويرجع item-level. وجود `AttemptCorrelationId` أو الاسم القديم `RequestCorrelationId` داخل JSON لا يعمل كـalias ويعطي `REQUEST_SCHEMA_INVALID` عند إمكان parsing.
- `RequestFingerprintVersion` للصفوف الجديدة هو `fp-v1` و`RequestFingerprintHash` هو SHA-256 بطول 32 byte. يبدأ encoder بالـbytes: ASCII `TransportERP.SyncOperationFingerprint`, ثم `00`, ثم ASCII `fp-v1`, ثم `00`, ثم UInt16 big-endian للقيمة `14`.
- الحقول الأربعة عشر، بهذا الترتيب والـUInt16 IDs من `0x0001` إلى `0x000e`، هي: `CompanyId`, `RegisteredDeviceId`, `UserId`, nullable `BranchId`, `ProtocolVersion`, `ActionCode`, `OperationType`, `EntityType`, nullable `EntityId`, `ClientOperationId`, `PayloadHash`, `ClientOccurredAt`, nullable `BaseVersion`, `OperationCorrelationId`.
- ترميز الحقل: UInt16 ID big-endian، ثم presence byte `00` للnull أو `01` للحاضر؛ عند الحضور فقط UInt32 byte length big-endian ثم القيمة. UUID هو 16 byte RFC4122/network order لا ترتيب `Guid.ToByteArray()` التاريخي. النص UTF-8 case-sensitive بلا trim/case-fold ويجب أن يكون NFC. `PayloadHash` يفك من 64 hex إلى 32 raw bytes. الوقت UTC `Z` فقط و0..6 fractional digits، ويرمز Int64 big-endian microseconds منذ Unix epoch بلا rounding. `BaseVersion` Int64 big-endian. `OperationType` أحد `CREATE|UPDATE|DELETE|COMMAND` وبقية الرموز canonical حسب العقد.
- يجب تثبيت golden vectors مستقلة عن production encoder قبل بدء runtime، وإعادة حسابها بأداتين مستقلتين مع byte dump. لا يصبح هذا القسم `IMPLEMENTATION_READY` إذا كان expected hash مولدًا من الكود نفسه أو غير مثبت في commit العقد.
- replay ببصمة مطابقة يعيد نفس `ServerOperationId`, `ActionCode`, `ResultEntityId`, `Status`, `ResultVersion`, `ErrorCode`, `ConflictCaseId` والـoutcome المحفوظ، ولا يزيد server execution retry counter ولا يكرر Audit قبول/أثر تجاري.
- اختلاف أي حقل في fingerprint، بما فيه `OperationCorrelationId`, تحت المفتاح نفسه يرفض `IDEMPOTENCY_MISMATCH` بلا بيان الحقل المختلف. `ActionCode` جزء من fingerprint لا من uniqueness، وبذلك يظل `ClientOperationId` فريدًا لكل جهاز داخل شركته.
- PoP replay وbusiness replay طبقتان مستقلتان: proof نفسه single-use، أما العملية التجارية فتعاد بproof جديد ومفتاحها الثابت.

#### 19.9.1 متجهات `fp-v1` الذهبية

حُسب المتجهان مستقلًا بتنفيذ Python ثم أعيد بناؤهما من الصفر بـNode.js `crypto` في 2026-08-26، وتطابقت الأطوال والـSHA-256 والـbyte prefixes/suffixes. لا يجوز أن تولد اختبارات القبول expected hash من production encoder.

**Vector A**

- `CompanyId=11111111-1111-1111-1111-111111111111`
- `RegisteredDeviceId=22222222-2222-2222-2222-222222222222`
- `UserId=33333333-3333-3333-3333-333333333333`, `BranchId=NULL`
- `ProtocolVersion=sync-v1`, `ActionCode=CreateWaybillDraft`, `OperationType=CREATE`, `EntityType=Waybill`, `EntityId=NULL`
- `ClientOperationId=op-0001`
- `PayloadHash=000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f`
- `ClientOccurredAt=2026-08-26T00:00:00.123456Z`, `BaseVersion=NULL`
- `OperationCorrelationId=44444444-4444-4444-4444-444444444444`
- encoded length=`281`; SHA-256=`d373caf8c441d4380784ca768d6020c1c556e4f7d02bff5f216afb9c813e47dd`
- first 96 bytes=`5472616e73706f72744552502e53796e634f7065726174696f6e46696e6765727072696e740066702d763100000e0001010000001011111111111111111111111111111111000201000000102222222222222222222222222222222200030100`
- last 96 bytes=`0900000a01000000076f702d30303031000b0100000020000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f000c0100000008000659e7e6860240000d00000e010000001044444444444444444444444444444444`

**Vector B**

- `CompanyId=aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa`
- `RegisteredDeviceId=bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb`
- `UserId=cccccccc-cccc-4ccc-8ccc-cccccccccccc`, `BranchId=dddddddd-dddd-4ddd-8ddd-dddddddddddd`
- `ProtocolVersion=sync-v1`, `ActionCode=UpdateWaybillDraft`, `OperationType=UPDATE`, `EntityType=Waybill`, `EntityId=eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee`
- `ClientOperationId=طلب-١`
- `PayloadHash=ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff`
- `ClientOccurredAt=1970-01-01T00:00:00Z`, `BaseVersion=42`
- `OperationCorrelationId=ffffffff-ffff-4fff-8fff-ffffffffffff`
- encoded length=`335`; SHA-256=`2143fd79345d63ccc154202cbf56eb317f06ef7054a7d72c5c5f29074e3f9b43`
- first 96 bytes=`5472616e73706f72744552502e53796e634f7065726174696f6e46696e6765727072696e740066702d763100000e00010100000010aaaaaaaaaaaa4aaa8aaaaaaaaaaaaaaa00020100000010bbbbbbbbbbbb4bbb8bbbbbbbbbbbbbbb00030100`
- last 96 bytes=`a82dd9a1000b0100000020ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000c01000000080000000000000000000d0100000008000000000000002a000e0100000010ffffffffffff4fff8fffffffffffffff`

### 19.10 سياسة migration وlegacy

- تنفذ Stage 4 بمهاجرة additive مستقلة مع preflight و`Up -> Down -> Up` PostgreSQL test؛ لا تعدل migration Stage 3.
- تضاف حقول proof key إلى `RegisteredDevice` nullable للسجلات التاريخية، وتضاف `ActionCode`, `ProtocolVersion`, `OperationCorrelationId`, `ResultEntityId`, `RequestFingerprintVersion`, `RequestFingerprintHash` وحقول proof provenance المحددة أدناه إلى `SyncOperation` بطريقة تسمح ببقاء legacy rows قابلة للقراءة والتدقيق.
- يجعل `EntityId` nullable لأن CREATE الخادمي قد لا يملك ID قبل النجاح. يفرض trigger/check على **كل INSERT جديد بعد migration** وجود registered-device/proof provenance والحقول الحاكمة؛ لا يستخدم nullable schema كمسار downgrade.
- يسقط unique index التاريخي العام `(DeviceId, ClientOperationId)` فقط بعد preflight، ثم تنشأ الفهارس الجزئية exact في 19.10.1 باستخدام `RequestFingerprintVersion` discriminator. صفوف Stage 3 ذات `RegisteredDeviceId` ولكن بلا fingerprint تظل legacy؛ لا توجد uniqueness عالمية عابرة للشركات.
- لا backfill تخميني لـRegisteredDeviceId أو ActionCode أو fingerprint من النصوص التاريخية. كل row بـ`RequestFingerprintVersion IS NULL` تبقى legacy immutable وغير مؤهلة لتنفيذ Stage 4، سواء كان RegisteredDeviceId فيها NULL أم لا. إذا اصطدم طلب جديد بـlegacy `(CompanyId, DeviceId, ClientOperationId)` يرفض `LEGACY_IDEMPOTENCY_CONFLICT` ولا ينشئ صفًا موازيًا.
- تحفظ proof provenance (`RegisteredDeviceId`, `ProofKeyVersion`, thumbprint/hash reference وaccepted proof record ID) immutable بعد INSERT، مع بقاء تحديث status/result التاريخي ممكنًا حتى لو عُلّق أو دُوّر الجهاز لاحقًا.
- `Down` غير مدمر. قبل أول DDL وفي المعاملة نفسها يفشل `STAGE4_DOWN_BLOCKED_DATA_PRESENT` إذا وجد row بـ`RequestFingerprintVersion` أو accepted proof، أو مفتاح proof مربوط، أو nonce/replay/challenge/key-change data. وحتى بدونها يفشل `STAGE4_DOWN_LEGACY_SHAPE_CONFLICT` إذا تعذر استعادة nullability/index التاريخيين بلا فقد. عند الفشل لا يحدث partial DDL. يختبر `Up→Down→Up` ببيانات legacy حقيقية، وتختبر حالات block منفصلة.

#### 19.10.1 سجل schema الأدنى وأسماء القيود

كل العناصر في schema `transport_erp`. الأسماء التالية نهائية لهذا القرار وتبقى دون حد PostgreSQL ذي 63 byte:

- `registered_devices`: يضاف bundle nullable للـlegacy: `ProofPublicJwkCanonicalJson varchar(512)`, `ProofKeyThumbprint varchar(43)`, `ProofKeyVersion integer`, `ProofKeyChangedAt timestamptz`, `ProofKeyChangedByUserId uuid`. القيد `ck_reg_device_proof_key_bundle` هو: إما الخمسة NULL، أو الخمسة NOT NULL مع `ProofKeyVersion>=1 AND char_length(ProofKeyThumbprint)=43`. unique partial `ux_registered_device_proof_thumbprint` على `(ProofKeyThumbprint) WHERE ProofKeyThumbprint IS NOT NULL`. FK `fk_reg_device_proof_changed_by` على `ProofKeyChangedByUserId` إلى `users.Id` بـRESTRICT. تعدل `enforce_registered_device_user_scope()` وtriggerه الحالي ليأخذا advisory lock لـProofKeyChangedByUserId ويفرضا `actor.CompanyId IS NULL OR actor.CompanyId=NEW.CompanyId`؛ وهذا يسمح platform actor وفق سياسة Stage 3 دون cross-tenant actor. كما تعدل `prevent_user_scope_reference_drift()` الحالية لتفحص reverse references الجديدة: `registered_devices.ProofKeyChangedByUserId`, `registered_device_proof_key_challenges.CreatedByUserId`, و`registered_device_proof_key_changes.ChangedByUserId`، فتمنع نقل actor إلى نطاق يجعل أي provenance قائمًا عابرًا للشركة تحت advisory lock نفسه. function `fn_reg_device_proof_key_transition` وtrigger `trg_reg_device_proof_key_transition BEFORE UPDATE OF` حقول proof يفرضان: انتقال bundle من NULL إلى مربوط يجب أن يكون `ProofKeyVersion=1`؛ bundle مربوط لا يمسح؛ وأي تغيير مفتاح لاحق يجب أن يكون `NEW.ProofKeyVersion=OLD.ProofKeyVersion+1`. لا يغير القرار `CredentialHash/CredentialVersion`.
- `sync_proof_nonces`: `Id uuid PK`, `CompanyId uuid`, `RegisteredDeviceId uuid`, `DeviceId varchar(120)`, `ProofKeyVersion integer`, `NonceHash bytea`, `IssuedAt timestamptz`, `ExpiresAt timestamptz`، كلها NOT NULL. القيود: `pk_sync_proof_nonces`; `fk_sync_nonce_registered_device` على `(RegisteredDeviceId,CompanyId,DeviceId)` إلى `(Id,CompanyId,DeviceId)` بـRESTRICT؛ `ck_sync_nonce_key_version: ProofKeyVersion>=1`; `ck_sync_nonce_hash_len: octet_length(NonceHash)=32`; `ck_sync_nonce_window: ExpiresAt>IssuedAt`. الفهارس: unique `ux_sync_nonce_hash` على `(NonceHash)`؛ unique alternate `ux_sync_nonce_scope` على `(Id,CompanyId,RegisteredDeviceId,DeviceId,ProofKeyVersion)`؛ `ix_sync_nonce_device_key_expiry` على `(RegisteredDeviceId,ProofKeyVersion,ExpiresAt)`؛ `ix_sync_nonce_expiry` على `(ExpiresAt)`. nonce غير single-use ولا يحوي raw value.
- `sync_proof_replays`: `Id uuid PK` وهو AcceptedProofReplayId، ثم `CompanyId uuid`, `RegisteredDeviceId uuid`, `DeviceId varchar(120)`, `DeviceAssignmentId uuid`, `UserId uuid`, `BranchId uuid`, `ProofKeyVersion integer`, `ProofKeyThumbprint varchar(43)`, `JtiHash bytea`, `HtuHash bytea`, `HttpMethod varchar(8)`, `NonceRecordId uuid`, `IssuedAt timestamptz`, `FirstSeenAt timestamptz`, `ExpiresAt timestamptz`, `AttemptCorrelationId uuid`، كلها NOT NULL. يضاف إلى `registered_device_assignments` alternate unique `ux_device_assignment_proof_scope` على `(Id,RegisteredDeviceId,CompanyId,UserId,BranchId)`. القيود: `pk_sync_proof_replays`; `fk_sync_replay_registered_device` على `(RegisteredDeviceId,CompanyId,DeviceId)` إلى `(Id,CompanyId,DeviceId)` بـRESTRICT؛ `fk_sync_replay_assignment_scope` على `(DeviceAssignmentId,RegisteredDeviceId,CompanyId,UserId,BranchId)` إلى assignment alternate key بـRESTRICT؛ `fk_sync_replay_nonce_scope` على `(NonceRecordId,CompanyId,RegisteredDeviceId,DeviceId,ProofKeyVersion)` إلى nonce alternate key نفسه بـRESTRICT؛ `ck_sync_replay_key_version: ProofKeyVersion>=1`; `ck_sync_replay_hash_len: octet_length(JtiHash)=32 AND octet_length(HtuHash)=32 AND char_length(ProofKeyThumbprint)=43`; `ck_sync_replay_method: HttpMethod='POST'`; `ck_sync_replay_window: ExpiresAt>FirstSeenAt AND FirstSeenAt>=IssuedAt`. unique `ux_sync_replay_device_key_jti` على `(RegisteredDeviceId,ProofKeyVersion,JtiHash)`؛ `ix_sync_replay_expiry` على `(ExpiresAt)`؛ `ix_sync_replay_nonce` على `(NonceRecordId)`. function `fn_sync_replay_append_only` وtrigger `trg_sync_replay_append_only BEFORE UPDATE` يمنعان UPDATE؛ cleanup role يحذف replays المنتهية أولًا بعد 10m، ثم يحذف nonce منتهيًا فقط عندما لا يعود له replay تابع. انتهاء nonce عند 5m يمنع الاستعمال ولا يفرض الحذف الفوري.
- `registered_device_proof_key_challenges`: `Id uuid` مع PK `pk_device_key_challenges`; ثم `CompanyId uuid`, `RegisteredDeviceId uuid`, `DeviceId varchar(120)`, `ChangeRequestId uuid`, `ChangeType varchar(8)`, `ExpectedProofKeyVersion integer NULL`, `NewProofKeyThumbprint varchar(43)`, `ChallengeHash bytea`, `IssuedAt timestamptz`, `ExpiresAt timestamptz`, `ConsumedAt timestamptz NULL`, `CreatedByUserId uuid`؛ البقية NOT NULL. `fk_key_challenge_registered_device` يستخدم `(RegisteredDeviceId,CompanyId,DeviceId)` بـRESTRICT؛ `fk_key_challenge_created_by` على CreatedByUserId إلى `users.Id` بـRESTRICT؛ `ck_key_challenge_type: ChangeType IN ('BIND','ROTATE','RECOVER')`; `ck_key_challenge_expected_version: (ChangeType='BIND' AND ExpectedProofKeyVersion IS NULL) OR (ChangeType IN ('ROTATE','RECOVER') AND ExpectedProofKeyVersion IS NOT NULL AND ExpectedProofKeyVersion>=1)`; `ck_key_challenge_hash_len: octet_length(ChallengeHash)=32 AND char_length(NewProofKeyThumbprint)=43`; `ck_key_challenge_window: ExpiresAt>IssuedAt AND (ConsumedAt IS NULL OR ConsumedAt>=IssuedAt)`. unique `ux_device_key_challenge_request` على `(RegisteredDeviceId,ChangeRequestId)`؛ unique alternate `ux_key_challenge_change_scope` على `(Id,CompanyId,RegisteredDeviceId,DeviceId,ChangeRequestId,ChangeType,NewProofKeyThumbprint)`؛ و`ix_device_key_challenge_expiry` على `(ExpiresAt)`. function `fn_key_challenge_user_scope` وtrigger `trg_key_challenge_user_scope BEFORE INSERT OR UPDATE OF CreatedByUserId,CompanyId` يقفلان RegisteredDevice أولًا ويعيدان فحص company/device وحالة النوع والنسخة: BIND=`PENDING|ACTIVE` مع key NULL وExpected NULL؛ ROTATE=`ACTIVE` مع current version=Expected؛ RECOVER=`ACTIVE|SUSPENDED|EXPIRED` لا REVOKED مع current version=Expected؛ ثم يأخذان user-scope advisory ويفرضان actor CompanyId NULL أو نفس CompanyId. لا يعاد challenge الخام بعد الاستجابة الأولى.
- `registered_device_proof_key_changes`: append-only بالأعمدة `Id uuid` مع PK `pk_device_key_changes`; ثم `CompanyId uuid`, `RegisteredDeviceId uuid`, `DeviceId varchar(120)`, `ChangeRequestId uuid`, `ChallengeId uuid`, `ChangeType varchar(8)`, `ExpectedProofKeyVersion integer NULL`, `PreviousProofKeyThumbprint varchar(43) NULL`, `NewProofKeyThumbprint varchar(43)`, `ResultProofKeyVersion integer`, `ChangedByUserId uuid`, `Reason varchar(500) NULL`, `ChangedAt timestamptz`؛ البقية NOT NULL. `fk_key_change_registered_device` يستخدم الثلاثي نفسه بـRESTRICT؛ `fk_key_change_challenge_scope` على `(ChallengeId,CompanyId,RegisteredDeviceId,DeviceId,ChangeRequestId,ChangeType,NewProofKeyThumbprint)` إلى alternate challenge key بـRESTRICT؛ `fk_key_change_changed_by` إلى `users.Id` بـRESTRICT؛ `ck_key_change_type` لنفس القيم؛ `ck_key_change_version_shape: (ChangeType='BIND' AND ExpectedProofKeyVersion IS NULL AND PreviousProofKeyThumbprint IS NULL AND ResultProofKeyVersion=1) OR (ChangeType IN ('ROTATE','RECOVER') AND ExpectedProofKeyVersion IS NOT NULL AND ExpectedProofKeyVersion>=1 AND PreviousProofKeyThumbprint IS NOT NULL AND char_length(PreviousProofKeyThumbprint)=43 AND ResultProofKeyVersion=ExpectedProofKeyVersion+1)`; `ck_key_change_recovery_reason: ChangeType<>'RECOVER' OR (Reason IS NOT NULL AND char_length(trim(Reason))>0)`. unique `ux_device_key_change_request` على `(RegisteredDeviceId,ChangeRequestId)`. function واحدة `fn_key_change_insert_guard` وtrigger واحد `trg_key_change_insert_guard BEFORE INSERT` يفترضان الجهاز مقفولًا ويعيدان قفله idempotently أولًا، ثم يعيدان فحص الحالة تحت القفل: BIND=`PENDING|ACTIVE` مع current key NULL؛ ROTATE=`ACTIVE`؛ RECOVER=`ACTIVE|SUSPENDED|EXPIRED` ورفض REVOKED؛ ثم يأخذان user advisory ويفرضان actor CompanyId NULL أو نفس CompanyId، ثم يقفلان challenge row ويطلبان `ExpectedProofKeyVersion IS NOT DISTINCT FROM NEW.ExpectedProofKeyVersion` إضافة إلى تطابق FK المركب، وأن ConsumedAt NULL والتحدي غير منتهٍ؛ BIND يعطي 1، وROTATE/RECOVER يتطلب current version=Expected وPreviousThumbprint=current وResult=Expected+1. ledger INSERT يجب أن يقع قبل device update كما حسم 19.2.1. function `fn_device_key_change_append_only` وtrigger `trg_device_key_change_append_only BEFORE UPDATE OR DELETE` يرفضان UPDATE وDELETE؛ لا cleanup لهذا ledger في Stage 4.
- `sync_operations`: يحول `EntityId` إلى uuid NULL؛ ويضاف `ActionCode varchar(120)`, `ProtocolVersion varchar(20)`, `OperationCorrelationId uuid`, `ResultEntityId uuid NULL`, `RequestFingerprintVersion varchar(16)`, `RequestFingerprintHash bytea`, `ProofKeyVersion integer`, `ProofKeyThumbprint varchar(43)`, `AcceptedProofReplayId uuid`. صفوف legacy — ومنها صفوف Stage 3 ذات RegisteredDeviceId — تعرف بـ`RequestFingerprintVersion IS NULL`. `ck_sync_stage4_contract_bundle` يفرض إما جميع حقول Stage 4 التالية NULL: ActionCode/ProtocolVersion/OperationCorrelationId/RequestFingerprintVersion/RequestFingerprintHash/ProofKeyVersion/ProofKeyThumbprint/AcceptedProofReplayId، أو: `RequestFingerprintVersion='fp-v1'`, `ProtocolVersion='sync-v1'`, RegisteredDeviceId/BranchId وكل الحقول السابقة NOT NULL، OperationCorrelationId غير zero UUID، hash=32 byte، thumbprint=43، ProofKeyVersion>=1. `AcceptedProofReplayId` وsnapshot provenance يبقيان immutable بعد حذف replay، والفهرس `ix_sync_op_accepted_proof` على `(AcceptedProofReplayId)`.
- لا ينشأ trigger ثالث متداخل على sync_operations. تعدل migration الوظيفتين الحاليتين مع الإبقاء على trigger names/timing: `trg_sync_operations_device_binding BEFORE INSERT OR UPDATE` ينفذ `enforce_sync_operation_device_binding()` أولًا أبجديًا؛ و`trg_sync_operations_user_scope BEFORE INSERT OR UPDATE OF UserId,CompanyId,BranchId` ينفذ `enforce_sync_operation_user_scope()`. في `TG_OP='INSERT'` يرفض device-binding كل legacy row، ويثبت bundle ووجود replay مطابقًا حرفيًا لـCompanyId/RegisteredDeviceId/DeviceId/UserId/BranchId/ProofKeyVersion/ProofKeyThumbprint؛ DeviceAssignmentId يبقى provenance في replay لا في SyncOperation. في `TG_OP='UPDATE'` **لا يبحث عن replay محذوف ولا يعيد فحص current device/assignment/credential**؛ بل يمنع تغيير الحقول immutable التالية: CompanyId, BranchId, DeviceId, RegisteredDeviceId, RegisteredDeviceCredentialVersion, UserId, ActionCode, ProtocolVersion, OperationType, EntityType, EntityId, ClientOperationId, PayloadJson, PayloadHash, ClientOccurredAt, BaseVersion, OperationCorrelationId, RequestFingerprintVersion, RequestFingerprintHash, ProofKeyVersion, ProofKeyThumbprint, AcceptedProofReplayId، ثم يسمح status/result/retry transitions المصرح بها. user-scope function تحتفظ بالـadvisory lock المتبادل الحالي ثم تتحقق من Company/User/Branch references؛ ويأخذ proof claim القفل نفسه بعد device row وقبل assignment. لا توجد functions أو triggers جديدة باسم `fn/trg_sync_stage4_*`.
- يسقط index القديم بعد preflight وينشأ `ux_sync_op_registered_device_client` على `(CompanyId,RegisteredDeviceId,ClientOperationId) WHERE RegisteredDeviceId IS NOT NULL AND RequestFingerprintVersion='fp-v1'`، و`ux_sync_op_legacy_company_device_client` على `(CompanyId,DeviceId,ClientOperationId) WHERE RequestFingerprintVersion IS NULL`. بذلك لا تصنف صفوف Stage 3 خطأ كـStage 4.
- `audit_events` يبقي `CorrelationId uuid` الحالي بوصفه **AttemptCorrelationId** لكل محاولة، ويضاف `OperationCorrelationId uuid NULL` للحدث المرتبط بعنصر، مع index `ix_audit_event_operation_correlation` على `(OperationCorrelationId)`. أخطاء request-level تعيد JSON property `CorrelationId` بقيمة AttemptCorrelationId الحالية؛ نتائج items تعيد أيضًا OperationCorrelationId الأصلي. الحدث العام قد يبقي OperationCorrelationId NULL، ولا يخزن الاثنان داخل JSON غير مقيد.
- لا تمس migration الجديدة FK Stage 3 الحالي ذي الاسم المنطقي الطويل `FK_sync_operations_registered_devices_RegisteredDeviceId_CompanyId_DeviceId`؛ PostgreSQL اختصره فعليًا إلى `FK_sync_operations_registered_devices_RegisteredDeviceId_Compan`. إذا احتاج تغييرًا مستقبلًا فيجب اكتشاف الاسم عبر catalog أو مهاجرة rename مستقلة، لا DROP بالاسم الطويل.
- FK المركبة تثبت أن `RegisteredDeviceId` و`DeviceId` يتبعان `CompanyId`. خريطة `PostgresException.ConstraintName` الحرفية الوحيدة هي:

| ConstraintName | الإجراء/الخطأ الحاكم |
|---|---|
| `ux_sync_replay_device_key_jti` | `401 invalid_dpop_proof` بوصفه replay؛ لا enqueue. |
| `ux_sync_nonce_hash` | rollback ثم regenerate بحد 3 محاولات في معاملة جديدة؛ بعد الاستنفاد `NONCE_GENERATION_FAILED` داخلي عام. |
| `ux_registered_device_proof_thumbprint` | `409 DEVICE_PROOF_KEY_CONFLICT` بلا كشف الجهاز المالك. |
| `ux_device_key_challenge_request` | بعد rollback اقرأ بنفس `(RegisteredDeviceId,ChangeRequestId)`؛ الطلب المطابق يعيد metadata بلا raw challenge، وغير المطابق `409 PROOF_KEY_CHALLENGE_MISMATCH`. |
| `ux_device_key_change_request` | بعد rollback اقرأ النتيجة؛ fingerprint مطابق يعيدها، وغير مطابق `409 PROOF_KEY_CHANGE_MISMATCH`. |
| `ux_sync_op_registered_device_client` | بعد rollback اقرأ بـ`(CompanyId,RegisteredDeviceId,ClientOperationId)`؛ hash ثابت مطابق يعيد outcome، وغير مطابق `409 IDEMPOTENCY_MISMATCH`. |
| `ux_sync_op_legacy_company_device_client` | `409 LEGACY_IDEMPOTENCY_CONFLICT`. |

أي constraint غير موجود حرفيًا في الجدول يفشل داخليًا ولا يتحول إلى replay/idempotency. لا تستمر أي قراءة داخل معاملة PostgreSQL أبطلتها `23505` دون savepoint rollback صريح.
- يجب أن يطابق EF model/snapshot هذه الأسماء والأنواع حرفيًا، وأن تختبر migration تطبيقًا وrollback وreapply على PostgreSQL الحقيقي. أي تغيير في الاسم أو النوع أو delete behavior يحتاج تعديلًا حاكمًا قبل الكود؛ لا يفوض هذا السجل التخمين.

### 19.11 ترتيب التحقق والأخطاء والتدقيق

بعد فتح G5 فقط يكون الترتيب: valid Bearer/session → local registered-device binding/assignment → permission/scope → proof syntax/algorithm/key/signature → `htm/htu/iat/ath/nonce/tbh` → atomic jti claim → batch/protocol/action validation → per-operation idempotency/enqueue. لا يؤدي فشل عنصر تجاري إلى إلغاء proof claim أو نجاح عناصر أخرى وفق ذرية القسم 13.

- missing/expired nonce: `401 use_dpop_nonce` مع nonce جديد عند أهلية السياق.
- malformed/signature/key/freshness/ath/tbh/replay failure: `401 invalid_dpop_proof` ورسالة عامة مع JSON `CorrelationId` يساوي AttemptCorrelationId من header/proof فقط.
- device/session/assignment/permission/scope: `401/403` الحالي العام دون كشف وجود جهاز أو صف خارج النطاق.
- Offline gate المغلق: `403 OFFLINE_DISABLED` قبل nonce/proof state mutation.
- لا تتضمن الاستجابة أو AuditEvent أو logs raw credential/private key/public JWK/raw proof/raw nonce/raw jti/Bearer token أو body. يسجل التدقيق hashes/IDs/versions والنتيجة و`AttemptCorrelationId` و`OperationCorrelationId` فقط.

### 19.12 اختبارات القبول الإلزامية لقرار Stage 4

1. table-driven رفض missing/multiple/oversized/malformed proof، و`typ`/`alg`/curve/private-JWK/unknown-critical/duplicate-claim/signature/thumbprint failures.
2. حدود `iat` عند `-121/-120/+30/+31 seconds`، وnonce missing/wrong/expired/downgrade، وقبول current وrecent unexpired nonces للتوازي.
3. `htm` وcanonical `htu`، ورفض query/http/backend URI وHost أو `X-Forwarded-*` spoof من مصدر غير موثوق.
4. `ath` لBearer token الدقيق، و`tbh` للraw body، وتغيير whitespace/order/metadata/body بعد التوقيع يرفض قبل enqueue.
5. نفس `jti` sequential/concurrent وعبر أكثر من app instance يعطي قبول proof واحدًا، مع cleanup لا يحذف قبل 10 دقائق وعدم تخزين raw artifacts.
6. تعليق/إلغاء/انتهاء الجهاز أو التعيين، وتدوير proof key أثناء الطلب، يمنع المفتاح القديم ولا يترك عملية جديدة بلا provenance.
7. proof جديد مع نفس business operation يعيد النتيجة نفسها؛ وتغيير كل fingerprint field منفردًا يعطي `IDEMPOTENCY_MISMATCH`; ولا يزيد replay server retry counter؛ ويطابق encoder متجهات `fp-v1` الثابتة المحسوبة خارج production code.
8. تزامن عمليتين بنفس `(CompanyId, RegisteredDeviceId, ClientOperationId)` ينتج صفًا وأثرًا تجاريًا واحدًا؛ وتثبت شركتان بنفس DeviceId/ClientOperationId عدم التصادم؛ ولا يبتلع mapping أي `23505` غير معروف.
9. BIND/ROTATE/RECOVER وnew/old-key possession وsession revoke وchange idempotency، وسباق rotation-wins/claim-wins، وترتيب الأقفال وعدم deadlock.
10. migration preflight وlegacy collision والـpartial indexes وقيود INSERT وimmutable provenance و`Up -> Down -> Up` على PostgreSQL الحقيقي، مع إثبات Down fail-closed عند وجود Stage 4 data.
11. حدود body `2 MiB/+1` وpayload `16 KiB/+1` وdepth `32/33` وchunked وContent-Encoding، مع منع استهلاك proof في رفض request-level.
12. يبقى production `sync.offline.enabled=false` و`OFFLINE_DISABLED` حتى يثبت exact SHA في G4 ويصدر G5؛ test override لا يدخل إعداد إنتاج ولا يفتح External Authority.

المراجع المعيارية: [RFC 9449 §§4.2–4.3, 7, 9, 11.1–11.7](https://www.rfc-editor.org/rfc/rfc9449.html)، [RFC 7638](https://www.rfc-editor.org/rfc/rfc7638.html)، [RFC 8725](https://www.rfc-editor.org/rfc/rfc8725.html). أما أرقام `120s/30s/5m/10m` وclaim `tbh` ومصدر `public_origin` فهي قرارات TransportERP الموثقة أعلاه.
