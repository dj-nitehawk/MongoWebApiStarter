using MongoDB.Entities;
using MongoWebApiStarter.Data.Base;
using MongoWebApiStarter.Data.Entities;
using System.IO;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Data.Repos
{
    public class ImageRepo : RepoBase<Image>
    {
        public async Task<string> UploadAsync(Image image, Stream stream)
        {
            image.Save();

            await image.Data.UploadWithTimeoutAsync(stream, 60, 128);

            return image.ID;
        }

        public Task DownloadAsync(string id, Stream destination)
        {
            DB.Update<Image>()
              .Match(i => i.ID == id)
              .Modify(b => b.CurrentDate(i => i.AccessedOn))
              .ExecuteAsync();

            return DB.File<Image>(id)
                     .DownloadWithTimeoutAsync(destination, 30);
        }
    }
}
