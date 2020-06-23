using Data;
using SCVault;
using ServiceStack;

namespace Main.Account.Save
{
    [Route("/api/account")]
    public class Request : Model, IRequest<Response>
    {
        public bool EmailBelongsToSomeOneElse()
        {
            if (EmailAddress == null) return true;

            var idForEmail = RepoAccount.GetID(EmailAddress.LowerCase());
            return idForEmail != ID;
        }
    }
}
