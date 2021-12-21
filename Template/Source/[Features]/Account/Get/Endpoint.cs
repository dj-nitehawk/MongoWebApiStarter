using MongoWebApiStarter.Auth;

namespace Account.Get;

public class Endpoint : Endpoint<Request, Response, Dom.Account>
{
    public override void Configure()
    {
        Get("/account/get");
        Permissions(Allow.Account_Read);
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var acc = await Data.GetAccountAsync(r.AccountID);

        if (acc is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendAsync(MapFromEntity(acc), 200, ct);
    }

    public override Response MapFromEntity(Dom.Account a) => new()
    {
        AccountID = a.ID,
        City = a.Address.City,
        CountryCode = a.Address.CountryCode,
        EmailAddress = a.Email,
        FirstName = a.FirstName,
        IsEmailVerified = a.IsEmailVerified,
        LastName = a.LastName,
        Mobile = a.Mobile,
        Password = "",
        State = a.Address.State,
        Street = a.Address.Street,
        Title = a.Title,
        ZipCode = a.Address.ZipCode
    };
}

