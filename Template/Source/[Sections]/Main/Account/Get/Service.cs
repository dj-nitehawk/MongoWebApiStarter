using Data;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;

namespace Main.Account.Get
{
    public class Service : Service<Request, Response>
    {
        [Permission(Allow.Account_Read)]
        public override Response Get(Request r)
        {
            var acc = RepoAccount.FindExcluding(
                r.ID,
                a => new
                {
                    a.PasswordHash,
                    a.EmailVerificationCode
                });

            if (acc == null) throw HttpError.NotFound("Unable to find the right Account!");

            Response.FromEntity(acc);

            return Response;
        }
    }
}
