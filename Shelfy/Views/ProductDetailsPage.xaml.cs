using Shelfy.ViewModels;

namespace Shelfy.Views;

public partial class ProductDetailsPage : ContentPage
{
    public ProductDetailsPage(ProductDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}