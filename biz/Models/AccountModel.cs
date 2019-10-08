using App.Biz.Base;
using App.Data.Entities;
using App.Data.Managers;
using FluentValidation;
using MlkPwgen;
using System.Linq;

namespace App.Biz.Models
{
    public class AccountModel : ModelBase<AccountManager>
    {
        public bool NeedsEmailVerification = false;

        public string ID { get; set; }

        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }

        public bool IsEmailVerified { get; set; }

        public void Save()
        {
            CheckIfEmailValidationIsNeeded();

            ID = Manager.Save(
                new Account
                {
                    ID = ID, //if ID is null, a new record will be created. if ID is not null, it will replace existing record in db.
                    Email = EmailAddress.Trim().ToLower(),
                    PasswordHash = Password.ToSaltedHash(),
                    Title = Title,
                    FirstName = FirstName,
                    LastName = LastName,
                    Address = new Address
                    {
                        Street = Street,
                        City = City,
                        State = State,
                        ZipCode = ZipCode,
                        CountryCode = CountryCode
                    }
                },
                NeedsEmailVerification);
        }

        public void Load()
        {
            var ac = Manager.Find(ID);

            EmailAddress = ac.Email;
            Title = ac.Title;
            FirstName = ac.FirstName;
            LastName = ac.LastName;
            Street = ac.Address.Street;
            City = ac.Address.City;
            State = ac.Address.State;
            ZipCode = ac.Address.ZipCode;
            CountryCode = ac.Address.CountryCode;
            IsEmailVerified = ac.IsEmailVerified;
        }

        public bool ValidateEmailAddress(string code)
        {
            return Manager.ValidateEmail(ID, code);
        }

        public void SendVerificationEmail(string baseURL, Settings.Email settings)
        {
            if (NeedsEmailVerification)
            {
                var code = PasswordGenerator.Generate(20);
                Manager.SetEmailValidationCode(code, ID);

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
            if (string.IsNullOrEmpty(ID))
            {
                NeedsEmailVerification = true;
            }
            else if (ID != Manager.Find(a => a.Email == EmailAddress.Trim().ToLower(),
                                        a => a.ID)
                                  .SingleOrDefault())
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

            var idForEmail = Manager.Find(a => a.Email == EmailAddress.Trim().ToLower(),
                                          a => a.ID)
                                    .SingleOrDefault();
            return idForEmail != ID;
        }

        public class Validator : AbstractValidator<AccountModel>
        {
            public Validator()
            {
                RuleFor(a => a.EmailAddress)
                    .NotEmpty().WithMessage("Email address can't be empty")
                    .EmailAddress().WithMessage("Email format is wrong")
                    .Must((a, e) => !a.EmailBelongsToSomeOneElse()).WithMessage("Email address already in use");

                RuleFor(a => a.Password)
                    .Must(p => p.IsAValidPassword()).WithMessage("Password format is not acceptable");

                RuleFor(a => a.FirstName)
                    .NotEmpty().WithMessage("First name can't be empty")
                    .MinimumLength(3).WithMessage("First name must be longer than 3 characters");

                RuleFor(a => a.LastName)
                    .NotEmpty().WithMessage("Last name can't be empty")
                    .MinimumLength(3).WithMessage("Last name must be longer than 3 characters");
            }
        }

        public static class Perms
        {
            public const string Save = "account.perm.save";
            public const string Delete = "account.perm.delete";
        }

        public static class Claims
        {
            public const string ID = "account.claim.id";
        }
    }


}
