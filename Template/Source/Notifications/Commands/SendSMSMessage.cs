// ReSharper disable InconsistentNaming

namespace MongoWebApiStarter.Notifications;

sealed class SendSMSMessage : ICommand
{
    public string Mobile { get; set; }
    public string Body { get; set; }
}

sealed class SendSMSMessageHandler : ICommandHandler<SendSMSMessage>
{
    public Task ExecuteAsync(SendSMSMessage cmd, CancellationToken c)

        // TODO: implement sms gateway client
        => Task.CompletedTask;
}