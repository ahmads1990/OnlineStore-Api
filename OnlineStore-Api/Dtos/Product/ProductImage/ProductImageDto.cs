namespace OnlineStore_Api.Dtos.Product.ProductImage;

public class ProductImageDto
{
    public int ProductImageID { get; set; }
    public byte Order { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
