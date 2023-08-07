namespace Image.Delete;

internal sealed class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("/image/{ID}");
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        await Logic.Image.DeleteAsync(r.ID);
        await SendOkAsync();
    }
}