using MongoDB.Bson;
using MongoDB.Entities;
using MongoWebApiStarter.Data.Base;
using MongoWebApiStarter.Data.Entities;
using System;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Data.Repos
{
    public class ImageRepo : RepoBase<Image>
    {
        public async Task<Image> GetAsync(string id)
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
