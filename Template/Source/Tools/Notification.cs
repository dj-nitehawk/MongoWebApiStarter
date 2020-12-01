using Dom;
using MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MongoWebApiStarter
{
    public record Notification
    {
        private static readonly Dictionary<string, NotificationTemplate> templates = new Dictionary<string, NotificationTemplate>();
        private static readonly Regex rx = new Regex("{.*}", RegexOptions.Compiled);

        public static async Task Initialize()
        {
            foreach (var t in await DB.Find<NotificationTemplate>().Match(_ => true).ExecuteAsync())
                templates.Add(t.ID, t);
        }

        public string ReceiverName { get; init; }
        public string Email { get; init; }
        public string EmailSubject { get; init; }
        public string Mobile { get; init; }
        public bool SendEmail { get; init; }
        public bool SendSMS { get; init; }
        public string Type { get; init; }

        private readonly HashSet<(string Name, string Value)> mergeFields = new HashSet<(string Name, string Value)>();
        private readonly List<string> missingTags = new List<string>();

        public Notification Merge(string fieldName, string fieldValue)
        {
            mergeFields.Add((fieldName, fieldValue));
            return this;
        }

        public Task AddToSendingQueueAsync()
        {
            if (ReceiverName.HasNoValue() ||
                (SendEmail && (Email.HasNoValue() || EmailSubject.HasNoValue())) ||
                (SendSMS && Mobile.HasNoValue()) ||
                Type.HasNoValue())
            {
                throw new ArgumentNullException("Unable to send notification without all required parameters!");
            }

            templates.TryGetValue(Type, out var template);

            if (template == null)
                throw new ApplicationException($"Unable to find a message template for [{Type}]");

            string emailBody = null, smsBody = null;

            if (SendEmail)
                emailBody = MergeFields(template.EmailBody, "Email");

            if (SendSMS)
                smsBody = MergeFields(template.SMSBody, "SMS");

            if (missingTags.Count > 0)
                throw new ApplicationException($"Replacements are missing for: [{string.Join(",", missingTags.Distinct())}]");

            Task emlTask = Task.CompletedTask, smsTask = Task.CompletedTask;

            if (emailBody.HasValue())
            {
                emlTask = new EmailMessage
                {
                    ToEmail = Email,
                    ToName = ReceiverName,
                    Subject = EmailSubject,
                    Body = emailBody
                }.SaveAsync();
            }

            if (smsBody.HasValue())
            {
                smsTask = new SMSMessage
                {
                    Mobile = Mobile,
                    Body = smsBody
                }.SaveAsync();
                //todo: create sms sending background service
            }

            return Task.WhenAll(emlTask, smsTask);
        }

        private string MergeFields(string template, string callerName)
        {
            if (template.HasNoValue())
                throw new ApplicationException($"The template [{Type}] has no {callerName} message body!");

            var sb = new StringBuilder(template);
            foreach (var (Name, Value) in mergeFields)
                sb.Replace(Name, Value);
            var body = sb.ToString();
            missingTags.AddRange(rx.Matches(body).Select(m => m.Value).Distinct());
            return body;
        }
    }
}
