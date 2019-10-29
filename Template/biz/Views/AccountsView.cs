using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using System.Linq;

namespace MongoWebApiStarter.Biz.Views
{
    public class AccountsView : ViewBase<AccountRepo>
    {
        public int TotalAccounts { get; set; }
        public int UnverifiedAccounts { get; set; }
        public AccountDetailView[] AccountList { get; set; }

        public override void Load()
        {
            AccountList = Repo.GetAccounts(a =>
                            new AccountDetailView
                            {
                                ID = a.ID,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                EmailValidated = a.IsEmailVerified
                            },
                            0, 100).ToArray();

            var stats = Repo.GetStats();

            TotalAccounts = stats.TotalCount;
            UnverifiedAccounts = stats.UnverifiedCount;
        }

        public static class Perms
        {
            public const string View = "perm.accounts.view";
            public const string Delete = "perm.accounts.delete";
        }
    }

    public class AccountDetailView
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailValidated { get; set; }
    }
}
