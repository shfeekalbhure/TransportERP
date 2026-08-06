# تقرير مراجعة النسخة المرفقة من TransportERP

**تاريخ المراجعة:** 2026-08-06  
**مصدر المراجعة:** النسخة المضغوطة المرفقة من المستخدم  
**الفرع داخل النسخة:** `setup/initial-solution-structure`  
**رأس الفرع:** `d82467cf3b18933bd1d78dd8bfd0e4f8729be459`

## 1. الحكم التنفيذي

النسخة تحتوي على شاشة الدول الجديدة وملف ربط الخطوط الجزئي، لكن إعداد ملف المشروع يستبعد حاليًا الملف الجزئي `FrmCountries.Typography.cs` من التجميع. لذلك لا يوجد في الحالة الحالية سبب منطقي داخل الملفات المراجعة يجعل `FrmCountries` يرث من `object` بدل `Form`؛ الأخطاء التي ظهرت سابقًا ترتبط غالبًا بحالة محلية أقدم، أو ملفات `obj/bin` قديمة، أو اختلاف بين ملف المشروع المفتوح وما تم تقييمه فعليًا في Visual Studio.

لا يمكن اعتماد البناء على أنه ناجح لأن بيئة المراجعة لا تحتوي على .NET SDK، ولذلك لم يتم تنفيذ `dotnet build`.

## 2. الملفات المتعلقة بشاشة الدول

الموجود داخل المشروع:

- `TransportERP.Desktop/Forms/FrmCountries.cs`
- `TransportERP.Desktop/Forms/FrmCountries.Typography.cs`

ولا توجد داخل النسخة الملفات القديمة التالية:

- `FrmCountries.Designer.cs`
- `FrmCountries.resx`

ملف `FrmCountries.cs` يعرّف الشاشة هكذا:

```csharp
public sealed partial class FrmCountries : Form
```

وهذا تعريف صحيح من ناحية الوراثة ويسمح بوجود أجزاء `partial` أخرى.

ملف `FrmCountries.Typography.cs` يحتوي على:

```csharp
public partial class FrmCountries
{
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        UiTypographyService.Apply(this, "GEN-003");
    }
}
```

هذا الملف صحيح فقط إذا دخل مع الجزء الرئيسي في نفس التجميع، لأن الجزء الرئيسي هو الذي يحدد الوراثة من `Form`.

## 3. مراجعة ملف المشروع

الإعداد الحالي داخل `TransportERP.Desktop.csproj` هو:

```xml
<Compile Remove="Forms\FrmCountries.*.cs" />
<EmbeddedResource Remove="Forms\FrmCountries.*.resx" />
<Compile Update="Forms\FrmCountries.cs" />
```

### النتيجة الفعلية

- النمط `Forms\FrmCountries.*.cs` يستبعد `FrmCountries.Typography.cs` وأي ملف جزئي مشابه.
- النمط لا يستبعد `FrmCountries.cs` نفسه، لأن اسم الملف لا يحتوي على نقطة إضافية بعد `FrmCountries`.
- عنصر `Compile Update="Forms\FrmCountries.cs"` لا يعيد إضافة الملف؛ هو فقط يعدل بيانات عنصر موجود تلقائيًا.
- شاشة الدول الرئيسية تبقى ضمن التجميع.
- ربط `UiTypographyService` لا يدخل ضمن التجميع حاليًا.

### الملاحظة

تعليق ملف المشروع يقول إن الشاشة مبنية بالكامل في `FrmCountries.cs`، لكن وجود `FrmCountries.Typography.cs` يناقض ذلك. يجب اختيار سياسة واحدة واضحة:

1. دمج `OnShown` داخل `FrmCountries.cs` وحذف ملف Typography، وهو الخيار الأنظف لشاشة يراد بناؤها دون ترقيع.
2. أو إبقاء ملف Typography وإزالة الاستبعاد العام، مع استبعاد `Designer` ونسخ النسخ الاحتياطية فقط.

## 4. حالة Git داخل النسخة

النسخة ليست في حالة عمل نظيفة.

ظهر تعديل مرحلي على:

- `TransportERP.Desktop/TransportERP.Desktop.csproj`

والتعديل يزيل فقط:

