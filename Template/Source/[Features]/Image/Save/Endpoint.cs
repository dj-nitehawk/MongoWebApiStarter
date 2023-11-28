using MongoWebApiStarter;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Image.Save;

sealed class Endpoint : Endpoint<Request, object, Mapper>
{
    public override void Configure()
    {
        Verbs(Http.POST, Http.PUT);
        AllowAnonymous(Http.POST);
        Routes("/image");
        AllowFileUploads();
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        if (r.ID.HasValue())
            _ = Data.DeleteImageAsync(r.ID!); //delete old image (cause of cloudflare caching) and nullify image ID so a new ID will be set

        r.ID = null;

        using var img = await SixLabors.ImageSharp.Image.LoadAsync(r.File.OpenReadStream(), ct);
        img.Mutate(
            x => x.Resize(
                new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new() { Width = r.Width, Height = r.Height }
                }));
        using var memStream = new MemoryStream();
        await img.SaveAsJpegAsync(memStream, new() { Quality = 80 }, ct);

        var newID = await Data.UploadAsync(Map.ToEntity(r), memStream);

        await SendAsync(new { ImageID = newID });
    }
}