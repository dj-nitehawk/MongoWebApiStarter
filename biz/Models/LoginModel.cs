using App.Biz.Base;
using App.Data.Repos;
using FluentValidation;
using System.Linq;

namespace App.Biz.Models
{
    public class LoginModel : ModelBase<AccountRepo>
    {
        public string AccountID = null;
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string SingIn()
        {
            var acc = Repo.Find(a =>
                        a.Email == UserName.ToLower().Trim(),
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
                if (!BCrypt.Net.BCrypt.Verify(Password, acc.PasswordHash))
                {
                    return "The supplied credentials are invalid. Please try again...";
                }
            }
            else
            {
                return "Sorry, couldn't locate your account...";
            }

            if (!acc.IsEmailVerified)
                return "Please verify your email address before logging in...";

            AccountID = acc.ID;
            FullName = $"{acc.Title}. {acc.FirstName} {acc.LastName}";

            return null;
        }

        public override void Load()
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
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
