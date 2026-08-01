using Shelfy.Views;

namespace Shelfy;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
        Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
        Routing.RegisterRoute(nameof(ManualEntryPage), typeof(ManualEntryPage));
    }
}