using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class LoginModel
    {
        public string AccountID = null;
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
    }
}
