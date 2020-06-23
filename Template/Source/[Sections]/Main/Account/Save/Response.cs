using SCVault;

namespace Main.Account.Save
{
    public class Response : IResponse
    {
        public string ID { get; set; }
        public bool EmailSent { get; set; }
    }
}