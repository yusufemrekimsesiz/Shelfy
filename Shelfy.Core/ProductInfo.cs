namespace Shelfy.Core;

public class ProductInfo
{
    public bool Found { get; set; }
    public bool NetworkError { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}