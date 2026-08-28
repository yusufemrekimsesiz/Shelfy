using Shelfy.ViewModels;

namespace Shelfy.Views;

public partial class ManualEntryPage : ContentPage
{
    private readonly ManualEntryViewModel _viewModel;

    public ManualEntryPage(ManualEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshCategoryOptions();
    }
}