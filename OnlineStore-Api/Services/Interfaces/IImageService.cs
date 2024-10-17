using OnlineStore_Api.Dtos.Product.ProductImage;

namespace OnlineStore_Api.Services.Interfaces;

public interface IImageService
{
    public Task<ProductImage?> SaveImage(AddProductImageDto imageDto, int productID);
}
