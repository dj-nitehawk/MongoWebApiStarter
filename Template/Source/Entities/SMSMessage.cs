using MongoDB.Entities;

namespace Dom
{
    public class SMSMessage : Entity
    {
        public string Mobile { get; set; }
        public string Body { get; set; }

        [Ignore] public bool IsSent { get; set; }
    }
}
