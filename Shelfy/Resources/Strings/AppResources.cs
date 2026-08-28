using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Shelfy.Resources.Strings;

public static class AppResources
{
    private static readonly ResourceManager ResourceManager =
        new ResourceManager("Shelfy.Resources.Strings.AppResources", typeof(AppResources).GetTypeInfo().Assembly);

    private static string Get(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? name;

    public static string Inventory_Title => Get(nameof(Inventory_Title));
    public static string Inventory_SearchPlaceholder => Get(nameof(Inventory_SearchPlaceholder));
    public static string Inventory_AddManually => Get(nameof(Inventory_AddManually));
    public static string Inventory_Empty_Title => Get(nameof(Inventory_Empty_Title));
    public static string Inventory_Empty_Subtitle => Get(nameof(Inventory_Empty_Subtitle));
    public static string Inventory_NoResults => Get(nameof(Inventory_NoResults));
    public static string Inventory_Expiry_Format => Get(nameof(Inventory_Expiry_Format));
    public static string Inventory_Delete_Title => Get(nameof(Inventory_Delete_Title));
    public static string Inventory_Delete_Message => Get(nameof(Inventory_Delete_Message));
    public static string Inventory_Delete_Confirm => Get(nameof(Inventory_Delete_Confirm));
    public static string Cancel => Get(nameof(Cancel));
    public static string Sort_Title => Get(nameof(Sort_Title));
    public static string Category_Title => Get(nameof(Category_Title));
    public static string Category_All => Get(nameof(Category_All));

    public static string Scan_Title => Get(nameof(Scan_Title));
    public static string Scan_Instruction => Get(nameof(Scan_Instruction));
    public static string Back => Get(nameof(Back));

    public static string ProductDetails_Title => Get(nameof(ProductDetails_Title));
    public static string ProductDetails_NotFound => Get(nameof(ProductDetails_NotFound));
    public static string ProductDetails_NetworkError => Get(nameof(ProductDetails_NetworkError));
    public static string ProductDetails_Retry => Get(nameof(ProductDetails_Retry));
    public static string ProductDetails_NoImage => Get(nameof(ProductDetails_NoImage));
    public static string TakePhoto => Get(nameof(TakePhoto));
    public static string PickPhoto => Get(nameof(PickPhoto));
    public static string RemovePhoto => Get(nameof(RemovePhoto));
    public static string Quantity_Label => Get(nameof(Quantity_Label));
    public static string ExpirationDate_Label => Get(nameof(ExpirationDate_Label));
    public static string ExpirationDate_Warning => Get(nameof(ExpirationDate_Warning));
    public static string AddToPantry => Get(nameof(AddToPantry));
    public static string EnterManually => Get(nameof(EnterManually));

    public static string ManualEntry_Title => Get(nameof(ManualEntry_Title));
    public static string ProductName_Label => Get(nameof(ProductName_Label));
    public static string ProductName_Placeholder => Get(nameof(ProductName_Placeholder));
    public static string Brand_Label => Get(nameof(Brand_Label));
    public static string Brand_Placeholder => Get(nameof(Brand_Placeholder));
    public static string Photo_Label => Get(nameof(Photo_Label));

    public static string Alert_NotSupported_Title => Get(nameof(Alert_NotSupported_Title));
    public static string Alert_CameraNotSupported => Get(nameof(Alert_CameraNotSupported));
    public static string Alert_Error_Title => Get(nameof(Alert_Error_Title));
    public static string Alert_PhotoCaptureFailed => Get(nameof(Alert_PhotoCaptureFailed));
    public static string Alert_PhotoPickFailed => Get(nameof(Alert_PhotoPickFailed));

    public static string Category_Dairy => Get(nameof(Category_Dairy));
    public static string Category_Meat => Get(nameof(Category_Meat));
    public static string Category_Canned => Get(nameof(Category_Canned));
    public static string Category_Beverage => Get(nameof(Category_Beverage));
    public static string Category_Snack => Get(nameof(Category_Snack));
    public static string Category_Cleaning => Get(nameof(Category_Cleaning));
    public static string Category_Produce => Get(nameof(Category_Produce));
    public static string Category_Other => Get(nameof(Category_Other));

    public static string Sort_ByExpiration => Get(nameof(Sort_ByExpiration));
    public static string Sort_ByName => Get(nameof(Sort_ByName));
    public static string Sort_ByCreatedDate => Get(nameof(Sort_ByCreatedDate));
}