#pragma warning disable CS8618
namespace MongoWebApiStarter.Auth;

public class JwtToken
{
    /// <summary>
    /// The JWT token string
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The expiry date of the JWT Token
    /// </summary>
    public string Expiry { get; set; }
}