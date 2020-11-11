using MongoWebApiStarter;
using ServiceStack;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Save
{
    [Authenticate(ApplyTo.Patch)]
    public class Service : Service<Request, Nothing, Database>
    {
        public Task<string> PostAsync(Request r) => PatchAsync(r);

        public async Task<string> PatchAsync(Request r)
        {
            var file = Request.Files.FirstOrDefault();

            if (file == null)
                ThrowError("No file data was detected in the request!");

            if (!IsAllowedSize(file.ContentLength))
                AddError("The file size is not acceptable!");

            if (!IsAllowedType(file.ContentType))
                AddError("Only JPG or PNG format is allowed!");

            ThrowIfAnyErrors();

            if (r.ID.HasValue())
                _ = Data.DeleteImageAsync(r.ID); //delete old image (cause of cloudflare caching) and nullify image ID so a new ID will be set

            r.ID = null;

            using var strInput = file.InputStream;
            using var img = SixLabors.ImageSharp.Image.Load(strInput);
            img.Mutate(
                x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size { Width = r.Width, Height = r.Height }
                }));
            using var strOutput = new MemoryStream();
            img.SaveAsJpeg(strOutput, new JpegEncoder { Quality = 80 });

            return await Data.UploadAsync(r.ToEntity(), strOutput);
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
