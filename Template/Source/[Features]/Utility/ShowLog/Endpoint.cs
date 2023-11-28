namespace Utility.ShowLog;

sealed class Endpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/show-log"); //todo: protect this route with nginx
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        if (!File.Exists("output.log"))
            return SendNotFoundAsync();

        var fileInfo = new FileInfo(Path.Combine(Env.ContentRootPath, "output.log"));

        return SendStreamAsync(
            stream: fileInfo.OpenRead(),
            fileName: null,
            fileLengthBytes: null,
            contentType: "text/plain");
    }
}