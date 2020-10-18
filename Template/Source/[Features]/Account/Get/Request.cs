using MongoWebApiStarter;
using ServiceStack;

namespace Account.Get
{
    [Route("/account/{ID}")]
    public class Request : IRequest<Response>
    {
        public string ID { get; set; }
    }
}
