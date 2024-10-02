namespace OnlineStore_Api.Models;

public class ProductImage
{
    public int ProductImageID { get; set; }
    public byte Order { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Relations
    public int ProductID { get; set; }
}
