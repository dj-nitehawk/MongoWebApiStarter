using MongoWebApiStarter.Auth;

namespace Account.Login;

public class Request
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class Response
{
    public string FullName { get; set; }
    public JwtToken Token { get; set; } = new JwtToken();
    public IEnumerable<string> PermissionSet { get; set; }
}