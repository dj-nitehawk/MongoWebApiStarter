using MongoWebApiStarter;
using ServiceStack;
using System;

namespace Main.Image.Save
{
    [Route("/image")]
    public class Request : IRequest<Data.Image, Nothing>
    {
        public string ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Data.Image ToEntity()
        {
            return new Data.Image
            {
                AccessedOn = DateTime.UtcNow,
                Height = Height,
                Width = Width,
                ID = ID
            };
        }
    }
}
