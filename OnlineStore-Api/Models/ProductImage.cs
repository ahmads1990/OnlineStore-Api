namespace OnlineStore_Api.Models;

public class ProductImage : BaseEntity
{
    public byte Order { get; set; }
    public string ImagePath { get; set; } = string.Empty;

    // Relations
    public int ProductID { get; set; }
}
