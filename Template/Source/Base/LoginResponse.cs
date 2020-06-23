using SCVault.Auth;
using System.Linq;

namespace SCVault.Base
{
    /// <summary>
    /// Abstract class for login responses
    /// </summary>
    public abstract class LoginResponse : IResponse
    {
        /// <summary>
        /// The token
        /// </summary>
        public JwtToken Token { get; set; }

        /// <summary>
        /// The list of permissions for the user
        /// </summary>
        public string[] PermissionSet { get; set; }

        /// <summary>
        /// Generates the JWT token for the give user with a set of permissions and roles
        /// </summary>
        /// <param name="userSession">The user session to create the token for</param>
        /// <param name="permissions">The permissions to assign for the user</param>
        /// <param name="roles">The roles to assign the user</param>
        public void SignIn(UserSession userSession, Allow[] permissions, string[] roles = default)
        {
            Token = Authentication.GenerateToken(userSession, permissions, roles);
            PermissionSet = permissions
                .Select(p => p.ToString())
                .OrderBy(s => s)
                .ToArray();
        }
    }
}
