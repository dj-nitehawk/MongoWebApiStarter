using MongoDB.Entities;
using MongoWebApiStarter.Data.Base;
using MongoWebApiStarter.Data.Entities;
using MongoWebApiStarter.Data.Views;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MongoWebApiStarter.Data.Repos
{
    public class AccountRepo : RepoBase<Account>
    {
        public string GetID(string email)
        {
            return DB.Find<Account, string>()
                     .Match(a => a.Email == email)
                     .Project(a => a.ID)
                     .Execute()
                     .SingleOrDefault();
        }

        public void SetEmailValidationCode(string code, string id)
        {
            DB.Update<Account>()
              .Match(a => a.ID == id)
              .Modify(a => a.EmailVerificationCode, code)
              .Execute();
        }

        public bool ValidateEmail(string id, string code)
        {
            var acc = DB.Queryable<Account>()
                        .Where(a => a.ID == id && a.EmailVerificationCode == code)
                        .Any();

            if (!acc) return false;

            DB.Update<Account>()
              .Match(a => a.ID == id)
              .Modify(a => a.IsEmailVerified, true)
              .Execute();

            return true;
        }

        public TResult[] GetAccounts<TResult>(Expression<Func<Account, TResult>> projection, int skip, int take)
        {
            return DB.Queryable<Account>()
                     .Skip(skip)
                     .Take(take)
                     .Select(projection)
                     .ToArray();
        }

        public AccountStats GetStats()
        {
            var groups = DB.Queryable<Account>()
                           .GroupBy(a => a.IsEmailVerified)
                           .Select(g => new { Verified = g.Key, Count = g.Count() })
                           .ToArray();

            return new AccountStats
            {
                UnverifiedCount = groups.Where(g => g.Verified == false).Count(),
                TotalCount = groups.Sum(g => g.Count)
            };
        }

    }
}
