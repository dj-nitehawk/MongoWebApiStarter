using MongoDB.Entities;
using MongoWebApiStarter;
using System.Threading.Tasks;

namespace Account.Save
{
    public static class Data
    {
        public static async Task CreateOrUpdateAsync(Dom.Account acc)
        {
            using var TN = DB.Transaction();

            if (acc.ID.HasValue()) // existing account
            {
                await TN.SavePreservingAsync(acc);
            }
            else // new account
            {
                await TN.SaveAsync(acc);
            }

            await TN.CommitAsync();
        }

        public static Task SetEmailValidationCodeAsync(string code, string accoundID)
        {
            return DB.Update<Dom.Account>()
                     .Match(a => a.ID == accoundID)
                     .Modify(a => a.EmailVerificationCode, code)
                     .ExecuteAsync();
        }

        public static Task<string> GetAccountIDAsync(string emailAddress)
        {
            return DB.Find<Dom.Account, string>()
                     .Match(a => a.Email == emailAddress.LowerCase())
                     .Project(a => a.ID)
                     .ExecuteSingleAsync();
        }
    }
}
