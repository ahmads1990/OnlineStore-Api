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

    public async Task<string> SaveImage(IFormFile imageFile)
    {
        // Try to save image
        try
        {
            var imageRelativePath = await _imageProcessor.SaveImage(imageFile);
            if (string.IsNullOrEmpty(imageRelativePath)) {
            }

            
        }
        catch (Exception)
        {

            throw;
        }
        return string.Empty;
    }
}
public class ImageProcessLocal : IImageProcessor
{
    private readonly IWebHostEnvironment _webHostEnv;
    private readonly LocalFileSettings _localFileSettings;

    public ImageProcessLocal(IWebHostEnvironment webHostEnv, IOptions<LocalFileSettings> localFileSettings)
    {
        _webHostEnv = webHostEnv;
        _localFileSettings = localFileSettings.Value;
    }
    public async Task<string> SaveImage(IFormFile imageFile)
    {
        var imageSaveDir = Path.Combine(_webHostEnv.WebRootPath, _localFileSettings.ImageFileDir);
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
