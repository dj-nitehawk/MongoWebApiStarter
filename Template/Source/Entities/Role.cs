using MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dom
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public bool SystemRole { get; set; }
        public IEnumerable<ushort> Permissions { get; set; }
        public One<Account> Account { get; set; }

        public static T[] GetPermissionsForRoles<T>(string[] roleIDs) where T : Enum
        {
            return DB.Queryable<Role>()
                     .Where(r => roleIDs.Contains(r.ID))
                     .SelectMany(r => r.Permissions)
                     .Distinct()
                     .ToArray()
                     .Cast<T>()
                     .ToArray();
        }

        public static string[] GetRoleNames(string[] roleIDs)
        {
            return DB.Find<Role, string>()
                     .Match(r => roleIDs.Contains(r.ID))
                     .Project(r => r.Name)
                     .Execute()
                     .ToArray();
        }

        public static bool RolesContainsPermission(string[] roleIDs, ushort permission)
        {
            return DB.Queryable<Role>()
                     .Where(r => roleIDs.Contains(r.ID))
                     .SelectMany(r => r.Permissions)
                     .Where(p => p == permission)
                     .Any();
        }

        static Role()
        {
            DB.Index<Role>()
              .Key(r => r.Account.ID, KeyType.Ascending)
              .CreateAsync();

            DB.Index<Role>()
              .Key(r => r.SystemRole, KeyType.Ascending)
              .CreateAsync();
        }
    }
}
