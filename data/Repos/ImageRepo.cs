using App.Data.Base;
using App.Data.Entities;
using MongoDB.Bson;
using MongoDB.Entities;
using System;
using System.Threading.Tasks;

namespace App.Data.Repos
{
    public class ImageRepo : RepoBase<Image>
    {
        new public async Task<Image> Find(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId tmp)) return null;

            _ = DB.Update<Image>()
                  .Match(i => i.ID == id)
                  .Modify(i => i.AccessedOn, DateTime.UtcNow)
                  .ExecuteAsync();

            return await DB.Find<Image>().OneAsync(id);
        }
    }
}
