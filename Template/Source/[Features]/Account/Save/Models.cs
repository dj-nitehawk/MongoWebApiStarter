using Dom;
using FastEndpoints;
using FastEndpoints.Validation;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;

namespace Account.Save
{
    public class Request : Model, IRequest<Dom.Account>
    {
        [From(Claim.AccountID, IsRequired = false)]
        public string AccountID { get; set; }

        public Dom.Account ToEntity() => new()
        {
            ID = ID,
            Email = EmailAddress.LowerCase(),
            PasswordHash = Password.SaltedHash(),
            Title = Title,
            FirstName = FirstName.TitleCase(),
            LastName = LastName.TitleCase(),
            Address = new Address
            {
                Street = Street.TitleCase(),
                City = City,
                State = State,
                ZipCode = ZipCode,
                CountryCode = CountryCode
            },
            Mobile = Mobile,
        };
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email address can't be empty")
                .EmailAddress().WithMessage("Email format is wrong")
                .MustAsync(async (x, _, __) => !await EmailBelongsToSomeOneElseAsync(x)).WithMessage("Email address already in use");

            RuleFor(a => a.Password)
                .Must(p => p.IsAValidPassword()).WithMessage("Password format is not acceptable");

            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("First name can't be empty")
                .MinimumLength(3).WithMessage("First name must be longer than 3 characters");

            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("Last name can't be empty")
                .MinimumLength(3).WithMessage("Last name must be longer than 3 characters");

            RuleFor(a => a.Mobile)
                .NotEmpty().WithMessage("Mobile number can't be empty")
                .Length(10).WithMessage("Mobile number must be 10 digits");
        }

        private static async Task<bool> EmailBelongsToSomeOneElseAsync(Request r)
        {
            if (r.EmailAddress == null) return true;

            var idForEmail = await Data.GetAccountIDAsync(r.EmailAddress);
            return idForEmail != r.ID;
        }
    }

    public class Response
    {
        public string ID { get; set; }
        public bool EmailSent { get; set; }
    }
}
