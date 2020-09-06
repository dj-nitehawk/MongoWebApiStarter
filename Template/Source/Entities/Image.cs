using MongoDB.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dom
{
    public class Image : FileEntity, ICreatedOn
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
        public bool IsLinked { get; set; }
        public DateTime CreatedOn { get; set; }

        public static Task DeleteAsync(string imageID)
        {
            return DB.DeleteAsync<Image>(imageID);
        }

        public static Task LinkAsync(params string[] imageIDs)
        {
            if (imageIDs.Length > 0)
            {
                return DB.Update<Image>()
                         .Match(i => imageIDs.Contains(i.ID))
                         .Modify(i => i.IsLinked, true)
                         .ExecuteAsync();
            }

            return Task.CompletedTask;
        }

        public static async Task<long> DeleteUnlinkedAsync()
        {
            var anHourAgo = DateTime.UtcNow.AddHours(-1);
            return (await DB.DeleteAsync<Image>(i =>
                    i.CreatedOn <= anHourAgo &&
                    !i.IsLinked))
                    .DeletedCount;
        }
    }
}
