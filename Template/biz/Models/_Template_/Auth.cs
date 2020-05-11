namespace MongoWebApiStarter.Biz.Models
{
    public partial class TemplateModel
    {
        public static class Perms
        {
            public const string Full = "perm.template.full-access";
            public const string Save = "perm.template.save";
            public const string Delete = "perm.template.delete";
        }

        public static class Claims
        {
            public const string ID = "claim.template.id";
        }
    }
}