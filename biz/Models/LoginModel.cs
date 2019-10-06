using App.Data.Managers;
using FluentValidation;

namespace App.Biz.Models
{
    public class LoginModel
    {
        private AccountManager manager = new AccountManager();

        public string AccountID = null;

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public int DaysToRemember { get; set; }

        public string SingIn()
        {
            var acc = manager.Find(UserName.ToLower().Trim(), Password);

            if (acc == null) return "The supplied credentials are invalid. Please try again...";

            if (!acc.IsEmailVerified) return "Please verify your email address before logging in...";

            AccountID = acc.ID;

            return null;
        }

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
