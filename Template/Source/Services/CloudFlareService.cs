using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace MongoWebApiStarter.Services;

sealed class CloudFlareService
{
    const string BaseURL = "https://api.cloudflare.com/client/v4";
    readonly Settings.CloudFlareSettings? _settings;
    readonly ILogger? _log;

    public CloudFlareService(IOptions<Settings> appSettings, ILogger<CloudFlareService> logger)
    {
        _settings = appSettings.Value.CloudFlare;
        _log = logger;
        FlurlHttp.Configure(x => x.Timeout = TimeSpan.FromSeconds(10));
    }

    public async Task<bool> PurgeCacheAsync()
    {
        try
        {
            var response = await BaseURL
                                 .AppendPathSegment("zones")
                                 .AppendPathSegment(_settings?.ZoneID)
                                 .AppendPathSegment("purge_cache")
                                 .WithOAuthBearerToken(_settings?.Token)
                                 .PostJsonAsync(new { purge_everything = true })
                                 .ReceiveJson<CfPurgeCacheResponse>();

            if (response.Success)
                return true;

            var msg = $"COULD NOT CLEAR CF CACHE: {string.Join(" | ", response.Errors)}";
            _log?.LogError("{msg}", msg + Environment.NewLine);

            return false;
        }
        catch (FlurlHttpException x)
        {
            var msg = $"COULD NOT CLEAR CF CACHE: {x.Message}";
            _log?.LogError(x, "{msg}", msg + Environment.NewLine);

            return false;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    class CfPurgeCacheResponse
    {
        public bool Success { get; set; }
        public string[] Errors { get; } = Array.Empty<string>();
        public string[] Messages { get; set; } = Array.Empty<string>();
    }
}