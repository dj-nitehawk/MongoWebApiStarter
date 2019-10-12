using FluentValidation;
using Microsoft.AspNetCore.Http;
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
    public class ImageModel : ModelBase<ImageRepo>
    {
        public string ID { get; set; }
        public IFormFile File { get; set; }
        public byte[] FileBytes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public async Task LoadAsync()
        {
            FileBytes = (await Repo.GetAsync(ID))?.Bytes;
        }

        public async Task SaveAsync()
        {
            using (var strInput = File.OpenReadStream())
            {
                using (var img = Image.Load(strInput))
                {
                    img.Mutate(x =>
                               x.Resize(new ResizeOptions
                               {
                                   Mode = ResizeMode.Crop,
                                   Size = new Size { Width = Width, Height = Height }
                               }));

                    using (var strOutput = new MemoryStream())
                    {
                        img.SaveAsJpeg(strOutput, new JpegEncoder { Quality = 80 });

                        var image = new Data.Entities.Image
                        {
                            AccessedOn = DateTime.UtcNow,
                            Height = Height,
                            Width = Width,
                            ID = ID,
                            Bytes = strOutput.ToArray()
                        };

                        ID = await Repo.SaveAsync(image);
                    }
                }
            }
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
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public class Validator : AbstractValidator<ImageModel>
        {
            public Validator()
            {
                RuleFor(i => i.Width)
                    .GreaterThan(10).WithMessage("Image width too small")
                    .LessThan(2000).WithMessage("Image width is too large");

                RuleFor(i => i.Height)
                    .GreaterThan(10).WithMessage("Image height too small")
                    .LessThan(2000).WithMessage("Image width is too large"); ;

                RuleFor(i => i.File)
                    .NotEmpty().WithMessage("Image is required")
                    .Must((e, i) => e.IsAllowedType()).WithMessage("File type not allowed")
                    .Must((e, i) => e.IsAllowedSize()).WithMessage("File size not allowed");
            }
        }

        public static class Perms
        {
            public const string Save = "image.perm.save";
        }
    }


}
