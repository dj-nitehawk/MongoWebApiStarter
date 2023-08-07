#pragma warning disable CS8618

namespace MongoWebApiStarter;

internal sealed class Settings
{
    public string AppName { get; set; }
    public DatabaseSettings Database { get; set; }
    public JobQueueDatabaseSettings JobDatabase { get; set; }
    public FileBucketSettings FileBucket { get; set; }
    public EmailSettings Email { get; set; }
    public JWTAuthSettings Auth { get; set; }
    public CloudFlareSettings CloudFlare { get; set; }
    public SMSSettings SMS { get; set; }
    public PaymentGatewaySettings PaymentGateway { get; set; }

    public class DatabaseSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class JobQueueDatabaseSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class FileBucketSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
    }

    public class JWTAuthSettings
    {
        public int TokenValidityMinutes { get; set; }
        public string SigningKey { get; set; }
    }

    public class CloudFlareSettings
    {
        public string Token { get; set; }
        public string ZoneID { get; set; }
    }

    public class SMSSettings
    {
    }

    public class PaymentGatewaySettings
    {
    }
}
