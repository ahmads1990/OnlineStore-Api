using OnlineStore_Api.Dtos.Category;
using OnlineStore_Api.Dtos.Product.ProductImage;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore_Api.Dtos.Product;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public float Price { get; set; }
    public CategoryDto Category { get; set; } = default!;
    public IEnumerable<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
}
