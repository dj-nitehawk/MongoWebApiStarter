using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Login
{
    [Route("/account/login")]
    public class Request : IRequest<Response>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
