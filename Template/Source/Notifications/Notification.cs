using Dom;
using System.Text;
using System.Text.RegularExpressions;

namespace MongoWebApiStarter;

public partial record Notification
{
    private static readonly Dictionary<string, NotificationTemplate> _templates = new();

    public static async Task Initialize()
    {
        foreach (var t in await DB.Find<NotificationTemplate>().Match(_ => true).ExecuteAsync())
            _templates.Add(t.ID!, t);
    }

    public string ToName { get; init; } = null!;
    public string ToEmail { get; init; } = null!;
    public string ToMobile { get; init; } = null!;
    public bool SendEmail { get; init; }
    public bool SendSMS { get; init; }
    public string Type { get; init; } = null!;

    private readonly HashSet<(string Name, string Value)> _mergeFields = new();
    private readonly List<string> _missingTags = new();

    public Notification Merge(string fieldName, string fieldValue)
    {
        _mergeFields.Add((fieldName, fieldValue));
        return this;
    }

    public async Task AddToSendingQueueAsync()
    {
        if (ToName.HasNoValue() ||
           (SendEmail && ToEmail.HasNoValue()) ||
           (SendSMS && ToMobile.HasNoValue()) ||
            Type.HasNoValue())
        {
            throw new ArgumentNullException("Unable to send notification without all required parameters!");
        }

        _templates.TryGetValue(Type, out var template);

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

        if (_missingTags.Count > 0)
            throw new ApplicationException($"Replacements are missing for: [{string.Join(",", _missingTags.Distinct())}]");

        if (SendEmail)
        {
            await new SendEmailMessage
            {
                ToEmail = ToEmail,
                ToName = ToName,
                Subject = emailSubject!,
                Body = emailBody!
            }.QueueJobAsync();
        }

        if (SendSMS)
        {
            await new SendSMSMessage
            {
                Mobile = ToMobile,
                Body = smsBody!
            }.QueueJobAsync();
        }
    }

    private string MergeFields(string input, string fieldName)
    {
        if (input.HasNoValue())
            throw new ApplicationException($"The template [{Type}] has no {fieldName} value!");

        var sb = new StringBuilder(input);

        foreach (var (Name, Value) in _mergeFields)
            sb.Replace(Name, Value);

        var body = sb.ToString();

        _missingTags.AddRange(Rx().Matches(body).Select(m => m.Value).Distinct());

        return body;
    }

    [GeneratedRegex("{.*}")]
    private static partial Regex Rx();
}