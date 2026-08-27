using Shelfy.Core;
using Xunit;

namespace Shelfy.Tests;

public class PantryFilterServiceTests
{
    private static List<PantryItem> SampleItems() => new()
    {
        new PantryItem { ProductName = "Süt", Brand = "Sütaş", Category = "Süt Ürünleri", ExpirationDate = DateTime.Today.AddDays(5), CreatedDate = DateTime.Now.AddDays(-2) },
        new PantryItem { ProductName = "Ekmek", Brand = "Uno", Category = "Diğer", ExpirationDate = DateTime.Today.AddDays(1), CreatedDate = DateTime.Now.AddDays(-1) },
        new PantryItem { ProductName = "Peynir", Brand = "Sütaş", Category = "Süt Ürünleri", ExpirationDate = DateTime.Today.AddDays(10), CreatedDate = DateTime.Now }
    };

    [Fact]
    public void Filter_BySearchText_ReturnsMatchingItems()
    {
        var result = PantryFilterService.Filter(SampleItems(), "süt", "Tümü", PantrySortOptions.ByExpiration);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Filter_ByCategory_ReturnsOnlyThatCategory()
    {
        var result = PantryFilterService.Filter(SampleItems(), "", "Süt Ürünleri", PantrySortOptions.ByExpiration);
        Assert.All(result, x => Assert.Equal("Süt Ürünleri", x.Category));
    }

    [Fact]
    public void Filter_SortByName_ReturnsAlphabeticalOrder()
    {
        var result = PantryFilterService.Filter(SampleItems(), "", "Tümü", PantrySortOptions.ByName);
        Assert.Equal("Ekmek", result.First().ProductName);
    }

    [Fact]
    public void Filter_SortByExpiration_ReturnsSoonestFirst()
    {
        var result = PantryFilterService.Filter(SampleItems(), "", "Tümü", PantrySortOptions.ByExpiration);
        Assert.Equal("Ekmek", result.First().ProductName);
    }
}