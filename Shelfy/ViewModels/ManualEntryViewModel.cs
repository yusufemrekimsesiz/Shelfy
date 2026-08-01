using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Models;
using Shelfy.Services;

namespace Shelfy.ViewModels;

[QueryProperty(nameof(Barcode), "Barcode")]
public partial class ManualEntryViewModel : ObservableObject
{
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

    public ManualEntryViewModel(DatabaseService databaseService, NotificationService notificationService)
    {
        _databaseService = databaseService;
        _notificationService = notificationService;
    }

    partial void OnExpirationDateChanged(DateTime value)
    {
        HasSelectedExpirationDate = true;
        SaveCommand.NotifyCanExecuteChanged();
    }

    partial void OnProductNameChanged(string value)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    private bool CanSave() => HasSelectedExpirationDate && !string.IsNullOrWhiteSpace(ProductName);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        var item = new PantryItem
        {
            Barcode = Barcode,
            ProductName = ProductName,
            Brand = string.IsNullOrWhiteSpace(Brand) ? "Bilinmeyen Marka" : Brand,
            ImageUrl = ImageUrl,
            Quantity = Quantity,
            ExpirationDate = ExpirationDate
        };

        await _databaseService.SaveAsync(item);
        await _notificationService.ScheduleExpirationNotificationAsync(item);
        await Shell.Current.GoToAsync("//InventoryPage");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("//InventoryPage");
    }
}