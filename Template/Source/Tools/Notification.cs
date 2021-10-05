using Dom;
using MongoDB.Entities;
using System.Text;
using System.Text.RegularExpressions;

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

#pragma warning disable CS8618
        public string ToName { get; init; }
        public string ToEmail { get; init; }
        public string ToMobile { get; init; }
        public bool SendEmail { get; init; }
        public bool SendSMS { get; init; }
        public string Type { get; init; }
#pragma warning restore CS8618

        private readonly HashSet<(string Name, string Value)> mergeFields = new HashSet<(string Name, string Value)>();
        private readonly List<string> missingTags = new List<string>();

        public Notification Merge(string fieldName, string fieldValue)
        {
            mergeFields.Add((fieldName, fieldValue));
            return this;
        }

        public Task AddToSendingQueueAsync()
        {
            if (ToName.HasNoValue() ||
                (SendEmail && ToEmail.HasNoValue()) ||
                (SendSMS && ToMobile.HasNoValue()) ||
                Type.HasNoValue())
            {
                throw new ArgumentNullException("Unable to send notification without all required parameters!");
            }

            templates.TryGetValue(Type, out var template);

            if (template == null)
                throw new ApplicationException($"Unable to find a message template for [{Type}]");

            string? emailBody = null, emailSubject = null, smsBody = null;

            if (SendEmail)
            {
                emailBody = MergeFields(template.EmailBody, nameof(NotificationTemplate.EmailBody));
                emailSubject = MergeFields(template.EmailSubject, nameof(NotificationTemplate.EmailSubject));
            }

            if (SendSMS)
                smsBody = MergeFields(template.SMSBody, nameof(NotificationTemplate.SMSBody));

            if (missingTags.Count > 0)
                throw new ApplicationException($"Replacements are missing for: [{string.Join(",", missingTags.Distinct())}]");

            Task emlTask = Task.CompletedTask, smsTask = Task.CompletedTask;

            if (SendEmail)
            {
                emlTask = new EmailMessage
                {
                    ToEmail = ToEmail,
                    ToName = ToName,
                    Subject = emailSubject ?? "",
                    Body = emailBody ?? ""
                }.SaveAsync();
            }

            if (SendSMS)
            {
                smsTask = new SMSMessage
                {
                    Mobile = ToMobile,
                    Body = smsBody ?? ""
                }.SaveAsync();
                //todo: create sms sending background service
            }

            return Task.WhenAll(emlTask, smsTask);
        }

        private string MergeFields(string input, string fieldName)
        {
            if (input.HasNoValue())
                throw new ApplicationException($"The template [{Type}] has no {fieldName} value!");

            var sb = new StringBuilder(input);
            foreach (var (Name, Value) in mergeFields)
                sb.Replace(Name, Value);
            var body = sb.ToString();
            missingTags.AddRange(rx.Matches(body).Select(m => m.Value).Distinct());
            return body;
        }
    }
}
