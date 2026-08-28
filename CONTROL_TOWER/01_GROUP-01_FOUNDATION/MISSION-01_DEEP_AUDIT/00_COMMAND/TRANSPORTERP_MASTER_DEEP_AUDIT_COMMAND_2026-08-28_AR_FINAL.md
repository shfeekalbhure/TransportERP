# TRANSPORTERP — الأمر الرئيسي النهائي لتشكيل فرق متعددة التخصصات للمراجعة التأسيسية العميقة والمطابقة وبناء التصور الصحيح للمشروع

**التاريخ المرجعي:** 2026-08-28  
**صفة الوثيقة:** ميثاق مراجعة وحوكمة تأسيسية للمشروع  
**نوع المهمة:** قراءة، فحص، تتبع، تحقق، مقارنة، تحليل، مصالحة، تصميم مقترح، وتقرير فقط  
**حالة الوثيقة:** `FINAL — READY FOR DEEP-AUDIT EXECUTION`  

---

## أولًا — صفة هذا التكليف

يبدأ هذا التكليف بتشكيل **عدة فرق مستقلة ومتعددة التخصصات** لمراجعة مشروع **TransportERP** مراجعة عميقة من جذوره، ومنذ تأسيسه، وحتى حالته الفعلية الحالية.

المطلوب ليس مراجعة تقرير سابق فقط، وليس الاعتماد على أسماء الملفات أو المجلدات أو الفروع أو المحادثات، وليس إثبات حكم سابق.

المطلوب هو:

> **إعادة فهم المشروع نفسه كما هو موجود فعليًا، ومعرفة كيف تأسس، وكيف تطور، وما الذي بُني بصورة صحيحة، وما الذي بُني بصورة جزئية أو خاطئة، وما الذي ينقصه، وما الذي يجب الحفاظ عليه أو إعادة تنظيمه أو استكماله مستقبلًا.**

هذه المهمة في مرحلتها الحالية:

`BASELINE + READ + INSPECT + TRACE + VERIFY + COMPARE + ANALYZE + RECONCILE + DESIGN-PROPOSAL + REPORT`

وليست:

`MODIFY + DELETE + MOVE + RENAME + MERGE + CHERRY-PICK + PUSH + FORCE-PUSH + RESTRUCTURE + IMPLEMENT`

كل تصميم يصدر في هذه المهمة هو **تصميم مقترح فقط**، ولا يمثل أمرًا بتنفيذ إعادة الهيكلة.

---

## ثانيًا — إنشاء الفرق والقيادة

يجب إنشاء الفرق التالية قبل بدء المراجعة، مع تعيين منسق رئيسي للمهمة يتولى تنظيم التسلسل، وحماية استقلال الفرق، وضبط الأدلة، دون أن ينفرد بإصدار الحكم الفني النهائي.

قبل بدء الفحص، ينشأ سجل باسم:

`TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md`

ويثبت لكل فريق:

- اسم الفريق والمرحلة.
- الوكلاء أو الجلسات الفعلية المخصصة له ومعرفاتها المتاحة.
- الأدوار التخصصية المسندة لكل وكيل أو مراجع.
- وقت بدء التكليف وحالته.
- نطاق القراءة والمصادر المسموح بها.
- حدود الاستقلال ومن يحق له قراءة أي تقرير وفي أي مرحلة.
- القيود الفعلية في عدد الوكلاء أو الأدوات أو الوصول.

ممنوع الادعاء بتشكيل فريق أو تخصص لم ينشأ أو لم يسند فعليًا. وإذا لم تسمح البيئة بتكوين فرق مستقلة حقيقية، يسجل ذلك مانعًا، ولا توصف مراجعة واحدة بأنها مراجعتان مستقلتان.

إذا تعذر تشغيل جميع الفرق بالتوازي بسبب حدود السعة، يجوز تشغيلها على موجات، بشرط بقاء TEAM-A وTEAM-B في سياقين مستقلين وعدم كشف Findings أو Assessments أو Recommendations بينهما قبل الإقفال.

### TEAM-A — فريق المراجعة المستقلة الأول

فريق متعدد التخصصات يقوم بالمراجعة الكاملة للمشروع باستقلال تام.

- لا يقرأ تقرير TEAM-B أثناء عمله الأولي.
- لا يتأثر بنتائج TEAM-B أو أي حكم سابق.
- يجوز له قراءة التقارير السابقة بوصفها **ادعاءات أو مصادر تاريخية يجب التحقق منها**، لا بوصفها حقيقة نهائية.
- يغلق تقريره ويختمه قبل السماح بالمقارنة.

ويصدر تقريره باسم:

`TEAM-A_INDEPENDENT_DEEP_AUDIT_REPORT.md`

### TEAM-B — فريق المراجعة المستقلة الثاني

ينفذ **نفس نطاق TEAM-A بالكامل** ولكن بصورة مستقلة.

- لا يقرأ تقرير TEAM-A أثناء عمله الأولي.
- لا يتأثر بنتائج TEAM-A أو أي حكم سابق.
- الهدف هو إنتاج تقرير ثانٍ مستقل يمكن مقارنته Finding-by-Finding مع تقرير TEAM-A.
- يغلق تقريره ويختمه قبل السماح بالمقارنة.

ويصدر تقريره باسم:

`TEAM-B_INDEPENDENT_DEEP_AUDIT_REPORT.md`

### TEAM-C — فريق المعمارية والبناء والتنظيم

ينقسم عمله إلى مرحلتين واضحتين:

#### TEAM-C1 — تقييم البنية الحالية

يدرس بصورة مستقلة:

- بنية Solution الحالية.
- المشاريع الموجودة داخل Visual Studio.
- المجلدات الفعلية.
- الأنظمة الوظيفية.
- الشاشات والخدمات والمكونات المشتركة.
- العلاقات بين المشاريع والتبعيات.
- أماكن الملفات والمسؤوليات الفعلية.
- المشكلات التنظيمية المثبتة.

ويصدر **تقرير البنية الحالية فقط** قبل بدء المصالحة، باسم:

`TEAM-C1_CURRENT_ARCHITECTURE_ASSESSMENT.md`

#### TEAM-C2 — تصميم البنية المستهدفة المقترحة

لا يبدأ TEAM-C2 تصميم الشجرة النهائية المقترحة إلا بعد:

1. تثبيت Audit Baseline.
2. إغلاق تقريري TEAM-A وTEAM-B.
3. إغلاق تقرير TEAM-C1.
4. إتمام TEAM-D للمصالحة Finding-by-Finding، وإقفال تقرير المصالحة وتحديد الحقائق الحاكمة.

ثم يصدر تصورًا معماريًا وتنظيميًا مستقبليًا **مقترحًا فقط**، دون نقل أو إعادة تسمية أو تعديل أي ملف أو Project، باسم:

`TEAM-C2_TARGET_ARCHITECTURE_PROPOSAL.md`

### TEAM-D — فريق مراجعة الأدلة والمطابقة

يبدأ عمله بعد إغلاق وختم تقارير TEAM-A وTEAM-B وTEAM-C1.

يقوم بـ:

- مقارنة تقريري TEAM-A وTEAM-B Finding-by-Finding.
- مطابقة تقييم TEAM-C1 مع الواقع المثبت.
- التحقق من الاختلافات.
- إعادة فتح الأدلة عند الحاجة.
- كشف الاستنتاجات غير المثبتة.
- الفصل بين الحقيقة الحالية والتاريخية والعمل غير المدمج.
- تحديد الدليل الحاكم والحكم الصحيح ودرجة الثقة.
- إصدار تقرير مصالحة موحد.

ويصدر تقريره المقفل باسم:

`TEAM-D_EVIDENCE_RECONCILIATION_REPORT.md`

لا يبدأ TEAM-C2 قبل اكتمال هذا التقرير وإقفاله. ويجوز لـTEAM-E تسجيل اعتراض أو تحفظ لاحق، لكنه لا يغير حقيقة مصالحة مقفلة إلا بدليل جديد موثق يعاد من خلاله فتح الـFinding المعني وتحديث سجل النسخ صراحةً.

### TEAM-E — المجلس الاستشاري متعدد التخصصات

يتكون، قدر الإمكان، من ممثلين في:

- هندسة البرمجيات.
- Enterprise Architecture.
- ERP Architecture.
- Domain-Driven Design.
- Windows Desktop / WinForms.
- ASP.NET Core.
- Mobile / MAUI / Android.
- PostgreSQL.
- EF Core.
- Security.
- Multi-Tenant Architecture.
- Offline-first وSync.
- Accounting.
- Transport & Logistics.
- Passenger Ticketing.
- Shipping & Warehousing.
- UX/UI وRTL.
- DevOps وCI/CD.
- QA وTest Engineering.
- Release Engineering.
- Governance وEvidence Assurance.
- Project Management.

يراجع المجلس نتائج TEAM-D وتصميم TEAM-C2، ولا يصدر قرارًا بناءً على رأي تخصص واحد. ويصدر تقريرًا مستقلًا باسم:

`TEAM-E_CRITICAL_FINDINGS_ADVISORY_REVIEW.md`

يتناول جميع P0/P1، وعينة مبررة من P2/P3، والمخاطر المتقاطعة، والخلافات المعمارية، والتحفظات، ومدى كفاية الأدلة.

---

## ثالثًا — التسلسل التنفيذي الحاكم

يكون التسلسل الإلزامي كما يلي:

`Audit Baseline`

ثم:

`TEAM-A + TEAM-B Independent Audits`

بالتوازي المنضبط مع:

`TEAM-C1 Current Architecture Assessment`

ثم:

`Seal TEAM-A + TEAM-B + TEAM-C1 Reports`

ثم:

`TEAM-D Evidence Reconciliation`

ثم:

`TEAM-C2 Target Architecture Proposal`

ثم:

`TEAM-E Multidisciplinary Review`

ثم:

`Master Deep Audit Report`

ثم:

`Audit Reconciliation Gate`

ممنوع تجاوز مرحلة الإغلاق والختم، أو تمكين TEAM-A وTEAM-B من قراءة نتائج بعضهما قبل إغلاق تقريري المراجعة المستقلين.

---

## رابعًا — منع العمل الفردي

ممنوع أن يتولى شخص أو تخصص واحد وحده إصدار الحكم النهائي على المشروع.

كل ملاحظة حرجة يجب أن تمر، على الأقل، عبر:

1. التخصص الفني المعني.
2. تخصص ثانٍ مرتبط بالأثر.
3. مراجع الأدلة والمطابقة.

مثال: مشكلة Database Isolation لا يحكم عليها مبرمج واجهة منفردًا، بل يراجعها ممثلو:

- Database.
- Security / Multi-Tenant.
- Application / Architecture.
- QA / Evidence.

