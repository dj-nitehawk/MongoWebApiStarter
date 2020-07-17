using MongoDB.Entities;
using MongoWebApiStarter;
using System.IO;
using System.Threading.Tasks;

namespace Main.Image.Save
{
    public class Database : IDatabase
    {
        public Task DeleteImage(string id)
        {
            return DB.DeleteAsync<Dom.Image>(id);
        }

        public async Task<string> UploadAsync(Dom.Image image, Stream stream)
        {
            image.Save();
            await image.Data.UploadWithTimeoutAsync(stream, 60, 128);
            return image.ID;
        }
    }
}
