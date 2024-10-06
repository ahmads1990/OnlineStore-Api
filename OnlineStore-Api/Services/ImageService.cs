using OnlineStore_Api.Dtos;
using System.Linq;

namespace OnlineStore_Api.Services;

public class ImageService : IImageService
{
    private readonly IImageRepo _imageRepo;

    public ImageService(IImageRepo imageRepo)
    {
        _imageRepo = imageRepo;
    }

    public async Task<ProductImage?> SaveImage(AddProductImageDto imageDto, int productID)
    {
        // Check order > 0

        // Check file size
        float fileSizeInMB = imageDto.ImageFile.Length / 1000000;
        if (fileSizeInMB > 0 || fileSizeInMB > 2)
            throw new ArgumentException($"Invalid file size:{fileSizeInMB} max size is 2");

        string fileExtension = Path.GetExtension(imageDto.ImageFile.FileName);
        List<string> allowedExtensions = [".png", ".jpeg", ".jpg"];

        if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
            throw new ArgumentException($"Invalid file extension:{fileExtension} allowed only {string.Join(',', allowedExtensions)}");

        var productImage = await _imageRepo.SaveImage(imageDto.ImageFile, imageDto.Order, productID);
        if (string.IsNullOrEmpty(productImage.ImagePath))
            throw new FileLoadException("Server error couldn't save file");

        return productImage;
    }
}
