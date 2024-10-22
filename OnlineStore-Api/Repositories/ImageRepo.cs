using Microsoft.Extensions.Options;
using OnlineStore_Api.Helpers.Config;

namespace OnlineStore_Api.Repositories;

public class ImageRepo : IImageRepo
{
    private  AppDbContext _context { get; set; }
    private  IImageProcessor _imageProcessor { get; set; }
    public ImageRepo(AppDbContext context, IImageProcessor imageProcessor)
    {
        _context = context;
        _imageProcessor = imageProcessor;
    }

    public async Task<ProductImage?> SaveImage(IFormFile imageFile, byte order, int productID)
    {
        ProductImage? newProductImage = null;
        try
        {
            // Try to save image
            var imageRelativePath = await _imageProcessor.SaveImage(imageFile);
            if (string.IsNullOrEmpty(imageRelativePath))
            {
            }
            newProductImage = new ProductImage
            {
                ProductID = productID,
                ImagePath = imageRelativePath,
                CreatedAt = DateTime.Now,
                Order = order
            };
            await _context.ProductImages.AddAsync(newProductImage);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }
        return newProductImage;
    }
}
public class ImageProcessLocal : IImageProcessor
{
    private readonly IWebHostEnvironment _webHostEnv;
    private readonly FileStorage _fileStorage;

    public ImageProcessLocal(IWebHostEnvironment webHostEnv, IOptions<FileStorage> fileStorage)
    {
        _webHostEnv = webHostEnv;
        _fileStorage = fileStorage.Value;
    }
    public async Task<string> SaveImage(IFormFile imageFile)
    {
        var imageSaveDir = Path.Combine(_webHostEnv.WebRootPath, _fileStorage.RelativeDirPaths.Images);
        var newImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        var filePath = Path.Combine(imageSaveDir, newImageFilename);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }

        return newImageFilename;
    }
}
