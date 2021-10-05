using FastEndpoints;
using MongoWebApiStarter.Auth;

namespace Account.Get
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public Endpoint()
        {
            Verbs(Http.GET);
            Routes("/account/{ID}");
            Permissions(Allow.Account_Read);
        }

        protected override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var acc = await Data.GetAccountAsync(r.AccountID);

            if (acc is null) await SendNotFoundAsync();

            Response.FromEntity(acc);

            await SendAsync(Response, cancellation: ct);
        }
    }
}
