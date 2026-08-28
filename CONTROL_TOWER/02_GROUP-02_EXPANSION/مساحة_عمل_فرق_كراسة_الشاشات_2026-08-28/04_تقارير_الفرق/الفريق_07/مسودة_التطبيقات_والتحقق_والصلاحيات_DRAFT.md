# مسودة الفريق 07 — التطبيقات والتسجيل والتحقق والصلاحيات والثقة الرقمية

**حالة التقرير:** `DRAFT — CORRECTED — NOT FOR HANDOFF`

**قيد حاكم:** هذه جولة تصحيح محدودة لمسودة TEAM-07 السابقة وفق مراجعة Wave A. لا تعيد التحليل من البداية، ولا تنشئ تصميمًا بصريًا أو عقد API/DDL/Permission أو سياسة قانونية/خصوصية أو تصريح برمجة، ولا تفتح TEAM-03 أو TEAM-06 أو TEAM-08.

## 1. بيانات الجولة والتتبع

| الحقل | القيمة |
|---|---|
| الفريق | 07 — التطبيقات والتسجيل والتحقق والصلاحيات والثقة الرقمية |
| الصفة | تصحيح قنوات الاستخدام والهوية والثقة والصلاحيات والأدلة وOffline فقط |
| التاريخ | 2026-08-29 |
| الفرع | `governance/control-tower-20260828-screens-workspace` |
| HEAD عند بدء TEAM-07-CORRECTION | `e3fed20944753d4b33c06868eb66763a2160f4ee` |
| حدود الكتابة | هذا الملف فقط داخل مجلد الفريق 07 |
| حالة البوابة | `DRAFT — CORRECTED — NOT FOR HANDOFF` |

### 1.1 استمرارية المسودة

عند بدء التصحيح كان المسار الإلزامي لهذه المسودة غير موجود في HEAD الفرع، بينما كانت نسخة TEAM-07 التي راجعتها Wave A محفوظة ضمن ملفات المشروع. استُخدمت آخر نسخة TEAM-07 التي كانت محل مراجعة Wave A كأساس تصحيحي، وأعيدت إلى **المسار نفسه المحدد في أمر الفريق** دون إنشاء ملف تقرير نهائي أو Population موازية.

هذا اكتشاف استمرارية مساحة عمل فقط، ولا يغير سلطة أي نتيجة سابقة ولا يفتح أي بوابة.

## 2. المدخلات الحاكمة والمستهلكة

| المرجع | النسخة/الهوية المستخدمة | الاستخدام في التصحيح |
|---|---|---|
| أمر TEAM-07 | `03_أوامر_الفرق/الفريق_07_التطبيقات_والتسجيل_والتحقق_والصلاحيات.md`؛ blob `1a0b914e92979348505e287aab800f4a247dbdc0` | حدود المهمة وملكية الهوية/الدليل/GPS/Offline |
| مراجعة Wave A | `WAVE_A_DRAFT_INDEPENDENT_REVIEW_2026-08-28_AR.md` | أوامر TEAM-07 CORRECTIONS وWA-R-011/013/014/015/016 |
| TEAM-04-CORRECTION | commit `9afec06269cd618d281743f6424c3ead6354d3a0`؛ blob `a99fd408c75c6e7888a5c3ef2bedc107e2acb630` | الوقائع التشغيلية، P04-16، أحداث العهدة/التسليم/RTO/المطالبة/التحصيل/GPS/POD |
| TEAM-05-CORRECTION | commit `e3fed20944753d4b33c06868eb66763a2160f4ee`؛ blob `3ae9ca38a9e0a7d36a000dd84e7b09f2c4d88b7a` | Handoff الأفعال المالية الحساسة التسعة ومتطلبات SoD |
| AUTH-001 | commit `e8d443dc5cefb6a1ea131311cfb7b2ded569b8df`؛ blob `cb60e2a434de275465fc33232983e177072b2885` | local application authority مع server-resolved tenant/permission/device authority |
| OFFLINE-001 | commit `e8d443dc5cefb6a1ea131311cfb7b2ded569b8df`؛ blob `4064125c3d5ba8439b4f9c295177c0c09006ef50` | fail-closed، queue envelope، replay re-authorization، online-authoritative boundaries |
| CLIENT-001 | commit `e8d443dc5cefb6a1ea131311cfb7b2ded569b8df`؛ blob `1fe32f285bedde7571046cc79f1165e2e4c7e980` | Desktop + Android Admin/Customer/Driver؛ iOS مؤجل |
| ACC-001 | commit `e8d443dc5cefb6a1ea131311cfb7b2ded569b8df`؛ blob `7487cbdd1e442f695c65a0776404cd77e861c48a` | CollectionTransaction تشغيلي، Settlement حد محاسبي، SoD threshold=0، reversal permission منفصلة |

**المدخلان المصححان المستهلكان صراحة في هذه الجولة:**

`TEAM-04-CORRECTION @ 9afec06269cd618d281743f6424c3ead6354d3a0`

`TEAM-05-CORRECTION @ e3fed20944753d4b33c06868eb66763a2160f4ee`

## 3. الحدود التي لم تتغير عن المسودة السابقة

تبقى الاستنتاجات التالية دون إعادة تحليل:

