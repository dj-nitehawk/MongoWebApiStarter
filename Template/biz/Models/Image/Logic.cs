using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel : ModelBase<ImageRepo>
    {
        public async Task SaveAsync()
        {
            if (ID.HasValue())
            {
                //delete old image (cause of cloudflare caching) and nullify image ID so a new ID will be set
                _ = Repo.DeleteAsync(ID);
                ID = null;
            }

            using var strInput = File.OpenReadStream();
            using var img = Image.Load(strInput);
            img.Mutate(
                x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = new Size { Width = Width, Height = Height }
                }));
            using var strOutput = new MemoryStream();
            img.SaveAsJpeg(strOutput, new JpegEncoder { Quality = 80 });

            ID = await Repo.UploadAsync(ToEntity(), strOutput);
        }

        public Task WriteImageData(Stream stream)
        {
            return Repo.DownloadAsync(ID, stream);
        }

        public bool IsAllowedType()
        {
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return allowedTypes.Contains(File?.ContentType.ToLower()); ;
        }

        public bool IsAllowedSize()
        {
            return File?.Length >= 100 && File?.Length <= 10485760;
        }

        #region unused
        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
