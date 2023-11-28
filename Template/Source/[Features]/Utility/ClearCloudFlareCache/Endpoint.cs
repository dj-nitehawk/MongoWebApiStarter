﻿using MongoWebApiStarter.Services;

namespace Utility.ClearCloudFlareCache;

sealed class Endpoint : EndpointWithoutRequest
{
    public CloudFlareService CloudFlare { get; set; } = null!;

    public override void Configure()
    {
        Get("/purge-cf-cache"); //todo: protect this route with nginx or disable in production
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (await CloudFlare.PurgeCacheAsync())
            await SendAsync("SUCCESS!!!");
        else
            ThrowError("FAILED TO CLEAR CLOUDFLARE CACHE. CHECK APP LOG.");
    }
}