- `AUTH-001` اختار local application authority، لكنه لم يحسم MFA factors أو step-up triggers أو مدد الجلسات أو recovery policy.
- `CLIENT-001` يثبت Desktop وثلاثة Android clients فقط: Admin/Customer/Driver. توزيع تطبيق السوق/الناقل/مالك المركبة داخل هذه القنوات غير محسوم.
- مشروعات Mobile التي فُحصت في الجولة الأصلية لم تثبت runtime ميدانيًا مكتملًا لـGPS/POD/Offline client.
- token لا يصبح tenant/permission/device authority؛ السلطة الفعلية تعاد من الخادم وفق الحالة الحالية.
- الأدلة الحساسة، GPS الخلفي، صور الهوية، البصمة، retention/deletion والمشاركة تحتاج سياسة مثبتة ولا تُحسم بالتخمين.
- الحدث التشغيلي تملكه TEAM-04؛ الأثر المالي تملكه TEAM-05؛ TEAM-07 يقتصر على الهوية والدليل والوصول والصلاحية وOffline/replay.

## 4. القنوات والهوية والجلسة والجهاز

| القناة | الحالة الحاكمة | ملاحظة TEAM-07 |
|---|---|---|
| Desktop | Release target مثبت في CLIENT-001 | موظفو المكتب/الإدارة بحسب permission وscope الخادمي؛ اسم القناة لا يمنح Action |
| Mobile Admin Android | Release target مثبت | مراجعة/إدارة بحسب permission وscope؛ لا تفويض عام |
| Mobile Customer Android | Release target مثبت | العميل يرى/ينفذ ما يخصه وفق الخدمة والهوية والعقد |
| Mobile Driver Android | Release target مثبت | السائق/المندوب الميداني بحسب التعيين والpermission والscope |
| تطبيق السوق/الناقل/مالك المركبة | غير محسوم داخل CLIENT-001 | `NEEDS OWNER DECISION` بشأن التوزيع داخل القنوات الحالية أو قرار client إضافي |
| iOS | مؤجل | خارج نطاق الإصدار الحالي |

قواعد الهوية والجلسة والجهاز الحاكمة:

1. Authentication منفصل عن authorization.
2. tenant/company/branch/effective permission/device/session authority تعاد من الخادم عند الطلب/replay.
3. revoke/expiry يعامل fail-closed ويجمّد outbound المحمي إلى أن تتم إعادة المصادقة/المعالجة المسموحة.
4. لا ينقل queue محمي بين مستخدم أو شركة أو فرع أو جهاز مختلف دون عقد صريح؛ لا يوجد عقد من هذا النوع مثبت هنا.
5. MFA/step-up/session durations/recovery تبقى `NEEDS OWNER DECISION` عندما لا يوجد قرار مثبت.

## 5. الخصوصية وGPS وPOD — الحدود الحاكمة

### 5.1 GPS

المسودة لا تعتمد background tracking كافتراض عام. يلزم قرار حاكم يحدد على الأقل:

- الغرض من الجمع.
- بدء/إيقاف التتبع.
- المصدر: هاتف أم جهاز مركبة أم مصدر آخر.
- freshness/delay المقبول.
- accuracy إن كانت مطلوبة.
- cadence.
- من يرى الموقع الحي والتاريخ.
- مشاركة العميل.
- retention/deletion/legal hold.
- أثر رفض أو سحب الإذن.
- ضوابط التلاعب/المحاكاة/إعادة الإرسال.

### 5.2 POD

لا يعتمد نوع POD واحد لجميع الخدمات. الحد الأدنى المتناسب لكل خدمة/خطر/منطقة يبقى `NEEDS OWNER DECISION`، وقد يشمل عند ثبوت السياسة OTP أو توقيعًا أو صورة أو بيانات وقت/موقع أو خليطًا منها. البصمة الحيوية لا تعتمد دون ضرورة وقاعدة قانونية مثبتتين.

### 5.3 الأدلة الحساسة

- evidence capture لا يساوي final/legal acceptance.
- attachment staging لا يساوي server acceptance.
- الوصول إلى صور الهوية/POD/GPS/history يحتاج permission وscope مثبتين.
- لا retention duration مخترعة في هذه المسودة.

## 6. وظائف A11 القائمة — دون تغيير Population

تظل إحالات A11 الموجودة في المسودة السابقة مرجعًا فقط، ومنها:

- `PLT-001..003` للدخول/MFA/TenantContext.
- `SEC-001..020` للمستخدم/الدور/permission/scope/session/device/audit/privacy.
- `MOB-001/002/006/010/014..037` للقنوات Mobile والعمليات الميدانية.
- `PTY-006/009` للناقل/مقدم الخدمة والوثائق.
- `FLT-002/003/007/008/012/018/019/029` للمركبة والسائق وGPS/share tracking.
- `SHP-031/037` للتتبع/POD.
- `LMD-013/017` للتتبع/POD في last-mile.
- `INT-011/012` للمراقبة الإدارية، لا كإثبات client queue UI.
- `TRV-035/039` للبيع/المعلومات الحية، مع خضوع أي Offline action لـOFFLINE-001.

لا يثبت أي ID أعلاه وحده Permission أو موضع شاشة نهائي أو Action authority.

## 7. إزالة Local Candidate IDs

تم إلغاء صفة المعرف عن **العناصر السبعة** التي كانت مسجلة في المسودة السابقة كـLocal Candidate. لا يوجد لها بديل ID، ولا Population موازية، ولا توريث إلى TEAM-03/06/08.

