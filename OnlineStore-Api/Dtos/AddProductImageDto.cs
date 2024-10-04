namespace OnlineStore_Api.Dtos;

public class AddProductImageDto
{
    public byte Order { get; set; }
    public IFormFile ImageFile { get; set; } = default!;
}
