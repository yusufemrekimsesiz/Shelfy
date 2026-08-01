using Shelfy.ViewModels;

namespace Shelfy.Views;

public partial class ManualEntryPage : ContentPage
{
    public ManualEntryPage(ManualEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}