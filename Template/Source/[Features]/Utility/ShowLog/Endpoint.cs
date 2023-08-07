namespace Utility.ShowLog;

internal sealed class Endpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/show-log"); //todo: protect this route with nginx
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (File.Exists("output.log"))
        {
            var fileInfo = new FileInfo(Path.Combine(Env.ContentRootPath, "output.log"));

            await SendStreamAsync(
                stream: fileInfo.OpenRead(),
                fileName: null,
                fileLengthBytes: null,
                contentType: "text/plain");
        }
        else
        {
            await SendNotFoundAsync();
        }
    }
}