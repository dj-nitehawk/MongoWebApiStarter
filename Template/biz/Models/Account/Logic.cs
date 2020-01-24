using MlkPwgen;
using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class AccountModel : ModelBase<AccountRepo>
    {
        public override void Save()
        {
            var account = ToEntity(this);

            CheckIfEmailValidationIsNeeded();

            if (ID.HasValue()) // it's an existing account being updated
            {
                if (NeedsEmailVerification) // don't replace the two fields below
                {
                    var acc = Repo.Find(
                                ID,
                                a => new
                                {
                                    a.IsEmailVerified,
                                    a.EmailVerificationCode
                                });
                    account.IsEmailVerified = acc.IsEmailVerified;
                    account.EmailVerificationCode = acc.EmailVerificationCode;
                }
            }

            ID = Repo.Save(account);
        }

        public override void Load()
        {
            LoadFrom(Repo.Find(ID));
        }

        public bool ValidateEmailAddress(string code)
        {
            return Repo.ValidateEmail(ID, code);
        }

        public void SendVerificationEmail(string baseURL, Settings.Email settings)
        {
            if (NeedsEmailVerification)
            {
                var code = PasswordGenerator.Generate(20);
                Repo.SetEmailValidationCode(code, ID);

                var salutation = $"{Title} {FirstName} {LastName}";

                var email = new EmailModel(
                    settings.FromName,
                    settings.FromEmail,
                    salutation,
                    EmailAddress,
                    "Please validate your Account...",
                    EmailTemplates.Email_Address_Validation);

                email.MergeFields.Add("Salutation", salutation);
                email.MergeFields.Add("ValidationLink", $"{baseURL}#/account/{ID}-{code}/validate");

                email.AddToSendingQueue();
            }
        }

        public void CheckIfEmailValidationIsNeeded()
        {
            if (ID.HasNoValue())
            {
                NeedsEmailVerification = true;
            }
            else if (ID != Repo.GetID(EmailAddress.Trim().ToLower()))
            {
                NeedsEmailVerification = true;
                IsEmailVerified = false;
            }
            else
            {
                NeedsEmailVerification = false;
                IsEmailVerified = false;
            }
        }

        public bool EmailBelongsToSomeOneElse()
        {
            if (EmailAddress == null) return true;

            var idForEmail = Repo.GetID(EmailAddress.Trim().ToLower());
            return idForEmail != ID;
        }
    }
}
