using FastEndpoints;

namespace Image.Delete
{
    public class Endpoint : Endpoint<Request>
    {
        public Endpoint()
        {
            Verbs(Http.DELETE);
            Routes("/image/{ID}");
        }

        protected override async Task HandleAsync(Request r, CancellationToken ct)
        {
            await Logic.Image.DeleteAsync(r.ID);
            await SendOkAsync();
        }
    }
}
