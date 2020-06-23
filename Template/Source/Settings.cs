using System;

namespace SCVault
{
    public class Settings
    {
        public string AppName { get; set; } = "DEFAULT";
        public Version AppVersion { get; set; } = new Version(0, 0, 0);
        public Database Database { get; set; } = new Database();
        public Email Email { get; set; } = new Email();
        public JWT Auth { get; set; } = new JWT();
        public CloudFlare CloudFlare { get; set; } = new CloudFlare();
        public SMS SMS { get; set; } = new SMS();
        public PaymentGateway PaymentGateway { get; set; } = new PaymentGateway();
    }

    public class Database
    {
        public string Host { get; set; } = "DEFAULT";
        public int Port { get; set; }
        public string Name { get; set; } = "DEFAULT";
        public string Username { get; set; } = "DEFAULT";
        public string Password { get; set; } = "DEFAULT";
    }

    public class Email
    {
        public string Server { get; set; } = "DEFAULT";
        public int Port { get; set; }
        public string Username { get; set; } = "DEFAULT";
        public string Password { get; set; } = "DEFAULT";
        public string FromName { get; set; } = "DEFAULT";
        public string FromEmail { get; set; } = "DEFAULT";
        public int BatchSize { get; set; } = 10;
    }

    public class JWT
    {
        public int TokenValidityMinutes { get; set; }
        public string SigningKey { get; set; } = "ZGVmYXVsdC1zaWduaW5nLWtleQ=="; //has to be a base64 string for tests
    }

    public class CloudFlare
    {
        public string Token { get; set; } = "DEFAULT";
        public string ZoneID { get; set; } = "DEFAULT";
    }

    public class SMS
    {
    }

    public class PaymentGateway
    {
    }
}
