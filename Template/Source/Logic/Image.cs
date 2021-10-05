using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using MongoWebApiStarter;

namespace Logic
{
    public static class Image
    {
        public static Task DeleteAsync(string imageID)
        {
            return DB.DeleteAsync<Dom.Image>(i => i.ID == imageID);
        }

        public static Task LinkAsync(string[] imageIDs)
        {
            var validIDs = imageIDs.Where(i => i.HasValue());

            if (validIDs.Any())
            {
                return DB.Update<Dom.Image>()
                         .Match(i => validIDs.Contains(i.ID))
                         .Modify(i => i.IsLinked, true)
                         .ExecuteAsync();
            }

            return Task.CompletedTask;
        }

        public static async Task<long> DeleteUnlinkedAsync()
        {
            var anHourAgo = DateTime.UtcNow.AddHours(-1);
            return (await DB.DeleteAsync<Dom.Image>(i =>
                    i.CreatedOn <= anHourAgo &&
                    !i.IsLinked))
                    .DeletedCount;
        }
    }
}
