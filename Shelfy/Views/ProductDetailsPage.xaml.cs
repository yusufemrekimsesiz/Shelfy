using Shelfy.ViewModels;

namespace Shelfy.Views;

public partial class ProductDetailsPage : ContentPage
{
    private readonly ProductDetailsViewModel _viewModel;

    public ProductDetailsPage(ProductDetailsViewModel viewModel)
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