using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Authentication;
using TransportERP.EmptyForms;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop;

public partial class FrmMain
{
    private UserControl? CreateConnectedWorkspaceControl(string code)
    {
        var control = CreatePhaseOneControl(code);
        if (control != null) ConnectLedgerSettingsControl(control);
        if (control is UcScreen_04_04_01 receipt && AuthenticatedSession != null)
        {
            var scopedSession = AuthenticatedSession.CreateReceiptSessionCopy();
            receipt.Disposed += (_, _) => scopedSession.Dispose();
            bool initialized = false;
            receipt.Load += async (_, _) =>
            {
                if (initialized) return;
                initialized = true;
                try
                {
                    var bootstrap = await ReceiptRequest<ReceiptBootstrap>(HttpMethod.Get, "bootstrap", session: scopedSession);
                    if (receipt.IsDisposed) return;
                    List<ScopeOption> branches = [scopedSession.Current!.Scope];
                    string? branchError = null;
                    try { branches = await scopedSession.ReceiptScopesAsync(); }
                    catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or TaskCanceledException or JsonException)
                    { branchError = "تم ربط سند القبض بالفرع الحالي؛ تعذر تحميل فروع أخرى: " + ex.Message; }
                    if (!receipt.IsDisposed)
                    {
                        ConfigureReceiptWorkspace(receipt, scopedSession, bootstrap, branches);
                        if (branchError != null) receipt.ShowReceiptBranchStatus(branchError);
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException or TaskCanceledException or JsonException)
                { if (!receipt.IsDisposed) MessageBox.Show(receipt, "تعذر ربط سند القبض بالخادم: " + ex.Message, "سند القبض", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
        }
        return control;
    }

    private void ConfigureReceiptWorkspace(UcScreen_04_04_01 receipt, Authentication.DesktopSession scopedSession,
        ReceiptBootstrap bootstrap, List<ScopeOption> branches)
    {
        Task<T> ReceiptScopedRequest<T>(HttpMethod method, string path, object? body = null) =>
            ReceiptRequest<T>(method, path, body, scopedSession);
                    receipt.SetReceiptOrganization(scopedSession.Current!.Scope.CompanyName);
                    receipt.ReceiptSettingsRequested += async (_, _) =>
                    {
                        try
                        {
                            var current = await ReceiptScopedRequest<ReceiptBootstrap>(HttpMethod.Get, "bootstrap");
                            using var dialog = new ReceiptConfigurationDialog(current);
                            if (dialog.ShowDialog(receipt) != DialogResult.OK) return;
                            await ReceiptScopedRequest<ReceiptConfiguration>(HttpMethod.Put, "configuration", new ReceiptConfigurationUpdate(dialog.Configuration, current.ConfigurationVersion));
                            MessageBox.Show(receipt, "حُفظت الإعدادات. أعد فتح شاشة القبض لتحميل القوائم الجديدة.", "إعدادات القبض");
                        }
                        catch (Exception ex) { MessageBox.Show(receipt, "لم تُحفظ الإعدادات: " + ex.Message, "إعدادات القبض"); }
                    };
                    receipt.ConnectReceiptWorkspace(bootstrap, async (action, draft, id, version) =>
                    {
                        if (action == "View")
                        {
                            var list = await ReceiptScopedRequest<List<ReceiptListItem>>(HttpMethod.Get, "");
                            using var selector = new Form { Text = "اختيار سند قبض — أحدث 200 سند", Width = 850, Height = 500, StartPosition = FormStartPosition.CenterParent,
                                RightToLeft = RightToLeft.Yes, RightToLeftLayout = true };
                            var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
                                AutoGenerateColumns = true, DataSource = list, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
                            var open = new Button { Text = "فتح", Dock = DockStyle.Bottom, Height = 38, DialogResult = DialogResult.OK };
                            selector.Controls.Add(grid); selector.Controls.Add(open); selector.AcceptButton = open;
                            if (selector.ShowDialog(receipt) != DialogResult.OK || grid.CurrentRow?.DataBoundItem is not ReceiptListItem selected) return null;
                            return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Get, selected.Id.ToString());
                        }
                        if (action is "Create" or "Edit") return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Put, "", draft);
                        if (action == "Post" && id != null) return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Post, id + "/post", new { Version = version });
                        if (action == "Approve" && id != null) return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Post, id + "/approve", new { Version = version });
                        if (action == "Review" && id != null) return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Post, id + "/review", new { Version = version });
                        if (action is "Cancel" or "Reverse" && id != null)
                        {
                            using var reasonDialog = new Form { Text = action == "Cancel" ? "سبب الإلغاء" : "سبب وتاريخ العكس", Width = 520, Height = 290,
                                StartPosition = FormStartPosition.CenterParent, RightToLeft = RightToLeft.Yes };
                            var reason = new TextBox { Multiline = true, Dock = DockStyle.Fill, MaxLength = 500 };
                            var date = new DateTimePicker { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short, Visible = action == "Reverse" };
                            var confirm = new Button { Text = "تأكيد", Dock = DockStyle.Bottom, Height = 38 };
                            confirm.Click += (_, _) => { if (!string.IsNullOrWhiteSpace(reason.Text)) reasonDialog.DialogResult = DialogResult.OK; };
                            reasonDialog.Controls.Add(reason); reasonDialog.Controls.Add(date); reasonDialog.Controls.Add(confirm);
                            if (reasonDialog.ShowDialog(receipt) != DialogResult.OK) return null;
                            return await ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Post, id + "/" + action.ToLowerInvariant(),
                                new { Version = version, Reason = reason.Text, Date = date.Value.Date });
                        }
                        throw new InvalidOperationException("الأمر غير متاح بعد في خدمة سند القبض.");
                    });
                    receipt.ConnectReceiptTabServices(async configuration =>
                    {
                        await ReceiptScopedRequest<ReceiptConfiguration>(HttpMethod.Put, "configuration", new ReceiptConfigurationUpdate(configuration, bootstrap.ConfigurationVersion));
                        bootstrap = await ReceiptScopedRequest<ReceiptBootstrap>(HttpMethod.Get, "bootstrap"); return bootstrap;
                    }, (id, upload) => ReceiptScopedRequest<ReceiptDocument>(HttpMethod.Post, id + "/attachments", upload),
                    (id, attachment) => ReceiptScopedRequest<ReceiptAttachmentDownload>(HttpMethod.Get, id + "/attachments/" + attachment));

        var scope = scopedSession.Current!.Scope;
        receipt.ConnectReceiptBranches(scope.BranchId,
            branches.Where(b => b.CompanyId == scope.CompanyId && b.FiscalPeriodId == scope.FiscalPeriodId)
                .Select(b => new ReceiptChoice(b.BranchId, b.BranchName)),
            branchId => ReplaceReceiptBranchAsync(receipt, scopedSession, branchId));
    }

    private async Task<bool> ReplaceReceiptBranchAsync(UcScreen_04_04_01 receipt,
        Authentication.DesktopSession currentSession, Guid branchId)
    {
        if (receipt.Parent is not TabPage page || receipt.HasUnsavedChanges || receipt.Binding.ExpectedVersion != null) return false;
        Authentication.DesktopSession? nextSession = null;
        UcScreen_04_04_01? replacement = null;
        try
        {
            nextSession = await currentSession.CreateReceiptBranchSessionAsync(branchId);
            var bootstrap = await ReceiptRequest<ReceiptBootstrap>(HttpMethod.Get, "bootstrap", session: nextSession);
            var scopes = await nextSession.ReceiptScopesAsync();
            if (receipt.IsDisposed || page.IsDisposed || receipt.Parent != page) return false;
            replacement = new UcScreen_04_04_01();
            var ownedSession = nextSession;
            replacement.Disposed += (_, _) => ownedSession.Dispose();
            ConfigureReceiptWorkspace(replacement, nextSession, bootstrap, scopes);
            SharedScreenProperties.ConfigureWorkspace(replacement);
            replacement.CloseRequested += (_, _) => CloseWorkspacePage(page);
            // All network checks and new-screen initialization succeeded before replacing the old draft.
            page.Controls.Add(replacement);
            page.Controls.Remove(receipt);
            workspaceChanges[page] = new WorkspaceChangeSnapshot(replacement);
            receipt.Dispose();
            replacement.Select();
            nextSession = null; replacement = null;
            return true;
        }
        finally
        {
            if (replacement != null) { page.Controls.Remove(replacement); replacement.Dispose(); }
            nextSession?.Dispose();
        }
    }

    private async Task<T> ReceiptRequest<T>(HttpMethod method, string path, object? body = null, Authentication.DesktopSession? session = null)
    {
        using var request = new HttpRequestMessage(method, "api/v1/receipts/" + path);
        if (body != null) request.Content = JsonContent.Create(body);
        using var response = await (session ?? AuthenticatedSession ?? throw new InvalidOperationException("جلسة الدخول مطلوبة.")).SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            string message = response.StatusCode switch
            {
                System.Net.HttpStatusCode.Forbidden => "الصلاحية الحالية لا تسمح بهذا الإجراء.",
                System.Net.HttpStatusCode.NotFound => "السند أو خدمة القبض غير موجودة.",
                System.Net.HttpStatusCode.Unauthorized => "انتهت جلسة الدخول.",
                _ => "تعذر إكمال العملية؛ أعد تحميل السند للتحقق من حالته."
            };
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                using var error = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                if (error.RootElement.TryGetProperty("message", out var text)) message = text.GetString() ?? message;
            }
            throw new InvalidOperationException(message);
        }
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new InvalidOperationException("استجابة سند القبض غير مكتملة.");
    }
}
