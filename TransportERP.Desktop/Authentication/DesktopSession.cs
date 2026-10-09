using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TransportERP.Contracts.Authentication;

namespace TransportERP.Desktop.Authentication;

public sealed class DesktopSession : IDisposable
{
    private readonly HttpClient client;
    public SessionResponse? Current { get; private set; }
    public DesktopSession(string address)
    {
        if (!Uri.TryCreate(address.TrimEnd('/') + "/", UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Query) || !string.IsNullOrEmpty(uri.Fragment))
            throw new ArgumentException("أدخل عنوان خادم HTTPS صحيحًا، مثل https://localhost:7011/");
        client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
        { BaseAddress = uri, Timeout = TimeSpan.FromSeconds(20) };
    }
    public Task<LoginResponse> LoginAsync(string name, string password) =>
        Post<LoginRequest, LoginResponse>("api/v1/auth/login", new(name, password));
    public async Task SelectAsync(SelectScopeRequest request)
    {
        Current = await Post<SelectScopeRequest, SessionResponse>("api/v1/auth/select-scope", request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Current.AccessToken);
    }
    // A receipt owns its child client/token. Never changes the main session or other tabs.
    public DesktopSession CreateReceiptSessionCopy()
    {
        var current = Current ?? throw new InvalidOperationException("جلسة الدخول مطلوبة.");
        if (current.ExpiresAt <= DateTimeOffset.UtcNow) throw new InvalidOperationException("انتهت جلسة الدخول.");
        var child = new DesktopSession(client.BaseAddress!.AbsoluteUri);
        child.SetReceiptSession(current);
        return child;
    }
    public async Task<List<ScopeOption>> ReceiptScopesAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/auth/receipt-scopes");
        using var response = await SendAsync(request);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException("تعذر تحميل الفروع المتاحة لسند القبض؛ تحقق من الجلسة والصلاحية.");
        return await response.Content.ReadFromJsonAsync<List<ScopeOption>>() ?? throw new InvalidOperationException("قائمة الفروع غير مكتملة.");
    }
    public async Task<DesktopSession> CreateReceiptBranchSessionAsync(Guid branchId)
    {
        var current = Current ?? throw new InvalidOperationException("جلسة الدخول مطلوبة.");
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/auth/receipt-scope")
        { Content = JsonContent.Create(new SelectReceiptScopeRequest(branchId)) };
        using var response = await SendAsync(request);
        if (!response.IsSuccessStatusCode) throw new InvalidOperationException("لم يُغيّر الفرع. قد تكون الصلاحية أو الفترة أو جلسة الدخول قد تغيرت.");
        var selected = await response.Content.ReadFromJsonAsync<SessionResponse>() ?? throw new InvalidOperationException("استجابة الفرع غير مكتملة.");
        if (selected.Scope.CompanyId != current.Scope.CompanyId || selected.Scope.FiscalPeriodId != current.Scope.FiscalPeriodId ||
            selected.Scope.BranchId != branchId || selected.ExpiresAt > current.ExpiresAt || selected.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("استجابة الفرع لا تطابق نطاق الجلسة المسموح.");
        var child = new DesktopSession(client.BaseAddress!.AbsoluteUri);
        child.SetReceiptSession(selected);
        return child;
    }
    private void SetReceiptSession(SessionResponse response)
    {
        Current = response;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
    }
    // Shared authenticated transport for subsequently connected screens; token stays in memory.
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
    {
        if (Current is null || Current.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("انتهت الجلسة. سجل الدخول من جديد.");
        return await client.SendAsync(request, ct);
    }
    private async Task<TResponse> Post<TRequest,TResponse>(string path, TRequest body)
    {
        using var response = await client.PostAsJsonAsync(path, body);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "بيانات الدخول غير صحيحة أو انتهت مهلة اختيار النطاق.",
                HttpStatusCode.Forbidden => "لا يوجد نطاق عمل مسموح أو تغيرت صلاحيات المستخدم.",
                HttpStatusCode.TooManyRequests => "محاولات كثيرة. انتظر دقيقة ثم حاول مجددًا.",
                HttpStatusCode.ServiceUnavailable => "خدمة الدخول غير مهيأة على الخادم.",
                HttpStatusCode.NotFound => "الخادم لا يحتوي إصدار خدمة الدخول المطلوبة.",
                _ => "تعذر إكمال الطلب على الخادم. راجع إعداد الخدمة وقاعدة البيانات."
            });
        return await response.Content.ReadFromJsonAsync<TResponse>()
            ?? throw new InvalidOperationException("استجابة الخادم غير مكتملة.");
    }
    public void Dispose() { Current = null; client.Dispose(); }
}