---

## خامسًا — قاعدة منع التخمين

# NO GUESSING

تطبق هذه القاعدة بصورة مطلقة:

- ممنوع افتراض أن شيئًا موجود لأنه مذكور في تقرير.
- ممنوع افتراض أن شيئًا منفذ لأنه موجود كملف.
- ممنوع افتراض وظيفة Project أو Folder من اسمه.
- ممنوع افتراض عدد مشاريع Visual Studio قبل فحص Solution الفعلي.
- ممنوع افتراض أن اسم Branch يدل على محتواه.
- ممنوع افتراض أن وجود Test يعني نجاحه.
- ممنوع افتراض أن وجود Migration يعني أنها صالحة أو مطبقة.
- ممنوع افتراض أن شاشة موجودة لأنها تحتوي Designer فقط.
- ممنوع افتراض أن Backend مكتمل بسبب وجود Endpoint.
- ممنوع اعتبار تقرير Codex أو GitHub أو الكراسة حقيقة نهائية دون مطابقة.
- ممنوع تحويل توصية أو تصميم مقترح إلى حقيقة حالية.
- لا يعد أي مصدر منفرد—سواء كان الكود أو الكراسة أو GitHub أو تقريرًا سابقًا—حقيقة كاملة للمشروع دون مطابقة المصادر المرتبطة بالأثر.

أي معلومة لم يتم إثباتها تسجل:

`UNKNOWN — REQUIRES VERIFICATION`

وأي مصدر تعذر الوصول إليه يسجل:

`ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`

---

## سادسًا — تثبيت Audit Baseline قبل أي تحليل

قبل بدء التحليل، يصدر المنسق سجلًا باسم:

`AUDIT_BASELINE_2026-08-28.md`

ويحتوي، بحسب ما يمكن إثباته فعليًا، على:

- وقت اللقطة بـUTC والتوقيت المحلي.
- موضوع التدقيق (`AUDIT SUBJECT`).
- الخط الحالي الحاكم لهذه المراجعة (`AUTHORITATIVE CURRENT LINE FOR THIS AUDIT`).
- اسم المستودع والمسار الفعلي.
- Repository root وGit root.
- ملفات `.sln` و`.slnx` المكتشفة.
- الفرع الحالي.
- HEAD وSHA الحالي.
- حالة working tree.
- الملفات المعدلة وغير المتتبعة.
- remotes وعناوينها.
- default branch المحلي والبعيد.
- الفروع المحلية والبعيدة المتاحة.
- رؤوس PRs التي يمكن التحقق منها.
- Issues وCI workflows وحالة checks المتاحة.
- Worktrees وStashes.
- Codex workspaces المتاحة فعليًا.
- Scratch وExecution وReview workspaces المرتبطة التي يمكن إثباتها.
- حالة الوصول إلى الكراسة والمستودع وGitHub وCodex.
- الأدوات والبيئة التي ستستخدم للمراجعة.

### موضوع التدقيق والخط الحالي الحاكم

لا يكفي تسجيل Current Branch وDefault Branch وPR heads كلٌّ على حدة؛ بل يجب أن يحدد سجل الأساس صراحةً ما المرجع الذي يمثل **الخط الحالي الحاكم للمشروع في هذه المراجعة**.

يسجل بالشكل التالي:

`AUDIT SUBJECT: TransportERP — <repository/path/scope>`

`AUTHORITATIVE CURRENT LINE FOR THIS AUDIT: <ref> @ <full SHA>`

إذا تعذر إثبات الخط الحالي الحاكم، يسجل حصريًا:

