using Amazon.SimpleEmailV2;
using Microsoft.Extensions.Options;

namespace MongoWebApiStarter;

internal sealed class SendEmailMessage : ICommand
{
    public string ToName { get; set; }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

internal sealed class SendEmailMessageHandler : ICommandHandler<SendEmailMessage>
{
    private readonly AmazonSimpleEmailServiceV2Client _ses;
    private readonly Settings.EmailSettings _cfg;

    public SendEmailMessageHandler(AmazonSimpleEmailServiceV2Client sesClient, IOptions<Settings> cfg)
    {
        _ses = sesClient;
        _cfg = cfg.Value.Email;
    }

    public Task ExecuteAsync(SendEmailMessage cmd, CancellationToken c)
    {
        return _ses.SendEmailAsync(new()
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
            },
        }, c);
    }
}