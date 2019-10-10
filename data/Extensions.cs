using MongoDB.Entities;

namespace MongoWebApiStarter.Data
{
    public static class Extensions
    {
        public static bool IsNotNew(this Entity entity)
        {
            return !string.IsNullOrEmpty(entity.ID);
        }

        public static bool IsNew(this Entity entity)
        {
            return string.IsNullOrEmpty(entity.ID);
        }
    }
}
