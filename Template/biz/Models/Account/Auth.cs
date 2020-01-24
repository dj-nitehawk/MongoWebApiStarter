namespace MongoWebApiStarter.Biz.Models
{
    public partial class AccountModel
    {
        public static class Perms
        {
            public const string Full = "perm.account.full";
            public const string Save = "perm.account.save";
            public const string Delete = "perm.account.delete";
        }

        public static class Claims
        {
            public const string ID = "claim.account.id";
            public const string Email = "claim.account.email";
        }
    }
}
