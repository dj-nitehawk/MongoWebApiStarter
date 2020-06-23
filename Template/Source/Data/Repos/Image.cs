using MongoDB.Entities;
using System.IO;
using System.Threading.Tasks;

namespace Data
{
    public class RepoImage : RepoBase<Image>
    {
        public static async Task<string> UploadAsync(Image image, Stream stream)
        {
            image.Save();

            await image.Data.UploadWithTimeoutAsync(stream, 60, 128);

            return image.ID;
        }

        public static Task DownloadAsync(string id, Stream destination)
        {
            DB.Update<Image>()
              .Match(i => i.ID == id)
              .Modify(b => b.CurrentDate(i => i.AccessedOn))
              .ExecuteAsync();

            return DB.File<Image>(id)
                     .DownloadWithTimeoutAsync(destination, 30);

            //todo: write a background service to purge images that haven't been hit in the past 6 months
        }
    }
}
