using App.Data.Entities;
using App.Data.Managers;
using FluentValidation;
using MlkPwgen;

namespace App.Biz.Models
{
    public class AccountModel
    {
        private readonly AccountManager acManager = new AccountManager();

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

            ID = acManager.Save(
                new Account
                {
                    ID = ID, //if ID is null, a new record will be created. if ID is not null, it will update existing record in db.
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

        public bool ValidateEmailAddress(string code)
        {
            return acManager.ValidateEmail(ID, code);
        }

        public void Load()
        {
            var ac = acManager.Find(ID);
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

        public void SendVerificationEmail(string baseURL, string fromName, string fromEmail)
        {
            if (NeedsEmailVerification)
            {
                var code = PasswordGenerator.Generate(20);
                acManager.SetEmailValidationCode(code, ID);

                var salutation = $"{Title} {FirstName} {LastName}";

                var email = new EmailModel(
                    fromName,
                    fromEmail,
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
            else if (ID != acManager.GetID(EmailAddress.Trim().ToLower()))
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

            var idForEmail = acManager.GetID(EmailAddress.Trim().ToLower());
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
