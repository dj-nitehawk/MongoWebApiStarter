using Amazon.SimpleEmailV2;
using Microsoft.Extensions.Options;

namespace MongoWebApiStarter.Notifications;

sealed class SendEmailMessage : ICommand
{
    public string ToName { get; set; }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

sealed class SendEmailMessageHandler(IAmazonSimpleEmailServiceV2 sesClient, IOptions<Settings> cfg)
    : ICommandHandler<SendEmailMessage>
{
    readonly Settings.EmailSettings _cfg = cfg.Value.Email;

    public Task ExecuteAsync(SendEmailMessage cmd, CancellationToken c)
        => sesClient.SendEmailAsync(
            request: new()
            {
                FromEmailAddress = $"{_cfg.FromName}<{_cfg.FromEmail}>",
                Destination = new()
                {
                    ToAddresses = new() { $"{cmd.ToName}<{cmd.ToEmail}>" }
                },
                Content = new()
                {
                    Simple = new()
                    {
                        Subject = new() { Data = cmd.Subject },
                        Body = new() { Html = new() { Data = cmd.Body } }
                    }
                }
            },
            cancellationToken: c);
}