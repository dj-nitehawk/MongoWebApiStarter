using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Save
{
    [Route("/account")]
    public class Request : Model, IRequest<Response>
    { }
}
