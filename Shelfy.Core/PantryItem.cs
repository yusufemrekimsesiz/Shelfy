using SQLite;

namespace Shelfy.Core;

public class PantryItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = Categories.Other;
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Ignore]
    public bool IsExpired => ExpirationDate.Date < DateTime.Today;

    [Ignore]
    public bool IsExpiringSoon => !IsExpired && (ExpirationDate.Date - DateTime.Today).TotalDays <= 3;
}