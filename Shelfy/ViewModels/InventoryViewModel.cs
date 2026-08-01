using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Models;
using Shelfy.Services;
using Shelfy.Views;

namespace Shelfy.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly NotificationService _notificationService;
    private List<PantryItem> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<PantryItem> pantryItems = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool isEmpty;

    [ObservableProperty]
    private bool showNoResults;

    [ObservableProperty]
    private string searchText = string.Empty;

    public InventoryViewModel(DatabaseService databaseService, NotificationService notificationService)
    {
        _databaseService = databaseService;
        _notificationService = notificationService;
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        _allItems = await _databaseService.GetAllAsync();
        ApplyFilter();
        IsRefreshing = false;
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allItems
            : _allItems.Where(x =>
                x.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                x.Brand.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

        PantryItems.Clear();
        foreach (var item in filtered)
            PantryItems.Add(item);

        IsEmpty = _allItems.Count == 0;
        ShowNoResults = _allItems.Count > 0 && filtered.Count == 0;
    }

    [RelayCommand]
    private async Task GoToScanAsync()
    {
        await Shell.Current.GoToAsync(nameof(ScanPage));
    }

    [RelayCommand]
    private async Task GoToManualEntryAsync()
    {
        await Shell.Current.GoToAsync(nameof(ManualEntryPage));
    }

    [RelayCommand]
    private async Task DeleteItemAsync(PantryItem item)
    {
        if (item is null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Ürünü Sil",
            $"\"{item.ProductName}\" kileri listesinden silinsin mi?",
            "Sil", "Vazgeç");

        if (!confirm) return;

        await _databaseService.DeleteAsync(item);
        await _notificationService.CancelNotificationAsync(item.Id);

        _allItems.Remove(item);
        PantryItems.Remove(item);
        IsEmpty = _allItems.Count == 0;
        ShowNoResults = _allItems.Count > 0 && PantryItems.Count == 0;
    }
}