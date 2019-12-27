using FluentValidation;
using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MongoWebApiStarter.Biz.Models
{
    public class LoginModel : ModelBase<AccountRepo>
    {
        public string AccountID = null;
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

        public void SingIn()
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
                    AddError("The supplied credentials are invalid.Please try again...");
                    return;
                }
            }
            else
            {
                AddError("Sorry, couldn't locate your account...");
                return;
            }

            if (!acc.IsEmailVerified)
            {
                AddError("Please verify your email address before logging in...");
                return;
            }

            AccountID = acc.ID;
            FullName = $"{acc.Title}. {acc.FirstName} {acc.LastName}";
            Claims.Add(AccountModel.Claims.ID, AccountID);
            Claims.Add(AccountModel.Claims.Email, UserName);
            Claims.Add(Auth.Claims.Role, Auth.Roles.Owner);
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