`AUTHORITATIVE CURRENT LINE: UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

وفي هذه الحالة:

- لا يختار أي فريق خطًا بديلًا من عنده.
- لا يعتبر Default Branch أو Current Local Branch أو PR head خطًا حاكمًا تلقائيًا.
- لا يسمح بأن يراجع TEAM-A خطًا بوصفه الحالة الحالية بينما يراجع TEAM-B خطًا آخر بالصفة نفسها.
- يجوز فحص الفروع وPRs وWorktrees الأخرى بوصفها `UNMERGED` أو `LOCAL-ONLY` أو `HISTORICAL`، مع ربط كل نتيجة بمرجعها وSHA الخاص بها.
- تسجل فجوة السلطة ضمن `UNKNOWN_AND_BLOCKERS_REGISTER.md` إلى أن تحسم بدليل مستودع أو توجيه صريح من المالك.
- يجوز للفرق عندئذٍ متابعة جرد المصادر والتاريخ والأعمال غير المدمجة، لكن يمنع إصدار حكم نهائي على `CURRENT STATE` أو فتح بوابة الجاهزية قبل تحديد الخط الحالي الحاكم.

كل حكم في التقارير يجب أن يوضح أنه صالح بالنسبة إلى هذه اللقطة فقط.

إذا تغير HEAD أو PR head أو working tree أثناء المراجعة:

- لا تخلط الأدلة القديمة بالجديدة.
- يسجل التغير في `AUDIT_BASELINE_DELTA_LOG.md`.
- يربط كل دليل بالـSHA الذي فُحص عليه.
- لا تنقل نتيجة Build/Test من SHA إلى SHA آخر.
- يعاد التحقق من النتائج المتأثرة، أو تصنف بأنها لم تعد حاكمة على الرأس الجديد.

---

## سابعًا — منع خلط الحالة الحالية بالحالة التاريخية

Git history مطلوب لفهم تطور المشروع، لكنه لا يرفع عملًا تاريخيًا أو غير مدمج إلى الحالة الحالية.

كل Finding وكل Evidence يجب أن يحمل أحد التصنيفات التالية:

- `CURRENT` — موجود في الخط الحالي عند لقطة المراجعة.
- `HISTORICAL` — كان موجودًا تاريخيًا ولا يمثل الوضع الحالي.
- `UNMERGED` — موجود في فرع أو PR غير مدمج.
- `LOCAL-ONLY` — موجود محليًا فقط.
- `SUPERSEDED` — استُبدل بعمل أحدث.
- `PROPOSED` — مقترح غير منفذ.
- `UNKNOWN` — لم يمكن إثبات حالته.

لا يجوز جمع هذه الحالات تحت عبارة عامة مثل «المشروع يحتوي على» دون توضيح حالة العنصر.

---

## ثامنًا — سياسة الوصول والقراءة عن بعد

المراجعة تقرأ فقط، ولا تعدل المستودع الأصلي أو حالته.

- لا يشغل `git pull` أو `git fetch` داخل المستودع الأصلي إذا كان ذلك سيعدل `.git` أو remote-tracking refs.
- تستخدم واجهات GitHub أو الوسائل البعيدة للقراءة فقط للتحقق من PRs وIssues وBranches وCI وremote heads.
- لا يفتح Visual Studio بطريقة تنشئ `.vs` أو تعدل Solution أو user settings؛ يمكن تحليل `.sln/.slnx/.csproj` والملفات نصيًا وبأدوات قراءة آمنة.
- إذا تعذر الوصول إلى GitHub أو Codex أو الكراسة أو Workspace معين، يوثق ذلك ولا يعوض بالتخمين.
- لا يعتبر غياب المورد من مساحة واحدة دليلًا على عدم وجوده في مساحة أخرى.

---

## تاسعًا — سياسة Build/Test والتحقق التشغيلي

وجود الاختبار لا يعني نجاحه، لكن تشغيل Build/Test داخل نسخة العمل الأصلية قد ينشئ `bin/obj` وملفات مؤقتة.

لذلك:

1. يمنع تشغيل Build/Test داخل المستودع الأصلي إذا كان سيولد ملفات أو يغير حالته.
2. عند الحاجة إلى Runtime verification، يسمح بإنشاء **Clone أو Sandbox مؤقت معزول خارج Repository Root الأصلي** عند SHA محدد.
3. لا يسمح بإنشاء Worktree جديد مرتبط بالمستودع الأصلي ضمن هذا التكليف.
4. كل المخرجات المؤقتة تكون داخل البيئة المعزولة فقط.
5. يجوز استخدام قاعدة بيانات أو Container مؤقت داخل البيئة المعزولة، بشرط أن يكون Disposable، وألا يحتوي بيانات إنتاج أو أسرارًا حقيقية، وأن يستخدم بيانات صناعية أو منزوعة الهوية فقط.
6. لا تستخدم البيئة المعزولة للوصول إلى Production أو قاعدة بيانات حقيقية أو خدمة خارجية حقيقية دون تفويض صريح مستقل.
7. يمنع نسخ Production credentials أو tokens أو connection strings أو مفاتيح حقيقية إلى بيئة التدقيق.
8. لا تشغل migrations مدمرة أو خدمات إنتاج.
9. لا يجرى Commit أو Push من البيئة المعزولة.
10. يوثق أمر الاختبار، والبيئة، وSDK، ووضع قاعدة البيانات، وSHA، والنتيجة، وسجلات الفشل.
11. بعد حفظ الأدلة، يجوز التخلص من الموارد المؤقتة داخل حدود الـSandbox المحدد فقط، دون المساس بالمستودع الأصلي أو أي بيانات خارجية.
12. إذا تعذر إنشاء بيئة آمنة، تسجل النتيجة:

`BLOCKED — WRITE RESTRICTION`

ولا يجوز عندها إعلان PASS أو FAIL تشغيلي دون تشغيل مثبت.

---

## عاشرًا — مساحة عمل التقارير

تنشأ مساحة مراجعة مستقلة خارج Repository Root الأصلي، ويفضل أن تكون:

`<project-parent>/PROJECT_DEEP_AUDIT_2026-08-28/`

أو Audit workspace مستقلة مكافئة.

وتحتوي على:

- `00_BASELINE`
- `TEAM-A`
- `TEAM-B`
- `TEAM-C1`
- `TEAM-D`
- `TEAM-C2`
- `TEAM-E`
- `MASTER`
- `EVIDENCE-INDEX`
- `REGISTERS`

إذا تعذر وضعها خارج Git root، يسمح بإنشائها داخل مساحة المشروع بشرط:

- توثيقها كـAudit Artifact.
- عدم وضعها داخل Production أو Source أو Tests أو Migrations أو الكراسة الرسمية.
- عدم Commit أو Push لها.

هذا هو الاستثناء الكتابي الوحيد، إضافة إلى المخرجات المؤقتة داخل Sandbox المعزول المخصص للاختبارات.

---

## الحادي عشر — سجل المصادر والأدلة الموحد

ينشأ منذ البداية:

- `SOURCE_ACCESS_REGISTER.md`
- `EVIDENCE_INDEX.md`
- `FILES_REVIEWED_REGISTER.md`
- `UNKNOWN_AND_BLOCKERS_REGISTER.md`
- `TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md`
- `AUDIT_REPORT_SEAL_REGISTER.md`
- `AUDIT_OUTPUT_MANIFEST.md`
- `DOMAIN_COVERAGE_MATRIX.md`
- `WORKSPACE_PRESERVATION_REGISTER.md`

وينشأ `AUDIT_BASELINE_DELTA_LOG.md` فور حدوث أي تغير في لقطة الأساس. ولا ينشأ سجل فارغ بقصد الإيحاء بحدوث تغير إذا لم يحدث.

### حماية استقلال السجلات أثناء المراجعة الأولى

- يجوز مشاركة Audit Baseline و`SOURCE_ACCESS_REGISTER.md` المحايد بين الفرق؛ لأنه يسجل الهوية والوصول ولا يحتوي أحكامًا فنية.
- يحتفظ TEAM-A أثناء مرحلته المستقلة بنسخ خاصة به من Evidence Index وFiles Reviewed وUnknown/Blockers داخل مجلده.
- يحتفظ TEAM-B بالنسخ المقابلة داخل مجلده، ولا يقرأ نسخ TEAM-A قبل إقفال التقريرين.
- يحتفظ TEAM-C1 بسجلاته الخاصة حتى الإقفال.
- ممنوع استخدام سجل مركزي حي يكشف Findings أو Assessments أو Recommendations بين TEAM-A وTEAM-B قبل الختم.
- بعد إقفال TEAM-A وTEAM-B وTEAM-C1، ينشئ TEAM-D فهرسًا موحدًا مع Crosswalk يحافظ على المعرفات الأصلية ويربطها بأحكام المصالحة.
- أي اطلاع مبكر غير مقصود يسجل كخرق استقلال داخل `AUDIT_REPORT_SEAL_REGISTER.md`، ويحدد أثره وهل يلزم إعادة المراجعة المستقلة.

### حقول SOURCE_ACCESS_REGISTER

يسجل لكل مصدر:

- Source ID.
- نوع المصدر.
- الاسم والمسار أو المرجع.
- وقت الوصول بـUTC والتوقيت المحلي.
- ref وSHA أو Version عند الانطباق.
- حالة الوصول: `AVAILABLE / PARTIALLY AVAILABLE / ACCESS BLOCKED`.
- الفريق أو الدور الذي راجعه.
- نطاق ما تمت قراءته.
- القيود والمحتوى الذي لم يكن متاحًا.

### حقول EVIDENCE_INDEX

يسجل لكل دليل:

- Evidence ID.
- Finding ID المرتبط.
- Source ID.
- نوع الدليل: كود مباشر، Runtime، Test، CI، Database، وثيقة، تاريخ Git، أو ادعاء يحتاج تحققًا.
- الموضع الدقيق: Project وFile وSymbol أو Endpoint أو Migration أو Artifact عند الانطباق.
- ref وBranch وfull SHA.
- وقت جمع الدليل.
- نتيجة الدليل وحدود ما يثبته.
- `SHA-256` لسلامة الـArtifact عند الحاجة.
- الفريق أو الدور الذي جمعه.

### حقول FILES_REVIEWED_REGISTER

يسجل لكل ملف مهم تمت مراجعته:

- File ID.
- المسار الكامل داخل المصدر.
- Project وModule أو Domain.
- ref وfull SHA.
- المراجع أو الدور.
- سبب المراجعة.
- Classes أو Methods أو Sections التي تمت قراءتها.
- Findings المرتبطة.
- حالة التغطية: `FULL FOR STATED PURPOSE / PARTIAL / BLOCKED`.
- ما لم تتم مراجعته داخل الملف إن وجد.

### حقول UNKNOWN_AND_BLOCKERS_REGISTER

يسجل لكل مجهول أو مانع:

- Blocker ID.
- السؤال أو الحقيقة غير المحسومة.
- Domain وFindings المتأثرة.
- المصدر أو الصلاحية أو البيئة المفقودة.
- سبب التعذر.
- أثره على P0/P1 وعلى الحكم النهائي.
- الجهة أو السلطة المطلوبة للحسم.
- خطوة التحقق التالية.
- أثره على بوابة الجاهزية: `BLOCKS READY / DOES NOT BLOCK READY / UNKNOWN`.

### حقول AUDIT_OUTPUT_MANIFEST

يسجل لكل مخرج:

- اسم الملف ووظيفته.
- الفريق أو المرحلة المالكة.
- Version وحالة `SEALED / REOPENED / SUPERSEDED`.
- وقت الإقفال.
- `SHA-256`.
- Audit baseline وfull SHA المرجعيان.
- المخرج السابق الذي يستبدله إن وجد.

### حقول DOMAIN_COVERAGE_MATRIX

ينشأ هذا السجل لضمان أن كثرة الملفات المقروءة لا تخفي Domain لم تتم مراجعته فعليًا. ويسجل، على الأقل:

- Domain / Business System / Technical Area.
- حالة التغطية لدى TEAM-A: `REVIEWED / PARTIAL / BLOCKED / NOT REVIEWED`.
- حالة التغطية لدى TEAM-B بالقيم نفسها.
- حالة TEAM-C1 عند الانطباق.
- حالة المصالحة لدى TEAM-D: `RECONCILED / PARTIAL / UNKNOWN / ACCESS BLOCKED / N/A`.
- Evidence IDs الحاكمة.
- أهم الملفات أو المكونات التي بُني عليها الحكم.
- الفجوات أو المصادر غير المتاحة.
- هل توجد P0/P1 غير محسومة في هذا المجال؟
- أثر نقص التغطية على بوابة الجاهزية.

ويجب أن يغطي السجل جميع المجالات الداخلة في نطاق هذا الأمر، بما فيها Architecture وDatabase وSecurity وMulti-Tenant وOffline/Sync وDesktop وMobile وShipping وTicketing وAccounting وCI/CD وSupply Chain وPrivacy وKurrasa/Governance وRelease/Deployment.

لا يجوز إعلان اكتمال المراجعة لمجال حالته `PARTIAL / BLOCKED / NOT REVIEWED` دون توضيح أثر ذلك صراحةً في التقرير النهائي والبوابة.

### حقول WORKSPACE_PRESERVATION_REGISTER

ينشأ هذا السجل لحصر العمل المحلي وغير المدمج الذي يجب عدم فقدانه قبل أي معالجة أو إعادة تنظيم مستقبلية. ويسجل، على الأقل:

- Preservation ID.
- نوع الأصل: `BRANCH / WORKTREE / STASH / LOCAL-ONLY COMMIT / DIRTY WORKTREE / UNTRACKED ARTIFACT / CODEX WORKSPACE / PATCH / ALTERNATIVE COPY / OTHER`.
- الاسم والمسار أو المرجع.
- Branch/ref وfull SHA عند الانطباق.
- حالة الأصل: `CURRENT / UNMERGED / LOCAL-ONLY / SUPERSEDED / UNKNOWN`.
- وصف مختصر للقيمة أو العمل الموجود.
- Evidence IDs وFindings المرتبطة.
- حالة الحفظ المقترحة: `PRESERVE / KEEP UNTIL RECONCILED / SUPERSEDED-CANDIDATE / UNKNOWN`.
- سبب المحافظة أو سبب عدم القدرة على الحكم.
- الاعتمادات أو الأعمال الأخرى المرتبطة.
- المخاطر عند الفقد أو الدمج الخاطئ.
- الجهة أو القرار المطلوب قبل أي حذف أو دمج أو استبعاد لاحق.

وجود أصل في هذا السجل لا يعني اعتماد دمجه؛ المقصود إثبات وجوده وقيمته المحتملة ومنع فقده قبل صدور قرار مستقل لاحق.

### معرفات الأدلة

تستخدم معرفات ثابتة، مثل:

- `A-ARCH-001`
- `A-DB-014`
- `B-SEC-006`
- `B-OFFLINE-011`
- `C1-CURRENT-021`
- `D-REC-009`
- `C2-TARGET-013`
- `E-ADVISORY-004`

### الحد الأدنى لكل Finding

كل ملاحظة يجب أن تتضمن:

- Finding ID.
- العنوان.
- المجال والتخصص.
- حالة التنفيذ (`IMPLEMENTATION STATUS`): `RUNTIME COMPLETE / PARTIAL / FOUNDATION ONLY / CONTRACT ONLY / PROTOTYPE / NOT IMPLEMENTED / NOT APPLICABLE / UNKNOWN`.
- حالة التحقق (`VERIFICATION STATUS`): `VERIFIED / PARTIALLY VERIFIED / UNVERIFIED / ACCESS BLOCKED`.
- التصنيف الزمني: `CURRENT / HISTORICAL / UNMERGED / LOCAL-ONLY / SUPERSEDED / PROPOSED / UNKNOWN`.
- `OBSERVED FACT` — الحقيقة المشاهدة فقط.
- `EVIDENCE` — الدليل.
- Project وFile وClass وMethod/Endpoint عند الانطباق.
- Migration أو Database object عند الانطباق.
- Test وبيئته ونتيجته عند الانطباق.
- Branch وSHA.
- أثر الملاحظة.
- درجة الثقة: `HIGH / MEDIUM / LOW` مع سبب الدرجة.
- `RECOMMENDATION` — التوصية منفصلة عن الحقيقة.
- الأولوية: `P0 / P1 / P2 / P3 / INFO / N/A`.
- ما الذي لم يتم إثباته.
- المراجع الفني المختص.
- مراجع التخصص المتأثر.
- مراجع الأدلة.

أما حكم المصالحة (`RECONCILIATION DETERMINATION`) فلا يصدره TEAM-A أو TEAM-B بوصفه حكمًا نهائيًا، ويقتصر على TEAM-D بعد المقارنة، باستخدام القيم المحددة في قسم المطابقة.

لا تستخدم P3 لاستيعاب الحقائق الإيجابية أو عناصر الجرد التي لا تحتاج معالجة؛ تستخدم لها `INFO` أو `N/A`.

لا تستخدم درجة ثقة `LOW` لإعلان حقيقة حالية أو لتثبيت P0/P1. تسجل المعلومة عندها بوصفها فرضية تحتاج تحققًا أو `UNKNOWN — REQUIRES VERIFICATION`، مع بيان الدليل الناقص.

### الفصل بين الحقيقة والتوصية

لا يجوز خلط:

`OBSERVED FACT`

مع:

`RECOMMENDATION`

مثال:

- الحقيقة: يوجد Formان ينفذان lookup متشابهًا مع تكرار في المنطق.
- التوصية: تقييم تحويلهما إلى Shared Lookup أو Reusable Dialog.

التوصية ليست دليلًا على أن البنية المقترحة هي الوحيدة الصحيحة.

---

## الثاني عشر — نقطة البداية الصحيحة للمراجعة

لا تبدأ الفرق من المحاسبة، ولا من الشحن، ولا من الكراسة وحدها، ولا من أسماء المشاريع.

تبدأ من **تثبيت الواقع الفعلي للمشروع** وفق Audit Baseline، ثم تحديد أين يوجد العمل الحقيقي.

لا يتم الانتقال إلى الأحكام التفصيلية قبل توثيق:

- المستودع والجذر الفعلي.
- Solution والـProjects.
- الخط الحالي وSHA.
- الفروع وPRs والأعمال المحلية المتاحة.
- مصادر الكراسة والوثائق.
- حدود الوصول والمصادر المفقودة.

---

## الثالث عشر — مراجعة تاريخ تأسيس المشروع

راجعوا Git history والـcommits والفروع والتقارير والقرارات لتحديد:

- أول بنية للمشروع.
- أول Solution.
- المشاريع التي أنشئت أولًا.
- المشاريع التي أضيفت لاحقًا.
- المشاريع التي ألغيت أو استبدلت.
- التغييرات المعمارية المهمة.
- تغير قاعدة البيانات والتقنية والنطاق.
- الفروع التي تحتوي أعمالًا لم تصل إلى الخط الحالي.
- إعادة البناء أو المحاولات التجريبية السابقة.
- القرارات التي غيرت الاتجاه الفني.

الهدف هو معرفة لماذا وصلت البنية الحالية إلى شكلها الحالي، مع عدم خلط التاريخ بالحالة الحالية.

---

## الرابع عشر — فحص Visual Studio Solution بعمق

افحصوا Solution الفعلي، ولا تعتمدوا على تقرير يذكر عدد المشاريع.

احصروا فعليًا:

- `.sln`
- `.slnx`
- `.slnf`
- `.csproj`
- Project References.
- `global.json`.
- `Directory.Build.props` و`Directory.Build.targets`.
- `Directory.Packages.props`.
- `NuGet.config` و`packages.lock.json` إن وجدا.
- ملفات إعداد البناء والتشغيل المشتركة المؤثرة فعليًا.

ولكل Project سجلوا:

- الاسم والمسار.
- نوع المشروع وOutput Type.
- SDK وFramework وTarget Framework.
- Startup capability.
- Dependencies وProject References.
- NuGet dependencies وإصداراتها المهمة.
- الوظيفة الفعلية من الكود.
- هل هو مستخدم فعليًا أم Prototype أم Test Project؟
- هل هو Executable أم Library؟
- هل يبنى على SHA المفحوص، إذا تم تشغيل Build آمن؟
- هل تتكرر مسؤوليته مع Project آخر؟
- هل موقعه وNamespace الخاص به متسقان؟

ثم حددوا العدد الحقيقي للمشاريع، دون افتراض أنه 10 أو 13 أو أي رقم سابق.

---

## الخامس عشر — الفصل بين Projects والأنظمة الوظيفية

يجب عدم الخلط بين:

`Visual Studio Projects`

و:

`Business Systems / Modules`

أنشئوا جدولًا مستقلًا للأنظمة الوظيفية المستخرجة من الكود والكراسة والقرارات المعتمدة، وقد تشمل دون افتراض نهائي:

- النظام العام والمنصة.
- التهيئة والإعدادات.
- الشركات والفروع.
- المستخدمين والأدوار والصلاحيات.
- الحسابات والصناديق والبنوك والعملات.
- القيود والسندات والتقارير المالية.
- العملاء والموردين والوكلاء.
- الشحن والبوالص والطرود.
- المخازن والترحيل والتسليم والتحصيل.
- الرحلات والمركبات والسائقين.
- المطالبات والمرتجعات والجمارك.
- التذاكر والركاب والحجوزات والتسويات.
- Offline وSync.
- Mobile وDesktop.
- Audit وSecurity وAdministration.

لكل نظام سجلوا: مطلوب، موجود، Runtime، جزئي، Foundation فقط، Prototype، غير منفذ، أو مجهول.

---

## السادس عشر — مراجعة بنية المجلدات والكود الفعلي

راجعوا كل Project على مستوى المجلدات والملفات والمحتوى.

لا يكفي القول «يوجد مجلد Services»، بل يجب تحديد:

- الخدمات الموجودة ومن يستخدمها.
- هل موقعها صحيح؟
- هل هي عامة أم خاصة بوحدة؟
- هل لها نسخة أخرى؟
- هل المسؤوليات مختلطة؟
- هل توجد ملفات كبيرة متعددة الوظائف؟
- هل توجد Circular Dependencies أو Coupling غير ضروري؟
- هل توجد قواعد عمل مخفية داخل UI أو Controllers؟
- هل توجد طبقات اسمية دون فصل فعلي للمسؤوليات؟

يجب الوصول عند الحاجة إلى مستوى:

- Classes وInterfaces.
- Methods وServices.
- Controllers وEndpoints.
- Forms وEvent Handlers.
- DbContext وEntity Configurations.
- Migrations وValidators.
- Policies وMiddleware.
- Authentication وAuthorization.
- Sync handlers وOffline queue.
- Conflict resolution.
- Tests وWorkflows.

---

## السابع عشر — مراجعة الشاشات وتنظيمها

لكل شاشة تحققوا من:

- Form وDesigner.
- ViewModel أو Presenter إن وجد.
- Service وAPI Client.
- Requests وResponses.
- Validation وPermissions.
- Resources وLocalization وRTL.
- Tests وNavigation.
- Lookup dependencies.
- Startup/DI registration إن وجد.

وصنفوا كل شاشة إلى:

- مكتملة ومتصلة وتعمل.
- جزئية.
- Prototype.
- Designer فقط.
- موجودة لكنها غير مستخدمة.
- مكررة.
- موجودة في مكان غير مناسب.
- مطلوبة وغير موجودة.
- مجهولة لعدم كفاية الدليل التشغيلي.

وجود Form أو Designer لا يثبت إمكان الفتح أو الحفظ أو الاتصال بالـAPI.

---

## الثامن عشر — المكونات والشاشات المشتركة

ركزوا على العناصر القابلة لإعادة الاستخدام، مثل:

- اختيار الحساب والعميل والمورد والوكيل.
- اختيار الشركة والفرع.
- اختيار العملة والصندوق والبنك ومركز التكلفة.
- اختيار الرحلة والمركبة والسائق والموقع.
- اختيار المحافظة والمديرية والمنطقة.
- البحث والمرفقات والتأكيد والرسائل.
- الصلاحيات والطباعة وAudit display.

ابحثوا هل أنشأت شاشات متعددة نسخًا خاصة من الوظيفة نفسها.

إذا ثبتت قابلية إعادة الاستخدام، يمكن التوصية بأحد الأنماط:

- `Shared Component`
- `Shared Lookup`
- `Common Dialog`
- `Reusable Control`

لكن لا ينفذ أي تحويل ضمن هذه المهمة.

---

## التاسع عشر — مراجعة قاعدة البيانات

راجعوا بعمق:

- DbContext وEntities وConfigurations.
- Keys وComposite Keys وForeign Keys.
- Indexes وUnique Constraints وCheck Constraints.
- Query Filters.
- Company isolation وBranch isolation.
- Audit والعلاقات المالية والتشغيلية.
- Shipping وTicketing relationships.
- Migrations وترتيبها وسلامة سلسلتها.
- Schema وPostgreSQL-specific logic.
- Append-only structures.
- Concurrency وRowVersion أو بدائل PostgreSQL.
- Idempotency.
- Transaction boundaries وAtomicity في العمليات المركبة.
- Monetary precision وRounding وDate/Time/Timezone handling.
- Row-Level Security أو بدائل العزل الفعلية إن وجدت.
- Seed وBootstrap data.
- Backup/Restore وRecovery evidence وسياسات فقدان البيانات، ضمن بيئة غير إنتاجية وآمنة فقط.
- فروقات Model مقابل Migration مقابل قاعدة البيانات المتوقعة.

صنفوا كل عنصر إلى: مكتمل، جزئي، ناقص، خطر، متكرر، متعارض، أو غير قابل للإثبات.

وجود Migration لا يثبت أنها طبقت أو أنها صالحة على قاعدة جديدة أو ترقية قائمة.

---

## العشرون — الأمن والعزل متعدد الشركات والفروع

راجعوا:

- Authentication وSessions وRefresh Tokens.
- RBAC وPermission Scope.
- Company وBranch Isolation.
- IDOR.
- Device Identity وDevice Registration.
- Proof-of-Possession.
- Audit وLegal Hold وRetention.
- Secret handling.
- Bootstrap وAdmin creation.
- Privilege escalation.
- Scope propagation عبر API وServices وDatabase.

### الخصوصية والبيانات الحساسة

راجعوا بصورة صريحة:

- تصنيف بيانات الهوية والركاب والعملاء والموظفين والسائقين والمرفقات.
- تقليل جمع البيانات إلى الحد المطلوب تشغيليًا ونظاميًا.
- التشفير أثناء النقل والتخزين وفي قواعد البيانات والنسخ المحلية.
- منع ظهور البيانات الحساسة والأسرار في Logs وExceptions وTelemetry وExports.
- Masking وRedaction في الشاشات والتقارير والأدلة.
- صلاحيات الاطلاع والتنزيل والطباعة والتصدير.
- Retention وLegal Hold والحذف أو الإخفاء النظامي عند الانطباق.
- حماية البيانات داخل Desktop وMobile وOffline caches والنسخ الاحتياطية.
- استخدام بيانات صناعية أو منزوعة الهوية في الاختبارات.

### أمن سلسلة التوريد وCI/CD

راجعوا بصورة صريحة:

- NuGet package sources والحزم المباشرة والمتعدية والإصدارات المقفلة.
- الحزم المعروفة بثغرات، والحزم المهجورة، وتعارضات الإصدارات.
- تراخيص الحزم والمكونات الخارجية عند إمكان التحقق.
- `global.json` وCentral Package Management وlock files وقابلية إعادة البناء.
- CI workflow permissions وSecret exposure وToken scope.
- تثبيت إصدارات Actions أو الأدوات الخارجية ومصدرها.
- Branch protection وRequired checks وسياسات المراجعة والدمج.
- مصدر Artifacts وسلامتها وRetention وإمكان ربطها بـSHA.
- أي Scripts أو Downloads غير مثبتة المصدر أو قابلة للتغيير دون ضبط.

يجب فحص العزل في الاتجاهين متى أمكن:

- شركة A تحاول الوصول إلى B.
- شركة B تحاول الوصول إلى A.
- فرع A يحاول الوصول إلى فرع B والعكس.

لا يعتبر الاختبار في اتجاه واحد إثباتًا كاملًا للعزل العكسي.

---

## الحادي والعشرون — Offline وSync

راجعوا:

- Offline queue.
- Encryption وSQLCipher وDPAPI وAndroid Keystore.
- Retry وReplay وIdempotency.
- Conflict وReapply وPartial Failure.
- Retention وLegal Hold.
- ClientOperationId وDevice Binding.
- Restart recovery.
- Sync after network loss.
- ترتيب العمليات والاعتمادات.
- حماية البيانات المحلية.

حددوا الفارق بدقة بين:

- `IMPLEMENTED RUNTIME`
- `GOVERNED CONTRACT ONLY`
- `FOUNDATION ONLY`
- `PROTOTYPE`
- `NOT IMPLEMENTED`
- `UNKNOWN`

---

## الثاني والعشرون — مراجعة Desktop

افحصوا التطبيق الفعلي وليس صور الشاشات فقط.

حددوا ما الذي يعمل فعليًا من:

- Startup.
- Login.
- Main shell وNavigation.
- Session وTenant/Branch Scope.
- API وDatabase connectivity.
- Offline وSync.
- Forms وPermissions.

ولكل Form تحققوا من:

- هل يمكن فتحها؟
- هل تستدعي API؟
- هل تحفظ وتحمل بيانات؟
- هل تطبق الصلاحيات والنطاق؟
- هل تعمل RTL؟
- هل لها Validation وError handling؟
- هل لها اختبارات أو دليل تشغيل؟

إذا لم يتم تشغيلها في بيئة آمنة فلا تصف بأنها تعمل؛ سجل حدود الإثبات.

---

## الثالث والعشرون — مراجعة Mobile

افحصوا كل تطبيق Mobile بصورة منفصلة وحددوا:

- هل هو Executable أم Library؟
- Target platforms.
- هل يبنى ويعمل على SHA المفحوص، إذا توفر اختبار آمن؟
- هل هو Prototype؟
- الوظائف المنفذة والناقصة.
- Offline وAuthentication وSecurity.
- API integration وDevice identity.
- Android Keystore أو بدائل المنصة.
- هل توجد تطبيقات منفصلة للعميل والسائق والموظف أو مجرد عقود مشتركة؟

لا تستنتجوا وجود تطبيق عامل من اسم Project أو مجلد Mobile.

---

## الرابع والعشرون — مراجعة النطاق التشغيلي الكامل

طابقوا الكود مع دورة العمل الحقيقية للنقل.

### الشحن

من استقبال العميل، إلى البوليصة والطرد، ثم المخزن والترحيل والرحلة والوصول والتسليم والتحصيل والمحاسبة، ثم المرتجعات والمطالبات والجمارك.

### التذاكر

من الحجز وإصدار التذكرة والدفع، إلى الرحلة والركوب والتحويل والوصول والتسوية.

لكل خطوة حددوا:

- Requirement.
- Runtime.
- Data model.
- UI/API integration.
- Permissions.
- Accounting effect.
- Offline/Sync effect.
- Tests/Evidence.

---

## الخامس والعشرون — مراجعة المحاسبة

تحققوا من الواقع الفعلي لـ:

- دليل الحسابات.
- مراكز التكلفة.
- العملات وأسعار الصرف.
- الصناديق والبنوك.
- سند القبض وسند الصرف.
- القيود والاعتماد.
- توازن القيد المزدوج ومنع القيود غير المتوازنة.
- الفترات المالية والإقفال وإعادة الفتح وصلاحياتهما.
- Posting وUnposting وReversal.
- Subledger والتسويات.
- ترقيم المستندات ومنع التكرار والتسلسل حسب الشركة والفرع والسنة عند الانطباق.
- الدقة النقدية والتقريب والعملات وأسعار الصرف وآثارها المحاسبية.
- الربط مع الشحن والتذاكر.
- Trial Balance وGeneral Ledger والتقارير.

حددوا بوضوح ما هو Runtime فعلي، وما هو Entity أو Migration أو Contract أو Foundation فقط.

لا تقبل عبارة «المحاسبة مكتملة» دون تتبع عملية مالية كاملة من الإدخال إلى القيد والتقرير.

---

## السادس والعشرون — مراجعة الكراسة والوثائق

بعد تثبيت الواقع التقني، افتحوا:

**الكراسة التنفيذية الأساسية الرسمية لمشروع TransportERP**

وجميع المراجع المرتبطة المتاحة فعليًا.

راجعوا المحتوى لا أسماء الملفات فقط، وصنفوا الوثائق والقرارات إلى:

- `CURRENT APPROVED`
- `DRAFT`
- `SUPERSEDED`
- `HISTORICAL`
- `PROPOSED`
- `CONFLICTING`
- `UNKNOWN AUTHORITY`

ثم أنشئوا مصفوفة:

`REQUIREMENT → CODE → DATA → TEST → EVIDENCE`

وسجلوا أيضًا:

- ما في الكراسة وليس في الكود.
- ما في الكود وليس له قرار أو متطلب معروف.
- ما يتعارض بين الكراسة والكود.
- ما تغير تاريخيًا دون قرار حاكم ظاهر.

---

## السابع والعشرون — مراجعة GitHub وCodex والعمل غير المدمج

راجعوا، ضمن حدود الوصول:

- PRs وIssues وBranches وCommits.
- Reviews وCI وArtifacts وGovernance reports.
- Codex workspaces.
- Local worktrees وStashes.
- Scratch وExecution وReview workspaces.

### جرد Git المحلي الكامل والوسوم والنسخ البديلة

يجب أن يشمل الجرد، بقدر ما يمكن إثباته دون تعديل المستودع الأصلي:

- Local branches مقابل Remote branches، وتحديد ما يوجد محليًا فقط أو بعيدًا فقط.
- Local-only commits وCommits غير الموجودة على الخط الحاكم الحالي.
- Worktrees الحالية وحالتها وHEAD/SHA لكل منها.
- Stashes ومراجعها وتواريخها ومحتواها على مستوى الوصف والدليل الآمن.
- Modified وUntracked files، مع تحديد ما إذا كانت ذات قيمة أو مجرد مخرجات مؤقتة.
- Tags المحلية والبعيدة المتاحة، وربط كل Tag بالـSHA، وتحديد ما إذا كان يستخدم كمرجع إصدار أو حوكمة أو تاريخ فقط.
- أي تعارض بين Tags وReleases وDefault branch وPR heads في وصف «الإصدار» أو «الحالة الحالية».
- Reflog relevant state عند الحاجة وعندما يمكن قراءته دون تعديل، فقط لاستعادة سياق حركة HEAD أو عمل محلي محتمل؛ ولا يعد Reflog وحده سلطة حالية.
- أي نسخ بديلة أو مكررة للمشروع أو Solution أو Repository داخل المساحات المتاحة، مع تصنيفها: `AUTHORITATIVE / ALTERNATIVE COPY / ARCHIVE / EXPERIMENTAL / UNKNOWN`.
- Local ↔ Remote reconciliation يوضح الفروع والرؤوس والعمل الذي لم يصل إلى المصدر البعيد أو لم يصل إلى الخط الحالي.

كل أصل ذي قيمة محتملة يضاف إلى `WORKSPACE_PRESERVATION_REGISTER.md`.

### جرد Codex والجلسات والأعمال غير الملتزمة

لا يقتصر فحص Codex على أسماء Workspaces. يجب، ضمن حدود الوصول الفعلي، البحث عن:

- Codex sessions أو execution/review sessions المرتبطة بالمشروع.
- Workspace/branch/SHA الذي كانت تعمل عليه كل جلسة.
- Patches أو تغييرات غير committed أو local-only changes الناتجة عن الجلسات.
- Codex local branches أو workspaces التي لا يظهر عملها في الخط الحالي.
- Test/Build evidence الناتج داخل الجلسات وربطه بالـSHA والبيئة، وعدم نقله تلقائيًا إلى رأس آخر.
- أعمال abandoned / incomplete / superseded مع دليل حالتها.
- تقارير أو governance artifacts أنشأتها الجلسات وتصف رأسًا محددًا.
- عمل صحيح محتمل لم يدمج أو لم يدفع أو لم يحفظ خارج مساحة الجلسة.

إذا لم تسمح أدوات Codex المتاحة بقراءة هذه التفاصيل فعليًا، تسجل البنود المتعذرة في `UNKNOWN_AND_BLOCKERS_REGISTER.md`، ولا يدعى فحصها.

ابحثوا عن:

- عمل صحيح لم يدمج.
- عمل مكرر أو قديم أو تجريبي.
- فروع متعارضة.
- أعمال مهمة موجودة محليًا فقط.
- ملفات معدلة أو غير متتبعة.
- Worktrees ذات قيمة يجب المحافظة عليها.
- دليل حوكمة يصف رأسًا غير الرأس الحالي.

ممنوع حذف أي فرع أو Worktree أو Stash أو ملف.

---

## الثامن والعشرون — اكتشاف الفوضى التقنية

يجب البحث صراحة عن:

- Duplicate code/forms/services.
- Dead code وOrphan files.
- Unused projects.
- Experimental code داخل المسار الرئيسي.
- ملفات في Project غير مناسب.
- Namespaces غير متناسقة.
- God classes.
- Circular dependencies وTight coupling.
- Inconsistent naming وMixed responsibilities.
- Hard-coded configuration.
- Hidden business rules.
- Missing validation.
- Disconnected UI.
- Empty handlers.
- Swallowed exceptions.
- TODO/FIXME/HACK.
- Skipped أو Disabled tests.
- Fake success paths.
- Placeholder implementation.

كل ادعاء بالتكرار أو عدم الاستخدام يجب أن يستند إلى تتبع مراجع واستدعاءات بقدر كافٍ، لا إلى تشابه الأسماء فقط.

---

## التاسع والعشرون — فحص الاختبارات والأدلة

لكل اختبار أو مجموعة اختبارات سجلوا:

- الاسم والنطاق.
- SHA.
- البيئة والأوامر.
- النتيجة: `PASS / FAIL / SKIPPED / BLOCKED / NOT RUN`.
- السجل أو Artifact الداعم.
- هل الاختبار يثبت السلوك المطلوب فعليًا أم يختبر جزءًا شكليًا؟

إذا تغير HEAD، تلغى صلاحية الحكم السابق للرأس الجديد حتى يعاد إثباته.

لا يجوز استخدام نتيجة CI قديمة أو من فرع آخر لإعلان نجاح الخط الحالي.

### مراجعة Release / Deployment Reality

يجب تنفيذ مراجعة مستقلة لواقع الإصدار والنشر والتشغيل، لا الاكتفاء بوجود CI أو ملفات Publish. وتشمل، ضمن حدود الوصول المثبتة:

- طريقة بناء Artifacts القابلة للتسليم وربطها بـCommit/Tag/SHA.
- Versioning وTags وReleases ومصدر النسخة الحاكمة.
- Desktop packaging/installer والتوقيع الرقمي إن كان مطلوبًا أو موجودًا.
- Mobile packaging/signing ومتطلبات Android/iOS أو المنصات المستهدفة عند الانطباق.
- API/Backend deployment model وEnvironment configuration.
- Provisioning وإعداد بيئة جديدة من الصفر.
- Database setup، migrations، upgrade path، rollback/recovery path، دون تنفيذ مدمر.
- Configuration وSecrets المطلوبة للتشغيل، مع منع إظهار الأسرار نفسها.
- Startup dependencies والخدمات الخارجية المطلوبة.
- Release notes أو deployment instructions أو runbooks المتاحة.
- Operator/Admin documentation اللازمة للتثبيت والتشغيل والاسترداد.
- Backup/Restore وDisaster/Recovery evidence عند الانطباق.
- قدرة ربط Artifact منشور أو قابل للنشر بالـSHA والاختبارات الحاكمة.
- أي فجوة تجعل النظام قابلًا للبناء نظريًا لكنه غير قابل للتثبيت أو الترقية أو الاسترداد بصورة مضبوطة.

وتصنف النتيجة لكل مكون إلى:

`RELEASE-READY / PARTIAL / FOUNDATION ONLY / NOT IMPLEMENTED / BLOCKED / UNKNOWN`

ولا يعني وجود Artifact أو Workflow أن Release/Deployment مكتمل ما لم تثبت سلسلة: Source SHA → Build/Test → Artifact → Configuration/Provisioning → Install/Deploy → Upgrade/Recovery Evidence.

---

## الثلاثون — المطلوب من TEAM-A وTEAM-B

يصدر كل فريق تقريرًا مستقلًا يحتوي، على الأقل، على:

1. Audit baseline المستخدم.
2. حالة المشروع الفعلية.
3. شجرة Solution والعدد الحقيقي للمشاريع.
4. وظيفة وحالة كل Project.
5. الأنظمة الوظيفية الموجودة والمفقودة.
6. الملفات والمكونات المهمة.
7. الشاشات المكتملة والجزئية وغير المتصلة والمفقودة.
8. المكونات المشتركة والتكرار.
9. الملفات الموجودة في أماكن غير مناسبة.
10. المشكلات المعمارية.
11. قاعدة البيانات والمهاجرات.
12. الأمن والعزل.
13. Offline/Sync.
14. Desktop وMobile.
15. الشحن والتذاكر والمحاسبة.
16. CI والاختبارات وأمن سلسلة التوريد.
17. الفروع والعمل المحلي وغير المدمج.
18. الفجوات بين الكراسة والكود.
19. P0/P1/P2/P3 وINFO/N/A.
20. الأمور غير المؤكدة وموانع الوصول.
21. سجل الملفات والمصادر التي تمت مراجعتها.
22. فصل الحقائق عن التوصيات.
23. الخصوصية والبيانات الحساسة.
24. `DOMAIN_COVERAGE_MATRIX.md` وحالة تغطية كل Domain.
25. جرد Git المحلي والـTags والنسخ البديلة ومصالحة Local ↔ Remote.
26. جرد Codex sessions والـpatches والعمل المحلي/غير الملتزم ضمن حدود الوصول.
27. `WORKSPACE_PRESERVATION_REGISTER.md` للأعمال التي يجب المحافظة عليها.
28. Release/Deployment Reality وحالة الجاهزية التشغيلية للإصدار والنشر.

---

## الحادي والثلاثون — إقفال تقارير المراحل وحماية استقلالها

ينشأ سجل مركزي باسم:

`AUDIT_REPORT_SEAL_REGISTER.md`

عند إكمال أي تقرير مرحلي أو نهائي، يسجل:

- اسم الفريق والمرحلة.
- وقت البدء ووقت الإغلاق بـUTC والتوقيت المحلي.
- Audit baseline المرجعي.
- `AUDIT SUBJECT` و`AUTHORITATIVE CURRENT LINE`.
- full SHA أو الرؤوس الأخرى التي شملها التقرير وتصنيف كل منها.
- اسم التقرير ومساره.
- `SHA-256` لمحتوى التقرير بعد الإقفال.
- Version لكل من Evidence Index وSource Register وFiles Reviewed Register وUnknown Register المستخدم في التقرير.
- عدد Findings والأدلة والمجهولات.
- أدوار المراجعين الموقعين: المراجع الفني، مراجع الأثر، ومراجع الأدلة عند الانطباق.
- حالة التقرير: `SEALED / REOPENED / SUPERSEDED`.
- تصريح بأن أي تعديل لاحق للتقرير ينشئ نسخة جديدة موثقة ولا يستبدل النسخة المختومة بصمت.

يضيف TEAM-A وTEAM-B كل على حدة التصريح التالي:

`INDEPENDENCE DECLARATION: THIS TEAM DID NOT READ OR RELY ON THE OTHER INDEPENDENT TEAM'S INITIAL REPORT BEFORE SEALING ITS OWN INITIAL REPORT.`

