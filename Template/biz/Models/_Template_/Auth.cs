namespace MongoWebApiStarter.Biz.Models
{
    public partial class TemplateModel
    {
        public static class Perms
        {
            public const string Read = "perm.template.read";
            public const string Write = "perm.template.write";
            public const string Delete = "perm.template.delete";
        }

        public static class Claims
        {
            public const string ID = "claim.template.id";
        }
    }
}