```xml
<SubType>Form</SubType>
```

كما ظهرت ملفات عربية محذوفة مع ملفات جديدة بأسماء مشوهة الترميز. هذا يبدو أثرًا من طريقة إنشاء أو فك الملف ZIP، وليس تغييرًا وظيفيًا مقصودًا. يمنع عمل `Commit All` من هذه النسخة قبل تنظيف هذه الحالة، لأنه قد يحذف ملفات التوثيق العربية ويضيف نسخًا بأسماء تالفة.

ظهرت كذلك موارد غير متتبعة:

- `TransportERP.Desktop/Forms/FrmDashboard.Governorates.resx`
- `TransportERP.Desktop/Forms/Setup/Geographic/FrmDirectorates.resx`

يجب مراجعتها قبل الإضافة، خصوصًا بسبب تاريخ أخطاء تكرار أسماء الموارد المضمنة.

## 5. مراجعة كود شاشة الدول

### نقاط صحيحة

- الشاشة معرفة كـ `Form` و`partial`.
- الاتجاه `RightToLeft` و`RightToLeftLayout` مفعّلان.
- النصوص الظاهرة للمستخدم عربية.
- الأزرار والحقول تعتمد على CoreUI.
- توجد حقول ISO2 وISO3 ومفتاح الاتصال ورمز العملة والحالة والملاحظات.
- توجد وظائف البحث والتنقل والحفظ والإيقاف والحذف والطباعة.
- توجد دالة `ConfigureForTabHosting` للعرض داخل Dashboard.

### ملاحظات جودة

- ملف `FrmCountries.cs` مكتوب كاملًا تقريبًا في سطر واحد، وهذا لا يمنع التجميع لكنه يضعف القراءة والمراجعة والصيانة ويصعّب تحديد الأخطاء.
- توجد بيانات تجريبية داخل الشاشة بدل خدمة أو DTO.
- لا يوجد تحقق من تكرار كود الدولة أو ISO2 أو ISO3.
- لا توجد قيود طول واضحة لحقول ISO2 وISO3 ورمز العملة.
- زر الإغلاق يستدعي `Close()` حتى عند الاستضافة داخل Dashboard؛ قد يكون الأنسب إغلاق التبويب عبر حدث يرفعه للـDashboard.
- شريط الحالة داخل الشاشة يُخفى عند الاستضافة، وهذا متوافق مع قرار وجود شريط حالة واحد في النافذة الرئيسية.

## 6. تفسير أخطاء OnShown السابقة

الأخطاء:

- `no suitable method found to override`
- `cannot convert FrmCountries to Form`
- `object does not contain OnShown`

تحدث عادة عندما يدخل ملف `FrmCountries.Typography.cs` في التجميع دون الجزء الرئيسي الذي يرث من `Form`، أو عندما تكون هناك نسخة أخرى جزئية من الكلاس ضمن ملفات مولدة أو مؤقتة.

في النسخة المرفقة الحالية، ملف المشروع يستبعد `FrmCountries.Typography.cs`، لذلك هذه الأخطاء لا يفترض أن تصدر من الحالة الحالية بعد تنظيف `bin` و`obj` وإعادة تحميل المشروع.

## 7. الإجراء الموصى به

الأفضل اعتماد ملف واحد لشاشة الدول:

1. نقل دالة `OnShown` إلى `FrmCountries.cs`.
2. حذف `FrmCountries.Typography.cs`.
3. حذف مجموعة الاستبعاد الخاصة بـ`FrmCountries` من ملف المشروع، أو قصرها على `Designer` القديم فقط.
4. إعادة تنسيق `FrmCountries.cs` إلى أسطر منظمة.
5. حذف `bin` و`obj`.
6. إعادة فتح الحل ثم تنفيذ Rebuild.
7. عدم رفع تغييرات أسماء الملفات العربية المشوهة الظاهرة في النسخة المضغوطة.

## 8. حدود التحقق

تمت مراجعة الملفات وحالة Git والإعدادات نصيًا. تعذر تنفيذ البناء لأن أمر `dotnet` غير متوفر في بيئة المراجعة. لذلك لا يحتوي هذا التقرير على ادعاء بأن المشروع يبني بنجاح أو أن عدد الأخطاء صفر.
