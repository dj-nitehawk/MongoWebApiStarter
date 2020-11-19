using MongoDB.Entities;
using System.Threading.Tasks;

namespace Account.Get
{
    public static class Data
    {
        public static Task<Dom.Account> GetAccountAsync(string accountID)
        {
            return DB.Find<Dom.Account>()
                     .Match(a => a.ID == accountID)
                     .ProjectExcluding(a => new
                     {
                         a.PasswordHash,
                         a.EmailVerificationCode
                     })
                     .ExecuteSingleAsync();
        }
    }
}
