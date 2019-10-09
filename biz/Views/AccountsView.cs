using App.Data.Repos;
using System.Linq;

namespace App.Biz.Views
{
    public class AccountDetailView
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailValidated { get; set; }
    }

    public class AccountsView
    {
        public int TotalAccounts { get; set; }
        public int UnverifiedAccounts { get; set; }
        public AccountDetailView[] AccountList { get; set; }

        AccountRepo repo = new AccountRepo();

        public AccountsView Load()
        {
            AccountList = repo.GetAccounts(a =>
                            new AccountDetailView
                            {
                                ID = a.ID,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                EmailValidated = a.IsEmailVerified
                            },
                            0, 100).ToArray();

            var stats = repo.GetStats();

            TotalAccounts = stats.TotalCount;
            UnverifiedAccounts = stats.UnverifiedCount;

            return this;
        }

        public static class Perms
        {
            public const string View = "accounts.perm.view";
            public const string Delete = "accounts.perm.delete";
        }
    }

}
