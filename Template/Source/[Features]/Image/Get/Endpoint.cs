using FastEndpoints;

namespace Image.Get
{
    public class Endpoint : Endpoint<Request>
    {
        public Endpoint()
        {
            Verbs(Http.GET);
            Routes("/image/{ID}.jpg"); //jpg extension is used so files can be cached by CDNs and browsers
            AllowAnnonymous();
        }

        protected override async Task HandleAsync(Request r, CancellationToken ct)
        {
            try
            {
                HttpContext.Response.ContentType = "image/jpeg";
                await Data.DownloadAsync(r.ID, HttpContext.Response.Body);
            }
            catch (Exception)
            {
                await SendNotFoundAsync();
            }
        }
    }
}
