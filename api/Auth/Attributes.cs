using Microsoft.AspNetCore.Authorization;
using System;

namespace MongoWebApiStarter.Api.Auth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AllowedRolesAttribute : AuthorizeAttribute
    {
        public AllowedRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class NeedPermissionAttribute : AuthorizeAttribute
    {
        public NeedPermissionAttribute(string policyName = null)
        {
            Policy = policyName;
        }
    }
}
