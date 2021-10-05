using Flurl;
using Flurl.Http;

namespace MongoWebApiStarter.Services
{
    public class CloudFlareService
    {
        private static readonly string base_url = "https://api.cloudflare.com/client/v4";
        private static Settings settings;
        private static ILogger log;

        public CloudFlareService(Settings appSettings, ILogger<CloudFlareService> logger)
        {
            settings = appSettings;
            log = logger;
            FlurlHttp.Configure(x => x.Timeout = TimeSpan.FromSeconds(10));
        }

        public static async Task<bool> PurgeCacheAsync()
        {
            try
            {
                var response = await base_url
                    .AppendPathSegment("zones")
                    .AppendPathSegment(settings.CloudFlare.ZoneID)
                    .AppendPathSegment("purge_cache")
                    .WithOAuthBearerToken(settings.CloudFlare.Token)
                    .PostJsonAsync(new { purge_everything = true })
                    .ReceiveJson<CfPurgeCacheResponse>();

                if (response.success) return true;

                var msg = $"COULD NOT CLEAR CF CACHE: {string.Join(" | ", response.errors)}";
                log.LogError(msg + Environment.NewLine);
                return false;
            }
            catch (FlurlHttpException x)
            {
                var msg = $"COULD NOT CLEAR CF CACHE: {x.Message}";
                log.LogError(x, msg + Environment.NewLine);
                return false;
            }
        }

        private class CfPurgeCacheResponse
        {
            public bool success { get; set; }
            public string[] errors { get; set; }
            public string[] messages { get; set; }
        }
    }
}