using SCVault;
using ServiceStack;

namespace Main.Account.Verify
{
    [Route("/account/{ID}/{Code}/validate")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
        public string Code { get; set; }
    }
}
