using System;

namespace App.Biz.Settings
{
    public class AppSettings
    {
        public string AppName { get; set; }
        public Version AppVersion { get; set; }
        public Database Database { get; set; }
        public Email Email { get; set; }
        public Auth Auth { get; set; }
        public SMS SMS { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }

    public class Database
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Email
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public int BatchSize { get; set; }
    }

    public class Auth
    {
        public int TokenValidityMinutes { get; set; }
        public string SigningKey { get; set; }
    }

    public class SMS
    {
    }

    public class PaymentGateway
    {
    }
}
