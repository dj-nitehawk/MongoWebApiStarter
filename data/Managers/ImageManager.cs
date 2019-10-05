using MongoDB.Bson;
using MongoDB.Entities;
using App.Data.Entities;
using System;
using System.Threading.Tasks;

namespace App.Data.Managers
{
    public class ImageManager
    {
        public async Task<string> SaveAsync(Image image)
        {
            await image.SaveAsync();
            return image.ID;
        }

        public async Task<Image> Find(string id)
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
