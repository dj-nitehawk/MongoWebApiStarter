using FastEndpoints;
using MongoWebApiStarter;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Image.Save
{
    public class Endpoint : Endpoint<Request, object>
    {
        public Endpoint()
        {
            Verbs(Http.POST, Http.PUT);
            Routes("/image");
            AllowFileUploads();
            AllowAnnonymous();
        }

        protected override async Task HandleAsync(Request r, CancellationToken ct)
        {
            if (HttpMethod is Http.PUT && User?.Identity?.IsAuthenticated is false)
            {
                await SendUnauthorizedAsync();
                return;
            }

            var file = Files[0];

            if (file is null)
                ThrowError("No file data was detected in the request!");

            if (!IsAllowedSize(file.Length))
                AddError("The file size is not acceptable!");

            if (!IsAllowedType(file.ContentType))
                AddError("Only JPG or PNG format is allowed!");

            ThrowIfAnyErrors();

            if (r.ID.HasValue())
                _ = Data.DeleteImageAsync(r.ID); //delete old image (cause of cloudflare caching) and nullify image ID so a new ID will be set

            r.ID = null;

            using var img = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
            img.Mutate(
                x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size { Width = r.Width, Height = r.Height }
                }));
            using var memStream = new MemoryStream();
            img.SaveAsJpeg(memStream, new JpegEncoder { Quality = 80 });

            var newID = await Data.UploadAsync(r.ToEntity(), memStream);

            await SendAsync(new { ImageID = newID });
        }

        public bool IsAllowedType(string contentType)
        {
            return (new[]
            {
                "image/jpeg",
                "image/png"
            })
            .Contains(contentType.ToLower());
        }

        public bool IsAllowedSize(long fileLength) =>
            fileLength >= 100 && fileLength <= 10485760;
    }
}
