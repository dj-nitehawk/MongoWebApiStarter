using MongoWebApiStarter.Biz.Auth;
using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using System.Linq;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class LoginModel : ModelBase<AccountRepo>
    {
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

            Claims.Add((AccountModel.Claims.ID, AccountID));
            Claims.Add((AccountModel.Claims.Email, UserName));
            Claims.Add((Auth.Claims.Role, Roles.Admin));
            Claims.Add((Auth.Claims.Role, Roles.Owner));
        }

        #region unused
        public override void Load()
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
