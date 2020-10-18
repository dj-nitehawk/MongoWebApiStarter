using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;
using System.Threading.Tasks;

namespace Account.Get
{
    public class Service : Service<Request, Response, Database>
    {
        [
            Permission(Allow.Account_Read)
        ]
        public async Task<Response> Get(Request r)
        {
            var acc = await Data.GetAccountAsync(r.ID);

            if (acc == null)
                throw HttpError.NotFound("Unable to find the right Account!");

            Response.FromEntity(acc);

            return Response;
        }
    }
}
