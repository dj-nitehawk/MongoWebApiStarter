using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using MongoWebApiStarter;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Verify
{
    public class Database : IDatabase
    {
        public async Task<bool> ValidateEmailAsync(string accountID, string code)
        {
            var accExists = await DB.Queryable<Dom.Account>()
                                    .Where(a => a.ID == accountID && a.EmailVerificationCode == code)
                                    .AnyAsync();

            if (!accExists) return false;

            await DB.Update<Dom.Account>()
                    .Match(a => a.ID == accountID)
                    .Modify(a => a.IsEmailVerified, true)
                    .ExecuteAsync();

            return true;
        }
    }
}
