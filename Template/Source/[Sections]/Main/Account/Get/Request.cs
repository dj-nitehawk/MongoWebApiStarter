using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Get
{
    [Route("/account/{ID}", "GET")]
    public class Request : IRequest<Response>
    {
        public string ID { get; set; }
    }
}
