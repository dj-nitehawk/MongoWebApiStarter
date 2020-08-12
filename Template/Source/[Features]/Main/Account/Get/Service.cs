using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;

namespace Main.Account.Get
{
    public class Service : Service<Request, Response, Database>
    {
        [Permission(Allow.Account_Read)]
        public Response Get(Request r)
        {
            var acc = Data.GetAccount(r.ID);

            if (acc == null)
                throw HttpError.NotFound("Unable to find the right Account!");

            Response.FromEntity(acc);

            return Response;
        }
    }
}
