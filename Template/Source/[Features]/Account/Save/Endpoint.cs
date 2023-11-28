using MlkPwgen;
using MongoWebApiStarter;
using MongoWebApiStarter.Notifications;

namespace Account.Save;

sealed class Endpoint(IWebHostEnvironment env) : Endpoint<Request, Response, Mapper>
{
    bool _needsEmailVerification;
    readonly bool _isTestEnv = env.EnvironmentName == "Testing";

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

        Response.EmailSent = _needsEmailVerification;
        Response.ID = acc.ID!;
    }

    async Task SendVerificationEmailAsync(Dom.Account a)
    {
        if (_needsEmailVerification && !_isTestEnv)
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

    async Task CheckIfEmailValidationIsNeededAsync(Request r)
        => _needsEmailVerification = r.AccountID.HasNoValue() || r.AccountID != await Data.GetAccountIDAsync(r.EmailAddress);
}