# جرد واجهات TransportERP وتقسيم العمل حسب بنية التصميم

نسخة المصدر: `9d02ac8d19e39971ad233108faecc8154a1d5745`، الفرع `feature/receipt-general-ledger-settings`. فحص المصدر ساكن؛ لم يُشغّل WinForms ولم تُعتمد سلامة التصميم أو وظائف الحفظ.

## منهج العد

عُدّت تعريفات أصناف Form وUserControl بما فيها الوراثة غير المباشرة؛ جُمعت ملفات partial والمصمم داخل الدليل نفسه. عدد التعريفات الفيزيائية هو 328، وليس عدد الشاشات المكتملة. استُبعدت أغلفة Form والقوالب والأدوات المشتركة والنوافذ المساعدة من عدد واجهات الأعمال. اختلاف صنفين لشاشة ذات غرض واحد يظل تنفيذين مستقلين ولا يُحسم دمجهما آليًا. مصادر CoreUI والقوالب مستبعدة من تجميع Desktop المباشر لكنها مرتبطة ضمن SharedVisuals، فلا تُعد مفقودة.

| الفئة | عدد تعريفات الأصناف |
|---|---:|
| أصناف أساس | 4 |
| واجهات أعمال ذات عناصر معلنة؛ وجودها لا يعني اكتمالها | 163 |
| أغلفة Form تحوي واجهة أو تقبلها من المستدعي | 119 |
| نوافذ حوار واختيار نطاق وإعدادات مساعدة | 6 |
| واجهات أعمال بلا حقول؛ تحتوي تذييل تدقيق مشترك فقط | 9 |
| واجهات اختيار واستعلام مساعدة | 7 |
| أدوات مشتركة/أداة صف | 7 |
| الرئيسية ولوحة المعلومات والدخول وتغيير كلمة المرور | 5 |
| قوالب | 6 |
| نموذج تحقق | 1 |
| مضيف تحقق معزول | 1 |

واجهات الأعمال غير الفارغة: **163**. واجهات الأعمال الفارغة: **9**. مجموع أصناف محتوى الأعمال 172؛ يشمل تنفيذات قديمة أو بديلة غير مستهدفة بالمصنع النشط.

مصنع المحتوى النشط يعرّف 138 حالات رمز، تقابل 136 أصناف محتوى مميزة. تعدد رموز المستخدمين/الأدوار/الصلاحيات لا يساوي ثلاث شاشات مستقلة. الوصول إلى رمز لا يثبت أن نافذته قابلة للاستخدام.

## مجموعات التصميم المرصودة والتكليف المقترح

| الفريق المقترح | البنية المرصودة | واجهات أعمال |
|---|---|---:|
| UI-01-FIELDS | FIELDS_SETTINGS | 19 |
| UI-02-MASTER-GRID | MASTER_GRID | 45 |
| UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 6 |
| UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 10 |
| UI-03-TABBED-FIELDS | TABBED_FIELDS | 23 |
| UI-04-TABBED-GRID | TABBED_GRID | 55 |
| UI-05-TREE-DETAIL | TREE_DETAIL | 5 |

التقسيم مبني على وجود TreeView وTabPage وDataGridView فعليًا في التعريفات؛ مجموعة TABBED_GRID واسعة وتحتاج تقسيمًا فرعيًا بعد مقارنة ترتيب الحاويات وأعمدة السندات والمخزون. أسماء فرق الجدول تكليف مقترح للتنفيذ اللاحق وليست فرقًا جديدة شُغّلت في هذا الجرد. لكل فريق مراجعة الحاويات ثم الحقول ثم الأعمدة والأزرار ثم الأحداث ثم الوصول API؛ يجب إبقاء التصميم الحالي وإثبات أي نقص بمرجع مستقل.

## الواجهات الفارغة بأدلة

