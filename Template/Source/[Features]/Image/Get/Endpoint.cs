namespace Image.Get;

internal sealed class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Get("/image/{ID}.jpg"); //jpg extension is used so files can be cached by CDNs and browsers
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
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