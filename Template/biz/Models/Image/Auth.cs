namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel
    {
        public static class Perms
        {
            public const string Read = "perm.image.read";
            public const string Write = "perm.image.write";
            public const string Delete = "perm.image.delete";
        }
    }
}
