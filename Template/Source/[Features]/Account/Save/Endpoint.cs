using Dom;
using MlkPwgen;
using MongoWebApiStarter;

namespace Account.Save;

public class Endpoint : Endpoint<Request, Response, Dom.Account>
{
    private bool needsEmailVerification;

    public override void Configure()
    {
        Verbs(Http.POST, Http.PUT);
        Routes("/account/save");
        AllowAnonymous(Http.POST);
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        await CheckIfEmailValidationIsNeededAsync(r);

        var acc = MapToEntity(r);

        await Data.CreateOrUpdateAsync(acc);

        await SendVerificationEmailAsync(acc);

        Response.EmailSent = needsEmailVerification;
        Response.ID = acc.ID;

        await SendAsync(Response, cancellation: ct);
    }

    public override Dom.Account MapToEntity(Request r) => new()
    {
        ID = r.AccountID,
        Email = r.EmailAddress.LowerCase(),
        PasswordHash = r.Password.SaltedHash(),
        Title = r.Title,
        FirstName = r.FirstName.TitleCase(),
        LastName = r.LastName.TitleCase(),
        Address = new Address
        {
            Street = r.Street.TitleCase(),
            City = r.City,
            State = r.State,
            ZipCode = r.ZipCode,
            CountryCode = r.CountryCode
        },
        Mobile = r.Mobile,
    };

    private async Task SendVerificationEmailAsync(Dom.Account a)
    {
        if (needsEmailVerification)
        {
            var code = PasswordGenerator.Generate(20);
            await Data.SetEmailValidationCodeAsync(code, a.ID);

            await new Notification
            {
                Type = NotificationType.Account_Welcome,
                ToEmail = a.Email,
                ToName = $"{a.FirstName} {a.LastName}",
                SendEmail = true,
                ToMobile = a.Mobile,
                SendSMS = a.Mobile.HasValue()
            }
            .Merge("{Salutation}", $"{a.FirstName}")
            .Merge("{ValidationLink}", $"{BaseURL}#/account/{a.ID}-{code}/validate")
            .AddToSendingQueueAsync();
        }
    }

    private async Task CheckIfEmailValidationIsNeededAsync(Request r)
    {
        if (r.AccountID.HasNoValue())
            needsEmailVerification = true;

        else if (r.AccountID != await Data.GetAccountIDAsync(r.EmailAddress))
            needsEmailVerification = true;

        else
            needsEmailVerification = false;
    }
}

