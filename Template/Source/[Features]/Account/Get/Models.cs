using MongoWebApiStarter.Auth;

namespace Account.Get;

sealed class Request
{
    [From(Claim.AccountID)]
    public string AccountID { get; set; }
}

sealed class Response : Model
{
    public string AccountID { get; set; }
    public bool IsEmailVerified { get; set; }
}