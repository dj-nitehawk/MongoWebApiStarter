namespace Account.Verify;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/account/validate");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        if (await Data.ValidateEmailAsync(r.ID, r.Code))
            await SendOkAsync();
        else
            ThrowError("Sorry! Could not validate your email address...");
    }
}

