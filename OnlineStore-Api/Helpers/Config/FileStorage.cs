namespace OnlineStore_Api.Helpers.Config;

public class FileStorage
{
    public RelativePathOptions RelativeDirPaths { get; set; } = new();
}

public class RelativePathOptions
{
    public string Images { get; set; } = string.Empty;
}