namespace Shelfy.Core;

public static class PantrySortOptions
{
    public const string ByExpiration = "SKT'ye Göre";
    public const string ByName = "Alfabetik";
    public const string ByCreatedDate = "Eklenme Tarihine Göre";

    public static readonly string[] All = { ByExpiration, ByName, ByCreatedDate };
}

public static class PantryFilterService
{
    public static List<PantryItem> Filter(
        IEnumerable<PantryItem> items,
        string searchText,
        string category,
        string sortOption)
    {
        IEnumerable<PantryItem> result = items;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            result = result.Where(x =>
                x.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                x.Brand.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category) && category != "Tümü")
        {
            result = result.Where(x => x.Category == category);
        }

        result = sortOption switch
        {
            PantrySortOptions.ByName => result.OrderBy(x => x.ProductName),
            PantrySortOptions.ByCreatedDate => result.OrderByDescending(x => x.CreatedDate),
            _ => result.OrderBy(x => x.ExpirationDate)
        };

        return result.ToList();
    }
}