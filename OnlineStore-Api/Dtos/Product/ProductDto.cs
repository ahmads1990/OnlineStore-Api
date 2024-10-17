using OnlineStore_Api.Dtos.Category;
using OnlineStore_Api.Dtos.Product.ProductImage;
using System.Text.Json.Serialization;

namespace OnlineStore_Api.Dtos.Product;

public class ProductDto
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonPropertyName("Title")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("Description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("Details")]
    public string Details { get; set; } = string.Empty;
    [JsonPropertyName("Price")]
    public float Price { get; set; }
    [JsonPropertyName("Category")]
    public CategoryDto Category { get; set; } = default!;
    [JsonPropertyName("Images")]
    public IEnumerable<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
}
