using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Save
{
    [Route("/account", "POST,PATCH")]
    public class Request : Model, IRequest<Response>
    { }
}
