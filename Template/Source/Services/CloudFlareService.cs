using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace MongoWebApiStarter.Services;

public class CloudFlareService
{
    private const string base_url = "https://api.cloudflare.com/client/v4";
    private readonly Settings.CloudFlareSettings? settings;
    private readonly ILogger? log;

    public CloudFlareService(IOptions<Settings> appSettings, ILogger<CloudFlareService> logger)
    {
        settings = appSettings.Value.CloudFlare;
        log = logger;
        FlurlHttp.Configure(x => x.Timeout = TimeSpan.FromSeconds(10));
    }

    public async Task<bool> PurgeCacheAsync()
    {
        try
        {
            var response = await base_url
                .AppendPathSegment("zones")
                .AppendPathSegment(settings?.ZoneID)
                .AppendPathSegment("purge_cache")
                .WithOAuthBearerToken(settings?.Token)
                .PostJsonAsync(new { purge_everything = true })
                .ReceiveJson<CfPurgeCacheResponse>();

            if (response.success) return true;

            var msg = $"COULD NOT CLEAR CF CACHE: {string.Join(" | ", response.errors)}";
            log?.LogError(msg + Environment.NewLine);
            return false;
        }
        catch (FlurlHttpException x)
        {
            var msg = $"COULD NOT CLEAR CF CACHE: {x.Message}";
            log?.LogError(x, msg + Environment.NewLine);
            return false;
        }
    }

    private class CfPurgeCacheResponse
    {
        public bool success { get; set; }
        public string[] errors { get; set; } = Array.Empty<string>();
        public string[] messages { get; set; } = Array.Empty<string>();
    }
}