لا يبدأ TEAM-D قبل إقفال تقارير TEAM-A وTEAM-B وTEAM-C1. ولا يبدأ TEAM-C2 قبل إقفال تقرير TEAM-D. ولا يبدأ TEAM-E قبل إقفال TEAM-C2. ولا تصدر بوابة المصالحة قبل إقفال التقرير الموحد النهائي.

---

## الثاني والثلاثون — المطلوب من TEAM-C1 وTEAM-C2

### تقرير TEAM-C1

يوثق:

- Current Visual Studio Solution Tree.
- Current Physical Repository Tree.
- العلاقات والتبعيات الحالية.
- مواقع Projects وModules وScreens وServices وContracts وData وTests.
- المشكلات التنظيمية المثبتة.
- الفروق بين Solution Folders والمجلدات الفعلية.

### تقرير TEAM-C2

يصمم تصورًا مقترحًا يشمل:

- Target Visual Studio Solution Tree.
- Target Physical Repository Tree.
- Projects وModules وFeature folders.
- Screens وShared components وLookups.
- Services وContracts وData وInfrastructure.
- Tests وMobile وDesktop وAPI.
- Offline وReporting.

ولكل تغيير مقترح يوضح:

- الوضع الحالي المثبت.
- الوضع المقترح.
- السبب.
- الأثر والفائدة.
- المخاطر.
- الاعتمادات والمتطلبات السابقة.
- متطلب الحفظ (`PRESERVATION REQUIREMENT`): ما الذي يجب الحفاظ عليه قبل أي Move أو Merge أو Split أو Rename أو Refactoring مستقبلي.
- ما الذي يحتاج قرار مالك قبل التنفيذ.

