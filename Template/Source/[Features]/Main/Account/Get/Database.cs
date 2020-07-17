using MongoDB.Entities;
using MongoWebApiStarter;
using System.Linq;

namespace Main.Account.Get
{
    public class Database : IDatabase
    {
        public Dom.Account GetAccount(string accountID)
        {
            return DB.Find<Dom.Account>()
                     .Match(a => a.ID == accountID)
                     .ProjectExcluding(a => new
                     {
                         a.PasswordHash,
                         a.EmailVerificationCode
                     })
                     .Execute()
                     .SingleOrDefault();
        }
    }
}
