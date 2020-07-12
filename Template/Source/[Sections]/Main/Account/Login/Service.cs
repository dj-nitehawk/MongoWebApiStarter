using Data;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;
using System.Linq;

namespace Main.Account.Login
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Response>
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

            var permissions = default(Allow).All().ToArray();

            var session = new UserSession((Claim.AccountID, acc.ID));

            Response.SignIn(session, permissions);
            Response.FullName = $"{acc.Title} {acc.FirstName} {acc.LastName}";

            return Response;
        }
    }
}
