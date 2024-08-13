namespace OnlineStore_Api.Models;

public class Category
{
    public byte CategoryID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
