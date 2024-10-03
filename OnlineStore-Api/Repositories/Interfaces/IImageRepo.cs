namespace OnlineStore_Api.Repositories.Interfaces;

public interface IImageRepo
{
    public Task<string> SaveImage(IFormFile imageFile);
}
public interface IImageProcessor
{
    public Task<string> SaveImage(IFormFile imageFile);
}
