using FastEndpoints;

namespace Utility.ShowLog
{
    public class Endpoint : EndpointWithoutRequest
    {
        public Endpoint()
        {
            Verbs(Http.GET);
            Routes("/show-log");
            AllowAnnonymous();
        }

        protected override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
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
}
