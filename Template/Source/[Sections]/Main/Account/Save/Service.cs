using Data;
using MlkPwgen;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;

namespace Main.Account.Save
{
    [Authenticate(ApplyTo.Patch)]
    public class Service : Service<Request, Response>
    {
        public bool NeedsEmailVerification;

        public override Response Post(Request r)
        {
            return Patch(r);
        }

        [Need(Claim.AccountID)]
        public override Response Patch(Request r)
        {
            r.ID = User.ClaimValue(Claim.AccountID); //post tampering protection

            CheckIfEmailValidationIsNeeded(r);

            var acc = r.ToEntity();

            if (r.ID.HasValue()) // existing account
            {
                RepoAccount.SavePreserving(acc);
            }
            else // new account
            {
                RepoAccount.Save(acc);
            }

            SendVerificationEmail(acc);

            Response.EmailSent = NeedsEmailVerification;
            Response.ID = acc.ID;

            return Response;
        }

        private void SendVerificationEmail(Data.Account a)
        {
            if (NeedsEmailVerification)
            {
                var code = PasswordGenerator.Generate(20);
                RepoAccount.SetEmailValidationCode(code, a.ID);

                var salutation = $"{a.Title} {a.FirstName} {a.LastName}";

                var email = new MongoWebApiStarter.Models.Email(
                    Settings.Email.FromName,
                    Settings.Email.FromEmail,
                    salutation,
                    a.Email,
                    "Please validate your Virtual Practice account...",
                    EmailTemplates.Email_Address_Validation);

                email.MergeFields.Add("Salutation", salutation);
                email.MergeFields.Add("ValidationLink", $"{BaseURL}#/account/{a.ID}-{code}/validate");

                email.AddToSendingQueue();
            }
        }

        private void CheckIfEmailValidationIsNeeded(Request r)
        {
            if (r.ID.HasNoValue())
            {
                NeedsEmailVerification = true;
            }
            else if (r.ID != RepoAccount.GetID(r.EmailAddress.LowerCase()))
            {
                NeedsEmailVerification = true;
            }
            else
            {
                NeedsEmailVerification = false;
            }
        }
    }
}
