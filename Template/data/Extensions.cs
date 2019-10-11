using MongoDB.Entities;

namespace MongoWebApiStarter.Data
{
    public static class Extensions
    {
        /// <summary>
        /// The entity does have an ID value
        /// </summary>
        public static bool IsNotNew(this Entity entity)
        {
            return !string.IsNullOrEmpty(entity.ID);
        }

        /// <summary>
        /// The entity doesn't have an ID value
        /// </summary>
        public static bool IsNew(this Entity entity)
        {
            return string.IsNullOrEmpty(entity.ID);
        }
    }
}
