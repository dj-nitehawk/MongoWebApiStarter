using MongoWebApiStarter;
using ServiceStack;

namespace Main.Image.Save
{
    [Route("/image", "POST,PATCH")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
