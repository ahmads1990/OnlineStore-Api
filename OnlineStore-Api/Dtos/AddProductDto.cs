namespace OnlineStore_Api.Dtos;

public class AddProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public float Price { get; set; }

    // Relations
    public int CategoryId { get; set; }
    public IEnumerable<AddProductImageDto> ProductImageDtos { get; set; } = new List<AddProductImageDto>();
}
