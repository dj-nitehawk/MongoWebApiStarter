using SCVault;
using ServiceStack;

namespace Main.Account.Verify
{
    [Route("/api/account/{ID}/{Code}/validate")]
    public class Request : IRequest<Nothing>
    {
        public string ID { get; set; }
        public string Code { get; set; }
    }
}
