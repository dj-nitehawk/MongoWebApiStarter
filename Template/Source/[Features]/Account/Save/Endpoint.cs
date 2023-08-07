using MlkPwgen;
using MongoWebApiStarter;

namespace Account.Save;

internal sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private bool needsEmailVerification;
    private readonly bool isTestEnv;

    public Endpoint(IWebHostEnvironment env)
    {
        isTestEnv = env.ApplicationName == "testhost";
    }

    public override void Configure()
    {
        Verbs(Http.POST, Http.PUT);
        AllowAnonymous(Http.POST);
        Routes("/account/save");
    }

    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        await CheckIfEmailValidationIsNeededAsync(r);

        var acc = Map.ToEntity(r);

        await Data.CreateOrUpdateAsync(acc);

        await SendVerificationEmailAsync(acc);

        Response.EmailSent = needsEmailVerification;
        Response.ID = acc.ID!;
    }

    private async Task SendVerificationEmailAsync(Dom.Account a)
    {
        if (needsEmailVerification && !isTestEnv)
        {
            var code = PasswordGenerator.Generate(20);
            await Data.SetEmailValidationCodeAsync(code, a.ID!);

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
        => needsEmailVerification = r.AccountID.HasNoValue() || r.AccountID != await Data.GetAccountIDAsync(r.EmailAddress);
}