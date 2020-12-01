using MongoDB.Entities;

namespace Dom
{
    public class EmailMessage : Entity
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        [Ignore] public bool IsSent { get; set; }
    }
}
