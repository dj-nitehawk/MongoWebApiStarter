using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;

namespace Account.Get
{
    [Route("/account/{ID}")]
    public class Request : IRequest<Response>
    {
        public string ID { get; set; }

        [From(Claim.AccountID)]
        public string AccountID { get; set; }
    }
}
