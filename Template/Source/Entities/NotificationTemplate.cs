using MongoDB.Bson.Serialization.Attributes;

namespace Dom;

internal sealed class NotificationTemplate : IEntity
{
    [BsonId] public string? ID { get; set; } //set the template name as id
    public string SMSBody { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }

    public string GenerateNewID()
    {
        throw new NotImplementedException();
    }
}