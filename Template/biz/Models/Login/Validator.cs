using FluentValidation;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class LoginModel
    {
        public class Validator : AbstractValidator<LoginModel>
        {
            public Validator()
            {
                RuleFor(a => a.UserName)
                    .NotEmpty().WithMessage("Username can't be empty")
                    .EmailAddress().WithMessage("Username format is wrong");

                RuleFor(a => a.Password)
                    .Must(p => p.IsAValidPassword()).WithMessage("Password format is not acceptable");
            }
        }
    }
}
