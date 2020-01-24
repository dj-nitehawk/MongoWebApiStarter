using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;
using System;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel : IMapper<ImageModel, Image>
    {
        public void LoadFrom(Image img)
        {
            FileBytes = img?.Bytes;
        }

        public Image ToEntity(ImageModel m)
        {
            return new Image
            {
                AccessedOn = DateTime.UtcNow,
                Height = m.Height,
                Width = m.Width,
                ID = m.ID,
                Bytes = m.FileBytes
            };
        }
    }
}
