using ServiceStack;
using System;
using System.Linq;

namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// Use this attribute to allow access only to users who has at least one of the given permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionAttribute : RequiresAnyPermissionAttribute
    {
        /// <summary>
        /// Only allow access to users with at least one of the given permissions
        /// </summary>
        /// <param name="permissions">The list of permissions allowed</param>
        public PermissionAttribute(params Allow[] permissions) : base(permissions.Select(p => p.ToString("D")).ToArray())
        { }
    }
}
