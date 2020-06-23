using SCVault.Base;

namespace Main.Account.Login
{
    public class Response : LoginResponse
    {
        public string OwnerName { get; set; }
        public string VPName { get; set; }
        public string VPSubdomain { get; set; }
        public bool VPCreationDone { get; set; }
    }
}
