using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Models;
using Shelfy.Services;
using Shelfy.Views;

namespace Shelfy.ViewModels;

[QueryProperty(nameof(Barcode), "Barcode")]
public partial class ProductDetailsViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService;
    private readonly DatabaseService _databaseService;
    private readonly NotificationService _notificationService;

    [ObservableProperty]
    private string barcode = string.Empty;

    [ObservableProperty]
    private string productName = string.Empty;

    [ObservableProperty]
    private string brand = string.Empty;

    [ObservableProperty]
    private string imageUrl = string.Empty;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private DateTime expirationDate = DateTime.Today;

    [ObservableProperty]
    private bool hasSelectedExpirationDate;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isNotFound;

    public ProductDetailsViewModel(
        ProductApiService productApiService,
        DatabaseService databaseService,
        NotificationService notificationService)
    {
        _productApiService = productApiService;
        _databaseService = databaseService;
        _notificationService = notificationService;
    }

    async partial void OnBarcodeChanged(string value)
    {
        await LoadProductAsync(value);
    }

    partial void OnExpirationDateChanged(DateTime value)
    {
        HasSelectedExpirationDate = true;
        AddToPantryCommand.NotifyCanExecuteChanged();
    }

    private async Task LoadProductAsync(string code)
    {
        IsLoading = true;
        IsNotFound = false;
        HasSelectedExpirationDate = false;
        ExpirationDate = DateTime.Today;
        AddToPantryCommand.NotifyCanExecuteChanged();

        var info = await _productApiService.GetProductByBarcodeAsync(code);

        if (info.Found)
        {
            ProductName = info.ProductName;
            Brand = info.Brand;
            ImageUrl = info.ImageUrl;
        }
        else
        {
            IsNotFound = true;
            ProductName = "Ürün bulunamadı";
        }

        IsLoading = false;
    }

    private bool CanAddToPantry() => HasSelectedExpirationDate;

    [RelayCommand(CanExecute = nameof(CanAddToPantry))]
    private async Task AddToPantryAsync()
    {
        var item = new PantryItem
        {
            Barcode = Barcode,
            ProductName = ProductName,
            Brand = Brand,
            ImageUrl = ImageUrl,
            Quantity = Quantity,
            ExpirationDate = ExpirationDate
        };

        await _databaseService.SaveAsync(item);
        await _notificationService.ScheduleExpirationNotificationAsync(item);
        await Shell.Current.GoToAsync("//InventoryPage");
    }

    [RelayCommand]
    private async Task GoToManualEntryAsync()
    {
        await Shell.Current.GoToAsync(nameof(ManualEntryPage), new Dictionary<string, object>
        {
            { "Barcode", Barcode }
        });
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("//InventoryPage");
    }
}