| الوصف الوظيفي فقط | الحالة | حد الاستخدام |
|---|---|---|
| استرداد الحساب/كلمة المرور | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | flow محتمل تحت login/security؛ ليس هوية شاشة |
| تفعيل الحساب/الخدمة ومراجعة onboarding | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | يجب فصل طلب الخدمة عن اعتماد الصلاحية؛ ليس هوية شاشة |
| تسجيل الناقل/مالك المركبة/مقدم الخدمة | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | قناة السوق نفسها غير محسومة؛ ليس هوية شاشة |
| إدارة موافقة المستخدم وسحبها | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | privacy/consent function؛ ليس هوية شاشة |
| enrollment/recovery/transfer للجهاز | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | device lifecycle function؛ ليس هوية شاشة |
| حالة queue المحلية والتعارضات للمستخدم الميداني | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | client component محتمل؛ `INT-011` لا يثبته؛ ليس هوية شاشة |
| تفسير إذن GPS وحالة التتبع الخلفي | `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` | OS/application handoff؛ ليس هوية شاشة |

## 8. نتائج ذرية مصححة من المسودة السابقة

| ID موجود سابقًا | النوع | النتيجة بعد التصحيح | الحالة |
|---|---|---|---|
| `T07-F-001` | حقيقة | local application authority مختارة | ثابت من AUTH-001 |
| `T07-F-002` | حقيقة | ثلاث قنوات Android فقط ضمن CLIENT-001 | ثابت |
| `T07-F-003` | تعارض | نموذج السوق المقترح لا يطابق client scope مباشرة | `NEEDS OWNER DECISION` |
| `T07-F-004` | فجوة | Mobile runtime لم يكن مثبتًا في الجولة الأصلية | فجوة تنفيذ؛ لا تعاد مراجعتها هنا |
| `T07-F-005` | فجوة | login/session lifecycle الكامل غير مثبت في Source الجولة الأصلية | فجوة تنفيذ؛ لا قرار شاشة |
| `T07-F-006` | فجوة | permission/device authority لا يجوز أن تعتمد claims عميل كحقيقة نهائية | ثابت من AUTH-001 |
| `T07-F-007` | فجوة | device/session/PoP persistence تخضع لبواباتها الحاكمة | ثابت كحد حوكمي |
| `T07-F-008` | قاعدة | `ELIGIBLE CATEGORY DOES NOT MEAN ACTION AUTHORITY` | مصحح وفق Wave A/OFFLINE-001 |
| `T07-F-009` | قاعدة | لا silent merge للحسابات/العهدة/الكميات/الموافقات/الأمن | ثابت |
| `T07-F-010` | فجوة | قاموس Queue الحالي لا يطابق كامل النموذج التحليلي | `NEEDS TEAM CORRECTION — QUEUE GOVERNANCE` |
| `T07-F-011` | سؤال | MFA/step-up/session durations غير محسومة | `NEEDS OWNER DECISION` |
| `T07-F-012` | سؤال | GPS background purpose/cadence/retention غير محسومة | `NEEDS OWNER DECISION` |
| `T07-F-013` | سؤال | POD/هوية المستلم/البصمة تحتاج أقل جمع وسياسة | `NEEDS OWNER DECISION` |
| `T07-F-014` | تعارض مرجع | A11 fingerprint reconciliation | `GOVERNANCE DELTA` — ليس قرار مالك وظيفيًا |
| `T07-F-015` | فجوة | Application IDs التي كانت في scaffold لا تطابق CLIENT-001 | `GOVERNANCE DELTA`/تنفيذ لاحق |

## 9. استهلاك Handoff المالي من TEAM-05 — مصفوفة الأفعال الحساسة

قاعدة المصفوفة: لا يُنشأ Role أو Permission جديد. أي خانة لا تثبتها TEAM-05/ACC-001/AUTH-001/OFFLINE-001 تبقى `UNRESOLVED`. `Settlement` محاسبي يخضع دائمًا للحد الأدنى من SoD في ACC-001: `SoD threshold = 0`، collector لا يعتمد/يرحل تسويته الخاصة، وSettlement maker لا يكون final approver/poster.

