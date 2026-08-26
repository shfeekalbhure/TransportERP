namespace TransportERP.Mobile.Driver;

public sealed class MainPage : ContentPage
{
    public MainPage()
    {
        Title = "TransportERP Driver";
        BackgroundColor = Color.FromArgb("#F4F7F8");
        Content = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 12,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = "TransportERP Driver",
                    FontSize = 28,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0B3A53")
                },
                new Label
                {
                    Text = "Offline is closed. Sign in and explicitly activate an authorized scope to use local synchronization.",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#263238")
                }
            }
        };
    }
}
