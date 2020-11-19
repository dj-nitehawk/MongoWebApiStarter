using MongoDB.Entities;
using System.IO;
using System.Threading.Tasks;

namespace Image.Get
{
    public static class Data
    {
        public static Task DownloadAsync(string id, Stream destination)
        {
            return DB.File<Dom.Image>(id)
                     .DownloadWithTimeoutAsync(destination, 30);
        }
    }
}
