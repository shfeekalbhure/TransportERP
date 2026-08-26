using Android.App;
using Android.Runtime;

namespace TransportERP.Mobile.Driver;

[Application]
public sealed class MainApplication(IntPtr handle, JniHandleOwnership ownership)
    : MauiApplication(handle, ownership)
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