يجب أن يغطي `PRESERVATION REQUIREMENT`، بحسب نوع التغيير، عناصر مثل:

- Runtime behavior الصحيح المثبت.
- Migration lineage وتسلسل قاعدة البيانات.
- سلامة البيانات ومعانيها ومعرفاتها.
- API وContract compatibility.
- Audit history وLegal Hold وRetention.
- Security وTenant/Branch isolation boundaries.
- Offline/Sync compatibility وIdempotency guarantees.
- الاختبارات والأدلة الحاكمة المرتبطة بالسلوك القائم.
- أي عمل غير مدمج أو محلي ذي قيمة يجب حفظه قبل إعادة التنظيم.

لا يعتمد أي اقتراح لإعادة التنظيم إذا لم يبين بوضوح ما الذي قد يضيع أو ينكسر، وكيف يجب الحفاظ عليه قبل التنفيذ اللاحق.

ممنوع تنفيذ إعادة الهيكلة ضمن هذه المهمة.

---

## الثالث والثلاثون — متطلبات الشجرة المقترحة

يجب أن تكون الشجرة واضحة للمطور داخل Visual Studio، وأن تجيب بوضوح عن:

- أين المحاسبة والشحن والتذاكر والرحلات؟
- أين الإعدادات وMaster Data والعمليات والتقارير؟
- أين الشاشات؟
- أين Dialog اختيار الحساب والعميل وبقية Lookups المشتركة؟
- أين المكونات والخدمات المشتركة؟
- أين API Contracts؟
- أين قاعدة البيانات وMigrations؟
- أين Offline وMobile والاختبارات؟

