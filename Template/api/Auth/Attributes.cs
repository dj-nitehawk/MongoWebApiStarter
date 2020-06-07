using Microsoft.AspNetCore.Authorization;
using System;

namespace MongoWebApiStarter.Api.Auth
{
    /// <summary>
    /// Restrict access based on Role claim type
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Only allow certain Roles to access an action
        /// </summary>
        /// <param name="roles">The roles to allow</param>
        public RolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }

    /// <summary>
    /// Restrict access based on application policies/ permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Only allow users matching a given security policy/ permission
        /// </summary>
        /// <param name="policyName">The policy to allow access with</param>
        public PermissionAttribute(string policyName = null)
        {
            Policy = policyName;
        }
    }
}
