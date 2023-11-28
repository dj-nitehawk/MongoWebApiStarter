using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoWebApiStarter;

namespace Logic;

static class Image
{
    public static Task DeleteAsync(string imageID)
    {
        return DB.DeleteAsync<Dom.Image>(i => i.ID == imageID);
    }

    public static Task LinkAsync(IEnumerable<string> imageIDs)
    {
        var validIDs = imageIDs.Where(i => i.HasValue());

        return validIDs.Any()
                   ? DB.Update<Dom.Image>()
                       .Match(i => validIDs.Contains(i.ID))
                       .Modify(i => i.IsLinked, true)
                       .ExecuteAsync()
                   : Task.CompletedTask;
    }

    public static async Task<long> DeleteUnlinkedAsync()
    {
        var anHourAgo = DateTime.UtcNow.AddHours(-1);

        return (await DB.DeleteAsync<Dom.Image>(i => i.CreatedOn <= anHourAgo && !i.IsLinked)).DeletedCount;
    }
}