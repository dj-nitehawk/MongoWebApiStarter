using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using MongoWebApiStarter.Biz.Settings;
using MongoWebApiStarter.Data.Entities;
using MongoWebApiStarter.Data.Repos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Biz.Services
{
    public class EmailService : BackgroundService
    {
        private readonly Email settings;
        private readonly bool isProduction;
        private bool startMsgLogged;
        private readonly EmailRepo repo = new EmailRepo();
        private readonly ILogger log;

        public EmailService(AppSettings settings, IHostEnvironment environment, ILogger<EmailService> log)
        {
            this.settings = settings.Email;
            isProduction = environment.IsProduction();
            this.log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var smtp = new SmtpClient())
            {
                var lastActiveAt = DateTime.UtcNow;
                var msgs = new List<EmailMessage>();

                while (isProduction && !stoppingToken.IsCancellationRequested)
                {
                    if (!startMsgLogged)
                    {
                        startMsgLogged = true;
                        log.LogWarning("EMAIL SERVICE HAS STARTED..." + "[" + settings.Username + "]" + Environment.NewLine);
                    }

                    msgs = repo.FetchNextBatch(settings.BatchSize);

                    if (msgs.Count > 0)
                    {
                        if (!smtp.IsConnected)
                        {
                            try
                            {
                                await smtp.ConnectAsync(settings.Server, settings.Port, true, stoppingToken);
                                await smtp.AuthenticateAsync(settings.Username, settings.Password, stoppingToken);
                                lastActiveAt = DateTime.UtcNow;
                            }
                            catch (Exception x)
                            {
                                log.LogError(x, "COULD NOT CONNECT TO SMTP SERVER SUCCESSFULLY!!!" + "[" + settings.Username + "]" + Environment.NewLine);
                                await Task.Delay((int)TimeSpan.FromMinutes(10).TotalMilliseconds);
                            }
                        }

                        if (smtp.IsConnected && smtp.IsAuthenticated)
                        {
                            foreach (var m in msgs)
                            {
                                try
                                {
                                    await smtp.SendAsync(ComposeEmail(m), stoppingToken);
                                    lastActiveAt = DateTime.UtcNow;
                                    repo.MarkAsSent(m.ID);
                                }
                                catch (Exception x)
                                {
                                    log.LogError(x, "COULD NOT SEND EMAIL VIA SMTP SERVER!!!" + Environment.NewLine);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (smtp.IsConnected && DateTime.UtcNow.Subtract(lastActiveAt).TotalSeconds >= 30)
                        {
                            await smtp.DisconnectAsync(true, stoppingToken);
                        }
                        await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
                    }
                }
            }
        }

        private MimeMessage ComposeEmail(EmailMessage msg)
        {
            var m = new MimeMessage();

            m.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));
            m.To.Add(new MailboxAddress(msg.ToName, msg.ToEmail));
            m.Subject = msg.Subject;
            m.Body = new TextPart(TextFormat.Html)
            {
                Text = msg.BodyHTML
            };
            return m;
        }
    }
}
