using SCVault;
using ServiceStack;

namespace Main.Image.Delete
{
    [Route("/image/{ID}")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
    }
}
