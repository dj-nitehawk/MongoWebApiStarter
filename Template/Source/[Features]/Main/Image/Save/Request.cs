using MongoWebApiStarter;
using ServiceStack;
using System;

namespace Main.Image.Save
{
    [Route("/image")]
    public class Request : IRequest<Dom.Image, Nothing>
    {
        public string ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Dom.Image ToEntity()
        {
            return new Dom.Image
            {
                Height = Height,
                Width = Width,
                ID = ID
            };
        }
    }
}
