namespace OnlineStore_Api.Dtos;

public class AddProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public float Price { get; set; }

    // Relations
    public AddCategoryDto CategoryDto { get; set; } = default!;
    public IEnumerable<IFormFile> ProductImages { get; set; } = new List<IFormFile>();
}
