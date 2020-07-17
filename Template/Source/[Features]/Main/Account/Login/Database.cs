using MongoDB.Entities;
using MongoWebApiStarter;
using System.Linq;

namespace Main.Account.Login
{
    public class Database : IDatabase
    {
        public Dom.Account GetAccount(string userName)
        {
            return DB.Find<Dom.Account>()
                     .Match(a => a.Email == userName.LowerCase())
                     .Project(a => new Dom.Account
                     {
                         PasswordHash = a.PasswordHash,
                         IsEmailVerified = a.IsEmailVerified,
                         ID = a.ID,
                         Title = a.Title,
                         FirstName = a.FirstName,
                         LastName = a.LastName
                     })
                     .Execute()
                     .SingleOrDefault();
        }
    }
}
