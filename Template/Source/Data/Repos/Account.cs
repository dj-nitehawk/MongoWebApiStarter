using MongoDB.Entities;
using System.Linq;

namespace Data
{
    public class RepoAccount : RepoBase<Account>
    {
        static RepoAccount()
        {
            DB.Index<Account>()
              .Key(a => a.Email, KeyType.Ascending)
              .Option(o => o.Unique = true)
              .CreateAsync();

            DB.Index<Account>()
              .Key(a => a.EmailVerificationCode, KeyType.Ascending)
              .CreateAsync();
        }

        public static string GetID(string email)
        {
            return DB.Queryable<Account>()
                     .Where(a => a.Email == email)
                     .Select(a => a.ID)
                     .SingleOrDefault();
        }

        public static void SetEmailValidationCode(string code, string id)
        {
            DB.Update<Account>()
              .Match(a => a.ID == id)
              .Modify(a => a.EmailVerificationCode, code)
              .Execute();
        }

        public static bool ValidateEmail(string id, string code)
        {
            var accExists = DB.Queryable<Account>()
                              .Where(a => a.ID == id && a.EmailVerificationCode == code)
                              .Any();

            if (!accExists) return false;

            DB.Update<Account>()
              .Match(a => a.ID == id)
              .Modify(a => a.IsEmailVerified, true)
              .Execute();

            return true;
        }
    }
}
