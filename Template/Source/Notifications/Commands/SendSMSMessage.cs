namespace MongoWebApiStarter.Notifications;

internal sealed class SendSMSMessage : ICommand
{
    public string Mobile { get; set; }
    public string Body { get; set; }
}

internal sealed class SendSMSMessageHandler : ICommandHandler<SendSMSMessage>
{
    public Task ExecuteAsync(SendSMSMessage cmd, CancellationToken c)
    {
        // TODO: implement sms gateway client
        return Task.CompletedTask;
    }
}