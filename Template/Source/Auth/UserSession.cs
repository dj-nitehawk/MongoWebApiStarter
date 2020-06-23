using ServiceStack;
using System.Collections.Generic;

namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// A custom user session class for the application
    /// </summary>
    public class UserSession : AuthUserSession
    {
        /// <summary>
        /// Instantiate a user session with the given claims
        /// </summary>
        /// <param name="claims">A tuple of claimTypes & claimValues</param>
        public UserSession(params (string claimType, string ClaimValue)[] claims)
        {
            foreach (var (claimType, ClaimValue) in claims)
            {
                Claims.Add(claimType, ClaimValue);
            }
        }

        /// <summary>
        /// The claims for the user
        /// </summary>
        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns the claim value for a given claim type if the user has the claim. if not found, returns default value.
        /// </summary>
        /// <param name="claimType"></param>
        public string ClaimValue(string claimType)
        {
            return Claims.GetValueOrDefault(claimType);
        }
    }
}
