using SCVault;
using ServiceStack;

namespace Main.Image.Delete
{
    [Route("/api/image/{ID}")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
    }
}
