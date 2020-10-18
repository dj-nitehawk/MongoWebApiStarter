using MongoWebApiStarter;
using ServiceStack;

namespace Image.Delete
{
    [Route("/image/{ID}")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
    }
}
