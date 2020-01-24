using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;
using System;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class EmailModel : IMapper<EmailMessage>
    {
        public void LoadFrom(EmailMessage entity)
        {
            throw new NotImplementedException();
        }

        public EmailMessage ToEntity()
        {
            return new EmailMessage
            {
                FromName = fromName,
                FromEmail = fromEmail,
                ToEmail = toEmail,
                ToName = toName,
                Subject = subject,
                BodyHTML = template
            };
        }
    }
}
