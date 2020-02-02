using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;
using System;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel : IMapper<Image>
    {
        public void LoadFrom(Image i)
        {
            ID = i.ID;
            Height = i.Height;
            Width = i.Width;
        }

        public Image ToEntity()
        {
            return new Image
            {
                AccessedOn = DateTime.UtcNow,
                Height = Height,
                Width = Width,
                ID = ID
            };
        }
    }
}
