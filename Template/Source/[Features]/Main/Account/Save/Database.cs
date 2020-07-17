using MongoDB.Entities;
using MongoWebApiStarter;
using System.Linq;

namespace Main.Account.Save
{
    public class Database : IDatabase
    {
        public void CreateOrUpdate(Dom.Account acc)
        {
            using var TN = DB.Transaction();

            if (acc.ID.HasValue()) // existing account
            {
                TN.SavePreserving(acc);
            }
            else // new account
            {
                TN.Save(acc);
            }

            TN.Commit();
        }

        public void SetEmailValidationCode(string code, string accoundID)
        {
            DB.Update<Dom.Account>()
              .Match(a => a.ID == accoundID)
              .Modify(a => a.EmailVerificationCode, code)
              .Execute();
        }

        public string GetAccountID(string emailAddress)
        {
            return DB.Find<Dom.Account, string>()
                     .Match(a => a.Email == emailAddress.LowerCase())
                     .Project(a => a.ID)
                     .Execute()
                     .SingleOrDefault();
        }
    }
}