| Action | Actor | Company/Branch/Entity Scope | Permission | Maker | Checker | Approver/Poster | Channel | Offline/Replay | Audit | Decision Gap |
|---|---|---|---|---|---|---|---|---|---|---|
| التقاط التحصيل | Collector؛ قد يكون موظف فرع/سائق/مندوب/وكيل بحسب الواقعة، والهوية الدقيقة لكل قناة `UNRESOLVED` | CollectionTransaction مرتبط بالمصدر/البوليصة + amount/currency/party + company/branch/entity context | `UNRESOLVED` | المحصل/منشئ أمر الالتقاط | لا Checker محاسبي مثبت لمجرد الالتقاط؛ server validation إلزامي عند replay | لا posting عند الالتقاط؛ collector ممنوع من اعتماد/ترحيل تسويته الخاصة | `UNRESOLVED` | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; replay يعيد tenant/permission/device/session/idempotency/amount/currency/state validation | ClientOperationId + immutable provenance + server outcome/audit مطلوب عند التعاقد | Action identity/permission/channel/envelope غير مثبتة |
| تعديل السعر | `UNRESOLVED` — دور تسعير مخول غير مثبت بالاسم | Quote/Price Snapshot/Customer Charge ضمن company/branch/entity scope غير المثبت تفصيلًا | `UNRESOLVED` | طالب/صانع التعديل غير مثبت بالدور | `UNRESOLVED` | سلطة قبول override وحدودها `UNRESOLVED` | `UNRESOLVED` | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` | قبل/بعد + actor + reason + scope + correlation مطلوب عند التعاقد | OWNER: من يملك التعديل والحدود؛ TEAM-07 لا يخترع Permission |
| إعداد Settlement | Settlement maker كوظيفة SoD؛ اسم الدور `UNRESOLVED` | accepted collections/custody/differences ضمن company/branch scope المصرح؛ تفاصيل scope `UNRESOLVED` | `UNRESOLVED` | Settlement maker | maker-checker separation إلزامي عند settlement accounting boundary | maker لا يكون final approver/poster | قناة مالية مخولة؛ الاسم/القناة `UNRESOLVED` | أي posting `ONLINE AUTHORITATIVE`; إعداد action نفسه غير مفوض Offline دون عقد | source rows + maker + version/state + correlation + immutable audit | role/scope/action identity/workflow states غير مثبتة |
| مراجعة Settlement | Reviewer/Checker كوظيفة؛ اسم الدور `UNRESOLVED` | Settlement + source rows/attachments/differences ضمن scope مخول | `UNRESOLVED` | ليس maker في حالة تحقق الفصل | Checker منفصل عن maker | كون checker نفسه final approver/poster غير محسوم فوق حد ACC-001 | `UNRESOLVED` | `ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; لا اعتماد مالي Offline | reviewer identity + outcome + reason + correlation + immutable audit | هل يلزم فصل إضافي reviewer/poster غير مثبت |
| اعتماد/ترحيل Settlement | Final approver/poster كوظيفة؛ اسم الدور `UNRESOLVED` | Settlement مكتمل ضمن company/branch/fiscal period المسموح | `UNRESOLVED` | لا يكون maker | maker-checker separation إلزامي | final approver/poster؛ collector لا يعتمد/يرحل تسويته الخاصة | `ONLINE AUTHORITATIVE` | `DENY/ONLINE AUTHORITATIVE` | poster identity + source bindings + voucher/journal refs + period + correlation + immutable audit | permission code/role/scope limits `UNRESOLVED` |
| عكس Settlement/التصحيح المحاسبي | مستخدم مخول بالعكس؛ اسم الدور `UNRESOLVED` | الأصل + reason + allowed fiscal period + company/branch/source references | `UNRESOLVED — distinct reversal permission required by ACC-001` | منفذ العكس | مراجعة إضافية `UNRESOLVED` | approver إضافي إن وجد `UNRESOLVED`; الأصل لا يحذف | `ONLINE AUTHORITATIVE` | `DENY/ONLINE AUTHORITATIVE` | mandatory reason + original link + correlation + immutable audit | اسم permission وحدود scope/approval الإضافي غير مثبتة |
| الاسترداد | Actor مالي مخول `UNRESOLVED` | original payment/collection + amount/part + channel + reason + company/entity context | `UNRESOLVED` | منشئ طلب الاسترداد `UNRESOLVED` | `UNRESOLVED` | اعتماد الاسترداد وحدوده `UNRESOLVED` | القناة/استثناءاتها `UNRESOLVED` | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; أي accounting posting/reversal Online authoritative | original reference + actor + reason + amount + approval/rejection + immutable audit | OWNER: policy/fees/limits/channel exceptions; actor/SoD/permission غير مثبتة |
| اعتماد/تسوية العمولة | Actor عمولات/مالية `UNRESOLVED` | Commission entitlement by beneficiary/base/trigger/executed part; company/entity scope تفصيلي `UNRESOLVED` | `UNRESOLVED` | صانع الاحتساب/التسوية `UNRESOLVED` | مراجعة وظيفية مطلوبة كمرشح؛ checker permission `UNRESOLVED` | approver/poster `UNRESOLVED` | `UNRESOLVED` | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; posting المالي Online authoritative | rule/version/basis/beneficiary/executed part + actor/review/approval + audit | OWNER: trigger/base/approval policy; TEAM-07 لا يخترع actor/permission |
| تسوية عهدة السائق/المندوب | Actor مالي/عهدة `UNRESOLVED` | collections attributed + remittance + differences under FIN-034/035 context; scope تفصيلي `UNRESOLVED` | `UNRESOLVED` | maker تسوية العهدة كوظيفة؛ اسم الدور `UNRESOLVED` | maker-checker عند الحد المحاسبي | collector لا يعتمد/يرحل تسويته الخاصة | posting `ONLINE AUTHORITATIVE`; قناة الإعداد `UNRESOLVED` | الترحيل `DENY/ONLINE AUTHORITATIVE`; أي capture سابق يبقى غير مفوض دون عقد | source collections + custody/remittance/differences + actor/checker/poster + immutable audit | OWNER: shortage/overage policy; actor/scope/permission غير مثبتة |

**الحصيلة:** تمت مطابقة الأفعال المالية الحساسة التسعة فقط، دون إنشاء Role أو Permission أو Action ID جديد.

## 10. P04-16 — حكم قوة المدخل فقط

`P04-16` في TEAM-05 بقي `STILL UNRESOLVED` للوقائع التالية: الكيلومترات، وقت/موقع الحدث، الوقود، ورسوم الطريق كوقائع تشغيلية. هذا القسم لا يقرر سعرًا أو بدلًا أو خصمًا أو أثرًا ماليًا.

### 10.1 فحص TEAM-07 للدليل

