using System.Collections.Generic;
using System.Linq;

namespace MongoWebApiStarter.Biz.Auth
{
    public static class Roles
    {
        public const string Admin = "role.admin";
        public const string Owner = "role.owner";
        public const string Employee = "role.employee";

        static Roles()
        {
            AllRoles = new Dictionary<string, string>();
            foreach (var f in typeof(Roles).GetFields())
            {
                AllRoles.Add(f.Name, f.GetValue(null) as string);
            }
        }

        public static Dictionary<string, string> AllRoles { get; private set; }

        public static string[] GetRoleNames(string[] roleValues)
        {
            return AllRoles.Where(r => roleValues.Contains(r.Value))
                           .Select(r => r.Key)
                           .ToArray();
        }

        public static string GetRoleName(string roleValue)
        {
            return GetRoleNames(new[] { roleValue }).Single();
        }

        public static string[] GetRoleValues(string[] roleNames)
        {
            return AllRoles.Where(r => roleNames.Contains(r.Key))
                           .Select(r => r.Value)
                           .ToArray();
        }

        public static string GetRoleValue(string roleName)
        {
            return GetRoleValues(new[] { roleName }).Single(); ;
        }
    }
}
