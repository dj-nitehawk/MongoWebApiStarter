using Microsoft.AspNetCore.Http;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel
    {
        public string ID { get; set; }
        public IFormFile File { get; set; }
        public byte[] FileBytes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