- `UcOnyxSCREEN0092` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0092.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0092.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0104` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0104.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0104.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0105` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0105.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0105.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcOnyxSCREEN0106` — TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0106.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0106.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_01_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_01_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_01_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_02_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_02_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_02_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_03_03_01` — TransportERP.EmptyForms/Forms/FrmScreen_03_03_01.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_03_03_01.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_04_11_06` — TransportERP.EmptyForms/Forms/FrmScreen_04_11_06.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_04_11_06.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.
- `UcScreen_04_11_07` — TransportERP.EmptyForms/Forms/FrmScreen_04_11_07.cs:4؛ المصمم `TransportERP.EmptyForms/Forms/FrmScreen_04_11_07.Designer.cs` يضيف standardAuditMetadata فقط، بلا حقول أو جدول أعمال.

## قيود الجرد

عدد العناصر في screen-inventory.csv هو عدد الأسماء المرصودة في تعريفات المصدر، وقد يشمل متغيرات مساعدة داخل الدوال. control-inventory.csv أضيق: العناصر والأعمدة المعلنة في ملفات Designer، مع القيم الحرفية والأحداث المباشرة في المصمم والملفات المكملة. الإنشاء المتأخر والمصانع والأوامر المفوضة وAddRange وتغيير الخصائص في التشغيل تحتاج متابعة. عدم وجود Click مباشر لا يثبت أن الزر غير مربوط. لون الحقل مسجل كما هو دون افتراض أنه إلزامي. تعليق الملخص أو عنوان Label موسوم باعتباره مصدرًا احتياطيًا؛ لا يثبت عنوان النافذة وقت التشغيل.

## جميع واجهات الأعمال وتقسيمها

| الصنف | الفريق | بنية التصميم | تبويبات معلنة | عناصر مسماة | مصنع المحتوى النشط | دليل المصدر |
|---|---|---|---:|---:|---|---|
| UcPrintSettings | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 63 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/FrmPrintSettings.cs:5 |
| UcCityData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 36 | 02.02.07 | TransportERP.Desktop/Forms/SystemSettings/General/UcCityData.cs:6 |
| UcCountryData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 48 | 02.02.04 | TransportERP.Desktop/Forms/SystemSettings/General/UcCountryData.cs:7 |
| UcDefaultAccountingCharts | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 38 | INPUT:DEFAULT-CHARTS | TransportERP.Desktop/Forms/SystemSettings/General/UcDefaultAccountingCharts.cs:6 |
| UcGeneralVariables | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 27 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralVariables.cs:7 |
| UcGovernorateData | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 36 | 02.02.05 | TransportERP.Desktop/Forms/SystemSettings/General/UcGovernorateData.cs:6 |
| UcInternationalRegions | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 31 | SETUP:INTERNATIONAL-REGIONS | TransportERP.Desktop/Forms/SystemSettings/General/UcInternationalRegions.cs:6 |
| UcOtherAccountGroups | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 30 | INPUT:OTHER-ACCOUNT-GROUPS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAccountGroups.cs:6 |
| UcOtherAccountLinks | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 34 | INPUT:OTHER-ACCOUNT-LINKS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAccountLinks.cs:6 |
| UcOtherAnalyticalAccounts | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 40 | INPUT:OTHER-ANALYTICAL-ACCOUNTS | TransportERP.Desktop/Forms/SystemSettings/General/UcOtherAnalyticalAccounts.cs:6 |
| UcShipmentBooks | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 61 | SHIP:011 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentBooks.cs:11 |
| UcShipmentContentTypes | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentContentTypes.cs:11 |
| UcShipmentUnits | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 53 | SHIP:003 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentUnits.cs:11 |
| UcAccountLinking | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 77 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/ربط_الحسابات/UcAccountLinking.cs:11 |
| UcOnyxSCREEN0079 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 30 | ONYX:SCREEN-0079 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0079.cs:4 |
| UcOnyxSCREEN0084 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 41 | ONYX:SCREEN-0084 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0084.cs:4 |
| UcOnyxSCREEN0085 | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 34 | ONYX:SCREEN-0085 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0085.cs:4 |
| UcAccountOpeningRequest | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 51 | ONYX:SCREEN-0086 | TransportERP.EmptyForms/Forms/UcAccountOpeningRequest.cs:6 |
| UcCustomerDebtReport | UI-01-FIELDS | FIELDS_SETTINGS | 0 | 84 | CUS:DEBT-REPORT | TransportERP.EmptyForms/Forms/UcCustomerDebtReport.cs:5 |
| UcActivitySetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:ACTIVITIES | TransportERP.Desktop/Forms/SystemSettings/General/UcActivitySetup.cs:5 |
| UcAdditionalFieldSetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:ADDITIONAL-FIELDS | TransportERP.Desktop/Forms/SystemSettings/General/UcAdditionalFieldSetup.cs:6 |
| UcAdministrativeStructureTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:STRUCTURE-TYPES | TransportERP.Desktop/Forms/SystemSettings/General/UcAdministrativeStructureTypes.cs:5 |
| UcApprovalLevels | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:APPROVAL-LEVELS | TransportERP.Desktop/Forms/SystemSettings/General/UcApprovalLevels.cs:8 |
| UcAreaData | UI-02-MASTER-GRID | MASTER_GRID | 0 | 34 | 02.02.08 | TransportERP.Desktop/Forms/SystemSettings/General/UcAreaData.cs:6 |
| UcBranchGroups | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:BRANCH-GROUPS | TransportERP.Desktop/Forms/SystemSettings/General/UcBranchGroups.cs:6 |
| UcEmployeeGeneralCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 26 | SETUP:EMPLOYEE-CODES | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeGeneralCodes.cs:6 |
| UcEmployeeProfessionLinks | UI-02-MASTER-GRID | MASTER_GRID | 0 | 29 | INPUT:EMPLOYEE-PROFESSIONS | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeProfessionLinks.cs:6 |
| UcExchangeRates | UI-02-MASTER-GRID | MASTER_GRID | 0 | 80 | 02.04.03 | TransportERP.Desktop/Forms/SystemSettings/General/UcExchangeRates.cs:8 |
| UcGeneralCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 28 | SETUP:GENERAL-CODES | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralCodes.cs:7 |
| UcProjectExtraFieldCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:PROJECT-EXTRA-FIELDS | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectExtraFieldCodes.cs:6 |
| UcProjectSetup | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:PROJECTS | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectSetup.cs:5 |
| UcScreenBackgrounds | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:SCREEN-BACKGROUNDS | TransportERP.Desktop/Forms/SystemSettings/General/UcScreenBackgrounds.cs:6 |
| UcSystemPeriods | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | 04.11.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcSystemPeriods.cs:7 |
| UcTaxBracketCodes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 23 | SETUP:TAX-BRACKETS | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxBracketCodes.cs:6 |
| UcTaxTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | SETUP:TAX-TYPES | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxTypes.cs:7 |
| UcTextTranslation | UI-02-MASTER-GRID | MASTER_GRID | 0 | 25 | SETUP:TEXT-TRANSLATION | TransportERP.Desktop/Forms/SystemSettings/General/UcTextTranslation.cs:7 |
| UcShipmentCategories | UI-02-MASTER-GRID | MASTER_GRID | 0 | 59 | SHIP:002 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentCategories.cs:8 |
| dgvShipmentPriorities | UI-02-MASTER-GRID | MASTER_GRID | 0 | 48 | SHIP:004 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentPriorities.cs:11 |
| UcShipmentTypes | UI-02-MASTER-GRID | MASTER_GRID | 0 | 45 | SHIP:001 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentTypes.cs:11 |
| UcOnyxSCREEN0080 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 48 | ONYX:SCREEN-0080 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0080.cs:5 |
| UcOnyxSCREEN0089 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 36 | ONYX:SCREEN-0089 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0089.cs:4 |
| UcOnyxSCREEN0090 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 27 | ONYX:SCREEN-0090 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0090.cs:4 |
| UcOnyxSCREEN0091 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 66 | ONYX:SCREEN-0091 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0091.cs:4 |
| UcOnyxSCREEN0102 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 52 | ONYX:SCREEN-0102 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0102.cs:6 |
| UcScreen_02_02_04 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 61 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_04.cs:5 |
| UcScreen_02_02_05 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_05.cs:5 |
| UcScreen_02_02_06 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | 02.02.06 | TransportERP.EmptyForms/Forms/FrmScreen_02_02_06.cs:5 |
| UcScreen_02_02_07 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_07.cs:5 |
| UcScreen_02_02_08 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_02_02_08.cs:5 |
| UcScreen_02_04_06 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 54 | 02.04.06 | TransportERP.EmptyForms/Forms/FrmScreen_02_04_06.cs:4 |
| UcScreen_02_05_21 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 53 | 02.05.21 | TransportERP.EmptyForms/Forms/FrmScreen_02_05_21.cs:5 |
| UcScreen_04_03_04 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 73 | 04.03.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_04.cs:4 |
| UcScreen_04_11_01 | UI-02-MASTER-GRID | MASTER_GRID | 0 | 59 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_04_11_01.cs:4 |
| UcAccountCostCenterLinking | UI-02-MASTER-GRID | MASTER_GRID | 0 | 46 | ONYX:SCREEN-0093 | TransportERP.EmptyForms/Forms/UcAccountCostCenterLinking.cs:6 |
| UcAccountProjectLinking | UI-02-MASTER-GRID | MASTER_GRID | 0 | 46 | ONYX:SCREEN-0094 | TransportERP.EmptyForms/Forms/UcAccountProjectLinking.cs:6 |
| UcCustomerInstallmentSettlement | UI-02-MASTER-GRID | MASTER_GRID | 0 | 41 | CUS:INSTALLMENT-SETTLEMENT | TransportERP.EmptyForms/Forms/UcCustomerInstallmentSettlement.cs:6 |
| UcForeignPurchaseCosting | UI-02-MASTER-GRID | MASTER_GRID | 0 | 29 | PUR:FOREIGN-COSTING | TransportERP.EmptyForms/Forms/UcForeignPurchaseCosting.cs:6 |
| UcForeignPurchaseReceipt | UI-02-MASTER-GRID | MASTER_GRID | 0 | 32 | PUR:FOREIGN-RECEIPT | TransportERP.EmptyForms/Forms/UcForeignPurchaseReceipt.cs:6 |
| UcInventoryCustodyIssue | UI-02-MASTER-GRID | MASTER_GRID | 0 | 37 | INV:CUSTODY-ISSUE | TransportERP.EmptyForms/Forms/UcInventoryCustodyIssue.cs:4 |
| UcInventoryCustodyReceipt | UI-02-MASTER-GRID | MASTER_GRID | 0 | 38 | INV:CUSTODY-RECEIPT | TransportERP.EmptyForms/Forms/UcInventoryCustodyReceipt.cs:4 |
| UcInventoryDamagedIssueOrder | UI-02-MASTER-GRID | MASTER_GRID | 0 | 26 | INV:DAMAGED-ISSUE | TransportERP.EmptyForms/Forms/UcInventoryDamagedIssueOrder.cs:4 |
| UcInventoryExternalRepairOrder | UI-02-MASTER-GRID | MASTER_GRID | 0 | 18 | INV:EXTERNAL-REPAIR | TransportERP.EmptyForms/Forms/UcInventoryExternalRepairOrder.cs:4 |
| UcInventoryQuantityReservation | UI-02-MASTER-GRID | MASTER_GRID | 0 | 12 | INV:QUANTITY-RESERVATION | TransportERP.EmptyForms/Forms/UcInventoryQuantityReservation.cs:4 |
| UcPurchaseRequest | UI-02-MASTER-GRID | MASTER_GRID | 0 | 45 | PUR:REQUEST | TransportERP.EmptyForms/Forms/UcPurchaseRequest.cs:6 |
| UcItemRelease | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 38 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcItemRelease.cs:6 |
| UcManifestHandover | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifestHandover.cs:6 |
| UcTripDeparture | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTripDeparture.cs:6 |
| UcWaybillApproval | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 26 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillApproval.cs:5 |
| UcWaybillFinancialStatus | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillFinancialStatus.cs:5 |
| UcWaybillPricing | UI-07-SHIPPING-FIELDS | SHIPPING_ACTION_FIELDS | 0 | 28 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillPricing.cs:5 |
| UcLoadPlanning | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 28 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcLoadPlanning.cs:6 |
| UcManifest | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 38 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifest.cs:6 |
| UcManifestLoading | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcManifestLoading.cs:6 |
| UcReadyToLoadWaybills | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 24 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcReadyToLoadWaybills.cs:6 |
| UcRemainingShippingQuantity | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcRemainingShippingQuantity.cs:6 |
| UcTrip | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 39 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTrip.cs:6 |
| UcTripAllocation | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 30 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcTripAllocation.cs:6 |
| UcWaybillCollections | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 22 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillCollections.cs:5 |
| UcWaybillDraft | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 4 | 53 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillDraft.cs:5 |
| UcWaybillPaymentPlan | UI-06-SHIPPING-GRID | SHIPPING_ACTION_GRID | 0 | 23 | لم يرصد مباشرة | TransportERP.Desktop/Waybills/UcWaybillPaymentPlan.cs:5 |
| UcActivityData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 45 | INPUT:ACTIVITY-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcActivityData.cs:7 |
| UcBranchData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 6 | 146 | 02.02.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcBranchData.cs:8 |
| UcCompanyData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 59 | 02.01.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyData.cs:4 |
| UcCompanyManagement | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 82 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcCompanyManagement.cs:8 |
| UcCostCenterData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 62 | 04.02.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcCostCenterData.cs:6 |
| UcGeneralVariablesSetup | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 137 | 02.04.07 | TransportERP.Desktop/Forms/SystemSettings/General/UcGeneralVariablesSetup.cs:4 |
| UcIntermediateAccounts | UI-03-TABBED-FIELDS | TABBED_FIELDS | 9 | 216 | INPUT:INTERMEDIATE-ACCOUNTS | TransportERP.Desktop/Forms/SystemSettings/General/UcIntermediateAccounts.cs:5 |
| UcProjectData | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 89 | INPUT:PROJECT-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcProjectData.cs:6 |
| UcBranchManagement | UI-03-TABBED-FIELDS | TABBED_FIELDS | 6 | 95 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/الفروع/UcBranchManagement.cs:6 |
| UcPackagingTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 58 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcPackagingTypes.cs:11 |
| UcShipmentPricingRules | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 70 | SHIP:009 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentPricingRules.cs:11 |
| dgvShipmentServiceFees | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 79 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentServiceFees.cs:11 |
| UcShipmentStatuses | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 63 | SHIP:010 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentStatuses.cs:11 |
| UcShipmentTransportMethods | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 48 | SHIP:007 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcShipmentTransportMethods.cs:11 |
| UcTransportGroups | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 44 | SHIP:006 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcTransportGroups.cs:11 |
| UcTransportTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 43 | SHIP:005 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcTransportTypes.cs:11 |
| UcDriverTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 57 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/الشاشات المشتركة/UcDriverTypes.cs:11 |
| UcShipmentDispatch | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 177 | SHIP:013 | TransportERP.Desktop/البوالص_والشحن/العمليات/الشحن_والترحيل/UcShipmentDispatch.cs:7 |
| UcWarehouseReceiptOrder | UI-03-TABBED-FIELDS | TABBED_FIELDS | 2 | 52 | SHIP:014 | TransportERP.Desktop/البوالص_والشحن/العمليات/توريد_مخزني/UcWarehouseReceiptOrder.cs:11 |
| UcScreen_04_11_02 | UI-03-TABBED-FIELDS | TABBED_FIELDS | 5 | 59 | 04.11.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_02.cs:3 |
| UcScreen_04_11_05 | UI-03-TABBED-FIELDS | TABBED_FIELDS | 4 | 60 | 04.11.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_05.cs:3 |
| UcAccountGroupsAndTypes | UI-03-TABBED-FIELDS | TABBED_FIELDS | 3 | 55 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/UcAccountGroupsAndTypes.cs:6 |
| UcInventoryStocktakeReport | UI-03-TABBED-FIELDS | TABBED_FIELDS | 1 | 86 | INV:STOCKTAKE-REPORT | TransportERP.EmptyForms/Forms/UcInventoryStocktakeReport.cs:5 |
| UcDocumentNumbering | UI-04-TABBED-GRID | TABBED_GRID | 4 | 99 | 02.04.01 | TransportERP.Desktop/Forms/SystemSettings/DocumentControl/UcDocumentNumbering.cs:9 |
| UcGeneralSettings | UI-04-TABBED-GRID | TABBED_GRID | 8 | 102 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/FrmGeneralSettings.cs:5 |
| UcCashFlowAccountLinks | UI-04-TABBED-GRID | TABBED_GRID | 2 | 29 | INPUT:CASHFLOW-ACCOUNT-LINKS | TransportERP.Desktop/Forms/SystemSettings/General/UcCashFlowAccountLinks.cs:6 |
| UcChartSetup | UI-04-TABBED-GRID | TABBED_GRID | 4 | 38 | 04.01.02 | TransportERP.Desktop/Forms/SystemSettings/General/UcChartSetup.cs:6 |
| UcCostCenterSetup | UI-04-TABBED-GRID | TABBED_GRID | 2 | 30 | SETUP:COST-CENTERS | TransportERP.Desktop/Forms/SystemSettings/General/UcCostCenterSetup.cs:6 |
| UcCurrencyManagement | UI-04-TABBED-GRID | TABBED_GRID | 3 | 65 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcCurrencyManagement.cs:7 |
| UcCurrencySetup | UI-04-TABBED-GRID | TABBED_GRID | 3 | 124 | 02.04.02 | TransportERP.Desktop/Forms/SystemSettings/General/UcCurrencySetup.cs:10 |
| UcElectronicInvoiceSequences | UI-04-TABBED-GRID | TABBED_GRID | 1 | 37 | SETUP:EINVOICE-SEQUENCES | TransportERP.Desktop/Forms/SystemSettings/General/UcElectronicInvoiceSequences.cs:6 |
| UcEmployeeData | UI-04-TABBED-GRID | TABBED_GRID | 26 | 314 | INPUT:EMPLOYEE-DATA | TransportERP.Desktop/Forms/SystemSettings/General/UcEmployeeData.cs:4 |
| UcTaxCalculationMethods | UI-04-TABBED-GRID | TABBED_GRID | 2 | 47 | SETUP:TAX-METHODS | TransportERP.Desktop/Forms/SystemSettings/General/UcTaxCalculationMethods.cs:7 |
| UcTextSetup | UI-04-TABBED-GRID | TABBED_GRID | 1 | 29 | لم يرصد مباشرة | TransportERP.Desktop/Forms/SystemSettings/General/UcTextSetup.cs:6 |
| UcRoutes | UI-04-TABBED-GRID | TABBED_GRID | 2 | 63 | SHIP:008 | TransportERP.Desktop/البوالص_والشحن/الاعدادات/UcRoutes.cs:11 |
| UcShipmentWaybill | UI-04-TABBED-GRID | TABBED_GRID | 13 | 250 | SHIP:012 | TransportERP.Desktop/البوالص_والشحن/العمليات/البوليصه/UcShipmentWaybill.cs:11 |
| UcShipmentTransfer | UI-04-TABBED-GRID | TABBED_GRID | 4 | 69 | لم يرصد مباشرة | TransportERP.Desktop/البوالص_والشحن/العمليات/ترحيل_الشحنات/UserControl1.cs:11 |
| UcOnyxSCREEN0081 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 42 | ONYX:SCREEN-0081 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0081.cs:4 |
| UcOnyxSCREEN0082 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 47 | ONYX:SCREEN-0082 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0082.cs:4 |
| UcOnyxSCREEN0083 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 55 | ONYX:SCREEN-0083 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0083.cs:4 |
| UcOnyxSCREEN0098 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 125 | ONYX:SCREEN-0098 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0098.cs:3 |
| UcOnyxSCREEN0100 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 124 | ONYX:SCREEN-0100 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0100.cs:3 |
| UcOnyxSCREEN0103 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 35 | ONYX:SCREEN-0103 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0103.cs:3 |
| UcOnyxSCREEN0110 | UI-04-TABBED-GRID | TABBED_GRID | 1 | 51 | ONYX:SCREEN-0110 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0110.cs:5 |
| UcOnyxSCREEN0111 | UI-04-TABBED-GRID | TABBED_GRID | 1 | 51 | ONYX:SCREEN-0111 | TransportERP.EmptyForms/Forms/FrmOnyxSCREEN0111.cs:5 |
| UcScreen_04_03_01 | UI-04-TABBED-GRID | TABBED_GRID | 6 | 83 | 04.03.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_01.cs:5 |
| UcScreen_04_03_02 | UI-04-TABBED-GRID | TABBED_GRID | 4 | 97 | 04.03.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_02.cs:3 |
| UcScreen_04_03_03 | UI-04-TABBED-GRID | TABBED_GRID | 6 | 88 | 04.03.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_03_03.cs:4 |
| UcScreen_04_04_01 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 160 | 04.04.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_01.cs:3 |
| UcScreen_04_04_02 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 117 | 04.04.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_02.cs:4 |
| UcScreen_04_04_03 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_03.cs:3 |
| UcScreen_04_04_04 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_04.cs:3 |
| UcScreen_04_04_05 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_05.cs:3 |
| UcScreen_04_04_06 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 66 | 04.04.06 | TransportERP.EmptyForms/Forms/FrmScreen_04_04_06.cs:3 |
| UcScreen_04_05_01 | UI-04-TABBED-GRID | TABBED_GRID | 10 | 137 | 04.05.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_05_01.cs:3 |
| UcScreen_04_07_16 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 101 | 04.07.16 | TransportERP.EmptyForms/Forms/FrmScreen_04_07_16.cs:4 |
| UcScreen_04_08_01 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 87 | 04.08.01 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_01.cs:3 |
| UcScreen_04_08_02 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 87 | 04.08.02 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_02.cs:3 |
| UcScreen_04_08_05 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 100 | 04.08.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_08_05.cs:3 |
| UcScreen_04_09_06 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 80 | 04.09.06 | TransportERP.EmptyForms/Forms/FrmScreen_04_09_06.cs:3 |
| UcScreen_04_10_03 | UI-04-TABBED-GRID | TABBED_GRID | 3 | 58 | 04.10.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_03.cs:3 |
| UcScreen_04_10_04 | UI-04-TABBED-GRID | TABBED_GRID | 3 | 56 | 04.10.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_04.cs:3 |
| UcScreen_04_10_05 | UI-04-TABBED-GRID | TABBED_GRID | 2 | 44 | 04.10.05 | TransportERP.EmptyForms/Forms/FrmScreen_04_10_05.cs:3 |
| UcScreen_04_11_03 | UI-04-TABBED-GRID | TABBED_GRID | 5 | 58 | 04.11.03 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_03.cs:3 |
| UcScreen_04_11_04 | UI-04-TABBED-GRID | TABBED_GRID | 7 | 64 | 04.11.04 | TransportERP.EmptyForms/Forms/FrmScreen_04_11_04.cs:3 |
| UcGeneralLedgerSettings | UI-04-TABBED-GRID | TABBED_GRID | 5 | 49 | ONYX:SCREEN-0078 | TransportERP.EmptyForms/Forms/UcGeneralLedgerSettings.cs:10 |
| UcInventoryIssueOrder | UI-04-TABBED-GRID | TABBED_GRID | 2 | 76 | INV:ISSUE-ORDER | TransportERP.EmptyForms/Forms/UcInventoryIssueOrder.cs:6 |
| UcInventoryManualStocktake | UI-04-TABBED-GRID | TABBED_GRID | 1 | 27 | INV:MANUAL-STOCKTAKE | TransportERP.EmptyForms/Forms/UcInventoryManualStocktake.cs:6 |
| UcInventoryMaterialRequest | UI-04-TABBED-GRID | TABBED_GRID | 2 | 57 | INV:MATERIAL-REQUEST | TransportERP.EmptyForms/Forms/UcInventoryMaterialRequest.cs:6 |
| UcInventoryReceiptAuthorization | UI-04-TABBED-GRID | TABBED_GRID | 1 | 37 | INV:RECEIPT-AUTHORIZATION | TransportERP.EmptyForms/Forms/UcInventoryReceiptAuthorization.cs:6 |
| UcInventoryReceiptOrder | UI-04-TABBED-GRID | TABBED_GRID | 2 | 68 | INV:RECEIPT-ORDER | TransportERP.EmptyForms/Forms/UcInventoryReceiptOrder.cs:6 |
| UcInventorySettlement | UI-04-TABBED-GRID | TABBED_GRID | 2 | 84 | INV:SETTLEMENT | TransportERP.EmptyForms/Forms/UcInventorySettlement.cs:6 |
| UcInventoryTransfer | UI-04-TABBED-GRID | TABBED_GRID | 2 | 70 | INV:TRANSFER | TransportERP.EmptyForms/Forms/UcInventoryTransfer.cs:6 |
| UcInventoryTransferReceipt | UI-04-TABBED-GRID | TABBED_GRID | 1 | 38 | INV:TRANSFER-RECEIPT | TransportERP.EmptyForms/Forms/UcInventoryTransferReceipt.cs:6 |
| UcPurchaseAdditionalDiscount | UI-04-TABBED-GRID | TABBED_GRID | 1 | 66 | SUP:ADDITIONAL-PURCHASE-DISCOUNT | TransportERP.EmptyForms/Forms/UcPurchaseAdditionalDiscount.cs:6 |
| UcPurchaseComparisonNomination | UI-04-TABBED-GRID | TABBED_GRID | 1 | 39 | PUR:COMPARISON-NOMINATION | TransportERP.EmptyForms/Forms/UcPurchaseComparisonNomination.cs:6 |
| UcPurchaseTechnicalComparison | UI-04-TABBED-GRID | TABBED_GRID | 1 | 32 | PUR:TECHNICAL-COMPARISON | TransportERP.EmptyForms/Forms/UcPurchaseTechnicalComparison.cs:6 |
| UcRepresentativeCommissions | UI-04-TABBED-GRID | TABBED_GRID | 2 | 48 | CUS:REPRESENTATIVE-COMMISSIONS | TransportERP.EmptyForms/Forms/UcRepresentativeCommissions.cs:5 |
| UcUsersPermissions | UI-05-TREE-DETAIL | TREE_DETAIL | 5 | 129 | 02.03.01;02.03.02;02.03.03 | TransportERP.Desktop/Forms/Setup/Security/FrmUsersPermissions.cs:7 |
| UcAccountingChartData | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 105 | 04.01.01 | TransportERP.Desktop/Forms/SystemSettings/General/UcAccountingChartData.cs:5 |
| UcAdministrativeStructure | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 42 | INPUT:ADMINISTRATIVE-STRUCTURE | TransportERP.Desktop/Forms/SystemSettings/General/UcAdministrativeStructure.cs:6 |
| UcScreen_04_02_01 | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 67 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/FrmScreen_04_02_01.cs:6 |
| UcChartOfAccounts | UI-05-TREE-DETAIL | TREE_DETAIL | 0 | 73 | لم يرصد مباشرة | TransportERP.EmptyForms/Forms/UcChartOfAccounts.cs:7 |

## محتوى المصنع النشط مقابل التنفيذات الأخرى

مصنع المحتوى النشط يستهدف 126 واجهة أعمال غير فارغة و9 واجهات أعمال فارغة وواجهة تغيير كلمة المرور؛ يعرّف 138 حالة رمز بسبب رموز المستخدمين والأدوار والصلاحيات التي تفتح الصنف نفسه. عدد الرموز أو تعريفات الأصناف لا يعبر عن اكتمال العمل.

| بنية محتوى المصنع النشط | عدد أصناف الأعمال غير الفارغة |
|---|---:|
| FIELDS_SETTINGS | 15 |
| MASTER_GRID | 40 |
| TABBED_FIELDS | 17 |
| TABBED_GRID | 51 |
| TREE_DETAIL | 3 |

يوجد 37 تنفيذ محتوى أعمال غير مستهدف مباشرة بالمصنع النشط. بعضه يُستخدم داخل واجهة أخرى أو يحتفظ به المضيف القديم، لذلك لا يجوز اعتباره ميتًا أو حذفه دون تتبع كل المراجع:

- `UcPrintSettings` — FrmPrintSettings.cs:5.
- `UcGeneralVariables` — UcGeneralVariables.cs:7.
- `UcShipmentContentTypes` — UcShipmentContentTypes.cs:11.
- `UcAccountLinking` — UcAccountLinking.cs:11.
- `UcScreen_02_02_04` — FrmScreen_02_02_04.cs:5.
- `UcScreen_02_02_05` — FrmScreen_02_02_05.cs:5.
- `UcScreen_02_02_07` — FrmScreen_02_02_07.cs:5.
- `UcScreen_02_02_08` — FrmScreen_02_02_08.cs:5.
- `UcScreen_04_11_01` — FrmScreen_04_11_01.cs:4.
- `UcItemRelease` — UcItemRelease.cs:6.
- `UcManifestHandover` — UcManifestHandover.cs:6.
- `UcTripDeparture` — UcTripDeparture.cs:6.
- `UcWaybillApproval` — UcWaybillApproval.cs:5.
- `UcWaybillFinancialStatus` — UcWaybillFinancialStatus.cs:5.
- `UcWaybillPricing` — UcWaybillPricing.cs:5.
- `UcLoadPlanning` — UcLoadPlanning.cs:6.
- `UcManifest` — UcManifest.cs:6.
- `UcManifestLoading` — UcManifestLoading.cs:6.
- `UcReadyToLoadWaybills` — UcReadyToLoadWaybills.cs:6.
- `UcRemainingShippingQuantity` — UcRemainingShippingQuantity.cs:6.
- `UcTrip` — UcTrip.cs:6.
- `UcTripAllocation` — UcTripAllocation.cs:6.
- `UcWaybillCollections` — UcWaybillCollections.cs:5.
- `UcWaybillDraft` — UcWaybillDraft.cs:5.
- `UcWaybillPaymentPlan` — UcWaybillPaymentPlan.cs:5.
- `UcCompanyManagement` — UcCompanyManagement.cs:8.
- `UcBranchManagement` — UcBranchManagement.cs:6.
- `UcPackagingTypes` — UcPackagingTypes.cs:11.
- `dgvShipmentServiceFees` — UcShipmentServiceFees.cs:11.
- `UcDriverTypes` — UcDriverTypes.cs:11.
- `UcAccountGroupsAndTypes` — UcAccountGroupsAndTypes.cs:6.
- `UcGeneralSettings` — FrmGeneralSettings.cs:5.
- `UcCurrencyManagement` — UcCurrencyManagement.cs:7.
- `UcTextSetup` — UcTextSetup.cs:6.
- `UcShipmentTransfer` — UserControl1.cs:11.
- `UcScreen_04_02_01` — FrmScreen_04_02_01.cs:6.
- `UcChartOfAccounts` — UcChartOfAccounts.cs:7.

## تفاصيل عناصر المصمم

تم توثيق 11,750 اسم عنصر/عمود/مكون في 214 ملف مصمم يحتوي عناصر، منها 10,979 اسمًا تابعًا لواجهات الأعمال 163. سجل control-inventory.csv يتضمن النص والمقاس والموقع والرسو واللون والظهور والتفعيل وقراءة فقط والخصائص الحرفية وأدلة ملفات المصدر؛ رُصدت أحداث مباشرة لـ519 عنصرًا. لا تساوي هذه الأرقام اختبارات وظائف أو عدد عناصر ظاهرة للمستخدم.

الواجهات التسع المصنفة فارغة تجاريًا تضم تذييل تدقيق فقط؛ لذا فحص Controls.Count>0 قد يعطي إشارة مضللة عن جاهزيتها. يجب قياس وجود حقول العمل ومسار الحفظ.
