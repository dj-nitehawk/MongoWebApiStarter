using SCVault;
using ServiceStack;

namespace Main.Image.Save
{
    [Route("/api/image")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
