using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using System.Linq;

namespace MongoWebApiStarter.Biz.Views
{
    public partial class AccountsView : ViewBase<AccountRepo>
    {
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
    }
}
