namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: Scribd 973153595, ONYX ERP v8 تهيئة النظام, printed pp.71–85.
public sealed partial class UcEmployeeData
{
    private sealed record Section(string Title, Field[] Fields, Table? Grid = null, string? Notice = null);
    private static Field T(string name, string label) => new(name, label);
    private static Field L(string name, string label) => new(name, label, FieldKind.Lookup);
    private static Field D(string name, string label) => new(name, label, FieldKind.Derived);
    private static Field B(string name, string label) => new(name, label, FieldKind.Check);
    private static Field C(string name, string label, params string[] choices) => new(name, label, FieldKind.Choice, choices);
    private static Field E(string name, string label) => new(name, label, FieldKind.ForeignText);
    private const string Unspecified = "تفاصيل الحقول غير موضحة في النص المتاح؛ لم تُنشأ حقول أو بيانات افتراضية.";

    private static readonly Section[] Sections =
    {
        new("البيانات الرئيسية", new[] {
            T("txtEmployeeNumber", "رقم الموظف"), D("txtBranch", "الفرع"), L("cboCurrency", "العملة"),
            T("txtFirstLocal", "الاسم الأول (محلي)"), T("txtSecondLocal", "الاسم الثاني (محلي)"),
            T("txtThirdLocal", "الاسم الثالث (محلي)"), T("txtTitleLocal", "اللقب (محلي)"),
            E("txtFirstForeign", "الاسم الأول (أجنبي)"), E("txtSecondForeign", "الاسم الثاني (أجنبي)"),
            E("txtThirdForeign", "الاسم الثالث (أجنبي)"), E("txtTitleForeign", "اللقب (أجنبي)"),
            D("txtFullName", "الاسم الكامل"), T("txtHiredAt", "تاريخ التعيين"), L("cboStructure", "الهيكل الإداري") }),
        new("بيانات التوظيف", new[] {
            L("cboJob", "المسمى الوظيفي"), L("cboGrade", "الدرجة الوظيفية"), L("cboEmploymentType", "نوع التوظيف"),
            L("cboManager", "المسئول المباشر"), L("cboQualification", "المؤهل"), L("cboSpecialty", "التخصص"),
            D("txtCurrentStatus", "الوضع الحالي للموظف"), D("txtJobStatus", "الموقف من الوظيفة"),
            D("txtCurrentJobDate", "تاريخ الوظيفة الحالية"), D("txtLastEvaluation", "آخر تقييم للموظف"),
            new Field("chkEndBenefits", "تم استلام مستحقات نهاية الخدمة", FieldKind.DerivedCheck),
            C("cboWorkType", "نوع الدوام", "دوام اعتيادي", "دوام ورديات"), B("chkRetired", "تقاعد"),
            T("txtCivilCode", "الرمز الوظيفي"), T("txtBiometricNumber", "رقم الموظف في جهاز البصمة"),
            B("chkProbation", "فترة تجريبية"), B("chkProduction", "موظف إنتاج"), B("chkMaintenance", "موظف صيانة"),
            B("chkAttendanceSchedule", "مرتبط بالجدول الزمني للحضور والانصراف") }),
        new("البيانات الشخصية", new[] {
            L("cboMaritalStatus", "الحالة الاجتماعية"), T("txtBirthPlace", "محل الميلاد"), T("txtBirthDate", "تاريخ الميلاد"),
            L("cboNationality", "الجنسية"), L("cboGender", "الجنس"), T("txtIdentity", "رقم البطاقة"),
            B("chkSelfService", "الخدمة الذاتية للموظفين"), T("txtMobilePassword", "كلمة السر"),
            L("cboAttendanceMethod", "طريقة تحضير الموظف"),
            C("cboMobileAttendance", "نوع التحضير من الموبايل", "يدوية", "بصمة يد وعين ووجه"),
            C("cboLocations", "مواقع التحضير من الموبايل", "غير مستخدم", "كل المواقع", "مواقع محددة"),
            C("cboPhoto", "صورة الموظف أثناء التحضير", "غير مستخدم", "اختياري", "إجباري"),
            D("txtMobileSerial", "الرقم التسلسلي للموبايل"), T("txtTimeZone", "المنطقة الزمنية"),
            new Field("lstLocations", "ربط المواقع بالموظف", FieldKind.MultiLookup) }),
        new("البيانات المالية", new[] {
            D("txtSalaryState", "حالة الراتب"), C("cboPayment", "طريقة الدفع", "نقداً من الصندوق", "شيك من البنك", "إلى حساب الموظف في البنك"),
            T("txtWorkDays", "عدد أيام العمل"), T("txtWorkHours", "ساعات العمل في اليوم"),
            L("cboCenter", "مركز التكلفة"), L("cboProject", "المشروع"), L("cboActivity", "النشاط"),
            L("cboLeavePolicy", "رقم لائحة مستحقات الإجازة"), T("txtSocialInsurance", "رقم التأمين الاجتماعي"),
            C("cboPayBasis", "طبيعة عمل الموظف", "شهري", "يومي", "بالساعة"),
            C("cboPolicyValue", "القيمة الافتراضية للبند عند تطبيق اللائحة المالية", "القيمة الافتراضية", "القيمة البديلة"),
            L("cboDistribution", "توزيع البنود الدائمة على الأدلة الفرعية باستخدام"),
            new Field("chkSalaryCalculated", "مؤشر احتساب الراتب", FieldKind.DerivedCheck) }),
        new("البنود الدائمة", Array.Empty<Field>(), new("gridPermanent", "البنود الدائمة", true,
            D("colItem", "اسم البند"), D("colType", "نوع البند"), D("colCalculation", "طريقة احتسابه"))),
        new("الإجازات", Array.Empty<Field>(), new("gridLeave", "الإجازات", true,
            D("colType", "نوع الإجازة"), D("colOpening", "الرصيد الافتتاحي"), D("colEntitled", "الرصيد المستحق"),
            D("colDuration", "مدة الإجازة الممنوحة"), D("colToday", "الرصيد المتبقي حتى اليوم"), D("colYearEnd", "الرصيد المتبقي حتى نهاية السنة"))),
        new("بيانات الاتصال", new[] { T("txtPhone", "رقم التلفون"), T("txtAddress", "العنوان"), E("txtEmail", "البريد الإلكتروني") }),
        new("وثائق الموظف", new[] { L("cboDocumentType", "نوع الوثيقة") }, new("gridDocuments", "تفاصيل الوثيقة", false,
            T("colDocument", "اسم الوثيقة"), T("colIssued", "تاريخ الإصدار"), T("colExpiry", "تاريخ الانتهاء"))),
        new("البيانات العائلية والتأمين", new[] {
            C("cboFather", "حالة الأب", "حي", "متوفى"), C("cboMother", "حالة الأم", "حية", "متوفاة"),
            T("txtSons", "عدد الأبناء الذكور"), T("txtDaughters", "عدد الأبناء الإناث"),
            T("txtInsuranceStart", "تاريخ بداية التأمين"), T("txtInsuranceEnd", "تاريخ نهاية التأمين"),
            T("txtInsuranceAmount", "مبلغ التأمين للفرد"), T("txtInsuranceShare", "نسبة تحمل الموظف") },
            new("gridDependents", "بيانات المعالين", false, L("colRelation", "صلة القرابة"), T("colName", "اسم المعال"),
                T("colBirthDate", "تاريخ الميلاد"), T("colBirthPlace", "مكان الميلاد"), L("colNationality", "الجنسية"),
                B("colInsurance", "التأمين الصحي"), L("colCategory", "الفئة"))),
        new("بيانات الضامنين", Array.Empty<Field>(), new("gridGuarantors", "بيانات الضامنين", false,
            L("colNumber", "رقم الضامن"), D("colName", "اسم الضامن"), B("colPayroll", "مرتبط بكشف راتب"))),
        new("بيانات الكفلاء", Array.Empty<Field>(), new("gridSponsors", "بيانات الكفلاء", false,
            L("colNumber", "رقم الكفيل"), D("colName", "اسم الكفيل"), B("colPayroll", "مرتبط بكشف راتب"))),
        new("بيانات إضافية", Array.Empty<Field>(), Notice: Unspecified),
        new("حسابات البنوك", Array.Empty<Field>(), new("gridBanks", "حسابات البنوك", false,
            L("colNumber", "رقم البنك"), D("colName", "اسم البنك"), L("colType", "نوع الحساب"), T("colAccount", "رقم الحساب"),
            B("colPayroll", "مرتبط بكشف الراتب"), B("colStopped", "موقف"))),
        new("التغير في البنود", Array.Empty<Field>(), new("gridItemChanges", "التغير في البنود", true,
            D("colItem", "البند"), D("colPrevious", "القيمة السابقة"), D("colNew", "القيمة الجديدة"))),
        new("بيانات الأصول", Array.Empty<Field>(), new("gridAssets", "بيانات الأصول", true,
            D("colName", "اسم الأصل"), D("colQuantity", "الكمية"))),
        new("بيانات تذاكر الطيران", new[] {
            B("chkTickets", "يستحق تذاكر سفر"), C("cboTicketMethod", "طريقة احتساب تذاكر السفر", "من تاريخ التعيين", "من تاريخ آخر استلام"),
            T("txtLastTickets", "تاريخ آخر استلام"), T("txtTicketCount", "عدد تذاكر الموظف"), T("txtTicketShare", "نسبة تحمل الموظف"),
            T("txtTicketValue", "إجمالي قيمة التذاكر"), T("txtTicketDays", "إجمالي أيام استحقاق التذاكر") },
            new("gridCompanions", "بيانات المرافقين", false, L("colCompanion", "المرافق"), T("colCount", "عدد التذاكر"),
                T("colValue", "قيمة التذاكر"), T("colShare", "نسبة تحمل الموظف"))),
        new("المهن الوظيفية", new[] { new Field("lstProfessions", "المهن الوظيفية", FieldKind.MultiLookup) }),
        new("بيانات التدرج الوظيفي", Array.Empty<Field>(), Notice: "تظهر من القرارات الإدارية والترقيات بعد ربط الخدمة. " + Unspecified),
        new("الحركات الإدارية", Array.Empty<Field>(), Notice: "تظهر من عمليات الموارد البشرية بعد ربط الخدمة. " + Unspecified),
        new("خبرات ومهارات الموظف", Array.Empty<Field>(), Notice: Unspecified),
        new("التكاليف الإضافية للموظفين", Array.Empty<Field>(), Notice: "تظهر من عمليات التكاليف الإضافية بعد ربط الخدمة. " + Unspecified),
        new("المؤهلات", Array.Empty<Field>(), new("gridQualifications", "المؤهلات", false,
            L("colType", "نوع المؤهل"), L("colSpecialty", "التخصص"), L("colIssuer", "جهة الإصدار"))),
        new("الدورات التدريبية", Array.Empty<Field>(), Notice: Unspecified),
        new("اللغات", new[] { new Field("lstLanguages", "اللغات", FieldKind.MultiLookup) }),
        new("بيانات الحضور والانصراف", new[] {
            B("chkRestPolicy", "تفعيل آلية احتساب أيام الراحة"), T("txtWeekDays", "عدد أيام الأسبوع"),
            T("txtQualifyingDays", "عدد أيام دوام الموظف ليستحق بعدها أيام راحة"),
            C("cboEqualDays", "احتساب أيام الراحة عند تساوي أيام الإجازة مع أيام الغياب", "إجازة", "غياب"),
            L("cboRestLeave", "رقم الإجازة") }),
        new("بيانات أخرى", new[] {
            T("txtHealthNumber", "الرقم التأميني"), L("cboHealthType", "نوع التأمين"), T("txtInsuredChildren", "عدد الأولاد المؤمنين"),
            L("cboMealType", "نوع تغذية الموظف"), C("cboMealPeriod", "نوع سقف التغذية", "يومي", "شهري"), T("txtMealLimit", "سقف التغذية") },
            Notice: "بيانات مكتب العمل: " + Unspecified)
    };
}
