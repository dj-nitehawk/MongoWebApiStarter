using SCVault;
using ServiceStack;

namespace Main.Account.Get
{
    [Route("/account/{ID}")]
    public class Request : IRequest<Response>
    {
        public string ID { get; set; }
    }
}
