using Shelfy.Core;
using Shelfy.Resources.Strings;

namespace Shelfy.Localization;

public static class CategoryLocalizer
{
    public static string GetDisplayName(string key) => key switch
    {
        Categories.Dairy => AppResources.Category_Dairy,
        Categories.Meat => AppResources.Category_Meat,
        Categories.Canned => AppResources.Category_Canned,
        Categories.Beverage => AppResources.Category_Beverage,
        Categories.Snack => AppResources.Category_Snack,
        Categories.Cleaning => AppResources.Category_Cleaning,
        Categories.Produce => AppResources.Category_Produce,
        Categories.Other => AppResources.Category_Other,
        Categories.AllKey => AppResources.Category_All,
        _ => key
    };

    public static List<PickerOption> BuildCategoryOptions() =>
        Categories.All.Select(k => new PickerOption(k, GetDisplayName(k))).ToList();

    public static List<PickerOption> BuildCategoryOptionsWithAll()
    {
        var list = new List<PickerOption> { new(Categories.AllKey, AppResources.Category_All) };
        list.AddRange(BuildCategoryOptions());
        return list;
    }
}