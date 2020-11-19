using MongoWebApiStarter;
using ServiceStack;
using System.Threading.Tasks;

namespace Account.Verify
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Nothing>
    {
        public async Task<Nothing> GetAsync(Request r)
        {
            if (await Data.ValidateEmailAsync(r.ID, r.Code))
                return Nothing;

            throw HttpError.BadRequest("Sorry! Could not validate your email address...");
        }
    }
}
