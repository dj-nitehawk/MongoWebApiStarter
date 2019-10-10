using System;

namespace MongoWebApiStarter.Biz.Settings
{
    public class AppSettings
    {
        public string AppName { get; set; } = "app name";
        public Version AppVersion { get; set; } = new Version(0, 0, 0);
        public Database Database { get; set; } = new Database();
        public Email Email { get; set; } = new Email();
        public Auth Auth { get; set; } = new Auth();
        public SMS SMS { get; set; } = new SMS();
        public PaymentGateway PaymentGateway { get; set; } = new PaymentGateway();
    }

    public class Database
    {
        public string Host { get; set; } = "host";
        public int Port { get; set; } = 0;
        public string Name { get; set; } = "database";
        public string Username { get; set; } = "username";
        public string Password { get; set; } = "password";
    }

    public class Email
    {
        public string Server { get; set; } = "server";
        public int Port { get; set; } = 0;
        public string Username { get; set; } = "username";
        public string Password { get; set; } = "password";
        public string FromName { get; set; } = "from name";
        public string FromEmail { get; set; } = "from@address.com";
        public int BatchSize { get; set; } = 10;
    }

    public class Auth
    {
        public int TokenValidityMinutes { get; set; } = 60;
        public string SigningKey { get; set; } = "ZGVmYXVsdC1zaWduaW5nLWtleQ==";
    }

    public class SMS
    {
    }

    public class PaymentGateway
    {
    }
}
