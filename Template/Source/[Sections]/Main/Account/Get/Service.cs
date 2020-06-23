using Data;
using SCVault;
using SCVault.Auth;
using ServiceStack;

namespace Main.Account.Get
{
    public class Service : Service<Request, Response, Data.Account>
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

            Response = ToResponse(acc);

            Response.VPName = RepoVP.Find(acc.VirtualPractice.ID, v => v.Name);

            return Response;
        }


    }
}
