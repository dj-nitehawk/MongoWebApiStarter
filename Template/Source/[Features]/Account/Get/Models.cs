using MongoWebApiStarter.Auth;

namespace Account.Get;

internal sealed class Request
{
    [From(Claim.AccountID)]
    public string AccountID { get; set; }
}

internal sealed class Response : Model
{
    public string AccountID { get; set; }
    public bool IsEmailVerified { get; set; }
}