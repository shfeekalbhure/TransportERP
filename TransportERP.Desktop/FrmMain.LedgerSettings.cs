using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TransportERP.Contracts.Accounting;
using TransportERP.EmptyForms;

namespace TransportERP.Desktop;

public partial class FrmMain
{
    private void ConnectLedgerSettingsControl(UserControl? control)
    {
        if (control is UcScreen_04_05_01 journal)
        {
            if (AuthenticatedSession != null)
            {
                var scopedSession = AuthenticatedSession.CreateReceiptSessionCopy();
                journal.Disposed += (_, _) => scopedSession.Dispose();
                journal.ConnectDescriptionSettings(() => LedgerSettingsRequest(HttpMethod.Get,
                    send: request => scopedSession.SendAsync(request)));
            }
            return;
        }
        if (control is not UcGeneralLedgerSettings settings || AuthenticatedSession == null) return;
        bool initialized = false;
        settings.Load += async (_, _) =>
        {
            if (initialized) return;
            initialized = true;
            try
            {
                var document = await LedgerSettingsRequest(HttpMethod.Get);
                if (settings.IsDisposed) return;
                settings.ConnectLedgerSettings(document,
                    update => LedgerSettingsRequest(HttpMethod.Put, update),
                    () => LedgerSettingsRequest(HttpMethod.Get));
                settings.ConnectReceiptSettings(async () =>
                {
                    var current = await ReceiptRequest<ReceiptBootstrap>(HttpMethod.Get, "bootstrap");
                    if (!current.Actions.Contains("accounting.receipts.configure"))
                        throw new InvalidOperationException("صلاحية إعداد سند القبض غير متاحة للمستخدم الحالي.");
                    using var dialog = new ReceiptConfigurationDialog(current);
                    if (dialog.ShowDialog(settings) != DialogResult.OK) return;
                    // The endpoint rechecks the current permission, scope and configuration version.
                    await ReceiptRequest<ReceiptConfiguration>(HttpMethod.Put, "configuration",
                        new ReceiptConfigurationUpdate(dialog.Configuration, current.ConfigurationVersion));
                    MessageBox.Show(settings, "حُفظت إعدادات القبض في مصدرها الحالي. أعد فتح شاشة القبض لتحميل القوائم الجديدة.",
                        "إعدادات سند القبض", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or TaskCanceledException or JsonException)
            { if (!settings.IsDisposed) settings.ShowConnectionError("تعذر تحميل إعدادات الأستاذ العام: " + ex.Message); }
        };
    }

    private async Task<LedgerSettingsDocument> LedgerSettingsRequest(HttpMethod method, LedgerSettingsUpdate? update = null,
        Func<HttpRequestMessage, Task<HttpResponseMessage>>? send = null)
    {
        using var request = new HttpRequestMessage(method, "api/v1/accounting/settings");
        if (update != null) request.Content = JsonContent.Create(update);
        using var response = await (send != null ? send(request) : AuthenticatedSession!.SendAsync(request));
        if (!response.IsSuccessStatusCode)
        {
            string message = response.StatusCode switch
            {
                System.Net.HttpStatusCode.Forbidden => "الصلاحية الحالية لا تسمح بعرض أو تعديل إعدادات الشركة.",
                System.Net.HttpStatusCode.Unauthorized => "انتهت جلسة الدخول.",
                System.Net.HttpStatusCode.Conflict => "تغيرت الإعدادات منذ تحميلها. أعد التحميل قبل الحفظ.",
                _ => "تعذر إكمال الطلب؛ أعد التحميل للتحقق من الإعدادات."
            };
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                using var error = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                if (error.RootElement.TryGetProperty("message", out var text)) message = text.GetString() ?? message;
            }
            throw new InvalidOperationException(message);
        }
        return await response.Content.ReadFromJsonAsync<LedgerSettingsDocument>()
            ?? throw new InvalidOperationException("استجابة إعدادات الأستاذ العام غير مكتملة.");
    }
}
