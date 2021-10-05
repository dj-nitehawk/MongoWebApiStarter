using FastEndpoints;
using MlkPwgen;
using MongoWebApiStarter;

namespace Account.Save
{
    public class Endpoint : Endpoint<Request, Response>
    {
        private bool needsEmailVerification;

        public Endpoint()
        {
            Verbs(Http.POST, Http.PUT);
            Routes("/account");
            AllowAnnonymous();
        }

        protected override async Task HandleAsync(Request r, CancellationToken ct)
        {
            if (HttpMethod is Http.PUT && User?.Identity?.IsAuthenticated is false)
            {
                await SendUnauthorizedAsync();
                return;
            }

            await CheckIfEmailValidationIsNeededAsync(r);

            var acc = r.ToEntity();

            await Data.CreateOrUpdateAsync(acc);

            await SendVerificationEmailAsync(acc);

            Response.EmailSent = needsEmailVerification;
            Response.ID = acc.ID;

            await SendAsync(Response, cancellation: ct);
        }

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
            if (r.ID.HasNoValue())
                needsEmailVerification = true;

            else if (r.ID != await Data.GetAccountIDAsync(r.EmailAddress))
                needsEmailVerification = true;

            else
                needsEmailVerification = false;
        }
    }
}
