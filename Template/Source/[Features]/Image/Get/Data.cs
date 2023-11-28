namespace Image.Get;

static class Data
{
    public static Task DownloadAsync(string id, Stream destination)
        => DB.File<Dom.Image>(id)
             .DownloadWithTimeoutAsync(destination, 30);
}