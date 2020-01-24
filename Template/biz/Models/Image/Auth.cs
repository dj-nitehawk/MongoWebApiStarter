namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel
    {
        public static class Perms
        {
            public const string Full = "perm.image.full";
            public const string Save = "perm.image.save";
            public const string Delete = "perm.image.delete";
        }
    }
}
