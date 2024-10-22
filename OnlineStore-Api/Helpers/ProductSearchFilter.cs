namespace OnlineStore_Api.Helpers;

public class ProductSearchFilter
{
    public int? CategoryId { get; set; }
    public string? Name { get; set; }
    public int? Page { get; set; } = 1;
    public int? Limit { get; set; } = 50;
    public bool IncludeImages { get; set; } = false;
}
