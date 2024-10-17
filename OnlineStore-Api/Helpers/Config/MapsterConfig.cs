using Mapster;
using OnlineStore_Api.Dtos.Product.ProductImage;

namespace OnlineStore_Api.Helpers.Config;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // for nested mapping
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        // testing custom config rules
        //config
        //    .NewConfig<Genre, GenreDto>()
        //    .Map(dest => dest.Description, src => $"{src.Name}", src => src.Name.StartsWith('a'));
    }
    public static void RegisterMappings(FileStorage fileStorage)
    {
        TypeAdapterConfig<ProductImage, ProductImageDto>
            .NewConfig()
            .Map(dest => dest.ImagePath,
                 src => Path.Combine(fileStorage.RelativeDirPaths.Images, src.ImagePath)
                            .Replace('\\', '/'));
    }
}