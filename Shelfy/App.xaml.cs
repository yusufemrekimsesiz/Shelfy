using Microsoft.Extensions.DependencyInjection;
using Plugin.LocalNotification;
using Shelfy.Services;

namespace Shelfy;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();
        var notificationService = IPlatformApplication.Current?.Services.GetService<NotificationService>();
        _ = notificationService?.RequestPermissionAsync();
    }
}