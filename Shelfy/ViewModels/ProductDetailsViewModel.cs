using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shelfy.Core;
using Shelfy.Services;
using Shelfy.Views;

namespace Shelfy.ViewModels;

[QueryProperty(nameof(Barcode), "Barcode")]
public partial class ProductDetailsViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService;
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
    private string category = "Diğer";

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

    [ObservableProperty]
    private bool isNetworkError;

    public string[] CategoryOptions => Categories.All;

    public bool IsCameraSupported => DeviceInfo.Platform != DevicePlatform.WinUI;

    public bool ShowContent => !IsLoading && !IsNetworkError;

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowContent));
    partial void OnIsNetworkErrorChanged(bool value) => OnPropertyChanged(nameof(ShowContent));
    partial void OnImageUrlChanged(string value) => HasImage = !string.IsNullOrWhiteSpace(value);

    public ProductDetailsViewModel(
        ProductApiService productApiService,
        IPantryRepository pantryRepository,
        NotificationService notificationService)
    {
        _productApiService = productApiService;
        _pantryRepository = pantryRepository;
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
        IsNetworkError = false;
        HasSelectedExpirationDate = false;
        ExpirationDate = DateTime.Today;
        AddToPantryCommand.NotifyCanExecuteChanged();

        var info = await _productApiService.GetProductByBarcodeAsync(code);

        if (info.NetworkError)
        {
            IsNetworkError = true;
            ProductName = "İnternet bağlantısı bulunamadı";
        }
        else if (info.Found)
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

    [RelayCommand]
    private async Task TakePhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("Desteklenmiyor", "Bu cihazda kamera kullanılamıyor.", "Tamam");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await SavePhotoLocallyAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Fotoğraf çekilemedi: {ex.Message}", "Tamam");
        }
    }

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo is null) return;

            await SavePhotoLocallyAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Fotoğraf seçilemedi: {ex.Message}", "Tamam");
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
    private async Task GoToManualEntryAsync()
    {
        await Shell.Current.GoToAsync(nameof(ManualEntryPage), new Dictionary<string, object>
        {
            { "Barcode", Barcode }
        });
    }

    [RelayCommand]
    private async Task RetryAsync() => await LoadProductAsync(Barcode);

    [RelayCommand]
    private async Task CancelAsync() => await Shell.Current.GoToAsync("//InventoryPage");
}