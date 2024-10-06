namespace OnlineStore_Api.Repositories.Interfaces;

public interface IImageRepo
{
    public Task<ProductImage> SaveImage(IFormFile imageFile, byte order, int productID);
}
public interface IImageProcessor
{
    public Task<string> SaveImage(IFormFile imageFile);
}
