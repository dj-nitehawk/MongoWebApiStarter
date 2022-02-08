using MongoWebApiStarter.Services;

namespace Utility.ClearCloudFlareCache;

public class Endpoint : EndpointWithoutRequest
{
    public CloudFlareService CloudFlare { get; set; }

    public override void Configure()
    {
        Get("/purge-cf-cache");//todo: protect this route with nginx or disable in production
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (await CloudFlare.PurgeCacheAsync())
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
