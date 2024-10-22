namespace OnlineStore_Api.Dtos.Product.ProductImage;

public class AddProductImageDto
{
    public byte Order { get; set; }
    public IFormFile ImageFile { get; set; } = default!;
}
