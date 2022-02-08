namespace Utility.ShowLog;

public class Endpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/show-log");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (File.Exists("output.log"))
        {
            var fileInfo = new FileInfo(Path.Combine(Env.ContentRootPath, "output.log"));

            await SendStreamAsync(
                stream: fileInfo.OpenRead(),
                contentType: "text/plain",
                fileName: null,
                fileLengthBytes: null);
        }
        else
        {
            await SendNotFoundAsync();
        }
    }
}