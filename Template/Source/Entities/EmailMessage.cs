using MongoDB.Entities;

namespace Dom
{
    public class EmailMessage : Entity
    {
        [Ignore] public bool IsSent { get; set; }

        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string BodyHTML { get; set; }

        static EmailMessage()
        {
            DB.Index<EmailMessage>()
              .Key(e => e.IsSent, KeyType.Ascending)
              .CreateAsync();
        }
    }
}
