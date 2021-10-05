namespace MongoWebApiStarter
{
    public class Settings
    {
        public string AppName { get; set; } = "DEFAULT";
        public Version AppVersion { get; set; } = new Version(0, 0, 0);
        public DatabaseSettings Database { get; set; } = new DatabaseSettings();
        public FileBucketSettings FileBucket { get; set; } = new FileBucketSettings();
        public EmailSettings Email { get; set; } = new EmailSettings();
        public JWTAuthSettings Auth { get; set; } = new JWTAuthSettings();
        public CloudFlareSettings CloudFlare { get; set; } = new CloudFlareSettings();
        public SMSSettings SMS { get; set; } = new SMSSettings();
        public PaymentGatewaySettings PaymentGateway { get; set; } = new PaymentGatewaySettings();

        public class DatabaseSettings
        {
            public string Host { get; set; } = "DEFAULT";
            public int Port { get; set; }
            public string Name { get; set; } = "DEFAULT";
            public string Username { get; set; } = "DEFAULT";
            public string Password { get; set; } = "DEFAULT";
        }

        public class FileBucketSettings
        {
            public string Host { get; set; } = "DEFAULT";
            public int Port { get; set; }
            public string Name { get; set; } = "DEFAULT";
            public string Username { get; set; } = "DEFAULT";
            public string Password { get; set; } = "DEFAULT";
        }

        public class EmailSettings
        {
            public string Server { get; set; } = "DEFAULT";
            public int Port { get; set; }
            public string Username { get; set; } = "DEFAULT";
            public string Password { get; set; } = "DEFAULT";
            public string FromName { get; set; } = "DEFAULT";
            public string FromEmail { get; set; } = "DEFAULT";
            public int BatchSize { get; set; } = 10;
        }

        public class JWTAuthSettings
        {
            public int TokenValidityMinutes { get; set; } = 10; //default expiry for tests
            public string SigningKey { get; set; } = "xxxxxxxxxxxxxxxx";
        }

        public class CloudFlareSettings
        {
            public string Token { get; set; } = "DEFAULT";
            public string ZoneID { get; set; } = "DEFAULT";
        }

        public class SMSSettings
        {
        }

        public class PaymentGatewaySettings
        {
        }
    }
}