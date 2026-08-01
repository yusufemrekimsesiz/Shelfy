using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using ZXing.Net.Maui.Controls;
using Shelfy.Services;
using Shelfy.ViewModels;
using Shelfy.Views;

namespace Shelfy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servisler
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<ProductApiService>();
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddSingleton<HttpClient>();

        // ViewModel'ler
        builder.Services.AddSingleton<InventoryViewModel>();
        builder.Services.AddTransient<ScanViewModel>();
        builder.Services.AddTransient<ProductDetailsViewModel>();
        builder.Services.AddTransient<ManualEntryViewModel>();

        // Sayfalar
        builder.Services.AddSingleton<InventoryPage>();
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<ProductDetailsPage>();
        builder.Services.AddTransient<ManualEntryPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}