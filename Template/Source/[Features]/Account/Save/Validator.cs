using MongoWebApiStarter;
using ServiceStack.FluentValidation;
using System.Threading.Tasks;

namespace Account.Save
{
    public class Validator : AbstractValidator<Request>
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
}
