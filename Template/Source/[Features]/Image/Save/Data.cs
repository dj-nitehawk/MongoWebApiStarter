namespace Image.Save;

internal static class Data
{
    public static Task DeleteImageAsync(string id)
    {
        return DB.DeleteAsync<Dom.Image>(id);
    }

    public static async Task<string> UploadAsync(Dom.Image image, Stream stream)
    {
        await image.SaveAsync();
        await image.Data.UploadWithTimeoutAsync(stream, 60, 128);
        return image.ID!;
    }
}