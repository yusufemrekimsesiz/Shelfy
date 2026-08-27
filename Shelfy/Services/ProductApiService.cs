using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Shelfy.Core;

namespace Shelfy.Services;

public class ProductApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductApiService> _logger;
    private const string BaseUrl = "https://world.openfoodfacts.org/api/v0/product/";

    public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProductInfo> GetProductByBarcodeAsync(string barcode)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            _logger.LogWarning("İnternet bağlantısı yok: {Barcode}", barcode);
            return new ProductInfo { Found = false, NetworkError = true };
        }

        try
        {
            var url = $"{BaseUrl}{barcode}.json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API başarısız yanıt döndü: {StatusCode}", response.StatusCode);
                return new ProductInfo { Found = false };
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<OpenFoodFactsResponse>(json, options);

            if (result?.Status != 1 || result.Product is null)
                return new ProductInfo { Found = false };

            return new ProductInfo
            {
                Found = true,
                ProductName = result.Product.ProductName ?? "Bilinmeyen Ürün",
                Brand = result.Product.Brands ?? "Bilinmeyen Marka",
                ImageUrl = result.Product.ImageUrl ?? string.Empty
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Ağ hatası: {Barcode}", barcode);
            return new ProductInfo { Found = false, NetworkError = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen hata: {Barcode}", barcode);
            return new ProductInfo { Found = false };
        }
    }

    private class OpenFoodFactsResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("product")]
        public ProductRaw? Product { get; set; }
    }

    private class ProductRaw
    {
        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("brands")]
        public string? Brands { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }
    }
}