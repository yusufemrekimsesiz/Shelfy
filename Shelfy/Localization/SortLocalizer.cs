using Shelfy.Core;
using Shelfy.Resources.Strings;

namespace Shelfy.Localization;

public static class SortLocalizer
{
    public static string GetDisplayName(string key) => key switch
    {
        PantrySortOptions.ByExpiration => AppResources.Sort_ByExpiration,
        PantrySortOptions.ByName => AppResources.Sort_ByName,
        PantrySortOptions.ByCreatedDate => AppResources.Sort_ByCreatedDate,
        _ => key
    };

    public static List<PickerOption> BuildSortOptions() =>
        PantrySortOptions.All.Select(k => new PickerOption(k, GetDisplayName(k))).ToList();
}