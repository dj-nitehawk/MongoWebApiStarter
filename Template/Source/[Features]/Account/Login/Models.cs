using MongoWebApiStarter.Auth;

namespace Account.Login;

sealed class Request
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

sealed class Response
{
    public string FullName { get; set; }
    public JwtToken Token { get; set; } = new();
    public IEnumerable<string> PermissionSet { get; set; }
}