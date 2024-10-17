using OnlineStore_Api.Dtos.Category;
using OnlineStore_Api.Dtos.Product.ProductImage;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore_Api.Dtos.Product;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public float Price { get; set; }
    public CategoryDto Category { get; set; } = default!;
    [JsonPropertyName("Images")]
    public IEnumerable<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
}
