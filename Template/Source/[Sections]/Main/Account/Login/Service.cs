using Data;
using SCVault;
using SCVault.Auth;
using ServiceStack;
using System.Linq;

namespace Main.Account.Login
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Response, Data.Account>
    {
        public override Response Post(Request r)
        {
            var acc = RepoAccount.Find(a =>
            a.Email == r.UserName.LowerCase(),
            a => new
            {
                a.PasswordHash,
                a.IsEmailVerified,
                a.ID,
                a.Title,
                a.FirstName,
                a.LastName
            }).SingleOrDefault();

            if (acc != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(r.Password, acc.PasswordHash))
                    AddError("The supplied credentials are invalid. Please try again...");
            }
            else
            {
                AddError("Sorry, couldn't locate your account...");
            }

            ThrowIfAnyErrors();

            if (!acc.IsEmailVerified)
                AddError("Please verify your email address before logging in...");

            ThrowIfAnyErrors();

            var vp = RepoVP.Find(v =>
                        v.OwnerAccount.ID == acc.ID,
                        v => new
                        {
                            v.ID,
                            v.Name,
                            v.CreationComplete,
                            v.SubDomain
                        }).Single();

            var permissions = default(Allow).All().Except(new[]
                    {
                        Allow.Consultation_Update_Nurse_Notes //todo: fix this hack to let owner update consultation
                    }).ToArray();

            var session = new UserSession(
                (Claim.AccountID, acc.ID),
                (Claim.VirtualPractiseID, vp.ID));

            Response.SignIn(session, permissions);
            Response.OwnerName = $"{acc.Title} {acc.FirstName} {acc.LastName}";
            Response.VPCreationDone = vp.CreationComplete;
            Response.VPName = vp.Name;
            Response.VPSubdomain = vp.SubDomain;

            return Response;
        }
    }
}
