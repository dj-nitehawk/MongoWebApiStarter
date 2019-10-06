using System;

namespace App.Api.Auth
{
    public class JwtToken
    {
        public string Value { get; set; }
        public DateTime Expiry { get; set; }
    }
}
