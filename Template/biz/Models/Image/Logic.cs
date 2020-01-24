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
        public async Task LoadAsync()
        {
            LoadFrom(await Repo.GetAsync(ID));
        }

        public async Task SaveAsync()
        {
            using var strInput = File.OpenReadStream();
            using var img = Image.Load(strInput);

            img.Mutate(x =>
                       x.Resize(new ResizeOptions
                       {
                           Mode = ResizeMode.Crop,
                           Size = new Size { Width = Width, Height = Height }
                       }));

            using var strOutput = new MemoryStream();
            img.SaveAsJpeg(strOutput, new JpegEncoder { Quality = 80 });
            FileBytes = strOutput.ToArray();

            ID = await Repo.SaveAsync(ToEntity(this));
        }

        public bool IsAllowedType()
        {
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return allowedTypes.Contains(File?.ContentType.ToLower()); ;
        }

        public bool IsAllowedSize()
        {
            return (File?.Length >= 100 && File?.Length <= 10485760);
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
