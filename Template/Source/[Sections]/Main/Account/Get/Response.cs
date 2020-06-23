using SCVault;

namespace Main.Account.Get
{
    public class Response : Model, IResponse
    {
        public bool IsEmailVerified { get; set; }
    }
}
