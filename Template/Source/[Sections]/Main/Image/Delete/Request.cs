using MongoWebApiStarter;
using ServiceStack;

namespace Main.Image.Delete
{
    [Route("/image/{ID}", "DELETE")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
    }
}
