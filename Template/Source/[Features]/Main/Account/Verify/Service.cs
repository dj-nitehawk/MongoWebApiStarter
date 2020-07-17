using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Verify
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Nothing, Database>
    {
        public override Nothing Get(Request r)
        {
            if (Data.ValidateEmail(r.ID, r.Code))
                return Nothing;

            throw HttpError.BadRequest("Sorry! Could not validate your email address...");
        }
    }
}
