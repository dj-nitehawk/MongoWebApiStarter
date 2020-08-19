using MongoDB.Entities;
using MongoWebApiStarter;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dom
{
    [Database(Constants.FileBucketDB)]
    public class Image : FileEntity, ICreatedOn
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
        public bool IsLinked { get; set; }
        public DateTime CreatedOn { get; set; }

        public static void Delete(string imageID)
        {
            DB.Delete<Image>(imageID);
        }

        public static void Link(params string[] imageIDs)
        {
            if (imageIDs.Length > 0)
            {
                DB.Update<Image>()
                  .Match(i => imageIDs.Contains(i.ID))
                  .Modify(i => i.IsLinked, true)
                  .Execute();
            }
        }

        public static async Task<long> DeleteUnlinked()
        {
            var anHourAgo = DateTime.UtcNow.AddHours(-1);
            return (await
                DB.DeleteAsync<Image>(i =>
                    i.CreatedOn <= anHourAgo &&
                    !i.IsLinked))
                .DeletedCount;
        }
    }
}
