using MongoWebApiStarter.Services;

namespace Utility.ClearCloudFlareCache;

public class Endpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/purge-cf-cache");//todo: protect this route with nginx or disable in production
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest r, CancellationToken ct)
    {
        if (await CloudFlareService.PurgeCacheAsync())
        {
            await SendAsync("SUCCESS!!!");
            return;
        }
        else
        {
            ThrowError("FAILED TO CLEAR CLOUDFLARE CACHE. CHECK APP LOG.");
        }
    }
}
