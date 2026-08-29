using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Core;
using Shelfy.Localization;
using Shelfy.Resources.Strings;
using Shelfy.Services;

namespace Shelfy.ViewModels;

[QueryProperty(nameof(Barcode), "Barcode")]
public partial class ManualEntryViewModel : ObservableObject
{
    private readonly IPantryRepository _pantryRepository;
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
    private bool hasImage;

    [ObservableProperty]
    private PickerOption selectedCategoryOption = null!;

    public string Category => SelectedCategoryOption.Key;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private DateTime expirationDate = DateTime.Today;

    [ObservableProperty]
    private bool hasSelectedExpirationDate;

    [ObservableProperty]
    private List<PickerOption> categoryOptions = CategoryLocalizer.BuildCategoryOptions();

    public bool IsCameraSupported => DeviceInfo.Platform != DevicePlatform.WinUI;

    public ManualEntryViewModel(IPantryRepository pantryRepository, NotificationService notificationService)
    {
        _pantryRepository = pantryRepository;
        _notificationService = notificationService;
        selectedCategoryOption = CategoryOptions.First(o => o.Key == Categories.Other);
    }

    public void RefreshCategoryOptions()
    {
        var previousKey = SelectedCategoryOption?.Key ?? Categories.Other;
        CategoryOptions = CategoryLocalizer.BuildCategoryOptions();
        SelectedCategoryOption = CategoryOptions.First(o => o.Key == previousKey);
    }

    partial void OnExpirationDateChanged(DateTime value)
    {
        HasSelectedExpirationDate = true;
        SaveCommand.NotifyCanExecuteChanged();
    }

    partial void OnProductNameChanged(string value) => SaveCommand.NotifyCanExecuteChanged();

    partial void OnImageUrlChanged(string value) => HasImage = !string.IsNullOrWhiteSpace(value);

    [RelayCommand]
    private async Task TakePhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlertAsync(AppResources.Alert_NotSupported_Title, AppResources.Alert_CameraNotSupported, "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await SavePhotoLocallyAsync(photo);
        }
        catch (Exception ex)
        {
             await Shell.Current.DisplayAlertAsync(AppResources.Alert_Error_Title, string.Format(AppResources.Alert_PhotoCaptureFailed, ex.Message), "OK");
        }
    }

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        try
        {
            var photos = await MediaPicker.Default.PickPhotosAsync();
            var photo = photos?.FirstOrDefault();
            if (photo is null) return;

            await SavePhotoLocallyAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync(AppResources.Alert_Error_Title, string.Format(AppResources.Alert_PhotoPickFailed, ex.Message), "OK");
        }
    }

    private async Task SavePhotoLocallyAsync(FileResult photo)
    {
        var localFileName = $"{Guid.NewGuid()}.jpg";
        var localPath = Path.Combine(FileSystem.AppDataDirectory, localFileName);

        using var sourceStream = await photo.OpenReadAsync();
        using var localStream = File.OpenWrite(localPath);
        await sourceStream.CopyToAsync(localStream);

        ImageUrl = localPath;
    }

    [RelayCommand]
    private void RemovePhoto() => ImageUrl = string.Empty;

    private bool CanSave() => HasSelectedExpirationDate && !string.IsNullOrWhiteSpace(ProductName);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        var item = new PantryItem
        {
            Barcode = Barcode,
            ProductName = ProductName,
            Brand = string.IsNullOrWhiteSpace(Brand) ? AppResources.Category_Other : Brand,
            ImageUrl = ImageUrl,
            Category = Category,
            Quantity = Quantity,
            ExpirationDate = ExpirationDate,
            CreatedDate = DateTime.Now
        };

        await _pantryRepository.SaveAsync(item);
        await _notificationService.ScheduleExpirationNotificationAsync(item);
        await Shell.Current.GoToAsync("//InventoryPage");
    }

    [RelayCommand]
    private async Task CancelAsync() => await Shell.Current.GoToAsync("//InventoryPage");
}