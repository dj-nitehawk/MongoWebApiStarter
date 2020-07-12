using MongoDB.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class RepoImage : RepoBase<Image>
    {
        //todo: write tests for uploading and downloading images

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
        }

        //todo: call this method when account is being saved to link the uploaded image
        public static void LinkImages(params string[] imageIDs)
        {
            if (imageIDs.Any())
                DB.Update<Image>()
                  .Match(i => imageIDs.Contains(i.ID))
                  .Modify(i => i.IsLinked, true)
                  .Execute();

            //todo: write a background service to purge images that haven't been linked [IsLinked=false] after 24hrs of creation
        }

        public static void DeleteUnlinkedImages()
        {
            var aDayAgo = DateTime.UtcNow.AddDays(-1);
            _ = DB.DeleteAsync<Image>(i => i.ModifiedOn <= aDayAgo);
        }
    }
}