يجب التفريق بين:

1. **Visual Studio Solution Folders** — التنظيم المنطقي الظاهر داخل Visual Studio.
2. **Physical Folder Structure** — المجلدات الفعلية على القرص وداخل Git.

ولا يجوز تكرار شاشة أو مكون مشترك في وحدات متعددة إذا ثبت أن الصحيح جعله مكونًا مشتركًا، مع بقاء ذلك توصية لا تنفيذًا.

---

## الرابع والثلاثون — المطابقة النهائية بواسطة TEAM-D

يقارن TEAM-D كل Finding، ولا يختار تقريرًا كاملًا على حساب الآخر.

ينشئ TEAM-D Crosswalk شاملًا لكل Findings الصادرة عن TEAM-A وTEAM-B وTEAM-C1. ولا تقتصر المصالحة على نقاط الخلاف؛ فالنتائج المتطابقة تسجل كذلك، والنتيجة التي اكتشفها فريق واحد فقط يعاد التحقق منها ولا تسقط بسبب غيابها من تقرير الفريق الآخر.

لكل اختلاف ينشئ سجلًا يحتوي:

- Finding ID.
- رأي TEAM-A ودليله.
- رأي TEAM-B ودليله.
- علاقة TEAM-C1 إن وجدت.
- إعادة التحقق.
- الدليل الحاكم.
- حكم المصالحة (`RECONCILIATION DETERMINATION`).
- حالة التنفيذ المنفصلة (`IMPLEMENTATION STATUS`).
- حالة التحقق (`VERIFICATION STATUS`).
- التصنيف الزمني.
- درجة الثقة.
- الأثر.
- الإجراء المقترح.

قيم `RECONCILIATION DETERMINATION` المسموح بها:

- `CONFIRMED`
- `PARTIALLY CONFIRMED`
- `SUPERSEDED`
- `FALSE`
- `UNKNOWN — REQUIRES VERIFICATION`
- `ACCESS BLOCKED — UNKNOWN`

لا يبقى اختلاف بلا حكم أو حالة UNKNOWN صريحة مرتبطة بسبب واضح.

---

## الخامس والثلاثون — مراجعة TEAM-E

بعد TEAM-D وTEAM-C2، يراجع المجلس:

