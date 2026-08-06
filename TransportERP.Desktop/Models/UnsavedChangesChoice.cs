namespace TransportERP.Desktop.Models;

/// <summary>
/// يحدد قرار المستخدم عند محاولة مغادرة شاشة تحتوي على تعديلات غير محفوظة.
/// </summary>
public enum UnsavedChangesChoice
{
    /// <summary>
    /// إلغاء عملية المغادرة والعودة إلى الشاشة لاستكمال العمل.
    /// </summary>
    Cancel = 0,

    /// <summary>
    /// حفظ التعديلات ثم متابعة العملية المطلوبة.
    /// </summary>
    Save = 1,

    /// <summary>
    /// تجاهل التعديلات غير المحفوظة ومتابعة العملية المطلوبة.
    /// </summary>
    Discard = 2,

    /// <summary>
    /// اسم بديل صريح لخيار الحفظ والمتابعة.
    /// </summary>
    SaveAndContinue = Save,

    /// <summary>
    /// اسم بديل صريح لخيار التجاهل والمتابعة.
    /// </summary>
    DiscardAndContinue = Discard,

    /// <summary>
    /// اسم بديل صريح للعودة إلى التحرير دون مغادرة الشاشة.
    /// </summary>
    KeepEditing = Cancel
}
