using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;
using System;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class EmailModel : IMapper<EmailModel, EmailMessage>
    {
        public void LoadFrom(EmailMessage entity)
        {
            throw new NotImplementedException();
        }

        public EmailMessage ToEntity(EmailModel m)
        {
            return new EmailMessage
            {
                FromName = m.fromName,
                FromEmail = m.fromEmail,
                ToEmail = m.toEmail,
                ToName = m.toName,
                Subject = m.subject
            };
        }
    }
}
