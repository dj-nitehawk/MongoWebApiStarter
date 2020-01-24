namespace MongoWebApiStarter.Biz.Views
{
    public partial class AccountsView
    {
        public int TotalAccounts { get; set; }
        public int UnverifiedAccounts { get; set; }
        public AccountDetailView[] AccountList { get; set; }
    }

    public class AccountDetailView
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailValidated { get; set; }
    }
}
