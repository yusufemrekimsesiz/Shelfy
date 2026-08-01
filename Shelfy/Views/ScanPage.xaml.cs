using Shelfy.ViewModels;
using ZXing.Net.Maui;

namespace Shelfy.Views;

public partial class ScanPage : ContentPage
{
    private readonly ScanViewModel _viewModel;

    public ScanPage(ScanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        await _viewModel.BarcodeDetectedCommand.ExecuteAsync(e);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.IsDetecting = true;
    }
}