using MongoWebApiStarter.Auth;

namespace Account.Get;

public class Endpoint : Endpoint<Request, Response, Mapper>
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
            await SendNotFoundAsync();
        else
            await SendAsync(Map.FromEntity(acc));
    }
}