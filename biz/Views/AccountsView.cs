using App.Data.Entities;
using App.Data.Managers;
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

        AccountManager manager = new AccountManager();

        public AccountsView Load()
        {
            AccountList = manager.GetAccounts(a =>
                                    new AccountDetailView
                                    {
                                        ID = a.ID,
                                        FirstName = a.FirstName,
                                        LastName = a.LastName,
                                        EmailValidated = a.IsEmailVerified
                                    }, 
                                    0, 100).ToArray();

            TotalAccounts = manager.TotalCount();
            UnverifiedAccounts = manager.UnverifiedCount();

            return this;
        }

        public static class Perms
        {
            public const string View = "accounts.perm.view";
            public const string Delete = "accounts.perm.delete";
        }
    }

}
