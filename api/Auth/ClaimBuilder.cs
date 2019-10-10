using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MongoWebApiStarter.Api.Auth
{
    public class ClaimBuilder
    {
        HashSet<Claim> claims = new HashSet<Claim>();

        public ClaimBuilder WithClaim(string claimType, string claimValue)
        {
            claims.Add(new Claim(claimType, claimValue));
            return this;
        }

        public Claim[] GetClaims()
        {
            return claims.ToArray();
        }
    }
}
