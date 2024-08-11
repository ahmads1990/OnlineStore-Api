using Mapster;

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
}