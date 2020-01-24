using System.Collections.Generic;
using System.Linq;

namespace MongoWebApiStarter.Biz.Auth
{
    public static class Roles
    {
        // set your apps roles here
        public const string Admin = "role.admin";
        public const string Owner = "role.owner";
        public const string Employee = "role.employee";
        // set your apps roles here

        public static Dictionary<string, string> AllRoles { get; private set; }

        static Roles()
        {
            AllRoles = new Dictionary<string, string>();
            foreach (var f in typeof(Roles).GetFields())
            {
                AllRoles.Add(f.Name, f.GetValue(null) as string);
            }
        }        

        /// <summary>
        /// Get an array of role names given an array of role values
        /// </summary>
        /// <param name="roleValues">The role values</param>
        public static string[] GetRoleNames(string[] roleValues)
        {
            return AllRoles.Where(r => roleValues.Contains(r.Value))
                           .Select(r => r.Key)
                           .ToArray();
        }


        /// <summary>
        /// Get the role name given a role value
        /// </summary>
        /// <param name="roleValue">The role value</param>
        public static string GetRoleName(string roleValue)
        {
            return GetRoleNames(new[] { roleValue }).Single();
        }

        /// <summary>
        /// Get role values given an array of role names
        /// </summary>
        /// <param name="roleNames">The role names</param>
        public static string[] GetRoleValues(string[] roleNames)
        {
            return AllRoles.Where(r => roleNames.Contains(r.Key))
                           .Select(r => r.Value)
                           .ToArray();
        }

        /// <summary>
        /// Get the role value given a role name
        /// </summary>
        /// <param name="roleName">The role name</param>
        public static string GetRoleValue(string roleName)
        {
            return GetRoleValues(new[] { roleName }).Single(); ;
        }
    }
}
