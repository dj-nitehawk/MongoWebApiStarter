using MongoDB.Entities.Core;
using System;

namespace Data
{
    public class EmailMessage : Entity
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string BodyHTML { get; set; }
        public bool Sent { get; set; } = false;
        public DateTime SentOn { get; set; }
    }
}
