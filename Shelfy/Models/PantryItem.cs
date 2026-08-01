using SQLite;

namespace Shelfy.Models;

public class PantryItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }

    public bool IsExpired => ExpirationDate.Date < DateTime.Today;
}