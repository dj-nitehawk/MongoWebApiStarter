using MongoDB.Entities;

namespace App.Data
{
    public static class Extensions
    {
        public static bool IsNotNew(this Entity entity)
        {
            return !string.IsNullOrEmpty(entity.ID);
        }
    }
}