| البند | ما هو مثبت | الفجوة المؤثرة في قوة المدخل |
|---|---|---|
| مصدر GPS/الموقع | TEAM-04 يحدد GPS/المركبة/الرحلة كمصدر تشغيلي محتمل | لا يوجد contract مثبت يميز هاتفًا من جهاز مركبة لكل sample، ولا runtime ميداني مثبت في الجولة الأصلية |
| هوية الجهاز/المستخدم | AUTH-001/OFFLINE-001 يفرضان server-resolved user/company/branch/device/session provenance | device registry/assignment/PoP التنفيذية لم تثبتها الجولة الأصلية؛ لا يجوز اعتبار claim عميل سلطة |
| Timestamp | OFFLINE-001 يفرض client occurred-at + server received-at | لا يوجد policy مثبت لحدود clock skew أو freshness المقبولة لـP04-16 |
| حداثة الموقع | غير مثبتة | لا freshness threshold أو max-age معتمد |
| دقة الموقع | غير مثبتة | لا accuracy requirement أو measured accuracy evidence |
| الهاتف مقابل جهاز المركبة | غير محسوم | لا source precedence أو source-binding معتمد |
| قوة/سلامة الدليل | envelope/hash/provenance/replay requirements مثبتة كحد حاكم | لا attestation/anti-spoof/tamper evidence ميداني مثبت للموقع/الوقود/رسوم الطريق |
| احتمالات التلاعب | يجب fail-closed وعدم قبول client-authoritative trust | لا سياسة مثبتة لكشف mock location أو gallery substitution أو sensor/device mismatch |
| صلاحية الوصول | permission/scope يجب أن تكون server-resolved | من يرى/يعدل/يعتمد كل عنصر من P04-16 غير مثبت تفصيليًا |
| Offline capture/replay | eligible category قد تكون ممكنة لبعض append-only events إذا اكتمل العقد | لا Action identity/permission/envelope/version/conflict rule خاص بـP04-16؛ يبقى DENY/UNRESOLVED |
| Audit | immutable provenance/correlation/replay outcome مطلوبة | field-level audit policy لوقود/رسوم/odometer/GPS source غير مثبتة |

### 10.2 الحكم

`P04-16 INPUT STRENGTH = INSUFFICIENT EVIDENCE`

السبب: توجد **حدود تحكم حاكمة** جيدة للهوية/replay/audit، لكن الدليل المتاح لا يثبت بعد مصدرًا ميدانيًا متعاقدًا، source binding، freshness، accuracy، anti-tamper أو access/action authority كافية لجعل وقائع P04-16 مدخلًا قويًا. لا ينتج عن هذا الحكم أي قرار مالي.

## 11. Crosswalk محدود لأحداث TEAM-04

TEAM-04 هو مرجع الواقعة التشغيلية. الجدول التالي لا يعيد تعريف event/state machine؛ يربط فقط الهوية والدليل والوصول والاعتماد وOffline.

