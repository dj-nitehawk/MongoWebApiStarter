using MongoWebApiStarter.Data.Repos;
using System.Collections.Generic;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class EmailModel
    {
        private readonly string fromName;
        private readonly string fromEmail;
        private readonly string toName;
        private readonly string toEmail;
        private readonly string subject;
        private readonly string template;

        public Dictionary<string, string> MergeFields { get; set; } = new Dictionary<string, string>();

        public EmailModel(string fromName, string fromEmail, string toName, string toEmail, string subject, EmailTemplates template)
        {
            this.fromName = fromName;
            this.fromEmail = fromEmail;
            this.toName = toName;
            this.toEmail = toEmail;
            this.subject = subject;
            this.template = Repo.GetTemplate(template);
        }
    }
}