- جميع P0 وP1.
- عينة مبررة من P2 وP3.
- الأحكام التي اختلفت فيها الفرق.
- سلامة العزل والأمن وقاعدة البيانات.
- الخصوصية والبيانات الحساسة.
- أمن سلسلة التوريد وCI/CD.
- اكتمال الربط بين التشغيل والمحاسبة.
- قابلية البنية المقترحة للتطبيق.
- مخاطر نقل الملفات أو دمج المشاريع أو فصلها مستقبلًا.
- كفاية الأدلة لإعداد خطة معالجة مستقلة.

لا يغير المجلس الحقيقة المثبتة برأي مجرد؛ أي اعتراض يجب أن يرتبط بدليل أو فجوة دليل.

بالنسبة إلى عينة P2/P3، يسجل TEAM-E حجم المجتمع، ومعيار الاختيار، وعدد العناصر المختارة، ومعرفاتها، وتغطية المجالات، وحدود الاستنتاج. كما تسجل الآراء المخالفة والتحفظات دون إخفائها، مع بيان هل تغير الحكم الحاكم أم بقي اعتراضًا استشاريًا.

### قاعدة إعادة فتح المصالحة بعد TEAM-E

إذا قدم TEAM-E دليلًا جديدًا موثقًا يؤدي إلى إعادة فتح Finding سبق أن أقفله TEAM-D، تطبق الدورة الحاكمة التالية دون اختصار:

1. يسجل الـFinding والتقرير المتأثران بحالة `REOPENED` داخل `AUDIT_REPORT_SEAL_REGISTER.md` و`AUDIT_OUTPUT_MANIFEST.md`، مع الحفاظ على النسخة المقفلة السابقة وسلسلة نسبها وعدم استبدالها بصمت.
2. يعود TEAM-D للتحقق من الدليل الجديد، ويصدر نسخة جديدة موثقة من حكم المصالحة للـFinding المتأثر. ولا تصبح النسخة السابقة `SUPERSEDED` إلا بعد إقفال النسخة الجديدة وتسجيل SHA-256 الخاص بها.
3. إذا كان الحكم المتغير يؤثر في أي قرار أو شجرة أو فصل مسؤوليات أو `PRESERVATION REQUIREMENT` داخل TEAM-C2، يعيد TEAM-C2 تقييم الأجزاء المتأثرة فقط، ويصدر نسخة جديدة موثقة من تقريره أو ملحقًا حاكمًا مرتبطًا بالنسخة الأصلية.
4. يعيد TEAM-E مراجعة النتيجة التي تغيرت، ويصدر نسخة محدثة موثقة من حكمه الاستشاري على الـFinding أو المجال المتأثر.
5. يعاد التحقق من كل Finding أو Priority أو Recommendation أو جزء من Master Report أو شرط Gate يعتمد على الحكم المعاد فتحه، مع تحديث Crosswalk وسلسلة الاعتماد صراحةً.
6. إذا انتهت إعادة التحقق إلى عدم تغيير الحكم، تسجل النتيجة `REOPENED → RECONFIRMED` مع إقفال نسخة جديدة موثقة؛ ولا يعاد استخدام الختم القديم بوصفه كافيًا وحده.
7. لا يجوز إصدار أو إقفال `TRANSPORTERP_MASTER_DEEP_AUDIT_AND_ARCHITECTURE_REPORT_2026-08-28.md` أو `AUDIT_RECONCILIATION_GATE_2026-08-28.md` بينما يوجد Finding حاكم أو تقرير تابع له بحالة `REOPENED` لم يعاد إقفاله.
8. يجب أن يحافظ `AUDIT_OUTPUT_MANIFEST.md` على سلسلة النسخ كاملة: النسخة السابقة، سبب إعادة الفتح، الدليل الجديد، النسخة البديلة، حالة `SEALED / REOPENED / SUPERSEDED / RECONFIRMED`، وSHA-256 لكل مخرج حاكم.

تكون دورة إعادة الفتح الحاكمة عند تحقق هذه الحالة:

`TEAM-E NEW EVIDENCE → REOPEN → TEAM-D RECONCILIATION → TEAM-C2 REASSESSMENT IF IMPACTED → TEAM-E RE-REVIEW → MASTER/GATE REVALIDATION`

ولا يجوز تجاوز أي مرحلة متأثرة أو الإبقاء على Master Report أو Gate مبنيين على نسخة مصالحة أصبحت `REOPENED` أو `SUPERSEDED`.

---

## السادس والثلاثون — التقرير الموحد النهائي

يصدر التقرير باسم:

`TRANSPORTERP_MASTER_DEEP_AUDIT_AND_ARCHITECTURE_REPORT_2026-08-28.md`

ويحتوي على:

### أ — Audit Baseline وحدود الوصول

موضوع التدقيق، والخط الحالي الحاكم وSHA الخاص به، والرؤوس والتواريخ والمصادر والبيئات التي بُني عليها الحكم.

### ب — الواقع الحالي

ما هو موجود فعليًا في الخط الحالي.

### ج — المنجز

ما ثبت أنه يعمل، مع SHA واختبار أو دليل مناسب.

### د — الجزئي

ما بدأ ولم يكتمل.

### هـ — غير المنجز

ما لا يوجد له Runtime حقيقي.

### و — الأخطاء والتكرار وسوء التنظيم

العيوب المثبتة والكود والمشاريع والشاشات والخدمات المكررة والملفات الموضوعة في أماكن غير مناسبة.

### ز — الأعمال غير المدمجة

Git وPRs وWorktrees وCodex والعمل المحلي.

### ح — الفجوات مقابل الكراسة

Requirement Gap Matrix وربط Requirement → Code → Data → Test → Evidence.

### ط — المخاطر

P0/P1/P2/P3 مرتبطة بالأدلة.

### ي — الشجرة الحالية

Current Solution Tree وCurrent Physical Tree.

### ك — الشجرة المقترحة

Target Solution Tree وTarget Physical Tree.

### ل — خطة المعالجة

خطة على مستوى الترتيب والاعتمادات والمخاطر فقط، دون تنفيذ.

### م — المجهولات والموانع

كل ما لم يمكن إثباته ولماذا.

### ن — سجل المصالحة والمراجعة الاستشارية

ملخص أحكام TEAM-D وTEAM-E والتحفظات المتبقية.

### س — فهرس المخرجات وسلامتها

أسماء جميع التقارير والسجلات المقفلة، ونسخها، وSHA-256 لكل مخرج، وحالة `SEALED / REOPENED / SUPERSEDED`.

### ص — تغطية المجالات

خلاصة `DOMAIN_COVERAGE_MATRIX.md`، مع بيان أي مجال بقي `PARTIAL / BLOCKED / NOT REVIEWED` وأثره على الحكم والبوابة.

### ق — حماية الأعمال المحلية وغير المدمجة

خلاصة `WORKSPACE_PRESERVATION_REGISTER.md` للأصول التي يجب المحافظة عليها قبل أي Merge أو Cleanup أو Reorganization لاحق.

### ر — واقع الإصدار والنشر

نتائج Release/Deployment Reality، بما يشمل Artifact traceability وPackaging/Signing وProvisioning وDatabase upgrade/rollback وRunbooks وRecovery evidence.

### ع — الخصوصية وسلسلة التوريد

النتائج المتعلقة بالبيانات الحساسة، والحزم والتبعيات، وCI/CD، ومصدر Artifacts وسلامتها.

---

## السابع والثلاثون — تصنيف الأولويات

تستخدم الأولويات التالية:

- `P0` — خطر مثبت قد يؤدي إلى خرق أمني، أو كسر العزل، أو فساد أو فقدان البيانات، أو فساد محاسبي، أو فقدان عمل ذي قيمة، أو يجعل الاستمرار في التنفيذ أو الإطلاق غير آمن قبل معالجته.
- `P1` — خلل جوهري عالي الأثر يجب معالجته في الموجة التالية المباشرة.
- `P2` — نقص مهم لكنه لا يمنع تثبيت الأساس إذا كانت مخاطره مضبوطة.
- `P3` — تحسين أو تنظيف أو تنظيم مؤجل.
- `INFO` — حقيقة أو نتيجة جرد أو نقطة إيجابية لا تتطلب معالجة.
- `N/A` — الأولوية لا تنطبق على هذا السجل.

لا تمنح الأولوية بناءً على الانطباع؛ يجب توضيح الاحتمال والأثر والاعتمادات والدليل.

---

## الثامن والثلاثون — بوابة المصالحة والإغلاق

بعد مراجعة TEAM-E وإصدار التقرير الرئيسي، يصدر سجل:

`AUDIT_RECONCILIATION_GATE_2026-08-28.md`

بحالة واحدة فقط:

- `READY FOR REMEDIATION PLANNING`

أو:

- `NOT READY — CRITICAL EVIDENCE GAPS REMAIN`

لا تصدر حالة `READY FOR REMEDIATION PLANNING` إلا عند تحقق جميع الشروط التالية:

- تحديد `AUDIT SUBJECT` و`AUTHORITATIVE CURRENT LINE` وfull SHA.
- إقفال تقارير TEAM-A وTEAM-B وTEAM-C1 مع إثبات الاستقلال.
- اكتمال `TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md` وعدم وجود ادعاء بفريق أو استقلال غير مثبت.
- إقفال تقرير TEAM-D بعد الحكم على جميع الاختلافات Finding-by-Finding.
- إقفال TEAM-C2 وTEAM-E والتقرير الموحد النهائي.
- عدم بقاء فجوة دليل حرجة تمنع تحديد أو تقييم P0 أو P1.
- ارتباط كل P0 وP1 بدليل مباشر أو بحالة تحقق صريحة، وعدم تقديم مجهول حرج بوصفه حقيقة.
- اكتمال السجلات الأساسية المحددة في هذا الأمر ضمن حدود المصادر المتاحة، مع توثيق كل قيد وصول.
- اكتمال `DOMAIN_COVERAGE_MATRIX.md` وعدم بقاء Domain حرج بلا حالة تغطية وحكم واضح.
- اكتمال `WORKSPACE_PRESERVATION_REGISTER.md` لكل عمل محلي أو غير مدمج ذي قيمة محتملة تم اكتشافه، أو تسجيل سبب تعذر الحكم عليه.
- اكتمال مراجعة Release/Deployment Reality ضمن حدود الوصول، وعدم وجود فجوة P0/P1 غير محسومة تمنع التثبيت أو الترقية أو الاسترداد الآمن.
- ضبط أي تغير في Baseline داخل `AUDIT_BASELINE_DELTA_LOG.md` وإعادة التحقق من النتائج المتأثرة.
- إقفال فهرس المخرجات وتسجيل SHA-256 لكل تقرير حاكم.

تعني `READY FOR REMEDIATION PLANNING` أن الواقع أصبح معروفًا بدرجة كافية لإعداد **خطة معالجة مستقلة قابلة للمراجعة**. ولا تعني السماح بالتعديل أو الحذف أو النقل أو الدمج أو بدء التنفيذ البرمجي.

