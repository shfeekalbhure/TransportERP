# مراجعة الأكواد والإصلاحات — 2026-10-05
تمت المراجعة على النسخة الرسمية المحفوظة التي سبق تسليمها، وليس على المرفقات التي تعذر تنزيلها.
فُحصت مراجع المشاريع وملفات XML، مع مراجعات مركزة للمسارات الموضحة أدناه. ليست شهادة خلو جميع الأكواد من الأخطاء.
لم يتوفر dotnet أو بيئة ويندوز، لذلك لم ينفذ Build أو اختبارات التشغيل أو قاعدة البيانات. الاختبارات الجديدة لم تشغل.
لأخطاء حظر الموارد المحلية، نفذ في PowerShell إذا كنت تثق بالملفات:
```powershell
Get-ChildItem -LiteralPath 'C:\SS\9674a920-42c7-46e1-809e-536f05aeca13\TransportERP' -Recurse -File -Filter '*.resx' | Unblock-File
```
ثم افتح الحل المعدل ونفذ Clean Solution ثم Rebuild Solution. إزالة الحظر ليست إصلاحًا لتكرار الموارد؛ تعديل المشروع المرفق يعالج التكرار المثبت.


# مراجعة جزء Desktop — 2026-10-05

النطاق: حصر 144 ملف C# داخل TransportERP.Desktop. مراجعة مركزة لمسار الدخول واختيار النطاق، DesktopSession، إغلاق مساحة العمل، التسجيل والملاحة، FoundationUiSession وWorkspaceChangeSnapshot، وشاشة المستخدمين. هذا ليس إثباتًا لسلامة كل سطر في المشروع.

## إصلاحات مثبتة
1. `TransportERP.Desktop/TransportERP.Desktop.csproj`: أضيفت قيمتان فريدتان ManifestResourceName وLogicalName إلى FrmMain.AuthenticatedSession.resx. الملف الجزئي يعرّف FrmMain وكان موردُه يستنتج نفس اسم FrmMain.resx؛ وهذا يفسر ازدواج ملف FrmMain.resources. احتُفظ بملفات الموارد ومحتواها وبإعداد PhaseOneNavigation الموجود.
2. `TransportERP.Desktop/Forms/Setup/Security/FrmUsersPermissions.cs`: إزالة المراجع القديمة إلى عناصر غير موجودة في المصمم (usersCommandBar1، policyActionsV20، btnV20...). المصمم الحالي يحتوي pnlToolbar مع button1 بعنوان جديد وbtnClose. رُبط الزران بأوامر NewUser وCloseScreen الموجودة أصلًا عبر ToolbarCommandBindings.TryInvoke. اختصار F6 يستخدم نفس الربط. لم تُنشأ أدوات شكلية لإسكات الأخطاء، ولم تُحذف وظائف أعمال.
3. `TransportERP.Desktop/Forms/Setup/Security/FrmUsersPermissions.SourceAdditions.cs`: تطبيق ضبط المظهر على pnlToolbar الموجود، وإزالة قياس الشريط الثاني الغائب من حساب الارتفاع.

الأزرار الأخرى في شريط المستخدمين الحالي لا تحمل أي Click handlers في المصمم أو الكود؛ أصبحت معطلة مع وصف عدم توفر الربط بالخدمة، محافظة على نية DisableUnimplementedPolicyActions السابقة. أزرار إضافة/حذف/تحقق/حفظ الأدوار في roleActions مستقلة ولم تتغير.

## تحقق فعلي
- قراءة XML ملف المشروع نجحت.
- أسماء موارد الأجزاء الثلاثة للشاشة الرئيسية أصبحت منفصلة في التعريف الصريح/الافتراضي.
- لا تبقى مراجع إلى الأسماء الثمانية الغائبة في ملفات شاشة المستخدمين.
- التصميم وأسماء الحقول ومفاتيح البيانات بقيت كما هي؛ تعديل ملفات الكود والمشروع الثلاثة المذكورة فقط.

## حدود
لا يتوفر dotnet/Windows هنا؛ لم يُنفّذ تجميع أو فتح المصمم أو اختبار تشغيل. يجب تنفيذ Rebuild Solution أولًا؛ هذا التقرير لا يضمن خلو المشروع من أخطاء أخرى. لم تُجر تغييرات على FoundationUiSession رغم قراءته لأنه يُجمع من مشروع مشترك وتحتاج سلوكياته لاختبارات مخصصة قبل أي تعديل.


# EmptyForms code review
Scope: TransportERP.EmptyForms only. Structural scan covered 141 C# files, 70 matching designer/code partial pairs, 47 resource XML files. All designer event-handler references were checked against code; no missing handler found. Duplicate private fields/control-use identifiers were scanned (string action keys and multi-field declarations inspected). All 47 XML resources parsed. This is static evidence, not compilation or execution.

## Fixed
File: TransportERP.EmptyForms/Forms/BatchFiveUiSession.cs
- New document/reset retained CSV preview in the three newly added documentImport tabs: original Clear looked only for Name dgvImport/txtCsvPath, while new controls had different/unset Names. Replaced name-based cleanup with registration of each actual preview/path pair in ChooseCsv, and reset all registered previews whenever a document is applied (including Clear). Prevents importing a stale preview from the previous document after reset or navigation.
- ChooseCsv now exits when session has no grid rather than dereferencing grid!. This public session API supports null grid.
- CSV import validates date cells before any rows are appended. Previously invalid date strings passed local import and reached the draft before subsequent validation.
- CSV checkbox values are parsed to bool before adding cells; nonempty invalid values reject the import before mutation. Previously assigning strings into checkbox cells could cause DataError or wrong payload type.

