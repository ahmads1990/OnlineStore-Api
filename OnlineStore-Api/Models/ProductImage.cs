namespace OnlineStore_Api.Models;

public class ProductImage
{
    public int ProductImageID { get; set; }
    public byte Order { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
}
