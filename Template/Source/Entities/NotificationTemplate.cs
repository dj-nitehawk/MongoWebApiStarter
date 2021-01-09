using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Dom
{
    public class NotificationTemplate : IEntity
    {
        [BsonId] public string ID { get; set; } //set the template name as id
        public string SMSBody { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }

        public void SetNewID()
            => throw new System.NotImplementedException();
    }
}
