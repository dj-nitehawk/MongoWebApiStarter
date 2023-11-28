using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable InconsistentNaming

namespace Dom;

sealed class NotificationTemplate : IEntity
{
    [BsonId]
    public string ID { get; set; } //set the template name as id

    public string SMSBody { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }

    public object GenerateNewID()
        => ID; //because we're setting the ID manually

    public bool HasDefaultID()
        => !string.IsNullOrEmpty(ID);
}