using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;
using System;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel : IMapper<Image>
    {
        public void LoadFrom(Image img)
        {
            FileBytes = img?.Bytes;
        }

        public Image ToEntity()
        {
            return new Image
            {
                AccessedOn = DateTime.UtcNow,
                Height = Height,
                Width = Width,
                ID = ID,
                Bytes = FileBytes
            };
        }
    }
}