## Preserved / inspected
Existing command bindings, service keys, constructor signatures, permissions, persistence expectations, designer files and navigation untouched by this pass. Six ConfigureImportLayout methods preserve grid data and restore captured row heights; two notices remain based on their existing allocation service model. All eight designer class pair names match. Existing additional-account/defaults controls remain service-incomplete; no DTOs invented.

## Validation limitations / remaining risks
No dotnet SDK/Windows runtime available. No build, runtime DataGridView execution, designer load, DPI rendering, or backend persistence test was performed. Static structure checks are not proof that the whole solution compiles. Exact reference visual match and completion of display-only fields remain outstanding. CSV maximum remains 5 MiB / 1000 rows; native Excel not implemented. Manual Windows regression should cover choosing CSV then clearing/loading another document (preview/path must clear), valid/invalid date import, and True/False checkbox import on any caller using checkbox columns.


# Backend/domain review

Scope: TransportERP domain, Application, Contracts, Infrastructure, API, Tests and Mobile project folders. Broad textual scan of hand-written C# for exception handling, persistence/transaction patterns, placeholders and blocking calls; focused semantic review of SyncOperationService, DesktopLoginModule, API Program, VoucherLifecycleService, WaybillFinancialRules, WaybillFinanceApplicationService, finance store replay/transaction path, ConcurrencySafeWaybillRepository, money/context contracts and persistence interceptors. This is NOT exhaustive semantic verification of all source or generated EF migrations. Mobile projects have no substantive application source beyond generated scaffolding.

## Proven fixes (2 files)
- TransportERP.Infrastructure/Persistence/SyncOperationService.cs
  1. EnqueueSyncOperationCommand.BaseVersion was accepted by API but not assigned to SyncOperation; conflicts relying on stored base version lost it. Now persisted.
  2. Device/client operation keys and hash were trimmed when stored but not consistently normalized before security/replay queries and hash equality. Whitespace on replay could fail lookup and unique conflict recovery. Normalize once after validation and use canonical values consistently.
  3. Transition to FAILED changed EF-tracked Status before checking mandatory ErrorCode. Rejected request left a dirty entity that later SaveChanges could persist. Validate first, then mutate. Null/blank status also returns domain error instead of NullReferenceException.
- TransportERP.Tests/SyncOperationPersistenceTests.cs
  Added two PostgreSQL regression cases: canonical replay with persisted BaseVersion; rejected transition survives subsequent SaveChanges without changing stored SENDING status. Existing test infrastructure and database seed patterns reused.

## Validation
Changes inspected against API input and entity persistence flow. Existing CRLF preserved. Regression tests ADDED BUT NOT RUN: dotnet unavailable, no configured PostgreSQL test database. No claim of successful compilation, database migration or runtime validation.

## Remaining review items (not changed without wider contract/testing)
- Sync state changes and audit append occur in separate saves; audit failure may occur after business state commits. Requires deliberate transaction semantics review.
- SetPaymentPlan application checks ExpectedVersion before store's replay branch; repeated successful request may be rejected as stale before reaching idempotent store replay. Needs application/store regression test and clear payload-reuse contract before changing.
- Voucher lifecycle actorId parameters currently not recorded by that service, and external-reference replay lacks explicit payload comparison; validate intended P1 integration contract.
- Full generated migration/model equivalence, concurrency under PostgreSQL, and all API authorization paths require execution in configured environment. No blanket security or defect-free assurance.


# Components / SharedVisuals static audit

Scope: components/src, components/host, components/toolbar-preparation active projects, TransportERP.SharedVisuals. Historical host-before-reference-sync and followup/recovery-snapshots are excluded from live-code findings. bin/obj and evidence payloads excluded.

Result: No proven defect requiring a change found in this scoped review. No source changes made.

Reviewed:
- All six project XML files parsed; every explicit ProjectReference target exists.
- SharedUI intentionally excludes UserControl1*.cs and historical CoreUI; DesignerDraft explicitly includes the toolbar files and references SharedUI commands. These are not missing production compile items.
- Host resources directory resolves through ReferenceHost.local.props to Desktop/Properties; optional host has an explicit missing-resource build guard.
- SharedVisuals links Desktop/CoreUI and Templates, while Desktop excludes those paths. GroupCodesGrid has matching designer/partial declarations.
- Toolbar constructor calls routing initialization once; button routing matches six enum commands; owner replacement replaces state rather than adding event handlers; disposed and disabled toolbar/button checks prevent dispatch; invocation finally clears reentry state after callback exceptions.
- Audit display mapping aligns created/modified users, timestamps, devices, and print/modification counts with designer labels. DisplayValues is hidden from designer serialization. Display controls are read-only.
- Existing verification harness covers synthetic click routing and disposal; it was inspected, NOT executed here.

Potential hardening (not changed): toolbar reentry guard is entered after its canInvoke predicate, so a predicate recursively calling TryInvoke for the same command could recurse. The actual production bindings inspected use simple disposal predicates and do not trigger this case; no proven production defect identified.

Limits: no .NET SDK/Windows compilation or WinForms runtime available. XML/path validation is not compilation. Visual DPI/RTL rendering, business integration, image/resource resolution by MSBuild, and linked Desktop/CoreUI behavior require separate reviews/build execution.
