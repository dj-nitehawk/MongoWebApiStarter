using MongoDB.Entities;
using MongoWebApiStarter;
using System.Threading.Tasks;

namespace Account.Login
{
    public class Database : IDatabase
    {
        public Task<Dom.Account> GetAccountAsync(string userName)
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
                     .ExecuteSingleAsync();
        }
    }
}
