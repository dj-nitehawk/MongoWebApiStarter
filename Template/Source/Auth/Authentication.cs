using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoWebApiStarter.Auth
{
    public static class Authentication
    {
        private static Settings Settings;
        public static JwtAuthProvider JWTProvider;
        public static AuthFeature AuthFeature;

        public static void Initialize(Settings settings)
        {
            Settings = settings;
            JWTProvider = new JwtAuthProvider()
            {
                AuthKeyBase64 = Settings.Auth.SigningKey,
                ExpireTokensIn = TimeSpan.FromMinutes(Settings.Auth.TokenValidityMinutes),
                CreatePayloadFilter = CreatePayload,
                PopulateSessionFilter = PopulateSession,
                RequireSecureConnection = false
            };
            AuthFeature = new AuthFeature(
                () => new UserSession(),
                new[] {
                    JWTProvider
                })
            { HtmlRedirect = null, IncludeAssignRoleServices = false };
        }

        private static readonly Action<IAuthSession, JsonObject, ServiceStack.Web.IRequest> PopulateSession = (s, p, _) =>
        {
            var session = (UserSession)s;

            foreach (var f in typeof(Claim).GetFields().Select(f => f.Name))
            {
                if (p[f] != null)
                    session.Claims.Add(f, p[f]);
            }
        };

        private static readonly Action<JsonObject, IAuthSession> CreatePayload = (p, s) =>
        {
            foreach (var c in ((UserSession)s).Claims)
            {
                p[c.Key] = c.Value;
            }
        };

        /// <summary>
        /// Generates a JWT token for the given user embedding the supplied permissions & roles
        /// </summary>
        /// <param name="userSession">The user to sign in</param>
        /// <param name="permissions">Permissions to assign to the user</param>
        /// <param name="roles">Roles to assign to the user</param>
        public static JwtToken GenerateToken(UserSession userSession, IEnumerable<Allow> permissions, string[] roles = default)
        {
            var token = JWTProvider.CreateJwtBearerToken(userSession, roles, permissions.Select(p => p.ToString("D")));

            return new JwtToken
            {
                Value = token,
                Expiry = DateTime.UtcNow.AddMinutes(Settings.Auth.TokenValidityMinutes).ToLocal().ToString("yyyy-MM-ddTHH:mm:ss")
            };
        }
    }
}
