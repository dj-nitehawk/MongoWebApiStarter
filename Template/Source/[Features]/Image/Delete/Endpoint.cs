namespace Image.Delete;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/image/{ID}");
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        await Logic.Image.DeleteAsync(r.ID);
        await SendOkAsync();
    }
}

