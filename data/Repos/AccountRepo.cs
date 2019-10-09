using App.Data.Base;
using App.Data.Entities;
using App.Data.Views;
using MongoDB.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Data.Repos
{
    public class AccountRepo : RepoBase<Account>
    {
        public string Save(Account account, bool pendingEmailValidation)
        {
            if (account.IsNotNew()) // existing account
            {
                var acc = DB.Find<Account>()
                            .Match(a => a.ID == account.ID)
                            .Project(a => new Account
                            {
                                IsEmailVerified = a.IsEmailVerified,
                                EmailVerificationCode = a.EmailVerificationCode,
                            })
                            .Execute()
                            .Single();

                if (pendingEmailValidation)
                {
                    account.IsEmailVerified = false;
                    account.EmailVerificationCode = null;
                }
            }
            else // new account
            {
                // do something with new accounts here
            }

            account.Save();
            return account.ID;
        }

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
