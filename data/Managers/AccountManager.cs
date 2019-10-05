using MongoDB.Entities;
using App.Data.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Data.Managers
{
    public class AccountManager
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

        public Account Find(string id)
        {
            return DB.Find<Account>().One(id);
        }

        public Account Find(string email, string password)
        {
            var acc = DB.Find<Account>()
                        .Match(a => a.Email == email)
                        .Project(x => new Account
                        {
                            ID = x.ID,
                            PasswordHash = x.PasswordHash,
                            IsEmailVerified = x.IsEmailVerified,
                            Title = x.Title,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                        })
                        .Execute()
                        .SingleOrDefault();

            if (acc != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, acc.PasswordHash))
                {
                    return acc;
                }
            }

            return null;
        }

        public string GetID(string email)
        {
            return DB.Queryable<Account>()
                     .Where(a => a.Email == email)
                     .Select(a => a.ID)
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
            var vpExists = DB.Queryable<Account>()
                             .Where(a => a.ID == id && a.EmailVerificationCode == code)
                             .Any();

            if (!vpExists) return false;

            DB.Update<Account>()
              .Match(a => a.ID == id)
              .Modify(a => a.IsEmailVerified, true)
              .Execute();

            return true;
        }

        public Account[] GetAccounts(Expression<Func<Account, Account>> projection, int skip, int take)
        {
            return DB.Queryable<Account>()
                     .Skip(skip)
                     .Take(take)
                     .Select(projection).ToArray();
        }

        public int UnverifiedCount()
        {
            return DB.Queryable<Account>()
                     .Count(a => a.IsEmailVerified == false);
        }

        public int TotalCount()
        {
            return DB.Queryable<Account>()
                     .Count();
        }
    }
}