| Event | Actor | Evidence | Identity Requirement | GPS | Who Can View | Who Can Act | Who Can Approve | Offline State | Owner Decision Gap |
|---|---|---|---|---|---|---|---|---|---|
| انتقال العهدة | الحامل الحالي + المستلم التشغيلي بحسب TEAM-04؛ role/security mapping تفصيلي `UNRESOLVED` | unit/manifest/custody references + طرفان + وقت + نتيجة handover؛ أي صورة/توقيع حسب policy | user/company/branch + current membership/permission + device/session provenance عند القناة الميدانية | سياق فقط ما لم تفرض خدمة/سياسة غير مثبتة | `UNRESOLVED` حسب permission/scope | الأطراف المعينة تشغيليًا فقط بعد permission؛ mapping `UNRESOLVED` | `UNRESOLVED` | append-only custody category قد تكون eligible، لكن `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` | هل يلزم POD/توقيع/OTP/موقع لكل نوع انتقال |
| قبول/رفض العهدة | المستلم التشغيلي المعين؛ identity mapping `UNRESOLVED` | accept/reject + reason + unit/manifest refs + time + الطرفين | current user/membership/scope + device/session provenance | `UNRESOLVED` كمتطلب | `UNRESOLVED` | المستلم المعين إذا ثبت permission | approval إضافي `UNRESOLVED` | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` | الدليل الأدنى عند الرفض/الاستلام الخارجي |
| التسليم الجزئي | منفذ التسليم + سياق المستلم؛ security roles تفصيلًا `UNRESOLVED` | الوحدات المقبولة/المرفوضة + reason + attempt/time + POD وفق policy | منفذ مخول + source task/company/branch/entity + device/session | location context مفيد لكن إلزامه/دقته `NEEDS OWNER DECISION` | منفذ/مراجعون/عميل وفق data minimization؛ permissions `UNRESOLVED` | منفذ المهمة إذا ثبت permission | final/legal close authority `UNRESOLVED` | capture قد يكون eligible category؛ final close لا يفوض Offline | الحد الأدنى لـPOD وهوية المستلم حسب الخدمة/الخطر/المنطقة |
| تعذر التسليم | منفذ المهمة | reason + attempt time + address/contact context + available evidence | current task assignment + user/device/session provenance | location/time قد يدعمان الواقعة؛ requirement/freshness `UNRESOLVED` | `UNRESOLVED` | منفذ المهمة إذا ثبت permission | إعادة محاولة/RTO approval إن وجد `UNRESOLVED` | append-only exception capture category فقط؛ action authority غير متعاقد | عدد المحاولات وسياسة الموقع/POD والخصوصية |
| رفض المستلم | منفذ التسليم + recipient context؛ recipient identity level `UNRESOLVED` | refusal reason + affected units + time + POD/evidence حسب policy | actor authenticated؛ recipient verification level `NEEDS OWNER DECISION` | context فقط؛ إلزامه غير مثبت | `UNRESOLVED` | منفذ المهمة إذا ثبت permission | next-step/RTO approval `UNRESOLVED` | capture category فقط؛ `ACTION DENY/UNRESOLVED UNTIL CONTRACTED` | إثبات الرفض الأدنى وهوية المستلم/التوقيع/OTP |
| RTO | actor تشغيلي يملكه TEAM-04؛ security mapping `UNRESOLVED` | source shipment + failed/refused delivery refs + reason + custody transition + time | authenticated actor + company/branch/entity scope + current assignment | `UNRESOLVED` كمتطلب | `UNRESOLVED` | `UNRESOLVED` حتى permission/action contract | `UNRESOLVED` | event capture قد يكون eligible category؛ final state/authority تخضع server validation | متى يبدأ RTO ومن يحمل العهدة بعده وما دليل الانتقال الأدنى |
| المطالبة/التلف/النقص/الفقد | reporter + custodians/inspectors according to TEAM-04; security roles `UNRESOLVED` | event type + affected units/quantity + first discovery + custody chain + reason + photos/docs only if policy permits | authenticated actor + scope + device/session for field capture | location may support discovery/custody; requirement `UNRESOLVED` | least-privilege; sensitive evidence access `UNRESOLVED` | report/capture permission `UNRESOLVED` | claim liability/financial approval خارج 07؛ evidence acceptance `UNRESOLVED` | incident capture category only; attachments staging depends on policy | evidence minimum, sensitive-media policy, legal/privacy, claim responsibility outside 07 |
| التحصيل | Collector كما تثبته الواقعة؛ channel identity mapping `UNRESOLVED` | CollectionTransaction source + amount/currency/party + time/location context + ClientOperationId/provenance عند Offline contract | user/company/branch/device/session + current permission + source entity scope | location operational context؛ إلزامه/دقته غير مثبتة | collector + authorized finance/operations according to permission; mapping `UNRESOLVED` | collector may capture only if action contracted | collector cannot approve/post own settlement; posting authority Online | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; server re-authorizes on replay; no accounting posting | actor×channel×scope/permission + financial policies from TEAM-05 |
| GPS | assigned field actor/device/vehicle relationship `UNRESOLVED` | sample source + coordinates + timestamps + task/trip ref + accuracy if captured + provenance | current user/task + device/session; vehicle-device binding if applicable | هو الحدث نفسه؛ source/freshness/accuracy/precedence غير مثبتة | `NEEDS OWNER DECISION` للموقع الحي/history/share tracking | capture فقط إذا consent/purpose/permission/action contract مكتمل | لا approval وظيفي مثبت؛ acceptance server-side وفق contract | `UNRESOLVED`; لا queue authority حتى consent/purpose/cadence/retention/action contract | background tracking, source precedence, cadence, accuracy, retention, sharing, anti-tamper |
| POD | منفذ التسليم + recipient context | service-specific POD set: OTP/signature/photo/time/location بحسب قرار لاحق؛ server ack منفصل | actor authenticated + task/scope/device/session; recipient proof level بحسب policy | location may be one signal, not proof alone | least-privilege; customer view only required subset; permissions `UNRESOLVED` | field capture إذا action contracted | final/legal delivery acceptance authority `UNRESOLVED` | metadata capture may be eligible category; sensitive attachments `UNRESOLVED`; final authority not granted Offline | POD minimum by service/risk/region, identity/photo/biometric legality and retention |

**الحصيلة:** تم Crosswalk لـ`10` أحداث TEAM-04 فقط.

## 12. Offline — القاعدة المصححة الحاكمة

تثبت هذه المسودة العبارتين دون استثناء لغوي:

`ELIGIBLE CATEGORY DOES NOT MEAN ACTION AUTHORITY`

`ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`

أهلية فئة عامة وفق OFFLINE-001 **لا** تمنح Action authority. لا يصبح أي فعل مسموحًا للتنفيذ/الإرسال بصفته Action حتى يثبت له جميع الآتي:

1. Action identity.
2. Permission.
3. Envelope.
4. Protocol/version.
5. Entity base/expected version عند الحاجة.
6. Conflict handling + deterministic conflict owner.
7. Replay rule/idempotency.
8. Server-side re-authorization على current authority.
9. Immutable audit/provenance.

كل نقص في واحد من هذه العناصر يبقي الفعل `DENY/UNRESOLVED`.

### 12.1 مصفوفة Offline السابقة — تصحيح الدلالة دون إنشاء Actions جديدة

الـIDs التالية كانت موجودة في المسودة السابقة؛ لم تُنشأ في هذه الجولة. التصنيف هنا يصحح فقط دلالة الفئة مقابل سلطة الفعل.

| ID موجود سابقًا | Action تحليلي | الحالة المصححة |
|---|---|---|
| `OF-001` | Login/token issue/refresh | `DENY/ONLINE AUTHORITATIVE` |
| `OF-002` | Password/account recovery | `DENY/ONLINE AUTHORITATIVE` |
| `OF-003` | MFA enrollment/challenge/reset | `DENY/ONLINE AUTHORITATIVE` |
| `OF-004` | إنشاء حساب/تفعيل خدمة/اعتماد ملف | `DENY/ONLINE AUTHORITATIVE`؛ local draft غير سلطوي فقط إن نص العقد لاحقًا |
| `OF-005` | Role/permission/scope/delegation/block | `DENY/ONLINE AUTHORITATIVE` |
| `OF-006` | Device enroll approve/assign/transfer/revoke/recover | `DENY/ONLINE AUTHORITATIVE` |
| `OF-007` | Server logout/revoke command | `DENY/ONLINE AUTHORITATIVE`؛ local credential clearing فوري ليس server acceptance |
| `OF-008` | Consent withdrawal | server effect `DENY/ONLINE AUTHORITATIVE`; إيقاف الجمع محليًا عند السحب لا يعني قبول الخادم |
| `OF-009` | draft طلب خدمة | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-010` | draft بوليصة | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-011` | Submit/approve/finalize/official numbering | `DENY/ONLINE AUTHORITATIVE` |
| `OF-012` | draft حجز راكب | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-013` | إصدار تذكرة/تثبيت مقعد/بيع نهائي | `DENY/ONLINE AUTHORITATIVE` |
| `OF-014` | قبول مهمة/عهدة كحدث pending | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-015` | Scan/load/arrival/unload/handover capture | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-016` | correction/reversal تشغيلي للكمية/العهدة | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; عملية جديدة فقط، لا rewrite |
| `OF-017` | POD metadata pending | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-018` | Final delivery close/accept | `DENY/ONLINE AUTHORITATIVE` إذا أنشأ authority نهائية/قانونية |
| `OF-019` | Photo/signature attachment staging | `UNRESOLVED`; DENY حتى attachment/privacy policy + action contract |
| `OF-020` | صورة هوية recipient/driver staging | `UNRESOLVED`; DENY حتى necessity/legal/privacy/access/retention contract |
| `OF-021` | Biometric fingerprint capture | `DENY` حتى قرار صريح بضرورة وقانونية |
| `OF-022` | OTP verification Offline | `UNRESOLVED`; DENY حتى anti-replay/expiry/issuer contract |
| `OF-023` | GPS samples أثناء مهمة | `UNRESOLVED`; DENY حتى consent/purpose/cadence/retention/source/action contract |
| `OF-024` | Cached read للخرائط/المهام/الحالة | read-only حسب classification؛ يظهر as-of/last-sync ولا يمثل authority حالية |
| `OF-025` | Collection capture كـPENDING command | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; لا posting |
| `OF-026` | Settlement/posting/unpost/reversal/period change | `DENY/ONLINE AUTHORITATIVE` |
| `OF-027` | Vehicle inspection/fuel/odometer metadata | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; الصور منفصلة عن metadata |
| `OF-028` | Incident/damage/shortage/exception report | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-029` | driver/vehicle/ownership/guarantee master-data change | `DENY/ONLINE AUTHORITATIVE` |
| `OF-030` | Notification/task acknowledgement | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED` |
| `OF-031` | destructive delete/history rewrite | `DENY` |
| `OF-032` | conflict resolution/override | `DENY/ONLINE AUTHORITATIVE` |
| `OF-033` | reapply after conflict | `ELIGIBLE CATEGORY ONLY — ACTION DENY/UNRESOLVED UNTIL CONTRACTED`; عملية جديدة فقط بعد server state review |

### 12.2 Queue envelope

أي Action يُتعاقد لاحقًا ضمن فئة eligible يجب أن يربط:

- stable ClientOperationId.
- protocol version.
- entity/action identity.
- payload hash.
- expected/base version عندما يلزم.
- user/company/branch/device/session provenance.
- client occurred-at + server received-at.
- server permission/action code.
- bounded retention metadata.
- deterministic conflict owner.
- replay rule/idempotency.
- server-side re-authorization.
- local protection at rest.

لا silent merge للحسابات أو العهدة أو الكميات أو الموافقات أو الأمن.

## 13. Queue وA11 — إعادة التصنيف الحوكمي

### 13.1 Queue vocabulary

المصدر الحالي الذي راجعته المسودة الأصلية كان يستخدم vocabulary مختلفًا عن النموذج التحليلي `pending/sending/accepted/rejected/conflict/frozen/retry`.

الحالة بعد التصحيح:

`NEEDS TEAM CORRECTION — QUEUE GOVERNANCE`

هذه ليست سياسة وظيفية للمالك. يجب أن تُصالح vocabulary مع contract/implementation الحاكم في مسار الفريق/الحوكمة عند فتح بوابته، مع الحفاظ على الأدلة القديمة وعدم اختراع state transition هنا.

### 13.2 A11 fingerprint reconciliation

الحالة بعد التصحيح:

`GOVERNANCE DELTA — A11 FINGERPRINT RECONCILIATION`

هذه ليست `OWNER DECISION`. يجب أن تُعالج كسجل حوكمي مركزي/مرجعي لاحقًا؛ TEAM-07 لا يخلق fingerprint بديلًا ولا يرفعها كقرار وظيفي للمالك.

## 14. التعارضات والفجوات والمخاطر — الحالة المصححة

| ID موجود سابقًا | التصنيف | الحالة بعد التصحيح |
|---|---|---|
| `T07-C-001` | CLIENT-001 مقابل نموذج تطبيق السوق | `NEEDS OWNER DECISION` |
| `T07-C-002` | App IDs scaffold مقابل CLIENT-001 | `GOVERNANCE DELTA`/تنفيذ لاحق |
| `T07-C-003` | iOS config مقابل scope المؤجل | `GOVERNANCE DELTA`؛ CLIENT-001 يحكم النطاق |
| `T07-C-004` | ADR قديم مقابل AUTH-001 | `GOVERNANCE DELTA`؛ AUTH-001 الأحدث يحكم |
| `T07-C-005` | Offline contracts أقدم مقابل OFFLINE-001 | `GOVERNANCE DELTA`؛ OFFLINE-001 يحكم |
| `T07-C-006` | A11 fingerprint | `GOVERNANCE DELTA`؛ ليس قرار مالك |
| `T07-C-007` | Mobile runtime غير مثبت في الجولة الأصلية | فجوة تنفيذ؛ لا يعاد فحصها هنا |
| `T07-C-008` | permission/device authority من claims | فجوة أمن/تنفيذ؛ AUTH-001 يفرض server authority |
| `T07-C-009` | device registry/assignment/PoP غير مثبتة | فجوة تنفيذ/DB-GOV |
| `T07-C-010` | Sync provenance/action contract غير مكتمل | `NEEDS TEAM CORRECTION`/تنفيذ لاحق |
| `T07-C-011` | privacy للهوية/GPS/biometric | `NEEDS OWNER/LEGAL/SECURITY DECISION` |
| `T07-C-012` | local UI يعرض حالة نهائية قبل ack | خطر؛ server acceptance authoritative |

## 15. القرارات الحقيقية للمالك فقط

لا تحسم هذه الجولة ما يلي:

1. توزيع تطبيق السوق/الناقل/مالك المركبة ضمن قنوات CLIENT-001، أو إصدار قرار client إضافي.
2. الحد الأدنى المتناسب لـPOD/هوية المستلم/OTP/توقيع/صورة لكل خدمة/خطر/منطقة.
3. GPS background tracking: الغرض، المصدر/الأولوية، البدء/الإيقاف، cadence، accuracy/freshness، retention، من يرى، مشاركة العميل، أثر رفض/سحب الإذن.
4. MFA factors، step-up triggers، session durations/concurrency، recovery إذا لم تغطها سلطة مثبتة لاحقًا.
5. الضمانات ومستويات الثقة والاعتراض والتصحيح.
6. أي قرار قانوني/خصوصية بشأن صور الهوية، biometric، tracking، consent، retention/deletion/legal hold.
7. سياسات TEAM-05 التي بقيت `NEEDS OWNER DECISION`، ومنها على سبيل الحصر دون حسم: أولوية مصادر السعر، سلطة/حدود تعديل السعر، binding/repricing policy، الوزن القابل للتحصيل، الضرائب/الجمارك/الرسوم وملكية المبلغ، revenue recognition، Commission/Driver Pay/Carrier Cost triggers/bases، تخصيص الاستحقاق بين الرحلات/الناقلين، نقص/زيادة العهدة والمقاصة، refund policy/fees/channel exceptions، claim liability/compensation، Settlement workflow authorities فوق الحد الأدنى لـACC-001، branch/agent uncleared remittance policy، وأي dynamic pricing policy.

ليست قرارات مالك في هذه المسودة:

- موضع شاشة/تبويب/مكون.
- إنشاء IDs أو Population.
- قاموس Queue.
- A11 fingerprint reconciliation.
- SHA/citation/crosswalk corrections.

## 16. ملخص التصحيحات المنفذة

| البند | النتيجة |
|---|---|
| Local Candidate identifiers | أزيلت صفة ID من العناصر السبعة؛ أصبحت أوصافًا `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` فقط |
| TEAM-05 financial handoff | تمت مطابقة `9` أفعال حساسة في مصفوفة Actor/Scope/Permission/SoD/Channel/Offline/Audit |
| P04-16 | `INSUFFICIENT EVIDENCE` لقوة المدخل فقط؛ لا أثر مالي حُسم |
| TEAM-04 event crosswalk | `10` أحداث |
| Offline | `CORRECTED — ELIGIBLE CATEGORY DOES NOT MEAN ACTION AUTHORITY`؛ كل Action غير متعاقد `DENY/UNRESOLVED` |
| Accounting posting/reversal | `ONLINE AUTHORITATIVE` وفق ACC-001/OFFLINE-001 |
| Queue vocabulary | `NEEDS TEAM CORRECTION — QUEUE GOVERNANCE` |
| A11 fingerprint | `GOVERNANCE DELTA — A11 FINGERPRINT RECONCILIATION` |
| TEAM-03/06/08 | لم تُفتح أي بوابة ولم يُطلب توريث identifiers محلية |

## 17. Handoff لاحق — مقفول الآن

هذه المسودة **لا** تبدأ TEAM-03 ولا TEAM-06 ولا TEAM-08، ولا تطلب من أي فريق توريث التسميات المحلية السابقة.

عند صدور أمر منفصل لاحقًا لفريق التصنيف/المراجعة، يجب أن يستهلك فقط:

- A11 IDs القائمة كما هي.
- الأوصاف السبعة `UNRESOLVED FUNCTION — NEEDS A11 CROSSWALK` كوظائف بلا هوية شاشة.
- TEAM-04 كمرجع الواقعة التشغيلية.
- TEAM-05 كمرجع السياسة/الفجوات المالية.
- AUTH-001/OFFLINE-001/CLIENT-001/ACC-001 كقرارات حاكمة ضمن حدودها.
- Queue/A11 fingerprint كتصحيحات حوكمة، لا كقرارات مالك.

## 18. الحفظ والتوقف

- مسار المسودة: `CONTROL_TOWER/02_GROUP-02_EXPANSION/مساحة_عمل_فرق_كراسة_الشاشات_2026-08-28/04_تقارير_الفرق/الفريق_07/مسودة_التطبيقات_والتحقق_والصلاحيات_DRAFT.md`
- الحالة: `DRAFT — CORRECTED — NOT FOR HANDOFF`
- Git blob SHA: يحسب بعد الحفظ ويبلغ خارج المسودة.
- Commit SHA: يسجل بعد commit محصور بهذا الملف فقط.
- لا تصميم بصري، لا Source/Tests/Database/Migrations، لا TEAM-03/06/08.

**NEW FINDING DURING CORRECTION:** مسار مسودة TEAM-07 كان غائبًا من HEAD الفرع عند بدء التصحيح رغم وجود النسخة التي راجعتها Wave A ضمن ملفات المشروع؛ أُعيدت النسخة إلى المسار الإلزامي نفسه وصُححت، دون تعديل أي ملف TEAM-04 أو TEAM-05 أو ملف مشترك.
