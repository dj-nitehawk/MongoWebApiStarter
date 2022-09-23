using Microsoft.Extensions.Options;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;

namespace Account.Login;

public class Endpoint : Endpoint<Request, Response>
{
    public IOptions<Settings> Settings { get; set; }

    public override void Configure()
    {
        Post("/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var acc = await Data.GetAccountAsync(r.UserName);

        if (acc is not null)
        {
            if (!BCrypt.Net.BCrypt.Verify(r.Password, acc.PasswordHash))
                ThrowError("The supplied credentials are invalid. Please try again...");
        }
        else
        {
            ThrowError("Sorry, couldn't locate your account...");
        }

        if (acc?.IsEmailVerified is false)
            ThrowError("Please verify your email address before logging in...");

        var expiryDate = DateTime.UtcNow.AddDays(1);

        Response.FullName = $"{acc!.Title} {acc.FirstName} {acc.LastName}";
        Response.Token.Expiry = expiryDate.ToLocal().ToString("yyyy-MM-ddTHH:mm:ss");
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Settings.Value.Auth.SigningKey,
            expireAt: expiryDate,
            permissions: new Allow().Select(x => x.PermissionCode),
            claims: (Claim.AccountID, acc.ID));
        Response.PermissionSet = new Allow().Select(x => x.PermissionName);

        await SendAsync(Response, cancellation: ct);
    }
}