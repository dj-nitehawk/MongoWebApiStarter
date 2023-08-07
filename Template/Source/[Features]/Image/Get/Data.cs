namespace Image.Get;

internal static class Data
{
    public static Task DownloadAsync(string id, Stream destination)
    {
        return DB.File<Dom.Image>(id)
                 .DownloadWithTimeoutAsync(destination, 30);
    }
}