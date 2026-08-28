using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Core;
using Shelfy.Localization;
using Shelfy.Resources.Strings;
using Shelfy.Services;
using Shelfy.Views;

namespace Shelfy.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    private readonly IPantryRepository _pantryRepository;
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

    [ObservableProperty]
    private PickerOption selectedSortOption = null!;

    [ObservableProperty]
    private PickerOption selectedCategory = null!;

    public List<PickerOption> SortOptions { get; } = SortLocalizer.BuildSortOptions();
    public List<PickerOption> CategoryFilterOptions { get; } = CategoryLocalizer.BuildCategoryOptionsWithAll();

    public InventoryViewModel(IPantryRepository pantryRepository, NotificationService notificationService)
    {
        _pantryRepository = pantryRepository;
        _notificationService = notificationService;

        selectedSortOption = SortOptions.First(o => o.Key == PantrySortOptions.ByExpiration);
        selectedCategory = CategoryFilterOptions.First(o => o.Key == Categories.AllKey);
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        _allItems = await _pantryRepository.GetAllAsync();
        ApplyFilter();
        IsRefreshing = false;
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedSortOptionChanged(PickerOption value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(PickerOption value) => ApplyFilter();

    private void ApplyFilter()
    {
        var result = PantryFilterService.Filter(_allItems, SearchText, SelectedCategory.Key, SelectedSortOption.Key);

        PantryItems.Clear();
        foreach (var item in result)
            PantryItems.Add(item);

        IsEmpty = _allItems.Count == 0;
        ShowNoResults = _allItems.Count > 0 && result.Count == 0;
    }

    [RelayCommand]
    private async Task GoToScanAsync() => await Shell.Current.GoToAsync(nameof(ScanPage));

    [RelayCommand]
    private async Task GoToManualEntryAsync() => await Shell.Current.GoToAsync(nameof(ManualEntryPage));

    [RelayCommand]
    private async Task IncrementQuantityAsync(PantryItem item)
    {
        if (item is null) return;
        item.Quantity++;
        await _pantryRepository.SaveAsync(item);
        RefreshItemInList(item);
    }

    [RelayCommand]
    private async Task DecrementQuantityAsync(PantryItem item)
    {
        if (item is null) return;

        if (item.Quantity <= 1)
        {
            await DeleteItemAsync(item);
            return;
        }

        item.Quantity--;
        await _pantryRepository.SaveAsync(item);
        RefreshItemInList(item);
    }

    private void RefreshItemInList(PantryItem item)
    {
        var index = PantryItems.IndexOf(item);
        if (index >= 0)
        {
            PantryItems.RemoveAt(index);
            PantryItems.Insert(index, item);
        }
    }

    [RelayCommand]
    private async Task DeleteItemAsync(PantryItem item)
    {
        if (item is null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            AppResources.Inventory_Delete_Title,
            string.Format(AppResources.Inventory_Delete_Message, item.ProductName),
            AppResources.Inventory_Delete_Confirm,
            AppResources.Cancel);

        if (!confirm) return;

        await _pantryRepository.DeleteAsync(item);
        await _notificationService.CancelNotificationAsync(item.Id);

        _allItems.Remove(item);
        PantryItems.Remove(item);
        IsEmpty = _allItems.Count == 0;
        ShowNoResults = _allItems.Count > 0 && PantryItems.Count == 0;
    }
}