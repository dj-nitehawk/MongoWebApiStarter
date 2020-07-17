using MongoDB.Entities;
using MongoWebApiStarter;
using System.Linq;

namespace Main.Account.Verify
{
    public class Database : IDatabase
    {
        public bool ValidateEmail(string accountID, string code)
        {
            var accExists = DB.Queryable<Dom.Account>()
                              .Where(a => a.ID == accountID && a.EmailVerificationCode == code)
                              .Any();

            if (!accExists) return false;

            DB.Update<Dom.Account>()
              .Match(a => a.ID == accountID)
              .Modify(a => a.IsEmailVerified, true)
              .Execute();

            return true;
        }
    }
}
