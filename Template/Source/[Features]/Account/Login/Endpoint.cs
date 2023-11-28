using Microsoft.Extensions.Options;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;

namespace Account.Login;

sealed class Endpoint : Endpoint<Request, Response>
{
    public IOptions<Settings> Settings { get; set; } = null!;

    public override void Configure()
    {
        Post("/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var acc = await Data.GetAccountAsync(r.UserName);

        if (acc is null)
            ThrowError("Sorry, couldn't locate your account...");
        else if (!BCrypt.Net.BCrypt.Verify(r.Password, acc.PasswordHash))
            ThrowError("The supplied credentials are invalid. Please try again...");

        if (!acc.IsEmailVerified)
            ThrowError("Please verify your email address before logging in...");

        var expiryDate = DateTime.UtcNow.AddDays(1);
        var permissions = new Allow();

        Response.FullName = $"{acc!.Title} {acc.FirstName} {acc.LastName}";
        Response.Token.Expiry = expiryDate.ToLocal().ToString("yyyy-MM-ddTHH:mm:ss");
        Response.PermissionSet = permissions.AllNames();
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Settings.Value.Auth.SigningKey,
            expireAt: expiryDate,
            privileges: u =>
            {
                u.Permissions.AddRange(permissions.AllCodes());
                u[Claim.AccountID] = acc.ID!;
            });
    }
}