بعد اعتماد خطة المعالجة من المالك، يظل بدء التنفيذ محتاجًا إلى أمر تنفيذ مستقل وصريح.

وتكون الحالة `NOT READY — CRITICAL EVIDENCE GAPS REMAIN` إذا أخفق أي شرط حاكم مما سبق، أو بقيت فجوات أدلة حرجة تمنع فهم الحالة الحالية أو ترتيب المخاطر بصورة موثوقة.

يجب أن يسجل ملف البوابة: الحالة، والأسباب، والأدلة، والشروط غير المحققة، والمجهولات المانعة، وأسماء/أدوار المراجعين، وSHA-256 للتقرير الموحد الذي بني عليه الحكم.

---

## التاسع والثلاثون — ما يمنع منعًا مطلقًا

ممنوع داخل المستودع الأصلي:

- حذف أو نقل أو إعادة تسمية الملفات.
- حذف الفروع أو Worktrees أو Stashes.
- إنشاء Worktree جديد مرتبط بالمستودع الأصلي.
- تعديل Solution أو `.csproj`.
- تعديل Source أو Tests أو Migrations.
- Merge أو Cherry-pick أو Rebase.
- Commit أو Push أو Force-push.
- إغلاق PR أو تعديل Issue أو Review.
- تعديل Production configuration.
- تعديل قاعدة بيانات حقيقية.
- تشغيل destructive migrations.
- تشغيل خدمات إنتاج.
- اعتبار التخمين حقيقة.
- إعادة الهيكلة أو تنفيذ التصميم المقترح.

الاستثناءات الوحيدة:

1. إنشاء ملفات وتقارير المراجعة في مساحة Audit المستقلة.
2. إنشاء Clone/Sandbox مؤقت خارج المستودع الأصلي للتحقق الآمن، إذا أمكن، دون اتصال مدمر ببيئات حقيقية ودون دفع أي تغيير.

---

## الأربعون — قاعدة الدليل

لا تقبل جملة مثل:

> المحاسبة مكتملة.

بل يجب أن يكون الحكم مثل:

`ACCOUNTING-017`

حالة التنفيذ (`IMPLEMENTATION STATUS`):

`PARTIAL`

حالة التحقق (`VERIFICATION STATUS`):

`PARTIALLY VERIFIED`

التصنيف الزمني:

`CURRENT`

الأولوية:

`P1`

الحقيقة المشاهدة:

> الكيانات والعقود الأساسية موجودة، لكن Posting Runtime أو Desktop integration أو التقارير لم تثبت مكتملة.

الدليل:

- Project.
- File.
- Class.
- Method/Endpoint.
- Migration.
- Test/Result.
- Branch/SHA.

التوصية تسجل منفصلة عن الحقيقة.

---

## الحادي والأربعون — منع التسرع والبحث الانتقائي

لا يصدر تقرير سريع مبني على:

- أسماء المجلدات.
- أول نتيجة بحث.
- README فقط.
- تقرير سابق.
- رأي مساعد سابق.

إذا احتاج الحكم إلى فتح ملفات مترابطة، أو تتبع Git history، أو مقارنة Migration مع DbContext، أو مقارنة شاشة مع API وContract، فيجب تنفيذ ذلك وتسجيل الملفات التي تمت مراجعتها.

سجل `FILES_REVIEWED_REGISTER.md` هو جزء من الدليل على عمق المراجعة، لكنه لا يحول عدد الملفات إلى بديل عن جودة التحليل.

---

## الثاني والأربعون — الأسئلة النهائية الملزمة

يجب أن يقدم التقرير النهائي إجابة موثقة عن:

1. ما هو TransportERP فعليًا اليوم؟
2. ما `AUDIT SUBJECT` وما `AUTHORITATIVE CURRENT LINE` وSHA اللذان بُني عليهما الحكم؟
3. كم Project موجود فعليًا؟
4. لماذا يوجد كل Project؟
5. هل بنية Solution الحالية صحيحة؟
6. هل التنظيم داخل كل Project صحيح؟
7. ما الأنظمة الوظيفية الموجودة والناقصة؟
8. ما الشاشات المنفذة فعليًا، وما الشكلية أو غير المتصلة؟
9. ما المكونات التي يجب تقييم جعلها Shared؟
10. ما التكرارات والملفات الموضوعة في أماكن غير مناسبة؟
11. ما الفروع وWorktrees والأعمال المحلية التي يجب المحافظة عليها؟
12. ما الذي يطابق الكراسة وما الذي يخالفها؟
13. ما الموجود في الكود دون قرار أو متطلب معروف؟
14. ما P0/P1/P2/P3، وما السجلات المصنفة INFO أو N/A؟
15. ما الشجرة الحالية والشجرة الصحيحة المقترحة؟
16. ما ترتيب المعالجة الصحيح واعتماداته؟
17. ما `PRESERVATION REQUIREMENTS` اللازمة قبل أي إعادة تنظيم مستقبلية؟
18. ما الذي يقترح دمجه أو إعادة تنظيمه مستقبلًا؟
19. ما الذي يقترح إيقافه أو استبعاده لاحقًا بعد قرار مستقل؟
20. ما الذي ما زال مجهولًا ولم يمكن إثباته؟
21. ما مخاطر الخصوصية والبيانات الحساسة؟
22. ما مخاطر الحزم والتبعيات وسلسلة التوريد وCI/CD؟
23. هل جميع التقارير والسجلات مقفلة ومرتبطة بـSHA-256؟
24. هل غطت المراجعة جميع Domains المطلوبة، وما المجالات الجزئية أو المحجوبة؟
25. ما الفروع والـTags والنسخ البديلة وCodex sessions والعمل المحلي الذي يجب الحفاظ عليه؟
26. هل توجد سلسلة Release/Deployment قابلة للإثبات من Source SHA حتى Install/Deploy/Upgrade/Recovery؟
27. هل الأدلة كافية للانتقال إلى Remediation Planning؟

---

## الثالث والأربعون — شرط الإغلاق النهائي

لا تعتبر المهمة منتهية بمجرد كتابة تقرير.

تغلق فقط عندما:

- يثبت Audit Baseline وحدود الوصول.
- يحدد `AUDIT SUBJECT` و`AUTHORITATIVE CURRENT LINE` وSHA؛ أو تسجل عدم القدرة على تحديدها كفجوة حرجة تمنع بوابة الجاهزية.
- يسلم TEAM-A تقريره المستقل المقفل.
- يسلم TEAM-B تقريره المستقل المقفل.
- يسلم TEAM-C1 تقرير البنية الحالية المقفل.
- ينهي TEAM-D المطابقة Finding-by-Finding ويقفل تقرير المصالحة.
- يسلم TEAM-C2 البنية المستهدفة المقترحة المقفلة.
- يراجع TEAM-E النتائج الحرجة ويصدر تقريره المقفل.
- لا يبقى اختلاف دون حكم أو UNKNOWN واضح.
- ترتبط P0/P1/P2/P3 بأدلة.
- تراجع الخصوصية وسلسلة التوريد وCI/CD ضمن حدود الوصول المثبتة.
- تكتمل `DOMAIN_COVERAGE_MATRIX.md` وتظهر حالة كل Domain المطلوب.
- يكتمل `WORKSPACE_PRESERVATION_REGISTER.md` للأعمال المحلية وغير المدمجة ذات القيمة المحتملة.
- تكتمل مراجعة Release/Deployment Reality ضمن حدود الوصول وتنعكس نتائجها في التقرير الموحد.
- تكتمل السجلات الأساسية و`AUDIT_REPORT_SEAL_REGISTER.md` و`AUDIT_OUTPUT_MANIFEST.md`.
- يثبت `TEAM_FORMATION_AND_ASSIGNMENT_REGISTER.md` تشكيل الفرق والأدوار وحدود الاستقلال الفعلية.
- توثق الشجرة الحالية والمقترحة.
- توضح خطة المعالجة التالية دون تنفيذ.
- يصدر ويقفل `TRANSPORTERP_MASTER_DEEP_AUDIT_AND_ARCHITECTURE_REPORT_2026-08-28.md` بعد اكتمال المصالحة والمراجعة الاستشارية.
- تصدر بوابة المصالحة بحكمها النهائي بعد إصدار التقرير الموحد ومراجعته.

بعد استكمال ما سبق فقط تعتبر حزمة المراجعة النهائية، بما فيها:

**MASTER DEEP AUDIT REPORT**

و:

**AUDIT RECONCILIATION GATE**

مغلقة وجاهزة للتسليم. ولا يبدأ أي تعديل أو إعادة هيكلة أو تنفيذ إلا بأمر مستقل جديد وصريح من المالك.

---

# الأمر التنفيذي النهائي للفرق

ابدؤوا الآن بتشكيل الفرق المذكورة وتوزيع الأدوار والتخصصات والصلاحيات ضمن حدود القراءة والمراجعة.

ثبتوا Audit Baseline قبل التحليل.

اعمل TEAM-A وTEAM-B باستقلال كامل، واعمل TEAM-C1 على توثيق البنية الحالية، ثم أغلقوا التقارير واختِموها قبل تبادل النتائج.

ممنوع التخمين، وممنوع الاعتماد على أسماء الملفات والعناوين، وممنوع إصدار حكم فردي غير مراجع.

راجعوا المشروع من الجذر ومن تاريخ تأسيسه وحتى حالته الحالية، ضمن حدود الوصول المثبتة، وشملوا:

**Repository + Git + GitHub + Codex + Worktrees + Local/Unmerged Work + Tags + Alternative Copies + Visual Studio + Solution + Projects + Source Code + Database + Migrations + Tests + CI/CD + Supply Chain + Release/Deployment + Kurrasa + Screens + Business Domains + Privacy + Governance + Evidence + Preservation.**

لا تعدلوا المشروع، ولا تحذفوا أو تنقلوا أو تدمجوا أو تدفعوا شيئًا.

اكتشفوا الواقع كما هو، وافصلوا الحقيقة عن التوصية، والحالة الحالية عن التاريخية، والعمل المدمج عن غير المدمج.

ثم نفذ TEAM-D المطابقة، وأعد TEAM-C2 الشجرة المقترحة، وراجع TEAM-E النتائج الحرجة، ثم قدموا التقرير الموحد وبوابة المصالحة.

**أي معلومة بلا دليل = `UNKNOWN — REQUIRES VERIFICATION`.**

**أي مصدر متعذر الوصول = `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`.**

**أي نتيجة Build/Test بلا SHA وبيئة وسجل = غير صالحة كدليل حاكم.**

**لا يبدأ التنفيذ إلا بأمر مستقل جديد من المالك.**
