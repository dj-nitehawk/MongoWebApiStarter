using Dom;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace MongoWebApiStarter.Services;

public class EmailService : BackgroundService
{
    private readonly Settings.EmailSettings settings;
    private readonly bool isProduction;
    private readonly bool isTesting;
    private bool startMsgLogged;
    private readonly ILogger log;

    public EmailService(IOptions<Settings> settings, IWebHostEnvironment environment, ILogger<EmailService> log)
    {
        this.settings = settings.Value.Email;
        isProduction = environment.IsProduction();
        //isTesting = environment.IsEnvironment("Testing");//not working
        isTesting = environment.ApplicationName == "testhost";
        this.log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        if (isTesting) return;

        using var smtp = new SmtpClient();
        var lastActiveAt = DateTime.UtcNow;
        var msgs = new List<EmailMessage>();

        while (isProduction && !cancellation.IsCancellationRequested)
        {
            if (!startMsgLogged)
            {
                startMsgLogged = true;
                log.LogWarning("EMAIL SERVICE HAS STARTED... [" + settings.Username + "]" + Environment.NewLine);
            }

            msgs = await Data.FetchNextBatchAsync(settings.BatchSize);

            if (msgs.Count > 0)
            {
                if (!smtp.IsConnected)
                {
                    try
                    {
                        await smtp.ConnectAsync(settings.Server, settings.Port, true, cancellation);
                        await smtp.AuthenticateAsync(settings.Username, settings.Password, cancellation);
                        lastActiveAt = DateTime.UtcNow;
                    }
                    catch (Exception x)
                    {
                        log.LogError(x, "COULD NOT CONNECT TO SMTP SERVER SUCCESSFULLY!!! [" + settings.Username + "]" + Environment.NewLine);
                        await Task.Delay((int)TimeSpan.FromMinutes(10).TotalMilliseconds, cancellation);
                    }
                }

                if (smtp.IsConnected && smtp.IsAuthenticated)
                {
                    foreach (var m in msgs)
                    {
                        try
                        {
                            await smtp.SendAsync(ComposeEmail(m), cancellation);
                            lastActiveAt = DateTime.UtcNow;
                            m.IsSent = true;
                        }
                        catch (Exception x)
                        {
                            log.LogError(x, "COULD NOT SEND EMAIL VIA SMTP SERVER!!!" + Environment.NewLine);
                            await Task.Delay((int)TimeSpan.FromMinutes(10).TotalMilliseconds, cancellation);
                            break;
                        }
                        finally
                        {
                            await Data.DeleteMailsAsync(
                                    msgs.Where(m => m.IsSent)
                                        .Select(m => m.ID));
                        }
                    }
                }
            }
            else
            {
                if (smtp.IsConnected && DateTime.UtcNow.Subtract(lastActiveAt).TotalSeconds >= 30)
                    await smtp.DisconnectAsync(true, cancellation);

                await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds, cancellation);
            }
        }
    }

    private MimeMessage ComposeEmail(EmailMessage msg)
    {
        var m = new MimeMessage();

        m.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));
        m.To.Add(new MailboxAddress(msg.ToName, msg.ToEmail));
        m.Subject = msg.Subject;
        m.Body = new TextPart(TextFormat.Html) { Text = msg.Body };
        return m;
    }

    public static class Data
    {
        public static Task<List<EmailMessage>> FetchNextBatchAsync(int batchSize)
        {
            return DB.Find<EmailMessage>()
                     .Limit(batchSize)
                     .ExecuteAsync();
        }

        public static Task DeleteMailsAsync(IEnumerable<string> mailIDs)
        {
            return DB.DeleteAsync<EmailMessage>(mailIDs);
        }
    }
}

