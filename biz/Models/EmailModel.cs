using App.Biz.Base;
using App.Data.Entities;
using App.Data.Managers;
using System;
using System.Collections.Generic;

namespace App.Biz.Models
{
    public class EmailModel : ModelBase<EmailManager>
    {
        private readonly string fromName;
        private readonly string fromEmail;
        private readonly string toName;
        private readonly string toEmail;
        private readonly string subject;
        private string template;

        public Dictionary<string, string> MergeFields { get; set; } = new Dictionary<string, string>();

        public EmailModel(string fromName, string fromEmail, string toName, string toEmail, string subject, EmailTemplates template)
        {
            this.fromName = fromName;
            this.fromEmail = fromEmail;
            this.toName = toName;
            this.toEmail = toEmail;
            this.subject = subject;
            this.template = Manager.GetTemplate(template);
        }

        public void AddToSendingQueue()
        {
            if (MergeFields.Count == 0) throw new InvalidOperationException("Cannot proceed without any MergeFields!");

            foreach (var f in MergeFields)
            {
                template = template.Replace("{" + f.Key + "}", f.Value);
            }

            Manager.Save(new EmailMessage
            {
                FromName = fromName,
                FromEmail = fromEmail,
                ToEmail = toEmail,
                ToName = toName,
                Subject = subject,
                BodyHTML = template
            });
        }
    }
}
