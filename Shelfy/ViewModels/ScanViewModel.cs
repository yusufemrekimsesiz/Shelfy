using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing.Net.Maui;
using Shelfy.Views;

namespace Shelfy.ViewModels;

public partial class ScanViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isDetecting = true;

    [RelayCommand]
    private async Task BarcodeDetectedAsync(BarcodeDetectionEventArgs args)
    {
        if (!IsDetecting) return;

        var result = args.Results?.FirstOrDefault();
        if (result is null) return;

        IsDetecting = false;
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);

        var barcode = result.Value;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(nameof(ProductDetailsPage),
                new Dictionary<string, object> { { "Barcode", barcode } });
        });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
